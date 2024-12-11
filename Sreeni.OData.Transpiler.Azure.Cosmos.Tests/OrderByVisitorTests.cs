using Shouldly;
using Sreeni.OData.Transpiler.Core;
using System.Text;

namespace Sreeni.OData.Transpiler.Azure.Cosmos.Tests
{
    [TestClass]
    public class OrderByVisitorTests
    {
        private OrderByVisitor sut;
        private StringBuilder _sqlBuilder;
        private List<QueryParameter> _queryParameters;

        [TestInitialize]
        public void Setup()
        {
            sut = new OrderByVisitor();
            _sqlBuilder = new StringBuilder();
            _queryParameters = new List<QueryParameter>();
        }

        [TestMethod]
        public void Visit_ShouldAppendOrderByClause_WhenOrderByIsNotNullOrEmpty()
        {
            // Arrange
            var query = new ODataQuery
            {
                OrderBy = new List<string> { "Name asc", "Age desc" }
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBe(" ORDER BY c.Name ASC, c.Age DESC");
        }

        [TestMethod]
        public void Visit_ShouldNotAppendOrderByClause_WhenOrderByIsNull()
        {
            // Arrange
            var query = new ODataQuery
            {
                OrderBy = null
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBeEmpty();
        }

        [TestMethod]
        public void Visit_ShouldNotAppendOrderByClause_WhenOrderByIsEmpty()
        {
            // Arrange
            var query = new ODataQuery
            {
                OrderBy = new List<string>()
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBeEmpty();
        }
        
        [TestMethod]
        public void Visit_ShouldAppendOrderByClause_WhenOrderDirectionIsEmpty()
        {
            // Arrange
            var query = new ODataQuery
            {
                OrderBy = new List<string> { "Name", "Age" }
            };
            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);
            // Assert
            _sqlBuilder.ToString().ShouldBe(" ORDER BY c.Name ASC, c.Age ASC");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Visit_ShouldAppendOrderByClause_WhenOrderDirectionIsNotASCOrDESC()
        {
            // Arrange
            var query = new ODataQuery
            {
                OrderBy = new List<string> { "Name WEX", "Age FOX" }
            };
            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);
        }

        
        [TestMethod]
        public void Visit_ShouldAppendOrderByClause_WhenOrderByHasNestedProperties()
        {
            // Arrange
            var query = new ODataQuery
            {
                OrderBy = new List<string> { "Name asc", "Age desc", "Address/Street asc" }
            };
            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);
            // Assert
            _sqlBuilder.ToString().ShouldBe(" ORDER BY c.Name ASC, c.Age DESC, c.Address.Street ASC");
        }
    }
}
