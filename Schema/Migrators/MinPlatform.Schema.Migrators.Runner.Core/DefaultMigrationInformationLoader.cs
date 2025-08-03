#region License
//
// Copyright (c) 2018, Fluent Migrator Project
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
    using MinPlatform.Schema.Migrators.Abstractions.Exceptions;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Runner.Exceptions;
    using MinPlatform.Schema.Migrators.Runner.Initialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DefaultMigrationInformationLoader : IMigrationInformationLoader
    {
        [NotNull, ItemNotNull]
        private readonly string[] tagsToMatch;

        private readonly bool includeUntaggedMigrations;

        [NotNull]
#pragma warning disable 618
        private readonly IMigrationSource _source;
#pragma warning restore 618

        [CanBeNull]
        private SortedList<long, IMigrationInfo> _migrationInfos;

        public DefaultMigrationInformationLoader(
#pragma warning disable 618
            [NotNull] IMigrationSource source,
#pragma warning restore 618
            TypeFilterOptions filterOptions,
            [NotNull] IMigrationRunnerConventions conventions,
            RunnerOptions runnerOptions)
        {
            _source = source;
            Namespace = filterOptions.Namespace;
            LoadNestedNamespaces = filterOptions.NestedNamespaces;
            Conventions = conventions;
            tagsToMatch = runnerOptions.Tags ?? Array.Empty<string>();
            includeUntaggedMigrations = runnerOptions.IncludeUntaggedMigrations;
        }

        [NotNull]
        public IMigrationRunnerConventions Conventions { get; }

        [CanBeNull]
        public string Namespace { get; }

        public bool LoadNestedNamespaces { get; }

        public SortedList<long, IMigrationInfo> LoadMigrations()
        {
            if (_migrationInfos != null)
            {
                if (_migrationInfos.Count == 0)
                    throw new MissingMigrationsException();
                return _migrationInfos;
            }

            _migrationInfos = new SortedList<long, IMigrationInfo>();
            var migrationInfos = FindMigrations(
                _source,
                Conventions,
                Namespace,
                LoadNestedNamespaces,
                tagsToMatch,
                includeUntaggedMigrations);
            foreach (var migrationInfo in migrationInfos)
            {
                if (_migrationInfos.ContainsKey(migrationInfo.Version))
                {
                    throw new DuplicateMigrationException($"Duplicate migration version {migrationInfo.Version}.");
                }

                _migrationInfos.Add(migrationInfo.Version, migrationInfo);
            }

            if (_migrationInfos.Count == 0)
                throw new MissingMigrationsException();

            return _migrationInfos;
        }

        [NotNull, ItemNotNull]
        private static IEnumerable<IMigrationInfo> FindMigrations(
#pragma warning disable 618
            [NotNull] IMigrationSource source,
#pragma warning restore 618
            [NotNull] IMigrationRunnerConventions conventions,
            [CanBeNull] string @namespace,
            bool loadNestedNamespaces,
            [NotNull, ItemNotNull] string[] tagsToMatch,
            bool includeUntagged)
        {
            bool IsMatchingMigration(Type type)
            {
                if (!type.IsInNamespace(@namespace, loadNestedNamespaces))
                    return false;
                if (!conventions.TypeIsMigration(type))
                    return false;
                return conventions.HasRequestedTags(type, tagsToMatch, includeUntagged);
            }

            IReadOnlyCollection<IMigration> migrations;

            if (source is IFilteringMigrationSource filteringSource)
            {
                migrations = filteringSource.GetMigrations(IsMatchingMigration).ToList();
            }
            else
            {
                migrations =
                    (from migration in source.GetMigrations()
                     where IsMatchingMigration(migration.GetType())
                     select migration).ToList();
            }

            if (migrations.Count == 0)
            {
                throw new MissingMigrationsException("No migrations found");
            }

            var migrationInfos = migrations
                .Select(conventions.GetMigrationInfoForMigration)
                .ToList();

            return migrationInfos;
        }
    }
}
