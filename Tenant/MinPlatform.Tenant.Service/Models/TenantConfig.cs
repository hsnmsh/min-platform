namespace MinPlatform.Tenant.Service.Models
{
    using System.Collections.Generic;

    public sealed class TenantConfig
    {
        public string TenantId 
        { 
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public IDictionary<string, object> Properties
        {
            get;
            set;
        }
    }
}
