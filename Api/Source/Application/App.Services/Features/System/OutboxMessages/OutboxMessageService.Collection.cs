using Core.Domain.Entities.System.OutboxMessages;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class OutboxMessageService
{
    public Task<Result<IReadOnlyList<OutboxMessage>>> GetUnprocessedMessagesAsync(int batchSize, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}