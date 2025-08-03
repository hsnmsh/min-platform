namespace MinPlatform.Validators.Service.Definitions
{
    using MinPlatform.Validators.Service.Models;
    using MinPlatform.Validators.Service.Validators;
    using System.Collections.Generic;

    public sealed class BooleanDefinition<T> : BaseDefinition
    {
        internal override string DefinitionName => "condition";

        internal override ValidationCheckResult CheckValidatorsPerDefinition(object value)
        {
            var validationCheckResult = new ValidationCheckResult()
            {
                ErrorDescription = new List<string>()
            };

            foreach (IValidator<T> validator in Validators)
            {
                var validatonResult = validator.IsValid(this, (T)value);

                if (validatonResult.ErrorDescription != null && validatonResult.ErrorDescription.Count > 0)
                {
                    validationCheckResult.ErrorDescription.AddRange(validatonResult.ErrorDescription);
                }

            }

            return validationCheckResult;
        }
    }
}
