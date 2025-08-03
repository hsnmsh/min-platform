namespace MinPlatform.FormBuilder.Elements.Inputs
{
    using System;
    using System.Runtime.Serialization;

    internal sealed class ErrorInputException : Exception
    {
        public ErrorInputException()
        {
        }

        public ErrorInputException(string message) : base(message)
        {
        }

        public ErrorInputException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ErrorInputException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
