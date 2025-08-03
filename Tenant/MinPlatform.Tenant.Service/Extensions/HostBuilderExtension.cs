namespace MinPlatform.Tenant.Service.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using MinPlatform.Tenant.Service.Models;
    using System;

    public static class HostBuilderExtension
    {
        private const string ConfigureServices = nameof(ConfigureServices);
        private const string ConfigureTenantServices = nameof(ConfigureTenantServices);

        public static IHostBuilder UseStartup<TStartup>(
            this IHostBuilder hostBuilder, TenantInfo tenantInfo) where TStartup : TenantStartup
        {
            hostBuilder.ConfigureServices((ctx, serviceCollection) =>
            {
                var cfgServicesMethod = typeof(TStartup).GetMethod(ConfigureServices,
                    new Type[] { typeof(IServiceCollection) });

                var cfgTenantServicesMethod = typeof(TStartup).GetMethod(ConfigureTenantServices,
                    new Type[] { typeof(IServiceCollection) });

                var hasConfigCtor = typeof(TStartup).GetConstructor(
                    new Type[] { typeof(IConfiguration), typeof(TenantInfo) }) != null;

                var startUpObj = hasConfigCtor ?
                    (TStartup)Activator.CreateInstance(typeof(TStartup), ctx.Configuration, tenantInfo) :
                    (TStartup)Activator.CreateInstance(typeof(TStartup), null);

                serviceCollection = (IServiceCollection)cfgTenantServicesMethod?.Invoke(startUpObj, new object[] { serviceCollection });
                cfgServicesMethod?.Invoke(startUpObj, new object[] { serviceCollection });
            });

            return hostBuilder;
        }
    }
}
