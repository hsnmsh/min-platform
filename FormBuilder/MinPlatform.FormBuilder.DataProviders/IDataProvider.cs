namespace MinPlatform.FormBuilder.DataProviders
{
    using MinPlatform.Data.Service;
    using MinPlatform.FormBuilder.Context;
    using System.Threading.Tasks;


    public interface IDataProvider
    {
        string Name 
        { 
            get;
        }

        Task<object> GetDataAsync(DataManager dataManager, IFormContext<string> formContext);
    }

    public interface IDataProvider<OutputType>
    {
        string Name 
        { 
            get;
        }

        Task<OutputType> GetDataAsync(DataManager dataManager, IFormContext<string> formContext);
    }
}
