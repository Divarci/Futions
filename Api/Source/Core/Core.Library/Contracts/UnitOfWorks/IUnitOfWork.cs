using Core.Library.ResultPattern;

namespace Core.Library.Contracts.UnitOfWorks;

/// <summary>
/// Defines the contract for the Unit of Work pattern, coordinating the writing of changes.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persists all pending changes to the data store.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous commit operation.</returns>
    Task CommitAsync(CancellationToken cancellationToken = default);
}