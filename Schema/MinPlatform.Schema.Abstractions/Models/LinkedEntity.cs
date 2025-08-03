namespace MinPlatform.Schema.Abstractions.Models
{
    public sealed class LinkedEntity
    {
        
        public string ConstarintName
        {
            get;
            set;
        }

        public string SourceColumn 
        { 
            get; 
            set;
        }

        public string SchemaName
        {
            get;
            set;
        }

        public string TargetTable 
        { 
            get;
            set;
        }

        public string TargetColumn
        {
            get;
            set;
        }

        public RelationType RelationType
        {
            get;
            set;
        }

        public bool? CascadeUpdate
        {
            get;
            set;
        }

        public bool? CascadeDelete
        {
            get;
            set;
        }

        public bool? SetNullOnUpdate
        {
            get;
            set;
        }

        public bool? SetNullOnDelete
        {
            get;
            set;
        }
    }

    public enum RelationType
    {
        OneToOne,
        OneToMany,
        ManyToMany,
    }
}
