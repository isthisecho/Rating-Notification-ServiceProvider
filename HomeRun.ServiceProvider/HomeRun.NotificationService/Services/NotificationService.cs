
namespace HomeRun.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly List<Notification> _notifications = new();
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }
        public IEnumerable<Notification> GetAllNewNotifications()
        {
                List<Notification> notificationsToReturn = new(_notifications);
                _notifications.Clear();

                return notificationsToReturn;
        }

        public void AddNotification(Notification notification)
        {
                _notifications.Add(notification);

        }
    }
}
