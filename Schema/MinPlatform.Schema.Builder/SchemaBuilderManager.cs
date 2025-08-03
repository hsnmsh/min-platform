namespace MinPlatform.Schema.Builder
{
    using MinPlatform.Schema.Abstractions.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public sealed class SchemaBuilderManager
    {
        private readonly ISchemaBuilderMigratorEngine schemaBuilderEngine;

        public SchemaBuilderManager(ISchemaBuilderMigratorEngine schemaBuilderEngine)
        {
            this.schemaBuilderEngine = schemaBuilderEngine;
        }

        public MigrationResult MigrateUp()
        {
            var migrationResult = new MigrationResult();

            IEnumerable<BaseSchemaMigrator> migrationConfigs = Assembly
                                                                .GetEntryAssembly()
                                                                .GetTypes()
                                                                .Where(t => t.IsSubclassOf(typeof(BaseSchemaMigrator)) && !t.IsAbstract && t.IsPublic)
                                                                .Select(t => (BaseSchemaMigrator)Activator.CreateInstance(t));


            if (migrationConfigs != null && migrationConfigs.Any())
            {
                foreach (var migrationConfig in migrationConfigs)
                {
                    var schemaBuilder = new SchemaBuilder();
                    migrationConfig.Up(schemaBuilder);

                    DataModel dataModel = schemaBuilder.Build();

                    try
                    {
                        //check in the database if the migration exist , if exist (dont run it)
                        schemaBuilderEngine.ApplyMigrationUp(dataModel);
                    }
                    catch (Exception ex)
                    {
                        migrationResult.AddMigration(migrationConfig.Name, ex.Message);
                    }
                }

            }

            IDictionary<string, MigrationExecutionResult> migrationResults = migrationResult.GetMigrationResults();

            foreach (var migrationresult in migrationResults)
            {
                if (migrationresult.Value.Success)
                {
                    //register migrator in DataBase
                }
            }

            return migrationResult;

        }

    }
}
