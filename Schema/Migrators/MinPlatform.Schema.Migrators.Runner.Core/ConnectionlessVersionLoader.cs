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

namespace MinPlatform.Schema.Migrators.Runner
{
    using JetBrains.Annotations;
    using MinPlatform.Schema.Migrators.Abstractions;
    using MinPlatform.Schema.Migrators.Abstractions.Expressions;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Abstractions.Model;
    using MinPlatform.Schema.Migrators.Runner.Conventions;
    using MinPlatform.Schema.Migrators.Runner.Generators;
    using MinPlatform.Schema.Migrators.Runner.Initialization;
    using MinPlatform.Schema.Migrators.Runner.Processors;
    using MinPlatform.Schema.Migrators.Runner.Versioning;
    using MinPlatform.Schema.Migrators.Runner.VersionTableInfo;
    using System;
    using System.Collections.Generic;

    public class ConnectionlessVersionLoader : IVersionLoader
    {
        [NotNull]
        private readonly IMigrationProcessor processor;

        [NotNull]
        private readonly IMigrationInformationLoader migrationInformationLoader;

        private readonly IQuoter _quoter;

        private bool _versionsLoaded;

        [Obsolete]
        internal ConnectionlessVersionLoader(
            IGeneratorAccessor generatorAccessor,
            IMigrationRunner runner,
            IAssemblyCollection assemblies,
            IConventionSet conventionSet,
            IMigrationRunnerConventions conventions,
            IRunnerContext runnerContext,
            IVersionTableMetaData versionTableMetaData = null)
        {
            migrationInformationLoader = runner.MigrationLoader;
            processor = runner.Processor;
            _quoter = generatorAccessor.Generator.GetQuoter();

            Runner = runner;
            Assemblies = assemblies;
            Conventions = conventions;
            StartVersion = runnerContext.StartVersion;
            TargetVersion = runnerContext.Version;

            VersionInfo = new VersionInfo();
            VersionTableMetaData = versionTableMetaData ??
                (IVersionTableMetaData)Activator.CreateInstance(assemblies.Assemblies.GetVersionTableMetaDataType(
                    Conventions, runnerContext));
            VersionMigration = new VersionMigration(VersionTableMetaData);
            VersionSchemaMigration = new VersionSchemaMigration(VersionTableMetaData);
            VersionUniqueMigration = new VersionUniqueMigration(VersionTableMetaData);
            VersionDescriptionMigration = new VersionDescriptionMigration(VersionTableMetaData);

            if (VersionTableMetaData is DefaultVersionTableMetaData defaultMetaData)
            {
                conventionSet.SchemaConvention?.Apply(defaultMetaData);
            }

            LoadVersionInfo();
        }

        public ConnectionlessVersionLoader(
            [NotNull] IGeneratorAccessor generatorAccessor,
            [NotNull] IProcessorAccessor processorAccessor,
            [NotNull] IMigrationRunnerConventions conventions,
            RunnerOptions runnerOptions,
            [NotNull] IMigrationInformationLoader migrationInformationLoader,
            [NotNull] IVersionTableMetaData versionTableMetaData)
        {
            processor = processorAccessor.Processor;
            this.migrationInformationLoader = migrationInformationLoader;
            _quoter = generatorAccessor.Generator.GetQuoter();
            Conventions = conventions;
            StartVersion = runnerOptions.StartVersion;
            TargetVersion = runnerOptions.Version;

            VersionInfo = new VersionInfo();
            VersionTableMetaData = versionTableMetaData;
            VersionMigration = new VersionMigration(VersionTableMetaData);
            VersionSchemaMigration = new VersionSchemaMigration(VersionTableMetaData);
            VersionUniqueMigration = new VersionUniqueMigration(VersionTableMetaData);
            VersionDescriptionMigration = new VersionDescriptionMigration(VersionTableMetaData);

            LoadVersionInfo();
        }

        [Obsolete]
        [CanBeNull]
        protected IAssemblyCollection Assemblies { get; set; }

        public IMigrationRunnerConventions Conventions { get; set; }
        public long StartVersion { get; set; }
        public long TargetVersion { get; set; }
        public VersionSchemaMigration VersionSchemaMigration { get; }
        public IMigration VersionMigration { get; }
        public IMigration VersionUniqueMigration { get; }
        public IMigration VersionDescriptionMigration { get; }

        [Obsolete]
        [CanBeNull]
        public IMigrationRunner Runner { get; set; }
        public IVersionInfo VersionInfo { get; set; }
        public IVersionTableMetaData VersionTableMetaData { get; set; }

        public bool AlreadyCreatedVersionSchema
        {
            get
            {
                return string.IsNullOrEmpty(VersionTableMetaData.SchemaName) ||
                       processor.SchemaExists(VersionTableMetaData.SchemaName);
            }
        }

        public bool AlreadyCreatedVersionTable
        {
            get { return processor.TableExists(VersionTableMetaData.SchemaName, VersionTableMetaData.TableName); }
        }

        public void DeleteVersion(long version)
        {
            var expression = new DeleteDataExpression {TableName = VersionTableMetaData.TableName, SchemaName = VersionTableMetaData.SchemaName};
            expression.Rows.Add(new DeletionDataDefinition
            {
                new KeyValuePair<string, object>(VersionTableMetaData.ColumnName, version)
            });
            expression.ExecuteWith(processor);
        }

        public IVersionTableMetaData GetVersionTableMetaData()
        {
            return VersionTableMetaData;
        }

        public void LoadVersionInfo()
        {
            if (_versionsLoaded)
            {
                return;
            }

            foreach (var migration in migrationInformationLoader.LoadMigrations())
            {
                if (migration.Key < StartVersion)
                {
                    VersionInfo.AddAppliedMigration(migration.Key);
                }
            }

            _versionsLoaded = true;
        }

        public void RemoveVersionTable()
        {
            var expression = new DeleteTableExpression {TableName = VersionTableMetaData.TableName, SchemaName = VersionTableMetaData.SchemaName};
            expression.ExecuteWith(processor);

            if (!string.IsNullOrEmpty(VersionTableMetaData.SchemaName))
            {
                var schemaExpression = new DeleteSchemaExpression {SchemaName = VersionTableMetaData.SchemaName};
                schemaExpression.ExecuteWith(processor);
            }
        }

        public void UpdateVersionInfo(long version)
        {
            UpdateVersionInfo(version, null);
        }

        public void UpdateVersionInfo(long version, string description)
        {
            var dataExpression = new InsertDataExpression();
            dataExpression.Rows.Add(CreateVersionInfoInsertionData(version, description));
            dataExpression.TableName = VersionTableMetaData.TableName;
            dataExpression.SchemaName = VersionTableMetaData.SchemaName;

            dataExpression.ExecuteWith(processor);
        }

        protected virtual InsertionDataDefinition CreateVersionInfoInsertionData(long version, string description)
        {
            object appliedOnValue;

            if (_quoter is null)
            {
                appliedOnValue = DateTime.UtcNow;
            }
            else
            {
                var quotedCurrentDate = _quoter.QuoteValue(SystemMethods.CurrentUTCDateTime);

                // Default to using DateTime if no system method could be obtained
                appliedOnValue = string.IsNullOrWhiteSpace(quotedCurrentDate)
                    ? (object) DateTime.UtcNow
                    : RawSql.Insert(quotedCurrentDate);
            }

            return new InsertionDataDefinition
            {
                new KeyValuePair<string, object>(VersionTableMetaData.ColumnName, version),
                new KeyValuePair<string, object>(VersionTableMetaData.AppliedOnColumnName, appliedOnValue),
                new KeyValuePair<string, object>(VersionTableMetaData.DescriptionColumnName, description)
            };
        }
    }
}
