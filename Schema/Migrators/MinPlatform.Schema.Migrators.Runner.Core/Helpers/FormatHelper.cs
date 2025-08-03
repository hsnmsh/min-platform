
namespace MinPlatform.Schema.Migrators.Runner.Helpers
{
    public class FormatHelper
    {
        public static string FormatSqlEscape(string sql)
        {
            return sql.Replace("'", "''");
        }
    }
}
