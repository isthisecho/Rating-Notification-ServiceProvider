using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace HomeRun.RatingService
{
    public class MessageProducer : IMessageProducer
    {
        public const string queueName = "ratings";
        public void SendingMessage<T>(T message)
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

            channel.QueueDeclare(queueName, durable: false, exclusive: false);

            string jsonString = JsonSerializer.Serialize(message);
            byte[] body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish(exchange: "", routingKey: queueName, body: body);
        }
    }
}
