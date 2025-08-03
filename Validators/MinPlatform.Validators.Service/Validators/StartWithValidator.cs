namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System.Collections.Generic;

    public sealed class StartWithValidator : IValidator<string>
    {
        private readonly string stringPart;
        private readonly bool ignoreCase;


        public StartWithValidator(string stringPart)
        {
            this.stringPart = stringPart;
        }

        public StartWithValidator(string stringPart, bool ignoreCase) : this(stringPart)
        {
            this.ignoreCase = ignoreCase;
        }

        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, string value)
        {
            var stringDefinition = typeDefinition as StringDefinition;

            if (stringDefinition is null)
            {
                throw new ValidatorException("invalid definition for Start With validator");
            }

            if (!CommonValidators.StartWith(value, stringPart, ignoreCase))
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string> { typeDefinition.GetErrorMessage(ErrorMessageType.StringStartWithMessage) },
                };
            }

            return new ValidationCheckResult
            {
                IsValid = true
            };
        }
    }
}
