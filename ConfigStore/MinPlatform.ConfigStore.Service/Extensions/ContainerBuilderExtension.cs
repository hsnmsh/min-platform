namespace MinPlatform.ConfigStore.Service.Extensions
{
    using Autofac;
    using System;

    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder AddConfigStore(this ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterType<ConfigStoreService>()
                .As<IConfigStoreService>()
                .InstancePerDependency();

            builder.RegisterType<ConfigStoreManager>()
                .InstancePerDependency();

            return builder;

        }
    }
}
