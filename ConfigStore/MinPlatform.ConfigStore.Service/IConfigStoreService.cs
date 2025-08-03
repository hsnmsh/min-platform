namespace MinPlatform.ConfigStore.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IConfigStoreService
    {
        Task<IDictionary<string, object>> GetConfigByIdAsync(int Id);

        Task<IDictionary<string, object>> GetConfigByNameAsync(string name);

    }
}
