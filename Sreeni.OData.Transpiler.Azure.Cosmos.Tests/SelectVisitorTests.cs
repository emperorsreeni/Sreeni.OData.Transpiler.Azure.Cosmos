using Sreeni.OData.Transpiler.Core;
using Shouldly;
using System.Text;

namespace Sreeni.OData.Transpiler.Azure.Cosmos.Tests
{
    [TestClass]
    public class SelectVisitorTests
    {
        private SelectVisitor sut;
        private StringBuilder _sqlBuilder;
        private List<QueryParameter> _queryParameters;

        [TestInitialize]
        public void Setup()
        {
            sut = new SelectVisitor();
            _sqlBuilder = new StringBuilder();
            _queryParameters = new List<QueryParameter>();
        }

        [TestMethod]
        public void Visit_ShouldAppendSelectClause_WhenSelectIsNotNullOrEmpty()
        {
            // Arrange
            var query = new ODataQuery
            {
                Select = new List<string> { "Name", "Age" }
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBe("SELECT c.Name, c.Age FROM c");
        }

        [TestMethod]
        public void Visit_ShouldAppendSelectAllClause_WhenSelectIsNull()
        {
            // Arrange
            var query = new ODataQuery
            {
                Select = null
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBe("SELECT * FROM c");
        }

        [TestMethod]
        public void Visit_ShouldAppendSelectAllClause_WhenSelectIsEmpty()
        {
            // Arrange
            var query = new ODataQuery
            {
                Select = new List<string>()
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBe("SELECT * FROM c");
        }
        //write unit tests for this scenario: Visit_ should return valid SQL query when select has nested properties
        [TestMethod]
        public void Visit_ShouldAppendSelectClause_WhenSelectHasNestedProperties()
        {
            // Arrange
            var query = new ODataQuery
            {
                Select = new List<string> { "Name", "Age", "Address/City", "Address/State" }
            };
            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);
            // Assert
            _sqlBuilder.ToString().ShouldBe("SELECT c.Name, c.Age, c.Address.City as AddressCity, c.Address.State as AddressState FROM c");
        }

        [TestMethod]
        public void Visit_ShouldAppendSelectClause_WhenSelectHasNestedPropertiesWithDotNotation()
        {
            // Arrange
            var query = new ODataQuery
            {
                Select = new List<string> { "Name", "Age", "Address.City", "Address.State" }
            };
            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);
            // Assert
            _sqlBuilder.ToString().ShouldBe("SELECT c.Name, c.Age, c.Address.City as AddressCity, c.Address.State as AddressState FROM c");
        }

    }
}
