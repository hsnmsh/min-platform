namespace MinPlatform.FormBuilder.Engine.Extensions
{
    using Autofac;
    using MinPlatform.FormBuilder.DataProviders.Extensions;
    using MinPlatform.FormBuilder.Engine.DataService;
    using System;

    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder BuildFormEngine(this ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterType<FormResolver>().As<IFormResolver>().InstancePerDependency();
            builder.RegisterType<FormInfoDataService>().As<IFormInfoDataService>().InstancePerDependency();
            builder.AddDataProviderFactory();
            builder.RegisterType<FormManager>().InstancePerDependency();

            return builder;

        }
    }
}
