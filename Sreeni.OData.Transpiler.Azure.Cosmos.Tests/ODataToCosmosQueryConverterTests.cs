using Shouldly;

namespace Sreeni.OData.Transpiler.Azure.Cosmos.Tests
{
    [TestClass]
    public class ODataToCosmosQueryConverterTests
    {
        private ODataAzureCosmosQueryTranslator sut;

        [TestInitialize]
        public void Setup()
        {
            sut = new ODataAzureCosmosQueryTranslator();
        }

        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterIsNotNullOrEmpty()
        {
            // Arrange
            var query = "$filter=Name eq 'John'";

            // Act
            var result = sut.Translate(query);

            // Assert
            result.Query.ShouldBe("SELECT * FROM c WHERE c.Name = @param0");
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "John");
        }

        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterContainsContainsFunction()
        {
            // Arrange
            var query = "$filter=contains(Name, 'John')";

            // Act
            var result = sut.Translate(query);

            // Assert
            result.Query.ShouldBe("SELECT * FROM c WHERE CONTAINS(c.Name, @param0)");
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "John");
        }

        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterContainsInFunction()
        {
            // Arrange
            var query = "$filter=Name in ('John', 'Doe')";

            // Act
            var result = sut.Translate(query);

            // Assert
            result.Query.ShouldBe("SELECT * FROM c WHERE c.Name IN (@param0, @param1)");
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "John");
            result.Parameters.ShouldContain(p => p.Name == "@param1" && (string)p.Value == "Doe");
        }

        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterContainsBetweenFunction()
        {
            // Arrange
            var query = "$filter=Age between 20 and 30";

            // Act
            var result = sut.Translate(query);

            // Assert
            result.Query.ShouldBe("SELECT * FROM c WHERE c.Age BETWEEN @param0 AND @param1");
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (int)p.Value == 20);
            result.Parameters.ShouldContain(p => p.Name == "@param1" && (int)p.Value == 30);
        }

        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterContainsBetweenFunctionWithDateTimeValues()
        {
            // Arrange
            var query = "$filter=CreatedDate between '2021-01-01T00:00:00' and '2021-12-31T23:59:59'";

            // Act
            var result = sut.Translate(query);

            // Assert
            result.Query.ShouldBe("SELECT * FROM c WHERE c.CreatedDate BETWEEN @param0 AND @param1");
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "2021-01-01T00:00:00");
            result.Parameters.ShouldContain(p => p.Name == "@param1" && (string)p.Value == "2021-12-31T23:59:59");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterIsNull()
        {
            // Arrange
            var query = (string)null;

            // Act
            var result = sut.Translate(query);

        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterIsInvalid()
        {
            // Arrange
            var query = "$filter=&$select=";

            // Act
            var result = sut.Translate(query);

        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterIsEmpty()
        {
            // Arrange
            var query = string.Empty;

            // Act
            var result = sut.Translate(query);

        }

        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterHasSelectFilterOrderByTopAndSkip()
        {
            // Arrange
            var query = "$select=Name&$filter=Name eq 'John'&$orderby=Name desc&$top=10&$skip=5";
            // Act
            var result = sut.Translate(query);
            // Assert
            result.Query.ShouldBe("SELECT c.Name FROM c WHERE c.Name = @param0 ORDER BY c.Name DESC OFFSET 5 LIMIT 10");
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "John");
        }

        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterHasSelectFilterOrderByTopAndSkipAndFilterContainsContainsFunction()
        {
            // Arrange
            var query = "$select=Name&$filter=contains(Name, 'John')&$orderby=Name desc&$top=10&$skip=5";
            // Act
            var result = sut.Translate(query);
            // Assert
            result.Query.ShouldBe("SELECT c.Name FROM c WHERE CONTAINS(c.Name, @param0) ORDER BY c.Name DESC OFFSET 5 LIMIT 10");
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "John");
        }

        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterHasSelectFilterOrderByTopAndSkipAndFilterContainsInFunction()
        {
            // Arrange
            var query = "$select=Name&$filter=Name in ('John', 'Doe')&$orderby=Name desc&$top=10&$skip=5";
            // Act
            var result = sut.Translate(query);
            // Assert
            result.Query.ShouldBe("SELECT c.Name FROM c WHERE c.Name IN (@param0, @param1) ORDER BY c.Name DESC OFFSET 5 LIMIT 10");
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "John");
            result.Parameters.ShouldContain(p => p.Name == "@param1" && (string)p.Value == "Doe");
        }

        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterHasSelectFilterOrderByTopAndSkipAndFilterContainsBetweenFunction()
        {
            // Arrange
            var query = "$select=Name&$filter=Age between 20 and 30&$orderby=Name desc&$top=10&$skip=5";
            // Act
            var result = sut.Translate(query);
            // Assert
            result.Query.ShouldBe("SELECT c.Name FROM c WHERE c.Age BETWEEN @param0 AND @param1 ORDER BY c.Name DESC OFFSET 5 LIMIT 10");
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (int)p.Value == 20);
            result.Parameters.ShouldContain(p => p.Name == "@param1" && (int)p.Value == 30);
        }

        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterHasSelectFilterOrderByTopAndSkipAndFilterContainsBetweenFunctionWithDateTimeValues()
        {
            // Arrange
            var query = "$select=Name&$filter=CreatedDate between '2021-01-01T00:00:00' and '2021-12-31T23:59:59'&$orderby=Name desc&$top=10&$skip=5";
            // Act
            var result = sut.Translate(query);
            // Assert
            result.Query.ShouldBe("SELECT c.Name FROM c WHERE c.CreatedDate BETWEEN @param0 AND @param1 ORDER BY c.Name DESC OFFSET 5 LIMIT 10");
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "2021-01-01T00:00:00");
            result.Parameters.ShouldContain(p => p.Name == "@param1" && (string)p.Value == "2021-12-31T23:59:59");
        }

        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterHasSelectFilterOrderByTopAndSkipAndFilterContainsBetweenFunctionWithDateTimeValuesAndFilterContainsContainsFunction()
        {
            // Arrange
            var query = "$select=Name&$filter=CreatedDate between '2021-01-01T00:00:00' and '2021-12-31T23:59:59' and contains(Name, 'John')&$orderby=Name desc&$top=10&$skip=5";
            // Act
            var result = sut.Translate(query);
            // Assert
            result.Query.ShouldBe("SELECT c.Name FROM c WHERE c.CreatedDate BETWEEN @param1 AND @param2 AND CONTAINS(c.Name, @param0) ORDER BY c.Name DESC OFFSET 5 LIMIT 10");
            result.Parameters.ShouldContain(p => p.Name == "@param1" && (string)p.Value == "2021-01-01T00:00:00");
            result.Parameters.ShouldContain(p => p.Name == "@param2" && (string)p.Value == "2021-12-31T23:59:59");
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "John");
        }

        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_WhenFilterHasSelectFilterOrderByTopAndSkipAndFilterContainsBetweenFunctionWithDateTimeValuesAndFilterContainsInFunction()
        {
            // Arrange
            var query = "$select=Name&$filter=CreatedDate between '2021-01-01T00:00:00' and '2021-12-31T23:59:59' and Name in ('John', 'Doe')&$orderby=Name desc&$top=10&$skip=5";
            // Act
            var result = sut.Translate(query);
            // Assert
            result.Query.ShouldBe("SELECT c.Name FROM c WHERE c.CreatedDate BETWEEN @param2 AND @param3 AND c.Name IN (@param0, @param1) ORDER BY c.Name DESC OFFSET 5 LIMIT 10");
            result.Parameters.ShouldContain(p => p.Name == "@param2" && (string)p.Value == "2021-01-01T00:00:00");
            result.Parameters.ShouldContain(p => p.Name == "@param3" && (string)p.Value == "2021-12-31T23:59:59");
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "John");
            result.Parameters.ShouldContain(p => p.Name == "@param1" && (string)p.Value == "Doe");
        }

        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_forNestedSelectedFields()
        {
            // Arrange
            var query = "$select=Name,Price,Category,Details/Manufacturer,Details/ReleaseDate";
            // Act
            var result = sut.Translate(query);
            // Assert
            result.Query.ShouldBe("SELECT c.Name, c.Price, c.Category, c.Details.Manufacturer as DetailsManufacturer, c.Details.ReleaseDate as DetailsReleaseDate FROM c");
        }


        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_forComplexQuery()
        {
            // Arrange
            var query = "$select=Name,Price,Category,Details/Manufacturer,Details/ReleaseDate&$filter=Price gt 20 and Category eq 'Electronics' and (Stock gt 100 or Discount gt 10 or contains(Name, 'Phone')) and Details/ReleaseDate between '2023-01-01T00:00:00Z' and '2023-12-31T23:59:59Z'&$orderby=Name asc,Price desc&$top=10&$skip=5";
            // Act
            var result = sut.Translate(query);
            // Assert
            result.Query.ShouldBe("SELECT c.Name, c.Price, c.Category, c.Details.Manufacturer as DetailsManufacturer, c.Details.ReleaseDate as DetailsReleaseDate FROM c WHERE c.Price > @param3 AND c.Category = @param4 AND (c.Stock > @param5 OR c.Discount > @param6 OR CONTAINS(c.Name, @param0)) AND c.Details.ReleaseDate BETWEEN @param1 AND @param2 ORDER BY c.Name ASC, c.Price DESC OFFSET 5 LIMIT 10");
            result.Parameters.ShouldContain(p => p.Name == "@param3" && (int)p.Value == 20);
            result.Parameters.ShouldContain(p => p.Name == "@param4" && (string)p.Value == "Electronics");
            result.Parameters.ShouldContain(p => p.Name == "@param5" && (int)p.Value == 100);
            result.Parameters.ShouldContain(p => p.Name == "@param6" && (int)p.Value == 10);
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "Phone");
            result.Parameters.ShouldContain(p => p.Name == "@param1" && (string)p.Value == "2023-01-01T00:00:00Z");
            result.Parameters.ShouldContain(p => p.Name == "@param2" && (string)p.Value == "2023-12-31T23:59:59Z");


        }

        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_forComplexQuery_withdot()
        {
            // Arrange
            var query = "$select=Name,Price,Category,Details.Manufacturer,Details.ReleaseDate&$filter=Price gt 20 and Category eq 'Electronics' and (Stock gt 100 or Discount gt 10 or contains(Name, 'Phone')) and Details/ReleaseDate between '2023-01-01T00:00:00Z' and '2023-12-31T23:59:59Z'&$orderby=Name asc,Price desc&$top=10&$skip=5";
            // Act
            var result = sut.Translate(query);
            // Assert
            result.Query.ShouldBe("SELECT c.Name, c.Price, c.Category, c.Details.Manufacturer as DetailsManufacturer, c.Details.ReleaseDate as DetailsReleaseDate FROM c WHERE c.Price > @param3 AND c.Category = @param4 AND (c.Stock > @param5 OR c.Discount > @param6 OR CONTAINS(c.Name, @param0)) AND c.Details.ReleaseDate BETWEEN @param1 AND @param2 ORDER BY c.Name ASC, c.Price DESC OFFSET 5 LIMIT 10");
            result.Parameters.ShouldContain(p => p.Name == "@param3" && (int)p.Value == 20);
            result.Parameters.ShouldContain(p => p.Name == "@param4" && (string)p.Value == "Electronics");
            result.Parameters.ShouldContain(p => p.Name == "@param5" && (int)p.Value == 100);
            result.Parameters.ShouldContain(p => p.Name == "@param6" && (int)p.Value == 10);
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "Phone");
            result.Parameters.ShouldContain(p => p.Name == "@param1" && (string)p.Value == "2023-01-01T00:00:00Z");
            result.Parameters.ShouldContain(p => p.Name == "@param2" && (string)p.Value == "2023-12-31T23:59:59Z");


        }

        //write unit test for given case: Convert should return query result for complex query with nested fields and nested group
        [TestMethod]
        public void Convert_ShouldReturnQueryResult_WithCorrectQueryAndParameters_forComplexQuery_withNestedFields_andNestedGroup()
        {
            // Arrange
            var query = "$select=Name,Price,Category,Details/Manufacturer,Details/ReleaseDate&$filter=Price gt 20 and Category eq 'Electronics' and (Stock gt 100 or (Discount gt 10 and contains(Name, 'Phone')) or Details/ReleaseDate between '2023-01-01T00:00:00Z' and '2023-12-31T23:59:59Z')&$orderby=Name asc,Price desc&$top=10&$skip=5";
            // Act
            var result = sut.Translate(query);
            // Assert
            result.Query.ShouldBe("SELECT c.Name, c.Price, c.Category, c.Details.Manufacturer as DetailsManufacturer, c.Details.ReleaseDate as DetailsReleaseDate FROM c WHERE c.Price > @param3 AND c.Category = @param4 AND (c.Stock > @param5 OR (c.Discount > @param6 AND CONTAINS(c.Name, @param0)) OR c.Details.ReleaseDate BETWEEN @param1 AND @param2) ORDER BY c.Name ASC, c.Price DESC OFFSET 5 LIMIT 10");
            result.Parameters.ShouldContain(p => p.Name == "@param3" && (int)p.Value == 20);
            result.Parameters.ShouldContain(p => p.Name == "@param4" && (string)p.Value == "Electronics");
            result.Parameters.ShouldContain(p => p.Name == "@param5" && (int)p.Value == 100);
            result.Parameters.ShouldContain(p => p.Name == "@param6" && (int)p.Value == 10);
            result.Parameters.ShouldContain(p => p.Name == "@param0" && (string)p.Value == "Phone");
            result.Parameters.ShouldContain(p => p.Name == "@param1" && (string)p.Value == "2023-01-01T00:00:00Z");
            result.Parameters.ShouldContain(p => p.Name == "@param2" && (string)p.Value == "2023-12-31T23:59:59Z");
        }
    }
}
