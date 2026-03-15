using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.ValueObjects.AuditStampValueObject;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.System.AuditLogs.Interfaces;

public interface IAuditLogService
{
    /// <summary>
    /// Retrieves a paginated list of audit logs according to the specified parameters.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="sortBy">The field to sort by.</param>
    /// <param name="isAscending">Sort direction: true for ascending, false for descending.</param>
    /// <param name="filterQuery">Optional filter string.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of audit logs.</returns>
    Task<PaginatedResult<AuditLog[]>> GetPaginatedAsync(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filterQuery,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an audit log by its unique identifier.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="id">The audit log ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the audit log if found.</returns>
    Task<Result<AuditLog>> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new audit log entity with the given created stamp.
    /// </summary>
    /// <param name="entityId">The entity ID.</param>
    /// <param name="description">The description of the audit log.</param>
    /// <param name="createModel">The audit stamp create model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created audit log.</returns>
    Task<Result<AuditLog>> CreateAsync(
        Guid entityId,
        string description,
        AuditStampCreateModel createModel,
        CancellationToken cancellationToken = default);  
}
