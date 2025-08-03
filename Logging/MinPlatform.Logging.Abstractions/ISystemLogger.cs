namespace MinPlatform.Logging.Abstractions
{
    using Microsoft.Extensions.Logging;
    using System;

    public interface ISystemLogger
    {
        void Information(string message);
        void Warning(string message);
        void Error(string message);
        void Error(string message, Exception exception);
        void LogMessageTemplate(string templateMessage, LogLevel logLevel, params object[] parameters);
        void LogErrorMessageTemplate(Exception exception, string templateMessage, params object[] parameters);


    }
}
