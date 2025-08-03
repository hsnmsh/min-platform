namespace MinPlatform.Tenant.Service.TenantResolver
{
    using MinPlatform.Tenant.Service.Models;

    public interface ITenantServicesResolver<ServiceBuilderType> where ServiceBuilderType : class
    {
        ServiceBuilderType BuildTenantService(ServiceBuilderType serviceBuilderType, TenantInfo tenantInfo);
    }
}
