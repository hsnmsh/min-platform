namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class CustomValidator<TProperty> : IValidator<TProperty>
    {
        private Func<TProperty, IEnumerable<string>> customValidator;

        public CustomValidator(Func<TProperty, IEnumerable<string>> customValidator)
        {
            this.customValidator = customValidator ?? throw new ArgumentNullException(nameof(customValidator));
        }

        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, TProperty value)
        {
            var customDefinition = typeDefinition as CustomDefinition<TProperty>;

            if (customDefinition is null)
            {
                throw new ValidatorException("Invalid definition for custom function type");
            }

            var errorKeys = customValidator(value);
            var errorMessages = new List<string>();

            if (customDefinition.FunctionErrors != null)
            {
                if (errorKeys.Any())
                {
                    foreach (var key in errorKeys)
                    {
                        errorMessages.Add(customDefinition.FunctionErrors[key]);
                    }
                }
            }

            return new ValidationCheckResult
            {
                ErrorDescription = errorMessages,
                IsValid = !errorMessages.Any()
            };
        }
    }
}
