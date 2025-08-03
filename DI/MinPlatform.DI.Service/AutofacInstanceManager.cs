using Autofac;
using MinPlatform.DI.Abstractions;

namespace MinPlatform.DI.Service
{
    public class AutofacInstanceManager : InstanceManager<IComponentContext>
    {
        public AutofacInstanceManager(IComponentContext resolverType) : base(resolverType)
        {
        }

        public override T Resolve<T>()
        {
            return resolverType.Resolve<T>();
        }
    }
}
