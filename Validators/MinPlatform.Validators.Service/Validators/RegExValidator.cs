namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class RegExValidator : IValidator<string>
    {
        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, string value)
        {
            var stringDefinition = typeDefinition as StringDefinition;

            if (stringDefinition is null)
            {
                throw new ValidatorException("invalid definition for RegEx validator");
            }

            Regex regExValidator = CreateRegEx(stringDefinition.RegexFormat);

            if (!regExValidator.IsMatch(value))
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string>() { stringDefinition.GetErrorMessage(ErrorMessageType.StringInvalidRegexMessage) }
                };
            }

            return new ValidationCheckResult 
            { 
                IsValid = true 
            };

        }

        private Regex CreateRegEx(string regexFormat)
        {
            const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;

            return new Regex(regexFormat, options, TimeSpan.FromSeconds(2.0));
        }
    }
}
