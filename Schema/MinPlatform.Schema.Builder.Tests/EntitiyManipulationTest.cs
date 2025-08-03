using System;
using System.Collections.Generic;
using Xunit;

namespace MinPlatform.Schema.Builder.Tests
{
    public class EntitiyManipulationTest
    {
        private readonly BaseSchemaBuilder _schemaBuilder;

        public EntitiyManipulationTest()
        {
            _schemaBuilder = new SchemaBuilder();
        }

        [Fact]
        public void InsertEntities_ShouldThrowArgumentNullException_WhenTableNameIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.InsertEntities(null, new List<IDictionary<string, object>>()));

            // Verify that the exception is for the correct parameter
            Assert.Equal("tableName", exception.ParamName);
        }

        [Fact]
        public void InsertEntities_ShouldThrowArgumentNullException_WhenTableNameIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.InsertEntities(string.Empty, new List<IDictionary<string, object>>()));

            // Verify that the exception is for the correct parameter
            Assert.Equal("tableName", exception.ParamName);
        }

        [Fact]
        public void InsertEntities_ShouldThrowArgumentNullException_WhenEntitiesIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.InsertEntities("TableName", null));

            // Verify that the exception is for the correct parameter
            Assert.Equal("entities", exception.ParamName);
        }

        [Fact]
        public void InsertEntities_ShouldThrowArgumentNullException_WhenEntitiesIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.InsertEntities("TableName", new List<IDictionary<string, object>>()));

            // Verify that the exception is for the correct parameter
            Assert.Equal("entities", exception.ParamName);
        }

        [Fact]
        public void InsertEntities_ShouldAddEntities_WhenValidTableNameAndEntitiesAreProvided()
        {
            // Arrange
            var tableName = "TableName";
            var entities = new List<IDictionary<string, object>>
            {
                new Dictionary<string, object> { { "Column1", "Value1" }, { "Column2", 123 } }
            };

            // Act
            _schemaBuilder.InsertEntities(tableName, entities);

            // Assert
            Assert.Contains(tableName, _schemaBuilder.InsertedRecords.Keys);  // Verify tableName is added
            Assert.Equal(entities, _schemaBuilder.InsertedRecords[tableName]); // Verify entities are added correctly
        }

        [Fact]
        public void UpdateEntities_ShouldThrowArgumentNullException_WhenTableNameIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.UpdateEntities(null, 1, new Dictionary<string, object>()));

            // Verify that the exception is for the correct parameter
            Assert.Equal("tableName", exception.ParamName);
        }

        [Fact]
        public void UpdateEntities_ShouldThrowArgumentNullException_WhenTableNameIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.UpdateEntities(string.Empty, 1, new Dictionary<string, object>()));

            // Verify that the exception is for the correct parameter
            Assert.Equal("tableName", exception.ParamName);
        }

        [Fact]
        public void UpdateEntities_ShouldThrowArgumentNullException_WhenIdValueIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.UpdateEntities("TableName", null, new Dictionary<string, object>()));

            // Verify that the exception is for the correct parameter
            Assert.Equal("IdValue", exception.ParamName);
        }

        [Fact]
        public void UpdateEntities_ShouldThrowArgumentNullException_WhenEntityIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.UpdateEntities("TableName", 1, null));

            // Verify that the exception is for the correct parameter
            Assert.Equal("entity", exception.ParamName);
        }

        [Fact]
        public void UpdateEntities_ShouldThrowArgumentNullException_WhenEntityIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.UpdateEntities("TableName", 1, new Dictionary<string, object>()));

            // Verify that the exception is for the correct parameter
            Assert.Equal("entity", exception.ParamName);
        }

        [Fact]
        public void UpdateEntities_ShouldAddNewEntity_WhenTableNameDoesNotExistInUpdatedRecords()
        {
            // Arrange
            var tableName = "TableName";
            var idValue = 1;
            var entity = new Dictionary<string, object> { { "Column1", "Value1" } };

            // Act
            _schemaBuilder.UpdateEntities(tableName, idValue, entity);

            // Assert
            Assert.Contains(tableName, _schemaBuilder.UpdatedRecords);
            Assert.Single(_schemaBuilder.UpdatedRecords[tableName]);
            Assert.Equal((idValue, entity), _schemaBuilder.UpdatedRecords[tableName][0]);
        }

        [Fact]
        public void UpdateEntities_ShouldAddEntityToExistingTable_WhenTableNameExistsInUpdatedRecords()
        {
            // Arrange
            var tableName = "TableName";
            var idValue1 = 1;
            var entity1 = new Dictionary<string, object> { { "Column1", "Value1" } };
            var idValue2 = 2;
            var entity2 = new Dictionary<string, object> { { "Column2", "Value2" } };

            _schemaBuilder.UpdateEntities(tableName, idValue1, entity1);

            // Act
            _schemaBuilder.UpdateEntities(tableName, idValue2, entity2);

            // Assert
            Assert.Contains(tableName, _schemaBuilder.UpdatedRecords);
            var entities = _schemaBuilder.UpdatedRecords[tableName];
            Assert.Equal(2, entities.Count);  // There should be two entries now
            Assert.Contains((idValue1, entity1), entities);
            Assert.Contains((idValue2, entity2), entities);
        }

        [Fact]
        public void RemoveEntities_ShouldThrowArgumentNullException_WhenTableNameIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.RemoveEntities(null, 1));

            // Verify that the exception is for the correct parameter
            Assert.Equal("tableName", exception.ParamName);
        }

        [Fact]
        public void RemoveEntities_ShouldThrowArgumentNullException_WhenTableNameIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.RemoveEntities(string.Empty, 1));

            // Verify that the exception is for the correct parameter
            Assert.Equal("tableName", exception.ParamName);
        }

        [Fact]
        public void RemoveEntities_ShouldThrowArgumentNullException_WhenIdValueIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.RemoveEntities("TableName", null));

            // Verify that the exception is for the correct parameter
            Assert.Equal("IdValue", exception.ParamName);
        }

        [Fact]
        public void RemoveEntities_ShouldAddNewIdValue_WhenTableNameDoesNotExistInRemovedRecords()
        {
            // Arrange
            var tableName = "TableName";
            var idValue = 1;

            // Act
            _schemaBuilder.RemoveEntities(tableName, idValue);

            // Assert
            Assert.Contains(tableName, _schemaBuilder.RemovedRecords);
            Assert.Single(_schemaBuilder.RemovedRecords[tableName]);  // There should be one Id in the list
            Assert.Contains(idValue, _schemaBuilder.RemovedRecords[tableName]);
        }

        [Fact]
        public void RemoveEntities_ShouldAddIdValueToExistingList_WhenTableNameExistsInRemovedRecords()
        {
            // Arrange
            var tableName = "TableName";
            var idValue1 = 1;
            var idValue2 = 2;

            // Add the first IdValue
            _schemaBuilder.RemoveEntities(tableName, idValue1);

            // Act
            _schemaBuilder.RemoveEntities(tableName, idValue2);

            // Assert
            Assert.Contains(tableName, _schemaBuilder.RemovedRecords);
            var ids = _schemaBuilder.RemovedRecords[tableName];
            Assert.Equal(2, ids.Count);  // There should be two Ids now
            Assert.Contains(idValue1, ids);
            Assert.Contains(idValue2, ids);
        }
    }
}
