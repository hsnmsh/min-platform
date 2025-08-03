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
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using MinPlatform.Schema.Migrators.Runner.Versioning;
    using MinPlatform.Schema.Migrators.Runner.VersionTableInfo;
    using MinPlatform.Schema.Migrators.Runner.Conventions;
    using MinPlatform.Schema.Migrators.Runner.Generators;
    using MinPlatform.Schema.Migrators.Runner.Initialization;
    using MinPlatform.Schema.Migrators.Runner.Processors;
    using JetBrains.Annotations;
    using MinPlatform.Schema.Migrators.Abstractions;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Abstractions.Expressions;
    using MinPlatform.Schema.Migrators.Abstractions.Model;

    public class VersionLoader : IVersionLoader
    {
        [NotNull]
        private readonly IMigrationProcessor processor;

        private readonly IConventionSet conventionSet;
        private readonly IQuoter quoter;
        private bool versionSchemaMigrationAlreadyRun;
        private bool versionMigrationAlreadyRun;
        private bool versionUniqueMigrationAlreadyRun;
        private bool versionDescriptionMigrationAlreadyRun;
        private IVersionInfo versionInfo;
        private IMigrationRunnerConventions Conventions { get; set; }

        [CanBeNull]
        [Obsolete]
        protected IAssemblyCollection Assemblies { get; set; }

        public IVersionTableMetaData VersionTableMetaData { get; }

        [NotNull]
        public IMigrationRunner Runner { get; set; }
        public VersionSchemaMigration VersionSchemaMigration { get; }
        public IMigration VersionMigration { get; }
        public IMigration VersionUniqueMigration { get; }
        public IMigration VersionDescriptionMigration { get; }

        [Obsolete]
        internal VersionLoader(
            [NotNull] IMigrationRunner runner,
            [NotNull] Assembly assembly,
            [NotNull] IGeneratorAccessor generatorAccessor,
            [NotNull] IConventionSet conventionSet,
            [NotNull] IMigrationRunnerConventions conventions,
            [NotNull] IRunnerContext runnerContext)
            : this(runner, new SingleAssembly(assembly), generatorAccessor, conventionSet, conventions, runnerContext)
        {
        }

        [Obsolete]
        internal VersionLoader(IMigrationRunner runner, IAssemblyCollection assemblies,
            [NotNull] IGeneratorAccessor generatorAccessor,
            [NotNull] IConventionSet conventionSet,
            [NotNull] IMigrationRunnerConventions conventions,
            [NotNull] IRunnerContext runnerContext,
            [CanBeNull] IVersionTableMetaData versionTableMetaData = null)
        {
            this.conventionSet = conventionSet;
            processor = runner.Processor;
            quoter = generatorAccessor.Generator.GetQuoter();

            Runner = runner;
            Assemblies = assemblies;

            Conventions = conventions;
            VersionTableMetaData = versionTableMetaData ?? CreateVersionTableMetaData(runnerContext);
            VersionMigration = new VersionMigration(VersionTableMetaData);
            VersionSchemaMigration = new VersionSchemaMigration(VersionTableMetaData);
            VersionUniqueMigration = new VersionUniqueMigration(VersionTableMetaData);
            VersionDescriptionMigration = new VersionDescriptionMigration(VersionTableMetaData);

            LoadVersionInfo();
        }

        public VersionLoader(
            [NotNull] IProcessorAccessor processorAccessor,
            [NotNull] IGeneratorAccessor generatorAccessor,
            [NotNull] IConventionSet conventionSet,
            [NotNull] IMigrationRunnerConventions conventions,
            [NotNull] IVersionTableMetaData versionTableMetaData,
            [NotNull] IMigrationRunner runner)
        {
            this.conventionSet = conventionSet;
            processor = processorAccessor.Processor;
            quoter = generatorAccessor.Generator.GetQuoter();

            Runner = runner;

            Conventions = conventions;
            VersionTableMetaData = versionTableMetaData;
            VersionMigration = new VersionMigration(VersionTableMetaData);
            VersionSchemaMigration = new VersionSchemaMigration(VersionTableMetaData);
            VersionUniqueMigration = new VersionUniqueMigration(VersionTableMetaData);
            VersionDescriptionMigration = new VersionDescriptionMigration(VersionTableMetaData);

            LoadVersionInfo();
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

        [NotNull]
        public IVersionTableMetaData GetVersionTableMetaData()
        {
            return VersionTableMetaData;
        }

        protected virtual InsertionDataDefinition CreateVersionInfoInsertionData(long version, string description)
        {
            object appliedOnValue;

            if (quoter is null)
            {
                appliedOnValue = DateTime.UtcNow;
            }
            else
            {
                var quotedCurrentDate = quoter.QuoteValue(SystemMethods.CurrentUTCDateTime);

                // Default to using DateTime if no system method could be obtained
                appliedOnValue = string.IsNullOrWhiteSpace(quotedCurrentDate)
                    ? (object) DateTime.UtcNow
                    : RawSql.Insert(quotedCurrentDate);
            }

            return new InsertionDataDefinition
                       {
                           new KeyValuePair<string, object>(VersionTableMetaData.ColumnName, version),
                           new KeyValuePair<string, object>(VersionTableMetaData.AppliedOnColumnName, appliedOnValue),
                           new KeyValuePair<string, object>(VersionTableMetaData.DescriptionColumnName, description),
                       };
        }

        public IVersionInfo VersionInfo
        {
            get => versionInfo;
            set => versionInfo = value ?? throw new ArgumentException("Cannot set VersionInfo to null");
        }

        public bool AlreadyCreatedVersionSchema => string.IsNullOrEmpty(VersionTableMetaData.SchemaName) ||
            processor.SchemaExists(VersionTableMetaData.SchemaName);

        public bool AlreadyCreatedVersionTable => processor.TableExists(VersionTableMetaData.SchemaName, VersionTableMetaData.TableName);

        public bool AlreadyMadeVersionUnique => processor.ColumnExists(VersionTableMetaData.SchemaName, VersionTableMetaData.TableName, VersionTableMetaData.AppliedOnColumnName);

        public bool AlreadyMadeVersionDescription => processor.ColumnExists(VersionTableMetaData.SchemaName, VersionTableMetaData.TableName, VersionTableMetaData.DescriptionColumnName);

        public bool OwnsVersionSchema => VersionTableMetaData.OwnsSchema;

        public void LoadVersionInfo()
        {
            if (!AlreadyCreatedVersionSchema && !versionSchemaMigrationAlreadyRun)
            {
                Runner.Up(VersionSchemaMigration);
                versionSchemaMigrationAlreadyRun = true;
            }

            if (!AlreadyCreatedVersionTable && !versionMigrationAlreadyRun)
            {
                Runner.Up(VersionMigration);
                versionMigrationAlreadyRun = true;
            }

            if (!AlreadyMadeVersionUnique && !versionUniqueMigrationAlreadyRun)
            {
                Runner.Up(VersionUniqueMigration);
                versionUniqueMigrationAlreadyRun = true;
            }

            if (!AlreadyMadeVersionDescription && !versionDescriptionMigrationAlreadyRun)
            {
                Runner.Up(VersionDescriptionMigration);
                versionDescriptionMigrationAlreadyRun = true;
            }

            versionInfo = new VersionInfo();

            if (!AlreadyCreatedVersionTable) return;

            var dataSet = processor.ReadTableData(VersionTableMetaData.SchemaName, VersionTableMetaData.TableName);
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                versionInfo.AddAppliedMigration(long.Parse(row[VersionTableMetaData.ColumnName].ToString()));
            }
        }

        public void RemoveVersionTable()
        {
            var expression = new DeleteTableExpression { TableName = VersionTableMetaData.TableName, SchemaName = VersionTableMetaData.SchemaName };
            expression.ExecuteWith(processor);

            if (OwnsVersionSchema && !string.IsNullOrEmpty(VersionTableMetaData.SchemaName))
            {
                var schemaExpression = new DeleteSchemaExpression { SchemaName = VersionTableMetaData.SchemaName };
                schemaExpression.ExecuteWith(processor);
            }
        }

        public void DeleteVersion(long version)
        {
            var expression = new DeleteDataExpression { TableName = VersionTableMetaData.TableName, SchemaName = VersionTableMetaData.SchemaName };
            expression.Rows.Add(new DeletionDataDefinition
                                    {
                                        new KeyValuePair<string, object>(VersionTableMetaData.ColumnName, version)
                                    });
            expression.ExecuteWith(processor);
        }

        [Obsolete]
        [NotNull]
        private IVersionTableMetaData CreateVersionTableMetaData(IRunnerContext runnerContext)
        {
            var type = Assemblies?.Assemblies.GetVersionTableMetaDataType(Conventions, runnerContext)
             ?? typeof(DefaultVersionTableMetaData);

            var instance = (IVersionTableMetaData) Activator.CreateInstance(type);
            if (instance is ISchemaExpression schemaExpression)
            {
                conventionSet.SchemaConvention?.Apply(schemaExpression);
            }

            return instance;
        }
    }
}
