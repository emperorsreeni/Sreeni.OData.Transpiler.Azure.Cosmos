using Sreeni.OData.Transpiler.Core;
using System.Text;

namespace Sreeni.OData.Transpiler.Azure.Cosmos
{
    public static class QueryExtensions
    {
        public static string AddParameter(this List<QueryParameter> parameters, string key, object value)
        {
            string paramName = $"@param{parameters.Count}";
            parameters.Add(new QueryParameter { Name = paramName, Value = value });
            return paramName;
        }

        public static bool IsEmpty(this ODataQuery query)
        {
            return string.IsNullOrEmpty(query.Filter) 
                && query.OrderBy == null 
                && query.Select == null 
                && query.Top == null
                && query.Skip == null;
        }

        public static bool HasOffsetAlready(this StringBuilder query)
        {
            var queryStr = query.ToString();
            return queryStr.Contains("OFFSET");
        }

        public static object GetValue(this string value)
        {
            //deduct the value types and return the value
            if (value == "true" || value == "false")
            {
                return bool.Parse(value);
            }
            else if (int.TryParse(value, out int intValue))
            {
                return intValue;
            }
            else if (double.TryParse(value, out double doubleValue))
            {
                return doubleValue;
            }
            else
            {
                return value;
            }
        }
        public static string AddOrderByField(this string field)
        {
            var cleanedField = field.Trim().Replace('/', '.'); 
            var expressions = cleanedField.Split(' ');
            if (expressions.Length > 1)
            {
                var orderDirection = expressions[1].ToUpperInvariant();
                if (orderDirection == "ASC" || orderDirection == "DESC")
                {
                    return $"c.{expressions[0]} {orderDirection}";
                }
                throw new ArgumentException("Invalid OrderBy expression");
            }
            return $"c.{field} ASC";
        }

        public static string CleanNestedField(this string field)
        {
            return field.Trim().Replace('/', '.');
        }
        public static string CheckAndAddAlias(this string field)
        {
            var result = field.Trim().Replace('/', '.');
            if (result.Contains("."))
            {
                if (!result.Contains(" as "))
                {
                    return $"c.{result} as {result.Replace(".", "")}";
                }
            }
            return $"c.{result}"; ;
        }
    }
}
