namespace MinPlatform.Data.Service.Lookup
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILookupDataService
    {
        Task<IDictionary<string, IEnumerable<LookupEntity>>> GetLookupsAsync(string tableName, bool hasDependentColumn, string column, string columnValue = null);

        Task<IEnumerable<LookupEntity>> GetLookupsByLanguageCodeAsync(string tableName, bool hasDependentColumn, string column, string languageCode, string columnValue = null);

    }
}