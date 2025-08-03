#region License
// Copyright (c) 2018, Fluent Migrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace MinPlatform.Schema.Migrators.Runner.Core.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using MinPlatform.Schema.Migrators.Runner.Conventions;
    using MinPlatform.Schema.Migrators.Runner.Initialization;
    using MinPlatform.Schema.Migrators.Runner.Processors;
    using MinPlatform.Schema.Migrators.Runner.VersionTableInfo;
    using JetBrains.Annotations;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;

    /// <summary>
    /// Extension methods for the <see cref="IMigrationRunnerBuilder"/> interface
    /// </summary>
    public static class MigrationRunnerBuilderServiceCollectionExtensions
    {
        
        public static IMigrationRunnerBuilder<IServiceCollection> ConfigureGlobalProcessorOptions(
            this IMigrationRunnerBuilder<IServiceCollection> builder,
            Action<ProcessorOptions> configureAction)
        {
            builder.Services.Configure(configureAction);
            return builder;
        }

        
        public static IMigrationRunnerBuilder<IServiceCollection> WithGlobalConnectionString(
            this IMigrationRunnerBuilder<IServiceCollection> builder,
            string connectionStringOrName)
        {
            builder.Services.Configure<ProcessorOptions>(opt => opt.ConnectionString = connectionStringOrName);
            return builder;
        }

        
        public static IMigrationRunnerBuilder<IServiceCollection> WithGlobalConnectionString(
            this IMigrationRunnerBuilder<IServiceCollection> builder, Func<IServiceProvider, string> configureConnectionString)
        {
            builder.Services
                .AddSingleton<IConfigureOptions<ProcessorOptions>>(
                    s =>
                    {
                        return new ConfigureNamedOptions<ProcessorOptions>(
                            Options.DefaultName,
                            opt => opt.ConnectionString = configureConnectionString(s));
                    });

            return builder;
        }

        
        public static IMigrationRunnerBuilder<IServiceCollection> WithGlobalCommandTimeout(
            this IMigrationRunnerBuilder<IServiceCollection> builder,
            TimeSpan commandTimeout)
        {
            builder.Services.Configure<ProcessorOptions>(opt => opt.Timeout = commandTimeout);
            return builder;
        }

        
        public static IMigrationRunnerBuilder<IServiceCollection> WithGlobalStripComments(
            this IMigrationRunnerBuilder<IServiceCollection> builder,
            bool stripComments)
        {
            builder.Services.Configure<ProcessorOptions>(opt => opt.StripComments = stripComments);
            return builder;
        }

        
        public static IMigrationRunnerBuilder<IServiceCollection> AsGlobalPreview(
            this IMigrationRunnerBuilder<IServiceCollection> builder,
            bool preview = true)
        {
            builder.Services.Configure<ProcessorOptions>(opt => opt.PreviewOnly = preview);
            return builder;
        }

        
        public static IMigrationRunnerBuilder<IServiceCollection> WithVersionTable(
            this IMigrationRunnerBuilder<IServiceCollection> builder,
            IVersionTableMetaData versionTableMetaData)
        {
            builder.Services
                .AddScoped<IVersionTableMetaDataAccessor>(
                    _ => new PassThroughVersionTableMetaDataAccessor(versionTableMetaData));
            return builder;
        }

        
        public static IMigrationRunnerBuilder<IServiceCollection> WithRunnerConventions(
            this IMigrationRunnerBuilder<IServiceCollection> builder,
            IMigrationRunnerConventions conventions)
        {
            builder.Services
                .AddSingleton<IMigrationRunnerConventionsAccessor>(
                    new PassThroughMigrationRunnerConventionsAccessor(conventions));
            return builder;
        }

        
        public static IMigrationRunnerBuilder<IServiceCollection> WithMigrationsIn(
            this IMigrationRunnerBuilder<IServiceCollection> builder,
            [NotNull, ItemNotNull] params Assembly[] assemblies)
        {
            builder.Services
                .AddSingleton<IMigrationSourceItem>(new AssemblyMigrationSourceItem(assemblies));
            return builder;
        }

        
        public static IScanInBuilder<IServiceCollection> ScanIn(
            this IMigrationRunnerBuilder<IServiceCollection> builder,
            [NotNull, ItemNotNull] params Assembly[] assemblies)
        {
            var sourceItem = new AssemblySourceItem(assemblies);
            return new ScanInBuilder(builder, sourceItem);
        }

        private class ScanInBuilder : IScanInBuilder<IServiceCollection>, IScanInForBuilder<IServiceCollection>
        {
            private readonly IMigrationRunnerBuilder<IServiceCollection> builder;

            public ScanInBuilder(IMigrationRunnerBuilder<IServiceCollection> builder, IAssemblySourceItem currentSourceItem)
            {
                if (builder.DanglingAssemblySourceItem != null)
                {
                    builder.Services
                        .AddSingleton(builder.DanglingAssemblySourceItem);
                }

                this.builder = builder;
                this.builder.DanglingAssemblySourceItem = currentSourceItem;
                SourceItem = currentSourceItem;
            }

            private ScanInBuilder(
                IMigrationRunnerBuilder<IServiceCollection> builder,
                IAssemblySourceItem currentSourceItem,
                IMigrationSourceItem sourceItem)
            {
                this.builder = builder;
                SourceItem = currentSourceItem;

                this.builder.DanglingAssemblySourceItem = null;
                Services.AddSingleton(sourceItem);
            }

            private ScanInBuilder(
                IMigrationRunnerBuilder<IServiceCollection> builder,
                IAssemblySourceItem currentSourceItem,
                IVersionTableMetaDataSourceItem sourceItem)
            {
                this.builder = builder;
                SourceItem = currentSourceItem;

                this.builder.DanglingAssemblySourceItem = null;
                Services.AddSingleton(sourceItem);
            }

            private ScanInBuilder(
                IMigrationRunnerBuilder<IServiceCollection> builder,
                IAssemblySourceItem currentSourceItem,
                ITypeSourceItem<IConventionSet> sourceItem)
            {
                this.builder = builder;
                SourceItem = currentSourceItem;

                this.builder.DanglingAssemblySourceItem = null;
                Services.AddSingleton(sourceItem);
            }

            private ScanInBuilder(
                IMigrationRunnerBuilder<IServiceCollection> builder,
                IAssemblySourceItem currentSourceItem,
                IEmbeddedResourceProvider sourceItem)
            {
                this.builder = builder;
                SourceItem = currentSourceItem;

                this.builder.DanglingAssemblySourceItem = null;
                Services.AddSingleton(sourceItem);
            }

            /// <inheritdoc />
            public IServiceCollection Services => builder.Services;

            /// <inheritdoc />
            public IAssemblySourceItem DanglingAssemblySourceItem
            {
                get => builder.DanglingAssemblySourceItem;
                set => builder.DanglingAssemblySourceItem = value;
            }

            /// <inheritdoc />
            public IAssemblySourceItem SourceItem { get; }

            /// <inheritdoc />
            public IScanInForBuilder<IServiceCollection> For => this;

            /// <inheritdoc />
            public IScanInBuilder<IServiceCollection> Migrations()
            {
                var sourceItem = new AssemblyMigrationSourceItem(SourceItem.Assemblies.ToList());
                return new ScanInBuilder(builder, SourceItem, sourceItem);
            }

            /// <inheritdoc />
            public IScanInBuilder<IServiceCollection> VersionTableMetaData()
            {
                var sourceItem = new AssemblyVersionTableMetaDataSourceItem(SourceItem.Assemblies.ToArray());
                return new ScanInBuilder(builder, SourceItem, sourceItem);
            }

            /// <inheritdoc />
            public IScanInBuilder<IServiceCollection> ConventionSet()
            {
                var sourceItem = new AssemblySourceItem<IConventionSet>(SourceItem.Assemblies.ToArray());
                return new ScanInBuilder(builder, SourceItem, sourceItem);
            }

            /// <inheritdoc />
            public IScanInBuilder<IServiceCollection> EmbeddedResources()
            {
                var sourceItem = new DefaultEmbeddedResourceProvider(SourceItem.Assemblies.ToArray());
                return new ScanInBuilder(builder, SourceItem, sourceItem);
            }

            /// <inheritdoc />
            public IMigrationRunnerBuilder<IServiceCollection> All()
            {
                Services.AddSingleton(SourceItem);
                builder.DanglingAssemblySourceItem = null;
                return builder;
            }
        }
    }
}
