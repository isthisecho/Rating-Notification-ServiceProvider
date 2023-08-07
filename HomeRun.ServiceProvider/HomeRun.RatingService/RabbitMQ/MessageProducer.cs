using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace HomeRun.RatingService
{
    public class MessageProducer : IMessageProducer
    {
            public const string queueName = "ratings";
            private readonly ILogger<MessageProducer> _logger;
            private readonly IConnectionFactory _connectionFactory;

            public MessageProducer(ILogger<MessageProducer> logger, IConnectionFactory connectionFactory)
            {
                _logger             = logger;
                _connectionFactory  = connectionFactory;
            }

            public void SendingMessage<T>(T message)
            {
                try
                {

                    IConnection connection = _connectionFactory.CreateConnection();

                    using IModel channel = connection.CreateModel();

                    channel.QueueDeclare(queueName, durable: false, exclusive: false);

                    string jsonString = JsonSerializer.Serialize(message);
                    byte[] body = Encoding.UTF8.GetBytes(jsonString);

                    channel.BasicPublish(exchange: "", routingKey: queueName, body: body);

                    _logger.LogInformation("Message sent to the queue successfully: {@message}", message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while sending the message to the queue: {@message}", message);
                    throw;
                }
            }
        
    }
}
