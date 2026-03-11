using Core.Library.ResultPattern;
using System.Text.Json;

namespace Core.Domain.Entities.System.OutboxMessages.Interfaces;

public interface IOutboxProcessor
{
    Task<Result> ProcessOutboxMessagesAsync(
        int batchSize,
        JsonSerializerOptions jsonSerializerOptions, 
        CancellationToken cancellationToken = default);
}
