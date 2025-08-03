#region License
//
// Copyright (c) 2007-2024, Fluent Migrator Project
// Copyright (c) 2011, Grant Archibald
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace MinPlatform.Schema.Migrators.Builders.IfDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MinPlatform.Schema.Migrators.Abstractions;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Alter;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Delete;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Execute;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.IfDatabase;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Insert;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Rename;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Schema;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Update;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Builders.Alter;
    using MinPlatform.Schema.Migrators.Builders.Create;
    using MinPlatform.Schema.Migrators.Builders.Delete;
    using MinPlatform.Schema.Migrators.Builders.Execute;
    using MinPlatform.Schema.Migrators.Builders.Insert;
    using MinPlatform.Schema.Migrators.Builders.Rename;
    using MinPlatform.Schema.Migrators.Builders.Schema;
    using MinPlatform.Schema.Migrators.Builders.Update;
    using MinPlatform.Schema.Migrators.Infrastructure;

    /// <summary>
    /// Allows for conditional inclusion of expressions based on the migration context
    /// </summary>
    public class IfDatabaseExpressionRoot : IIfDatabaseExpressionRoot
    {
        /// <summary>
        /// The context to add expressions into
        /// </summary>
        /// <remarks>If the database type doe snot apply then this will be a new context that is not used by the caller</remarks>
        private readonly IMigrationContext context;

        private readonly IEnumerable<IEmbeddedResourceProvider> embeddedResources;

        /// <summary>
        /// Initializes a new instance of a the <see cref="IfDatabaseExpressionRoot"/> class that will only add expressions to the provided <paramref name="context"/> if <paramref name="databaseType"/> matches the migration processor
        /// </summary>
        /// <remarks>If the database type does not apply then a <seealso cref="NullIfDatabaseProcessor"/> will be used as a container to void any fluent expressions that would have been executed</remarks>
        /// <param name="context">The context to add expressions to if the database type applies</param>
        /// <param name="databaseType">The database type that the expressions relate to</param>
        public IfDatabaseExpressionRoot(IMigrationContext context, IEnumerable<IEmbeddedResourceProvider> embeddedResources, params string[] databaseType)
        {
            if (databaseType == null) throw new ArgumentNullException(nameof(databaseType));

            this.embeddedResources = embeddedResources ?? throw new ArgumentNullException(nameof(embeddedResources));

            this.context = DatabaseTypeApplies(context, databaseType)
                ? context
                : CreateEmptyMigrationContext(context);
        }

        /// <summary>
        /// Initializes a new instance of a the <see cref="IfDatabaseExpressionRoot"/> class that will only add expressions to the provided <paramref name="context"/> if <paramref name="databaseTypePredicate"/> is true for the migration processor
        /// </summary>
        /// <remarks>If the database type does not apply then a <seealso cref="NullIfDatabaseProcessor"/> will be used as a container to void any fluent expressions that would have been executed</remarks>
        /// <param name="context">The context to add expressions to if the database type applies</param>
        /// <param name="databaseTypePredicate">The predicate that must be true for the expression to run</param>
        public IfDatabaseExpressionRoot(IMigrationContext context, Predicate<string> databaseTypePredicate)
        {
            if (databaseTypePredicate == null) throw new ArgumentNullException(nameof(databaseTypePredicate));

            this.context = DatabaseTypeApplies(context, databaseTypePredicate)
                ? context
                : CreateEmptyMigrationContext(context);
        }

        /// <summary>
        /// Alter the schema of an existing object
        /// </summary>
        public IAlterExpressionRoot Alter
        {
            get { return new AlterExpressionRoot(context); }
        }

        /// <summary>
        /// Create a new database object
        /// </summary>
        public ICreateExpressionRoot Create
        {
            get { return new CreateExpressionRoot(context); }
        }

        /// <summary>
        /// Delete a database object, table, or row
        /// </summary>
        public IDeleteExpressionRoot Delete
        {
            get { return new DeleteExpressionRoot(context); }
        }

        /// <summary>
        /// Rename tables / columns
        /// </summary>
        public IRenameExpressionRoot Rename
        {
            get { return new RenameExpressionRoot(context); }
        }

        /// <summary>
        /// Insert data into a table
        /// </summary>
        public IInsertExpressionRoot Insert
        {
            get { return new InsertExpressionRoot(context); }
        }

        /// <summary>
        /// Execute SQL statements
        /// </summary>
        public IExecuteExpressionRoot Execute
        {
            get { return new ExecuteExpressionRoot(context, embeddedResources); }
        }

        /// <inheritdoc />
        public ISchemaExpressionRoot Schema
        {
            get { return new SchemaExpressionRoot(context); }
        }

        /// <summary>
        /// Update an existing row
        /// </summary>
        public IUpdateExpressionRoot Update
        {
            get { return new UpdateExpressionRoot(context); }
        }

        /// <inheritdoc />
        public void Delegate(Action delegation)
        {
            if (context.QuerySchema is NullIfDatabaseProcessor)
            {
                return;
            }

            delegation.Invoke();
        }

        /// <summary>
        /// Checks if the database type matches the name of the context migration processor
        /// </summary>
        /// <param name="context">The context to evaluate</param>
        /// <param name="databaseTypes">The type to be checked</param>
        /// <returns><c>True</c> if the database type applies, <c>False</c> if not</returns>
        private static bool DatabaseTypeApplies(IMigrationContext context, params string[] databaseTypes)
        {
            if (context.QuerySchema is IMigrationProcessor mp)
            {
                var processorDbTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { mp.DatabaseType };
                foreach (var databaseType in mp.DatabaseTypeAliases)
                    processorDbTypes.Add(databaseType);

                return databaseTypes
                    .Any(db => processorDbTypes.Contains(db));
            }

            return false;
        }

        /// <summary>
        /// Checks if the database type matches the name of the context migration processor
        /// </summary>
        /// <param name="context">The context to evaluate</param>
        /// <param name="databaseTypePredicate">The predicate to be evaluated</param>
        /// <returns><c>True</c> if the database type applies, <c>False</c> if not</returns>
        private static bool DatabaseTypeApplies(IMigrationContext context, Predicate<string> databaseTypePredicate)
        {
            if (context.QuerySchema is IMigrationProcessor mp)
            {
                return databaseTypePredicate(mp.DatabaseType);
            }

            return false;
        }

        private static IMigrationContext CreateEmptyMigrationContext(IMigrationContext originalContext)
        {
            var result = new MigrationContext(
                new NullIfDatabaseProcessor(),
                string.Empty);
            return result;
        }
    }
}
