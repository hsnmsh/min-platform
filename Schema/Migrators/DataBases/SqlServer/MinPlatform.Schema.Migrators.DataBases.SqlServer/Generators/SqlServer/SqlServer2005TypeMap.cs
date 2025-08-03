using System.Data;

namespace MinPlatform.Schema.Migrators.DataBases.SqlServer.Generators.SqlServer
{
    public class SqlServer2005TypeMap : SqlServer2000TypeMap
    {
        protected override void SetupSqlServerTypeMaps()
        {
            base.SetupSqlServerTypeMaps();
            // Officially this is 1073741823 but we will allow the int.MaxValue Convention
            SetTypeMap(DbType.String, "NVARCHAR(MAX)", int.MaxValue);
            SetTypeMap(DbType.AnsiString, "VARCHAR(MAX)", AnsiTextCapacity);
            SetTypeMap(DbType.Binary, "VARBINARY(MAX)", ImageCapacity);

            SetTypeMap(DbType.Xml, "XML");
        }
    }
}
