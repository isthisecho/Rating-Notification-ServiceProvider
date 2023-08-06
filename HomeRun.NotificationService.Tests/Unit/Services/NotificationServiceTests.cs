using Xunit;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using HomeRun.NotificationService;
using System.Linq;

namespace HomeRun.NotificationService.Tests.UnitTests
{
    public class NotificationServiceTests
    {

        private readonly ILogger<NotificationService> _logger;

        public NotificationServiceTests()
        {
            _logger = A.Fake<ILogger<NotificationService>>();
        }

        [Fact]
        public void GetAllNewNotifications_ValidServiceProviderId_ReturnsMatchingNotificationsAndRemovesThem()
        {
            // Arrange
            var service = new NotificationService(_logger);

            var serviceProviderId = 2;

            Notification[] notifications = new[]
            {
                new Notification { Id = 1, RatingId =1,  ServiceProviderId= 1 ,RatingValue = 5 },
                new Notification { Id = 2, RatingId =2,  ServiceProviderId= 1 ,RatingValue = 3 },
                new Notification { Id = 3, RatingId =3,  ServiceProviderId= 2 ,RatingValue = 2 },
                new Notification { Id = 4, RatingId =4,  ServiceProviderId= 2 ,RatingValue = 4 }
            };

            foreach (var notification in notifications)
            {
                service.AddNotification(notification);
            }

            // Act
            var result = service.GetAllNewNotifications(serviceProviderId).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, n => Assert.Equal(serviceProviderId, n.ServiceProviderId));

            // Ensure the notifications are removed from the list
            var remainingNotifications = service.GetAllNewNotifications(serviceProviderId).ToList();
            Assert.Empty(remainingNotifications);
        }

        [Fact]
        public void GetAllNewNotifications_InvalidServiceProviderId_ReturnsEmptyList()
        {
            // Arrange
            var service = new NotificationService(_logger);

            var serviceProviderId = 2; 
            var notifications = new[]
            {
                new Notification { Id = 1, RatingId =1,  ServiceProviderId= 3 ,RatingValue = 5 },
            };

            foreach (var notification in notifications)
            {
                service.AddNotification(notification);
            }

            // Act
            var result = service.GetAllNewNotifications(serviceProviderId).ToList();

            // Assert
            Assert.Empty(result);
        }

    }
}