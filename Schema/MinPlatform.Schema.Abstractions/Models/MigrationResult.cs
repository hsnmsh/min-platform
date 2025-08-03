namespace MinPlatform.Schema.Abstractions.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class MigrationResult
    {
        private readonly IDictionary<string, MigrationExecutionResult> migrations;

        public MigrationResult()
        {
            migrations = new Dictionary<string, MigrationExecutionResult>();
        }

        public bool Success
        {
            get
            {
                return migrations.All(kv => kv.Value.Success);
            }
        }

        public void AddMigration(string migration, string errorMessage = null)
        {
            migrations.Add(migration, new MigrationExecutionResult
            {
                Name = migration,
                ErrorMessage = errorMessage,
                Success = string.IsNullOrEmpty(errorMessage) ? true : false

            });
        }

        public IDictionary<string, MigrationExecutionResult> GetMigrationResults()
        {
            return migrations;
        }

    }

    public class MigrationExecutionResult
    {
        public string Name
        {
            get;
            internal set;
        }

        public bool Success
        {
            get;
            internal set;
        }

        public string ErrorMessage
        {
            get;
            internal set;
        }
    }
}
