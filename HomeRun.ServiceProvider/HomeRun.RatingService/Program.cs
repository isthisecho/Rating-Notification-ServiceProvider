using Microsoft.EntityFrameworkCore;
using HomeRun.RatingService;
using HomeRun.Shared;
using AutoMapper;
using HomeRun.RatingService.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RatingDbContext>(context => context.UseNpgsql(builder.Configuration.GetConnectionString("WebApiConnection")));

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddScoped<DbContext, RatingDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddSingleton<IConfiguration>(x => builder.Configuration);


WebApplication app = builder.Build();


using IServiceScope scope = app.Services.CreateScope();
await using RatingDbContext dbContext = scope.ServiceProvider.GetRequiredService<RatingDbContext>();
await dbContext.Database.MigrateAsync();

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
