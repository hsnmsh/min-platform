namespace MinPlatform.Data.Service.Models
{
    public sealed class Join
    {
        public JoinType JoinType
        {
            get;
            set;
        }

        public JoinEntityInfo SourceEntity
        {
            get;
            set;
        }

        public JoinEntityInfo TargetEntity
        {
            get;
            set;
        }


    }

    public sealed class JoinEntityInfo : ElementInfo
    {
        public string ColumnName
        {
            get;
            set;
        }
    }

    
}