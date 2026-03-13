using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class AuditLogService
{
    public async Task<Result<AuditLog>> GetByIdAsync(
        Guid tenantId, 
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        // Get the AuditLog entity from the database using the repository.
        return await _auditLogRepository
            .GetByIdAsync(tenantId, id, cancellationToken);            
    }
}