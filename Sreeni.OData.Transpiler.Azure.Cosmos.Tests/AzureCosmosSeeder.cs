using Microsoft.Azure.Cosmos;
using AutoBogus;

namespace Sreeni.OData.Transpiler.Azure.Cosmos.Tests
{
    [TestClass]
    public class AzureCosmosSeeder
    {
        private List<Product> CreateProducts()
        {
            var products = new AutoFaker<Product>()
                .RuleFor(fake => fake.Id, fake => Guid.NewGuid().ToString())
                .RuleFor(fake => fake.Name, fake => fake.Commerce.ProductName())
                .RuleFor(fake => fake.Description, fake => fake.Commerce.ProductDescription())
                .RuleFor(fake => fake.Price, (fake,d) => fake.Random.Number(100, 1000))
                .RuleFor(fake => fake.Category, fake => fake.Commerce.Categories(1).First())
                .RuleFor(fake => fake.StockQuantity, fake => fake.Random.Number(1, 100))
                .RuleFor(fake => fake.CreatedDate, fake => fake.Date.Past())
                .RuleFor(fake => fake.UpdatedDate, fake => fake.Date.Recent())
                .Generate(1000);

            return products;

        }
        private List<Book> CreateBooks()
        {
            var books = new AutoFaker<Book>()
                .RuleFor(fake => fake.Id, fake => Guid.NewGuid().ToString())
                .RuleFor(fake => fake.Title, fake => fake.Commerce.ProductName())
                .RuleFor(fake => fake.ISBN, fake => fake.Commerce.Ean13())
                .RuleFor(fake => fake.PartitionKey, fake => "Book")
                .RuleFor(fake => fake.Author, fake => AutoFaker.Generate<Author>())
                .RuleFor(fake => fake.Category, fake => AutoFaker.Generate<Category>())
                .RuleFor(fake => fake.Tags, fake => AutoFaker.Generate<Tag>(5).ToArray())
                .Generate(200);
            return books;
        }
        //[TestMethod]
        public async Task SeedProducts()
        {
            //TODO: Add the connection string of Azure Cosmos DB
            var cosmosClient = new CosmosClient("");
            var databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync("SampleDb");
            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync("OData", "/Category");
            var container = containerResponse.Container;
            var products = CreateProducts();
            foreach (var product in products)
            {
                await container.CreateItemAsync(product, new PartitionKey(product.Category));
            }
        }

        //[TestMethod]
        public async Task SeedBooks()
        {
            //TODO: Add the connection string of Azure Cosmos DB
            var cosmosClient = new CosmosClient("");
            var databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync("SampleDb");
            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync("Books", "/PartitionKey");
            var container = containerResponse.Container;
            var books = CreateBooks();
            foreach (var book in books)
            {
                await container.CreateItemAsync(book, new PartitionKey("Book"));
            }
        }
    }
}
