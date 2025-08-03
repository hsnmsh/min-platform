namespace MinPlatform.FormBuilder.Elements.Inputs.InputResolvers
{
    using MinPlatform.Data.Service;
    using MinPlatform.EngineRule.Service;
    using MinPlatform.FormBuilder.Context;
    using MinPlatform.FormBuilder.DataProviders;
    using MinPlatform.FormBuilder.Elements.Inputs.InputTypes;
    using MinPlatform.FormBuilder.Elements.Inputs.Models;
    using System;
    using System.Threading.Tasks;

    internal class NumberInputPropertyResolver : BaseInputPropertyResolver<NumberPropertyInput, NumberInputType>
    {
        public NumberInputPropertyResolver(NumberPropertyInput propertyInputType,
            IFormContext<string> formContext,
            IDataProviderFactory dataProviderFactory,
            EngineRuleManager engineRuleManager,
            DataManager dataManager) :
            base(propertyInputType, formContext, dataProviderFactory, engineRuleManager, dataManager)
        {
        }
        public async override Task<NumberInputType> ResolveInputAsync()
        {
            await base.SetCommonPropertiesAsync();

            return new NumberInputType
            {
                InputType = InputType.Number.ToString().ToLower(),
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
                MaxValue = PropertyInputType.MaxValue,
                MinValue = PropertyInputType.MinValue
            };
        }
    }
}
