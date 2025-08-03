namespace MinPlatform.Data.Service
{
    using MinPlatform.Data.Service.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDataService
    {
        Task<IEnumerable<IDictionary<string, object>>> GetAllAsync(string selectQuery);
        Task<PagedItems<IDictionary<string, object>>> GetPagedItemsAsync(PageInfo pageInfo, string selectQuery);
        Task<IDictionary<string, object>> GetEntityByIdAsync(string selectQuery, IDictionary<string, object> data);
        Task<IEnumerable<IDictionary<string, object>>> SearchEntitiesAsync(string selectQuery, IDictionary<string, object> data);
        Task<(IEnumerable<IDictionary<string, object>>, int?)> SearchPagedEntitiesAsync(string selectQuery, IDictionary<string, object> data, bool returnTotalCount);
        Task<OperationResult<string>> CreateEntityAsync<IdType>(string insertQuery, IDictionary<string, object> data);
        Task<OperationResult<string>> UpdateEntityAsync<IdType>(string updateQuery, IDictionary<string, object> data);
        Task<OperationResult<string>> DeleteEntityAsync<IdType>(string deleteQuery, IDictionary<string, object> data);
        Task<OperationResult<int>> CreateEntitiesAsync(string insertQuery, IEnumerable<IDictionary<string, object>> recordsToInsert);
        Task<OperationResult<int>> UpdateEntitiesAsync(string updateQuery, IEnumerable<IDictionary<string, object>> recordsToUpdate);
        Task<OperationResult<int>> DeleteEntitiesAsync<IdType>(string deleteQuery, IEnumerable<IdType> Ids);
        Task<IDictionary<string, object>> ExecuteQueryAsync(StoredProcedureInfo storedProcedureInfo);


    }
}
