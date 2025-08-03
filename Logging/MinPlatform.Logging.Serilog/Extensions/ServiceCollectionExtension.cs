namespace MinPlatform.Logging.Serilog.Extensions
{
    using global::Serilog;
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.Logging.Abstractions;
    using MinPlatform.Logging.Abstractions.Models;
    using Serilog;
    using System;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSerilogSystemLoggerFactory(this IServiceCollection services, LoggingConfig config = null)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<ISystemLoggerFactory>(serviceProvider =>
            {
                var loggerResolver = serviceProvider.GetService<ILoggerResolver<ILogger>>();
                var serilogSystem = new SerilogSystemLoggerFactory(loggerResolver);

                if (config != null)
                {
                    serilogSystem.LoggingConfig = config;
                }

                return serilogSystem;
            });

            return services;
        }

        public static IServiceCollection AddSerilogLoggerResolver(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<ILoggerResolver<ILogger>, SerilogLoggerResolver>();

            return services;
        }
    }
}
