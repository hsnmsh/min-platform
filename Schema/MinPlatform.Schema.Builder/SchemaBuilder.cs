namespace MinPlatform.Schema.Builder
{
    using MinPlatform.Data.Abstractions;
    using MinPlatform.Data.Abstractions.Exceptions;
    using MinPlatform.Data.Abstractions.Models;
    using MinPlatform.Schema.Abstractions.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Xml.Linq;

    internal sealed class SchemaBuilder : BaseSchemaBuilder
    {
        public override void AddSchema(string schemaName)
        {
            if (string.IsNullOrEmpty(schemaName))
            {
                throw new ArgumentNullException(nameof(schemaName));
            }

            Schemas.Add(schemaName);
        }

        public override void AddFunction(Function function)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            if (string.IsNullOrEmpty(function.Body))
            {
                throw new ArgumentNullException(nameof(function.Body));
            }

            Functions.Add(function);
        }

        public override void AddStoredProcedure(StoredProcedure storedProcedure)
        {
            if (storedProcedure == null)
            {
                throw new ArgumentNullException(nameof(storedProcedure));
            }

            if (string.IsNullOrEmpty(storedProcedure.Body))
            {
                throw new ArgumentNullException(nameof(storedProcedure.Body));
            }

            StoredProcedures.Add(storedProcedure);
        }

        public override void AddView(View view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (string.IsNullOrEmpty(view.Body))
            {
                throw new ArgumentNullException(nameof(view.Body));
            }

            Views.Add(view);
        }

        public override void AddTable(TableConfig tableConfig)
        {
            if (tableConfig == null)
            {
                throw new ArgumentNullException(nameof(tableConfig));
            }

            if (string.IsNullOrEmpty(tableConfig.Name))
            {
                throw new ArgumentNullException(nameof(tableConfig.Name));
            }

            if (tableConfig.Columns == null || !tableConfig.Columns.Any())
            {
                throw new ArgumentNullException("column list must not be empty");
            }

            Tables.Add(tableConfig);
        }

        public override void AddTable<EntityType, IdType>(EntityType entity, Expression<Func<TableConfig>> entityDelegateExp)
        {
            var entityDelegate = entityDelegateExp.Compile();
            TableConfig tableConfig = entityDelegate.Invoke();

            if (tableConfig == null)
            {
                throw new SchemaException(nameof(tableConfig));
            }

            if (string.IsNullOrEmpty(tableConfig.Name))
            {
                throw new SchemaException(nameof(tableConfig.Name));
            }

            if (tableConfig.Columns == null || !tableConfig.Columns.Any())
            {
                throw new SchemaException("column list must not be empty");
            }

            if (!tableConfig.Columns.Any(col => col.Name == Constants.EntityBaseColumns.Id))
            {
                tableConfig.Columns.Add(new Column
                {
                    Name = Constants.EntityBaseColumns.Id,
                    DataType = GetDBTypeForCommonEntityColumn(typeof(IdType)),
                    IsPrimaryKey = true,
                });
            }

            if (!tableConfig.Columns.Any(col => col.Name == Constants.EntityBaseColumns.CreatedOn))
            {
                tableConfig.Columns.Add(new Column
                {
                    Name = Constants.EntityBaseColumns.CreatedOn,
                    DataType = GetDBTypeForCommonEntityColumn(typeof(DateTime?)),
                    Promoted = true,
                    AllowNull = true,
                });
            }

            if (!tableConfig.Columns.Any(col => col.Name == Constants.EntityBaseColumns.CreatedBy))
            {
                tableConfig.Columns.Add(new Column
                {
                    Name = Constants.EntityBaseColumns.CreatedBy,
                    DataType = GetDBTypeForCommonEntityColumn(typeof(string)),
                    Promoted = true,
                    AllowNull = true,
                });
            }

            if (!tableConfig.Columns.Any(col => col.Name == Constants.EntityBaseColumns.ModifiedOn))
            {
                tableConfig.Columns.Add(new Column
                {
                    Name = Constants.EntityBaseColumns.ModifiedOn,
                    DataType = GetDBTypeForCommonEntityColumn(typeof(DateTime?)),
                    Promoted = true,
                    AllowNull = true,
                });
            }

            if (!tableConfig.Columns.Any(col => col.Name == Constants.EntityBaseColumns.ModifiedBy))
            {
                tableConfig.Columns.Add(new Column
                {
                    Name = Constants.EntityBaseColumns.ModifiedBy,
                    DataType = GetDBTypeForCommonEntityColumn(typeof(string)),
                    Promoted = true,
                    AllowNull = true,
                });
            }

            Tables.Add(tableConfig);
        }

        public override void AddLookup(LookupInfoEntity lookupInfoEntity)
        {
            if (lookupInfoEntity == null)
            {
                throw new ArgumentNullException(nameof(lookupInfoEntity));
            }

            if (string.IsNullOrEmpty(lookupInfoEntity.TableName))
            {
                throw new ArgumentNullException(nameof(lookupInfoEntity.TableName));
            }

            Lookups.Add(lookupInfoEntity);
        }

        public override void AlterSchemaName((string oldSchemaName, string newSchemaName) schemaInfo)
        {
            AlteredSchemasNames.Add(schemaInfo);
        }

        public override void AlterTableName((string oldTableName, string newTableName) tableInfo)
        {
            AlteredTableNames.Add(tableInfo);
        }

        public override void AlterColumnName((string tableName, string oldColumnName, string newColumnName, string schemaName) columnNameInfo)
        {
            if (string.IsNullOrEmpty(columnNameInfo.tableName))
            {
                throw new ArgumentNullException(nameof(columnNameInfo.tableName));
            }

            if (string.IsNullOrEmpty(columnNameInfo.oldColumnName))
            {
                throw new ArgumentNullException(nameof(columnNameInfo.oldColumnName));
            }

            if (string.IsNullOrEmpty(columnNameInfo.newColumnName))
            {
                throw new ArgumentNullException(nameof(columnNameInfo.newColumnName));
            }

            AlteredColumnNames.Add(columnNameInfo);
        }

        public override void AlterColumnsForTable(string tableName, IList<Column> alteredColumns)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (alteredColumns == null || !alteredColumns.Any())
            {
                throw new ArgumentNullException(nameof(alteredColumns));
            }

            AlteredTableColumns.Add(tableName, alteredColumns);
        }

        public override void AlterColumnsForTable<EntityType, IdType>(EntityType entity, Expression<Func<IList<Column>>> entityDelegateExp)
        {
            Func<IList<Column>> entityDelegate = entityDelegateExp.Compile();
            IList<Column> result = entityDelegate.Invoke();

            if (result == null || !result.Any())
            {
                throw new SchemaException("column list must not be empty");
            }

            AlteredTableColumns.Add(nameof(entity), result);
        }

        public override void AlterLinkedEntitiesForTable(string tableName, IList<LinkedEntity> alteredLinkedEntities)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (alteredLinkedEntities == null || !alteredLinkedEntities.Any())
            {
                throw new ArgumentNullException(nameof(alteredLinkedEntities));
            }

            AlteredTablesLinkedEntities.Add(tableName, alteredLinkedEntities);
        }

        public override void AlterLinkedEntitiesForTable<EntityType, IdType>(EntityType entity, Expression<Func<IList<LinkedEntity>>> entityDelegateExp)
        {
            Func<IList<LinkedEntity>> entityDelegate = entityDelegateExp.Compile();
            IList<LinkedEntity> result = entityDelegate.Invoke();

            if (result == null || !result.Any())
            {
                throw new SchemaException("linked entity list must not be empty");
            }

            AlteredTablesLinkedEntities.Add(nameof(entity), result);
        }

        public override void AlterTriggersForTable(string tableName, IList<Trigger> alteredTriggers)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new SchemaException(nameof(tableName) + " must not be null or empty");
            }

            if (alteredTriggers == null || !alteredTriggers.Any())
            {
                throw new SchemaException("linked entities list must not be empty");
            }

            AlteredTablesTriggers.Add(tableName, alteredTriggers);
        }

        public override void AlterTriggersForTable<EntityType, IdType>(EntityType entity, Expression<Func<IList<Trigger>>> entityDelegateExp)
        {
            Func<IList<Trigger>> entityDelegate = entityDelegateExp.Compile();
            IList<Trigger> result = entityDelegate.Invoke();

            if (result == null || !result.Any())
            {
                throw new SchemaException("trigger list must not be empty");
            }

            AlteredTablesTriggers.Add(nameof(entity), result);
        }

        public override void AlterStoredProcedure(StoredProcedure storedProcedure)
        {
            if (storedProcedure == null)
            {
                throw new ArgumentNullException(nameof(storedProcedure));
            }

            if (string.IsNullOrEmpty(storedProcedure.Body))
            {
                throw new ArgumentNullException(nameof(storedProcedure.Body));
            }

            AlteredStoredProcedures.Add(storedProcedure);
        }

        public override void AlterFunction(Function function)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            if (string.IsNullOrEmpty(function.Body))
            {
                throw new ArgumentNullException(nameof(function.Body));
            }

            AlteredFunctions.Add(function);
        }

        public override void AlterView(View view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (string.IsNullOrEmpty(view.Body))
            {
                throw new ArgumentNullException(view.Body);
            }

            AlteredViews.Add(view);
        }

        public override void AlterLookupDependency(string lookupTableName, string subLookupTableName)
        {
            if (string.IsNullOrEmpty(lookupTableName))
            {
                throw new ArgumentNullException(nameof(lookupTableName));
            }

            if (string.IsNullOrEmpty(subLookupTableName))
            {
                throw new ArgumentNullException(nameof(subLookupTableName));
            }

            AlteredLookupDependencies.Add((lookupTableName, subLookupTableName));
        }

        public override void DropSchema(string schemaName)
        {
            if (string.IsNullOrEmpty(schemaName))
            {
                throw new ArgumentNullException(nameof(schemaName));
            }

            DroppedSchemas.Add(schemaName);
        }

        public override void DropTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            DroppedTables.Add(tableName);
        }

        public override void DropColumns(string tableName, IList<string> targetColumns)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (targetColumns == null || !targetColumns.Any())
            {
                throw new ArgumentNullException(nameof(targetColumns));
            }

            DroppedColumns.Add(tableName, targetColumns);
        }

        public override void DropConstraints(string tableName, IList<string> targetConstraints)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (targetConstraints == null || !targetConstraints.Any())
            {
                throw new ArgumentNullException(nameof(targetConstraints));
            }

            DroppedConstraints.Add(tableName, targetConstraints);
        }

        public override void DropTriggers(string tableName, IList<string> targetTriggers)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (targetTriggers == null || !targetTriggers.Any())
            {
                throw new ArgumentNullException(nameof(targetTriggers));
            }

            DroppedTriggers.Add(tableName, targetTriggers);
        }

        public override void DropStoredProcedure(string spName)
        {
            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException(nameof(spName));
            }

            DroppedStoredProcedures.Add(spName);
        }

        public override void DropFunction(string functionName)
        {
            if (string.IsNullOrEmpty(functionName))
            {
                throw new ArgumentNullException(nameof(functionName));
            }

            DroppedFunctions.Add(functionName);
        }

        public override void DropView(string view)
        {
            if (string.IsNullOrEmpty(view))
            {
                throw new ArgumentNullException(nameof(view));
            }

            DroppedViews.Add(view);
        }

        public override void DropLookup(string lookupTableName)
        {
            if (string.IsNullOrEmpty(lookupTableName))
            {
                throw new ArgumentNullException(nameof(lookupTableName));
            }

            DroppedLookups.Add(lookupTableName);
        }


        public override void InsertEntities(string tableName, IList<IDictionary<string, object>> entities)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (entities == null || !entities.Any())
            {
                throw new ArgumentNullException(nameof(entities));
            }

            InsertedRecords.Add(tableName, entities);
        }

        public override void UpdateEntities(string tableName, object IdValue, IDictionary<string, object> entity)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (IdValue == null)
            {
                throw new ArgumentNullException(nameof(IdValue));
            }

            if (entity == null || !entity.Any())
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (UpdatedRecords.ContainsKey(tableName))
            {
                IList<(object IdValue, IDictionary<string, object>)> entities = UpdatedRecords[tableName];
                entities.Add((IdValue, entity));
                UpdatedRecords[tableName] = entities;
            }
            else
            {
                var tableEntities = new List<(object IdValue, IDictionary<string, object>)>();
                tableEntities.Add((IdValue, entity));
                UpdatedRecords.Add(tableName, tableEntities);
            }

        }

        public override void RemoveEntities(string tableName, object IdValue)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (IdValue == null)
            {
                throw new ArgumentNullException(nameof(IdValue));
            }

            if (RemovedRecords.ContainsKey(tableName))
            {
                IList<object> Ids = RemovedRecords[tableName];
                Ids.Add(IdValue);
                RemovedRecords[tableName] = Ids;
            }
            else
            {
                var Ids = new List<object>();
                Ids.Add(IdValue);
                RemovedRecords.Add(tableName, Ids);
            }
        }

        
    }
}
