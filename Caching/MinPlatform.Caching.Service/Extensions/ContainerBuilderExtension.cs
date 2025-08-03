namespace MinPlatform.Caching.Service.Extensions
{
    using Autofac;
    using MinPlatform.Caching.Service.Models;
    using System;

    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder AddCachingContext(this ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Register(ctx =>
            {
                return new CachingOption();

            }).SingleInstance();

            builder.RegisterType<CachingManager>()
                .InstancePerDependency();

            return builder;

        }

        public static ContainerBuilder AddCachingContext(this ContainerBuilder builder, CachingOption cachingOption = null)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Register(ctx =>
            {
                if (cachingOption is null)
                {
                    return new CachingOption();
                }

                return cachingOption;

            }).SingleInstance();

            builder.RegisterType<CachingManager>()
                .InstancePerDependency();

            return builder;

        }

        public static ContainerBuilder AddCachingContext(this ContainerBuilder builder, Func<CachingOption> cachingOptionResolver = null)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Register(ctx =>
            {
                if (cachingOptionResolver is null)
                {
                    return new CachingOption();
                }

                return cachingOptionResolver();

            }).SingleInstance();

            builder.RegisterType<CachingManager>()
                .InstancePerDependency();

            return builder;

        }
    }
}
