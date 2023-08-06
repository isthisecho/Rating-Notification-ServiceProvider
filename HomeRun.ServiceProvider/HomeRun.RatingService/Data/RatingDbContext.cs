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

            modelBuilder.Entity<ServiceProvider>().HasData(new ServiceProvider
            {
                Id = 1,
                Name= "Provider A",
            });

            modelBuilder.Entity<ServiceProvider>().HasData(new ServiceProvider
            {
                Id = 2,
                Name = "Provider B",
            });

            modelBuilder.Entity<ServiceProvider>().HasData(new ServiceProvider
            {
                Id = 3,
                Name = "Provider C",
            });


         
        }
    }
}
