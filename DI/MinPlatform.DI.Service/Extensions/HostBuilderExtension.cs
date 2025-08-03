namespace MinPlatform.DI.Service.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;

    public static class HostBuilderExtension
    {
        private const string ConfigureServices = nameof(ConfigureServices);

        /// <summary>
        /// Specify the startup type to be used by the host.
        /// </summary>
        /// <typeparam name="TStartup">The type containing an optional constructor with
        /// an <see cref="IConfiguration"/> parameter. The implementation should contain a public
        /// method named ConfigureServices with <see cref="IServiceCollection"/> parameter.</typeparam>
        /// <param name="hostBuilder">The <see cref="IHostBuilder"/> to initialize with TStartup.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder UseStartup<TStartup>(
            this IHostBuilder hostBuilder) where TStartup : class
        {
            hostBuilder.ConfigureServices((ctx, serviceCollection) =>
            {
                var cfgServicesMethod = typeof(TStartup).GetMethod(ConfigureServices,
                    new Type[] { typeof(IServiceCollection) });

                var hasConfigCtor = typeof(TStartup).GetConstructor(
                    new Type[] { typeof(IConfiguration) }) != null;

                var startUpObj = hasConfigCtor ?
                    (TStartup)Activator.CreateInstance(typeof(TStartup), ctx.Configuration) :
                    (TStartup)Activator.CreateInstance(typeof(TStartup), null);

                cfgServicesMethod?.Invoke(startUpObj, new object[] { serviceCollection });
            });

            return hostBuilder;
        }

        public static IHostBuilder UseStartup(
           this IHostBuilder hostBuilder, Action<HostBuilderContext, IServiceCollection> actionConfig)
        {
            hostBuilder.ConfigureServices((ctx, serviceCollection) =>
            {
                actionConfig(ctx, serviceCollection);
            });

            return hostBuilder;
        }
    }
}
