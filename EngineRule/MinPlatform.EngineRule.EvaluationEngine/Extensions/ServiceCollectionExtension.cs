namespace MinPlatform.EngineRule.EvaluationEngine.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.EngineRule.Service;
    using System;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEvaluationEngine(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IEngineRuleService, EngineRuleService>();

            return services;
        }
    }
}
