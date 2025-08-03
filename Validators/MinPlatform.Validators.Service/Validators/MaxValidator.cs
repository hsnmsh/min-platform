namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System;
    using System.Collections.Generic;

    internal sealed class MaxValidator<TProperty> : IValidator<TProperty>
    {
        private TProperty maxValue;

        public MaxValidator(TProperty maxValue)
        {
            this.maxValue = maxValue ?? throw new ArgumentNullException(nameof(maxValue));
        }

        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, TProperty value)
        {
            if (!CommonValidators.IsMax(value, maxValue))
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string> { typeDefinition.GetErrorMessage(ErrorMessageType.MaxMessage) },
                };
            }

            return new ValidationCheckResult
            {
                IsValid = true
            };
        }
    }
}
