using HomeRun.NotificationService;
using Serilog;

namespace HomeRun.NotificationService;
public class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


        builder.Host.UseSerilog((context, configuration) => { configuration.ReadFrom.Configuration(context.Configuration); }); //Adding Serilog



        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services.AddContexts(); // Extension method for wrapping all relevant DI's.

        WebApplication app = builder.Build();



        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}