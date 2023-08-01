using HomeRun.NotificationService.Entities;

namespace HomeRun.NotificationService.Services
{
    public class RatingService : IRatingService
    {
        public Task<double> GetAverageRating(int serviceProviderId)
        {
            throw new NotImplementedException();
        }

        public Task NotifyNewRating()
        {
            throw new NotImplementedException();
        }

        public Task<Rating> SubmitRating(Rating rating)
        {
            throw new NotImplementedException();
        }
    }
}
