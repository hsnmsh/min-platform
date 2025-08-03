namespace MinPlatform.Validators.Service.ValidationExecutor
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public abstract class AbstractValidatorExecutor<TModel>
    {
        private readonly TModel model;

        private readonly IList<BaseDefinition> validatorsDefinitions;
        public AbstractValidatorExecutor(TModel model)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            validatorsDefinitions = new List<BaseDefinition>();
        }

        protected virtual void PreValidations()
        {
            if (validatorsDefinitions.Any(validatorsDefinition => string.IsNullOrEmpty(validatorsDefinition.Key)))
            {
                throw new ValidatorException("All validators must contains a key");
            }
        }

        protected TDefinition CreateDefinition<TDefinition>(TDefinition validatorDefinition) where TDefinition : BaseDefinition
        {
            validatorsDefinitions.Add(validatorDefinition);

            return validatorDefinition;
        }

        internal IList<ValidationResult> Validate()
        {
            var result = new List<ValidationResult>();

            PreValidations();

            foreach (BaseDefinition validatorDefinition in validatorsDefinitions)
            {
                PropertyInfo propertyInfo = GetPropertyInfo(validatorDefinition.Key);

                var validationCheckResult = validatorDefinition.CheckValidatorsPerDefinition(propertyInfo.GetValue(model));

                var definitionResult = new ValidationResult
                {
                    Key = validatorDefinition.Key,
                    ErrorDescriptions = validationCheckResult.ErrorDescription,
                    IsValid = (validationCheckResult.ErrorDescription is null || !validationCheckResult.ErrorDescription.Any())
                };

                if (validatorDefinition.ThrowInValidException && !definitionResult.IsValid)
                {
                    throw new ValidatorException($"the Key {validatorDefinition.Key} has the following issues: {string.Join(Environment.NewLine, validationCheckResult.ErrorDescription)}");
                }

                result.Add(definitionResult);

            }

            return result;
        }

        private PropertyInfo GetPropertyInfo(string propertyName)
        {
            // Use reflection to get the value of the property, ignoring case
            var propertyInfo = typeof(TModel).GetProperty(propertyName, BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Instance);

            if (propertyInfo != null)
            {
                return propertyInfo;
            }

            throw new ValidatorException($"invalid property Info for {propertyName}");
        }


    }
}
