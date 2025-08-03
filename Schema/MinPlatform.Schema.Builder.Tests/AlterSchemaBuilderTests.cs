using MinPlatform.Schema.Abstractions.Models;
using System.Collections.Generic;
using System;
using Xunit;
using MinPlatform.Data.Abstractions.Exceptions;
using System.Linq.Expressions;

namespace MinPlatform.Schema.Builder.Tests
{
    public class AlterSchemaBuilderTests
    {
        private readonly BaseSchemaBuilder _schemaAlteration;

        public AlterSchemaBuilderTests()
        {
            _schemaAlteration = new SchemaBuilder(); // Assuming SchemaAlteration is your class name
        }

        [Fact]
        public void AlterColumnName_ShouldThrowArgumentNullException_WhenTableNameIsNullOrEmpty()
        {
            var columnNameInfo = ("", "oldColumn", "newColumn");

            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterColumnName(columnNameInfo));

            Assert.Equal("Value cannot be null. (Parameter 'tableName')", exception.Message);
        }

        [Fact]
        public void AlterColumnName_ShouldThrowArgumentNullException_WhenOldColumnNameIsNullOrEmpty()
        {
            var columnNameInfo = ("table", "", "newColumn");

            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterColumnName(columnNameInfo));

            Assert.Equal("Value cannot be null. (Parameter 'oldColumnName')", exception.Message);
        }

        [Fact]
        public void AlterColumnName_ShouldThrowArgumentNullException_WhenNewColumnNameIsNullOrEmpty()
        {
            var columnNameInfo = ("table", "oldColumn", "");

            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterColumnName(columnNameInfo));

            Assert.Equal("Value cannot be null. (Parameter 'newColumnName')", exception.Message);
        }

        [Fact]
        public void AlterColumnName_ShouldAddToAlteredColumnNames_WhenValidData()
        {
            var columnNameInfo = ("table", "oldColumn", "newColumn");

            _schemaAlteration.AlterColumnName(columnNameInfo);

            Assert.Single(_schemaAlteration.AlteredColumnNames);
        }

        [Fact]
        public void AlterColumnsForTable_ShouldThrowArgumentNullException_WhenTableNameIsNullOrEmpty()
        {
            var alteredColumns = new List<Column> { new Column { } };

            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterColumnsForTable("", alteredColumns));

            Assert.Equal("Value cannot be null. (Parameter 'tableName')", exception.Message);
        }

        [Fact]
        public void AlterColumnsForTable_ShouldThrowArgumentNullException_WhenAlteredColumnsIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterColumnsForTable("table", new List<Column>()));

            Assert.Equal("Value cannot be null. (Parameter 'alteredColumns')", exception.Message);
        }

        [Fact]
        public void AlterColumnsForTable_ShouldAddToAlteredTableColumns_WhenValidData()
        {
            var alteredColumns = new List<Column>
            {
                new Column
                {
                    Name = "columnAlteredName",
                    DataType=System.Data.DbType.String,
                }
            };

            _schemaAlteration.AlterColumnsForTable("table", alteredColumns);

            Assert.Single(_schemaAlteration.AlteredTableColumns);
            Assert.Contains("table", _schemaAlteration.AlteredTableColumns.Keys);
        }

        [Fact]
        public void AlterLinkedEntitiesForTable_ShouldThrowArgumentNullException_WhenTableNameIsNullOrEmpty()
        {
            var alteredLinkedEntities = new List<LinkedEntity> { new LinkedEntity { } };

            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterLinkedEntitiesForTable("", alteredLinkedEntities));

            Assert.Equal("Value cannot be null. (Parameter 'tableName')", exception.Message);
        }

        [Fact]
        public void AlterLinkedEntitiesForTable_ShouldThrowArgumentNullException_WhenAlteredLinkedEntitiesIsNullOrEmpty()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterLinkedEntitiesForTable("table", new List<LinkedEntity>()));

            Assert.Equal("Value cannot be null. (Parameter 'alteredLinkedEntities')", exception.Message);
        }

        [Fact]
        public void AlterLinkedEntitiesForTable_ShouldAddToAlteredTablesLinkedEntities_WhenValidData()
        {
            var alteredLinkedEntities = new List<LinkedEntity>
            {
                new LinkedEntity
                {
                    TargetColumn="targetColumn",
                    TargetTable="targetTable"
                }
            };

            _schemaAlteration.AlterLinkedEntitiesForTable("table", alteredLinkedEntities);

            Assert.Single(_schemaAlteration.AlteredTablesLinkedEntities);
            Assert.Contains("table", _schemaAlteration.AlteredTablesLinkedEntities.Keys);
        }

        [Fact]
        public void AlterStoredProcedure_ShouldThrowArgumentNullException_WhenStoredProcedureIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterStoredProcedure(null));

        }

        [Fact]
        public void AlterStoredProcedure_ShouldThrowArgumentNullException_WhenStoredProcedureNameIsNullOrEmpty()
        {
            var storedProcedure = new StoredProcedure
            {
                Name = "",
                Action = "test"
            };

            Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterStoredProcedure(storedProcedure));

        }

        [Fact]
        public void AlterStoredProcedure_ShouldThrowArgumentNullException_WhenStoredProcedureActionIsNullOrEmpty()
        {
            var storedProcedure = new StoredProcedure
            {
                Name = "spName"
            };

            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterStoredProcedure(storedProcedure));

        }

        [Fact]
        public void AlterStoredProcedure_ShouldAddToAlteredStoredProcedures_WhenValidData()
        {
            var storedProcedure = new StoredProcedure
            {
                Name = "spName",
                Action = "some action"
            };

            _schemaAlteration.AlterStoredProcedure(storedProcedure);

            Assert.Single(_schemaAlteration.AlteredStoredProcedures);
        }

        [Fact]
        public void AlterFunction_ShouldThrowArgumentNullException_WhenFunctionIsNull()
        {
            Function? function = null;

            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterFunction(function));

        }

        [Fact]
        public void AlterFunction_ShouldThrowArgumentNullException_WhenFunctionNameIsNullOrEmpty()
        {
            var function = new Function { Name = "", Action = "body" };

            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterFunction(function));

        }

        [Fact]
        public void AlterFunction_ShouldThrowArgumentNullException_WhenFunctionActionIsNullOrEmpty()
        {
            var function = new Function { Name = "MyFunction", Action = "" };

            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterFunction(function));

        }

        [Fact]
        public void AlterFunction_ShouldAddToAlteredFunctions_WhenValidData()
        {
            var function = new Function { Name = "MyFunction", Action = "some body" };

            _schemaAlteration.AlterFunction(function);

            Assert.Single(_schemaAlteration.AlteredFunctions);
            Assert.Contains(function, _schemaAlteration.AlteredFunctions);
        }

        [Fact]
        public void AlterView_ShouldThrowArgumentNullException_WhenViewIsNull()
        {
            View? view = null;

            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterView(view));

        }

        [Fact]
        public void AlterView_ShouldThrowArgumentNullException_WhenViewNameIsNullOrEmpty()
        {
            var view = new View { Name = "", Action = "some function" };

            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterView(view));

        }

        [Fact]
        public void AlterView_ShouldThrowArgumentNullException_WhenViewActionIsNullOrEmpty()
        {
            var view = new View { Name = "MyView", Action = "" };

            var exception = Assert.Throws<ArgumentNullException>(() =>
                _schemaAlteration.AlterView(view));

        }

        [Fact]
        public void AlterView_ShouldAddToAlteredViews_WhenValidData()
        {
            var view = new View { Name = "MyView", Action = "some function" };

            _schemaAlteration.AlterView(view);

            Assert.Single(_schemaAlteration.AlteredViews);
            Assert.Contains(view, _schemaAlteration.AlteredViews);
        }

        [Fact]
        public void AlterColumnsForTable_ShouldThrowSchemaException_WhenColumnListIsNull()
        {
            // Arrange
            var entity = new TestEntity(); // This is a simple test entity.
            Expression<Func<IList<Column>>> entityDelegateExp = () => (IList<Column>)null;

            // Act & Assert
            var exception = Assert.Throws<SchemaException>(() =>
                _schemaAlteration.AlterColumnsForTable<TestEntity, int>(entity, entityDelegateExp));

            Assert.Equal("column list must not be empty", exception.Message);
        }

        [Fact]
        public void AlterColumnsForTable_ShouldThrowSchemaException_WhenColumnListIsEmpty()
        {
            // Arrange
            var entity = new TestEntity(); // This is a simple test entity.
            Expression<Func<IList<Column>>> entityDelegateExp = () => new List<Column>();

            // Act & Assert
            var exception = Assert.Throws<SchemaException>(() =>
                _schemaAlteration.AlterColumnsForTable<TestEntity, int>(entity, entityDelegateExp));

            Assert.Equal("column list must not be empty", exception.Message);
        }

        [Fact]
        public void AlterColumnsForTable_ShouldAddColumnsToAlteredTableColumns_WhenValidColumnList()
        {
            // Arrange
            var entity = new TestEntity(); // This is a simple test entity.
            var columns = new List<Column>
            {
                new Column{Name=nameof(entity.Age), DataType=System.Data.DbType.Int32 },
                new Column{Name=nameof(entity.FirstName), DataType=System.Data.DbType.String }
            };
            Expression<Func<IList<Column>>> entityDelegateExp = () => columns;

            // Act
            _schemaAlteration.AlterColumnsForTable<TestEntity, int>(entity, entityDelegateExp);

            // Assert
            Assert.Single(_schemaAlteration.AlteredTableColumns);
            Assert.Contains(nameof(entity), _schemaAlteration.AlteredTableColumns.Keys);
            Assert.Equal(columns, _schemaAlteration.AlteredTableColumns[nameof(entity)]);
        }

        [Fact]
        public void AlterLinkedEntitiesForTable_ShouldThrowSchemaException_WhenLinkedEntityListIsNull()
        {
            // Arrange
            var entity = new TestEntity(); // This is a simple test entity.
            Expression<Func<IList<LinkedEntity>>> entityDelegateExp = () => (IList<LinkedEntity>)null;

            // Act & Assert
            var exception = Assert.Throws<SchemaException>(() =>
                _schemaAlteration.AlterLinkedEntitiesForTable<TestEntity, int>(entity, entityDelegateExp));

            Assert.Equal("linked entity list must not be empty", exception.Message);
        }

        [Fact]
        public void AlterLinkedEntitiesForTable_ShouldThrowSchemaException_WhenLinkedEntityListIsEmpty()
        {
            // Arrange
            var entity = new TestEntity(); // This is a simple test entity.
            Expression<Func<IList<LinkedEntity>>> entityDelegateExp = () => new List<LinkedEntity>();

            // Act & Assert
            var exception = Assert.Throws<SchemaException>(() =>
                _schemaAlteration.AlterLinkedEntitiesForTable<TestEntity, int>(entity, entityDelegateExp));

            Assert.Equal("linked entity list must not be empty", exception.Message);
        }

        [Fact]
        public void AlterLinkedEntitiesForTable_ShouldAddLinkedEntitiesToAlteredTablesLinkedEntities_WhenValidLinkedEntityList()
        {
            // Arrange
            var entity = new TestEntity(); // This is a simple test entity.
            var linkedEntities = new List<LinkedEntity>
            {
                new LinkedEntity
                {
                    TargetTable="targetTable",
                    TargetColumn="targetColumn"
                },
                new LinkedEntity 
                {
                    TargetTable="targetTable",
                    TargetColumn="targetColumn"
                }
            };
            Expression<Func<IList<LinkedEntity>>> entityDelegateExp = () => linkedEntities;

            // Act
            _schemaAlteration.AlterLinkedEntitiesForTable<TestEntity, int>(entity, entityDelegateExp);

            // Assert
            Assert.Single(_schemaAlteration.AlteredTablesLinkedEntities);
            Assert.Contains(nameof(entity), _schemaAlteration.AlteredTablesLinkedEntities.Keys);
            Assert.Equal(linkedEntities, _schemaAlteration.AlteredTablesLinkedEntities[nameof(entity)]);
        }
    }
}
