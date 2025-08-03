namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System;
    using System.Collections.Generic;

    internal sealed class MinValidator<TProperty> : IValidator<TProperty>
    {
        private TProperty minValue;

        public MinValidator(TProperty minValue)
        {
            this.minValue = minValue ?? throw new ArgumentNullException(nameof(minValue));
        }

        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, TProperty value)
        {
            if (!CommonValidators.IsMin(value, minValue))
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string> { typeDefinition.GetErrorMessage(ErrorMessageType.MinMessage) },
                };
            }

            return new ValidationCheckResult
            {
                IsValid = true
            };
        }
    }
}
