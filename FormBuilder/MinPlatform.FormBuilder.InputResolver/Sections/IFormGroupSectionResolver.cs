namespace MinPlatform.FormBuilder.Elements.Sections
{
    using MinPlatform.Data.Service;
    using MinPlatform.EngineRule.Service;
    using MinPlatform.FormBuilder.Context;
    using MinPlatform.FormBuilder.DataProviders;
    using System.Threading.Tasks;

    internal interface IFormGroupSectionResolver
    {
        Task<FormGroupSection> ResolveSectionAsync(FormGroupPropertySection propertySection, IFormContext<string> formContext,
            IDataProviderFactory dataProviderFactory,
            EngineRuleManager engineRuleManager,
            DataManager dataManager);
    }
}
