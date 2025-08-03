namespace MinPlatform.Notifications.Service.Email
{
    using MinPlatform.ConfigStore.Service;
    using MinPlatform.Logging.Service;
    using MinPlatform.Notifications.Abstractions;
    using MinPlatform.Notifications.Abstractions.Email;
    using MinPlatform.Notifications.Abstractions.Exceptions;
    using MinPlatform.Notifications.Email.Sendgrid;
    using MinPlatform.Notifications.Email.Smtp;

    public sealed class EmailFactory : IEmailFactory
    {
        private const string smtp = nameof(smtp);
        private const string sendgrid = nameof(sendgrid);

        private readonly LoggerManager loggerManager;
        private readonly ConfigStoreManager configStoreManager;
        private readonly NotificationConfig notificationConfig;

        public EmailFactory(LoggerManager loggerManager, ConfigStoreManager configStoreManager, NotificationConfig notificationConfig)
        {
            this.loggerManager = loggerManager;
            this.configStoreManager = configStoreManager;
            this.notificationConfig = notificationConfig;
        }

        public NotificationService Create()
        {
            switch (notificationConfig.ProviderName)
            {
                case smtp:
                    return new SmtpNotificationService(loggerManager, configStoreManager, notificationConfig);
                case sendgrid:
                    return new SendgridNotificationService(loggerManager, configStoreManager, notificationConfig);

                default:
                    throw new NotificationException("Inavlid notification provider");
            }
        }
    }
}
