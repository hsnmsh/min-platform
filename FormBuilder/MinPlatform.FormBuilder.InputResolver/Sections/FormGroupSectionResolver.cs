namespace MinPlatform.FormBuilder.Elements.Sections
{
    using MinPlatform.Data.Service;
    using MinPlatform.EngineRule.Service;
    using MinPlatform.FormBuilder.Context;
    using MinPlatform.FormBuilder.DataProviders;
    using MinPlatform.FormBuilder.Elements.Extensions;
    using MinPlatform.FormBuilder.Elements.Inputs;
    using MinPlatform.FormBuilder.Elements.Inputs.InputTypes;
    using MinPlatform.Tenant.Service.TenantResolver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public sealed class FormGroupSectionResolver : IFormGroupSectionResolver
    {
        public async Task<FormGroupSection> ResolveSectionAsync(FormGroupPropertySection propertySection,
            IFormContext<string> formContext,
            IDataProviderFactory dataProviderFactory,
            EngineRuleManager engineRuleManager,
            DataManager dataManager)
        {
            var inputResolverFactory = new InputTypeResolverFactory(formContext, dataProviderFactory, engineRuleManager, dataManager);
            var inputs = new List<BaseInputType>();

            if (!string.IsNullOrEmpty(propertySection.DataProviderName))
            {
                string[] providerInputs = propertySection.DataProviderName.Split(',');
                string serviceName = providerInputs[0].ToString();
                string assemblyName = providerInputs[1].ToString();

                Type type = Assembly.Load(assemblyName).GetTypes().First(t => t.Name == serviceName);

                if (typeof(IFormSectionDataProvider).IsAssignableFrom(type))
                {
                    IFormSectionDataProvider dataProvider = (IFormSectionDataProvider)Activator.CreateInstance(type, null);
                    var output = await dataProvider.GetFormSectionDataAsync(dataManager, formContext, dataProviderFactory);

                    formContext.Properties.Merge(output);
                }
                else
                {
                    throw new InvalidOperationException("Invalid Data provider");
                }
            }


            foreach (var input in propertySection.GroupInputs)
            {
                var inputInfo = await inputResolverFactory.CreateInputTypeAsync(input);

                inputs.Add(inputInfo);

            }

            bool disabled = (string.IsNullOrEmpty(propertySection.Disabled)) ?
                false :
                (formContext.Properties is null || !formContext.Properties.Any()) ?
                await ResolveBooleanValueAsync(engineRuleManager, propertySection.Disabled) :
                await ResolveBooleanValueAsync(engineRuleManager, propertySection.Disabled, formContext.Properties);

            if (disabled)
            {
                foreach (var input in inputs)
                {
                    input.Disabled = true;
                }
            }

            return new FormGroupSection
            {
                Id = propertySection.Id,
                Name = propertySection.Name,
                Title = propertySection.Title,
                Description = propertySection.Description,
                Visibility = string.IsNullOrEmpty(propertySection.Visibility) ?
                false :
                await ResolveBooleanValueAsync(engineRuleManager, propertySection.Visibility, formContext.Properties),
                ElementsPerRow = propertySection.ElementsPerRow,
                Buttons = await ResolveButtonsAsync(engineRuleManager, propertySection.Buttons, formContext.Properties),
                GroupInputs = inputs,
            };
        }

        private async Task<IEnumerable<ActionButtonGroup>> ResolveButtonsAsync(EngineRuleManager engineRuleManager, IEnumerable<ActionButton> buttonActions, IDictionary<string, object> inputParameters)
        {
            var buttons = new List<ActionButtonGroup>();

            foreach (var buttonAction in buttonActions)
            {
                buttons.Add(new ActionButtonGroup
                {
                    Id = buttonAction.Id,
                    ActionName = buttonAction.ActionName,
                    Description = buttonAction.Description,
                    IconUrl = buttonAction.IconUrl,
                    Name = buttonAction.Name,
                    Text = buttonAction.Text,
                    Styles = buttonAction.Styles,
                    Visibility = string.IsNullOrEmpty(buttonAction.Visibility) ?
                        false :
                     await ResolveBooleanValueAsync(engineRuleManager, buttonAction.Visibility, inputParameters),
                    Disabled = string.IsNullOrEmpty(buttonAction.Disabled) ?
                        false :
                     await ResolveBooleanValueAsync(engineRuleManager, buttonAction.Disabled, inputParameters),
                });
            }

            return buttons;
        }

        private async Task<bool> ResolveBooleanValueAsync(EngineRuleManager engineRuleManager, string value, IDictionary<string, object> inputParameters = null)
        {
            if (bool.TryParse(value, out bool convertedValue))
            {
                return convertedValue;
            }

            return (inputParameters != null) ?
                await engineRuleManager.EvaluateAsync(value, inputParameters) :
                await engineRuleManager.EvaluateAsync(value);
        }
    }
}
