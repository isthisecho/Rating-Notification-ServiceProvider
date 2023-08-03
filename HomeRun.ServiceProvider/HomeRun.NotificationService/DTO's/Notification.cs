using HomeRun.Shared;

namespace HomeRun.NotificationService
{
    public class Notification : BaseEntity
    {

        public string    RatingId               { get; set; } = string.Empty;

        public string    ServiceProviderId      { get; set; } = string.Empty;

        public int       RatingValue            { get; set; }

    }
}
