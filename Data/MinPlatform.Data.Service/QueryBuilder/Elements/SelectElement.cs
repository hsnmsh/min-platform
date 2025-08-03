namespace MinPlatform.Data.Service.QueryBuilder.Elements
{
    using System.Collections.Generic;

    public sealed class SelectElement : IQueryElement
    {
        public IList<string> Columns
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

        public static SelectElement ToSelectElement(IList<string> columns, string tableName, bool isDistinct, bool singleRow)
        {
            return new SelectElement
            {
                Columns = columns,
                IsDistinct = isDistinct,
                Entity = tableName,
                SingleRow = singleRow
            };
        }

        public void Accept(SqlQueryBuilder sqlQueryBuilder)
        {
            sqlQueryBuilder.Visit(this);
        }
    }
}
