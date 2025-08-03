namespace MinPlatform.Data.Service.SqlStatmentBuilder
{
    using System.Collections.Generic;

    public interface ICreateCommandGenerator
    {
        string GenerateCommand(IEnumerable<string> columns, string table, IEnumerable<string> parametersName = null);
    }
}
