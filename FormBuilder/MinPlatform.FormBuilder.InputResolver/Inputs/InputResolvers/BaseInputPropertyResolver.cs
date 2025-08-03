namespace MinPlatform.FormBuilder.Elements.Inputs.InputResolvers
{
    using MinPlatform.Data.Service;
    using MinPlatform.EngineRule.Service;
    using MinPlatform.FormBuilder.Context;
    using MinPlatform.FormBuilder.DataProviders;
    using MinPlatform.FormBuilder.Elements.Inputs.InputTypes;
    using MinPlatform.FormBuilder.Elements.Inputs.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class BaseInputPropertyResolver<BaseInputPropertyType, OutBaseInputType>
        where BaseInputPropertyType : BasePropertyInput
        where OutBaseInputType : BaseInputType
    {
        protected readonly BaseInputPropertyType PropertyInputType;
        protected readonly IFormContext<string> FormContext;
        protected readonly EngineRuleManager EngineRuleManager;
        protected readonly IDataProviderFactory DataProviderFactory;
        protected readonly DataManager DataManager;

        //common properties
        protected bool Readonly;
        protected bool Disabled;
        protected bool Visibility;
        protected bool Required;
        protected object Value;


        protected BaseInputPropertyResolver(BaseInputPropertyType propertyInputType,
            IFormContext<string> formContext,
            IDataProviderFactory dataProviderFactory,
            EngineRuleManager engineRuleManager,
            DataManager dataManager)

        {
            DataProviderFactory = dataProviderFactory;
            DataManager = dataManager;
            PropertyInputType = propertyInputType;
            EngineRuleManager = engineRuleManager;
            FormContext = formContext;
        }

        protected async virtual Task SetCommonPropertiesAsync()
        {
            IDictionary<string, object> inputParameters = FormContext.Properties;

            Readonly = string.IsNullOrEmpty(PropertyInputType.Readonly) ? false : await ResolveBooleanValueAsync(PropertyInputType.Readonly, inputParameters);
            Disabled = string.IsNullOrEmpty(PropertyInputType.Disabled) ? false : await ResolveBooleanValueAsync(PropertyInputType.Disabled, inputParameters);
            Visibility = string.IsNullOrEmpty(PropertyInputType.Visibility) ? true : await ResolveBooleanValueAsync(PropertyInputType.Visibility, inputParameters);
            Required = string.IsNullOrEmpty(PropertyInputType.Required) ? false : await ResolveBooleanValueAsync(PropertyInputType.Required, inputParameters);

            if (!string.IsNullOrEmpty(PropertyInputType.DataProviderName))
            {
                var dataProvider = DataProviderFactory.GetDataProviders(PropertyInputType.DataProviderName);

                if (dataProvider != null)
                {
                    Value = await dataProvider.GetDataAsync(DataManager, FormContext);
                }
            }

        }

        public abstract Task<OutBaseInputType> ResolveInputAsync();

        private async Task<bool> ResolveBooleanValueAsync(string value, IDictionary<string, object> inputParameters)
        {
            if (bool.TryParse(value, out bool convertedValue))
            {
                return convertedValue;
            }

            return await EngineRuleManager.EvaluateAsync(value, inputParameters);
        }

    }
}
