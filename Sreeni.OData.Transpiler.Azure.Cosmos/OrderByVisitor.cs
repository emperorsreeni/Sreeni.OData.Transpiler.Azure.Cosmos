using Sreeni.OData.Transpiler.Core;
using System.Text;

namespace Sreeni.OData.Transpiler.Azure.Cosmos
{
    public class OrderByVisitor : IODataQueryVisitor
    {
        public void Visit(ODataQuery query, StringBuilder sqlBuilder, List<QueryParameter> queryParameters)
        {
            if (query.OrderBy != null && query.OrderBy.Count > 0)
            {
                sqlBuilder.Append(" ORDER BY " + string.Join(", ", query.OrderBy.Select(o => o.AddOrderByField())));
            }
        }
    }
}
