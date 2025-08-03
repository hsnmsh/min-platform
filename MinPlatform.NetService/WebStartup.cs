namespace MinPlatform.NetService
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.Caching.InMemory.Extensions;
    using MinPlatform.Caching.Service.Extensions;
    using MinPlatform.Caching.Service.Models;
    using MinPlatform.ConfigStore.Service.Extensions;
    using MinPlatform.Data.Service.Extensions;
    using MinPlatform.Data.Sql.Extensions;
    using MinPlatform.Logging.Abstractions.Models;
    using MinPlatform.Tenant.Service;
    using MinPlatform.Tenant.Service.Models;
    using System;
    using System.Linq;
    using MinPlatform.Logging.Serilog.Extensions;
    using MinPlatform.Logging.Service.Extensions;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;
    using Autofac;
    using MinPlatform.Notifications.Abstractions;
    using MinPlatform.Notifications.Service.Extensions;

    public sealed class WebStartup : TenantStartup
    {
        public WebStartup(IConfiguration configuration, TenantInfo tenantInfo) : base(configuration, tenantInfo)
        {
        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var sqlConnectionProperties = TenantInfo.TenantConfigs.Where(tenantInfo => tenantInfo.Name == Tenant.Service.Constants.SQLConnection).Single();
            string connectionString= sqlConnectionProperties.Properties["connectionString"].ToString();

            services.RegisterSqlServerBuilders(connectionString);
            services.RegisterDataContext();

            services.AddInMemoryCache();

            //caching section
            var config = Configuration.GetSection("MinPlatform:CachingOption");
            var cachingOption = new CachingOption();

            config.Bind(cachingOption);

            services.AddCachingContext(cachingOption);

            //confugstore section
            services.AddConfigStore();

            //logging section
            services.AddSerilogLoggerResolver();
            var loggingConfigSection = Configuration.GetSection("MinPlatform:LoggingConfig");
            var loggingConfig = new LoggingConfig();
            loggingConfig.LoggingProperties = new SeqProperties();


            loggingConfigSection.Bind(loggingConfig);

            services.AddSerilogSystemLoggerFactory(loggingConfig);
            services.AddLoggingContext();


            //Notification section
            var notificationConfigSection = Configuration.GetSection("MinPlatform:Notification");
            var notificationConfig = new NotificationConfig();
            notificationConfigSection.Bind(notificationConfig);

            services.AddNotificationContext(notificationConfig);



            return base.ConfigureServices(services);
        }
    }
}
