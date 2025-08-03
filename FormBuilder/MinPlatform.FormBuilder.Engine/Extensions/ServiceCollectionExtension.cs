namespace MinPlatform.FormBuilder.Engine.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.FormBuilder.DataProviders.Extensions;
    using MinPlatform.FormBuilder.Engine.DataService;
    using System;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection BuildFormEngine(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IFormResolver, FormResolver>();
            services.AddTransient<IFormInfoDataService, FormInfoDataService>();
            services.AddDataProviderFactory();
            services.AddTransient<FormManager>();

            return services;
        }
    }
}
