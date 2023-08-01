using Microsoft.EntityFrameworkCore;

namespace HomeRun.NotificationService
{
    public class NotificationDbContext :DbContext
    { 

        public DbSet<Notification> Notifications { get; set; }

        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
