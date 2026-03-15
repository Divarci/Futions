namespace Core.Domain.ValueObjects.AuditStampValueObject;

public sealed record AuditStampCreateModel
{
    public required Guid TenantId { get; init; }
    public required DateTime Timestamp { get; init; }
    public required Guid UserId { get; init; }
    public required string Username { get; init; }
}
