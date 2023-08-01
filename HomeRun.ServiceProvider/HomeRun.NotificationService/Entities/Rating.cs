namespace HomeRun.NotificationService.Entities
{
    public class Rating
    {
        public string Id                { get; set; } = string.Empty;
        public string ServiceProviderId { get; set; } = string.Empty;
        public int    RatingValue       { get; set; }
    }
}
