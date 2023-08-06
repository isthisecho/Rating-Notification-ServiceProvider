using AspNetCoreRateLimit;
using HomeRun.RatingService;
using HomeRun.RatingService.Middleware;
using HomeRun.Shared;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;

 namespace HomeRun.RatingService
{
    public class Program
    {
        private static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, configuration) => { configuration.ReadFrom.Configuration(context.Configuration); }); //Adding Serilog

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCustomRateLimiter();                        // Extension method for Custom Rate Limiter
            builder.Services.AddContexts();                                 // Extension method for wrapping all relevant DI's.
            builder.Services.AddAutoMapper();                               // Extension method for wrapping AutoMapper configuration.


            WebApplication app = builder.Build();

            app.ApplyPendingMigrations(builder.Services);                   // Extension method for DB Migration.

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRateLimiter();

            app.UseSerilogRequestLogging();                                 // Adding Request Logging
            app.UseMiddleware<ExceptionHandlerMiddleware>();                // Adding Middlewares

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }

}
