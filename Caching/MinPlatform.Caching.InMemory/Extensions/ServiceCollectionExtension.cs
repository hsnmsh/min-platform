namespace MinPlatform.Caching.InMemory.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.Caching.Service;
    using System;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<ICachingServiceFactory, InMemoryCachingServiceFactory>();

            return services;
        }
    }
}
