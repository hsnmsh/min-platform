namespace MinPlatform.Data.Service.Models
{
    using System.Collections.Generic;

    public sealed class QueryData
    {
        public IList<string>  Columns
        {
            get;
            set;
        }

        public bool IsDistinct
        {
            get;
            set;
        }

        public bool SingleRow
        {
            get;
            set;
        }

        public string Entity
        {
            get;
            set;
        }

        public LogicalOperator LogicalOperator
        {
            get;
            set;
        }

        public IList<ConditionGroup> Conditions
        {
            get;
            set;
        }

        public IList<Join> JoinEntity
        {
            get;
            set;
        }

        public OrderBy OrderBy 
        { 
            get;
            set;
        }
    }
}
