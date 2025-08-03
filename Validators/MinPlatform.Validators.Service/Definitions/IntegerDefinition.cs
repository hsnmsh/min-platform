namespace MinPlatform.Validators.Service.Definitions
{
    using MinPlatform.Validators.Service.Models;
    using MinPlatform.Validators.Service.Validators;
    using System.Collections.Generic;

    public sealed class IntegerDefinition : NumericDefinition
    {
        internal override string DefinitionName => "int";

        internal override ValidationCheckResult CheckValidatorsPerDefinition(object value)
        {
            var validationCheckResult = new ValidationCheckResult()
            {
                ErrorDescription = new List<string>()
            };

            foreach (IValidator<int> validator in Validators)
            {
                var validatonResult = validator.IsValid(this, (int)value);

                if (validatonResult.ErrorDescription != null && validatonResult.ErrorDescription.Count > 0)
                {
                    validationCheckResult.ErrorDescription.AddRange(validatonResult.ErrorDescription);
                }
            }

            return validationCheckResult;
        }
    }
}
