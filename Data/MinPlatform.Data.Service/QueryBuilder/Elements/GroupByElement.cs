namespace MinPlatform.Data.Service.QueryBuilder.Elements
{
    using System.Collections.Generic;

    public sealed class GroupByElement: IQueryElement
    {
        public IList<string> GroupBy
        {
            get;
            set;
        }

        public static GroupByElement ToGroupByElement(IList<string> groupBy)
        {
            return new GroupByElement 
            {
                GroupBy = groupBy
            };
        }

        public void Accept(SqlQueryBuilder sqlQueryBuilder)
        {
            sqlQueryBuilder.Visit(this);
        }
    }
}
