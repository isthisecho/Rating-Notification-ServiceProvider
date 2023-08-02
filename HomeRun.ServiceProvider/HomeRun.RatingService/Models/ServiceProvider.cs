using HomeRun.Shared;

namespace HomeRun.RatingService
{
    public class ServiceProvider : BaseEntity
    {
        public string?               Name    { get; set; }
        public ICollection<Rating>?  Ratings { get; set; } 
    }
}
