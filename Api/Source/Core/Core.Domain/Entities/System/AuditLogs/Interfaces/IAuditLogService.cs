using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.System.AuditLogs.Models;
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
    /// <param name="mapper">A function to map the audit log entities to the desired DTO type.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of audit logs.</returns>
    Task<PaginatedResult<TDto[]>> GetPaginatedAsync<TDto>(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filterQuery,
        Func<AuditLog[], TDto[]> mapper,
        CancellationToken cancellationToken = default) where TDto : class;

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
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="entityId">The entity ID.</param>
    /// <param name="description">The description of the audit log.</param>
    /// <param name="createModel">The audit log create model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created audit log.</returns>
    Task<Result<AuditLog>> CreateAsync(
        Guid tenantId,
        Guid entityId,
        string description,
        AuditLogCreateModel createModel,
        CancellationToken cancellationToken = default);  
}
