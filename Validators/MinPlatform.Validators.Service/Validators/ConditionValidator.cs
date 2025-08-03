namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class ConditionValidator<TProperty> : IValidator<TProperty>
    {
        private readonly Predicate<TProperty> condition;

        public ConditionValidator(Predicate<TProperty> condition)
        {
            this.condition= condition ?? throw new ArgumentNullException(nameof(condition));
        }

        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, TProperty value)
        {
            var booleanDefinition = typeDefinition as BooleanDefinition<TProperty>;

            if (booleanDefinition is null)
            {
                throw new ValidatorException("Invalid definition for boolean type");
            }

            var errorList = new List<string>();

            if (!condition.Invoke(value))
            {
                errorList.Add(string.Format("Condition of {0} didn't match", booleanDefinition.Key));
            }

            return new ValidationCheckResult
            {
                ErrorDescription = errorList,
                IsValid = !errorList.Any()
            };
        }
    }
}
