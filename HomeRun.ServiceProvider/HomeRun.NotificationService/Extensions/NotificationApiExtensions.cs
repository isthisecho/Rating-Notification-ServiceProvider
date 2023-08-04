using HomeRun.Shared;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;

namespace HomeRun.NotificationService
{
    public static class NotificationApiExtensions
    {

        public static void AddContexts(this IServiceCollection services)
        {
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddHostedService<NotificationProccessor>();
        }
    }
}
