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

            if (_db.Database.GetPendingMigrations().Any())
                await _db.Database.MigrateAsync();
        }

        public static void AddContexts(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<RatingDbContext>(context => context.UseNpgsql(configuration.GetConnectionString("WebApiConnection")));
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
