using Core.Library.Contracts.GenericRepositories;
using Core.Library.ResultPattern;

namespace Core.Domain.Entities.System.OutboxMessages.Interfaces;

public interface IOutboxMessageRepository : IGlobalRepository<OutboxMessage>
{
    /// <summary>
    /// Retrieves a batch of unprocessed outbox messages pending dispatch.
    /// </summary>
    /// <param name="batchSize">The maximum number of messages to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A read-only list of unprocessed <see cref="OutboxMessage"/> instances.</returns>
    Task<Result<IReadOnlyList<OutboxMessage>>> GetUnprocessedMessagesAsync(
        int batchSize,
        CancellationToken cancellationToken = default);
}