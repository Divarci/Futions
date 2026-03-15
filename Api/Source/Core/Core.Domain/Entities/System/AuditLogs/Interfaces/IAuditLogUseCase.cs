using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.System.AuditLogs.Interfaces;

public interface IAuditLogUseCase
{
    /// <summary>
    /// Retrieves a paginated collection of audit logs based on the provided parameters.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="pageQuery">The page number.</param>
    /// <param name="pageSizeQuery">The page size.</param>
    /// <param name="sortByQuery">The field to sort by.</param>
    /// <param name="isAscendingQuery">Indicates whether the sorting should be in ascending order.</param>
    /// <param name="filterQuery">The filter criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A paginated result containing the audit log entities.</returns>
    Task<PaginatedResult<AuditLog[]>> GetPaginatedAsync(
        Guid tenantId,
        int? pageQuery,
        int? pageSizeQuery,
        string? sortByQuery,
        bool? isAscendingQuery,
        string? filterQuery,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an audit log by its ID.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The audit log ID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result containing the audit log if found, or an error if not.</returns>
    Task<Result<AuditLog>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default);

}
