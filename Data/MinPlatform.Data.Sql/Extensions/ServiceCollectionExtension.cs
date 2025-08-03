namespace MinPlatform.Data.Sql.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using MinPlatform.Data.Service;
    using MinPlatform.Data.Service.QueryBuilder.Factory;
    using MinPlatform.Data.Service.SqlStatmentBuilder;
    using MinPlatform.Data.Sql.Factory;
    using System;

    public static class ServiceCollectionExtension
    {
        
        public static IServiceCollection RegisterSqlServerBuilders(this IServiceCollection services, string connectionString)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services = services.BuildSqlServerBuilders();

            services.AddTransient<IDataService>((serviceProvider) =>
            {
                return new DataService(connectionString);

            });

            return services;

        }

        public static IServiceCollection RegisterSqlServerBuilders(this IServiceCollection services, Func<string> connectionStringResolver)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services = services.BuildSqlServerBuilders();

            services.AddTransient<IDataService>((serviceProvider) =>
            {
                string connectionString = connectionStringResolver();

                return new DataService(connectionString);

            });

            return services;

        }

        private static IServiceCollection BuildSqlServerBuilders(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<ISqlQueryStatmentBuilder, SqlServerQueryStatmentBuilder>();
            services.AddTransient<ISqlQueryBuilderFactory, SqlServerQueryBuilderFactory>();
            services.AddTransient<ISqlStatmentBuilderFactory, SqlServerStatmentBuilderFactory>();

            return services;

        }
    }
}
