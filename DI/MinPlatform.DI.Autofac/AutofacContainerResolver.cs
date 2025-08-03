using Autofac;

namespace MinPlatform.DI.Autofac
{
    using MinPlatform.DI.Abstractions;

    public sealed class AutofacContainerResolver : IObjectResolver<ContainerBuilder, IContainer>
    {
        public IContainer ResolveObjectBuilder(ContainerBuilder containerBuilder)
        {
            return containerBuilder.Build();
        }

    }
}
