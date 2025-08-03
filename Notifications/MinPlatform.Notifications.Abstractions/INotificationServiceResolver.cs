namespace MinPlatform.Notifications.Abstractions
{
    public interface INotificationServiceResolver
    {
        NotificationService Create(NotificationTypes notificationType);
    }
}
