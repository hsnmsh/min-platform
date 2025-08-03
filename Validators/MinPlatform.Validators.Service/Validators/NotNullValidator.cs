namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System.Collections.Generic;

    internal sealed class NotNullValidator<TProperty> : IValidator<TProperty>
    {
        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, TProperty value)
        {
            if (!CommonValidators.NotNull(value))
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string> { typeDefinition.GetErrorMessage(ErrorMessageType.NotNullMessage) },
                };
            }

            return new ValidationCheckResult
            {
                IsValid = true
            };
        }
    }
}
