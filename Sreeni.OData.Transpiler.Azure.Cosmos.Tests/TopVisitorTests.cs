using Shouldly;
using Sreeni.OData.Transpiler.Core;
using System.Text;

namespace Sreeni.OData.Transpiler.Azure.Cosmos.Tests
{
    [TestClass]
    public class TopVisitorTests
    {
        private TopVisitor sut;
        private StringBuilder _sqlBuilder;
        private List<QueryParameter> _queryParameters;

        [TestInitialize]
        public void Setup()
        {
            sut = new TopVisitor();
            _sqlBuilder = new StringBuilder();
            _queryParameters = new List<QueryParameter>();
        }

        [TestMethod]
        public void Visit_ShouldAppendTopClause_WhenTopHasValue()
        {
            // Arrange
            var query = new ODataQuery
            {
                Top = 10
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBe(" OFFSET 0 LIMIT 10");
        }

        [TestMethod]
        public void Visit_ShouldNotAppendTopClause_WhenTopIsNull()
        {
            // Arrange
            var query = new ODataQuery
            {
                Top = null
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBeEmpty();
        }
    }
}
