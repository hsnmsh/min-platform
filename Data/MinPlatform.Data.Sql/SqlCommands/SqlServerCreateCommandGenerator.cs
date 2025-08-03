namespace MinPlatform.Data.Sql.SqlCommands
{
    using MinPlatform.Data.Service.SqlStatmentBuilder;
    using System.Collections.Generic;
    using System.Text;

    public sealed class SqlServerCreateCommandGenerator : ICreateCommandGenerator
    {
        public string GenerateCommand(IEnumerable<string> columns, string table, IEnumerable<string> parametersName = null)
        {
            var createCommand = new StringBuilder();

            createCommand.Append("INSERT INTO ")
                  .Append(table)
                  .Append(" (")
                  .Append(string.Join(',', columns))
                  .Append(")")
                  .Append(" OUTPUT INSERTED.Id VALUES ")
                  .Append("(")
                  .Append(parametersName != null ? GenerateStatmentParameters(parametersName) : GenerateStatmentParameters(columns))
                  .Append(")");

            return createCommand.ToString();

        }

        private string GenerateStatmentParameters(IEnumerable<string> keys)
        {
            var parameters = new StringBuilder();

            foreach (var column in keys)
            {
                parameters.Append("@" + column + ",");
            }

            parameters.Remove(parameters.Length - 1, 1);

            return parameters.ToString();

        }
    }
}
