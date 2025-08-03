namespace MinPlatform.Data.Sql.SqlCommands
{
    using MinPlatform.Data.Service.SqlStatmentBuilder;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public sealed class SqlServerUpdateCommandGenerator : IUpdateCommandGenerator
    {
        public string GenerateCommand(IEnumerable<string> columns, string table, IEnumerable<string> parametersName = null)
        {
            var updateCommand = new StringBuilder();

            updateCommand.Append("UPDATE ")
                   .Append(table)
                   .Append(" SET ")
                   .Append(GenerateUpdateStatmentParameters(columns, parametersName))
                   .Append(" OUTPUT INSERTED.Id WHERE  Id = @id");

            return updateCommand.ToString();
        }

        private string GenerateUpdateStatmentParameters(IEnumerable<string> keys, IEnumerable<string> parametersName = null)
        {
            var parameters = new StringBuilder();

            if (parametersName is null)
            {
                foreach (var column in keys)
                {
                    if (column.ToLower() != "id")
                    {
                        parameters.Append(column + "=" + "@" + column + ",");
                    }
                }
            }
            else
            {
                for (int i = 0; i < keys.Count(); i++)
                {
                    if (keys.ElementAt(i).ToLower() != "id")
                    {
                        parameters.Append(keys.ElementAt(i) + "=" + "@" + parametersName.ElementAt(i) + ",");
                    }
                }
            }

            parameters.Remove(parameters.Length - 1, 1);

            return parameters.ToString();

        }
    }
}
