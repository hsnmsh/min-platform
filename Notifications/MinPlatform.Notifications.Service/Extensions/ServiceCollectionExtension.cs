namespace MinPlatform.Notifications.Service.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.Notifications.Abstractions;
    using MinPlatform.Notifications.Abstractions.Data;
    using MinPlatform.Notifications.Abstractions.Email;
    using MinPlatform.Notifications.Service.Data;
    using MinPlatform.Notifications.Service.Email;
    using System;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddNotificationContext(this IServiceCollection services, NotificationConfig notificationConfig)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<INotificationDataService, NotificationDataService>();
            services.AddTransient<IEmailFactory, EmailFactory>();
            services.AddTransient<INotificationServiceResolver, NotificationServiceResolver>();
            services.AddSingleton(notificationConfig);

            services.AddTransient<NotificationManager>();

            return services;

        }
    }
}
