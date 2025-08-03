namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System.Collections.Generic;

    internal sealed class EmptyValidator : IValidator<string>
    {
        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, string value)
        {
            if (!CommonValidators.Empty(value))
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string>
                    {
                        typeDefinition.GetErrorMessage(ErrorMessageType.StringEmptyMessage)
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
