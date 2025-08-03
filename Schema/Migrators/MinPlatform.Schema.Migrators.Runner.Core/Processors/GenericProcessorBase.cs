#region License

// Copyright (c) 2007-2024, Fluent Migrator Project
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

namespace MinPlatform.Schema.Migrators.Runner.Processors
{
    using JetBrains.Annotations;
    using MinPlatform.Logging.Service;
    using MinPlatform.Schema.Migrators.Abstractions;
    using MinPlatform.Schema.Migrators.Runner.Initialization;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;

    public abstract class GenericProcessorBase : ProcessorBase
    {
        [Obsolete]
        private readonly string connectionString;

        [NotNull, ItemCanBeNull]
        private readonly Lazy<DbProviderFactory> dbProviderFactory;

        [NotNull, ItemCanBeNull]
        private readonly Lazy<IDbConnection> lazyConnection;

        [CanBeNull]
        private IDbConnection connection;

        private bool _disposed = false;

        [Obsolete]
        protected GenericProcessorBase(
            IDbConnection connection,
            IDbFactory factory,
            IMigrationGenerator generator,
            IAnnouncer announcer,
            [NotNull] IMigrationProcessorOptions options,
            LoggerManager logger)
            : base(generator, announcer, options, logger)
        {
            dbProviderFactory = new Lazy<DbProviderFactory>(() => (factory as DbFactoryBase)?.Factory);

            // Set the connection string, because it cannot be set by
            // the base class (due to the missing information)
            Options.ConnectionString = connection?.ConnectionString;

            // Prefetch connectionstring as after opening the security info could no longer be present
            // for instance on sql server
            connectionString = connection?.ConnectionString;

            Factory = factory;

            lazyConnection = new Lazy<IDbConnection>(() => connection);
        }

        protected GenericProcessorBase(
            [CanBeNull] Func<DbProviderFactory> factoryAccessor,
            [NotNull] IMigrationGenerator generator,
            LoggerManager logger,
            [NotNull] ProcessorOptions options,
            [NotNull] IConnectionStringAccessor connectionStringAccessor)
            : base(generator, logger, options)
        {
            dbProviderFactory = new Lazy<DbProviderFactory>(() => factoryAccessor?.Invoke());

            var connectionString = connectionStringAccessor.ConnectionString;

#pragma warning disable 612
            var legacyFactory = new DbFactoryWrapper(this);

            // Prefetch connectionstring as after opening the security info could no longer be present
            // for instance on sql server
            this.connectionString = connectionString;

            Factory = legacyFactory;
#pragma warning restore 612

            lazyConnection = new Lazy<IDbConnection>(
                () =>
                {
                    if (DbProviderFactory == null)
                        return null;
                    var connection = DbProviderFactory.CreateConnection();
                    Debug.Assert(connection != null, nameof(Connection) + " != null");
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    return connection;
                });
        }

        [Obsolete("Will change from public to protected")]
        public override string ConnectionString => connectionString;

        public IDbConnection Connection
        {
            get => connection ?? lazyConnection.Value;
            protected set => connection = value;
        }

        [Obsolete]
        [NotNull]
        public IDbFactory Factory { get; protected set; }

        [CanBeNull]
        public IDbTransaction Transaction { get; protected set; }

        [CanBeNull]
        protected DbProviderFactory DbProviderFactory => dbProviderFactory.Value;

        protected virtual void EnsureConnectionIsOpen()
        {
            if (Connection != null && Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        protected virtual void EnsureConnectionIsClosed()
        {
            if ((connection != null || (lazyConnection.IsValueCreated && Connection != null)) && Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }

        public override void BeginTransaction()
        {
            if (Transaction != null) return;

            EnsureConnectionIsOpen();

            Logger.Information("Beginning Transaction");

            Transaction = Connection?.BeginTransaction();
        }

        public override void RollbackTransaction()
        {
            if (Transaction == null) return;

            Logger.Information("Rolling back transaction");
            Transaction.Rollback();
            Transaction.Dispose();
            WasCommitted = true;
            Transaction = null;
        }

        public override void CommitTransaction()
        {
            if (Transaction == null) return;

            Logger.Information("Committing Transaction");
            Transaction.Commit();
            Transaction.Dispose();
            WasCommitted = true;
            Transaction = null;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!isDisposing || _disposed)
                return;

            _disposed = true;

            RollbackTransaction();
            EnsureConnectionIsClosed();
            if ((connection != null || (lazyConnection.IsValueCreated && Connection != null)))
            {
                Connection.Dispose();
            }
        }

        protected virtual IDbCommand CreateCommand(string commandText)
        {
            return CreateCommand(commandText, Connection, Transaction);
        }

        protected virtual IDbCommand CreateCommand(string commandText, IDbConnection connection, IDbTransaction transaction)
        {
            IDbCommand result;
            if (DbProviderFactory != null)
            {
                result = DbProviderFactory.CreateCommand();
                Debug.Assert(result != null, nameof(result) + " != null");
                result.Connection = connection;
                if (transaction != null)
                    result.Transaction = transaction;
                result.CommandText = commandText;
            }
            else
            {
#pragma warning disable 612
                result = Factory.CreateCommand(commandText, connection, transaction, Options);
#pragma warning restore 612
            }

            if (Options.Timeout != null)
            {
                result.CommandTimeout = (int) Options.Timeout.Value.TotalSeconds;
            }

            return result;
        }

        [Obsolete]
        private class DbFactoryWrapper : IDbFactory
        {
            private readonly GenericProcessorBase _processor;

            public DbFactoryWrapper(GenericProcessorBase processor)
            {
                _processor = processor;
            }

            /// <inheritdoc />
            public IDbConnection CreateConnection(string connectionString)
            {
                Debug.Assert(_processor.DbProviderFactory != null, "_processor.DbProviderFactory != null");
                var result = _processor.DbProviderFactory.CreateConnection();
                Debug.Assert(result != null, nameof(result) + " != null");
                result.ConnectionString = connectionString;
                return result;
            }

            /// <inheritdoc />
            [Obsolete]
            public IDbCommand CreateCommand(
                string commandText,
                IDbConnection connection,
                IDbTransaction transaction,
                IMigrationProcessorOptions options)
            {
                return _processor.CreateCommand(commandText);
            }
        }
    }
}
