using AutoMapper;
using HomeRun.Shared;
using HomeRun.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeRun.RatingService
{
    public class RatingService : IRatingService
    {
        private readonly  IRepository<ServiceProvider>  _serviceProviderRepository;
        private readonly  IRepository<Rating>           _ratingRepository         ;
        private readonly  IMapper                       _mapper                   ;
        private readonly  ILogger<RatingService>        _logger                   ;



        public RatingService(IRepository<Rating> ratingRepository, IMapper mapper, ILogger<RatingService> logger, IRepository<ServiceProvider> serviceProviderRepository)
        {
            _serviceProviderRepository = serviceProviderRepository  ;
            _ratingRepository          = ratingRepository           ;
            _mapper                    = mapper                     ;
            _logger                    = logger                     ;

        }

        public async Task<double> GetAverageRating(int serviceProviderId)
        {
            try
            {
                IEnumerable<Rating> ratings = await _ratingRepository.Where(x => x.ServiceProviderId == serviceProviderId);

                if (ratings.Any())
                {
                    double averageRating = ratings.Average(x => x.RatingValue);
                    return averageRating;
                }
                else
                {
                    _logger.LogWarning("No ratings found for the service provider with ID: {serviceProviderId}" , serviceProviderId);
                    return 0; 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while calculating the average rating: {ex.Message}", ex.Message);
                throw; 
            }

        }

    public async Task<Rating> SubmitRating(RatingDTO rating)
        {
            try
            {
                ServiceProvider? provider = await _serviceProviderRepository.GetById(rating.ServiceProviderId);

                if (provider is null)
                    throw new HomeRunException("No Service Provider exists with specified id.");

                else if (!(0 <= rating.RatingValue && rating.RatingValue <= 5))
                    throw new HomeRunException("Rating Value must be between 0-5");

                Rating ratingObj = _mapper.Map<Rating>(rating);
                Rating? newRating = await _ratingRepository.Create(ratingObj);

                if (newRating == null)
                    throw new ArgumentNullException();


                return newRating;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while submitting the rating: {ex.Message}" , ex.Message);
                throw; 
            }
        }
    }
}
