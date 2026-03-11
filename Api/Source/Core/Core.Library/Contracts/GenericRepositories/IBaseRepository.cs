using Core.Library.Abstractions;
using Core.Library.ResultPattern;
namespace Core.Library.Contracts.GenericRepositories;
/// <summary>
/// Defines the base repository contract for write operations that are
/// shared across all entities regardless of tenancy.
/// </summary>
/// <typeparam name="TEntity">The entity type. Must derive from <see cref="BaseEntity"/>.</typeparam>
public interface IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created entity.</returns>
    Task<Result<TEntity>> CreateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a collection of entities to the repository.
    /// </summary>
    /// <param name="entityCollection">The collection of entities to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating the outcome of the operation.</returns>
    Task<Result> CreateRangeAsync(
        IEnumerable<TEntity> entityCollection,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A result containing the updated entity.</returns>
    Result Update(TEntity entity);

    /// <summary>
    /// Updates a collection of existing entities in the repository.
    /// </summary>
    /// <param name="entityCollection">The collection of entities to update.</param>
    /// <returns>A result indicating the outcome of the operation.</returns>
    Result UpdateRange(IEnumerable<TEntity> entityCollection);

    /// <summary>
    /// Removes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A result indicating the outcome of the delete operation.</returns>
    Result Delete(TEntity entity);

    /// <summary>
    /// Removes a collection of entities from the repository.
    /// </summary>
    /// <param name="entityCollection">The collection of entities to delete.</param>
    /// <returns>A result indicating the outcome of the delete operation.</returns>
    Result DeleteRange(IEnumerable<TEntity> entityCollection);
}