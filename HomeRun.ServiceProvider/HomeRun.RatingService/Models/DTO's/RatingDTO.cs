using System.ComponentModel.DataAnnotations;

namespace HomeRun.RatingService
{
    public class RatingDTO 
    {
        [Required]
        public int ServiceProviderId  { get; set; }

        [Required]
        public int RatingValue        { get; set; }

    }
}
