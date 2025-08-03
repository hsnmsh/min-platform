namespace MinPlatform.Validators.Service.Extensions
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Validators;
    using System;
    using System.Collections.Generic;

    public static class CustomDefinitionExtensions
    {
        public static CustomDefinition<TProperty> Function<TProperty>(this CustomDefinition<TProperty> customDefinition, Func<TProperty, IEnumerable<string>> customValidator)
        {
            customDefinition.SetValidator(new CustomValidator<TProperty>(customValidator));

            return customDefinition;
        }
    }
}
