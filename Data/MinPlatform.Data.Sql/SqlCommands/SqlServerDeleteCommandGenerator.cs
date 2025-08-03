namespace MinPlatform.Data.Sql.SqlCommands
{
    using MinPlatform.Data.Service.SqlStatmentBuilder;
    using System.Text;

    public sealed class SqlServerDeleteCommandGenerator : IDeleteCommandGenerator
    {
        public string CreateCommand(string tableName, bool deleteMultiple)
        {
            var updateCommand = new StringBuilder();

            updateCommand.Append("DELETE FROM ")
                   .Append(tableName)
                   .Append(!deleteMultiple ? " OUTPUT DELETED.Id " : " ")
                   .Append(" WHERE Id ")
                   .Append(deleteMultiple ? " IN @id " : "= @id");

            return updateCommand.ToString();
        }
    }
}
