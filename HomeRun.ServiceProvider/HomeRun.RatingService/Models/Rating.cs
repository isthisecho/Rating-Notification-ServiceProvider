using HomeRun.Shared;

namespace HomeRun.RatingService
{
    public class Rating : BaseEntity
    {
        public int    ServiceProviderId     { get; set; } 
        public int    RatingValue           { get; set; }
    }
}