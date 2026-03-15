using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class AuditLogService
{
    public async Task<PaginatedResult<AuditLog[]>> GetPaginatedAsync(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filterQuery,
        CancellationToken cancellationToken = default)
    {
        // Get paginated list of audit logs
        Result<AuditLog[]> entityResult = await _auditLogRepository
            .GetPaginatedAsync(tenantId, page, pageSize, sortBy, isAscending, filterQuery, cancellationToken);

        if (entityResult.IsFailure)
            return PaginatedResult<AuditLog[]>.Failure(
                message: entityResult.Message,
                statusCode: entityResult.StatusCode);

        // Get total count of audit logs for pagination metadata
        Result<int> totalCountResult = await _auditLogRepository
            .CountAsync(tenantId, cancellationToken);

        if (totalCountResult.IsFailure)
            return PaginatedResult<AuditLog[]>.Failure(
                message: totalCountResult.Message,
                statusCode: totalCountResult.StatusCode);

        return PaginatedResult<AuditLog[]>.Success(
            message: "List retrieved successfully",
            data: entityResult.Data ?? [],
            pageNumber: page,
            pageSize: pageSize,
            totalCount: totalCountResult.Data,
            pageCount: (int)Math.Ceiling((double)totalCountResult.Data / pageSize));
    }
}