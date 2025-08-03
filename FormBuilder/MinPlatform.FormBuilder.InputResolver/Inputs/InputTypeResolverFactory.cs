namespace MinPlatform.FormBuilder.Elements.Inputs
{
    using MinPlatform.Data.Service;
    using MinPlatform.EngineRule.Service;
    using MinPlatform.FormBuilder.Context;
    using MinPlatform.FormBuilder.DataProviders;
    using MinPlatform.FormBuilder.Elements.Inputs.InputTypes;
    using MinPlatform.FormBuilder.Elements.Inputs.Models;
    using MinPlatform.FormBuilder.Elements.Inputs.InputResolvers;
    using System;
    using System.Threading.Tasks;

    public sealed class InputTypeResolverFactory : IInputTypeResolverFactory
    {
        private readonly IFormContext<string> formContext;
        private readonly IDataProviderFactory dataProviderFactory;
        private readonly EngineRuleManager engineRuleManager;
        private readonly DataManager dataManager;

        public InputTypeResolverFactory(
            IFormContext<string> formContext,
            IDataProviderFactory dataProviderFactory,
            EngineRuleManager engineRuleManager,
            DataManager dataManager)
        {
            this.formContext = formContext;
            this.dataProviderFactory = dataProviderFactory;
            this.engineRuleManager = engineRuleManager;
            this.dataManager = dataManager;
        }

        public async Task<BaseInputType> CreateInputTypeAsync(BasePropertyInput propertyInputType)
        {
            switch (propertyInputType.Type)
            {
                case InputType.Text:
                    var textPropertyInput = propertyInputType as TextPropertyInput;
                    var textResolver = new TextInputPropertyResolver(textPropertyInput,
                                         formContext,
                                         dataProviderFactory,
                                         engineRuleManager,
                                         dataManager);

                    return await textResolver.ResolveInputAsync();

                case InputType.Number:
                    var numberPropertyInput = propertyInputType as NumberPropertyInput;
                    var numberResolver = new NumberInputPropertyResolver(numberPropertyInput,
                                         formContext,
                                         dataProviderFactory,
                                         engineRuleManager,
                                         dataManager);

                    return await numberResolver.ResolveInputAsync();

                case InputType.DateTime:
                    var dateTimePropertyInput = propertyInputType as DateTimePropertyInput;
                    var dateTimeResolver = new DateTimeInputPropertyResolver(dateTimePropertyInput,
                                         formContext,
                                         dataProviderFactory,
                                         engineRuleManager,
                                         dataManager);

                    return await dateTimeResolver.ResolveInputAsync();

                case InputType.Date:
                    var datePropertyInput = propertyInputType as DatePropertyInput;
                    var dateResolver = new DateInputPropertyResolver(datePropertyInput,
                                         formContext,
                                         dataProviderFactory,
                                         engineRuleManager,
                                         dataManager);

                    return await dateResolver.ResolveInputAsync();

                case InputType.Email:
                    var emailPropertyInput = propertyInputType as EmailPropertyInput;
                    var emailResolver = new EmailInputPropertyResolver(emailPropertyInput,
                                         formContext,
                                         dataProviderFactory,
                                         engineRuleManager,
                                         dataManager);

                    return await emailResolver.ResolveInputAsync();

                case InputType.Telephone:
                    var telephonePropertyInput = propertyInputType as TelephonePropertyInput;
                    var telephoneResolver = new TelephoneInputPropertyResolver(telephonePropertyInput,
                                         formContext,
                                         dataProviderFactory,
                                         engineRuleManager,
                                         dataManager);

                    return await telephoneResolver.ResolveInputAsync();

                case InputType.TextArea:
                    var textAreaPropertyInput = propertyInputType as TextAreaPropertyInput;
                    var textAreaResolver = new TextAreaInputPropertyResolver(textAreaPropertyInput,
                                         formContext,
                                         dataProviderFactory,
                                         engineRuleManager,
                                         dataManager);

                    return await textAreaResolver.ResolveInputAsync();

                case InputType.Slider:
                    var sliderPropertyInput = propertyInputType as SliderPropertyInput;
                    var sliderResolver = new SliderInputPropertyResolver(sliderPropertyInput,
                                         formContext,
                                         dataProviderFactory,
                                         engineRuleManager,
                                         dataManager);

                    return await sliderResolver.ResolveInputAsync();

                case InputType.Toggle:
                    var togglePropertyInput = propertyInputType as TogglePropertyInput;
                    var toggleResolver = new ToggleInputPropertyResolver(togglePropertyInput,
                                         formContext,
                                         dataProviderFactory,
                                         engineRuleManager,
                                         dataManager);

                    return await toggleResolver.ResolveInputAsync();

                case InputType.CheckBox:
                    var checkBoxPropertyInput = propertyInputType as CheckBoxPropertyInput;
                    var checkBoxResolver = new CheckBoxInputPropertyResolver(checkBoxPropertyInput,
                                         formContext,
                                         dataProviderFactory,
                                         engineRuleManager,
                                         dataManager);

                    return await checkBoxResolver.ResolveInputAsync();
                case InputType.Radio:
                    var radioPropertyInput = propertyInputType as RadioPropertyInput;
                    var radioResolver = new RadioInputPropertyResolver(radioPropertyInput,
                                         formContext,
                                         dataProviderFactory,
                                         engineRuleManager,
                                         dataManager);

                    return await radioResolver.ResolveInputAsync();

                case InputType.DropDownList:
                    var dropDownListPropertyInput = propertyInputType as DropDownListPropertyInput;
                    var dropDownListResolver = new DropDownListPropertyResolver(dropDownListPropertyInput,
                                         formContext,
                                         dataProviderFactory,
                                         engineRuleManager,
                                         dataManager);

                    return await dropDownListResolver.ResolveInputAsync();

                case InputType.File:
                    var fileUploaderPropertyInput = propertyInputType as FileUploaderPropertyInput;
                    var fileUploaderResolver = new FileUploaderPropertyResolver(fileUploaderPropertyInput,
                                         formContext,
                                         dataProviderFactory,
                                         engineRuleManager,
                                         dataManager);

                    return await fileUploaderResolver.ResolveInputAsync();

                case InputType.ListView:
                    var listViewPropertyInput = propertyInputType as ListViewPropertyInput;
                    var listViewResolver = new ListViewInputPropertyResolver(listViewPropertyInput,
                                         formContext,
                                         dataProviderFactory,
                                         engineRuleManager,
                                         dataManager);

                    return await listViewResolver.ResolveInputAsync();
                default:
                    throw new InvalidOperationException("Inavlid input type");
            }
        }
    }
}
