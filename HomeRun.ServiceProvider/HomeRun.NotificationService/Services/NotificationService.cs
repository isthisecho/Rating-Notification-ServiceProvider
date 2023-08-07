
namespace HomeRun.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly List<NotificationDTO> _notifications = new();
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<NotificationDTO> GetAllNewNotifications(int serviceProivderId)
        {
            //It is returning notifications related with specified serviceProviderId  and removes from the list.
            List<NotificationDTO> notificationsToReturn = _notifications.Where(x => x.ServiceProviderId == serviceProivderId).ToList();
            _notifications.RemoveAll(x => x.ServiceProviderId == serviceProivderId);
            return notificationsToReturn;
        }

        public void AddNotification(NotificationDTO notification)
        {
                _notifications.Add(notification);
        }
    }
}
