namespace ProductServer.Application.Services.LastCreatedProductService
{
    public class LastCreatedProductDto
    {
        public LastCreatedProductDto(Guid id, int externalCode)
        {
            Id = id;
            ExternalCode = externalCode;
        }

        public Guid Id { get; private set; }
        public int ExternalCode { get; private set; }
    }
}
