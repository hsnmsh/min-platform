namespace MinPlatform.FormBuilder.Elements.Inputs.InputResolvers
{
    using MinPlatform.Data.Service;
    using MinPlatform.EngineRule.Service;
    using MinPlatform.FormBuilder.Context;
    using MinPlatform.FormBuilder.DataProviders;
    using MinPlatform.FormBuilder.Elements.Inputs.InputTypes;
    using MinPlatform.FormBuilder.Elements.Inputs.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ActionButtonProperty = MinPlatform.FormBuilder.Elements.Inputs.Models.ActionButtonProperty;

    internal sealed class ListViewInputPropertyResolver : BaseInputPropertyResolver<ListViewPropertyInput, ListViewInputType>
    {
        public ListViewInputPropertyResolver(ListViewPropertyInput propertyInputType,
            IFormContext<string> formContext,
            IDataProviderFactory dataProviderFactory,
            EngineRuleManager engineRuleManager,
            DataManager dataManager) : base(propertyInputType, formContext, dataProviderFactory, engineRuleManager, dataManager)
        {
        }

        public async override Task<ListViewInputType> ResolveInputAsync()
        {
            await SetCommonPropertiesAsync();

            if (PropertyInputType.PropertyInputs.Any(input => input is ListViewPropertyInput))
            {
                throw new ErrorInputException("nested list views are not allowed");
            }

            var inputFactory = new InputTypeResolverFactory(FormContext, DataProviderFactory, EngineRuleManager, DataManager);

            var inputTypes = await Task.WhenAll(PropertyInputType.PropertyInputs
                    .Select(async input => await inputFactory.CreateInputTypeAsync(input)));

            return new ListViewInputType
            {
                InputType = InputType.ListView.ToString().ToLower(),
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
                ActionProperties = await EvaluateActionPropertiesAsync(PropertyInputType.ActionProperties),
                MaxRows = PropertyInputType.MaxRows,
                MinRows = PropertyInputType.MinRows,
                PropertyInputs = inputTypes
            };


        }

        private async Task<IDictionary<ActionButton, InputTypes.ActionButtonProperty>> EvaluateActionPropertiesAsync(IDictionary<ActionButton, ActionButtonProperty> actions)
        {
            var actionProperties = new Dictionary<ActionButton, InputTypes.ActionButtonProperty>();

            foreach (var actionProperty in actions)
            {
                actionProperties[actionProperty.Key] = new InputTypes.ActionButtonProperty
                {
                    Visible = bool.TryParse(actionProperty.Value.Visible, out bool convertedVisibleValue)
                    ? convertedVisibleValue
                    : await EngineRuleManager.EvaluateAsync(actionProperty.Value.Visible, FormContext.Properties),

                    Disabled = bool.TryParse(actionProperty.Value.Disabled, out bool convertedDisabledValue)
                    ? convertedDisabledValue
                    : await EngineRuleManager.EvaluateAsync(actionProperty.Value.Disabled, FormContext.Properties),

                };
            }

            return actionProperties;

        }
    }
}
