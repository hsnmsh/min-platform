namespace MinPlatform.Data.Service.QueryBuilder.Elements
{

    public sealed class CountElement : IQueryElement
    {
       
        public string TableName
        {
            get;
            set;
        }

        public static CountElement ToCountElement( string tableName)
        {
            return new CountElement
            {
                TableName = tableName
            };
        }

        public void Accept(SqlQueryBuilder sqlQueryBuilder)
        {
            sqlQueryBuilder.Visit(this);
        }
    }
}
