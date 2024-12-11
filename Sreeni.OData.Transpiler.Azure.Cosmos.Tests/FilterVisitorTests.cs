using Sreeni.OData.Transpiler.Core;
using Shouldly;
using System.Text;

namespace Sreeni.OData.Transpiler.Azure.Cosmos.Tests
{
    [TestClass]
    public class FilterVisitorTests
    {
        private FilterVisitor sut;
        private StringBuilder _sqlBuilder;
        private List<QueryParameter> _queryParameters;

        [TestInitialize]
        public void Setup()
        {
            sut = new FilterVisitor();
            _sqlBuilder = new StringBuilder();
            _queryParameters = new List<QueryParameter>();
        }

        [TestMethod]
        public void Visit_ShouldAppendWhereClause_WhenFilterIsNotNullOrEmpty()
        {
            // Arrange
            var query = new ODataQuery
            {
                Filter = "Name eq 'John'"
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBe(" WHERE c.Name = @param0");
            _queryParameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "John");
        }

        [TestMethod]
        public void Visit_ShouldNotAppendWhereClause_WhenFilterIsNull()
        {
            // Arrange
            var query = new ODataQuery
            {
                Filter = null
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBeEmpty();
        }

        [TestMethod]
        public void Visit_ShouldNotAppendWhereClause_WhenFilterIsEmpty()
        {
            // Arrange
            var query = new ODataQuery
            {
                Filter = string.Empty
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBeEmpty();
        }

        [TestMethod]
        public void Visit_ShouldHandleContainsFunction()
        {
            // Arrange
            var query = new ODataQuery
            {
                Filter = "contains(Name, 'John')"
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBe(" WHERE CONTAINS(c.Name, @param0)");
            _queryParameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "John");
        }

        [TestMethod]
        public void Visit_ShouldHandleInFunction()
        {
            // Arrange
            var query = new ODataQuery
            {
                Filter = "Name in ('John', 'Doe')"
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBe(" WHERE c.Name IN (@param0, @param1)");
            _queryParameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "John");
            _queryParameters.ShouldContain(p => p.Name == "@param1" && (string)p.Value == "Doe");
        }

        [TestMethod]
        public void Visit_ShouldHandleBetweenFunction()
        {
            // Arrange
            var query = new ODataQuery
            {
                Filter = "Age between 20 and 30"
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBe(" WHERE c.Age BETWEEN @param0 AND @param1");
            _queryParameters.ShouldContain(p => p.Name == "@param0" && (int)p.Value == 20);
            _queryParameters.ShouldContain(p => p.Name == "@param1" && (int)p.Value == 30);
        }

        
        [TestMethod]
        public void Visit_ShouldHandleBetweenFunction_WhenFilterContainsDateTimeValues()
        {
            // Arrange
            var query = new ODataQuery
            {
                Filter = "CreatedDate between '2021-01-01T00:00:00' and '2021-12-31T23:59:59'"
            };
            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);
            // Assert
            _sqlBuilder.ToString().ShouldBe(" WHERE c.CreatedDate BETWEEN @param0 AND @param1");
            _queryParameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "2021-01-01T00:00:00");
            _queryParameters.ShouldContain(p => p.Name == "@param1" && (string)p.Value == "2021-12-31T23:59:59");
        }
        
        [TestMethod]
        public void Visit_ShouldHandleNestedProperties()
        {
            // Arrange
            var query = new ODataQuery
            {
                Filter = "Address/City eq 'Chennai'"
            };
            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);
            // Assert
            _sqlBuilder.ToString().ShouldBe(" WHERE c.Address.City = @param0");
            _queryParameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "Chennai");
        }

        [TestMethod]
        public void Visit_ShouldHandleNestedPropertiesWithDotNotation()
        {
            // Arrange
            var query = new ODataQuery
            {
                Filter = "Address.City eq 'Chennai'"
            };
            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);
            // Assert
            _sqlBuilder.ToString().ShouldBe(" WHERE c.Address.City = @param0");
            _queryParameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "Chennai");
        }
    }
}
