using EmailClient;
using EmailClient.Contracts;
using HPTA.DTO;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;   
        private IConnection _connection;
        private IChannel _channel;
        private readonly IEmailSender _emailService;

        public Worker(ILogger<Worker> logger ,IEmailSender emailService)
        {
            _logger = logger;
            _emailService = emailService;
             InitializeRabbitMQ().GetAwaiter().GetResult();
        }

        private async Task InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            //Map<String, Object> args = new HashMap<String, Object>();
            //args.put("x-dead-letter-exchange", "some.exchange.name");

            var arguments = new Dictionary<string, object?>
{
    { "x-dead-letter-exchange", "dlx_exchange" },
    { "x-dead-letter-routing-key", "email.failed" }
};

            await _channel.ExchangeDeclareAsync(exchange: "EmailExchange", type: ExchangeType.Direct);          
            await _channel.QueueDeclareAsync(queue: "EmailQueue", durable: true, exclusive: false, autoDelete: false, arguments:arguments);
            await _channel.QueueBindAsync(queue: "EmailQueue", exchange: "EmailExchange", routingKey: "email_queue");

            await _channel.ExchangeDeclareAsync(exchange: "dlx_exchange", type: ExchangeType.Direct);
            await _channel.QueueDeclareAsync(queue: "dlx_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            await _channel.QueueBindAsync(queue: "dlx_queue", exchange: "dlx_exchange", routingKey: "email.failed");

            _logger.LogInformation("RabbitMQ connection and channel initialized.");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var emailData = JsonConvert.DeserializeObject<EmailNotificationDTO>(message);
               var result =  await _emailService.SendAsync("HPTA Survey", emailData?.Body, new EmailRecipient[] { new EmailRecipient() { Email = "a.kardevon.nl" } });

                if (result)
                {
                    // Process the message
                    _logger.LogInformation($"Received message: {message}");

                    // Acknowledge the message
                   await  _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                else
                {
                    await _channel.BasicNackAsync(ea.DeliveryTag, false,false);

                }
            };

            _channel.BasicConsumeAsync(queue: "EmailQueue", autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.CloseAsync();
            _connection?.CloseAsync();
            base.Dispose();
        }
    }
}
