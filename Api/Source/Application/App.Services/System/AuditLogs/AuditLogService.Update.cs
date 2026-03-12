using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class AuditLogService
{
    public Task<Result<AuditLog>> SetUpdated(Guid tenantId, AuditStampModel stampModel, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}