namespace MinPlatform.Data.Service.QueryBuilder.Elements
{
    using MinPlatform.Data.Service.Models;
    using System.Collections.Generic;

    public sealed class WhereElement: IQueryElement
    {
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

        public static WhereElement ToWhereElement(IList<ConditionGroup> conditions, LogicalOperator logicalOperator)
        {
            return new WhereElement 
            { 
                Conditions = conditions,
                LogicalOperator = logicalOperator
            };
        }

        public void Accept(SqlQueryBuilder sqlQueryBuilder)
        {
            sqlQueryBuilder.Visit(this);
        }
    }
}
