namespace MinPlatform.Logging.Abstractions.Models
{
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;

    public abstract class Properties
    {
        public bool Enabled
        {
            get;
            set;
        }

        public LogLevel LogLevel 
        { 
            get; 
            set;
        }

        public IDictionary<string, object> StorageProperties
        {
            get;
            set;
        }
    }
}
