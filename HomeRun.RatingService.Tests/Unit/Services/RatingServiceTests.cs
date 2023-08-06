using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Xunit;
using HomeRun.RatingService.Models;
using HomeRun.Shared;
using HomeRun.Shared.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;

namespace HomeRun.RatingService.Tests.UnitTests.Services
{
    public class RatingServiceTests
    {

        private readonly IRepository<Rating> _ratingRepository;
        private readonly IRepository<ServiceProvider> _serviceProviderRepository;
        private readonly ILogger<RatingService> _logger;
        private readonly IMapper _mapper;



        public RatingServiceTests()
        {
            _serviceProviderRepository= A.Fake<IRepository<ServiceProvider>>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<RatingService>>();
            _ratingRepository = A.Fake<IRepository<Rating>>();
        }


        [Fact]
        public async Task GetAverageRating_WithExistingRatings_ReturnsAverageRating()
        {
            // Arrange
            var serviceProviderId = 123;
            var ratings = new List<Rating>
            {
                new Rating { ServiceProviderId = serviceProviderId, RatingValue = 4 },
                new Rating { ServiceProviderId = serviceProviderId, RatingValue = 5 },
                new Rating { ServiceProviderId = serviceProviderId, RatingValue = 3 }
            };

            var _ratingRepository = A.Fake<IRepository<Rating>>();
            A.CallTo(() => _ratingRepository.Where(A<Expression<Func<Rating, bool>>>._)).Returns(ratings);

            var ratingService = new RatingService(_ratingRepository, _mapper, _logger, _serviceProviderRepository);

            // Act
            var result = await ratingService.GetAverageRating(serviceProviderId);

            // Assert
            Assert.Equal(4.0, result); // The expected average value
        }

        [Fact]
        public async Task SubmitRating_ValidData_ReturnsNewRating()
        {
            // Arrange
            var ratingDto       = new RatingDTO         { ServiceProviderId = 123, RatingValue = 4 };
            var serviceProvider = new ServiceProvider   { Id = 123 };

            A.CallTo(() => _serviceProviderRepository.GetById(A<int>._)).Returns(serviceProvider);
            A.CallTo(() => _ratingRepository.Create(A<Rating>._)).Returns(new Rating());
            A.CallTo(() => _mapper.Map<Rating>(A<RatingDTO>._)).Returns(new Rating());

            var ratingService = new RatingService(_ratingRepository, _mapper, _logger, _serviceProviderRepository);

            // Act
            var result = await ratingService.SubmitRating(ratingDto);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(6)]
        public async Task SubmitRating_InvalidRatingValue_ReturnsException(int ratingValue)
        {
            // Arrange
            var ratingDto = new RatingDTO { ServiceProviderId = 123, RatingValue = ratingValue };
            A.CallTo(() => _serviceProviderRepository.GetById(A<int>._)).Returns(new ServiceProvider());

            var ratingService = new RatingService(_ratingRepository, _mapper, _logger, _serviceProviderRepository);

            // Act & Assert
            await Assert.ThrowsAsync<HomeRunException>(() => ratingService.SubmitRating(ratingDto));
        }

        [Fact]
        public async Task SubmitRating_InvalidServiceProvider_ReturnsException()
        {
            ServiceProvider? serviceProvider = null;
            // Arrange
            var ratingDto = new RatingDTO { ServiceProviderId = 123, RatingValue = 4 };
            A.CallTo(() => _serviceProviderRepository.GetById(A<int>._)).Returns(serviceProvider);

            var ratingService = new RatingService(_ratingRepository, _mapper, _logger, _serviceProviderRepository);

            // Act & Assert
            await Assert.ThrowsAsync<HomeRunException>(() => ratingService.SubmitRating(ratingDto));
        }


    }
}