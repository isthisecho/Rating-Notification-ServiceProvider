using AspNetCoreRateLimit;
using AutoMapper;
using HomeRun.RatingService.Mapper;
using HomeRun.Shared;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Serilog;
using Serilog.Core;
using System.Threading.RateLimiting;

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

        public static void AddContexts(this IServiceCollection services )
        {
            services.AddDbContext<RatingDbContext>(context => context.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING")));
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<DbContext, RatingDbContext>();
            services.AddScoped<IRatingService, RatingService>();
        }

        public static void AddRabbitMQContexts(this IServiceCollection services)
        {
            services.AddScoped<IMessageProducer, MessageProducer>();
            services.AddSingleton<IConnectionFactory>(sp =>
            {
                string hostName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_HOST") ?? "localhost";  //Pre-defined values in case of nullability
                string userName = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_USER") ?? "guest";      //Pre-defined values in case of nullability
                string password = Environment.GetEnvironmentVariable("RABBITMQ_DEFAULT_PASS") ?? "guest";      //Pre-defined values in case of nullability

                return new ConnectionFactory
                {
                    HostName = hostName,
                    UserName = userName,
                    Password = password
                };
            });
        }



        public static void AddAutoMapper(this IServiceCollection services)
        {
            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }



        public static void AddCustomRateLimiter(this IServiceCollection services)
        {

         services.AddRateLimiter(rateLimiterOptions =>
         {
             rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
                {
                    options.PermitLimit = 10;
                    options.Window = TimeSpan.FromSeconds(10);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 5;
                });
            });
        }


    }


}
