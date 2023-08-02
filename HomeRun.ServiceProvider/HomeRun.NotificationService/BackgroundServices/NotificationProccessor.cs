using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace HomeRun.NotificationService
{
    public class NotificationProccessor :BackgroundService
    {
        private readonly ILogger<NotificationProccessor> _logger;
        private readonly INotificationService _notificationService;
        private const string queueName = "ratings";

        public NotificationProccessor(ILogger<NotificationProccessor> logger , INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "pass",
                VirtualHost = "/"

            };

            IConnection connection = factory.CreateConnection();

            using IModel channel = connection.CreateModel();

            channel.QueueDeclare(queueName, exclusive: false, durable: false);

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Notification? result = JsonConvert.DeserializeObject<Notification>(message);

                if(result !=null)
                    _notificationService.AddNotification(result);

                Console.WriteLine($"Product message received: {message}");
            };

            channel.BasicConsume(queueName, false, consumer);

            _logger.LogInformation("Notification Service is working.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

    }
}
