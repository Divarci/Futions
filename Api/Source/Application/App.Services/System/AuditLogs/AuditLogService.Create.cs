using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.System.AuditLogs.Models;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class AuditLogService
{
    public Task<Result<AuditLog>> CreateAsync(Guid tenantId, AuditLogCreateModel createModel, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}