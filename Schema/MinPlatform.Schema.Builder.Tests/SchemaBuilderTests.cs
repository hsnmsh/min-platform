using MinPlatform.Data.Abstractions.Exceptions;
using MinPlatform.Data.Abstractions.Models;
using MinPlatform.Schema.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace MinPlatform.Schema.Builder.Tests
{

    public class SchemaBuilderTests
    {
        private readonly BaseSchemaBuilder _builder;

        public SchemaBuilderTests()
        {
            _builder = new SchemaBuilder();
        }

        //schema name
        [Fact]
        public void AddSchema_ValidSchemaName_AddsSchema()
        {
            // Arrange
            string schemaName = "TestSchema";

            // Act
            _builder.AddSchema(schemaName);

            // Assert
            Assert.Contains(schemaName, _builder.Schemas); // Assuming manager.Schemas is a collection
        }

        [Fact]
        public void AddSchema_EmptySchemaName_ThrowsArgumentNullException()
        {

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddSchema(string.Empty));
        }

        [Fact]
        public void AddSchema_NullSchemaName_ThrowsArgumentNullException()
        {

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddSchema(null));
        }

        //functions
        [Fact]
        public void AddFunction_ValidFunction_AddsFunction()
        {
            // Arrange
            var function = new Function { Name = "TestFunction", Action = "SomeAction" };

            // Act
            _builder.AddFunction(function);

            // Assert
            Assert.Contains(function, _builder.Functions); // Assuming manager.Functions is a collection
        }

        [Fact]
        public void AddFunction_NullFunction_ThrowsArgumentNullException()
        {

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddFunction(null));
        }

        [Fact]
        public void AddFunction_EmptyFunctionName_ThrowsArgumentNullException()
        {

            var function = new Function { Name = string.Empty, Action = "SomeAction" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddFunction(function));
        }

        [Fact]
        public void AddFunction_EmptyFunctionAction_ThrowsArgumentNullException()
        {

            var function = new Function { Name = "TestFunction", Action = string.Empty };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddFunction(function));
        }

        //sp

        [Fact]
        public void AddStoredProcedure_ValidStoredProcedure_AddsStoredProcedure()
        {

            var procedure = new StoredProcedure { Name = "TestProcedure", Action = "SomeAction" };

            // Act
            _builder.AddStoredProcedure(procedure);

            // Assert
            Assert.Contains(procedure, _builder.StoredProcedures); // Assuming manager.StoredProcedures is a collection
        }

        [Fact]
        public void AddStoredProcedure_NullStoredProcedure_ThrowsArgumentNullException()
        {


            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddStoredProcedure(null));
        }

        [Fact]
        public void AddStoredProcedure_EmptyProcedureName_ThrowsArgumentNullException()
        {

            var procedure = new StoredProcedure { Name = string.Empty, Action = "SomeAction" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddStoredProcedure(procedure));
        }

        [Fact]
        public void AddStoredProcedure_EmptyProcedureAction_ThrowsArgumentNullException()
        {
            var procedure = new StoredProcedure { Name = "TestProcedure", Action = string.Empty };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddStoredProcedure(procedure));
        }

        //view
        [Fact]
        public void AddView_ValidView_AddsView()
        {

            var view = new View { Name = "TestView", Action = "SomeAction" };

            // Act
            _builder.AddView(view);

            // Assert
            Assert.Contains(view, _builder.Views); // Assuming manager.Views is a collection
        }

        [Fact]
        public void AddView_NullView_ThrowsArgumentNullException()
        {

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddView(null));
        }

        [Fact]
        public void AddView_EmptyViewName_ThrowsArgumentNullException()
        {
            var view = new View { Name = string.Empty, Action = "SomeAction" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddView(view));
        }

        [Fact]
        public void AddView_EmptyViewAction_ThrowsArgumentNullException()
        {
            var view = new View { Name = "TestView", Action = string.Empty };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddView(view));
        }

        //table
        [Fact]
        public void AddTable_ValidTable_AddsTable()
        {
            // Arrange
            var tableConfig = new TableConfig
            {
                Name = "TestTable",
                Columns = new List<Column> { new Column { Name = "Column1" } }
            };

            // Act
            _builder.AddTable(tableConfig);

            // Assert
            Assert.Contains(tableConfig, _builder.Tables); // Assuming manager.Tables is a collection
        }

        [Fact]
        public void AddTable_NullTableConfig_ThrowsArgumentNullException()
        {

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddTable((TableConfig)null));
        }

        [Fact]
        public void AddTable_EmptyTableName_ThrowsArgumentNullException()
        {

            var tableConfig = new TableConfig
            {
                Name = string.Empty,
                Columns = new List<Column> { new Column { Name = "Column1" } }
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddTable(tableConfig));
        }

        [Fact]
        public void AddTable_NullColumns_ThrowsArgumentNullException()
        {
            var tableConfig = new TableConfig { Name = "TestTable", Columns = null };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddTable(tableConfig));
        }

        [Fact]
        public void AddTable_EmptyColumns_ThrowsArgumentNullException()
        {

            var tableConfig = new TableConfig { Name = "TestTable", Columns = new List<Column>() };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _builder.AddTable(tableConfig));
        }


        [Fact]
        public void AddTable_ValidEntity_AddsTableWithColumns()
        {

            var entity = new TestEntity(); // Example entity

            _builder.AddTable<TestEntity, int>(entity, () => new TableConfig
            {
                Name = nameof(entity),

                Columns = new List<Column>
                {
                    new Column
                    {
                        Name=nameof(entity.FirstName),
                        DataType=System.Data.DbType.String,
                        AllowNull=false,
                        Promoted=true,
                    },
                    new Column
                    {
                        Name=nameof(entity.LastName),
                        DataType=System.Data.DbType.String,
                        AllowNull=false,
                    },
                    new Column
                    {
                        Name=nameof(entity.Age),
                        DataType=System.Data.DbType.Int32,
                        AllowNull=false,
                    },
                    new Column
                    {
                        Name=nameof(entity.DOB),
                        DataType=System.Data.DbType.Date,
                    },
                    new Column
                    {
                        Name=nameof(entity.IsEmployee),
                        DataType=System.Data.DbType.Boolean,
                    },

                },

                Schema="MainData",
                
            });

            // Assert
            Assert.NotEmpty(_builder.Tables);
        }

        [Fact]
        public void AddTable_NullEntity_ThrowsArgumentNullException()
        {

            Expression<Func<TableConfig>> entityDelegateExp = () => new TableConfig { Name = null, Columns = new List<Column> { new Column { Name = "Column1" } } };

            // Act & Assert
            Assert.Throws<SchemaException>(() => _builder.AddTable<TestEntity, int>(null, entityDelegateExp));
        }
    }

}
