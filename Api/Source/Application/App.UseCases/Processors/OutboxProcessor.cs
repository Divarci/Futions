using Core.Domain.Entities.System.OutboxMessages;
using Core.Domain.Entities.System.OutboxMessages.Interfaces;
using Core.Library.Contracts.UnitOfWorks;
using Core.Library.ResultPattern;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace App.UseCases.Processors;

internal partial class OutboxProcessor(
    ITransactionalUnitOfWork unitOfWork,
    IServiceScopeFactory serviceScopeFactory,
    IOutboxMessageService outboxMessageService) : IOutboxProcessor
{
    private readonly ITransactionalUnitOfWork _unitOfWork = unitOfWork;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IOutboxMessageService _outboxMessageService = outboxMessageService;

    public async Task<Result> ProcessOutboxMessagesAsync(int batchSize,
        JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {

            Result<IReadOnlyList<OutboxMessage>> outboxMessagesResult = await _outboxMessageService
                .GetUnprocessedMessagesAsync(batchSize, cancellationToken);

            if (outboxMessagesResult.IsFailureAndNoData)
                return Result.Failure(
                    message: outboxMessagesResult.Message,
                    statusCode: outboxMessagesResult.StatusCode);

            foreach (var outboxMessage in outboxMessagesResult.Data)
            {
                Exception? exception = null;

                try
                {
                    await InitialiseHandlersAsync(
                        outboxMessage,
                        jsonSerializerOptions,
                        _serviceScopeFactory,
                        cancellationToken);
                }
                catch (Exception caughtException)
                {
                    exception = caughtException;
                }

                return await UpdateAsync(_outboxMessageService, outboxMessage, exception);
            }

            return Result.Success("Outbox messages processed successfully.");
        }, cancellationToken);
    }
}
