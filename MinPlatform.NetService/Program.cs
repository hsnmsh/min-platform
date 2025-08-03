using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinPlatform.Caching.InMemory;
using MinPlatform.Caching.Service;
using MinPlatform.DI.Autofac;
using MinPlatform.DI.Autofac.DynamicLoader;
using MinPlatform.Validators.Service;
using System.Collections.Generic;
using MinPlatform.DI.Autofac.Extensions;
using Microsoft.Extensions.Configuration;
using Castle.DynamicProxy;
using System.IO;
using System.Linq;
using System;
using Autofac.Extras.DynamicProxy;
using Autofac.Core;
using MinPlatform.DI.Abstractions;
using MinPlatform.DI.Service.Extensions;
using MinPlatform.DI.ServiceCollection.Extensions;
using Decor;
using System.Threading.Tasks;
using MinPlatform.Validators.Service.Extensions;
using MinPlatform.Data.Sql.Extensions;
using MinPlatform.Data.Service.Extensions;
using MinPlatform.Caching.Service.Models;
using MinPlatform.Caching.InMemory.Extensions;
using MinPlatform.Caching.Service.Extensions;
using MinPlatform.ConfigStore.Service.Extensions;
using MinPlatform.Tenant.Service.Extensions;
using MinPlatform.DI.ServiceCollection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Net;
using MinPlatform.Tenant.Service.Models;
using MinPlatform.Tenant.Service;
using MinPlatform.Tenant.Service.TenantConfigStore;
using MinPlatform.Tenant.Service.TenantResolver;
using Serilog;
using MinPlatform.Logging.Serilog;
using MinPlatform.Logging.Serilog.Extensions;
using MinPlatform.Logging.Abstractions.Models;
using MinPlatform.Logging.Service.Extensions;
using Serilog.Configuration;
using MinPlatform.Logging.Abstractions;

namespace MinPlatform.NetService
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // build datamanager for configuration
            var configContanerBuilder = new ContainerBuilder();
            configContanerBuilder.AddSqlServerBuilders(builder.Configuration.GetConnectionString("ConfigConnectionString"));
            configContanerBuilder.RegisterDataContext();
            configContanerBuilder.AddCachingContext();
            configContanerBuilder.AddInMemoryCache();
            configContanerBuilder.AddTenantContext();

            var configContainerBuilder = new AutofacContainerResolver().ResolveObjectBuilder(configContanerBuilder);

            string tenantId = builder.Configuration.GetValue<string>("TenantId");
            TenantManager tenantManager = configContainerBuilder.Resolve<TenantManager>();

            IEnumerable<TenantConfig> tenantConfigs = tenantManager.GetTenantConfigAsync(tenantId).GetAwaiter().GetResult();
            IEnumerable<SiteConfig> siteConfigs = tenantManager.GetSiteConfigByIdAsync(tenantId).GetAwaiter().GetResult();



            builder.Services.AddSingleton(siteConfigs);


            builder.Services.AddControllers();




            builder.Host.UseStartup<WebStartup>(new TenantInfo
            {
                TenantConfigs = tenantConfigs,
                TenantProfile = siteConfigs
            });

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
            }


            app.MapControllers();

            app.Run();
        }
    }

    public class LoggingDecorator : IDecorator
    {
        public async Task OnInvoke(Call call)
        {
            Console.WriteLine("Will do some work!");
            await call.Next();
            Console.WriteLine("Work is finished!");
        }
    }

    public class TestDecorator
    {
        [Decorate(typeof(LoggingDecorator))]
        public virtual void RunTestDecorator()
        {
            Console.WriteLine("Message from Test Decorator");
        }
    }

    public interface IEnvService
    {
        void Test();
    }

    public interface ITestService
    {
        void TestService();
    }

    public class DevService : IEnvService, ITestService
    {
        public void Test()
        {
            Console.WriteLine("Hello from dev Service");
        }

        public void TestService()
        {
            Console.WriteLine("Hello from dev Test Service");

        }
    }

    public class ProdService : IEnvService
    {
        public void Test()
        {
            Console.WriteLine("Hello from prod Service");
        }
    }
}
