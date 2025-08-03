namespace MinPlatform.Logging.Service.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLoggingContext(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<LoggerManager>();

            return services;
        }
    }
}
