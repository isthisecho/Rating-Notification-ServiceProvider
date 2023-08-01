using HomeRun.NotificationService.Entities;

namespace HomeRun.NotificationService.Services
{
    public interface IRatingService
    {
        Task<Rating> SubmitRating       (Rating rating        );
        Task<double> GetAverageRating   (int serviceProviderId);
        Task         NotifyNewRating                         ();
    }
}
