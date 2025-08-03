namespace MinPlatform.EngineRule.Service.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEvaluationService(this IServiceCollection service)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddTransient<EngineRuleManager>();

            return service;

        }
    }
}
