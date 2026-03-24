using Core.Domain.Entities.System.OutboxMessages;
using Core.Library.ResultPattern;

namespace App.Services.Features.System.OutboxMessages;

internal sealed partial class OutboxMessageService
{
    public async Task<Result<OutboxMessage>> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        // Get the outbox message by its ID using the repository
        return await _outboxMessageRepository
            .GetByIdAsync(id, cancellationToken);
    }
}