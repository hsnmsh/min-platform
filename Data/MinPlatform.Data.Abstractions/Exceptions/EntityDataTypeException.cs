namespace MinPlatform.Data.Abstractions.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public sealed class EntityDataTypeException : Exception
    {
        public EntityDataTypeException()
        {
        }

        public EntityDataTypeException(string message) : base(message)
        {
        }

        public EntityDataTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public EntityDataTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
