namespace MinPlatform.Schema.Migrators.Runner.Core
{
    using MinPlatform.Logging.Service;
    using MinPlatform.Schema.Abstractions.Models;
    using MinPlatform.Schema.Builder;
    using MinPlatform.Schema.Migrators.Abstractions;
    using MinPlatform.Schema.Migrators.Abstractions.Expressions;
    using MinPlatform.Schema.Migrators.Infrastructure;
    using MinPlatform.Schema.Migrators.Runner.Initialization;
    using System;
    using System.Collections.Generic;

    internal sealed class SchemaBuilderMigratorEngine : ISchemaBuilderMigratorEngine
    {
        private readonly IConnectionStringAccessor connectionStringAccessor;
        private readonly IQuerySchema querySchema;
        private readonly IStopWatch stopWatch;
        private readonly IMigrationProcessor migrationProcessor;
        private readonly MigrationValidator migrationValidator;
        private readonly LoggerManager logger;
        private readonly IList<Exception> exceptions;

        public SchemaBuilderMigratorEngine
        (
            IQuerySchema querySchema,
            IConnectionStringAccessor connectionStringAccessor,
            IStopWatch stopWatch,
            IMigrationProcessor migrationProcessor,
            MigrationValidator migrationValidator,
            LoggerManager logger
        )
        {
            
            this.migrationValidator = migrationValidator;
            this.stopWatch = stopWatch;
            this.logger = logger;
            this.migrationProcessor = migrationProcessor;
            this.querySchema = querySchema;
            this.connectionStringAccessor = connectionStringAccessor;

            exceptions = new List<Exception>();
        }

        public void ApplyMigrationUp(DataModel model)
        {
            var migrationContext = new MigrationContext(querySchema, connectionStringAccessor.ConnectionString);
            var schemaBuilderMigration = new SchemaBuilderMigration(model);
            schemaBuilderMigration.GetUpExpressions(migrationContext);

            migrationValidator.ApplyConventionsToAndValidateExpressions(schemaBuilderMigration, migrationContext.Expressions);
            ExecuteExpressions(migrationContext.Expressions);
        }

        public void ApplyMigrationDown(DataModel model)
        {
            var migrationContext = new MigrationContext(querySchema, connectionStringAccessor.ConnectionString);
            var schemaBuilderMigration = new SchemaBuilderMigration(model);
            schemaBuilderMigration.GetDownExpressions(migrationContext);

            migrationValidator.ApplyConventionsToAndValidateExpressions(schemaBuilderMigration, migrationContext.Expressions);
            ExecuteExpressions(migrationContext.Expressions);
        }

        private void ExecuteExpressions(ICollection<IMigrationExpression> expressions, bool silentlyFail = true)
        {
            long insertTicks = 0;
            var insertCount = 0;

            foreach (IMigrationExpression expression in expressions)
            {
                try
                {
                    if (expression is InsertDataExpression)
                    {
                        insertTicks += stopWatch.Time(() => expression.ExecuteWith(migrationProcessor)).Ticks;
                        insertCount++;
                    }
                    else
                    {
                        expression.ExecuteWith(migrationProcessor);
                        logger.Information(expression.ToString());
                    }
                }
                catch (Exception er)
                {
                    logger.Error(er.Message, er);

                    //catch the error and move onto the next expression
                    if (silentlyFail)
                    {
                        exceptions.Add(er);
                        continue;
                    }

                    throw;
                }
            }

            if (insertCount > 0)
            {
                var avg = new TimeSpan(insertTicks / insertCount);
                var msg = string.Format("-> {0} Insert operations completed in {1} taking an average of {2}", insertCount, new TimeSpan(insertTicks), avg);
                logger.Information(msg);
            }
        }


    }
}
