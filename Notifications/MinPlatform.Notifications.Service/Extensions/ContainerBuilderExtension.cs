namespace MinPlatform.Notifications.Service.Extensions
{
    using Autofac;
    using MinPlatform.Notifications.Abstractions;
    using MinPlatform.Notifications.Abstractions.Data;
    using MinPlatform.Notifications.Abstractions.Email;
    using MinPlatform.Notifications.Service.Data;
    using MinPlatform.Notifications.Service.Email;
    using System;

    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder AddNotificationContext(this ContainerBuilder builder, NotificationConfig notificationConfig)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterType<NotificationDataService>()
                .As<INotificationDataService>()
                .InstancePerDependency();

            builder.RegisterType<EmailFactory>()
                .As<IEmailFactory>()
                .InstancePerDependency();

            builder.RegisterType<NotificationServiceResolver>()
               .As<INotificationServiceResolver>()
               .InstancePerDependency();

            builder.Register((ctx) =>
            {
                return notificationConfig;

            }).SingleInstance();

            builder.RegisterType<NotificationManager>()
               .InstancePerDependency();

            return builder;
        }
    }
}
