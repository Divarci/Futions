using Core.Domain.Entities.System.AuditLogs;
using Core.Library.ResultPattern;
using Infra.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infra.Persistence.Repositories.System.AuditLogs;

internal sealed partial class AuditLogRepository
{
    public async Task<Result<AuditLog[]>> GetPaginatedAsync(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<AuditLog>()
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x =>
                EF.Functions.Like(x.Created.Username, $"%{filter}%"))
            .OrderByIf(isAscending, sortBy);

        AuditLog[] logs = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return Result<AuditLog[]>.Success(
            message: "Audit logs retrieved successfully.",
            data: logs,
            statusCode: HttpStatusCode.OK);
    }
}
