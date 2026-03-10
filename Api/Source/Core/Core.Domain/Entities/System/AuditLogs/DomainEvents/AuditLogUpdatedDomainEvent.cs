using Core.Library.Contracts.DomainEvents.Publish;

namespace Core.Domain.Entities.System.AuditLogs.DomainEvents;

public sealed class AuditLogUpdatedDomainEvent(
    Guid auditLogId) : DomainEvent
{
    public Guid AuditLogId { get; } = auditLogId;
}
