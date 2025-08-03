namespace MinPlatform.DI.ServiceCollection.Extensions
{
    using Decor;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Linq;
    using System.Reflection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection GetInstanceByEnvironment<ServiceType>(this IServiceCollection serviceCollection,
            Func<IServiceProvider, IHostEnvironment, ServiceType> resolveAction)
            where ServiceType : class
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddTransient(provider =>
            {
                var hostEnvironment = provider.GetService<IHostEnvironment>();

                return resolveAction(provider, hostEnvironment);
            });

            return serviceCollection;
        }

        public static IServiceCollection AddServicesByNamingConvention(this IServiceCollection serviceCollection, string implementationSuffix, Assembly assembly = null)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (assembly is null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }

            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith(implementationSuffix))
                .Select(t => new
                {
                    Interfaces = t.GetInterfaces(),
                    ImplementationType = t
                });

            foreach (var type in types)
            {
                foreach (Type item in type.Interfaces)
                {
                    serviceCollection.AddTransient(item, type.ImplementationType);

                }

            }

            return serviceCollection;
        }

        public static IServiceCollection RegisterCustomDecorator<ServiceType, DecoratorType>(this IServiceCollection serviceCollection) 
            where ServiceType : class
            where DecoratorType : class
        {
            serviceCollection
             .AddTransient<ServiceType>()
             .AddTransient<DecoratorType>()
             .Decorate<ServiceType>();

            return serviceCollection;
        }
    }
}
