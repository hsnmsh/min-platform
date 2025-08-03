namespace MinPlatform.Notifications.Service
{
    using MinPlatform.Notifications.Abstractions;
    using MinPlatform.Notifications.Abstractions.Email;
    using System;

    public sealed class NotificationServiceResolver : INotificationServiceResolver
    {
        private readonly Lazy<IEmailFactory> lazyEmailFactory;

        public NotificationServiceResolver(IEmailFactory emailFactory)
        {
            lazyEmailFactory = new Lazy<IEmailFactory>(() => emailFactory);
        }

        private IEmailFactory emailFactory
        {
            get
            {
                return lazyEmailFactory.Value;
            }
            set
            {
                value = lazyEmailFactory.Value;
            }
        }
        public NotificationService Create(NotificationTypes notificationType)
        {
            switch (notificationType)
            {
                case NotificationTypes.Email:
                    return emailFactory.Create();

                default:
                    throw new InvalidOperationException("Inavlid notification type");
            }
        }
    }
}
