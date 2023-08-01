using System.ComponentModel.DataAnnotations;

namespace HomeRun.RatingService
{
    public class NotificationDTO
    {
        public int    RatingId           { get; set; } 

        public int    ServiceProviderId  { get; set; } 

        public int    RatingValue        { get; set; }
    }
}
