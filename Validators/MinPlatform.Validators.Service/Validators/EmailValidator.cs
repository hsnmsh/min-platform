namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal sealed class EmailValidator : IValidator<string>
    {
        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, string value)
        {
            var emaildefinition = typeDefinition as EmailDefinition;

            if (emaildefinition is null)
            {
                throw new ValidatorException("invalid definition for Email validator");
            }

            Regex regExValidator = CreateRegEx(emaildefinition.emailRegEx);

            if (!regExValidator.IsMatch(value))
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string>() { emaildefinition.GetErrorMessage(ErrorMessageType.EmailMessage) }
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
