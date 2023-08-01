using HomeRun.RatingService.Entities;

namespace HomeRun.RatingService.Services
{
    public class NotificationService : INotificationService
    {
        public Task<IEnumerable<Notification>?> GetNewNotifications()
        {
            throw new NotImplementedException();
        }

        public Task<Notification> PostNotification(Notification notification)
        {
            throw new NotImplementedException();
        }
    }
}
