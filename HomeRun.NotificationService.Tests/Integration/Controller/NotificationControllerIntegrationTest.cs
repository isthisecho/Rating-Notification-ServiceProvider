using System.Net;
using System.Text;
using RabbitMQ.Client;
using HomeRun.Shared.Helpers;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using FluentAssertions;
using Testcontainers.RabbitMq;

namespace HomeRun.NotificationService.Tests.Integration
{

    public  class NotificationControllerIntegrationTest  : IClassFixture<NotificationApiFactory> //,IDisposable
    {
        private readonly NotificationApiFactory _factory;
        private readonly HttpClient _client;
        private readonly NotificationDTO _notification = new NotificationDTO() { RatingId = 1, RatingValue = 2, ServiceProviderId = 1 ,CreatedAt=DateTime.UtcNow};
        public NotificationControllerIntegrationTest(NotificationApiFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }


        [Fact]
        public async void ConsumeMessageFromQueue_WhenRatingApiPosts_ShouldReturnOk()
        {
            _factory.SendMessage(_notification);

            int serviceProivderId = 1;
            var response = await _client.GetAsync(HttpHelper.Urls.GetNewNotifications + serviceProivderId);

            var responseContent = await response.Content.ReadAsStringAsync();
            var notifications = JsonConvert.DeserializeObject<List<NotificationDTO>>(responseContent);


            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            _notification.Should().BeEquivalentTo(notifications?.FirstOrDefault()); // Equality Check

        }

        [Fact]
        public async void ConsumeMessageFromQueue_WhenRatingApiPosts_ShouldReturnEmpty_BecauseNoNotificationForServiceProivderId_OrNoNotificationsAtAll()
        {
            int serviceProivderId = 2;
            var response = await _client.GetAsync(HttpHelper.Urls.GetNewNotifications + serviceProivderId);
        
            var responseContent = await response.Content.ReadAsStringAsync();
            var notifications = JsonConvert.DeserializeObject<List<NotificationDTO>>(responseContent);
        
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Null(notifications?.FirstOrDefault());
        
        
        }

    }

}
