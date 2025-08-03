namespace MinPlatform.Schema.Migrators.Runner.Core.Extensions
{
    using Autofac;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Options;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Runner.Conventions;
    using MinPlatform.Schema.Migrators.Runner.Initialization;
    using MinPlatform.Schema.Migrators.Runner.Processors;
    using MinPlatform.Schema.Migrators.Runner.VersionTableInfo;
    using System;
    using System.Linq;
    using System.Reflection;

    public static class MigrationRunnerBuilderContainerBuilderExtensions
    {

        public static IMigrationRunnerBuilder<ContainerBuilder> ConfigureGlobalProcessorOptions(
            this IMigrationRunnerBuilder<ContainerBuilder> builder,
            Action<ProcessorOptions> configureAction)
        {
            builder.Services.Register(c => configureAction);
            return builder;
        }


        public static IMigrationRunnerBuilder<ContainerBuilder> WithGlobalConnectionString(
            this IMigrationRunnerBuilder<ContainerBuilder> builder,
            string connectionStringOrName)
        {
            builder.Services.RegisterType<ProcessorOptions>().OnActivating(e =>
            {
                var instance = e.Instance;
                instance.ConnectionString = connectionStringOrName;
            });

            return builder;
        }


        public static IMigrationRunnerBuilder<ContainerBuilder> WithGlobalConnectionString(
            this IMigrationRunnerBuilder<ContainerBuilder> builder, Func<IComponentContext, string> configureConnectionString)
        {
            builder.Services
                .RegisterType<IConfigureOptions<ProcessorOptions>>().OnActivating(
                    s =>
                    {

                        new ConfigureNamedOptions<ProcessorOptions>
                        (
                            Options.DefaultName,
                            opt => opt.ConnectionString = configureConnectionString(s.Context)
                        );

                    }).SingleInstance();

            return builder;
        }


        public static IMigrationRunnerBuilder<ContainerBuilder> WithGlobalCommandTimeout(
            this IMigrationRunnerBuilder<ContainerBuilder> builder,
            TimeSpan commandTimeout)
        {
            builder.Services.RegisterType<ProcessorOptions>().OnActivating(s => s.Instance.Timeout = commandTimeout);

            return builder;
        }


        public static IMigrationRunnerBuilder<ContainerBuilder> WithGlobalStripComments(
            this IMigrationRunnerBuilder<ContainerBuilder> builder,
            bool stripComments)
        {
            builder.Services.RegisterType<ProcessorOptions>().OnActivating(s => s.Instance.StripComments = stripComments);

            return builder;
        }


        public static IMigrationRunnerBuilder<ContainerBuilder> AsGlobalPreview(
            this IMigrationRunnerBuilder<ContainerBuilder> builder,
            bool preview = true)
        {
            builder.Services.RegisterType<ProcessorOptions>().OnActivating(s => s.Instance.PreviewOnly = preview);

            return builder;
        }


        public static IMigrationRunnerBuilder<ContainerBuilder> WithVersionTable(
            this IMigrationRunnerBuilder<ContainerBuilder> builder,
            IVersionTableMetaData versionTableMetaData)
        {
            builder.Services.RegisterType<IVersionTableMetaDataAccessor>()
                .OnActivating(_=> new PassThroughVersionTableMetaDataAccessor(versionTableMetaData))
                .InstancePerLifetimeScope();

            return builder;
        }


        public static IMigrationRunnerBuilder<ContainerBuilder> WithRunnerConventions(
            this IMigrationRunnerBuilder<ContainerBuilder> builder,
            IMigrationRunnerConventions conventions)
        {
            builder.Services.RegisterType<IMigrationRunnerConventionsAccessor>()
                .OnActivating(_ => new PassThroughMigrationRunnerConventionsAccessor(conventions))
                .SingleInstance();

            return builder;
        }

        public static IMigrationRunnerBuilder<ContainerBuilder> WithMigrationsIn(
            this IMigrationRunnerBuilder<ContainerBuilder> builder,
            [NotNull, ItemNotNull] params Assembly[] assemblies)
        {
            builder.Services
                .Register<IMigrationSourceItem>(_=> new AssemblyMigrationSourceItem(assemblies))
                .SingleInstance();

            return builder;
        }


        public static IScanInBuilder<ContainerBuilder> ScanIn(
            this IMigrationRunnerBuilder<ContainerBuilder> builder,
            [NotNull, ItemNotNull] params Assembly[] assemblies)
        {
            var sourceItem = new AssemblySourceItem(assemblies);
            return new ScanInBuilder(builder, sourceItem);
        }

        private class ScanInBuilder : IScanInBuilder<ContainerBuilder>, IScanInForBuilder<ContainerBuilder>
        {
            private readonly IMigrationRunnerBuilder<ContainerBuilder> builder;

            public ScanInBuilder(IMigrationRunnerBuilder<ContainerBuilder> builder, IAssemblySourceItem currentSourceItem)
            {
                if (builder.DanglingAssemblySourceItem != null)
                {
                    builder.Services.Register(_=> builder.DanglingAssemblySourceItem).SingleInstance();

                }

                this.builder = builder;
                this.builder.DanglingAssemblySourceItem = currentSourceItem;
                SourceItem = currentSourceItem;
            }

            private ScanInBuilder(
                IMigrationRunnerBuilder<ContainerBuilder> builder,
                IAssemblySourceItem currentSourceItem,
                IMigrationSourceItem sourceItem)
            {
                this.builder = builder;
                SourceItem = currentSourceItem;

                this.builder.DanglingAssemblySourceItem = null;

                Services.Register(_=> sourceItem).SingleInstance();
            }

            private ScanInBuilder(
                IMigrationRunnerBuilder<ContainerBuilder> builder,
                IAssemblySourceItem currentSourceItem,
                IVersionTableMetaDataSourceItem sourceItem)
            {
                this.builder = builder;
                SourceItem = currentSourceItem;

                this.builder.DanglingAssemblySourceItem = null;
                Services.Register(_ => sourceItem).SingleInstance();
            }

            private ScanInBuilder(
                IMigrationRunnerBuilder<ContainerBuilder> builder,
                IAssemblySourceItem currentSourceItem,
                ITypeSourceItem<IConventionSet> sourceItem)
            {
                this.builder = builder;
                SourceItem = currentSourceItem;

                this.builder.DanglingAssemblySourceItem = null;
                Services.Register(_ => sourceItem).SingleInstance();
            }

            private ScanInBuilder(
                IMigrationRunnerBuilder<ContainerBuilder> builder,
                IAssemblySourceItem currentSourceItem,
                IEmbeddedResourceProvider sourceItem)
            {
                this.builder = builder;
                SourceItem = currentSourceItem;

                this.builder.DanglingAssemblySourceItem = null;
                Services.Register(_ => sourceItem).SingleInstance();
            }

            public ContainerBuilder Services => builder.Services;

            public IAssemblySourceItem DanglingAssemblySourceItem
            {
                get => builder.DanglingAssemblySourceItem;
                set => builder.DanglingAssemblySourceItem = value;
            }

            public IAssemblySourceItem SourceItem { get; }

            public IScanInForBuilder<ContainerBuilder> For => this;

            public IScanInBuilder<ContainerBuilder> Migrations()
            {
                var sourceItem = new AssemblyMigrationSourceItem(SourceItem.Assemblies.ToList());
                return new ScanInBuilder(builder, SourceItem, sourceItem);
            }

            public IScanInBuilder<ContainerBuilder> VersionTableMetaData()
            {
                var sourceItem = new AssemblyVersionTableMetaDataSourceItem(SourceItem.Assemblies.ToArray());
                return new ScanInBuilder(builder, SourceItem, sourceItem);
            }

            public IScanInBuilder<ContainerBuilder> ConventionSet()
            {
                var sourceItem = new AssemblySourceItem<IConventionSet>(SourceItem.Assemblies.ToArray());
                return new ScanInBuilder(builder, SourceItem, sourceItem);
            }

            public IScanInBuilder<ContainerBuilder> EmbeddedResources()
            {
                var sourceItem = new DefaultEmbeddedResourceProvider(SourceItem.Assemblies.ToArray());
                return new ScanInBuilder(builder, SourceItem, sourceItem);
            }

            public IMigrationRunnerBuilder<ContainerBuilder> All()
            {
                Services.Register(_ => SourceItem).SingleInstance();
                builder.DanglingAssemblySourceItem = null;
                return builder;
            }
        }
    }
}
