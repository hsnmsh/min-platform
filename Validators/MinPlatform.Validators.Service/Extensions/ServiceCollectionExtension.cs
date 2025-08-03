namespace MinPlatform.Validators.Service.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<ValidatorManager>();

            return services;
        }
    }
}
