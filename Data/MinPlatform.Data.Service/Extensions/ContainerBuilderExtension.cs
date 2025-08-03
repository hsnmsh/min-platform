using Autofac;

namespace MinPlatform.Data.Service.Extensions
{
    using MinPlatform.Data.Service.Lookup;
    using MinPlatform.Validators.Service.Extensions;
    using System;

    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder RegisterDataContext(this ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddValidators();

            builder.RegisterType<DataManager>()
                .InstancePerDependency();

            builder.RegisterType<LookupDataService>()
                .As<ILookupDataService>()
                .InstancePerDependency();

            builder.RegisterType<LookupInfoDataService>()
               .As<ILookupInfoDataService>()
               .InstancePerDependency();

            builder.RegisterType<LookupManager>()
               .InstancePerDependency();


            return builder;


        }
    }
}
