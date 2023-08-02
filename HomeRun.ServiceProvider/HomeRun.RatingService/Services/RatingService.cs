using AutoMapper;
using HomeRun.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeRun.RatingService
{
    public class RatingService : IRatingService
    {
        private readonly  IRepository<Rating> _ratingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public RatingService(IRepository<Rating> ratingRepository, IMapper mapper, ILogger<Rating> logger)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<double> GetAverageRating(int serviceProviderId)
        {
            IEnumerable<Rating> ratings= await  _ratingRepository.GetAll();
            return ratings.Select(x=> x.RatingValue).Average();
        }

        public async Task<Rating> SubmitRating(RatingDTO rating)
        {
            Rating ratingObj  = _mapper.Map<Rating>(rating);
            Rating? newRating = await _ratingRepository.Create(ratingObj);

            return newRating;
        }
    }
}
