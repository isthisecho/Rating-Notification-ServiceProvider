
namespace HomeRun.NotificationService
{
    public interface INotificationService
    {
        IEnumerable<NotificationDTO> GetAllNewNotifications    (int serviceProviderId    );
        void                      AddNotification           (NotificationDTO notification);
    }
}
