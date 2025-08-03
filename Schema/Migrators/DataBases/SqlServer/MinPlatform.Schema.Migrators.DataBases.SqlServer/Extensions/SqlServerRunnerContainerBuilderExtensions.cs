using Autofac;
using Microsoft.Extensions.DependencyInjection;
using MinPlatform.Schema.Migrators.Abstractions;
using MinPlatform.Schema.Migrators.DataBases.SqlServer.BatchParser;
using MinPlatform.Schema.Migrators.DataBases.SqlServer.Generators.SqlServer;
using MinPlatform.Schema.Migrators.DataBases.SqlServer.Processors.SqlServer;
using MinPlatform.Schema.Migrators.Runner;
using MinPlatform.Schema.Migrators.Runner.Generators;
using MinPlatform.Schema.Migrators.Runner.Processors;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinPlatform.Schema.Migrators.DataBases.SqlServer.Extensions
{
    public static class SqlServerRunnerContainerBuilderExtensions
    {
        public static IMigrationRunnerBuilder<ContainerBuilder> AddSqlServer(this IMigrationRunnerBuilder<ContainerBuilder> builder)
        {
            builder.Services
                .RegisterType<SqlServerBatchParser>()
                .InstancePerDependency();

            builder.Services
                .RegisterType<SqlServer2008Quoter>()
                .InstancePerLifetimeScope()
                .IfNotRegistered(typeof(SqlServer2008Quoter));

            builder.Services
                .Register(e => new SqlServer2008TypeMap())
                .As<ISqlServerTypeMap>()
                .InstancePerLifetimeScope()
                .IfNotRegistered(typeof(SqlServer2008TypeMap));

            builder.Services
                .RegisterType<SqlServer2016Processor>()
                .InstancePerLifetimeScope();

            builder.Services
                .Register(ctx => ctx.Resolve<SqlServer2016Processor>())
                .As<IMigrationProcessor>()
                .InstancePerLifetimeScope();

            builder.Services
                .RegisterType<SqlServer2016Generator>()
                .InstancePerLifetimeScope();

            builder.Services
                .Register(ctx => ctx.Resolve<SqlServer2016Generator>())
                .As<IMigrationGenerator>()
                .InstancePerLifetimeScope();

            return builder;
        }

        public static IMigrationRunnerBuilder<ContainerBuilder> AddSqlServer2000(this IMigrationRunnerBuilder<ContainerBuilder> builder)
        {
            builder.Services
              .RegisterType<SqlServerBatchParser>()
              .InstancePerDependency()
              .IfNotRegistered(typeof(SqlServerBatchParser));

            builder.Services
              .RegisterType<SqlServerBatchParser>()
              .InstancePerDependency()
              .IfNotRegistered(typeof(SqlServerBatchParser));

            builder.Services
              .RegisterType<SqlServer2000Quoter>()
              .InstancePerLifetimeScope()
              .IfNotRegistered(typeof(SqlServer2000Quoter));


            builder.Services
               .RegisterType<SqlServer2000Processor>()
               .InstancePerLifetimeScope();

            builder.Services
                .Register(e => new SqlServer2000TypeMap())
                .As<ISqlServerTypeMap>()
                .InstancePerLifetimeScope()
                .IfNotRegistered(typeof(SqlServer2000TypeMap));

            builder.Services
                .Register(ctx => ctx.Resolve<SqlServer2000Processor>())
                .As<IMigrationProcessor>()
                .InstancePerLifetimeScope();

            builder.Services
                .RegisterType<SqlServer2000Generator>()
                .InstancePerLifetimeScope();

            builder.Services
                .Register(ctx => ctx.Resolve<SqlServer2000Generator>())
                .As<IMigrationGenerator>()
                .InstancePerLifetimeScope();

            return builder;
        }

        public static IMigrationRunnerBuilder<ContainerBuilder> AddSqlServer2005(this IMigrationRunnerBuilder<ContainerBuilder> builder)
        {
            builder.Services
              .RegisterType<SqlServerBatchParser>()
              .InstancePerDependency()
              .IfNotRegistered(typeof(SqlServerBatchParser));

            builder.Services
              .RegisterType<SqlServerBatchParser>()
              .InstancePerDependency()
              .IfNotRegistered(typeof(SqlServerBatchParser));

            builder.Services
              .RegisterType<SqlServer2005Quoter>()
              .InstancePerLifetimeScope()
              .IfNotRegistered(typeof(SqlServer2005Quoter));


            builder.Services
               .RegisterType<SqlServer2005Processor>()
               .InstancePerLifetimeScope();

            builder.Services
               .Register(e => new SqlServer2005TypeMap())
               .As<ISqlServerTypeMap>()
               .InstancePerLifetimeScope()
               .IfNotRegistered(typeof(SqlServer2005TypeMap));

            builder.Services
               .Register(ctx => ctx.Resolve<SqlServer2005Processor>())
               .As<IMigrationProcessor>()
               .InstancePerLifetimeScope();

            builder.Services
               .RegisterType<SqlServer2005Generator>()
               .InstancePerLifetimeScope();

            builder.Services
               .Register(ctx => ctx.Resolve<SqlServer2005Generator>())
               .As<IMigrationGenerator>()
               .InstancePerLifetimeScope();

            return builder;
        }

        public static IMigrationRunnerBuilder<ContainerBuilder> AddSqlServer2008(this IMigrationRunnerBuilder<ContainerBuilder> builder)
        {
            builder.Services
              .RegisterType<SqlServerBatchParser>()
              .InstancePerDependency()
              .IfNotRegistered(typeof(SqlServerBatchParser));

            builder.Services
              .RegisterType<SqlServerBatchParser>()
              .InstancePerDependency()
              .IfNotRegistered(typeof(SqlServerBatchParser));

            builder.Services
              .RegisterType<SqlServer2008Quoter>()
              .InstancePerLifetimeScope()
              .IfNotRegistered(typeof(SqlServer2008Quoter));


            builder.Services
               .RegisterType<SqlServer2008Processor>()
               .InstancePerLifetimeScope();

            builder.Services
               .Register(e => new SqlServer2008TypeMap())
               .As<ISqlServerTypeMap>()
               .InstancePerLifetimeScope()
               .IfNotRegistered(typeof(SqlServer2008TypeMap));

            builder.Services
               .Register(ctx => ctx.Resolve<SqlServer2008Processor>())
               .As<IMigrationProcessor>()
               .InstancePerLifetimeScope();

            builder.Services
               .RegisterType<SqlServer2008Generator>()
               .InstancePerLifetimeScope();

            builder.Services
               .Register(ctx => ctx.Resolve<SqlServer2008Generator>())
               .As<IMigrationGenerator>()
               .InstancePerLifetimeScope();


            return builder;
        }

        public static IMigrationRunnerBuilder<ContainerBuilder> AddSqlServer2012(this IMigrationRunnerBuilder<ContainerBuilder> builder)
        {
            builder.Services
              .RegisterType<SqlServerBatchParser>()
              .InstancePerDependency()
              .IfNotRegistered(typeof(SqlServerBatchParser));

            builder.Services
              .RegisterType<SqlServerBatchParser>()
              .InstancePerDependency()
              .IfNotRegistered(typeof(SqlServerBatchParser));

            builder.Services
              .RegisterType<SqlServer2008Quoter>()
              .InstancePerLifetimeScope()
              .IfNotRegistered(typeof(SqlServer2008Quoter));


            builder.Services
              .RegisterType<SqlServer2012Processor>()
              .InstancePerLifetimeScope();

            builder.Services
              .Register(e => new SqlServer2008TypeMap())
              .As<ISqlServerTypeMap>()
              .InstancePerLifetimeScope()
              .IfNotRegistered(typeof(SqlServer2008TypeMap));

            builder.Services
              .Register(ctx => ctx.Resolve<SqlServer2012Processor>())
              .As<IMigrationProcessor>()
              .InstancePerLifetimeScope();

            builder.Services
              .RegisterType<SqlServer2012Generator>()
              .InstancePerLifetimeScope();

            builder.Services
              .Register(ctx => ctx.Resolve<SqlServer2012Generator>())
              .As<IMigrationGenerator>()
              .InstancePerLifetimeScope();

            return builder;
        }

        public static IMigrationRunnerBuilder<ContainerBuilder> AddSqlServer2014(this IMigrationRunnerBuilder<ContainerBuilder> builder)
        {
            builder.Services
              .RegisterType<SqlServerBatchParser>()
              .InstancePerDependency()
              .IfNotRegistered(typeof(SqlServerBatchParser));

            builder.Services
              .RegisterType<SqlServerBatchParser>()
              .InstancePerDependency()
              .IfNotRegistered(typeof(SqlServerBatchParser));

            builder.Services
              .RegisterType<SqlServer2008Quoter>()
              .InstancePerLifetimeScope()
              .IfNotRegistered(typeof(SqlServer2008Quoter));

            builder.Services
              .RegisterType<SqlServer2014Processor>()
              .InstancePerLifetimeScope();

            builder.Services
              .Register(e => new SqlServer2008TypeMap())
              .As<ISqlServerTypeMap>()
              .InstancePerLifetimeScope()
              .IfNotRegistered(typeof(SqlServer2008TypeMap));

            builder.Services
              .Register(ctx => ctx.Resolve<SqlServer2014Processor>())
              .As<IMigrationProcessor>()
              .InstancePerLifetimeScope();

            builder.Services
              .RegisterType<SqlServer2014Generator>()
              .InstancePerLifetimeScope();

            builder.Services
              .Register(ctx => ctx.Resolve<SqlServer2014Generator>())
              .As<IMigrationGenerator>()
              .InstancePerLifetimeScope();

            return builder;
        }

        public static IMigrationRunnerBuilder<ContainerBuilder> AddSqlServer2016(this IMigrationRunnerBuilder<ContainerBuilder> builder)
        {
            builder.Services
              .RegisterType<SqlServerBatchParser>()
              .InstancePerDependency()
              .IfNotRegistered(typeof(SqlServerBatchParser));

            builder.Services
              .RegisterType<SqlServerBatchParser>()
              .InstancePerDependency()
              .IfNotRegistered(typeof(SqlServerBatchParser));

            builder.Services
              .RegisterType<SqlServer2008Quoter>()
              .InstancePerLifetimeScope()
              .IfNotRegistered(typeof(SqlServer2008Quoter));

            builder.Services
              .RegisterType<SqlServer2016Processor>()
              .InstancePerLifetimeScope();

            builder.Services
              .Register(e => new SqlServer2008TypeMap())
              .As<ISqlServerTypeMap>()
              .InstancePerLifetimeScope()
              .IfNotRegistered(typeof(SqlServer2008TypeMap));

            builder.Services
              .Register(ctx => ctx.Resolve<SqlServer2016Processor>())
              .As<IMigrationProcessor>()
              .InstancePerLifetimeScope();

            builder.Services
              .RegisterType<SqlServer2016Generator>()
              .InstancePerLifetimeScope();

            builder.Services
              .Register(ctx => ctx.Resolve<SqlServer2016Generator>())
              .As<IMigrationGenerator>()
              .InstancePerLifetimeScope();

            return builder;
        }

        public static IMigrationRunnerBuilder<ContainerBuilder> AddSqlServerOption(this IMigrationRunnerBuilder<ContainerBuilder> builder, string proccessType="sqlserver")
        {
            
            builder.Services
              .Register(ctx => new SelectingProcessorAccessorOptions { ProcessorId = proccessType })
              .SingleInstance();

            builder.Services
              .Register(ctx => new SelectingGeneratorAccessorOptions { GeneratorId = ProcessorId.SqlServer })
              .SingleInstance();

            return builder;
        }
    }
}
