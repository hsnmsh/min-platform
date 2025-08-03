using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace MinPlatform.DI.Service
{
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.DI.Abstractions;
    using System;

    public sealed class ServiceProviderFromContainerBuilderResolver : IObjectResolver<ContainerBuilder, IServiceProvider>
    {
        private readonly IServiceCollection services;

        public ServiceProviderFromContainerBuilderResolver(IServiceCollection services)
        {
            this.services = services;
        }

        public IServiceProvider ResolveObjectBuilder(ContainerBuilder objectBuilderType)
        {
            objectBuilderType.Populate(services);

            var container = objectBuilderType.Build();
            var serviceProvider = new AutofacServiceProvider(container);

            return serviceProvider;
        }
    }
}
