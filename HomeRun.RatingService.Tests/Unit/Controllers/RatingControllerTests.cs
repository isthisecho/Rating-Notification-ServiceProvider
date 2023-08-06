
using FakeItEasy;
using HomeRun.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomeRun.RatingService.Tests.UnitTests.Controllers
{
    public class RatingControllerTests
    {
        private readonly IRatingService              _ratingService;
        private readonly IMessageProducer            _messageProducer;
        private readonly ILogger<RatingController>   _logger;

        public RatingControllerTests()
        {
            _ratingService      = A.Fake<IRatingService>             ();
            _messageProducer    = A.Fake<IMessageProducer>           ();
            _logger             = A.Fake<ILogger<RatingController>>  ();
        }

        [Theory]
        [InlineData(1, 5)] // ServiceProviderId: 1, RatingValue: 5
        [InlineData(2, 4)] // ServiceProviderId: 2, RatingValue: 4
        [InlineData(3, 3)] // ServiceProviderId: 3, RatingValue: 3
        public async Task RatingController_SubmitRating_ValidRating_ReturnsOk(int serviceProviderId, int ratingValue)
        {
            // Arrange
            int i = 1;
            RatingDTO rating = new RatingDTO() { RatingValue = ratingValue, ServiceProviderId = serviceProviderId }; 
            A.CallTo(() => _ratingService.SubmitRating(A<RatingDTO>._)).Returns(new Rating() { Id=i++, RatingValue=ratingValue , ServiceProviderId = serviceProviderId });
            var controller = new RatingController(_logger, _ratingService, _messageProducer);

            // Act
            var result = await controller.SubmitRating(rating) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode); // OK status code
        }


        [Fact]
        public async Task RatingController_GetAverageRating_ValidId_ReturnsOk()
        {
            // Arrange
            int id = 1; // Set up your test ID
            A.CallTo(() => _ratingService.GetAverageRating(A<int>._)).Returns(4.5);

            var controller = new RatingController(_logger, _ratingService, _messageProducer);

            // Act
            var result = await controller.GetAverageRating(id) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode); // OK status code
        }
    }
}
