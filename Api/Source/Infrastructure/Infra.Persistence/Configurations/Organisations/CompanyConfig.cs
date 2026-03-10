using Core.Domain.Entities.Organisations.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Persistence.Configurations.Organisations;

public class CompanyConfig : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.TenantId)
            .IsRequired();

        builder.HasIndex(c => c.TenantId);

        builder.OwnsOne(c => c.Address, address =>
        {
            address.Property(a => a.LineOne)
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.LineTwo)
                .HasMaxLength(100);

            address.Property(a => a.LineThree)
                .HasMaxLength(100);

            address.Property(a => a.LineFour)
                .HasMaxLength(100);

            address.Property(a => a.Postcode)
                .HasMaxLength(20)
                .IsRequired();
        });

        builder.Property(c => c.IsDeleted)
            .IsRequired();
    }
}
