using HomeRun.Shared;
using System.ComponentModel.DataAnnotations;

namespace HomeRun.RatingService
{
    public class Rating : BaseEntity
    {
        [Required(ErrorMessage = "Service Provider ID is Required.")]
        public int    ServiceProviderId     { get; set; }

        [Required(ErrorMessage = "Rating Value is Required")]
        public int    RatingValue           { get; set; }
    }
}