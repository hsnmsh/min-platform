namespace MinPlatform.Tenant.Service
{
    using MinPlatform.Tenant.Service.Models;
    using System.Collections.Generic;

    public sealed class TenantContext
    {
        IEnumerable<SiteConfig> TenantProfiles 
        { 
            get;
            set;
        }

        IEnumerable<TenantConfig> TenantConfigs 
        { 
            get;
            set;
        }
    }
}
