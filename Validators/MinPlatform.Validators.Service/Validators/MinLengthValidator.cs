namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System.Collections.Generic;


    internal sealed class MinLengthValidator : IValidator<string>
    {
        private int minValue;

        public MinLengthValidator(int minValue)
        {
            this.minValue = minValue;
        }

        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, string value)
        {
            if (!CommonValidators.IsMin(value.Length, minValue))
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string> { typeDefinition.GetErrorMessage(ErrorMessageType.StringMinLengthMessage) },
                };
            }

            return new ValidationCheckResult
            {
                IsValid = true
            };
        }
    }
}
