namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System;
    using System.Collections.Generic;

    internal sealed class ContainsValidator : IValidator<string>
    {
        private readonly string stringPart;
        private readonly bool ignoreCase;


        public ContainsValidator(string stringPart)
        {
            this.stringPart = stringPart ?? throw new ArgumentNullException(nameof(stringPart));
        }

        public ContainsValidator(string stringPart, bool ignoreCase)
        {
            this.stringPart = stringPart ?? throw new ArgumentNullException(nameof(stringPart));
            this.ignoreCase = ignoreCase;

        }

        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, string value)
        {
            var stringDefinition = typeDefinition as StringDefinition;

            if (stringDefinition is null)
            {
                throw new ValidatorException("invalid definition for Contains validator");
            }

            if (!CommonValidators.Contains(value, stringPart, ignoreCase))
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string> { typeDefinition.GetErrorMessage(ErrorMessageType.StringEndWithMessage) },
                };
            }

            return new ValidationCheckResult
            {
                IsValid = true
            };
        }
    }
}
