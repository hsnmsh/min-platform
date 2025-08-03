namespace MinPlatform.Validators.Service.Definitions
{
    using MinPlatform.Validators.Service.Models;
    using MinPlatform.Validators.Service.Validators;
    using System.Collections.Generic;

    public class StringDefinition : BaseDefinition
    {
        internal override string DefinitionName => "string";

        public string RegexFormat
        {
            get;
            set;
        }

        internal override ValidationCheckResult CheckValidatorsPerDefinition(object value)
        {
            var validationCheckResult = new ValidationCheckResult()
            {
                ErrorDescription = new List<string>()
            };

            foreach (IValidator<string> validator in Validators)
            {
                var validatonResult = validator.IsValid(this, (string)value);

                if (validatonResult.ErrorDescription != null && validatonResult.ErrorDescription.Count > 0)
                {
                    validationCheckResult.ErrorDescription.AddRange(validatonResult.ErrorDescription);
                }
            }

            return validationCheckResult;
        }
    }
}
