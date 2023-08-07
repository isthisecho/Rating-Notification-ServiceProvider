
namespace HomeRun.NotificationService
{
    public class NotificationDTO 
    {

        public Guid      Id                         { get; set; } = Guid.NewGuid();
        public int       RatingId                   { get; set; } 
        public int       ServiceProviderId          { get; set; } 
        public int       RatingValue                { get; set; }
        public DateTime  CreatedAt                  { get; set; } 

    }
}
