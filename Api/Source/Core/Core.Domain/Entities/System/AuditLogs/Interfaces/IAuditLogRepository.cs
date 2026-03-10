using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Library.Contracts.GenericRepository;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.System.AuditLogs.Interfaces;

public interface IAuditLogRepository : ITenantedRepository<AuditLog>
{
    /// <summary>
    /// Retrieves a paginated list of audit logs according to the specified parameters.
    /// </summary>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="isAscending">Sort direction: true for ascending, false for descending.</param>
    /// <param name="filter">Optional filter string.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of audit logs.</returns>
    Task<Result<AuditLog[]>> GetPaginatedAsync(
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
        CancellationToken cancellationToken = default);
}
