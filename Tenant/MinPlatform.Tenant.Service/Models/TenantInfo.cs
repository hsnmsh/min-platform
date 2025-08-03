namespace MinPlatform.Tenant.Service.Models
{
    using System.Collections.Generic;

    public sealed class TenantInfo
    {
        public IEnumerable<SiteConfig> TenantProfile
        { 
            get;
            set;
        }

        public IEnumerable<TenantConfig> TenantConfigs 
        { 
            get;
            set;
        }
    }
}
