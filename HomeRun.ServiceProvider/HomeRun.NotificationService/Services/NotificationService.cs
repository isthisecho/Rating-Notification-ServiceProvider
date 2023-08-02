
namespace HomeRun.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly List<Notification> _notifications = new();
        private readonly ILogger _logger;
        public NotificationService(ILogger logger)
        {
            _logger = logger;
        }

        public  IEnumerable<Notification> GetAllNewNotifications()
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
