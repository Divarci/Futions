using Core.Domain.Entities.Organisations.CompanyPeople;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Persistence.Configurations.Organisations;

public class CompanyPersonConfig : IEntityTypeConfiguration<CompanyPerson>
{
    public void Configure(EntityTypeBuilder<CompanyPerson> builder)
    {
        builder.Property(op => op.Title)
            .HasMaxLength(100);      

        builder.HasOne(op => op.Company)
            .WithMany()
            .HasForeignKey(op => op.CompanyId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(op => op.Person)
            .WithMany()
            .HasForeignKey(op => op.PersonId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
