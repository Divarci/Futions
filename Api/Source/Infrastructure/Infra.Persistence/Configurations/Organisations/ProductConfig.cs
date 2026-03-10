using Core.Domain.Entities.Organisations.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Persistence.Configurations.Organisations;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.TenantId)
            .IsRequired();

        builder.HasIndex(c => c.TenantId);

        builder.Property(c => c.IsDeleted)
            .IsRequired();

        builder.HasOne(p => p.Company)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CompanyId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}