namespace MinPlatform.Notifications.Abstractions.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public sealed class NotificationException : Exception
    {
        public NotificationException()
        {
        }

        public NotificationException(string message) : base(message)
        {
        }

        public NotificationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NotificationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
