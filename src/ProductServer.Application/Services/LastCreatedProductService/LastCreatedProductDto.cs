namespace ProductServer.Application.Services.LastCreatedProductService
{
    public class LastCreatedProductDto
    {
        public LastCreatedProductDto(Guid id, string denomination)
        {
            Id = id;
            Denomination = denomination;
        }

        public Guid Id { get; private set; }
        public string Denomination { get; private set; }
    }
}
