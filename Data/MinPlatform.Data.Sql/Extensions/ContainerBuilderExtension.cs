using Autofac;

namespace MinPlatform.Data.Sql.Extensions
{
    using MinPlatform.Data.Service;
    using MinPlatform.Data.Service.QueryBuilder.Factory;
    using MinPlatform.Data.Service.SqlStatmentBuilder;
    using MinPlatform.Data.Sql.Factory;
    using System;


    public static class ContainerBuilderExtension
    {
        public static ContainerBuilder AddSqlServerBuilders(this ContainerBuilder builder, string connectionString)
        {
            if (builder is null) 
            { 
                throw new ArgumentNullException(nameof(builder));
            }

            builder = builder.BuildSqlServerBuilders();

            builder.Register(ctx =>
            {
                return new DataService(connectionString);

            }).As<IDataService>().InstancePerDependency();

            return builder;


        }
       
        public static ContainerBuilder AddSqlServerBuilders(this ContainerBuilder builder, Func<string> connectionStringResolver)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder = builder.BuildSqlServerBuilders();

            builder.Register(ctx =>
            {
                string connectionString = connectionStringResolver();

                return new DataService(connectionString);

            }).As<IDataService>().InstancePerDependency();

            return builder;


        }

        private static ContainerBuilder BuildSqlServerBuilders(this ContainerBuilder builder)
        {

            builder.RegisterType<SqlServerQueryStatmentBuilder>()
                .As<ISqlQueryStatmentBuilder>()
                .InstancePerDependency();

            builder.RegisterType<SqlServerQueryBuilderFactory>()
               .As<ISqlQueryBuilderFactory>()
               .InstancePerDependency();

            builder.RegisterType<SqlServerStatmentBuilderFactory>()
                .As<ISqlStatmentBuilderFactory>()
                .InstancePerDependency();

            return builder;
        }

    }
}
