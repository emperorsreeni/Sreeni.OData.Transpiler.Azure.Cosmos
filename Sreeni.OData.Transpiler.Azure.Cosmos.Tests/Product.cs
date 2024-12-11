using Newtonsoft.Json;

namespace Sreeni.OData.Transpiler.Azure.Cosmos.Tests
{
    public class Product
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } // Unique identifier for the product
        public string Name { get; set; } // Name of the product
        public string Description { get; set; } // Description of the product
        public decimal Price { get; set; } // Price of the product
        public string Category { get; set; } // Category of the product
        public int StockQuantity { get; set; } // Quantity in stock
        public DateTime CreatedDate { get; set; } // Date when the product was created
        public DateTime UpdatedDate { get; set; } // Date when the product was last updated
    }
}
