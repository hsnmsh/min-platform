namespace MinPlatform.DI.ServiceCollection
{
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.DI.Abstractions;
    using System;

    public sealed class ServiceProviderResolver : IObjectResolver<IServiceCollection, IServiceProvider>
    {
        public IServiceProvider ResolveObjectBuilder(IServiceCollection objectBuilderType)
        {
            if(objectBuilderType is ServiceCollection services)
            {
                return services.BuildServiceProvider();
            }

            throw new InvalidOperationException("the objectBuilderType must be Microsoft.Extensions.DependencyInjection.ServiceCollection");
        }
    }
}
