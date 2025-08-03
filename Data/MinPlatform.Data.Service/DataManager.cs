namespace MinPlatform.Data.Service
{
    using MinPlatform.Data.Abstractions.Models;
    using MinPlatform.Data.Service.Models;
    using MinPlatform.Data.Service.QueryBuilder;
    using MinPlatform.Data.Service.QueryBuilder.Factory;
    using MinPlatform.Data.Service.QueryBuilder.Validations;
    using MinPlatform.Data.Service.SqlStatmentBuilder;
    using MinPlatform.Data.Service.Validations;
    using MinPlatform.Validators.Service;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class DataManager
    {
        private readonly IDataService dataService;
        private readonly ISqlQueryStatmentBuilder sqlQueryStatmentBuilder;
        private readonly Lazy<SqlQueryVisitor> lazySqlQueryVisitor;
        private readonly Lazy<SqlStatmentBuilderCommands> lazySqlStatmentBuilderCommands;
        private readonly ValidatorManager validatorManager;


        public DataManager(
            IDataService dataService,
            ISqlStatmentBuilderFactory sqlStatmentBuilderFactory,
            ISqlQueryStatmentBuilder sqlQueryStatmentBuilder,
            ISqlQueryBuilderFactory sqlQueryBuilderFactory
            )
        {
            this.dataService = dataService;
            this.lazySqlStatmentBuilderCommands = new Lazy<SqlStatmentBuilderCommands>(sqlStatmentBuilderFactory.Create());
            this.sqlQueryStatmentBuilder = sqlQueryStatmentBuilder;
            validatorManager = new ValidatorManager();
            this.lazySqlQueryVisitor = new Lazy<SqlQueryVisitor>(new SqlQueryVisitor(sqlQueryBuilderFactory));

        }

        private SqlQueryVisitor sqlQueryVisitor
        {
            set
            {
                value = lazySqlQueryVisitor.Value;
            }
            get
            {
                return lazySqlQueryVisitor.Value;
            }
        }

        private SqlStatmentBuilderCommands sqlStatmentBuilderCommands
        {
            set
            {
                value = lazySqlStatmentBuilderCommands.Value;
            }
            get
            {
                return lazySqlStatmentBuilderCommands.Value;
            }
        }


        public async Task<IEnumerable<IDictionary<string, object>>> GetAllAsync(string[] columns, string tableName)
        {
            var queryInput = new QueryData()
            {
                Columns = columns,
                Entity = tableName
            };


            return await SearchEntitiesAsync(queryInput);

        }

        public async Task<IEnumerable<AbstractEntity<IdType>>> GetAllAsync<IdType>(string[] columns,
            string tableName,
            Func<IEnumerable<IDictionary<string, object>>, IEnumerable<AbstractEntity<IdType>>> mapFunction)
        {
            var listofRow = await GetAllAsync(columns, tableName);

            return mapFunction(listofRow);
        }

        public async Task<PagedItems<IDictionary<string, object>>> GetPagedItemsAsync(PageInfo pageInfo, string[] columns, string tableName)
        {
            var queryInput = new QueryData()
            {
                Columns = columns,
                Entity = tableName,
            };


            return await SearchPagedEntitiesAsync(queryInput, pageInfo);

        }

        public async Task<PagedItems<AbstractEntity<IdType>>> GetPagedItemsAsync<IdType>(PageInfo pageInfo,
            string[] columns, string tableName, Func<IEnumerable<IDictionary<string, object>>, PagedItems<AbstractEntity<IdType>>> mapFunction)
        {
            var listofRow = await GetPagedItemsAsync(pageInfo, columns, tableName);

            return mapFunction(listofRow.Items);
        }

        public async Task<IDictionary<string, object>> GetEntityByIdAsync<IdType>(IdType Id, string tableName)
        {
            string selectByIdQuery = sqlQueryStatmentBuilder.GenerateSelectByIdStatment(tableName);

            var entityInfo = await dataService.GetEntityByIdAsync(selectByIdQuery, new Dictionary<string, object>
            {
                {"id", Id}
            });

            return entityInfo;
        }

        public async Task<IEnumerable<IDictionary<string, object>>> SearchEntitiesAsync(QueryData queryData)
        {
            var queryDataValidatorExecutor = new QueryDataValidatorExecutor(queryData);
            validatorManager.ExecuteValidation(queryDataValidatorExecutor);

            var serachQuery = sqlQueryVisitor.GenerateQuery(queryData);

            return await dataService.SearchEntitiesAsync(serachQuery.Item1, serachQuery.Item2);
        }

        public async Task<IEnumerable<AbstractEntity<IdType>>> SearchEntitiesAsync<IdType>(QueryData queryData,
            Func<IEnumerable<IDictionary<string, object>>, IEnumerable<AbstractEntity<IdType>>> mapFunction)
        {
            var queryDataValidatorExecutor = new QueryDataValidatorExecutor(queryData);
            validatorManager.ExecuteValidation(queryDataValidatorExecutor);

            var serachQuery = sqlQueryVisitor.GenerateQuery(queryData);
            var results = await dataService.SearchEntitiesAsync(serachQuery.Item1, serachQuery.Item2);

            return mapFunction(results);
        }

        public async Task<PagedItems<IDictionary<string, object>>> SearchPagedEntitiesAsync(QueryData queryData, PageInfo pageInfo)
        {
            if (pageInfo is null)
            {
                throw new ArgumentNullException(nameof(pageInfo));
            }

            if (pageInfo.PageSize == 0)
            {
                pageInfo.PageSize = 5;
            }

            var serachQuery = sqlQueryVisitor.GenerateQuery(queryData, pageInfo);
            var pagedItems = await dataService.SearchPagedEntitiesAsync(serachQuery.Item1, serachQuery.Item2, pageInfo.ReturnTotalCount);

            return new PagedItems<IDictionary<string, object>>
            {
                Items = pagedItems.Item1,
                PageSize = pageInfo.PageSize,
                PageIndex = pageInfo.PageNumber,
                TotalCount = pagedItems.Item2
            };
        }

        public async Task<PagedItems<AbstractEntity<IdType>>> SearchPagedEntitiesAsync<IdType>(QueryData queryData, PageInfo pageInfo,
            Func<IEnumerable<IDictionary<string, object>>, PagedItems<AbstractEntity<IdType>>> mapFunction)
        {
            var pagedItems = await SearchPagedEntitiesAsync(queryData, pageInfo);

            return mapFunction(pagedItems.Items);
        }

        public async Task<AbstractEntity<IdType>> GetEntityByIdAsync<IdType>(IdType Id, string tableName, Func<IDictionary<string, object>, AbstractEntity<IdType>> mapFunction)
        {
            var entityInfo = await this.GetEntityByIdAsync(Id, tableName);

            return mapFunction(entityInfo);
        }

        public async Task<OperationResult<string>> CreateEntityAsync<IdType>(IDictionary<string, object> data, string tableName, bool addSystemData = false)
        {
            if (addSystemData)
            {
                AddSystemColumnsAndMerge(data, Constants.CreatedOn, Constants.CreatedBy);
            }

            string transactionStatment = sqlStatmentBuilderCommands.CreateCommandGenerator.GenerateCommand(data.Keys, tableName);

            return await dataService.CreateEntityAsync<IdType>(transactionStatment, data);

        }

        public async Task<OperationResult<string>> UpdateEntityAsync<IdType>(IDictionary<string, object> data, string tableName, IdType Id, bool addSystemData = false)
        {
            if (addSystemData)
            {
                AddSystemColumnsAndMerge(data, Constants.ModifiedOn, Constants.ModifiedBy);
            }

            string transactionStatment = sqlStatmentBuilderCommands.UpdateCommandGenerator.GenerateCommand(data.Keys, tableName);

            data.Add("id", Id);

            return await dataService.UpdateEntityAsync<IdType>(transactionStatment, data);
        }

        public async Task<OperationResult<string>> DeleteEntityAsync<IdType>(IdType Id, string tableName)
        {
            var data = new Dictionary<string, object>()
            {
                { "id", Id },
            };

            string transactionStatment = sqlStatmentBuilderCommands.DeleteCommandGenerator.CreateCommand(tableName, false);

            return await dataService.DeleteEntityAsync<IdType>(transactionStatment, data);

        }

        public async Task<OperationResult<int>> CreateEntitiesAsync(IList<string> columns, IEnumerable<IDictionary<string, object>> data, string tableName, bool addSystemData = false)
        {
            if (addSystemData)
            {
                AddSystemColumnsAndMerge(data, Constants.CreatedOn, Constants.CreatedBy);

                columns.Add(Constants.CreatedOn);
                columns.Add(Constants.CreatedBy);
            }

            string transactionStatment = sqlStatmentBuilderCommands.CreateCommandGenerator.GenerateCommand(columns, tableName);

            return await dataService.CreateEntitiesAsync(transactionStatment, data);

        }

        public async Task<OperationResult<int>> UpdateEntitiesAsync(IList<string> columns, IEnumerable<IDictionary<string, object>> data, string tableName, bool addSystemData = false)
        {
            if (addSystemData)
            {
                AddSystemColumnsAndMerge(data, Constants.ModifiedOn, Constants.ModifiedBy);

                columns.Add(Constants.ModifiedOn);
                columns.Add(Constants.ModifiedBy);
            }

            string transactionStatment = sqlStatmentBuilderCommands.UpdateCommandGenerator.GenerateCommand(columns, tableName);

            return await dataService.UpdateEntitiesAsync(transactionStatment, data);

        }

        public async Task<OperationResult<int>> DeleteEntitiesAsync<IdType>(IEnumerable<IdType> Ids, string tableName)
        {
            string transactionStatment = sqlStatmentBuilderCommands.DeleteCommandGenerator.CreateCommand(tableName, true);

            return await dataService.DeleteEntitiesAsync(transactionStatment, Ids);

        }

        public async Task<IDictionary<string, object>> ExecuteQuery(StoredProcedureInfo storedProcedureInfo)
        {
            var SstoredProcedureInfoValidatorExecutor = new StoredProcedureInfoValidatorExecutor(storedProcedureInfo);
            validatorManager.ExecuteValidation(SstoredProcedureInfoValidatorExecutor);

            return await dataService.ExecuteQueryAsync(storedProcedureInfo);
        }

        private IDictionary<string, object> AddSystemColumnsAndMerge(IDictionary<string, object> initialData, string timeColumn, string userColumn)
        {
            var systemColumns = new Dictionary<string, object>()
            {
                {timeColumn, DateTime.UtcNow },
                {userColumn, "admin" }
            };

            foreach (var kv in systemColumns)
            {
                initialData.Add(kv.Key, kv.Value);
            }

            return initialData;
        }

        private IEnumerable<IDictionary<string, object>> AddSystemColumnsAndMerge(IEnumerable<IDictionary<string, object>> initialData, string timeColumn, string userColumn)
        {
            foreach (var kv in initialData)
            {
                kv.Add(timeColumn, DateTime.UtcNow);
                kv.Add(userColumn, "admin");
            }

            return initialData;
        }


    }
}
