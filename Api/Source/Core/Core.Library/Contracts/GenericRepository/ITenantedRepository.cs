using Core.Library.Abstractions;
using Core.Library.ResultPattern;
namespace Core.Library.Contracts.GenericRepository;
/// <summary>
/// Defines the repository contract for tenant-scoped entities.
/// Extends <see cref="IBaseRepository{TEntity}"/> with tenant-aware read operations.
/// </summary>
/// <typeparam name="TEntity">The entity type. Must derive from <see cref="BaseEntity"/>.</typeparam>
public interface ITenantedRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    /// <summary>
    /// Retrieves an entity by its unique identifier within the specified tenant.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="tenantId">The unique identifier of the tenant.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the entity if found.</returns>
    Task<Result<TEntity>> GetByIdAsync(
        Guid id,
        Guid tenantId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether an entity with the specified identifier exists within the specified tenant.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="tenantId">The unique identifier of the tenant.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating whether the entity exists.</returns>
    Task<Result<bool>> ExistsAsync(
        Guid id,
        Guid tenantId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the total number of entities belonging to the specified tenant.
    /// </summary>
    /// <param name="tenantId">The unique identifier of the tenant.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the total count of entities.</returns>
    Task<Result<int>> CountAsync(
        Guid tenantId,
        CancellationToken cancellationToken = default);
}