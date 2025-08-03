namespace MinPlatform.Logging.Service.Extensions
{
    using Autofac;
    using System;

    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder AddLoggingContext(this ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterType<LoggerManager>().InstancePerDependency();

            return builder;

        }

    }
}
