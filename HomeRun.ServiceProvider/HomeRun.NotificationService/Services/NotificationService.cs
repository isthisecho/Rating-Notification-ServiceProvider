using HomeRun.Shared;

namespace HomeRun.NotificationService
{
    public class NotificationService : INotificationService
    {

        private readonly IRepository<Notification> _notificationRepository;

        public NotificationService(IRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

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
