using Autofac.Extensions.DependencyInjection;

namespace MinPlatform.DI.Autofac.Extensions
{
    using global::Autofac;
    using global::Autofac.Core;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Collections.Generic;

    public static class HostBuilderExtension
    {
        public static IHostBuilder GenerateAutofacFactory(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            return hostBuilder;
        }

        public static IHostBuilder GenerateAutofacFactory(this IHostBuilder hostBuilder, Action<ContainerBuilder> actionBuilder)
        {
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory(builder => actionBuilder(builder)));

            return hostBuilder;
        }

        public static IHostBuilder ConfiguerAppServices(this IHostBuilder hostBuilder,IEnumerable<IModule> modules)
        {
            hostBuilder.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.BuildModules(modules);


            });

            return hostBuilder;
        }

        public static IHostBuilder ConfiguerAppServices(this IHostBuilder hostBuilder, Action<ContainerBuilder> actionBuilder)
        {
            hostBuilder.ConfigureContainer<ContainerBuilder>(builder =>
            {
                actionBuilder(builder);

            });

            return hostBuilder;
        }
    }
}
