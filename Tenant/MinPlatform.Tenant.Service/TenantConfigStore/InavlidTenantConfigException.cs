namespace MinPlatform.Tenant.Service.TenantConfigStore
{
    using System;
    using System.Runtime.Serialization;

    public sealed class InavlidTenantConfigException : Exception
    {
        public InavlidTenantConfigException()
        {
        }

        public InavlidTenantConfigException(string message) : base(message)
        {
        }

        public InavlidTenantConfigException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InavlidTenantConfigException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
