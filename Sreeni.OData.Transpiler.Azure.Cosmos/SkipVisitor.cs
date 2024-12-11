using Sreeni.OData.Transpiler.Core;
using System.Text;

namespace Sreeni.OData.Transpiler.Azure.Cosmos
{
    public class SkipVisitor : IODataQueryVisitor
    {
        public void Visit(ODataQuery query, StringBuilder sqlBuilder, List<QueryParameter> queryParameters)
        {
            if (query.Skip.HasValue)
            {
                if (sqlBuilder.HasOffsetAlready())
                    return;
                var top = query.Top.HasValue ? query.Top.Value : 1;
                sqlBuilder.Append($" OFFSET {query.Skip.Value} LIMIT {top}");
            }
        }
    }
}
