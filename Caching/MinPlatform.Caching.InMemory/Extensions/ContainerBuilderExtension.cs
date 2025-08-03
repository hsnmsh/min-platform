namespace MinPlatform.Caching.InMemory.Extensions
{
    using Autofac;
    using MinPlatform.Caching.Service;
    using System;

    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder AddInMemoryCache(this ContainerBuilder builder)
        {
            if (builder is null) 
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterType<InMemoryCachingServiceFactory>()
                .As<ICachingServiceFactory>()
                .InstancePerDependency();

            return builder;


        }
    }
}
