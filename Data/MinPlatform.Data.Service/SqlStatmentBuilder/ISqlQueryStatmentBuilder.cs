namespace MinPlatform.Data.Service.SqlStatmentBuilder
{

    public interface ISqlQueryStatmentBuilder
    {
        string GenerateSelectByIdStatment(string tableName);

    }
}
