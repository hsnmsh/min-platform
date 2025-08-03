using Autofac;

namespace MinPlatform.Validators.Service.Extensions
{
    using System;

    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder AddValidators(this ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterType<ValidatorManager>().InstancePerDependency();

            return builder;
        }
    }
}
