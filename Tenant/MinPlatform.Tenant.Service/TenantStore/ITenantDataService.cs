namespace MinPlatform.Tenant.Service.TenantStore
{
    using MinPlatform.Tenant.Service.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITenantDataService
    {
        Task<IList<SiteConfig>> GetSiteConfigByIdAsync(string Id);

        Task<IList<SiteConfig>> GetSiteConfigByDomainAsync(string domain);

        Task<SiteConfig> GetSiteConfigByDomainAndLanguageCodeAsync(string domain, string languageCode);

        Task<SiteConfig> GetSiteConfigByIdAndLanguageCodeAsync(string Id, string languageCode);
    }
}
