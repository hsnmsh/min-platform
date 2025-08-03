namespace MinPlatform.Tenant.Service
{
    using MinPlatform.Caching.Service;
    using MinPlatform.Tenant.Service.Models;
    using MinPlatform.Tenant.Service.TenantConfigStore;
    using MinPlatform.Tenant.Service.TenantStore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class TenantManager
    {
        private const string cacheTenantPrefix = "Tenant_";
        private const string cacheDomainPrefix = "Domain_";
        private const string cacheLanguageCodePrefix = "LanguageCode_";
        private const string cacheTenantConfigPrefix = "TenantConfig_";

        private readonly ITenantDataService tenantDataService;
        private readonly ITenantConfigInfoDataService tenantConfigInfoDataService;
        private readonly CachingManager cachingManager;

        public TenantManager(
            ITenantDataService tenantDataService,
            ITenantConfigInfoDataService tenantInfoInfoDataService,
            CachingManager cachingManager)
        {
            this.tenantDataService = tenantDataService;
            this.tenantConfigInfoDataService = tenantInfoInfoDataService;
            this.cachingManager = cachingManager;
        }

        public async Task<SiteConfig> GetSiteConfigByDomainAndLanguageCodeAsync(string domain, string languageCode)
        {
            if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentNullException(nameof(domain));
            }

            if (string.IsNullOrEmpty(languageCode))
            {
                throw new ArgumentNullException(nameof(languageCode));
            }

            SiteConfig siteConfig = cachingManager.Get<SiteConfig>(cacheTenantPrefix + domain + "_" + languageCode);

            if (siteConfig is null)
            {
                siteConfig = await tenantDataService.GetSiteConfigByDomainAndLanguageCodeAsync(domain, languageCode);

                cachingManager.Set(cacheTenantPrefix + domain + "_" + languageCode, siteConfig);

            }

            return siteConfig;

        }

        public async Task<IEnumerable<SiteConfig>> GetSiteConfigAsync(string domain)
        {
            if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentNullException(nameof(domain));
            }

            IList<SiteConfig> siteConfig = cachingManager.Get<IList<SiteConfig>>(cacheTenantPrefix + cacheDomainPrefix + domain);

            if (siteConfig is null)
            {
                siteConfig = await tenantDataService.GetSiteConfigByDomainAsync(domain);

                cachingManager.Set(cacheTenantPrefix + cacheDomainPrefix + domain, siteConfig);
            }

            return siteConfig;
        }

        public async Task<SiteConfig> GetSiteConfigAsync(string Id, string languageCode)
        {
            if (string.IsNullOrEmpty(Id))
            {
                throw new ArgumentNullException(nameof(Id));
            }

            if (string.IsNullOrEmpty(languageCode))
            {
                throw new ArgumentNullException(nameof(languageCode));
            }

            SiteConfig siteConfig = cachingManager.Get<SiteConfig>(cacheTenantPrefix + cacheLanguageCodePrefix + Id);

            if (siteConfig is null)
            {
                siteConfig = await tenantDataService.GetSiteConfigByIdAndLanguageCodeAsync(Id, languageCode);

                cachingManager.Set(cacheTenantPrefix + cacheLanguageCodePrefix + Id, siteConfig);
            }

            return siteConfig;
        }

        public async Task<IEnumerable<SiteConfig>> GetSiteConfigByIdAsync(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                throw new ArgumentNullException(nameof(Id));
            }

            IList<SiteConfig> siteConfig = cachingManager.Get<IList<SiteConfig>>(cacheTenantPrefix + Id);

            if (siteConfig is null)
            {
                siteConfig = await tenantDataService.GetSiteConfigByIdAsync(Id);

                cachingManager.Set(cacheTenantPrefix + Id, siteConfig);
            }

            return siteConfig;
        }

        public async Task<IEnumerable<TenantConfig>> GetTenantConfigAsync(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            IList<TenantConfig> connectionInfo = cachingManager.Get<IList<TenantConfig>>(cacheTenantConfigPrefix + tenantId);

            if (connectionInfo is null)
            {
                connectionInfo = await tenantConfigInfoDataService.GetTenantConfigByTenantIdAsync(tenantId);

                cachingManager.Set(cacheTenantConfigPrefix + tenantId, connectionInfo);
            }

            return connectionInfo;

        }

        public async Task<TenantConfig> GetTenantConfigAsync(string tenantId, string connectionName)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (string.IsNullOrEmpty(connectionName))
            {
                throw new ArgumentNullException(nameof(connectionName));
            }

            TenantConfig connectionInfo = cachingManager.Get<TenantConfig>(cacheTenantConfigPrefix + tenantId + "_" + connectionName);

            if (connectionInfo is null)
            {
                connectionInfo = await tenantConfigInfoDataService.GetTenantConfigByTenantIdAndNameAsync(tenantId, connectionName);

                cachingManager.Set(cacheTenantConfigPrefix + tenantId + "_" + connectionName, connectionInfo);
            }

            return connectionInfo;
        }

        public async Task GetTenantConfigAsync(string tenantId, Action<IEnumerable<TenantConfig>> action)
        {
            IEnumerable<TenantConfig> connectionInfo = await this.GetTenantConfigAsync(tenantId);

            action(connectionInfo);

        }

        public async Task GetTenantConfigAsync(string tenantId, string connectionName, Action<TenantConfig> action)
        {
            var connectionInfo = await this.GetTenantConfigAsync(tenantId, connectionName);

            action(connectionInfo);

        }
    }
}
