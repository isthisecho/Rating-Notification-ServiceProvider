using HomeRun.RatingService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Testcontainers.RabbitMq;

namespace HomeRun.NotificationService.Tests.Integration
{
    public class NotificationApiFactory : WebApplicationFactory<HomeRun.NotificationService.Program>, IAsyncLifetime
    {
        private readonly RabbitMqContainer _rabbitMqContainer;

        public NotificationApiFactory()
        {
            _rabbitMqContainer = new RabbitMqBuilder()
                .WithExposedPort(5672)
                .WithPortBinding(5672)
                .WithEnvironment("RABBITMQ_DEFAULT_HOST", "rabbitmq")
                .WithEnvironment("RABBITMQ_DEFAULT_USER", "user")
                .WithEnvironment("RABBITMQ_DEFAULT_PASS", "pass")
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
        }


        public async Task InitializeAsync()
        {
            await _rabbitMqContainer.StartAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _rabbitMqContainer.StopAsync();
            await _rabbitMqContainer.DisposeAsync();
        }
    }
}
