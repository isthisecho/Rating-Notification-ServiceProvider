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
        private readonly IConnectionFactory _connectionFactory;
        private IConnection? _connection; // Keep Connection Global
        private IModel? _channel; // Keep Channel Global
        private const string queueName = "ratings";
        private EventingBasicConsumer? consumer;
        public NotificationProccessor(ILogger<NotificationProccessor> logger, INotificationService notificationService, IConnectionFactory connectionFactory)
        {
            _logger              = logger;
            _notificationService = notificationService;
            _connectionFactory   = connectionFactory;
        }

            private void InitializeRabbitMQ()
            {
                _connection = _connectionFactory.CreateConnection();
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
                    NotificationDTO? result = JsonConvert.DeserializeObject<NotificationDTO>(message);

                    if (result != null)
                        _notificationService.AddNotification(result);

                    _logger.LogInformation("Product message received: {@message}", message);
                };

                // Auto Acknowledge is true but we might change it to false and when endpoint called we can
                // acknowledge that we consumed message.

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
