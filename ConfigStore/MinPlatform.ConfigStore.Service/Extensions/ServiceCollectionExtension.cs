namespace MinPlatform.ConfigStore.Service.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddConfigStore(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IConfigStoreService, ConfigStoreService>();
            services.AddTransient<ConfigStoreManager>();

            return services;

        }
    }
}
