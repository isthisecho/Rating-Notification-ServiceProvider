using Microsoft.EntityFrameworkCore;

namespace HomeRun.RatingService
{
    public class RatingDbContext :DbContext
    { 

        public DbSet<Rating> Ratings { get; set; }

        public RatingDbContext(DbContextOptions<RatingDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ServiceProviderX>().HasData(new ServiceProviderX
            {
                Id = 1,
                Name= "ProviderA",
            });

            modelBuilder.Entity<ServiceProviderX>().HasData(new ServiceProviderX
            {
                Id = 2,
                Name = "ProviderB",
            });

            modelBuilder.Entity<ServiceProviderX>().HasData(new ServiceProviderX
            {
                Id = 3,
                Name = "ProviderC",
            });


         
        }
    }
}
