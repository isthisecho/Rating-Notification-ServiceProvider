using HomeRun.Shared;

namespace HomeRun.RatingService
{
    public class ServiceProviderX : BaseEntity
    {

        public string               Name    { get; set; }
        public ICollection<Rating>  Ratings { get; set; } 
    }
}
