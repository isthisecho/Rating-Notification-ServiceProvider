using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace HomeRun.RatingService.Tests.Integration
{
    public class RatingApiFactory : WebApplicationFactory<HomeRun.RatingService.Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer;
        internal readonly RabbitMqContainer _rabbitMqContainer;

        public RatingApiFactory() 
        {

             _dbContainer = new PostgreSqlBuilder()
                .WithCleanUp(true)
                .Build();

            _rabbitMqContainer = new RabbitMqBuilder()
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            string connectionString = _dbContainer.GetConnectionString();
            string rabbitConnectionString = _rabbitMqContainer.GetConnectionString();

            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<RatingDbContext>));
                services.RemoveAll(typeof(RatingDbContext));
                services.RemoveAll(typeof(ConnectionFactory));

                services.AddSingleton<RabbitMQ.Client.IConnectionFactory>(sp =>
                {
                    return new ConnectionFactory
                    {
                        Uri = new Uri(rabbitConnectionString)
                    };
                });

                services.AddDbContext<RatingDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });


            }

            );
        }
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            await _rabbitMqContainer.StartAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _rabbitMqContainer.StopAsync();
            await _rabbitMqContainer.DisposeAsync();
            await _dbContainer.StopAsync();
        }
    }
}
