namespace MinPlatform.Data.Service.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.Data.Service.Lookup;
    using MinPlatform.Validators.Service.Extensions;
    using System;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterDataContext(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddValidators();
            services.AddTransient<DataManager>();
            services.AddTransient<ILookupDataService, LookupDataService>();
            services.AddTransient<ILookupInfoDataService, LookupInfoDataService>();
            services.AddTransient<LookupManager>();

            return services;
        }
    }
}
