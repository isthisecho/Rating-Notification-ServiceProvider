
namespace HomeRun.NotificationService
{
    public interface INotificationService
    {
        IEnumerable<Notification> GetAllNewNotifications();
        void AddNotification(Notification notification);
    }
}
