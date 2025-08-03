namespace MinPlatform.Notifications.Abstractions.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INotificationDataService
    {
        Task<IEnumerable<Notification>> GetNotificationsAsync(string notificationName);

        Task<IEnumerable<Notification>> GetNotificationsAsync(string notificationName, string languageCode);

    }
}
