namespace MinPlatform.Notifications.Abstractions.Data
{
    public sealed class Notification
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Subject
        {
            get;
            set;
        }

        public string MessageContent
        {
            get;
            set;
        }

        public NotificationTypes NotificationType
        {
            get;
            set;
        }

        public string LanguageCode
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public string Properties
        {
            get;
            set;
        }
    }
}
