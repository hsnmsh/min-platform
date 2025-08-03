namespace MinPlatform.DI.Service.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterServiceCollectionInstanceManager(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<ServiceCollectionInstanceManager>();

            return services;
        }
    }
}
