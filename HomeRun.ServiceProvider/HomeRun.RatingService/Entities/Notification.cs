namespace HomeRun.RatingService.Entities
{
    public class Notification
    {
        public string    RatingId               { get; set; } = string.Empty;
        public string    ServiceProviderId      { get; set; } = string.Empty;
        public int       RatingValue            { get; set; }

    }
}
