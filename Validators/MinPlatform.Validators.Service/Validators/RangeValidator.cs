namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System;
    using System.Collections.Generic;

    internal sealed class RangeValidator<TProperty> : IValidator<TProperty>
    {
        private TProperty start;
        private TProperty end;

        public RangeValidator(TProperty start, TProperty end)
        {
            this.start = start ?? throw new ArgumentNullException(nameof(start));
            this.end = end ?? throw new ArgumentNullException(nameof(end));
        }

        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, TProperty value)
        {
            if (!Includes(value))
            {
                return new ValidationCheckResult
                {
                    ErrorDescription = new List<string>() 
                    { 
                        typeDefinition.GetErrorMessage(ErrorMessageType.IncludeMessage)
                    }
                };
            }

            return new ValidationCheckResult
            {
                IsValid = true,
            };
        }

        private bool Includes(TProperty value)
        {
            return (Comparer<TProperty>.Default.Compare(value, start) >= 0 &&
                Comparer<TProperty>.Default.Compare(value, end) <= 0);
        }
    }
}
