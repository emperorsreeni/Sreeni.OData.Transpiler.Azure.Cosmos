using Shouldly;

namespace Sreeni.OData.Transpiler.Azure.Cosmos.Tests
{
    [TestClass]
    public class ODataAzureCosmosClientIntegrationTests
    {
        private readonly string _connectionString = "";
        private readonly string _database = "SampleDb";
        private readonly string _productContainer = "Products";
        private readonly string _bookContainerName = "Books";
        [TestMethod]
        public async Task GetAsync_ShouldReturnResults_for_given_product_id()
        {
            // Arrange
            var id = "e1eb6f89-0acb-462e-93f9-66d41ecc5032";
            var client = new ODataAzureCosmosClient(_connectionString, _database, _productContainer);
            var query = $"$filter=id eq '{id}'";
            // Act
            var result = await client.GetItemByQueryAsync<Product>(query);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id);

        }


        [TestMethod]
        public async Task GetListByQueryAsync_ShouldReturnResults_for_price_greater_than_300()
        {
            // Arrange
            var client = new ODataAzureCosmosClient(_connectionString, _database, _productContainer);
            var query = "$filter=Price gt 300";
            // Act
            var results = await client.GetListByQueryAsync<Product>(query);

            // Assert
            results.ShouldNotBeNull();
            results.Count().ShouldBeGreaterThan(0);
        }


        [TestMethod]
        public async Task GetListByQueryAsync_ShouldReturnResults_for_name_contains_shoes()
        {
            // Arrange
            var client = new ODataAzureCosmosClient(_connectionString, _database, _productContainer);
            var query = "$filter=contains(Name,'Shoes')";
            // Act
            var results = await client.GetListByQueryAsync<Product>(query);
            // Assert
            results.ShouldNotBeNull();
            results.Count().ShouldBeGreaterThan(0);
        }


        [TestMethod]
        public async Task GetListByQueryAsync_ShouldReturnResults_for_price_between_100_and_200()
        {
            // Arrange
            var client = new ODataAzureCosmosClient(_connectionString, _database, _productContainer);
            var query = "$filter=Price between 100 and 200";
            // Act
            var results = await client.GetListByQueryAsync<Product>(query);
            // Assert
            results.ShouldNotBeNull();
            results.Count().ShouldBeGreaterThan(0);
        }


        [TestMethod]
        public async Task GetListByQueryAsync_ShouldReturnResults_for_created_date_between_2024_01_01_and_2024_12_31()
        {
            // Arrange
            var client = new ODataAzureCosmosClient(_connectionString, _database, _productContainer);
            var query = "$filter=CreatedDate between 2024-01-01 and 2024-12-31";
            // Act
            var results = await client.GetListByQueryAsync<Product>(query);
            // Assert
            results.ShouldNotBeNull();
            results.Count().ShouldBeGreaterThan(0);
        }

        [TestMethod]
        public async Task GetListByQueryAsync_ShouldReturnResults_for_created_date_between_2024_01_01_and_2024_12_31_and_price_greater_than_300()
        {
            // Arrange
            var client = new ODataAzureCosmosClient(_connectionString, _database, _productContainer);
            var query = "$filter=(CreatedDate between '2024-01-01' and '2024-12-31') and Price gt 300";
            // Act
            var results = await client.GetListByQueryAsync<Product>(query);
            // Assert
            results.ShouldNotBeNull();
            results.Count().ShouldBeGreaterThan(0);
        }


        [TestMethod]
        public async Task GetListByQueryAsync_ShouldReturnResults_for_created_date_between_2024_01_01_and_2024_12_31_and_price_greater_than_300_and_name_contains_shoes_or_shirt()
        {
            // Arrange
            var client = new ODataAzureCosmosClient(_connectionString, _database, _productContainer);
            var query = "$filter=(CreatedDate between '2024-01-01' and '2024-12-31') and Price gt 300 and (contains(Name,'Shoes') or contains(Name,'Shirt'))";
            // Act
            var results = await client.GetListByQueryAsync<Product>(query);
            // Assert
            results.ShouldNotBeNull();
            results.Count().ShouldBeGreaterThan(0);
        }


        [TestMethod]
        public async Task GetListByQueryAsync_ShouldReturnResults_for_created_date_between_2024_01_01_and_2024_12_31_and_price_greater_than_300_and_name_contains_shoes_or_shirt_and_stock_quantity_greater_than_10()
        {
            // Arrange
            var client = new ODataAzureCosmosClient(_connectionString, _database, _productContainer);
            var query = "$filter=(CreatedDate between '2024-01-01' and '2024-12-31') and Price gt 300 and (contains(Name,'Shoes') or contains(Name,'Shirt')) and StockQuantity gt 10";
            // Act
            var results = await client.GetListByQueryAsync<Product>(query);
            // Assert
            results.ShouldNotBeNull();
            results.Count().ShouldBeGreaterThan(0);
        }


        [TestMethod]
        public async Task GetListByQueryAsync_ShouldReturnResults_for_category_in_industrial_and_jewelery_and_price_greater_than_300_and_name_contains_shoes_or_shirt_and_stock_quantity_greater_than_10()
        {
            // Arrange
            var client = new ODataAzureCosmosClient(_connectionString, _database, _productContainer);
            var query = "$filter=Category in ('Industrial','Jewelery') and Price gt 300 and (contains(Name,'Shoes') or contains(Name,'Shirt')) and StockQuantity gt 10";
            // Act
            var results = await client.GetListByQueryAsync<Product>(query);
            // Assert
            results.ShouldNotBeNull();
            results.Count().ShouldBeGreaterThan(0);

        }

        
        [TestMethod]
        public async Task GetListByQueryAsync_ShouldReturnResults_for_selected_fields_book_title_category_name_author_name_and_tags()
        {
            // Arrange
            var client = new ODataAzureCosmosClient(_connectionString, _database, _bookContainerName);
            var query = "$select=Title,Category/Name as CategoryName ,Author/Name as AuthorName,Tags";
            // Act
            var results = await client.GetListByQueryAsync<BookDetail>(query);
            // Assert
            results.ShouldNotBeNull();
            results.Count().ShouldBeGreaterThan(0);
        }
        
        [TestMethod]
        public async Task GetListByQueryAsync_ShouldReturnResults_for_selected_fields_book_title_category_name_and_tags()
        {
            // Arrange
            var client = new ODataAzureCosmosClient(_connectionString, _database, _bookContainerName);
            var query = "$select=Title,Category.Name as CategoryName,Tags";
            // Act
            var results = await client.GetListByQueryAsync<BookDetail>(query);
            // Assert
            results.ShouldNotBeNull();
            results.Count().ShouldBeGreaterThan(0);
        }

        
        [TestMethod]
        public async Task GetListByQueryAsync_ShouldReturnTop10Results()
        {
            // Arrange
            var client = new ODataAzureCosmosClient(_connectionString, _database, _bookContainerName);
            var query = "$top=10";
            // Act
            var results = await client.GetListByQueryAsync<Book>(query);
            // Assert
            results.ShouldNotBeNull();
            results.Count().ShouldBe(10);
        }
        
        [TestMethod]
        public async Task GetListByQueryAsync_ShouldReturnSkip100Results()
        {
            // Arrange
            var client = new ODataAzureCosmosClient(_connectionString, _database, _bookContainerName);
            var query = "$skip=100&$top=100";
            // Act
            var results = await client.GetListByQueryAsync<Book>(query);
            // Assert
            results.ShouldNotBeNull();
            results.Count().ShouldBe(100);
        }

        //write unit test for given case: GetListByQueryAsync should return ascending ordered results by price
        [TestMethod]
        public async Task GetListByQueryAsync_ShouldReturnAscendingOrderedResultsByPrice()
        {
            // Arrange
            var client = new ODataAzureCosmosClient(_connectionString, _database, _productContainer);
            var query = "$orderby=Price&$top=10";
            // Act
            var results = await client.GetListByQueryAsync<Product>(query);
            // Assert
            results.ShouldNotBeNull();
            results.Count().ShouldBeGreaterThan(0);
            results.First().Price.ShouldBe(100);
        }

    }
}
