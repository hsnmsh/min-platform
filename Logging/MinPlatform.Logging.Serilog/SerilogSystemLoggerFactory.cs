using Serilog;

namespace MinPlatform.Logging.Serilog
{
    using MinPlatform.Logging.Abstractions;
    using MinPlatform.Logging.Abstractions.Models;

    public sealed class SerilogSystemLoggerFactory : ISystemLoggerFactory
    {
        public LoggingConfig LoggingConfig
        {
            get;
            set;
        }

        private readonly ILoggerResolver<ILogger> loggerResolver;

        public SerilogSystemLoggerFactory(ILoggerResolver<ILogger> loggerResolver)
        {
            this.loggerResolver = loggerResolver;
        }

        public ISystemLogger Create()
        {
            ILogger logger = loggerResolver.ResolveLogger(LoggingConfig);

            return new SerilogLogger(logger);

        }
    }
}
