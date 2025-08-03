using System.Collections.Generic;

namespace MinPlatform.Data.Service.Models
{
    public sealed class ConditionGroup
    {
        public IList<Condition> Condition
        {
            get;
            set;
        }

        public LogicalOperator? Operator
        {
            get;
            set;
        }
    }
}
