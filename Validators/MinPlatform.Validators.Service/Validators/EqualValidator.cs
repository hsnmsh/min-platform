namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System;
    using System.Collections.Generic;

    internal sealed class EqualValidator<TProperty> : IValidator<TProperty>
    {
        private readonly TProperty compareValue;

        public EqualValidator(TProperty compareValue)
        {
            this.compareValue = compareValue ?? throw new ArgumentNullException(nameof(compareValue));
        }
        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, TProperty value)
        {

            if (Comparer<TProperty>.Default.Compare(value, compareValue) != 0)
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string> { typeDefinition.GetErrorMessage(ErrorMessageType.EqualMessage) }
                };
            }

            return new ValidationCheckResult
            {
                IsValid = true
            };
        }
    }
}
