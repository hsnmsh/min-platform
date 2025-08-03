namespace MinPlatform.Data.Sql.Factory
{
    using MinPlatform.Data.Service.QueryBuilder;
    using MinPlatform.Data.Service.QueryBuilder.Factory;

    public class SqlServerQueryBuilderFactory : ISqlQueryBuilderFactory
    {
        public SqlQueryBuilder Create()
        {
            return new SqlServerQueryBuilder();
        }
    }
}
