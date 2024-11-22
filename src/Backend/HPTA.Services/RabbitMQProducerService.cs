using EmailClient.Contracts;
using HPTA.Common.Models;
using HPTA.DTO;
using HPTA.Services.Contracts;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace HPTA.Services
{
    public class RabbitMQProducerService : IRabbitMQProducerService
    {
        private readonly ILiquidTemplateService _liquidTemplateService;
        private readonly ITeamService _teamService;

        public RabbitMQProducerService(ILiquidTemplateService liquidTemplateService, ITeamService teamService)
        {
            _liquidTemplateService = liquidTemplateService;
            _teamService = teamService;
        }
        public async Task SendMessage()
        {
            try
            {

                var users = await _teamService.ListTeamMembers(173);
                var notifications = new List<EmailNotificationDTO>();

                // Loop through the users to build the notification body

                notifications = users
                    .Select(user => new EmailNotificationDTO
                    {
                        Name = user.Name,
                        Email = user.Email,
                        // Add other fields as needed
                    })
                    .ToList();
                // Create RabbitMQ connection and channel
                var factory = new ConnectionFactory { HostName = "localhost" };
                using (IConnection connection = await factory.CreateConnectionAsync())
                using (IChannel channel = await connection.CreateChannelAsync())
                {
                    string exchangeName = "EmailExchange";
                    string routingKey = "email_queue";
                    string queueName = "EmailQueue";


                    var arguments = new Dictionary<string, object?>
{
    { "x-dead-letter-exchange", "dlx_exchange" },
    { "x-dead-letter-routing-key", "email.failed" }
};


                    var basicProperties = new BasicProperties();
                    basicProperties.Persistent = true;

                    // Declare the exchange and queue, and bind them
                    await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct);
                    await channel.QueueDeclareAsync(queueName, true, false, false, arguments:arguments);
                    await channel.QueueBindAsync(queueName, exchangeName, routingKey, null);


                    await channel.ExchangeDeclareAsync(exchange: "dlx_exchange", type: ExchangeType.Direct);
                    await channel.QueueDeclareAsync(queue: "dlx_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    await channel.QueueBindAsync(queue: "dlx_queue", exchange: "dlx_exchange", routingKey: "email.failed");


                    notifications = new List<EmailNotificationDTO>() { new EmailNotificationDTO { Email="a.karappadi@devon.nl",Name="Athul"} };
                    // Loop through each notification, render the body, and publish it
                    foreach (var notification in notifications)
                    {
                        // Render the template with the current notification
                        var body = await _liquidTemplateService.RegisterType(typeof(EmailNotificationDTO))
                                                                .RenderAsync("Notification", notification);

                        notification.Body = body;
                        string emailJson = JsonConvert.SerializeObject(notification);
                        // Convert the rendered body to bytes
                        byte[] messageBodyBytes = Encoding.UTF8.GetBytes(emailJson);

                        // Publish the message to RabbitMQ
                        await channel.BasicPublishAsync(exchange: exchangeName,routingKey: routingKey,mandatory:true, basicProperties:basicProperties, body: messageBodyBytes);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
