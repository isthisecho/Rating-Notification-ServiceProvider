using HomeRun.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeRun.NotificationService
{
    public class Notification :BaseEntity
    {


        [Required]
        public string    RatingId               { get; set; } = string.Empty;

        [Required]
        public string    ServiceProviderId      { get; set; } = string.Empty;

        [Required]
        public int       RatingValue            { get; set; }

    }
}
