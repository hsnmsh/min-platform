namespace MinPlatform.Data.Service.Lookup
{
    using MinPlatform.Data.Abstractions.Models;
    using System.Threading.Tasks;

    public interface ILookupInfoDataService
    {
        Task<LookupInfoEntity> GetLookupInfoAsync(string tableName);
    }
}
