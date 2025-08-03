
namespace MinPlatform.Schema.Migrators.Runner.Core.Extensions
{
    using Autofac;
    using JetBrains.Annotations;
    using MinPlatform.Logging.Serilog.Extensions;
    using MinPlatform.Logging.Service.Extensions;
    using MinPlatform.Schema.Builder;
    using MinPlatform.Schema.Migrators.Abstractions;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Abstractions.Validation;
    using MinPlatform.Schema.Migrators.Infrastructure;
    using MinPlatform.Schema.Migrators.Runner;
    using MinPlatform.Schema.Migrators.Runner.Conventions;
    using MinPlatform.Schema.Migrators.Runner.Generators;
    using MinPlatform.Schema.Migrators.Runner.Initialization;
    using MinPlatform.Schema.Migrators.Runner.Initialization.AssemblyLoader;
    using MinPlatform.Schema.Migrators.Runner.Initialization.NetFramework;
    using MinPlatform.Schema.Migrators.Runner.Processors;
    using System;

    public static class SchemaMigratorContainerBuilderExtensions
    {
        [NotNull]
        public static ContainerBuilder AddSchemaMigratorCore(
          [NotNull] this ContainerBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            // Add logging support
            builder.AddSerilogLoggerResolver();
            builder.AddSerilogSystemLoggerFactory();
            builder.AddLoggingContext();

            //TODO: add custom logger
            //services
            //  // Add support for options
            //  .AddOptions()


            // The default assembly loader factory
            builder
                .RegisterType<AssemblyLoaderFactory>()
                .SingleInstance()
                .IfNotRegistered(typeof(AssemblyLoaderFactory));

            // Assembly loader engines
            builder.RegisterType<AssemblyNameLoadEngine>().As<IAssemblyLoadEngine>().SingleInstance();
            builder.RegisterType<AssemblyFileLoadEngine>().As<IAssemblyLoadEngine>().SingleInstance();

            // Defines the assemblies that are used to find migrations, profiles, maintenance code, etc...
            builder
                .RegisterType<AssemblySource>()
                .As<IAssemblySource>()
                .SingleInstance()
                .IfNotRegistered(typeof(IAssemblySource));

            // Configure the loader for migrations that should be executed during maintenance steps
            builder
             .RegisterType<MaintenanceLoader>()
             .As<IMaintenanceLoader>()
             .SingleInstance()
             .IfNotRegistered(typeof(IMaintenanceLoader));

            // Add the default embedded resource provider
            builder
             .Register(container => new DefaultEmbeddedResourceProvider(container.Resolve<IAssemblySource>().Assemblies))
             .As<IEmbeddedResourceProvider>()
             .SingleInstance();

            // Configure the runner conventions

            builder
             .RegisterType<AssemblySourceMigrationRunnerConventionsAccessor>()
             .As<IMigrationRunnerConventionsAccessor>()
             .SingleInstance()
             .IfNotRegistered(typeof(IMaintenanceLoader));

            builder
            .Register(container => container.Resolve<IMigrationRunnerConventionsAccessor>().MigrationRunnerConventions)
            .SingleInstance()
            .IfNotRegistered(typeof(IMaintenanceLoader));

            // The IStopWatch implementation used to show query timing
            builder
            .RegisterType<StopWatch>()
            .As<IStopWatch>()
            .SingleInstance();

            // Source for migrations

            builder
            .RegisterType<MigrationSource>()
#pragma warning disable 618

            .As<IMigrationSource>()
            .SingleInstance();

            builder
            .Register(container => container.Resolve<IMigrationSource>() as IFilteringMigrationSource)
            .SingleInstance()
            .IfNotRegistered(typeof(IFilteringMigrationSource));

#pragma warning restore 618

            // Source for profiles

            builder
            .RegisterType<ProfileSource>()
            .As<IProfileSource>()
            .SingleInstance();

            // Configure the accessor for the convention set

            builder
            .RegisterType<AssemblySourceConventionSetAccessor>()
            .As<IConventionSetAccessor>()
            .SingleInstance();

            // The default set of conventions to be applied to migration expressions

            builder
            .Register(container => container.Resolve<IConventionSetAccessor>().GetConventionSet())
            .InstancePerLifetimeScope();

            // Configure the accessor for the version table metadata

            builder
             .RegisterType<AssemblySourceVersionTableMetaDataAccessor>()
             .As<IVersionTableMetaDataAccessor>()
             .InstancePerLifetimeScope()
             .IfNotRegistered(typeof(AssemblySourceVersionTableMetaDataAccessor));

            // Configure the default version table metadata
            builder
            .Register(container => container.Resolve<IVersionTableMetaDataAccessor>().VersionTableMetaData)
            .InstancePerLifetimeScope();

            // Configure the migration information loader

            builder
            .RegisterType<DefaultMigrationInformationLoader>()
            .As<IMigrationInformationLoader>()
            .InstancePerLifetimeScope()
            .IfNotRegistered(typeof(DefaultMigrationInformationLoader));

            // Provide a way to get the migration generator selected by its options
            builder
            .RegisterType<SelectingGeneratorAccessor>()
            .As<IGeneratorAccessor>()
            .InstancePerLifetimeScope()
            .IfNotRegistered(typeof(SelectingGeneratorAccessor));

            // Provide a way to get the migration accessor selected by its options

            builder
            .RegisterType<SelectingProcessorAccessor>()
            .As<IProcessorAccessor>()
            .InstancePerLifetimeScope()
            .IfNotRegistered(typeof(SelectingProcessorAccessor));

            // IQuerySchema is the base interface for the IMigrationProcessor
            builder
            .Register(container => container.Resolve<IProcessorAccessor>().Processor)
            .As<IQuerySchema>()
            .InstancePerLifetimeScope()
            .IfNotRegistered(typeof(SelectingProcessorAccessor));

            // The profile loader needed by the migration runner
            builder
            .RegisterType<ProfileLoader>()
            .As<IProfileLoader>()
            .InstancePerLifetimeScope()
            .IfNotRegistered(typeof(ProfileLoader));

            // Some services especially for the migration runner implementation
            builder
            .RegisterType<DefaultMigrationExpressionValidator>()
            .As<IMigrationExpressionValidator>()
            .InstancePerLifetimeScope()
            .IfNotRegistered(typeof(DefaultMigrationExpressionValidator));

            builder
            .RegisterType<MigrationValidator>()
            .InstancePerLifetimeScope();

            builder
            .RegisterType<MigrationScopeHandler>()
            .InstancePerLifetimeScope();

            // Register ProcessorOptions explicitly, required by MigrationScopeHandler
            builder
            .Register(container => container.Resolve<ProcessorOptions>())
            .InstancePerLifetimeScope();

            // The connection string accessor that evaluates the readers

            builder
              .RegisterType<ConnectionStringAccessor>()
              .As<IConnectionStringAccessor>()
              .InstancePerLifetimeScope()
              .IfNotRegistered(typeof(ConnectionStringAccessor));



            builder
                .Register<IVersionLoader>(
                    container =>
                    {
                        var options = container.Resolve<RunnerOptions>();
                        var connAccessor = container.Resolve<IConnectionStringAccessor>();
                        var hasConnection = !string.IsNullOrEmpty(connAccessor.ConnectionString);
                        if (options.NoConnection || !hasConnection)
                        {
                            return container.Resolve<ConnectionlessVersionLoader>();
                        }

                        return container.Resolve<VersionLoader>();
                    });


            // Configure the runner
            builder
              .RegisterType<MigrationRunner>()
              .As<IMigrationRunner>()
              .InstancePerLifetimeScope()
              .IfNotRegistered(typeof(MigrationRunner));

            // Configure the task executor
            builder
              .RegisterType<TaskExecutorContainerBuilder>()
              .InstancePerLifetimeScope()
              .IfNotRegistered(typeof(TaskExecutorContainerBuilder));

            // Migration context

            builder
                .Register<IMigrationContext>(
                    container =>
                    {
                        var querySchema = container.Resolve<IQuerySchema>();
                        var options = container.Resolve<RunnerOptions>();
                        var connectionStringAccessor = container.Resolve<IConnectionStringAccessor>();
                        var connectionString = connectionStringAccessor.ConnectionString;
                        return new MigrationContext(querySchema, connectionString);
                    });

            builder
                .RegisterType<SchemaBuilderMigratorEngine>()
                .As<ISchemaBuilderMigratorEngine>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<SchemaBuilderManager>()
                .InstancePerDependency();

            return builder;
        }

        public static ContainerBuilder AddConventionSet(this ContainerBuilder builder, string defaultSchema = null)
        {
            builder
                .Register(ctx => new DefaultConventionSet(defaultSchema, null))
                .As<IConventionSet>().SingleInstance();

            return builder;
        }

        /// <summary>
        /// Configures the migration runner
        /// </summary>
        /// <param name="builder">The <see cref="ContainerBuilder" /> to add services to.</param>
        /// <param name="configure">The <see cref="IMigrationRunnerBuilder"/> configuration delegate.</param>
        /// <returns>The updated service collection</returns>
        public static ContainerBuilder ConfigureRunner(
            [NotNull] this ContainerBuilder builder,
            [NotNull] Action<IMigrationRunnerBuilder<ContainerBuilder>> configure)
        {
            var migrationRunnerBuilder = new MigrationRunnerBuilder(builder);
            configure.Invoke(migrationRunnerBuilder);

            if (migrationRunnerBuilder.DanglingAssemblySourceItem != null)
            {
                migrationRunnerBuilder.Services
                    .RegisterInstance(migrationRunnerBuilder.DanglingAssemblySourceItem)
                    .As<IAssemblySourceItem>()
                    .InstancePerLifetimeScope();
            }

            return builder;
        }

        /// <summary>
        /// Creates services for a given runner context, connection string provider and assembly loader factory.
        /// </summary>
        /// <param name="runnerContext">The runner context</param>
        /// <param name="connectionStringProvider">The connection string provider</param>
        /// <param name="defaultAssemblyLoaderFactory">The assembly loader factory</param>
        /// <param name="configureRunner">The runner builder config deletage</param>
        /// <returns>The new service collection</returns>
        [NotNull]
        [Obsolete]
        internal static ContainerBuilder CreateServices(
            [NotNull] this IRunnerContext runnerContext,
            [CanBeNull] IConnectionStringProvider connectionStringProvider,
            [CanBeNull] AssemblyLoaderFactory defaultAssemblyLoaderFactory = null,
            [CanBeNull] Action<IMigrationRunnerBuilder<ContainerBuilder>> configureRunner = null)
        {
            var containerBuilder = new ContainerBuilder();
            var assemblyLoaderFactory = defaultAssemblyLoaderFactory ?? new AssemblyLoaderFactory();

            if (!runnerContext.NoConnection && connectionStringProvider == null)
            {
                runnerContext.NoConnection = true;
            }

            // Configure the migration runner
            containerBuilder
                .AddSchemaMigratorCore()
                .ConfigureRunner(c => configureRunner?.Invoke(c));

            containerBuilder.RegisterType<SelectingProcessorAccessorOptions>().OnActivating(e =>
            {
                var instance = e.Instance;
                instance.ProcessorId = runnerContext.Database; // Assuming runnerContext.Database holds the value you want
            }).SingleInstance();


            containerBuilder.Register(container => assemblyLoaderFactory)
                .As<AssemblyLoaderFactory>()
                .SingleInstance();

            containerBuilder.RegisterType<TypeFilterOptions>().OnActivating(e =>
            {
                var instance = e.Instance;
                instance.Namespace = runnerContext.Namespace;
                instance.NestedNamespaces = runnerContext.NestedNamespaces;

            }).SingleInstance();

            containerBuilder.RegisterType<AssemblySourceOptions>().OnActivating(e =>
            {
                var instance = e.Instance;
                instance.AssemblyNames = runnerContext.Targets;

            }).SingleInstance();

            containerBuilder.RegisterType<RunnerOptions>().OnActivating(e =>
            {
                var instance = e.Instance;
                instance.SetValuesFrom(runnerContext);

            }).SingleInstance();

            containerBuilder.RegisterType<ProcessorOptions>().OnActivating(e =>
            {
                var instance = e.Instance;
                instance.SetValuesFrom(runnerContext);

            }).SingleInstance();

            containerBuilder.RegisterType<AppConfigConnectionStringAccessorOptions>().OnActivating(e =>
            {
                var instance = e.Instance;
                instance.ConnectionStringConfigPath = runnerContext.ConnectionStringConfigPath;

            }).SingleInstance();

            // Configure the processor
            if (runnerContext.NoConnection)
            {
                // Always return the connectionless processor
                containerBuilder
                   .RegisterType<ConnectionlessProcessorAccessor>()
                   .As<IProcessorAccessor>()
                   .InstancePerLifetimeScope()
                   .IfNotRegistered(typeof(ConnectionlessProcessorAccessor));
            }

            return containerBuilder;
        }

        private class MigrationRunnerBuilder : IMigrationRunnerBuilder<ContainerBuilder>
        {
            public MigrationRunnerBuilder(ContainerBuilder builder)
            {
                Services = builder;
                DanglingAssemblySourceItem = null;
            }

            /// <inheritdoc />
            public ContainerBuilder Services { get; }

            /// <inheritdoc />
            public IAssemblySourceItem DanglingAssemblySourceItem { get; set; }
        }

        [UsedImplicitly]
        private class ConnectionlessProcessorAccessor : IProcessorAccessor
        {
            public ConnectionlessProcessorAccessor(IComponentContext componentContext)
            {
                Processor = componentContext.Resolve<ConnectionlessProcessor>();
            }

            /// <inheritdoc />
            public IMigrationProcessor Processor { get; }
        }
    }


}
