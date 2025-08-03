namespace MinPlatform.Logging.Abstractions
{
    using MinPlatform.Logging.Abstractions.Models;

    public interface ILoggerResolver<LoggerType> where LoggerType : class
    {
        LoggerType ResolveLogger(LoggingConfig config);
    }
}
