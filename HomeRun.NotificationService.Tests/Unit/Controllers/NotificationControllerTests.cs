using Xunit;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

namespace HomeRun.NotificationService.Tests.UnitTests.Controllers
{
    public class NotificationControllerTests
    {

        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationControllerTests()
        {
            _notificationService = A.Fake<INotificationService>();
            _logger = A.Fake<ILogger<NotificationController>>();
        }

        [Fact]
        public void GetNewNotifications_ValidId_ReturnsOkResultWithNotifications()
        {
            // Arrange
            var controller = new NotificationController(_logger, _notificationService);

            var serviceProviderId = 1;
            var fakeNotifications = new List<Notification>
            {
                new Notification { Id = 1, RatingId =1,  ServiceProviderId= 1 ,RatingValue = 5 },
                new Notification { Id = 2, RatingId =2,  ServiceProviderId= 1 ,RatingValue = 3 },
                new Notification { Id = 3, RatingId =3,  ServiceProviderId= 2 ,RatingValue = 2 },
                new Notification { Id = 4, RatingId =4,  ServiceProviderId= 2 ,RatingValue = 4 }
            };

            A.CallTo(() => _notificationService.GetAllNewNotifications(serviceProviderId)).Returns(fakeNotifications);

            // Act
            var result = controller.GetNewNotifications(serviceProviderId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(200, okResult?.StatusCode);
            Assert.Equal(fakeNotifications, okResult?.Value);
        }

        [Fact]
        public void GetNewNotifications_Exception_ReturnsServerError()
        {
            // Arrange
            var controller = new NotificationController(_logger, _notificationService);

            var serviceProviderId = 1;

            A.CallTo(() => _notificationService.GetAllNewNotifications(A<int>._)).Throws(new Exception("Simulated exception"));

            // Act
            var result = controller.GetNewNotifications(serviceProviderId);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult?.StatusCode);
            Assert.Equal("An error occurred while processing the request.", objectResult?.Value);
        }
    }
}