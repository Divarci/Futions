namespace Core.Library.Abstractions.Interfaces;

public record IOutboxOptions
{
    int IntervalInSeconds { get; }

    int BatchSize { get; }
}
