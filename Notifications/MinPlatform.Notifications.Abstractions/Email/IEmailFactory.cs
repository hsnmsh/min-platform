namespace MinPlatform.Notifications.Abstractions.Email
{
    public interface IEmailFactory
    {
        NotificationService Create();
    }
}
