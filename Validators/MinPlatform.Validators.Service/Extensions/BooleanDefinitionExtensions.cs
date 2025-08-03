namespace MinPlatform.Validators.Service.Extensions
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Validators;
    using System;

    public static class BooleanDefinitionExtensions
    {
        public static BooleanDefinition<TProperty> Condition<TProperty>(this BooleanDefinition<TProperty> booleanDefinition, Predicate<TProperty> condition)
        {
            booleanDefinition.SetValidator(new ConditionValidator<TProperty>(condition));

            return booleanDefinition;
        }
    }
}
