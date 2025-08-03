namespace MinPlatform.FormBuilder.DataProviders.Extensions
{
    using Autofac;
    using System;
    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder AddDataProviderFactory(this ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterType<DataProviderFactory>().As<IDataProviderFactory>().InstancePerDependency();

            return builder;

        }
    }
}
