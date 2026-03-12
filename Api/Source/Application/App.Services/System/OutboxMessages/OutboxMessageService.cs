using Core.Domain.Entities.System.OutboxMessages.Interfaces;

namespace App.Services.Features.Organisations.Companies;

internal sealed partial class OutboxMessageService(
    IOutboxMessageRepository repository) : IOutboxMessageService
{
    private readonly IOutboxMessageRepository _repository = repository;
}
