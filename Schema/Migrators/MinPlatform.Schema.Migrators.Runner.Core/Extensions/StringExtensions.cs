namespace MinPlatform.Schema.Migrators.Runner.Core.Extensions
{
    public static class StringExtensions
    {
        public static string[] SplitToSchemaAndTableName(this string tableName)
        {
            return tableName.Split(".");
        }
    }
}
