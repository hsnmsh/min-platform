namespace MinPlatform.Data.Sql
{
    using MinPlatform.Data.Service.SqlStatmentBuilder;
    using System.Text;

    public sealed class SqlServerQueryStatmentBuilder : ISqlQueryStatmentBuilder
    {
        public string GenerateSelectByIdStatment(string tableName)
        {
            var selectStatmentBuilder = new StringBuilder();

            selectStatmentBuilder.Append("SELECT DISTINCT * FROM ").Append(tableName).Append(" WHERE Id=@id");

            return selectStatmentBuilder.ToString();
        }
    }
}
