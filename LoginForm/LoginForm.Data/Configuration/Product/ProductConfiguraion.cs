using LoginForm.Model.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoginForm.Data.Configuration.Product
{
    public class ProductConfiguraion : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.Property(t => t.RecordVersion).IsConcurrencyToken().IsRowVersion().IsRequired();
            builder.ToTable("Product");
        }
    }
}
