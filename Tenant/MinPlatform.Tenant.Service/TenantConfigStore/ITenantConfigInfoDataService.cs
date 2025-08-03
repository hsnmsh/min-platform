namespace MinPlatform.Tenant.Service.TenantConfigStore
{
    using MinPlatform.Tenant.Service.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITenantConfigInfoDataService
    {
        Task<IList<TenantConfig>> GetTenantConfigByTenantIdAsync(string tenantId);

        Task<TenantConfig> GetTenantConfigByTenantIdAndNameAsync(string TenantId, string connectionName);
    }
}
