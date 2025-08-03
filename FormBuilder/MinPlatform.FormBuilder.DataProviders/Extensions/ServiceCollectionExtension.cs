namespace MinPlatform.FormBuilder.DataProviders.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDataProviderFactory(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IDataProviderFactory, DataProviderFactory>();

            return services;
        }
    }
}
