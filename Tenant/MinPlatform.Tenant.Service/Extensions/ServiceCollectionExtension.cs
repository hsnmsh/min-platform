namespace MinPlatform.Tenant.Service.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.Tenant.Service.TenantConfigStore;
    using MinPlatform.Tenant.Service.TenantStore;
    using System;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddTenantContext(this IServiceCollection services)
        {
            if (services is null) 
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<ITenantDataService, TenantDataService>();
            services.AddTransient<ITenantConfigInfoDataService, TenantConfigInfoDataService>();
            services.AddTransient<TenantManager>();

            return services;

        }
    }
}
