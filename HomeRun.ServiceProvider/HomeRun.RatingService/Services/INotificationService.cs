using HomeRun.RatingService.Entities;

namespace HomeRun.RatingService.Services
{
    public interface INotificationService
    {

        Task<Notification>               PostNotification   (Notification notification);
        Task<IEnumerable<Notification>?> GetNewNotifications                         ();
    }
}
