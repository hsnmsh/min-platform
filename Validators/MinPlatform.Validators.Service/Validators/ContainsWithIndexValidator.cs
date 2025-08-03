namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System.Collections.Generic;

    internal sealed class ContainsWithIndexValidator : IValidator<string>
    {
        private readonly string stringPattern;
        private readonly int patternIndex;
        private readonly bool ignoreCase;

        public ContainsWithIndexValidator(string stringPattern, int patternIndex)
        {
            this.stringPattern = stringPattern;
            this.patternIndex = patternIndex;
        }

        public ContainsWithIndexValidator(string stringPattern, int patternIndex, bool ignoreCase) : 
            this(stringPattern, patternIndex)
        {
            this.ignoreCase = ignoreCase;
        }

        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, string value)
        {
            var stringDefinition = typeDefinition as StringDefinition;

            if (stringDefinition is null)
            {
                throw new ValidatorException("invalid definition for Contains with index validator");
            }

            if (!CommonValidators.ContainsWithIndexOf(value,
                stringPattern,
                patternIndex,
                ignoreCase))
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string> { typeDefinition.GetErrorMessage(ErrorMessageType.StringContainsWithIndexMessage) },
                };
            }

            return new ValidationCheckResult
            {
                IsValid = true
            };
        }
    }
}
