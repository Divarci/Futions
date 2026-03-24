using Core.Domain.Entities.System.OutboxMessages.Interfaces;

namespace App.Services.Features.System.OutboxMessages;

internal sealed partial class OutboxMessageService(
    IOutboxMessageRepository outboxMessageRepository) : IOutboxMessageService
{
    private readonly IOutboxMessageRepository _outboxMessageRepository = outboxMessageRepository;
}
