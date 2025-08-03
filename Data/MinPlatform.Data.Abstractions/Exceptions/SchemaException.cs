namespace MinPlatform.Data.Abstractions.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public sealed class SchemaException : Exception
    {
        public SchemaException()
        {
        }

        public SchemaException(string message) : base(message)
        {
        }

        public SchemaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SchemaException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
