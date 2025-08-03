namespace MinPlatform.Logging.Serilog.Extensions
{
    using Autofac;
    using global::Serilog;
    using MinPlatform.Logging.Abstractions;
    using MinPlatform.Logging.Abstractions.Models;
    using System;

    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder AddSerilogSystemLoggerFactory(this ContainerBuilder builder, LoggingConfig config = null)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Register<ISystemLoggerFactory>(ctx =>
            {
                var loggerResolver = ctx.Resolve<ILoggerResolver<ILogger>>();
                var serilogSystem = new SerilogSystemLoggerFactory(loggerResolver);

                if (config != null)
                {
                    serilogSystem.LoggingConfig = config;
                }

                return serilogSystem;

            }).InstancePerDependency();

            return builder;
        }

        public static ContainerBuilder AddSerilogLoggerResolver(this ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterType<SerilogLoggerResolver>().As<ILoggerResolver<ILogger>>().InstancePerDependency();

            return builder;
        }
    }
}
