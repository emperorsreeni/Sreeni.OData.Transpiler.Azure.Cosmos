using Sreeni.OData.Transpiler.Core;
using System.Text;

namespace Sreeni.OData.Transpiler.Azure.Cosmos
{
    public class ODataAzureCosmosQueryTranslator : IODataQueryTranslator
    {
        public QueryResult Translate(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query));
            }
            var odataQuery = new ODataQueryParser()
                .Parse(query);
            if(odataQuery == null || odataQuery.IsEmpty())
            {
                throw new ArgumentException("Invalid query", nameof(query));
            }
            var sqlBuilder = new StringBuilder();
            var queryParameters = new List<QueryParameter>();
            var visitors = new List<IODataQueryVisitor>
            {
                new SelectVisitor(),
                new FilterVisitor(),
                new OrderByVisitor(),
                new TopVisitor(),
                new SkipVisitor()
            };
            foreach (var visitor in visitors)
            {
                odataQuery.Accept(visitor, sqlBuilder, queryParameters);
            }
            return new QueryResult
            {
                Query = sqlBuilder.ToString(),
                Parameters = queryParameters
            };

        }
    }
}
