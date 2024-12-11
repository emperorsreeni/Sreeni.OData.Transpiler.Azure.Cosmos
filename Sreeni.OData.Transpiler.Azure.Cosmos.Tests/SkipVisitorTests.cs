using Sreeni.OData.Transpiler.Core;
using Sreeni.OData.Transpiler.Azure.Cosmos;
using Shouldly;
using System.Text;

namespace Sreeni.OData.Transpiler.Azure.Cosmos.Tests
{
    [TestClass]
    public class SkipVisitorTests
    {
        private SkipVisitor sut;
        private StringBuilder _sqlBuilder;
        private List<QueryParameter> _queryParameters;

        [TestInitialize]
        public void Setup()
        {
            sut = new SkipVisitor();
            _sqlBuilder = new StringBuilder();
            _queryParameters = new List<QueryParameter>();
        }

        [TestMethod]
        public void Visit_ShouldAppendOffsetClause_WhenSkipHasValue()
        {
            // Arrange
            var query = new ODataQuery
            {
                Skip = 5
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBe(" OFFSET 5 LIMIT 1");
        }

        [TestMethod]
        public void Visit_ShouldNotAppendOffsetClause_WhenSkipIsNull()
        {
            // Arrange
            var query = new ODataQuery
            {
                Skip = null
            };

            // Act
            sut.Visit(query, _sqlBuilder, _queryParameters);

            // Assert
            _sqlBuilder.ToString().ShouldBeEmpty();
        }
    }
}
