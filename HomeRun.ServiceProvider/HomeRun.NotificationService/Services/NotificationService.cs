
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
        public IEnumerable<Notification> GetAllNewNotifications(int serviceProivderId)
        {
            List<Notification> notificationsToReturn = _notifications.Where(x => x.ServiceProviderId == serviceProivderId).ToList();
            _notifications.RemoveAll(x => x.ServiceProviderId == serviceProivderId);
            return notificationsToReturn;
        }

        public void AddNotification(Notification notification)
        {
                _notifications.Add(notification);
        }
    }
}
