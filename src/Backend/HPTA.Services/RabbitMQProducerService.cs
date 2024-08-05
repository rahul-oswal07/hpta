using EmailClient.Contracts;
using HPTA.Common.Models;
using HPTA.DTO;
using HPTA.Services.Contracts;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

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
                foreach (var user in users)
                {
                    var notification = new EmailNotificationDTO
                    {
                        Name = user.Name,
                        Email = user.Email,
                        // Add other fields as needed
                    };
                    notifications.Add(notification);
                }

                // Create RabbitMQ connection and channel
                var factory = new ConnectionFactory { HostName = "localhost" };
                using (IConnection connection = factory.CreateConnection())
                using (IModel channel = connection.CreateModel())
                {
                    string exchangeName = "EmailExchange";
                    string routingKey = "email_queue";
                    string queueName = "EmailQueue";

                    // Declare the exchange and queue, and bind them
                    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
                    channel.QueueDeclare(queueName, true, false, false, null);
                    channel.QueueBind(queueName, exchangeName, routingKey, null);
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
                        channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, body: messageBodyBytes);
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
