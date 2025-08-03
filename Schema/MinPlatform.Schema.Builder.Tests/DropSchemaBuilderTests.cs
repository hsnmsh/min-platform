using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MinPlatform.Schema.Builder.Tests
{
    public class DropSchemaBuilderTests
    {
        private readonly BaseSchemaBuilder _schemaManagement;

        public DropSchemaBuilderTests()
        {
            _schemaManagement = new SchemaBuilder();
        }
        [Fact]
        public void DropSchema_ShouldThrowArgumentNullException_WhenSchemaNameIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropSchema(null));
            Assert.Equal("schemaName", exception.ParamName);
        }

        [Fact]
        public void DropSchema_ShouldThrowArgumentNullException_WhenSchemaNameIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropSchema(string.Empty));
            Assert.Equal("schemaName", exception.ParamName);
        }

        [Fact]
        public void DropSchema_ShouldAddSchema_WhenSchemaNameIsValid()
        {
            // Arrange
            string schemaName = "TestSchema";

            // Act
            _schemaManagement.DropSchema(schemaName);

            // Assert
            Assert.Contains(schemaName, _schemaManagement.DroppedSchemas);
        }



        [Fact]
        public void DropTable_ShouldThrowArgumentNullException_WhenTableNameIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropTable(null));
            Assert.Equal("tableName", exception.ParamName);
        }

        [Fact]
        public void DropTable_ShouldThrowArgumentNullException_WhenTableNameIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropTable(string.Empty));
            Assert.Equal("tableName", exception.ParamName);
        }

        [Fact]
        public void DropTable_ShouldAddTable_WhenTableNameIsValid()
        {
            // Arrange
            string tableName = "TestTable";

            // Act
            _schemaManagement.DropTable(tableName);

            // Assert
            Assert.Contains(tableName, _schemaManagement.DroppedTables);
        }

        [Fact]
        public void DropColumns_ShouldThrowArgumentNullException_WhenTableNameIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropColumns(null, new List<string> { "Column1" }));
            Assert.Equal("tableName", exception.ParamName);
        }

        [Fact]
        public void DropColumns_ShouldThrowArgumentNullException_WhenColumnListIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropColumns("TestTable", new List<string>()));
        }

        [Fact]
        public void DropColumns_ShouldAddColumns_WhenValidColumnsAreProvided()
        {
            // Arrange
            string tableName = "TestTable";
            var columns = new List<string> { "Column1", "Column2" };

            // Act
            _schemaManagement.DropColumns(tableName, columns);

            // Assert
            Assert.Contains(tableName, _schemaManagement.DroppedColumns.Keys);
            Assert.Equal(columns, _schemaManagement.DroppedColumns[tableName]);
        }

        [Fact]
        public void DropConstraints_ShouldThrowArgumentNullException_WhenTableNameIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropConstraints(null, new List<string> { "Constraint1" }));
            Assert.Equal("tableName", exception.ParamName);
        }

        [Fact]
        public void DropConstraints_ShouldThrowArgumentNullException_WhenConstraintsListIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropConstraints("TestTable", new List<string>()));
        }

        [Fact]
        public void DropConstraints_ShouldAddConstraints_WhenValidConstraintsAreProvided()
        {
            // Arrange
            string tableName = "TestTable";
            var constraints = new List<string> { "Constraint1", "Constraint2" };

            // Act
            _schemaManagement.DropConstraints(tableName, constraints);

            // Assert
            Assert.Contains(tableName, _schemaManagement.DroppedConstraints.Keys);
            Assert.Equal(constraints, _schemaManagement.DroppedConstraints[tableName]);
        }

        [Fact]
        public void DropTriggers_ShouldThrowArgumentNullException_WhenTableNameIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropTriggers(null, new List<string> { "Trigger1" }));
            Assert.Equal("tableName", exception.ParamName);
        }

        [Fact]
        public void DropTriggers_ShouldThrowArgumentNullException_WhenTriggersListIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropTriggers("TestTable", new List<string>()));
        }

        [Fact]
        public void DropTriggers_ShouldAddTriggers_WhenValidTriggersAreProvided()
        {
            // Arrange
            string tableName = "TestTable";
            var triggers = new List<string> { "Trigger1", "Trigger2" };

            // Act
            _schemaManagement.DropTriggers(tableName, triggers);

            // Assert
            Assert.Contains(tableName, _schemaManagement.DroppedTriggers.Keys);
            Assert.Equal(triggers, _schemaManagement.DroppedTriggers[tableName]);
        }

        [Fact]
        public void DropStoredProcedure_ShouldThrowArgumentNullException_WhenSpNameIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropStoredProcedure(null));
            Assert.Equal("spName", exception.ParamName);
        }

        [Fact]
        public void DropStoredProcedure_ShouldThrowArgumentNullException_WhenSpNameIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropStoredProcedure(string.Empty));
            Assert.Equal("spName", exception.ParamName);
        }

        [Fact]
        public void DropStoredProcedure_ShouldAddStoredProcedure_WhenSpNameIsValid()
        {
            // Arrange
            string spName = "TestProcedure";

            // Act
            _schemaManagement.DropStoredProcedure(spName);

            // Assert
            Assert.Contains(spName, _schemaManagement.DroppedStoredProcedures);
        }

        [Fact]
        public void DropFunction_ShouldThrowArgumentNullException_WhenFunctionNameIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropFunction(null));
            Assert.Equal("functionName", exception.ParamName);
        }

        [Fact]
        public void DropFunction_ShouldThrowArgumentNullException_WhenFunctionNameIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropFunction(string.Empty));
            Assert.Equal("functionName", exception.ParamName);
        }

        [Fact]
        public void DropFunction_ShouldAddFunction_WhenFunctionNameIsValid()
        {
            // Arrange
            string functionName = "TestFunction";

            // Act
            _schemaManagement.DropFunction(functionName);

            // Assert
            Assert.Contains(functionName, _schemaManagement.DroppedFunctions);
        }

        [Fact]
        public void DropView_ShouldThrowArgumentNullException_WhenViewIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropView(null));
            Assert.Equal("view", exception.ParamName);
        }

        [Fact]
        public void DropView_ShouldThrowArgumentNullException_WhenViewIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _schemaManagement.DropView(string.Empty));
            Assert.Equal("view", exception.ParamName);
        }

        [Fact]
        public void DropView_ShouldAddView_WhenViewIsValid()
        {
            // Arrange
            string view = "TestView";

            // Act
            _schemaManagement.DropView(view);

            // Assert
            Assert.Contains(view, _schemaManagement.DroppedViews);
        }
    }
}
