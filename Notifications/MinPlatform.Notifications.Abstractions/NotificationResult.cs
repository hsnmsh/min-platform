using System.Collections.Generic;

namespace MinPlatform.Notifications.Abstractions
{
    public sealed class NotificationResult
    {
        public bool Success 
        { 
            get;
            set;
        }

        public IList<string> Errors 
        { 
            get;
            set;
        }

        public IDictionary<string, object> Properties 
        { 
            get; 
            set;
        }
    }
}
