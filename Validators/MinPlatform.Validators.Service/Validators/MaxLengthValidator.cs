namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System.Collections.Generic;

    internal sealed class MaxLengthValidator : IValidator<string>
    {
        private readonly int maxValue;

        public MaxLengthValidator(int maxValue)
        {
            this.maxValue = maxValue;
        }

        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, string value)
        {
            if (!CommonValidators.IsMax(value.Length, maxValue))
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string> { typeDefinition.GetErrorMessage(ErrorMessageType.StringMaxLengthMessage) },
                };
            }

            return new ValidationCheckResult
            {
                IsValid = true
            };
        }
    }
}
