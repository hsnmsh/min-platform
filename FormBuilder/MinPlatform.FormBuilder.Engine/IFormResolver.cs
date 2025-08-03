namespace MinPlatform.FormBuilder.Engine
{
    using MinPlatform.Data.Service;
    using MinPlatform.EngineRule.Service;
    using MinPlatform.FormBuilder.Context;
    using MinPlatform.FormBuilder.DataProviders;
    using System.Threading.Tasks;

    public interface IFormResolver
    {
        Task<FormInfo> ResolveFromAsync(FormEntity form, IFormContext<string> formContext, IDataProviderFactory dataProviderFactory,
            EngineRuleManager engineRuleManager,
            DataManager dataManager);
    }
}
