using Core.Domain.ValueObjects.AuditStampValueObject;

namespace Core.Domain.Entities.System.AuditLogs.Models;

public sealed record AuditLogCreateModel
{
    public required Guid TenantId { get; init; }
    public required AuditStampModel CreatedStampModel { get; init; }
}
