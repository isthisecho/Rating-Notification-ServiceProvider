using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Channels;

namespace HomeRun.NotificationService
{
    public class NotificationProccessor : BackgroundService
    {
        private readonly ILogger<NotificationProccessor> _logger;
        private readonly INotificationService _notificationService;
        private IConnection? _connection; // Keep Connection Global
        private IModel? _channel; // Keep Channel Global
        private const string queueName = "ratings";
        private EventingBasicConsumer? consumer;
        public NotificationProccessor(ILogger<NotificationProccessor> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

            private void InitializeRabbitMQ()
            {
                ConnectionFactory factory = new ConnectionFactory()
                {
                    HostName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_HOST") ?? "localhost",  // if null use default values
                    UserName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER") ?? "user",       // if null use default values
                    Password = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS") ?? "pass",       // if null use default values
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(queueName, exclusive: false, durable: false);
            }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                InitializeRabbitMQ();

                consumer = new EventingBasicConsumer(_channel);

                consumer.Received += (model, eventArgs) =>
                { 
                    byte[] body          = eventArgs.Body.ToArray()                            ;
                    string message       = Encoding.UTF8.GetString(body)                       ;
                    Notification? result = JsonConvert.DeserializeObject<Notification>(message);

                    if (result != null)
                        _notificationService.AddNotification(result);

                    _logger.LogInformation("Product message received: {@message}", message);
                };

                _channel.BasicConsume(queueName, true, consumer);

                _logger.LogInformation("Notification Service is working.");

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing notifications from the queue.");
                throw;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
    }
}
