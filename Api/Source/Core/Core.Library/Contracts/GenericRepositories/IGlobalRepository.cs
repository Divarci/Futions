using Core.Library.Abstractions;
using Core.Library.ResultPattern;
namespace Core.Library.Contracts.GenericRepositories;
/// <summary>
/// Defines the repository contract for entities that are not scoped to a tenant.
/// Extends <see cref="IBaseRepository{TEntity}"/> with tenant-agnostic read operations.
/// </summary>
/// <typeparam name="TEntity">The entity type. Must derive from <see cref="BaseEntity"/>.</typeparam>
public interface IGlobalRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="entityId">The unique identifier of the entity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the entity if found.</returns>
    Task<Result<TEntity>> GetByIdAsync(
        Guid entityId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether an entity with the specified identifier exists.
    /// </summary>
    /// <param name="entityId">The unique identifier of the entity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating whether the entity exists.</returns>
    Task<Result<bool>> ExistsAsync(
        Guid entityId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the total number of entities in the repository.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the total count of entities.</returns>
    Task<Result<int>> CountAsync(
        CancellationToken cancellationToken = default);
}