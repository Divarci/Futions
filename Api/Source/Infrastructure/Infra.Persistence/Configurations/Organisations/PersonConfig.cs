using Core.Domain.Entities.Organisations.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Persistence.Configurations.Organisations;

public class PersonConfig : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(p => p.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.TenantId)
            .IsRequired();

        builder.HasIndex(p => p.TenantId);

        builder.OwnsOne(p => p.Fullname, fullname =>
        {
            fullname.Property(f => f.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            fullname.Property(f => f.LastName)
                .HasMaxLength(50)
                .IsRequired();
        });        
    }
}
