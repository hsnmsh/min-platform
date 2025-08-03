using Autofac;

namespace MinPlatform.DI.Autofac.DynamicLoader
{
    using System.Collections.Generic;

    public sealed class LoadByEnvModule<TService> : AbstractDynamicLoaderModule<IDictionary<string, TService>>
    {
        private readonly string envName;

        public LoadByEnvModule(IDictionary<string, TService> inputModule, string envName) : base(inputModule)
        {
            this.envName = envName;
        }

        public override void LoadModules(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType(inputModule[envName].GetType())
                .As<TService>().InstancePerDependency();
        }
    }
}
