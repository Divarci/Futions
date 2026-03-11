using Core.Domain.Entities.System.OutboxMessages;
using Core.Library.ResultPattern;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class OutboxMessageService
{
    public Task<Result<OutboxMessage>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}