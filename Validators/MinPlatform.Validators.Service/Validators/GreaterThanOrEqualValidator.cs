namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System;
    using System.Collections.Generic;

    internal sealed class GreaterThanOrEqualValidator<TProperty> : IValidator<TProperty>
    {
        private readonly TProperty compareValue;

        public GreaterThanOrEqualValidator(TProperty compareValue)
        {
            this.compareValue = compareValue ?? throw new ArgumentNullException(nameof(compareValue));
        }

        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, TProperty value)
        {
            var result = new ValidationCheckResult();

            if (TypeValidation.IsNumericType(typeof(TProperty)))
            {
                if (Comparer<TProperty>.Default.Compare(value, compareValue) < 0)
                {
                    result.ErrorDescription = new List<string>()
                    {
                        typeDefinition.GetErrorMessage(ErrorMessageType.GreaterThanOrEqualMessage)
                    };

                    return result;

                }

                result.IsValid = true;

                return result;

            }
            else if (typeof(TProperty) == typeof(DateTime))
            {
                DateTime targetDate = (DateTime)(object)value;
                DateTime comparedDate = (DateTime)(object)compareValue;

                if (DateTime.Compare(targetDate, comparedDate) < 0)
                {
                    result.ErrorDescription = new List<string>()
                    {
                        typeDefinition.GetErrorMessage(ErrorMessageType.GreaterThanOrEqualMessage)
                    };

                    return result;
                }

                result.IsValid = true;

                return result;

            }

            throw new ValidatorException($"Unsupported data type: {typeof(TProperty)}");
        }
    }
}
