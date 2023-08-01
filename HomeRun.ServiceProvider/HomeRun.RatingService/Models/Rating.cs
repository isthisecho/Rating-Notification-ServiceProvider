using HomeRun.Shared;

namespace HomeRun.RatingService
{
    public class Rating : BaseEntity
    {
        public int ServiceProviderXId   { get; set; } 
        public int    RatingValue       { get; set; }
    }
}
