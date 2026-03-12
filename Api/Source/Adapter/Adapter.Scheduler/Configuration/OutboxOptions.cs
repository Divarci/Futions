using Core.Library.Abstractions.Interfaces;

namespace Adapter.Scheduler.Configuration;

internal sealed record OutboxOptions : IOutboxOptions
{
    public int IntervalInSeconds { get; init; }

    public int BatchSize { get; init; }
}
