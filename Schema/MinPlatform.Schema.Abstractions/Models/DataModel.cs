namespace MinPlatform.Schema.Abstractions.Models
{
    using MinPlatform.Data.Abstractions.Models;
    using System.Collections.Generic;

    public sealed class DataModel
    {
        public DataBaseConfig DataBaseConfig
        {
            get;
            set;
        }

        public IList<string> Schemas
        {
            get;
            set;
        }

        public IList<TableConfig> Tables
        {
            get;
            set;
        }

        public IList<StoredProcedure> StoredProcedures
        {
            get;
            set;
        }

        public IList<Function> Functions
        {
            get;
            set;
        }

        public IList<View> Views
        {
            get;
            set;
        }

        public IList<(string oldSchemaName, string newSchemaName)> AlteredSchemasNames
        {
            get;
            set;
        }

        public IList<(string oldTableName, string newTableName)> AlteredTableNames
        {
            get;
            set;
        }

        public IList<(string tableName, string oldColumnName, string newColumnName, string schemaName)> AlteredColumns
        {
            get;
            set;
        }

        public IDictionary<string, IList<Column>> AlteredTableColumns
        {
            get;
            set;
        }

        public IDictionary<string, IList<LinkedEntity>> AlteredTablesLinkedEntities
        {
            get;
            set;
        }

        public IDictionary<string, IList<Trigger>> AlteredTablesTriggers 
        { 
            get;
            set;
        }

        public IList<StoredProcedure> AlteredStoredProcedures 
        { 
            get;
            set;
        }

        public IList<Function> AlteredFunctions 
        { 
            get;
            set;
        }

        public IList<View> AlteredViews 
        { 
            get;
            set; 
        }

        public IList<string> DroppedSchemas 
        { 
            get;
            set; 
        }

        public IList<string> DroppedTables 
        { 
            get;
            set; 
        }

        public IDictionary<string, IList<string>> DroppedColumns 
        { 
            get;
            set;
        }

        public IDictionary<string, IList<string>> DroppedConstraints 
        { 
            get;
            set;
        }

        public IDictionary<string, IList<string>> DroppedTriggers 
        { 
            get;
            set; 
        }

        public IList<string> DroppedStoredProcedures 
        { 
            get;
            set; 
        }

        public IList<string> DroppedFunctions 
        { 
            get; 
            set;
        }

        public IList<string> DroppedViews 
        { 
            get;
            set; 
        }

        public LookupDataModel LookupModel 
        { 
            get;
            set;
        }

        public ModifyEntityModel ModifyModel 
        {
            get;
            set; 
        }
    }

    public sealed class LookupDataModel
    {
        public IList<LookupInfoEntity> Lookups 
        { 
            get;
            set;
        }

        public IList<(string lookupTableName, string subLookupTableName)> AlteredLookupDependencies 
        { 
            get; 
            set;
        }

        public IList<string> DroppedLookups 
        { 
            get;
            set;
        }
    }

    public sealed class ModifyEntityModel
    {
        public IDictionary<string, IList<IDictionary<string, object>>> InsertedEntites
        {
            get;
            set;
        }

        public IDictionary<string, IList<(object IdValue, IDictionary<string, object>)>> UpdatedRecords
        {
            get;
            set;
        }

        public IDictionary<string, IList<object>> RemovedRecords
        {
            get;
            set;
        }
    }
}
