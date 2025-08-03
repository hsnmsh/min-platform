using MinPlatform.Logging.Abstractions.Models;

namespace MinPlatform.Logging.Abstractions
{
    public interface ISystemLoggerFactory
    {
        LoggingConfig LoggingConfig
        {
            get;
            set;
        }

        ISystemLogger Create();
    }
}
