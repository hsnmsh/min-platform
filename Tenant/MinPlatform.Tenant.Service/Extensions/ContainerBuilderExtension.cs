namespace MinPlatform.Tenant.Service.Extensions
{
    using Autofac;
    using MinPlatform.Tenant.Service.TenantConfigStore;
    using MinPlatform.Tenant.Service.TenantStore;
    using System;

    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder AddTenantContext(this ContainerBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterType<TenantDataService>()
                .As<ITenantDataService>()
                .InstancePerDependency();

            builder.RegisterType<TenantConfigInfoDataService>()
                .As<ITenantConfigInfoDataService>()
                .InstancePerDependency();

            builder.RegisterType<TenantManager>()
                .InstancePerDependency();

            return builder;
        }
    }
}
