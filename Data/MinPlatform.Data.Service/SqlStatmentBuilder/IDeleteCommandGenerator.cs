namespace MinPlatform.Data.Service.SqlStatmentBuilder
{
    public interface IDeleteCommandGenerator
    {
        string CreateCommand(string tableName, bool deleteMultiple);
    }
}
