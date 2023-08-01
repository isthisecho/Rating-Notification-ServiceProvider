
namespace HomeRun.NotificationService
{
    public interface INotificationService
    {

        Task<Notification>               PostNotification   (Notification notification);
        Task<IEnumerable<Notification>?> GetNewNotifications                         ();
    }
}
