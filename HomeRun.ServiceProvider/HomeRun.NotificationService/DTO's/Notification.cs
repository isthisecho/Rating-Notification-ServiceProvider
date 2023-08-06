using HomeRun.Shared;

namespace HomeRun.NotificationService
{
    public class Notification : BaseEntity
    {

        public int       RatingId             { get; set; } 
        public int       ServiceProviderId    { get; set; } 
        public int       RatingValue          { get; set; }

    }
}
