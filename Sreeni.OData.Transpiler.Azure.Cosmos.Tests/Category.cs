namespace Sreeni.OData.Transpiler.Azure.Cosmos.Tests
{
    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Tag[] Tags { get; set; }

    }
}
