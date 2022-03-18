namespace ProductServer.API.Models
{
    public class CreateProductRequest
    {
        public Guid Id { get; set; }
        public string? Denomination { get; set; }
        public decimal Price { get; set; }
    }
}
