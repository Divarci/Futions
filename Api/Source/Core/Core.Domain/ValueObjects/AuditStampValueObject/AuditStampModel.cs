namespace Core.Domain.ValueObjects.AuditStampValueObject;

public sealed record AuditStampModel
{
    public required DateTime Timestamp { get; init; }
    public required Guid UserId { get; init; }
    public required string Username { get; init; }
}
