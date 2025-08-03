namespace MinPlatform.Data.Service.Lookup
{
    using System;
    using System.Runtime.Serialization;

    public sealed class LookupException : Exception
    {
        public LookupException()
        {
        }

        public LookupException(string message) : base(message)
        {
        }

        public LookupException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public LookupException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
