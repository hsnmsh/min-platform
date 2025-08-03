namespace MinPlatform.Data.Abstractions.Models
{
    public class LookupInfoEntity : AbstractEntity<int>
    {
        public override int Id 
        { 
            get;
            set;
        }

        public string ValidationName 
        { 
            get;
            set;
        }

        public string TableName 
        { 
            get;
            set; 
        }

        public bool HasDependentColumn 
        { 
            get;
            set; 
        }

        public string ColumnName 
        { 
            get;
            set; 
        }
    }
}
