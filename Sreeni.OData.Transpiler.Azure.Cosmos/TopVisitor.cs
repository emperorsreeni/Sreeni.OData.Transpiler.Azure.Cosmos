using Sreeni.OData.Transpiler.Core;
using System.Text;

namespace Sreeni.OData.Transpiler.Azure.Cosmos
{
    public class TopVisitor : IODataQueryVisitor
    {
        public void Visit(ODataQuery query, StringBuilder sqlBuilder, List<QueryParameter> queryParameters)
        {
            if (query.Top.HasValue)
            {
                if (sqlBuilder.HasOffsetAlready())
                    return;
                var offset = query.Skip.HasValue ? query.Skip.Value : 0;
                sqlBuilder.Append($" OFFSET {offset} LIMIT {query.Top.Value}");
            }
        }
    }
}
