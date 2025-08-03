

namespace MinPlatform.Schema.Migrators.Runner.Initialization
{
    using Autofac;
    using JetBrains.Annotations;
    using MinPlatform.Logging.Service;
    using MinPlatform.Schema.Migrators.Abstractions.Infrastructure;
    using MinPlatform.Schema.Migrators.Runner.Core.Extensions;
    using MinPlatform.Schema.Migrators.Runner.Initialization.AssemblyLoader;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class TaskExecutorContainerBuilder
    {
        [NotNull]
        private readonly LoggerManager logger;

        [NotNull]
        private readonly IAssemblySource assemblySource;

        private readonly RunnerOptions runnerOptions;

        [NotNull, ItemNotNull]
        private readonly Lazy<IComponentContext> lazyComponentContext;

        private IReadOnlyCollection<Assembly> assemblies;

        public TaskExecutorContainerBuilder(
            LoggerManager logger,
            [NotNull] IAssemblySource assemblySource,
            RunnerOptions runnerOptions,
            [NotNull] IComponentContext componentContext)
        {
            this.logger = logger;
            this.assemblySource = assemblySource;
            this.runnerOptions = runnerOptions;
#pragma warning disable 612
            ConnectionStringProvider = componentContext.Resolve<IConnectionStringProvider>();
#pragma warning restore 612
            lazyComponentContext = new Lazy<IComponentContext>(() => componentContext);
        }

        [Obsolete]
        public TaskExecutorContainerBuilder([NotNull] IRunnerContext runnerContext,
            [CanBeNull] Action<IMigrationRunnerBuilder<ContainerBuilder>> configureRunner = null)
        {
            var runnerCtxt = runnerContext ?? throw new ArgumentNullException(nameof(runnerContext));
            runnerOptions = new RunnerOptions(runnerCtxt);
            var asmLoaderFactory = new AssemblyLoaderFactory();
            assemblySource = new AssemblySource(() => new AssemblyCollection(asmLoaderFactory.GetTargetAssemblies(runnerCtxt.Targets)));
            ConnectionStringProvider = new DefaultConnectionStringProvider();
            lazyComponentContext = new Lazy<IComponentContext>(
                () => runnerContext
                    .CreateServices(
                        ConnectionStringProvider,
                        asmLoaderFactory,
                        configureRunner)
                    .Build());
        }

        [Obsolete]
        public TaskExecutorContainerBuilder(
            [NotNull] IRunnerContext runnerContext,
            [NotNull] AssemblyLoaderFactory assemblyLoaderFactory,
            [CanBeNull] IConnectionStringProvider connectionStringProvider = null,
            [CanBeNull] Action<IMigrationRunnerBuilder<ContainerBuilder>> configureRunner = null)
        {
            var runnerCtxt = runnerContext ?? throw new ArgumentNullException(nameof(runnerContext));
            runnerOptions = new RunnerOptions(runnerCtxt);
            ConnectionStringProvider = connectionStringProvider;
            var asmLoaderFactory = assemblyLoaderFactory ?? throw new ArgumentNullException(nameof(assemblyLoaderFactory));
            assemblySource = new AssemblySource(() => new AssemblyCollection(asmLoaderFactory.GetTargetAssemblies(runnerCtxt.Targets)));
            lazyComponentContext = new Lazy<IComponentContext>(
                () => runnerContext
                    .CreateServices(
                        connectionStringProvider,
                        asmLoaderFactory,
                        configureRunner).Build());
        }

        /// <summary>
        /// Gets the current migration runner
        /// </summary>
        /// <remarks>
        /// This will only be set during a migration operation
        /// </remarks>
        [CanBeNull]
        protected IMigrationRunner Runner { get; set; }

        /// <summary>
        /// Gets the connection string provider
        /// </summary>
        [CanBeNull]
        [Obsolete]
        protected IConnectionStringProvider ConnectionStringProvider { get; }

        /// <summary>
        /// Gets the service provider used to instantiate all migration services
        /// </summary>
        [NotNull]
        protected IComponentContext ComponentContext => lazyComponentContext.Value;

        [Obsolete]
        protected virtual IEnumerable<Assembly> GetTargetAssemblies()
        {
            return assemblies ?? (assemblies = assemblySource.Assemblies);
        }

        /// <summary>
        /// Will be called during the runner scope initialization
        /// </summary>
        /// <remarks>
        /// The <see cref="Runner"/> isn't initialized yet.
        /// </remarks>
        protected virtual void Initialize()
        {
        }

        public void Execute()
        {
            using (var scope = new RunnerScope(this))
            {
                switch (runnerOptions.Task)
                {
                    case null:
                    case "":
                    case "migrate":
                    case "migrate:up":
                        if (runnerOptions.Version != 0)
                            scope.Runner.MigrateUp(runnerOptions.Version);
                        else
                            scope.Runner.MigrateUp();
                        break;
                    case "rollback":
                        if (runnerOptions.Steps == 0)
                            runnerOptions.Steps = 1;
                        scope.Runner.Rollback(runnerOptions.Steps);
                        break;
                    case "rollback:toversion":
                        scope.Runner.RollbackToVersion(runnerOptions.Version);
                        break;
                    case "rollback:all":
                        scope.Runner.RollbackToVersion(0);
                        break;
                    case "migrate:down":
                        scope.Runner.MigrateDown(runnerOptions.Version);
                        break;
                    case "validateversionorder":
                        scope.Runner.ValidateVersionOrder();
                        break;
                    case "listmigrations":
                        scope.Runner.ListMigrations();
                        break;
                }
            }

            logger.Information("Task completed.");
        }

        /// <summary>
        /// Checks whether the current task will actually run any migrations.
        /// Can be used to decide whether it's necessary perform a backup before the migrations are executed.
        /// </summary>
        public bool HasMigrationsToApply()
        {
            using (var scope = new RunnerScope(this))
            {
                switch (runnerOptions.Task)
                {
                    case null:
                    case "":
                    case "migrate":
                    case "migrate:up":
                        if (runnerOptions.Version != 0)
                            return scope.Runner.HasMigrationsToApplyUp(runnerOptions.Version);

                        return scope.Runner.HasMigrationsToApplyUp();
                    case "rollback":
                    case "rollback:all":
                        // Number of steps doesn't matter as long as there's at least
                        // one migration applied (at least that one will be rolled back)
                        return scope.Runner.HasMigrationsToApplyRollback();
                    case "rollback:toversion":
                    case "migrate:down":
                        return scope.Runner.HasMigrationsToApplyDown(runnerOptions.Version);
                    default:
                        return false;
                }
            }
        }

        private class RunnerScope : IDisposable
        {
            [NotNull]
            private readonly TaskExecutorContainerBuilder executor;

            [CanBeNull]
            private readonly ILifetimeScope lifetimeScope;

            private readonly bool _hasCustomRunner;

            public RunnerScope([NotNull] TaskExecutorContainerBuilder executor)
            {
                this.executor = executor;

                executor.Initialize();

                if (executor.Runner != null)
                {
                    Runner = executor.Runner;
                    _hasCustomRunner = true;
                }
                else
                {
                    var serviceScope = executor.ComponentContext as ILifetimeScope;

                    if(serviceScope != null)
                    {
                        this.lifetimeScope = serviceScope;
                    }
                    executor.Runner = Runner = serviceScope.Resolve<IMigrationRunner>();
                }
            }

            public IMigrationRunner Runner { get; }

            public void Dispose()
            {
                if (_hasCustomRunner)
                {
                    Runner.Processor.Dispose();
                }
                else
                {
                    executor.Runner = null;
                    lifetimeScope?.Dispose();
                }
            }
        }
    }
}
