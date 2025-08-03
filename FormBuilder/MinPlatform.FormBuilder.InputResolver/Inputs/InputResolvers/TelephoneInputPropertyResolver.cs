namespace MinPlatform.FormBuilder.Elements.Inputs.InputResolvers
{
    using MinPlatform.Data.Service;
    using MinPlatform.EngineRule.Service;
    using MinPlatform.FormBuilder.Context;
    using MinPlatform.FormBuilder.DataProviders;
    using MinPlatform.FormBuilder.Elements.Inputs.InputTypes;
    using MinPlatform.FormBuilder.Elements.Inputs.Models;
    using System.Threading.Tasks;

    internal sealed class TelephoneInputPropertyResolver : BaseInputPropertyResolver<TelephonePropertyInput, TelephoneInputType>
    {
        public TelephoneInputPropertyResolver(TelephonePropertyInput propertyInputType,
            IFormContext<string> formContext,
            IDataProviderFactory dataProviderFactory,
            EngineRuleManager engineRuleManager,
            DataManager dataManager) : base(propertyInputType, formContext, dataProviderFactory, engineRuleManager, dataManager)
        {
        }

        public async override Task<TelephoneInputType> ResolveInputAsync()
        {
            await SetCommonPropertiesAsync();

            return new TelephoneInputType
            {
                InputType = InputType.Telephone.ToString().ToLower(),
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
                Countries = PropertyInputType.Countries,
                DefaultMask = PropertyInputType.DefaultMask,
                DisableCountryGuess = PropertyInputType.DisableCountryGuess,
                DisableFormatting = PropertyInputType.DisableFormatting,
                Flags = PropertyInputType.Flags,
                HideDropdown = PropertyInputType.HideDropdown,
                PreferredCountries = PropertyInputType.PreferredCountries,
                Prefix = PropertyInputType.Prefix

            };
        }
    }
}
