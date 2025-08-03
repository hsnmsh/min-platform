using Autofac;
using System;
using System.Reflection;

namespace MinPlatform.DI.Autofac.DynamicLoader
{
    public sealed class RegistrationConventionModule : AbstractDynamicLoaderModule<string>
    {
        private readonly string pattern;
        private readonly Assembly assembly;

        public RegistrationConventionModule(string inputModule) : base(inputModule)
        {
            pattern = inputModule ?? throw new ArgumentNullException(nameof(inputModule));
        }

        public RegistrationConventionModule(string pattern, Assembly assembly) : this(pattern)
        {
            this.assembly = assembly is null ? Assembly.GetExecutingAssembly() : assembly;
        }

        public override void LoadModules(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(assembly)
               .Where(type => type.Name.EndsWith(pattern))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        }
    }
}
