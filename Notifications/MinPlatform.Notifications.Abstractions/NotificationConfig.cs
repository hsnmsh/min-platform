namespace MinPlatform.Notifications.Abstractions
{
    using System.Collections.Generic;

    public sealed class NotificationConfig
    {
        public string ProviderName 
        { 
            get;
            set;
        }

        public IDictionary<string, object> ConfigInputs 
        { 
            get;
            set;
        }
    }
}
