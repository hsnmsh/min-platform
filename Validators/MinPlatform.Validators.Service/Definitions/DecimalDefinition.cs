namespace MinPlatform.Validators.Service.Definitions
{
    using MinPlatform.Validators.Service.Models;
    using MinPlatform.Validators.Service.Validators;
    using System.Collections.Generic;

    public sealed class DecimalDefinition : NumericDefinition
    {
        internal override string DefinitionName => "decimal";

        internal override ValidationCheckResult CheckValidatorsPerDefinition(object value)
        {
            var validationCheckResult = new ValidationCheckResult()
            {
                ErrorDescription = new List<string>()
            };

            foreach (IValidator<decimal> validator in Validators)
            {
                var validatonResult = validator.IsValid(this, (decimal)value);

                if (validatonResult.ErrorDescription != null && validatonResult.ErrorDescription.Count > 0)
                {
                    validationCheckResult.ErrorDescription.AddRange(validatonResult.ErrorDescription);
                }
            }

            return validationCheckResult;
        }
    }


}
