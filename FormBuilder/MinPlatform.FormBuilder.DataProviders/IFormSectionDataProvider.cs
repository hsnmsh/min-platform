namespace MinPlatform.FormBuilder.DataProviders
{
    using MinPlatform.Data.Service;
    using MinPlatform.FormBuilder.Context;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFormSectionDataProvider
    {
        Task<IDictionary<string, object>> GetFormSectionDataAsync(DataManager dataManager, IFormContext<string> formContext, IDataProviderFactory dataProviderFactory);
    }
}
