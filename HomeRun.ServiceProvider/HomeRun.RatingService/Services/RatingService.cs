using AutoMapper;
using HomeRun.Shared;
using Microsoft.EntityFrameworkCore;

namespace HomeRun.RatingService
{
    public class RatingService : IRatingService
    {
        private readonly  IRepository<Rating> _ratingRepository;
        private readonly IMapper _mapper;

        public RatingService(IRepository<Rating> ratingRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<double> GetAverageRating(int serviceProviderId)
        {
            IEnumerable<Rating> ratings= await  _ratingRepository.GetAll();
            return ratings.Select(x=> x.RatingValue).Average();
        }

        public Task NotifyNewRating(NotificationDTO rating)
        {
            throw new NotImplementedException();
        }

        public async Task<Rating> SubmitRating(RatingDTO rating)
        {
            Rating ratingObj  = _mapper.Map<Rating>(rating);
            Rating? newRating = await _ratingRepository.Create(ratingObj);

            return newRating;
        }
    }
}
