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

          await _channel.ExchangeDeclareAsync(exchange: "EmailExchange", type: ExchangeType.Direct);
            await _channel.QueueDeclareAsync(queue: "EmailQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            await _channel.QueueBindAsync(queue: "EmailQueue", exchange: "EmailExchange", routingKey: "email_queue");

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
               var result =  await _emailService.SendAsync("HPTA Survey", emailData?.Body, new EmailRecipient[] { new EmailRecipient() { Email = emailData.Email } });

                if (result)
                {
                    // Process the message
                    _logger.LogInformation($"Received message: {message}");

                    // Acknowledge the message
                    _channel.BasicAckAsync(ea.DeliveryTag, false);
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
