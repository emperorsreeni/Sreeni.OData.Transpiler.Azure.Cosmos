using Sreeni.OData.Transpiler.Core;
using System.Text;

namespace Sreeni.OData.Transpiler.Azure.Cosmos
{
    public class ExpandVisitor : IODataQueryVisitor
    {
        public void Visit(ODataQuery query, StringBuilder sqlBuilder, List<QueryParameter> parameters)
        {
            //No implementation needed for Cosmos
        }
    }
}
