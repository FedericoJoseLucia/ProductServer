namespace ProductServer.Application.Services.ProductExternalServerService
{
    public class ProductExternalDataDto
    {
        public ProductExternalDataDto(string department, int stock)
        {
            Department = department;
            Stock = stock;
        }

        public string Department { get; set; }
        public int Stock { get; set; }
    }
}
