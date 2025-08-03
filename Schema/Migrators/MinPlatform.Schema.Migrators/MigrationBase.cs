#region License
//
// Copyright (c) 2007-2024, Fluent Migrator Project
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

namespace MinPlatform.Schema.Migrators
{
    using System;
    using System.Collections.Generic;
    using MinPlatform.Schema.Migrators.Abstractions;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Alter;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Create;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.IfDatabase;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Insert;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Rename;
    using MinPlatform.Schema.Migrators.Abstractions.Builders.Schema;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Builders.Alter;
    using MinPlatform.Schema.Migrators.Builders.Create;
    using MinPlatform.Schema.Migrators.Builders.IfDatabase;
    using MinPlatform.Schema.Migrators.Builders.Insert;
    using MinPlatform.Schema.Migrators.Builders.Rename;
    using MinPlatform.Schema.Migrators.Builders.Schema;

    /// <summary>
    /// The base migration class
    /// </summary>
    public abstract class MigrationBase : IMigration
    {
        /// <summary>
        /// Gets or sets the migration context
        /// </summary>
        [Obsolete("Use the Context property instead")]
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once MemberCanBePrivate.Global
        internal IMigrationContext _context;

        protected IEnumerable<IEmbeddedResourceProvider> embeddedResources;

        private readonly object mutex = new object();

        /// <inheritdoc />
        public string ConnectionString { get; protected set; }

        /// <summary>
        /// Gets the migration context
        /// </summary>
#pragma warning disable 618
        internal IMigrationContext Context => _context ?? throw new InvalidOperationException("The context is not set");
#pragma warning restore 618

        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public abstract void Up();

        /// <summary>
        /// Collects the DOWN migration expressions
        /// </summary>
        public abstract void Down();

        /// <inheritdoc />
        public virtual void GetUpExpressions(IMigrationContext context)
        {
            lock (mutex)
            {
#pragma warning disable 618
                _context = context;
#pragma warning restore 618
                ConnectionString = context.Connection;
                Up();
#pragma warning disable 618
                _context = null;
#pragma warning restore 618
            }
        }

        /// <inheritdoc />
        public virtual void GetDownExpressions(IMigrationContext context)
        {
            lock (mutex)
            {
#pragma warning disable 618
                _context = context;
#pragma warning restore 618
                ConnectionString = context.Connection;
                Down();
#pragma warning disable 618
                _context = null;
#pragma warning restore 618
            }
        }

        /// <summary>
        /// Gets the starting point for alterations
        /// </summary>
        public IAlterExpressionRoot Alter
        {
            get { return new AlterExpressionRoot(Context); }
        }

        /// <summary>
        /// Gets the starting point for creating database objects
        /// </summary>
        public ICreateExpressionRoot Create
        {
            get { return new CreateExpressionRoot(Context); }
        }

        /// <summary>
        /// Gets the starting point for renaming database objects
        /// </summary>
        public IRenameExpressionRoot Rename
        {
            get { return new RenameExpressionRoot(Context); }
        }

        /// <summary>
        /// Gets the starting point for data insertion
        /// </summary>
        public IInsertExpressionRoot Insert
        {
            get { return new InsertExpressionRoot(Context); }
        }

        /// <summary>
        /// Gets the starting point for schema-rooted expressions
        /// </summary>
        public ISchemaExpressionRoot Schema
        {
            get { return new SchemaExpressionRoot(Context); }
        }

        /// <summary>
        /// Gets the starting point for database specific expressions
        /// </summary>
        /// <param name="databaseType">The supported database types</param>
        /// <returns>The database specific expression</returns>
        public IIfDatabaseExpressionRoot IfDatabase(params string[] databaseType)
        {
            return new IfDatabaseExpressionRoot(Context, embeddedResources, databaseType);
        }

        /// <summary>
        /// Gets the starting point for database specific expressions
        /// </summary>
        /// <param name="databaseTypeFunc">The lambda that tests if the expression can be applied to the current database</param>
        /// <returns>The database specific expression</returns>
        public IIfDatabaseExpressionRoot IfDatabase(Predicate<string> databaseTypeFunc)
        {
            return new IfDatabaseExpressionRoot(Context, databaseTypeFunc);
        }
    }
}
