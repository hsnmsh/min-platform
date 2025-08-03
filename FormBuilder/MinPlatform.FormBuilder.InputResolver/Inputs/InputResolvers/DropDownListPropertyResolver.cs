namespace MinPlatform.FormBuilder.Elements.Inputs.InputResolvers
{
    using MinPlatform.FormBuilder.DataProviders;
    using System.Threading.Tasks;
    using MinPlatform.Data.Service;
    using MinPlatform.EngineRule.Service;
    using MinPlatform.FormBuilder.Context;
    using MinPlatform.FormBuilder.Elements.Inputs.Models;
    using MinPlatform.FormBuilder.Elements.Inputs.InputTypes;

    internal sealed class DropDownListPropertyResolver : BaseInputPropertyResolver<DropDownListPropertyInput, DropDownListInputType>
    {
        public DropDownListPropertyResolver(DropDownListPropertyInput propertyInputType,
            IFormContext<string> formContext,
            IDataProviderFactory dataProviderFactory,
            EngineRuleManager engineRuleManager,
            DataManager dataManager) : base(propertyInputType, formContext, dataProviderFactory, engineRuleManager, dataManager)
        {
        }

        public async override Task<DropDownListInputType> ResolveInputAsync()
        {
            await SetCommonPropertiesAsync();

            return new DropDownListInputType
            {
                InputType = InputType.DropDownList.ToString().ToLower(),
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
                AllowTextSearch = !string.IsNullOrEmpty(PropertyInputType.FilterTextUrl) ? true : PropertyInputType.AllowTextSearch,
                DefaultOptions = PropertyInputType.DefaultOptions,
                FilterTextUrl = PropertyInputType.FilterTextUrl,
                IsMultiValues = PropertyInputType.IsMultiValues,
                Options = PropertyInputType.Options,
                SelectedOptions = PropertyInputType.SelectedOptions,
                showCheckBox = PropertyInputType.ShowCheckBox
            };
        }
    }
}
