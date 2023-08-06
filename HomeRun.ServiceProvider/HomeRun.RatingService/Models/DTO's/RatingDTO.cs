using System.ComponentModel.DataAnnotations;

namespace HomeRun.RatingService
{
    public class RatingDTO 
    {
        [Required(ErrorMessage = "ServiceProviderId is required.")]
        public int ServiceProviderId  { get; set; }

        [Required(ErrorMessage = "RatingValue is required.")]
        [Range(0, 5, ErrorMessage = "RatingValue must be between 0 and 5.")]
        public int RatingValue        { get; set; }

    }
}
