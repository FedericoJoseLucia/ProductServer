namespace ProductServer.API.Models
{
    public class CreateProductRequest
    {
        public Guid Id { get; set; }
        public int ExternalCode { get; set; }
        public string Denomination { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
