using MinPlatform.Data.Abstractions.Models;
using System;
using Xunit;

namespace MinPlatform.Schema.Builder.Tests
{
    public class LookupTest
    {
        private readonly BaseSchemaBuilder _schemaBuilder;

        public LookupTest()
        {
            _schemaBuilder = new SchemaBuilder();
        }

        [Fact]
        public void AddLookup_ShouldThrowArgumentNullException_WhenLookupInfoEntityIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.AddLookup(null));

            // Verify that the exception is for the correct parameter
            Assert.Equal("lookupInfoEntity", exception.ParamName);
        }

        [Fact]
        public void AddLookup_ShouldThrowArgumentNullException_WhenTableNameIsNull()
        {
            // Arrange
            var lookupInfoEntity = new LookupInfoEntity { TableName = null };

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.AddLookup(lookupInfoEntity));

            // Verify that the exception is for the correct parameter
            Assert.Equal("TableName", exception.ParamName);
        }

        [Fact]
        public void AddLookup_ShouldThrowArgumentNullException_WhenTableNameIsEmpty()
        {
            // Arrange
            var lookupInfoEntity = new LookupInfoEntity { TableName = string.Empty };

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.AddLookup(lookupInfoEntity));

            // Verify that the exception is for the correct parameter
            Assert.Equal("TableName", exception.ParamName);
        }

        [Fact]
        public void AddLookup_ShouldAddLookupInfoEntity_WhenValidEntityIsProvided()
        {
            // Arrange
            var lookupInfoEntity = new LookupInfoEntity { TableName = "ValidTable" };

            // Act
            _schemaBuilder.AddLookup(lookupInfoEntity);

            // Assert
            Assert.Contains(lookupInfoEntity, _schemaBuilder.Lookups); // Assuming Lookups is a collection
        }

        [Fact]
        public void AlterLookupDependency_ShouldThrowArgumentNullException_WhenLookupTableNameIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.AlterLookupDependency(null, "SubTableName"));

            // Verify that the exception is for the correct parameter
            Assert.Equal("lookupTableName", exception.ParamName);
        }

        [Fact]
        public void AlterLookupDependency_ShouldThrowArgumentNullException_WhenLookupTableNameIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.AlterLookupDependency(string.Empty, "SubTableName"));

            // Verify that the exception is for the correct parameter
            Assert.Equal("lookupTableName", exception.ParamName);
        }

        [Fact]
        public void AlterLookupDependency_ShouldThrowArgumentNullException_WhenSubLookupTableNameIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.AlterLookupDependency("LookupTableName", null));

            // Verify that the exception is for the correct parameter
            Assert.Equal("subLookupTableName", exception.ParamName);
        }

        [Fact]
        public void AlterLookupDependency_ShouldThrowArgumentNullException_WhenSubLookupTableNameIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.AlterLookupDependency("LookupTableName", string.Empty));

            // Verify that the exception is for the correct parameter
            Assert.Equal("subLookupTableName", exception.ParamName);
        }

        [Fact]
        public void AlterLookupDependency_ShouldAddDependency_WhenValidTablesAreProvided()
        {
            // Arrange
            var lookupTableName = "LookupTableName";
            var subLookupTableName = "SubTableName";

            // Act
            _schemaBuilder.AlterLookupDependency(lookupTableName, subLookupTableName);

            // Assert
            var expectedDependency = (lookupTableName, subLookupTableName);
            Assert.Contains(expectedDependency, _schemaBuilder.AlteredLookupDependencies);
        }

        [Fact]
        public void DropLookup_ShouldThrowArgumentNullException_WhenLookupTableNameIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.DropLookup(null));

            // Verify that the exception is for the correct parameter
            Assert.Equal("lookupTableName", exception.ParamName);
        }

        [Fact]
        public void DropLookup_ShouldThrowArgumentNullException_WhenLookupTableNameIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaBuilder.DropLookup(string.Empty));

            // Verify that the exception is for the correct parameter
            Assert.Equal("lookupTableName", exception.ParamName);
        }

        [Fact]
        public void DropLookup_ShouldAddLookupToDroppedLookups_WhenValidTableNameIsProvided()
        {
            // Arrange
            var lookupTableName = "LookupTableName";

            // Act
            _schemaBuilder.DropLookup(lookupTableName);

            // Assert
            Assert.Contains(lookupTableName, _schemaBuilder.DroppedLookups);
        }
    }
}
