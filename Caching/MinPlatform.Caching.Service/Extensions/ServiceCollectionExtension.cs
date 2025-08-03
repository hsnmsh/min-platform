namespace MinPlatform.Caching.Service.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.Caching.Service.Models;
    using System;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCachingContext(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton(serviceProvider =>
            {
                return new CachingOption();
            });

            services.AddTransient<CachingManager>();

            return services;

        }

        public static IServiceCollection AddCachingContext(this IServiceCollection services, CachingOption cachingOption = null)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton(serviceProvider =>
            {
                if (cachingOption is null)
                {
                    return new CachingOption();
                }

                return cachingOption;

            });

            services.AddTransient<CachingManager>();

            return services;

        }

        public static IServiceCollection AddCachingContext(this IServiceCollection services, Func<CachingOption> cachingOptionResolver = null)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton(ctx =>
            {
                if (cachingOptionResolver is null)
                {
                    return new CachingOption();
                }

                return cachingOptionResolver();

            });

            services.AddTransient<CachingManager>();

            return services;

        }
    }
}
