namespace MinPlatform.Validators.Service
{
    using MinPlatform.Validators.Service.Models;
    using MinPlatform.Validators.Service.ValidationExecutor;
    using System;
    using System.Collections.Generic;

    public sealed class ValidatorManager
    {
        public IList<ValidationResult> ExecuteValidation<TModel>(AbstractValidatorExecutor<TModel> validatorExecutor)
        {
            if (validatorExecutor is null)
            {
                throw new ArgumentNullException(nameof(validatorExecutor));
            }

            return validatorExecutor.Validate();
        }

        public IDictionary<string, IList<ValidationResult>> ExecuteValidations<TModel>(IEnumerable<AbstractValidatorExecutor<TModel>> validatorExecutors)
        {
            if (validatorExecutors is null)
            {
                throw new ArgumentNullException(nameof(validatorExecutors));
            }

            var validationResults = new Dictionary<string, IList<ValidationResult>>();

            foreach (var validator in validatorExecutors)
            {
                IList<ValidationResult> validationResult = validator.Validate();
                validationResults.Add(validator.GetType().Name, validationResult);

            }

            return validationResults;
        }
    }
}
