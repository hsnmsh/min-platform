namespace MinPlatform.Data.Service.SqlStatmentBuilder
{
    using System.Collections.Generic;

    public interface IUpdateCommandGenerator
    {
        public string GenerateCommand(IEnumerable<string> columns, string table, IEnumerable<string> parametersName = null);
    }
}
