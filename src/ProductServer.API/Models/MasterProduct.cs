namespace ProductServer.API.Models
{
    public class MasterProduct
    {
        public Guid Id { get; set; }
        public int ExternalCode { get; set; }
        public string Denomination { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ProductState State { get; set; }

        public Guid? LastCreatedProductId { get; set; }
        public int? LastCreatedProductExternalCode { get; set; }

        public string? Department { get; set; }
        public int? Stock { get; set; }
    }
}
