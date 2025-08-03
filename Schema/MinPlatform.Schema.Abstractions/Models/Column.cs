namespace MinPlatform.Schema.Abstractions.Models
{
    using System.Data;

    public sealed class Column
    {
        public string Name 
        { 
            get;
            set; 
        }

        public string Alias
        {
            get;
            set;
        }

        public DbType DataType
        {
            get;
            set;
        }

        public bool IsPrimaryKey 
        { 
            get; 
            set; 
        }

        public bool? IsUnique
        {
            get;
            set;
        }

        public bool? Promoted
        {
            get;
            set;
        }

        public bool? AllowNull
        {
            get;
            set;
        }

        public bool? IsAutoIncrement
        {
            get;
            set;
        }

        public object DefaultValue
        {
            get;
            set;
        }

        public int? MaxLength
        {
            get;
            set;
        }
    }
}
