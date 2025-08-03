namespace MinPlatform.Schema.Builder
{
    using MinPlatform.Schema.Abstractions.Models;

    public interface ISchemaBuilderMigratorEngine
    {
        void ApplyMigrationUp(DataModel model);

        void ApplyMigrationDown(DataModel model);
    }
}
