namespace MinPlatform.DI.Service
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.DI.Abstractions;
    using MinPlatform.DI.ServiceCollection;
    using System;

    public class Startup
    {
        protected IConfiguration Configuration { get; }

        private readonly IObjectResolver<IServiceCollection, IServiceProvider> objectResolver;

        public Startup(IConfiguration configuration)
        {
            if (configuration is null)
            {
                var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

                Configuration = builder.Build();
            }
            else
            {
                Configuration = configuration;
            }

            objectResolver = new ServiceProviderResolver();
        }

        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return objectResolver.ResolveObjectBuilder(services);
        }
    }
}
