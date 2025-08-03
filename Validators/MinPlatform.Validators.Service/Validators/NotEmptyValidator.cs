namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System.Collections.Generic;

    internal sealed class NotEmptyValidator : IValidator<string>
    {
        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, string value)
        {
            if (CommonValidators.NotEmpty(value))
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string>
                    {
                        typeDefinition.GetErrorMessage(ErrorMessageType.StringNotEmptyMessage)
                    },
                };
            }

            return new ValidationCheckResult
            {
                IsValid = true
            };
        }
    }
}
