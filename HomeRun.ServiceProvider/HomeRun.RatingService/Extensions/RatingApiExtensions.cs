using AutoMapper;
using HomeRun.RatingService.Mapper;
using HomeRun.Shared;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;


namespace HomeRun.RatingService
{
    public static class RatingApiExtensions
    {
        public static async void ApplyPendingMigrations(this WebApplication webApplication, IServiceCollection services)
        {
            using IServiceScope scope       = webApplication.Services.CreateScope();
            await using RatingDbContext _db = scope.ServiceProvider.GetRequiredService<RatingDbContext>();

            await _db.Database.GetPendingMigrationsAsync();
        }

        public static void AddContexts(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<RatingDbContext>(context => context.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING")));
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<DbContext, RatingDbContext>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IMessageProducer, MessageProducer>();
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }


    }


}
