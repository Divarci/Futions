namespace Infra.Caching.Configuration;

public record CacheConfig
{
    public string? Host { get; init; }
    public string? Port { get; init; }
    public bool Ssl { get; init; }
    public int Timeout { get; init; }
    public string? AccessKey { get; init; }

    public string HostAndPort => $"{Host}:{Port}";
}