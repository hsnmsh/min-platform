using Autofac;

namespace MinPlatform.DI.Autofac.DynamicLoader
{
    using System;

    public abstract class AbstractDynamicLoaderModule<InputModule> : Module where InputModule : class
    {
        protected readonly InputModule inputModule;


        public AbstractDynamicLoaderModule()
        {

        }

        public AbstractDynamicLoaderModule(InputModule inputModule)
        {
            this.inputModule = inputModule ?? throw new ArgumentNullException(nameof(inputModule));
        }

        public abstract void LoadModules(ContainerBuilder containerBuilder);

        protected override void Load(ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            LoadModules(builder);
        }

    }
}
