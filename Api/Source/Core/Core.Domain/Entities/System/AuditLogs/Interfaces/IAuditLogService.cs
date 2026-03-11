using Core.Domain.Entities.Auditing.AuditLogs;
using Core.Domain.Entities.System.AuditLogs.Models;
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
    /// <param name="filter">Optional filter string.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the paginated array of audit logs.</returns>
    Task<Result<AuditLog[]>> GetPaginatedAsync(
        Guid tenantId,
        int page,
        int pageSize,
        string sortBy,
        bool isAscending,
        string? filter,
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
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="createModel">The audit log create model.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created audit log.</returns>
    Task<Result<AuditLog>> CreateAsync(
        Guid tenantId,
        AuditLogCreateModel createModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the updated stamp on an existing audit log.
    /// The created stamp remains immutable.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="stampModel">The audit stamp model for the updated entry.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the updated audit log.</returns>
    Task<Result<AuditLog>> SetUpdated(
        Guid tenantId,
        AuditStampModel stampModel,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the total number of audit logs.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the total count of audit logs.</returns>
    Task<Result<int>> CountAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default);
}
