namespace MinPlatform.Tenant.Service
{
    using MinPlatform.Tenant.Service.Models;
    using MinPlatform.Tenant.Service.TenantResolver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public sealed class Helpers
    {
        public static Type GetTypeFromTenantConfig(IEnumerable<TenantConfig> tenantConfigs)
        {
            var tenantResolverConfig =
              tenantConfigs
              .Where(tenantConfig => tenantConfig.Name == Constants.ResolverConfig)
              .FirstOrDefault();

            if (tenantResolverConfig != null &&
                tenantResolverConfig.Properties.TryGetValue(Constants.ResolverName, out object serviceObject) &&
                tenantResolverConfig.Properties.TryGetValue(Constants.AssemblyName, out object assemblyObject))
            {
                string serviceName = serviceObject.ToString();
                string assemblyName = assemblyObject.ToString();

                Type type = Assembly.Load(assemblyName).GetTypes().First(t => t.Name == serviceName);

                if (Array
                    .Exists(type.GetInterfaces(), i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITenantServicesResolver<>)))
                {
                    return type;

                }

            }

            return null;
        }
    }
}
