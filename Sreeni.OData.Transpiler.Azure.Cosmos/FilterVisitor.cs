using Sreeni.OData.Transpiler.Core;
using System.Text;
using System.Text.RegularExpressions;

namespace Sreeni.OData.Transpiler.Azure.Cosmos
{
    public class FilterVisitor : IODataQueryVisitor
    {
        public void Visit(ODataQuery query, StringBuilder sqlBuilder, List<QueryParameter> parameters)
        {
            if (!string.IsNullOrEmpty(query.Filter))
            {
                var filterBuilder = new FilterBuilder(query.Filter, parameters);
                sqlBuilder.Append(" WHERE " + filterBuilder.Build());
            }
        }

    }

    public class FilterBuilder
    {
        private string _filter;
        private readonly List<QueryParameter> _parameters;

        public FilterBuilder(string filter, List<QueryParameter> parameters)
        {
            _filter = filter;
            _parameters = parameters;
        }

        public string Build()
        {
            return ReplaceOperators()
                       .BuildContainsFunction()
                       .BuildInFunction()
                       .BuildBetweenFunction()
                       .BuildConditionalOperators()
                       .Filter();
        }

        private FilterBuilder ReplaceOperators()
        {
            _filter = _filter.Replace(" eq ", " = ")
                             .Replace(" ne ", " != ")
                             .Replace(" gt ", " > ")
                             .Replace(" lt ", " < ")
                             .Replace(" ge ", " >= ")
                             .Replace(" le ", " <= ")
                             .Replace(" and ", " AND ")
                             .Replace(" or ", " OR ");
            return this;
        }

        private FilterBuilder BuildContainsFunction()
        {
            _filter = Regex.Replace(_filter, @"([ (]?)contains\(([^,]+),([^\)]+)\)([ )]?)", match =>
            {
                var openParenthesis = match.Groups[1].Value;
                var field = match.Groups[2].Value.CleanNestedField();
                var value = match.Groups[3].Value.Trim().Trim('\'');
                var closeParenthesis = match.Groups[4].Value;
                var paramName = _parameters.AddParameter(field, value);
                return $"{openParenthesis}CONTAINS(c.{field}, {paramName}){closeParenthesis}";
            });
            return this;
        }

        private FilterBuilder BuildInFunction()
        {
            _filter = Regex.Replace(_filter, @"([ (]?)([^ ]+) in \(([^)]+)\)([ )]?)", match =>
            {
                var openParenthesis = match.Groups[1].Value;
                var field = match.Groups[2].Value.CleanNestedField();
                var values = match.Groups[3].Value.Split(',').Select(v => v.Trim().Trim('\'')).ToArray();
                var closeParenthesis = match.Groups[4].Value;
                var paramNames = values.Select(v => _parameters.AddParameter(field, v));
                return $"{openParenthesis}c.{field} IN ({string.Join(", ", paramNames)}){closeParenthesis}";
            });
            return this;
        }

        private FilterBuilder BuildBetweenFunction()
        {
            _filter = Regex.Replace(_filter, @"([ (]?)([^ ]+) between ([^ ]+) AND ([^ )]+)([ )]?)", match =>
            {
                var openParenthesis = match.Groups[1].Value;
                var field = match.Groups[2].Value.CleanNestedField();
                var startValue = match.Groups[3].Value.Trim().Trim('\'');
                var endValue = match.Groups[4].Value.Trim().Trim('\'');
                var closeParenthesis = match.Groups[5].Value;
                var paramName1 = _parameters.AddParameter(field, startValue.GetValue());
                var paramName2 = _parameters.AddParameter(field, endValue.GetValue());
                return $"{openParenthesis}c.{field} BETWEEN {paramName1} AND {paramName2}{closeParenthesis}";
            });
            return this;
        }

        private FilterBuilder BuildConditionalOperators()
        {
            _filter = Regex.Replace(_filter, @"([ (]?)([\w/.]+) (=|!=|>|>=|<|<=) ([^ )]+)([ )]?)", match =>
            {
                var openParenthesis = match.Groups[1].Value;
                var field = match.Groups[2].Value.CleanNestedField();
                var op = match.Groups[3].Value.Trim();
                var value = match.Groups[4].Value.Trim().Trim('\'');
                var closeParenthesis = match.Groups[5].Value;
                var paramName = _parameters.AddParameter(field, value.GetValue());
                return $"{openParenthesis}c.{field} {op} {paramName}{closeParenthesis}";
            });
            return this;
        }

        private string Filter() {
            return _filter;
        }
    }
}
