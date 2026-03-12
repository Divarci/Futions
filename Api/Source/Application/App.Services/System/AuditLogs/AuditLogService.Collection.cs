using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class AuditLogService
{
    public Task<Result<AuditLog[]>> GetPaginatedAsync(Guid tenantId, int? pageQuery, int? pageSizeQuery, string? sortByQuery, bool? isAscendingQuery, string? filterQuery, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}