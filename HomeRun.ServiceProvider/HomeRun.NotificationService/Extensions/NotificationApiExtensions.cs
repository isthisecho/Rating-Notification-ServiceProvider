using HomeRun.Shared;

namespace HomeRun.NotificationService
{
    public static class NotificationApiExtensions
    {

        public static void AddContexts(this IServiceCollection services)
        {
            services.AddHostedService<NotificationProccessor>();
            services.AddSingleton<IHostedService, NotificationProccessor>();
            services.AddSingleton<INotificationService, NotificationService>();
        }
    }
}
