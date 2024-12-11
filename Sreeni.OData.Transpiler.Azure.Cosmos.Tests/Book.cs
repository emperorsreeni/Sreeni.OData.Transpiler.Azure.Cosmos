using Newtonsoft.Json;

namespace Sreeni.OData.Transpiler.Azure.Cosmos.Tests
{
    public class Book
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public Author Author { get; set; }
        public string PartitionKey { get; set; }
        public Category Category { get; set; }
        public Tag[] Tags { get; set; }
    }
}
