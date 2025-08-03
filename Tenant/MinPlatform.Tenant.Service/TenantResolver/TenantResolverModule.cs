namespace MinPlatform.Tenant.Service.TenantResolver
{
    using Autofac;
    using MinPlatform.DI.Autofac.DynamicLoader;
    using MinPlatform.Tenant.Service.Models;
    using System;

    public class TenantResolverModule : AbstractDynamicLoaderModule<TenantInfo>
    {
        private readonly TenantInfo tenantInfo;

        public TenantResolverModule(TenantInfo inputModule) : base(inputModule)
        {
            tenantInfo = inputModule;
        }

        public override void LoadModules(ContainerBuilder containerBuilder)
        {
            if (containerBuilder is null)
            {
                containerBuilder = new ContainerBuilder();
            }

            Type type = Helpers.GetTypeFromTenantConfig(tenantInfo.TenantConfigs);

            if(type != null)
            {
                ITenantServicesResolver<ContainerBuilder> serviceType = (ITenantServicesResolver<ContainerBuilder>)Activator.CreateInstance(type, null);
                containerBuilder = serviceType.BuildTenantService(containerBuilder, tenantInfo);
            }
        }
    }
}
