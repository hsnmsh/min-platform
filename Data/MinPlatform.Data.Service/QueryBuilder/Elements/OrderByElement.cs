namespace MinPlatform.Data.Service.QueryBuilder.Elements
{
    using System.Collections.Generic;

    public sealed class OrderByElement: IQueryElement
    {
        public IList<string> Columns
        {
            get;
            set;
        }

        public OrderOperator Order
        {
            get;
            set;
        }

        public static OrderByElement ToOrderByElement(IList<string> columns, OrderOperator order)
        {
            return new OrderByElement 
            { 
                Columns = columns,
                Order = order 
            };
        }

        public void Accept(SqlQueryBuilder sqlQueryBuilder)
        {
            sqlQueryBuilder.Visit(this);
        }
    }
}
