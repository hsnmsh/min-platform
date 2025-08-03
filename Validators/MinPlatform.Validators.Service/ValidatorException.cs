namespace MinPlatform.Validators.Service
{
    using System;
    using System.Runtime.Serialization;

    public sealed class ValidatorException : Exception
    {
        public ValidatorException()
        {
        }

        public ValidatorException(string message) : base(message)
        {
        }

        public ValidatorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ValidatorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
