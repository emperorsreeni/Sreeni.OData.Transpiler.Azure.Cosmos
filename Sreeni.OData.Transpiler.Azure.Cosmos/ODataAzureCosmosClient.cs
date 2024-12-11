using Microsoft.Azure.Cosmos;
using Sreeni.OData.Transpiler.Core;
namespace Sreeni.OData.Transpiler.Azure.Cosmos
{
    public class ODataAzureCosmosClient : IODataClient,IODataListClient
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;
        private readonly IODataQueryTranslator _queryConverter;

        public ODataAzureCosmosClient(CosmosClient cosmosClient, string database, string container)
        {
            _cosmosClient = cosmosClient;
            _container = _cosmosClient.GetContainer(database, container);
            _queryConverter = new ODataAzureCosmosQueryTranslator();

        }

        public ODataAzureCosmosClient(string connectionString, string database, string container)
        {
            _cosmosClient = new CosmosClient(connectionString);
            _container = _cosmosClient.GetContainer(database, container);
            _queryConverter = new ODataAzureCosmosQueryTranslator();
        }

        public async Task<T> GetItemByQueryAsync<T>(string query) where T : class
        {
            QueryDefinition queryDefinition = CreateQueryDefinition(query);

            var iterator = _container.GetItemQueryIterator<T>(queryDefinition);
            var results = new List<T>();

            var response = await iterator.ReadNextAsync();
            results.AddRange(response.ToList());

            var result = results.FirstOrDefault();

            return result;
        }

        private QueryDefinition CreateQueryDefinition(string query)
        {
            var queryResult = _queryConverter.Translate(query);

            if (queryResult == null)
            {
                throw new ArgumentException("Invalid query");
            }
            var queryDefinition = new QueryDefinition(queryResult.Query);

            foreach (var parameter in queryResult.Parameters)
            {
                queryDefinition.WithParameter(parameter.Name, parameter.Value);
            }

            return queryDefinition;
        }

        public async Task<IEnumerable<T>> GetListByQueryAsync<T>(string query) where T : class
        {
            var queryDefinition = CreateQueryDefinition(query);
            var iterator = _container.GetItemQueryIterator<T>(queryDefinition);
            var results = new List<T>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }
    }
}
