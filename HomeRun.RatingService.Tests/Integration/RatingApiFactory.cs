using Docker.DotNet.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;

namespace HomeRun.RatingService.Tests.Integration
{
    public class RatingApiFactory : WebApplicationFactory<HomeRun.RatingService.Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer;
        private readonly RabbitMqContainer _rabbitMqContainer;

        public RatingApiFactory() 
        {

             _dbContainer = new PostgreSqlBuilder()
                .WithDatabase("RatingDB")
                .WithUsername("postgres")
                .WithPassword("1234")
                .WithCleanUp(true)
                .Build();

            _rabbitMqContainer = new RabbitMqBuilder()
                .WithExposedPort(5672)
                .WithPortBinding(5672)
                .WithEnvironment("RABBITMQ_DEFAULT_HOST", "rabbitmq")
                .WithEnvironment("RABBITMQ_DEFAULT_USER", "user"    )
                .WithEnvironment("RABBITMQ_DEFAULT_PASS", "pass"    )
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            string connectionString = _dbContainer.GetConnectionString();
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {

                services.RemoveAll(typeof(DbContextOptions<RatingDbContext>));
                services.RemoveAll(typeof(RatingDbContext));

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

            using (var scope = Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<RatingDbContext>();

                await cntx.Database.EnsureCreatedAsync();
                await cntx.SaveChangesAsync();
            }
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _rabbitMqContainer.StopAsync();
            await _dbContainer.StopAsync();
        }
    }
}
