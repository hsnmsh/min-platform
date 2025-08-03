namespace MinPlatform.Logging.Service
{
    using MinPlatform.Logging.Abstractions;
    using MinPlatform.Logging.Abstractions.Models;
    using System;

    public sealed class LoggerManager
    {
        private readonly ISystemLogger systemLogger;
        private readonly Lazy<ISystemLoggerFactory> lazySystemLoggerFactory;

        public LoggerManager(ISystemLoggerFactory systemLoggerFactory)
        {
            systemLogger = systemLoggerFactory.Create();
            this.lazySystemLoggerFactory = new Lazy<ISystemLoggerFactory>(systemLoggerFactory);
        }

        private ISystemLoggerFactory systemLoggerFactory
        {
            set
            {
                value = lazySystemLoggerFactory.Value;
            }
            get
            {
                return lazySystemLoggerFactory.Value;
            }
        }

        public void Error(string message)
        {
            systemLogger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            systemLogger.Error(message, exception);
        }

        public void Information(string message)
        {
            systemLogger.Information(message);
        }

        public void Warning(string message)
        {
            systemLogger.Warning(message);
        }

        public void LogErrorMessageTemplate(Exception exception, string templateMessage, params object[] parameters)
        {
            systemLogger.LogErrorMessageTemplate(exception, templateMessage, parameters);
        }

        public void LogMessageTemplate(string templateMessage, Microsoft.Extensions.Logging.LogLevel logLevel, params object[] parameters)
        {
            systemLogger.LogMessageTemplate(templateMessage, logLevel, parameters);
        }

        public LoggerManager OverrideAndCreateNewLoggingManager(LoggingConfig loggingConfig)
        {
            systemLoggerFactory.LoggingConfig = loggingConfig;

            return new LoggerManager(systemLoggerFactory);
        }
    }
}
