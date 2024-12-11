using Sreeni.OData.Transpiler.Core;
using System.Text;

namespace Sreeni.OData.Transpiler.Azure.Cosmos
{
    public class SelectVisitor : IODataQueryVisitor
    {
        public void Visit(ODataQuery query, StringBuilder sqlBuilder,List<QueryParameter> queryParameters)
        {
            if (query.Select != null && query.Select.Count > 0)
            {
                sqlBuilder.Append("SELECT ");
                sqlBuilder.Append(string.Join(", ", query.Select.Select(s => s.CheckAndAddAlias())));
                sqlBuilder.Append(" FROM c");
            }
            else
            {
                sqlBuilder.Append("SELECT * FROM c");
            }
        }


    }
}
