namespace MinPlatform.EngineRule.EvaluationEngine.Extensions
{
    using Autofac;
    using MinPlatform.EngineRule.Service;
    using System;

    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder AddEvaluationEngine(this ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterType<EngineRuleService>().As<IEngineRuleService>().InstancePerDependency();

            return builder;

        }
    }
}
