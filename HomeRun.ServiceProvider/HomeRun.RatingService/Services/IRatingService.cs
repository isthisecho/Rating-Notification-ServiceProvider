
namespace HomeRun.RatingService
{
    public interface IRatingService
    {
        Task<Rating> SubmitRating       (RatingDTO rating);
        Task<double> GetAverageRating   (int serviceProviderId);
        Task         NotifyNewRating    (NotificationDTO rating);
    }
}
