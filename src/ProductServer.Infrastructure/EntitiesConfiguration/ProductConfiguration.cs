using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductServer.Domain.Aggregates.Product;

namespace ProductServer.Infrastructure.EntitiesConfiguration
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(item => item.Id).ValueGeneratedNever();

            builder.Property(sale => sale.Price).HasPrecision(18, 8);

            builder.OwnsOne(x => x.State, builder =>
            {
                builder.Property(e => e.Id);
                builder.Property(e => e.Name);                
            }).Navigation(x => x.State).IsRequired();
        }
    }
}
