using Autofac;

namespace MinPlatform.DI.Service.Extensions
{
    using System;

    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterAutofacInstanceManager(this ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterType<AutofacInstanceManager>()
                .SingleInstance();

            return builder;
        }
    }
}
