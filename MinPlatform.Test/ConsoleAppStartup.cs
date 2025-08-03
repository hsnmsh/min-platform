namespace MinPlatform.Test
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.Caching.InMemory;
    using MinPlatform.Caching.Service;
    using MinPlatform.DI.Service;
    using MinPlatform.Validators.Service.Extensions;

    public sealed class ConsoleAppStartup : Startup
    {
        public ConsoleAppStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICachingService, InMemoryCachingService>();
            services.AddValidators();

            return base.ConfigureServices(services);
        }
    }
}
