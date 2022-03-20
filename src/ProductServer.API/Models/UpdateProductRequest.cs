namespace ProductServer.API.Models
{
    public class UpdateProductRequest
    {
        public Guid Id { get; set; }
        public string? Denomination { get; set; }
        public decimal? Price { get; set; }
        public ProductState? State { get; set; }
    }
}
