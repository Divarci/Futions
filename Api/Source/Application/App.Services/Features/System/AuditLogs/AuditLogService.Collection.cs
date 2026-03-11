using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class AuditLogService
{
    public Task<Result<AuditLog[]>> GetPaginatedAsync(Guid tenantId, int page, int pageSize, string sortBy, bool isAscending, string? filter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}