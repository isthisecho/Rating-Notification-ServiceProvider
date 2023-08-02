using HomeRun.RatingService;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);



builder.Host.UseSerilog((context,configuration) =>  { configuration.ReadFrom.Configuration(context.Configuration); });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddContexts(builder.Configuration);    // Extension method for wrapping all relevant DI's.
builder.Services.AddAutoMapper                   ();    // Extension method for wrapping AutoMapper configuration.


WebApplication app = builder.Build();


app.ApplyPendingMigrations(builder.Services); // Extension method for DB Migration.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
