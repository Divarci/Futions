namespace Adapter.Scheduler.Configuration;

internal sealed record OutboxOptions
{
    public int IntervalInSeconds { get; init; }

    public int BatchSize { get; init; }
}
