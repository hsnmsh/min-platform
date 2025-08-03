namespace MinPlatform.DI.Service
{
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.DI.Abstractions;
    using System;

    public sealed class ServiceCollectionInstanceManager : InstanceManager<IServiceProvider>
    {
        public ServiceCollectionInstanceManager(IServiceProvider resolverType) : base(resolverType)
        {
        }

        public override T Resolve<T>()
        {
            return resolverType.GetService<T>();
        }
    }
}
