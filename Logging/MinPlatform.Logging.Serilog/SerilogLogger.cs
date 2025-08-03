using Serilog;

namespace MinPlatform.Logging.Serilog
{
    using MinPlatform.Logging.Abstractions;
    using System;

    public sealed class SerilogLogger : ISystemLogger
    {
        private readonly ILogger logger;

        public SerilogLogger(ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            logger.Error(exception, message);
        }

        public void Information(string message)
        {
            logger.Information(message);
        }

        public void LogErrorMessageTemplate(Exception exception, string templateMessage, params object[] parameters)
        {
            logger.Error(exception, templateMessage, parameters);
        }

        public void LogMessageTemplate(string templateMessage, Microsoft.Extensions.Logging.LogLevel logLevel, params object[] parameters)
        {
            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    logger.Debug(templateMessage, parameters);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Information:
                    logger.Information(templateMessage, parameters);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    logger.Warning(templateMessage, parameters);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Error:
                    logger.Error(templateMessage, parameters);
                    break;
            }
        }

        public void Warning(string message)
        {
            logger.Warning(message);
        }

    }
}
