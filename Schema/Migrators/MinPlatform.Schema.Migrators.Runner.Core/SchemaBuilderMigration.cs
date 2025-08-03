namespace MinPlatform.Schema.Migrators.Runner.Core
{
    using Microsoft.Data.SqlClient;
    using MinPlatform.Data.Abstractions.Models;
    using MinPlatform.Schema.Abstractions.Models;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.ForeignKey;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create.Table;
    using MinPlatform.Schema.Migrators.Runner.Core.Extensions;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class SchemaBuilderMigration : Migration
    {
        private readonly DataModel dataModel;

        public SchemaBuilderMigration(DataModel dataModel)
        {
            this.dataModel = dataModel;
        }

        public override void Down()
        {
            CreateDataBase();
            AddSchemas(dataModel.Schemas);
            AddTables(dataModel.Tables);
            AddStoredProcedures(dataModel.StoredProcedures);
            AddFunctions(dataModel.Functions);
            AddViews(dataModel.Views);
            UpdateTableName(dataModel.AlteredTableNames);
            UpdateColumnNames(dataModel.AlteredColumns);
            UpdateColumns(dataModel.AlteredTableColumns);
            UpdateEntitiesRelationships(dataModel.AlteredTablesLinkedEntities);
            UpdateTriggers(dataModel.AlteredTablesTriggers);
            UpdateStoredProcedures(dataModel.AlteredStoredProcedures);
            UpdateFunctions(dataModel.AlteredFunctions);
            UpdateView(dataModel.AlteredViews);
            DeleteSchemas(dataModel.DroppedSchemas);
            DeleteTables(dataModel.DroppedTables);
            DeleteColumn(dataModel.DroppedColumns);
            DeleteForigeinKeys(dataModel.DroppedConstraints);
            DeleteTriggers(dataModel.DroppedTriggers);
            DeleteStoredProcedures(dataModel.DroppedStoredProcedures);
            DeleteFunctions(dataModel.DroppedFunctions);
            DeleteViews(dataModel.DroppedViews);
            AddLookups(dataModel.LookupModel.Lookups);
            SetDependentLookupValue(dataModel.LookupModel.AlteredLookupDependencies);
            DeleteLookups(dataModel.LookupModel.DroppedLookups);
            var insertedRecords = dataModel.ModifyModel.InsertedEntites.ToDictionary(kv => kv.Key, kv => kv.Value.Select(v => (Dictionary<string, object>)v).ToList());
            InsertRecords(insertedRecords);
            UpdateRecords(dataModel.ModifyModel.UpdatedRecords);
            DeleteRecords(dataModel.ModifyModel.RemovedRecords);
        }

        public override void Up()
        {
            CreateDataBase();
            AddSchemas(dataModel.Schemas);
            AddTables(dataModel.Tables);
            AddStoredProcedures(dataModel.StoredProcedures);
            AddFunctions(dataModel.Functions);
            AddViews(dataModel.Views);
            UpdateTableName(dataModel.AlteredTableNames);
            UpdateColumnNames(dataModel.AlteredColumns);
            UpdateColumns(dataModel.AlteredTableColumns);
            UpdateEntitiesRelationships(dataModel.AlteredTablesLinkedEntities);
            UpdateTriggers(dataModel.AlteredTablesTriggers);
            UpdateStoredProcedures(dataModel.AlteredStoredProcedures);
            UpdateFunctions(dataModel.AlteredFunctions);
            UpdateView(dataModel.AlteredViews);
            DeleteSchemas(dataModel.DroppedSchemas);
            DeleteTables(dataModel.DroppedTables);
            DeleteColumn(dataModel.DroppedColumns);
            DeleteForigeinKeys(dataModel.DroppedConstraints);
            DeleteTriggers(dataModel.DroppedTriggers);
            DeleteStoredProcedures(dataModel.DroppedStoredProcedures);
            DeleteFunctions(dataModel.DroppedFunctions);
            DeleteViews(dataModel.DroppedViews);
            AddLookups(dataModel.LookupModel.Lookups);
            SetDependentLookupValue(dataModel.LookupModel.AlteredLookupDependencies);
            DeleteLookups(dataModel.LookupModel.DroppedLookups);
            var insertedRecords = dataModel.ModifyModel.InsertedEntites.ToDictionary(kv => kv.Key, kv => kv.Value.Select(v => (Dictionary<string, object>)v).ToList());
            InsertRecords(insertedRecords);
            UpdateRecords(dataModel.ModifyModel.UpdatedRecords);
            DeleteRecords(dataModel.ModifyModel.RemovedRecords);

        }

        private void CreateDataBase()
        {
            Create.DataBase();
        }

        private void AddSchemas(IEnumerable<string> schemas)
        {
            foreach (var schema in schemas)
            {
                Create.Schema(schema);
            }
        }

        private void AddTables(IEnumerable<TableConfig> tables)
        {
            foreach (var table in tables)
            {
                ICreateTableWithColumnOrSchemaOrDescriptionSyntax tableBuilder = Create.Table(table.Name);

                if (!string.IsNullOrWhiteSpace(table.Schema))
                {
                    tableBuilder.InSchema(table.Schema);
                }

                foreach (Column column in table.Columns)
                {
                    ICreateTableColumnOptionOrWithColumnSyntax columnProperties =
                            tableBuilder
                              .WithColumn(column.Name)
                              .BuildDataBaseType(column);

                    if (table.LinkedEntities != null && table.LinkedEntities.Any())
                    {
                        LinkedEntity linkedEntity = table.LinkedEntities.Where(entity => entity.SourceColumn == column.Name).FirstOrDefault();

                        if (linkedEntity != null)
                        {
                            if (!string.IsNullOrWhiteSpace(linkedEntity.ConstarintName) && !string.IsNullOrWhiteSpace(table.Schema))
                            {
                                var columnForeignKey = columnProperties
                                    .ForeignKey
                                    (
                                        linkedEntity.ConstarintName,
                                        table.Schema,
                                        linkedEntity.TargetTable,
                                        linkedEntity.TargetColumn
                                    );

                                if (linkedEntity.CascadeUpdate != null && linkedEntity.CascadeUpdate.Value)
                                {
                                    columnForeignKey.OnUpdate(System.Data.Rule.Cascade);
                                }

                                if (linkedEntity.CascadeDelete != null && linkedEntity.CascadeDelete.Value)
                                {
                                    columnForeignKey.OnDelete(System.Data.Rule.Cascade);
                                }

                            }
                            else if (!string.IsNullOrWhiteSpace(linkedEntity.ConstarintName))
                            {
                                var columnForeignKey = columnProperties
                                    .ForeignKey
                                    (
                                        linkedEntity.ConstarintName,
                                        linkedEntity.TargetTable,
                                        linkedEntity.TargetColumn
                                    );

                                if (linkedEntity.CascadeUpdate != null && linkedEntity.CascadeUpdate.Value)
                                {
                                    columnForeignKey.OnUpdate(System.Data.Rule.Cascade);
                                }

                                if (linkedEntity.CascadeDelete != null && linkedEntity.CascadeDelete.Value)
                                {
                                    columnForeignKey.OnDelete(System.Data.Rule.Cascade);
                                }
                            }
                            else
                            {
                                var columnForeignKey = columnProperties
                                    .ForeignKey
                                    (
                                        linkedEntity.TargetTable,
                                        linkedEntity.TargetColumn
                                    );

                                if (linkedEntity.CascadeUpdate != null && linkedEntity.CascadeUpdate.Value)
                                {
                                    columnForeignKey.OnUpdate(System.Data.Rule.Cascade);
                                }

                                if (linkedEntity.CascadeDelete != null && linkedEntity.CascadeDelete.Value)
                                {
                                    columnForeignKey.OnDelete(System.Data.Rule.Cascade);
                                }
                            }

                        }
                    }
                }

                if (table.Triggers != null && table.Triggers.Any())
                {
                    foreach (Trigger trigger in table.Triggers)
                    {
                        if (trigger.Parameters != null && trigger.Parameters.Any())
                        {
                            Execute.Sql(trigger.Body, trigger.Parameters);

                        }
                        else
                        {
                            Execute.Sql(trigger.Body);
                        }
                    }
                }
            }
        }

        private void AddStoredProcedures(IEnumerable<StoredProcedure> storedProcedures)
        {
            foreach (StoredProcedure storedProcedure in storedProcedures)
            {
                if (storedProcedure.Parameters != null && storedProcedure.Parameters.Any())
                {
                    Execute.Sql(storedProcedure.Body, storedProcedure.Parameters);

                }
                else
                {
                    Execute.Sql(storedProcedure.Body);
                }
            }
        }

        private void AddFunctions(IEnumerable<Function> functions)
        {
            foreach (Function function in functions)
            {
                if (function.Parameters != null && function.Parameters.Any())
                {
                    Execute.Sql(function.Body, function.Parameters);

                }
                else
                {
                    Execute.Sql(function.Body);
                }
            }
        }

        private void AddViews(IEnumerable<View> views)
        {
            foreach (View view in views)
            {
                if (view.Parameters != null && view.Parameters.Any())
                {
                    Execute.Sql(view.Body, view.Parameters);

                }
                else
                {
                    Execute.Sql(view.Body);
                }
            }
        }

        private void UpdateTableName(IEnumerable<(string oldTableName, string newTableName)> alteredTableNames)
        {
            foreach (var tableInfo in alteredTableNames)
            {
                if (tableInfo.oldTableName.Contains(".") && tableInfo.newTableName.Contains("."))
                {
                    string[] tableNameWithSchema = tableInfo.oldTableName.Split(".");

                    Rename
                        .Table(tableInfo.oldTableName)
                        .To(tableInfo.newTableName)
                        .InSchema(tableNameWithSchema[0]);

                }
                else
                {
                    Rename
                        .Table(tableInfo.oldTableName)
                        .To(tableInfo.newTableName);

                }
            }
        }

        private void UpdateColumnNames(IEnumerable<(string tableName, string oldColumnName, string newColumnName, string schemaName)> columns)
        {
            foreach (var tableColumn in columns)
            {
                if (!string.IsNullOrWhiteSpace(tableColumn.schemaName))
                {
                    Rename
                        .Column(tableColumn.oldColumnName)
                        .OnTable(tableColumn.tableName)
                        .InSchema(tableColumn.schemaName)
                        .To(tableColumn.newColumnName);
                }
                else
                {
                    Rename
                        .Column(tableColumn.oldColumnName)
                        .OnTable(tableColumn.tableName)
                        .To(tableColumn.newColumnName);
                }

            }
        }

        private void UpdateColumns(IDictionary<string, IList<Column>> tables)
        {
            foreach (var table in tables)
            {
                foreach (Column item in table.Value)
                {
                    if (table.Key.Contains("."))
                    {
                        string[] tableName = table.Key.Split(".");

                        Alter
                            .Table(tableName[1])
                            .InSchema(tableName[0])
                            .AlterColumn(item.Name)
                            .BuildDataBaseType(item);
                    }
                    else
                    {
                        Alter
                            .Table(table.Key)
                            .AlterColumn(item.Name)
                            .BuildDataBaseType(item);
                    }
                }

            }
        }

        private void UpdateEntitiesRelationships(IDictionary<string, IList<LinkedEntity>> linkedEntities)
        {
            foreach (var entity in linkedEntities)
            {
                foreach (var linkedEntity in entity.Value)
                {
                    if (!string.IsNullOrWhiteSpace(linkedEntity.ConstarintName))
                    {

                        ICreateForeignKeyCascadeSyntax createdFK = Create
                            .ForeignKey(linkedEntity.ConstarintName)
                            .FromTable(entity.Key)
                            .ForeignColumn(linkedEntity.SourceColumn)
                            .ToTable(linkedEntity.TargetTable)
                            .PrimaryColumn(linkedEntity.TargetColumn);

                        if (linkedEntity.CascadeUpdate != null && linkedEntity.CascadeUpdate.Value)
                        {
                            createdFK.OnUpdate(System.Data.Rule.Cascade);
                        }

                        if (linkedEntity.CascadeDelete != null && linkedEntity.CascadeDelete.Value)
                        {
                            createdFK.OnDelete(System.Data.Rule.Cascade);
                        }

                    }
                    else
                    {
                        ICreateForeignKeyCascadeSyntax createdFK = Create
                           .ForeignKey($"ForeignKey_{entity.Key}_TO_{linkedEntity.TargetTable}_FOR_{linkedEntity.SourceColumn}_{linkedEntity.TargetColumn}")
                           .FromTable(entity.Key)
                           .ForeignColumn(linkedEntity.SourceColumn)
                           .ToTable(linkedEntity.TargetTable)
                           .PrimaryColumn(linkedEntity.TargetColumn);

                        if (linkedEntity.CascadeUpdate != null && linkedEntity.CascadeUpdate.Value)
                        {
                            createdFK.OnUpdate(System.Data.Rule.Cascade);
                        }

                        if (linkedEntity.CascadeDelete != null && linkedEntity.CascadeDelete.Value)
                        {
                            createdFK.OnDelete(System.Data.Rule.Cascade);
                        }
                    }
                }
            }
        }

        private void UpdateTriggers(IDictionary<string, IList<Trigger>> triggerCollection)
        {
            foreach (var item in triggerCollection)
            {
                foreach (var trigger in item.Value)
                {
                    if (trigger.Parameters != null && trigger.Parameters.Any())
                    {
                        Execute.Sql(trigger.Body, trigger.Parameters);

                    }
                    else
                    {
                        Execute.Sql(trigger.Body);
                    }
                }
            }
        }

        private void UpdateStoredProcedures(IEnumerable<StoredProcedure> storedProceduresCollection)
        {
            foreach (var storedProcedure in storedProceduresCollection)
            {
                if (storedProcedure.Parameters != null && storedProcedure.Parameters.Any())
                {
                    Execute.Sql(storedProcedure.Body, storedProcedure.Parameters);

                }
                else
                {
                    Execute.Sql(storedProcedure.Body);
                }
            }
        }

        private void UpdateFunctions(IEnumerable<Function> functions)
        {
            foreach (var function in functions)
            {
                if (function.Parameters != null && function.Parameters.Any())
                {
                    Execute.Sql(function.Body, function.Parameters);

                }
                else
                {
                    Execute.Sql(function.Body);
                }
            }
        }

        private void UpdateView(IEnumerable<View> views)
        {
            foreach (View view in views)
            {
                if (view.Parameters != null && view.Parameters.Any())
                {
                    Execute.Sql(view.Body, view.Parameters);

                }
                else
                {
                    Execute.Sql(view.Body);
                }
            }
        }

        private void DeleteSchemas(IEnumerable<string> schemas)
        {
            foreach (var schema in schemas)
            {
                Delete.Schema(schema);
            }
        }

        private void DeleteTables(IEnumerable<string> tables)
        {
            foreach (var tableName in tables)
            {
                if (tableName.Contains("."))
                {
                    string[] table = tableName.Split(".");

                    Delete
                        .Table(table[1])
                        .InSchema(table[0]);

                }
                else
                {
                    Delete.Table(tableName);
                }
            }
        }

        private void DeleteColumn(IDictionary<string, IList<string>> tables)
        {
            foreach (var table in tables)
            {
                string schemaName = null;
                string tableName = null;

                if (table.Key.Contains("."))
                {
                    string[] tableNameElements = table.Key.Split(".");

                    schemaName = tableNameElements[0];
                    tableName = tableNameElements[1];
                }

                foreach (var columnName in table.Value)
                {
                    if (!string.IsNullOrEmpty(schemaName))
                    {
                        Delete
                            .Column(columnName)
                            .FromTable(tableName)
                            .InSchema(schemaName);
                    }
                    else
                    {
                        Delete
                            .Column(columnName)
                            .FromTable(tableName);
                    }
                }
            }
        }

        private void DeleteForigeinKeys(IDictionary<string, IList<string>> keys)
        {
            foreach (var table in keys)
            {
                string schemaName = null;
                string tableName = null;

                if (table.Key.Contains("."))
                {
                    string[] tableNameElements = table.Key.Split(".");

                    schemaName = tableNameElements[0];
                    tableName = tableNameElements[1];
                }

                foreach (var foreignName in table.Value)
                {
                    if (!string.IsNullOrEmpty(schemaName))
                    {
                        Delete
                            .ForeignKey(foreignName)
                            .OnTable(tableName)
                            .InSchema(schemaName);
                    }
                    else
                    {
                        Delete
                           .ForeignKey(foreignName)
                           .OnTable(tableName);
                    }
                }
            }
        }

        private void DeleteTriggers(IDictionary<string, IList<string>> triggers)
        {
            foreach (var item in triggers)
            {
                foreach (var trigger in item.Value)
                {
                    Execute.Sql(trigger);
                }
            }
        }

        private void DeleteStoredProcedures(IEnumerable<string> storedProcedures)
        {
            foreach (var sp in storedProcedures)
            {
                Execute.Sql(sp);
            }
        }

        private void DeleteFunctions(IEnumerable<string> functions)
        {
            foreach (var funcDeleteExpression in functions)
            {
                Execute.Sql(funcDeleteExpression);
            }
        }

        private void DeleteViews(IEnumerable<string> views)
        {
            foreach (var viewDeleteExpression in views)
            {
                Execute.Sql(viewDeleteExpression);
            }
        }

        private void AddLookups(IEnumerable<LookupInfoEntity> lookups)
        {
            var lookupInfo = new Dictionary<string, List<Dictionary<string, object>>>();
            var records = new List<Dictionary<string, object>>();

            foreach (LookupInfoEntity lookup in lookups)
            {
                records.Add(new Dictionary<string, object>
                {
                    {"ValidationName", lookup.TableName },
                    {"TableName", lookup.TableName },
                    {"HasDependentColumn", lookup.HasDependentColumn  },
                    {"ColumnName", lookup.ColumnName  },
                });

                ICreateTableColumnOptionOrWithColumnSyntax lookupTable = null;

                if (lookup.TableName.Contains("."))
                {
                    string[] tableNameWithSchema = lookup.TableName.Split(".");

                    lookupTable = Create
                        .Table(tableNameWithSchema[1])
                        .InSchema(tableNameWithSchema[0])
                        .WithColumn("Id")
                        .AsString(255)
                        .PrimaryKey()
                        .WithColumn("LanguageCode")
                        .AsString(255)
                        .PrimaryKey()
                        .WithColumn("Title")
                        .AsString()
                        .WithColumn("Code")
                        .AsString(50)
                        .WithColumn("CreatedOn")
                        .AsDateTime()
                        .WithColumn("CreatedBy")
                        .AsString(20)
                        .WithColumn("ModifiedOn")
                        .AsDateTime()
                        .WithColumn("ModifiedBy")
                        .AsString(20);


                }
                else
                {
                    lookupTable = Create
                        .Table(lookup.TableName)
                        .WithColumn("Id")
                        .AsString(255)
                        .PrimaryKey()
                        .WithColumn("LanguageCode")
                        .AsString(255)
                        .PrimaryKey()
                        .WithColumn("Title")
                        .AsString()
                        .WithColumn("Code")
                        .AsString(50)
                        .WithColumn("CreatedOn")
                        .AsDateTime()
                        .WithColumn("CreatedBy")
                        .AsString(20)
                        .WithColumn("ModifiedOn")
                        .AsDateTime()
                        .WithColumn("ModifiedBy")
                        .AsString(20);
                }

                if (lookup.HasDependentColumn && !string.IsNullOrWhiteSpace(lookup.ColumnName))
                {
                    lookupTable = lookupTable
                        .WithColumn(lookup.ColumnName)
                        .AsString(255);

                }
            }

            lookupInfo.Add("[MinPlatform].[LookupInfo]", records);
            InsertRecords(lookupInfo);
        }

        private void SetDependentLookupValue
            (IEnumerable<(string lookupTableName, string subLookupTableName)> alteredLookupDependencies)
        {
            foreach (var item in alteredLookupDependencies)
            {
                Update
                       .Table("LookupInfo")
                       .InSchema("MinPlatform")
                       .Set(new { HasDependentColumn = true, ColumnName = $"{item.subLookupTableName}Id" })
                       .Where(new { TableName = item.lookupTableName });

                if (item.lookupTableName.Contains("."))
                {
                    string[] tableNameWithSchema = item.lookupTableName.SplitToSchemaAndTableName();

                    Alter
                        .Table(tableNameWithSchema[1])
                        .InSchema(tableNameWithSchema[0])
                        .AddColumn($"{item.subLookupTableName}Id")
                        .AsString(255);



                }
                else
                {
                    Alter
                        .Table(item.lookupTableName)
                        .AddColumn($"{item.subLookupTableName}Id")
                        .AsString(255);
                }
            }
        }

        private void DeleteLookups(IEnumerable<string> lookupTableNames)
        {
            foreach (var lookupTable in lookupTableNames)
            {
                Delete.FromTable("LookupInfo")
                      .InSchema("MinPlatform")
                      .Row(new { TableName = lookupTable });
            }

            DeleteTables(lookupTableNames);
        }

        private void InsertRecords(Dictionary<string, List<Dictionary<string, object>>> records)
        {
            foreach (var record in records)
            {
                if (record.Key.Contains("."))
                {
                    string[] tableNameWithSchema = record.Key.SplitToSchemaAndTableName();

                    foreach (var row in record.Value)
                    {
                        Insert
                        .IntoTable(tableNameWithSchema[1])
                        .InSchema(tableNameWithSchema[0])
                        .Row(row);
                    }

                }
                else
                {
                    foreach (var row in record.Value)
                    {
                        Insert
                        .IntoTable(record.Key)
                        .Row(row);
                    }
                }
            }
        }

        private void UpdateRecords(IDictionary<string, IList<(object IdValue, IDictionary<string, object>)>> records)
        {
            foreach (var record in records)
            {
                if (record.Key.Contains("."))
                {
                    string[] tableNameWithSchema = record.Key.SplitToSchemaAndTableName();

                    foreach (var row in record.Value)
                    {
                        Update
                        .Table(tableNameWithSchema[1])
                        .InSchema(tableNameWithSchema[0])
                        .Set(row.Item2)
                        .Where(new { Id = row.IdValue });
                    }

                }
                else
                {
                    foreach (var row in record.Value)
                    {
                        Update
                       .Table(record.Key)
                       .Set(row.Item2)
                       .Where(new { Id = row.IdValue });
                    }
                }
            }
        }

        private void DeleteRecords(IDictionary<string, IList<object>> records)
        {
            foreach (var record in records)
            {
                if (record.Key.Contains("."))
                {
                    string[] tableNameWithSchema = record.Key.SplitToSchemaAndTableName();

                    foreach (var Id in record.Value)
                    {
                        Delete
                        .FromTable(tableNameWithSchema[1])
                        .InSchema(tableNameWithSchema[0])
                        .Row(new { Id });
                    }

                }
                else
                {
                    foreach (var Id in record.Value)
                    {
                        Delete
                        .FromTable(record.Key)
                        .Row(new { Id });
                    }
                }
            }
        }
    }
}
