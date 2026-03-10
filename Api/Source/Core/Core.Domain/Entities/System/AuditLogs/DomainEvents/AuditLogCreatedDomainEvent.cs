using Core.Library.Contracts.DomainEvents.Publish;

namespace Core.Domain.Entities.System.AuditLogs.DomainEvents;

public sealed class AuditLogCreatedDomainEvent(
    Guid auditLogId) : DomainEvent
{
    public Guid AuditLogId { get; } = auditLogId;
}
