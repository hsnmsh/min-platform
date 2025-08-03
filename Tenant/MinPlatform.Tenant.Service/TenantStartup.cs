namespace MinPlatform.Tenant.Service
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.DI.Service;
    using MinPlatform.Tenant.Service.Models;
    using MinPlatform.Tenant.Service.TenantResolver;
    using System;
    using System.Data;
    using System.Linq;

    public class TenantStartup : Startup
    {
        protected readonly TenantInfo TenantInfo;
        public TenantStartup(IConfiguration configuration, TenantInfo tenantInfo) : base(configuration)
        {
            this.TenantInfo = tenantInfo;
        }

        public virtual IServiceCollection ConfigureTenantServices(IServiceCollection services)
        {
            if (services is null)
            {
                services = new ServiceCollection();
            }

            var tenantResolverConfig = TenantInfo.
                TenantConfigs
                .Where(tenantConfig => tenantConfig.Name == Constants.ResolverConfig)
                .FirstOrDefault();

            Type type = Helpers.GetTypeFromTenantConfig(TenantInfo.TenantConfigs);

            if(type != null)
            {
                ITenantServicesResolver<IServiceCollection> serviceType = (ITenantServicesResolver<IServiceCollection>)Activator.CreateInstance(type, null);
                services = (ServiceCollection)serviceType.BuildTenantService(services, TenantInfo);
            }

            return services;

        }
    }
}
