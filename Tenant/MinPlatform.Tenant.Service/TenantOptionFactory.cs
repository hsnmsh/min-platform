namespace MinPlatform.Tenant.Service
{
    using Microsoft.Extensions.Options;
    using MinPlatform.Tenant.Service.Models;
    using System;
    using System.Collections.Generic;

    public sealed class TenantOptionFactory : IOptionsFactory<IEnumerable<SiteConfig>>
    {
        private readonly TenantManager tenantManager;
        private readonly string tenantId;

        public TenantOptionFactory(TenantManager tenantManager, string tenantId)
        {
            this.tenantManager = tenantManager ?? throw new ArgumentNullException(nameof(tenantManager));
            this.tenantId = tenantId ?? throw new ArgumentNullException(nameof(tenantId));
        }

        public IEnumerable<SiteConfig> Create(string name)
        {
            var tenantSiteConfig = tenantManager.GetSiteConfigByIdAsync(tenantId).GetAwaiter().GetResult();

            return tenantSiteConfig;
        }
    }
}
