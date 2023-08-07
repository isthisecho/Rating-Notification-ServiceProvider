using HomeRun.RatingService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Testcontainers.RabbitMq;

namespace HomeRun.NotificationService.Tests.Integration
{
    public class NotificationApiFactory : WebApplicationFactory<HomeRun.NotificationService.Program>, IAsyncLifetime
    {
        public readonly RabbitMqContainer _rabbitMqContainer;

        public NotificationApiFactory()
        {
            _rabbitMqContainer = new RabbitMqBuilder().Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            string rabbitConnectionString = _rabbitMqContainer.GetConnectionString();

            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {

                services.RemoveAll(typeof(ConnectionFactory));

                services.AddSingleton<RabbitMQ.Client.IConnectionFactory>(sp =>
                {

                    return new ConnectionFactory
                    {
                        Uri = new Uri(_rabbitMqContainer.GetConnectionString())
                    };
                });

            });

        }

        internal void SendMessage(NotificationDTO notification)
        {
            const string queueName = "ratings";


            string jsonString = JsonSerializer.Serialize(notification);
            byte[] body = Encoding.UTF8.GetBytes(jsonString);


            // Signal the completion of message reception.
            EventWaitHandle waitHandle = new ManualResetEvent(false);

            ConnectionFactory factory = new ConnectionFactory
            {
                Uri = new Uri(_rabbitMqContainer.GetConnectionString())
            };
            // Create and establish a connection.
            using var connection = factory.CreateConnection();

            // Send a message to the channel.
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queueName, durable: false, exclusive: false);
            channel.BasicPublish(exchange: "", routingKey: queueName, body: body);

        }



        public async Task InitializeAsync()
        {
            await _rabbitMqContainer.StartAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _rabbitMqContainer.StopAsync();
            await _rabbitMqContainer.DisposeAsync();
        }
    }
}
