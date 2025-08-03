namespace MinPlatform.FormBuilder.Elements.Inputs.InputResolvers
{
    using MinPlatform.Data.Service;
    using MinPlatform.EngineRule.Service;
    using MinPlatform.FormBuilder.Context;
    using MinPlatform.FormBuilder.DataProviders;
    using MinPlatform.FormBuilder.Elements.Inputs.InputTypes;
    using MinPlatform.FormBuilder.Elements.Inputs.Models;
    using System.Threading.Tasks;

    internal sealed class TextAreaInputPropertyResolver : BaseInputPropertyResolver<TextAreaPropertyInput, TextAreaInputType>
    {
        public TextAreaInputPropertyResolver(TextAreaPropertyInput propertyInputType,
            IFormContext<string> formContext,
            IDataProviderFactory dataProviderFactory,
            EngineRuleManager engineRuleManager,
            DataManager dataManager) : base(propertyInputType, formContext, dataProviderFactory, engineRuleManager, dataManager)
        {
        }

        public async override Task<TextAreaInputType> ResolveInputAsync()
        {
            await SetCommonPropertiesAsync();

            return new TextAreaInputType
            {
                InputType = InputType.TextArea.ToString().ToLower(),
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
                MaxTextSize = PropertyInputType.MaxTextSize,
            };
        }
    }
}
