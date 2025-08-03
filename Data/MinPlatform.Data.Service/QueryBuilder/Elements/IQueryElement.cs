namespace MinPlatform.Data.Service.QueryBuilder.Elements
{
    public interface IQueryElement
    {
        void Accept(SqlQueryBuilder sqlQueryBuilder);
    }
}
