namespace MinPlatform.Schema.Builder
{
    using MinPlatform.Data.Abstractions.Exceptions;
    using MinPlatform.Data.Abstractions.Models;
    using MinPlatform.Schema.Abstractions.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;

    public abstract class BaseSchemaBuilder
    {
        public readonly IList<string> Schemas;
        public readonly IList<TableConfig> Tables;
        public readonly IList<StoredProcedure> StoredProcedures;
        public readonly IList<Function> Functions;
        public readonly IList<View> Views;

        public readonly IList<(string oldSchemaName, string newSchemaName)> AlteredSchemasNames;
        public readonly IList<(string oldTableName, string newTableName)> AlteredTableNames;
        public readonly IList<(string tableName, string oldColumnName, string newColumnName, string schemaName)> AlteredColumnNames;
        public readonly IDictionary<string, IList<Column>> AlteredTableColumns;
        public readonly IDictionary<string, IList<LinkedEntity>> AlteredTablesLinkedEntities;
        public readonly IDictionary<string, IList<Trigger>> AlteredTablesTriggers;
        public readonly IList<StoredProcedure> AlteredStoredProcedures;
        public readonly IList<Function> AlteredFunctions;
        public readonly IList<View> AlteredViews;

        public readonly IList<string> DroppedSchemas;
        public readonly IList<string> DroppedTables;
        public readonly IDictionary<string, IList<string>> DroppedColumns;
        public readonly IDictionary<string, IList<string>> DroppedConstraints;
        public readonly IDictionary<string, IList<string>> DroppedTriggers;
        public readonly IList<string> DroppedStoredProcedures;
        public readonly IList<string> DroppedFunctions;
        public readonly IList<string> DroppedViews;

        public readonly IList<LookupInfoEntity> Lookups;
        public readonly IList<(string lookupTableName, string subLookupTableName)> AlteredLookupDependencies;
        public readonly IList<string> DroppedLookups;

        public readonly IDictionary<string, IList<IDictionary<string, object>>> InsertedRecords;
        public readonly IDictionary<string, IList<(object IdValue, IDictionary<string, object>)>> UpdatedRecords;
        public readonly IDictionary<string, IList<object>> RemovedRecords;


        protected BaseSchemaBuilder()
        {
            Schemas = new List<string>();
            Tables = new List<TableConfig>();
            StoredProcedures = new List<StoredProcedure>();
            Functions = new List<Function>();
            Views = new List<View>();
            AlteredSchemasNames = new List<(string oldSchemaName, string newSchemaName)>();
            AlteredTableNames = new List<(string oldTableName, string newTableName)>();
            AlteredColumnNames = new List<(string tableName, string oldColumnName, string newColumnName, string schemaName)>();
            AlteredTableColumns = new Dictionary<string, IList<Column>>();
            AlteredTablesLinkedEntities = new Dictionary<string, IList<LinkedEntity>>();
            AlteredTablesTriggers = new Dictionary<string, IList<Trigger>>();
            AlteredStoredProcedures = new List<StoredProcedure>();
            AlteredFunctions = new List<Function>();
            AlteredViews = new List<View>();
            DroppedSchemas = new List<string>();
            DroppedTables = new List<string>();
            DroppedColumns = new Dictionary<string, IList<string>>();
            DroppedConstraints = new Dictionary<string, IList<string>>();
            DroppedTriggers = new Dictionary<string, IList<string>>();
            DroppedStoredProcedures = new List<string>();
            DroppedFunctions = new List<string>();
            DroppedViews = new List<string>();
            Lookups = new List<LookupInfoEntity>();
            AlteredLookupDependencies = new List<(string lookupTableName, string subLookupTableName)>();
            DroppedLookups = new List<string>();
            InsertedRecords = new Dictionary<string, IList<IDictionary<string, object>>>();
            UpdatedRecords = new Dictionary<string, IList<(object IdValue, IDictionary<string, object>)>>();
            RemovedRecords = new Dictionary<string, IList<object>>();

        }

        public abstract void AddSchema(string schemaName);
        public abstract void AddTable(TableConfig tableConfig);
        public abstract void AddTable<EntityType, IdType>(EntityType entity, Expression<Func<TableConfig>> entityDelegateExp)
            where EntityType : AbstractEntity<IdType>;
        public abstract void AddStoredProcedure(StoredProcedure storedProcedure);
        public abstract void AddFunction(Function function);
        public abstract void AddView(View view);
        public abstract void AddLookup(LookupInfoEntity lookupInfoEntity);
        public abstract void AlterSchemaName((string oldSchemaName, string newSchemaName) schemaInfo);
        public abstract void AlterTableName((string oldTableName, string newTableName) tableInfo);
        public abstract void AlterColumnName((string tableName, string oldColumnName, string newColumnName, string schemaName) columnNameInfo);
        public abstract void AlterColumnsForTable(string tableName, IList<Column> alteredColumns);
        public abstract void AlterColumnsForTable<EntityType, IdType>(EntityType entity, Expression<Func<IList<Column>>> entityDelegateExp)
            where EntityType : AbstractEntity<IdType>;
        public abstract void AlterLinkedEntitiesForTable(string tableName, IList<LinkedEntity> alteredLinkedEntities);
        public abstract void AlterLinkedEntitiesForTable<EntityType, IdType>(EntityType entity, Expression<Func<IList<LinkedEntity>>> entityDelegateExp)
            where EntityType : AbstractEntity<IdType>;
        public abstract void AlterTriggersForTable(string tableName, IList<Trigger> alteredTriggers);
        public abstract void AlterTriggersForTable<EntityType, IdType>(EntityType entity, Expression<Func<IList<Trigger>>> entityDelegateExp)
            where EntityType : AbstractEntity<IdType>;
        public abstract void AlterStoredProcedure(StoredProcedure storedProcedure);
        public abstract void AlterFunction(Function function);
        public abstract void AlterView(View view);
        public abstract void AlterLookupDependency(string lookupTableName, string subLookupTableName);
        public abstract void DropSchema(string schemaName);
        public abstract void DropTable(string tableName);
        public abstract void DropColumns(string tableName, IList<string> targetColumns);
        public abstract void DropConstraints(string tableName, IList<string> targetConstraints);
        public abstract void DropTriggers(string tableName, IList<string> targetTriggers);
        public abstract void DropStoredProcedure(string spName);
        public abstract void DropFunction(string functionName);
        public abstract void DropView(string view);
        public abstract void DropLookup(string lookupTableName);

        public abstract void InsertEntities(string tableName, IList<IDictionary<string, object>> entities);
        public abstract void UpdateEntities(string tableName, object IdValue, IDictionary<string, object> entity);
        public abstract void RemoveEntities(string tableName, object IdValue);




        protected DbType GetDBTypeForCommonEntityColumn(Type type)
        {
            if (type == typeof(int))
            {
                return DbType.Int32;
            }

            if (type == typeof(string))
            {
                return DbType.String;
            }

            if (type == typeof(DateTime?))
            {
                return DbType.DateTime;
            }

            if (type == typeof(Guid))
            {
                return DbType.Guid;
            }

            throw new EntityDataTypeException("Invalid data type for entity");
        }

        protected internal DataModel Build()
        {
            return new DataModel
            {
                DataBaseConfig = new DataBaseConfig(),
                Functions = Functions,
                Views = Views,
                Schemas = Schemas,
                Tables = Tables,
                AlteredSchemasNames = AlteredSchemasNames,
                AlteredTableNames = AlteredTableNames,
                AlteredColumns = AlteredColumnNames,
                AlteredStoredProcedures = AlteredStoredProcedures,
                AlteredFunctions = AlteredFunctions,
                AlteredTableColumns = AlteredTableColumns,
                AlteredTablesLinkedEntities = AlteredTablesLinkedEntities,
                AlteredTablesTriggers = AlteredTablesTriggers,
                AlteredViews = AlteredViews,
                DroppedColumns = DroppedColumns,
                DroppedConstraints = DroppedConstraints,
                DroppedFunctions = DroppedFunctions,
                DroppedSchemas = DroppedSchemas,
                DroppedStoredProcedures = DroppedStoredProcedures,
                DroppedTables = DroppedTables,
                DroppedTriggers = DroppedTriggers,
                DroppedViews = DroppedViews,
                StoredProcedures = StoredProcedures,
                LookupModel = new LookupDataModel
                {
                    Lookups = Lookups,
                    AlteredLookupDependencies = AlteredLookupDependencies,
                    DroppedLookups = DroppedLookups
                },
                ModifyModel = new ModifyEntityModel
                {
                    InsertedEntites = InsertedRecords,
                    UpdatedRecords = UpdatedRecords,
                    RemovedRecords = RemovedRecords
                }
            };
        }

    }
}
