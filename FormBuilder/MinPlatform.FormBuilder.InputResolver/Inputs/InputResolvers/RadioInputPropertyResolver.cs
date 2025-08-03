namespace MinPlatform.FormBuilder.Elements.Inputs.InputResolvers
{
    using MinPlatform.Data.Service;
    using MinPlatform.EngineRule.Service;
    using MinPlatform.FormBuilder.Context;
    using MinPlatform.FormBuilder.DataProviders;
    using MinPlatform.FormBuilder.Elements.Inputs.InputTypes;
    using MinPlatform.FormBuilder.Elements.Inputs.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal sealed class RadioInputPropertyResolver : BaseInputPropertyResolver<RadioPropertyInput, RadioInputType>
    {
        public RadioInputPropertyResolver(RadioPropertyInput propertyInputType,
            IFormContext<string> formContext,
            IDataProviderFactory dataProviderFactory,
            EngineRuleManager engineRuleManager,
            DataManager dataManager) : base(propertyInputType, formContext, dataProviderFactory, engineRuleManager, dataManager)
        {
        }

        public async override Task<RadioInputType> ResolveInputAsync()
        {
            await SetCommonPropertiesAsync();

            if (PropertyInputType.Options is null || !PropertyInputType.Options.Any())
            {
                throw new InvalidOperationException($"Options for {PropertyInputType.Name} radio must not be empty");
            }

            return new RadioInputType
            {
                InputType = InputType.CheckBox.ToString().ToLower(),
                Classes = PropertyInputType.Classes,
                Disabled = Disabled,
                ErrorMessage = PropertyInputType.ErrorMessage,
                Id = PropertyInputType.Id,
                Label = PropertyInputType.Label,
                Name = PropertyInputType.Name,
                PlaceHolder = PropertyInputType.PlaceHolder,
                Readonly = Readonly,
                Required = Required,
                Value = Value ?? PropertyInputType.Value,
                Variant = PropertyInputType.Variant.ToString(),
                Visibility = Visibility,
                Options = PropertyInputType.Options,
                DisabledOptions = PropertyInputType.DisabledOptions != null ? await EvaluateChoiceOptionsAsync(PropertyInputType.DisabledOptions) : null
            };
        }

        private async Task<IDictionary<string, bool>> EvaluateChoiceOptionsAsync(IDictionary<string, string> optionExpressions)
        {
            var disabledOptions = new Dictionary<string, bool>();

            foreach (var optionExpression in optionExpressions)
            {
                disabledOptions[optionExpression.Key] = string.IsNullOrEmpty(optionExpression.Value) ? false : bool.TryParse(optionExpression.Value, out bool convertedValue)
                    ? convertedValue
                    : await EngineRuleManager.EvaluateAsync(optionExpression.Value, FormContext.Properties);
            }

            return disabledOptions;
        }
    }
}
