namespace Sreeni.OData.Transpiler.Azure.Cosmos.Tests
{
    public class BookDetail
    {
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string AuthorName { get; set; }
        public Tag[] Tags { get; set; }
    }
}
