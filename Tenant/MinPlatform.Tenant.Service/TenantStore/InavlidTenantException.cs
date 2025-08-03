namespace MinPlatform.Tenant.Service.TenantStore
{
    using System;
    using System.Runtime.Serialization;

    public sealed class InavlidTenantException : Exception
    {
        public InavlidTenantException()
        {
        }

        public InavlidTenantException(string message) : base(message)
        {
        }

        public InavlidTenantException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InavlidTenantException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
