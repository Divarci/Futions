using Core.Domain.Entities.Auditing.AuditLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Persistence.Configurations.System;

public class AuditLogConfig : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.OwnsOne(c => c.Created, audit =>
        {
            audit.Property(a => a.UserId)                
                .IsRequired();

            audit.Property(a => a.Username)
                .HasMaxLength(100)
                .IsRequired();

            audit.Property(a => a.Timestamp)
                .IsRequired();
        });

        builder.OwnsOne(c => c.Updated, audit =>
        {
            audit.Property(a => a.UserId)
                .IsRequired(false);

            audit.Property(a => a.Username)
                .HasMaxLength(100)
                .IsRequired(false);

            audit.Property(a => a.Timestamp)
                .IsRequired(false);
        });


        builder.Property(c => c.TenantId)
            .IsRequired();

        builder.HasIndex(c => c.TenantId);
    }
}
