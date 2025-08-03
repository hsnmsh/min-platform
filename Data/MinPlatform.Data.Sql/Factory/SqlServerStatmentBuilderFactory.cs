namespace MinPlatform.Data.Sql.Factory
{
    using MinPlatform.Data.Service.SqlStatmentBuilder;
    using MinPlatform.Data.Sql.SqlCommands;

    public sealed class SqlServerStatmentBuilderFactory : ISqlStatmentBuilderFactory
    {
        public SqlStatmentBuilderCommands Create()
        {
            return new SqlStatmentBuilderCommands()
            {
                CreateCommandGenerator = new SqlServerCreateCommandGenerator(),
                UpdateCommandGenerator = new SqlServerUpdateCommandGenerator(),
                DeleteCommandGenerator = new SqlServerDeleteCommandGenerator(),
            };
        }
    }
}
