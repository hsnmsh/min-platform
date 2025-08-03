namespace MinPlatform.EngineRule.Service.Extensions
{
    using Autofac;
    using System;

    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder AddEvaluationService(this ContainerBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.RegisterType<EngineRuleManager>().InstancePerDependency();

            return builder;

        }
    }
}
