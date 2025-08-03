namespace MinPlatform.FormBuilder.Engine
{
    using System;
    using System.Runtime.Serialization;

    public sealed class FormEngineException : Exception
    {
        public FormEngineException()
        {
        }

        public FormEngineException(string message) : base(message)
        {
        }

        public FormEngineException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public FormEngineException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
