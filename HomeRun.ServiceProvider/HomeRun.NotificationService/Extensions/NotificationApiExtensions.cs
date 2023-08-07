using HomeRun.Shared;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using IConnectionFactory = RabbitMQ.Client.IConnectionFactory;

namespace HomeRun.NotificationService
{
    public static class NotificationApiExtensions
    {

        public static void AddContexts(this IServiceCollection services)
        {
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddHostedService<NotificationProccessor>();
        }

        public static void AddRabbitMQContexts(this IServiceCollection services)
        {
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

    }
}
