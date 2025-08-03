namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System.Collections.Generic;

    internal sealed class LengthValidator : IValidator<string>
    {
        private readonly int length;

        public LengthValidator(int length)
        {
            this.length = length;
        }

        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, string value)
        {
            if (!CommonValidators.Length(value.Length, length))
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string> { typeDefinition.GetErrorMessage(ErrorMessageType.StringInvalidLengthMessage) },
                };
            }

            return new ValidationCheckResult
            {
                IsValid = true
            };
        }
    }
}
