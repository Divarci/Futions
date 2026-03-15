using Core.Domain.ValueObjects.AuditStampValueObject;

namespace Adapter.RestApi.Controllers.VersionOne.System.AuditLogs;

internal static class AuditLogMapper
{
    public static AuditStampCreateModel ToCreateModel(Guid userId, string username, Guid tenantId)
    {
        return new AuditStampCreateModel
        {
            TenantId = tenantId,
            UserId = userId,
            Username = username,
            Timestamp = DateTime.UtcNow,
        };
    }
}
