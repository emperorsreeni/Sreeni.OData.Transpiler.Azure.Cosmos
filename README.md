# Sreeni.OData.Transpiler.Azure.Cosmos

Sreeni.OData.Transpiler.Azure.Cosmos is a .NET-based OData query translator specifically designed for Azure Cosmos DB. 

It translates OData queries into Cosmos DB SQL queries, enabling seamless integration and query translation for Azure Cosmos DB.

## Features

- Parse OData queries
- Convert OData queries to Cosmos DB SQL queries
- Support for common OData query options: `$select`, `$filter`, `$orderby`, `$top`, `$skip`, and `$expand`
- Support for nested fields in the select clause, filter expressions, and orderby expressions
- Execute translated queries against Azure Cosmos DB

## Getting Started

### Installation

To install the package, add the following dependency to your project:
```bash
dotnet add package Sreeni.OData.Transpiler.Azure.Cosmos
```
### Usage

This package provides the necessary components to translate and execute OData queries against Azure Cosmos DB. The following steps outline how to use the package:

1. **Create an instance of `ODataAzureCosmosClient`**:
   - You can create an instance using either a `CosmosClient` object or a connection string.
```csharp
using Microsoft.Azure.Cosmos; 
using Sreeni.OData.Transpiler.Azure.Cosmos;
// Using CosmosClient object 
var cosmosClient = new CosmosClient("your-cosmos-db-connection-string"); 
var client = new ODataAzureCosmosClient(cosmosClient, "your-database-name", "your-container-name");
// Using connection string 
var client = new ODataAzureCosmosClient("your-cosmos-db-connection-string", "your-database-name", "your-container-name");

```
2. **Execute OData queries**:
   - Use the `GetItemByQueryAsync` method to execute a query and get a single item.
   - Use the `GetListByQueryAsync` method to execute a query and get a list of items.
```csharp
// Example OData query 
string odataQuery = "$select=Name,Age&$filter=Age gt 30&$orderby=Name&$top=10&$skip=5";
// Get a single item 
var item = await client.GetItemByQueryAsync(odataQuery);
// Get a list of items 
var items = await client.GetListByQueryAsync(odataQuery);
```
### Implementation Details

The `ODataAzureCosmosClient` class uses the `ODataAzureCosmosQueryTranslator` to translate OData queries into Cosmos DB SQL queries. The translation process involves the following steps:

1. **Parse the OData query**:
   - The `ODataQueryParser` class parses the OData query string into an `ODataQuery` object.

2. **Translate the OData query**:
   - The `ODataAzureCosmosQueryTranslator` class translates the `ODataQuery` object into a Cosmos DB SQL query using various query visitors (`SelectVisitor`, `FilterVisitor`, `OrderByVisitor`, `TopVisitor`, `SkipVisitor`, `ExpandVisitor`).

3. **Execute the translated query**:
   - The `ODataAzureCosmosClient` class executes the translated query using the Azure Cosmos DB SDK.

### Example

Here is a complete example of how to use the `Sreeni.OData.Transpiler.Azure.Cosmos` package:
```csharp
using Microsoft.Azure.Cosmos; 
using Sreeni.OData.Transpiler.Azure.Cosmos; 
using System; using System.Threading.Tasks;

public class Program 
{ 
    public static async Task Main(string[] args) 
    { 
        var cosmosClient = new CosmosClient("your-cosmos-db-connection-string"); 
        var client = new ODataAzureCosmosClient(cosmosClient, "your-database-name", "your-container-name");
        string odataQuery = "$select=Name,Age&$filter=Age gt 30&$orderby=Name&$top=10&$skip=5";

        var items = await client.GetListByQueryAsync<YourEntityType>(odataQuery);

        foreach (var item in items)
        {
            Console.WriteLine(item);
        }
    }
}
```
## License

This project is licensed under the MIT License.