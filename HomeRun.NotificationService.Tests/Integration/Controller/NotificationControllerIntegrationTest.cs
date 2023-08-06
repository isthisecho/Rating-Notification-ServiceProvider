using System.Net;
using System.Text;
using RabbitMQ.Client;
using HomeRun.Shared.Helpers;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using FluentAssertions;

namespace HomeRun.NotificationService.Tests.Integration
{

    public  class NotificationControllerIntegrationTest  : IClassFixture<NotificationApiFactory> //,IDisposable
    {
        private readonly NotificationApiFactory _factory;
        private readonly HttpClient _client;
        private readonly Notification _notification = new Notification() { Id = 1, RatingId = 1, RatingValue = 2, ServiceProviderId = 1 ,CreatedAt=DateTime.UtcNow};
        public NotificationControllerIntegrationTest(NotificationApiFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }


        [Fact]
        public async void ConsumeMessageFromQueue_WhenRatingApiPosts_ShouldReturnOk()
        {
            SendMessage();

            int serviceProivderId = 1;
            var response = await _client.GetAsync(HttpHelper.Urls.GetNewNotifications + serviceProivderId);

            var responseContent = await response.Content.ReadAsStringAsync();
            var notifications = JsonConvert.DeserializeObject<List<Notification>>(responseContent);


            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _notification.Should().BeEquivalentTo(notifications?.FirstOrDefault()); // Equality Check

        }

        [Fact]
        public async void ConsumeMessageFromQueue_WhenRatingApiPosts_ShouldReturnEmpty_BecauseNoNotificationForServiceProivderId_OrNoNotificationsAtAll()
        {
            int serviceProivderId = 2;
            var response = await _client.GetAsync(HttpHelper.Urls.GetNewNotifications + serviceProivderId);
        
            var responseContent = await response.Content.ReadAsStringAsync();
            var notifications = JsonConvert.DeserializeObject<List<Notification>>(responseContent);
        
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Null(notifications?.FirstOrDefault());
        
        
        }

        private void SendMessage()
        {
            const string queueName = "ratings";


            string jsonString = JsonSerializer.Serialize(_notification);
            byte[] body       = Encoding.UTF8.GetBytes(jsonString);


            // Signal the completion of message reception.
            EventWaitHandle waitHandle = new ManualResetEvent(false);

            // Create and establish a connection.
            var connectionFactory = new ConnectionFactory()
            {
                UserName = "user",
                Password = "pass"
            };

            using var connection = connectionFactory.CreateConnection();

            // Send a message to the channel.
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queueName, durable: false, exclusive: false);
            channel.BasicPublish(exchange: "", routingKey: queueName, body: body);

        }


    }

}
