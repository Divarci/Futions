using Core.Domain.Entities.System.AuditLogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Persistence.Configurations.System;

public class AuditLogConfig : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.Property(c => c.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(c => c.EntityId)
            .IsRequired();

        builder.HasIndex(c => c.EntityId);

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


        builder.Property(c => c.TenantId)
            .IsRequired();

        builder.HasIndex(c => c.TenantId);
    }
}
