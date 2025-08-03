namespace MinPlatform.Logging.Abstractions.Models
{
    using System.Collections.Generic;
    using System.Data;

    public sealed class DataBaseProperties : Properties
    {
        public string ConnectionString 
        { 
            get; 
            set; 
        }

        public bool AutomaticTableCreation
        {
            get;
            set;
        }

        public TableInfo TableInfo 
        { 
            get; 
            set;
        }
    }

    public class ColumnInfo
    {
        public string Name 
        { 
            get;
            set; 
        }

        public int Size
        {
            get;
            set;
        }

        public SqlDbType DataType
        {
            get;
            set;
        }

    }

    public class TableInfo
    {
        public string Name
        {
            get;
            set;
        }

        public IList<ColumnInfo> Columns 
        { 
            get;
            set;
        } 

    }
}
