
namespace HomeRun.NotificationService
{
    public interface INotificationService
    {
        IEnumerable<Notification> GetAllNewNotifications    (int serviceProviderId    );
        void                      AddNotification           (Notification notification);
    }
}
