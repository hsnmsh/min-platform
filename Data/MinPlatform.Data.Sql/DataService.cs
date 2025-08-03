namespace MinPlatform.Data.Sql
{
    using Dapper;
    using MinPlatform.Data.Service;
    using MinPlatform.Data.Service.Models;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using static Dapper.SqlMapper;

    public sealed class DataService : IDataService
    {
        private readonly string connectionString;

        public DataService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<IEnumerable<IDictionary<string, object>>> GetAllAsync(string selectQuery)
        {
            IDbConnection dbConnection = GetSqlConnection();

            using (dbConnection)
            {
                dbConnection.Open();

                var listOfRows = dbConnection.Query(selectQuery);

                return await Task.Run(() =>
                {
                    return listOfRows.Cast<IDictionary<string, object>>();
                });

            }

        }


        public async Task<PagedItems<IDictionary<string, object>>> GetPagedItemsAsync(PageInfo pageInfo, string selectQuery)
        {
            IDbConnection dbConnection = GetSqlConnection();

            using (dbConnection)
            {
                dbConnection.Open();

                var listOfRows = dbConnection.Query(selectQuery).Cast<IDictionary<string, object>>();

                return await Task.Run(() =>
                {
                    return new PagedItems<IDictionary<string, object>>
                    {
                        Items = listOfRows,
                        PageSize = pageInfo.PageSize,
                        PageIndex = pageInfo.PageNumber,
                        TotalCount = listOfRows.First().TryGetValue("totalCount", out object totalCount) ? (int?)totalCount : null
                    };
                });


            }
        }

        public async Task<IDictionary<string, object>> GetEntityByIdAsync(string selectQuery, IDictionary<string, object> data)
        {
            IDbConnection dbConnection = GetSqlConnection();

            using (dbConnection)
            {
                dbConnection.Open();

                var entityInfo = await dbConnection.QueryFirstAsync(selectQuery, data);

                var dictionary = new Dictionary<string, object>();

                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(entityInfo))
                {
                    if (!dictionary.ContainsKey(propertyDescriptor.Name))
                    {
                        object obj = propertyDescriptor.GetValue(entityInfo);
                        dictionary.Add(propertyDescriptor.Name, obj);
                    }

                }

                return dictionary;

            }
        }

        public async Task<IEnumerable<IDictionary<string, object>>> SearchEntitiesAsync(string selectQuery, IDictionary<string, object> data)
        {
            IDbConnection dbConnection = GetSqlConnection();

            using (dbConnection)
            {
                dbConnection.Open();

                var listOfRows = dbConnection.Query(selectQuery, data);

                return await Task.Run(() =>
                {
                    return listOfRows.Cast<IDictionary<string, object>>();
                });

            }
        }

        public async Task<(IEnumerable<IDictionary<string, object>>, int?)> SearchPagedEntitiesAsync(string selectQuery, IDictionary<string, object> data, bool returnTotalCount)
        {
            IDbConnection dbConnection = GetSqlConnection();

            using (dbConnection)
            {
                dbConnection.Open();

                if (returnTotalCount)
                {
                    var pagedItems = await dbConnection.QueryMultipleAsync(selectQuery, data);

                    return await Task.Run(() =>
                    {
                        return (pagedItems.Read<dynamic>().Cast<IDictionary<string, object>>(), pagedItems.ReadSingleOrDefault<int>());
                    });
                }

                var listOfRows = dbConnection.Query(selectQuery, data);

                return await Task.Run(() =>
                {
                    return (listOfRows.Cast<IDictionary<string, object>>(), (int?)null);
                });

            }
        }

        public async Task<OperationResult<string>> CreateEntityAsync<IdType>(string insertQuery, IDictionary<string, object> data)
        {
            try
            {
                IDbConnection dbConnection = GetSqlConnection();

                using (dbConnection)
                {
                    dbConnection.Open();

                    var insertedId = await dbConnection.QuerySingleAsync<IdType>(insertQuery, data);

                    if (insertedId == null)
                    {
                        return new OperationResult<string>
                        {
                            Errors = new List<string>() { "DataBase Error Ocurred" }
                        };
                    }

                    return new OperationResult<string>
                    {
                        Id = insertedId.ToString().ToLower(),
                        Success = true
                    };

                }
            }
            catch (SqlException ex)
            {
                return GetOperationOnException<string>(ex);
            }

        }

        public async Task<OperationResult<string>> UpdateEntityAsync<IdType>(string updateQuery, IDictionary<string, object> data)
        {
            try
            {
                IDbConnection dbConnection = GetSqlConnection();

                using (dbConnection)
                {
                    dbConnection.Open();

                    var updatedId = await dbConnection.QuerySingleAsync<IdType>(updateQuery, data);

                    if (updatedId == null)
                    {
                        return new OperationResult<string>
                        {
                            Errors = new List<string>() { "DataBase Error Ocurred" }
                        };
                    }

                    return new OperationResult<string>
                    {
                        Id = updatedId.ToString().ToLower(),
                        Success = true
                    };

                }
            }
            catch (SqlException ex)
            {
                return GetOperationOnException<string>(ex);
            }
        }

        public async Task<OperationResult<string>> DeleteEntityAsync<IdType>(string deleteQuery, IDictionary<string, object> data)
        {
            try
            {
                IDbConnection dbConnection = GetSqlConnection();

                using (dbConnection)
                {
                    dbConnection.Open();

                    var deletedId = await dbConnection.QuerySingleAsync<IdType>(deleteQuery, data);

                    if (deletedId == null)
                    {
                        return new OperationResult<string>
                        {
                            Errors = new List<string>() { "DataBase Error Ocurred" }
                        };
                    }

                    return new OperationResult<string>
                    {
                        Id = deletedId.ToString().ToLower(),
                        Success = true
                    };

                }
            }
            catch (SqlException ex)
            {
                return GetOperationOnException<string>(ex);
            }
        }

        public async Task<OperationResult<int>> CreateEntitiesAsync(string insertQuery, IEnumerable<IDictionary<string, object>> recordsToInsert)
        {
            try
            {
                IDbConnection dbConnection = GetSqlConnection();

                using (dbConnection)
                {
                    dbConnection.Open();

                    var insertedId = await dbConnection.ExecuteAsync(insertQuery, recordsToInsert);

                    if (insertedId == 0 || recordsToInsert.Count() != insertedId)
                    {
                        return new OperationResult<int>
                        {
                            Errors = new List<string>() { "Not All Records Created" }
                        };
                    }

                    return new OperationResult<int>
                    {
                        RowsAffected = insertedId,
                        Success = true
                    };

                }
            }
            catch (SqlException ex)
            {
                return GetOperationOnException<int>(ex);
            }
        }

        public async Task<OperationResult<int>> UpdateEntitiesAsync(string updateQuery, IEnumerable<IDictionary<string, object>> recordsToUpdate)
        {
            try
            {
                IDbConnection dbConnection = GetSqlConnection();

                using (dbConnection)
                {
                    dbConnection.Open();
                    var updatedRecords = await dbConnection.ExecuteAsync(updateQuery, recordsToUpdate);

                    if (updatedRecords == 0 || recordsToUpdate.Count() != updatedRecords)
                    {
                        return new OperationResult<int>
                        {
                            Errors = new List<string>() { "Not All Records Updated" }
                        };
                    }

                    return new OperationResult<int>
                    {
                        RowsAffected = updatedRecords,
                        Success = true
                    };

                }
            }
            catch (SqlException ex)
            {
                return GetOperationOnException<int>(ex);
            }
        }

        public async Task<OperationResult<int>> DeleteEntitiesAsync<IdType>(string deleteQuery, IEnumerable<IdType> Ids)
        {
            try
            {
                IDbConnection dbConnection = GetSqlConnection();

                using (dbConnection)
                {
                    dbConnection.Open();

                    var deletedRecords = await dbConnection.ExecuteAsync(deleteQuery, new { id = Ids });

                    if (deletedRecords == 0 || Ids.Count() != deletedRecords)
                    {
                        return new OperationResult<int>
                        {
                            Errors = new List<string>() { "Not All Records Deleted" }
                        };
                    }

                    return new OperationResult<int>
                    {
                        RowsAffected = deletedRecords,
                        Success = true
                    };

                }
            }
            catch (SqlException ex)
            {
                return GetOperationOnException<int>(ex);
            }
        }

        public async Task<IDictionary<string, object>> ExecuteQueryAsync(StoredProcedureInfo storedProcedureInfo)
        {
            var spResults = new Dictionary<string, object>();
            object result = null;

            try
            {
                IDbConnection dbConnection = GetSqlConnection();

                using (dbConnection)
                {
                    dbConnection.Open();
                    var parameters = new DynamicParameters();

                    if (storedProcedureInfo.Parameters != null)
                    {
                        foreach (var parameter in storedProcedureInfo.Parameters)
                        {
                            parameters.Add("@" + parameter.Key,
                                parameter.Value.Value,
                                dbType: parameter.Value.DbType,
                                direction: parameter.Value.ParameterDirection,
                                size: parameter.Value.Size != null ? parameter.Value.Size : int.MaxValue);
                        }
                    }

                    switch (storedProcedureInfo.ReturnType)
                    {
                        case ReturnType.ScalarValue:
                            result = await dbConnection.ExecuteAsync(storedProcedureInfo.SPName, parameters, commandType: CommandType.StoredProcedure);
                            break;
                        case ReturnType.None:
                            dbConnection.Execute(storedProcedureInfo.SPName, parameters, commandType: CommandType.StoredProcedure);
                            break;
                        case ReturnType.SingleResult:
                            result = await dbConnection.QueryAsync(storedProcedureInfo.SPName, parameters, commandType: CommandType.StoredProcedure);
                            break;
                        case ReturnType.MultipleResult:
                            result = await dbConnection.QueryMultipleAsync(storedProcedureInfo.SPName, parameters, commandType: CommandType.StoredProcedure);
                            var reader = result as GridReader;
                            var dataSet = new DataSet();

                            int resultSetIndex = 1;

                            while (!reader.IsConsumed)
                            {
                                // Read the result set dynamically
                                var table = reader.Read<dynamic>().ToList();

                                // Add the result set to the DataSet
                                dataSet.Tables.Add(ConvertToDataTable(table, $"Table{resultSetIndex}"));

                                resultSetIndex++;
                            }

                            spResults.Add("returnedResult", dataSet);

                            break;

                    }

                    if (storedProcedureInfo.ReturnType != ReturnType.MultipleResult)
                    {
                        spResults.Add("returnedResult", result);
                    }


                    if (storedProcedureInfo.Parameters != null)
                    {
                        foreach (var parameter in storedProcedureInfo.Parameters)
                        {
                            if (parameter.Value.ParameterDirection == ParameterDirection.Output || parameter.Value.ParameterDirection == ParameterDirection.ReturnValue)
                            {
                                spResults.Add(parameter.Key, parameters.Get<object>("@" + parameter.Key));
                            }
                        }
                    }
                }

                return spResults;
            }
            catch
            {
                throw;
            }
        }


        private IDbConnection GetSqlConnection()
        {
            return new SqlConnection(connectionString);
        }

        private OperationResult<T> GetOperationOnException<T>(SqlException ex)
        {
            var operationResult = new OperationResult<T>
            {
                Errors = new List<string>()
            };

            foreach (SqlError error in ex.Errors)
            {
                operationResult.Errors.Add(error.Message);
            }

            return operationResult;
        }

        private static DataTable ConvertToDataTable(IList<dynamic> data, string tableName)
        {
            var dataTable = new DataTable(tableName);

            if (data.Count > 0)
            {
                // Assume all dynamic objects have the same properties
                var properties = ((IDictionary<string, object>)data[0]).Keys;

                foreach (var property in properties)
                {
                    dataTable.Columns.Add(property, typeof(object));
                }

                foreach (var item in data)
                {
                    var row = dataTable.NewRow();

                    foreach (var property in properties)
                    {
                        row[property] = ((IDictionary<string, object>)item)[property];
                    }

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }


    }
}
