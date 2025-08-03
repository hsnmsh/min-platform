namespace MinPlatform.Notifications.Abstractions
{
    using MinPlatform.Notifications.Abstractions.Data;
    using System.Collections.Generic;

    public sealed class NotificationInputs
    {
        public IDictionary<string, object> Inputs 
        { 
            get;
            set;
        }

        public IDictionary<string, object> PlaceHolders
        {
            get;
            set;
        }

        public Notification Notification
        {
            get;
            set;
        }
    }
}
