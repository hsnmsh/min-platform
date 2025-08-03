using Autofac;
using Autofac.Core;


namespace MinPlatform.DI.Autofac.Extensions
{
    using System.Collections.Generic;

    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder BuildModules(this ContainerBuilder containerBuilder, IEnumerable<IModule> modules)
        {
            if (containerBuilder is null)
            {
                containerBuilder = new ContainerBuilder();
            }

            foreach (var module in modules)
            {
                containerBuilder.RegisterModule(module);
            }

            return containerBuilder;
        }
    }
}
