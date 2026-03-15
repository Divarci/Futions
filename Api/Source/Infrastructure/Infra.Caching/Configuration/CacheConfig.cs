namespace Infra.Caching.Configuration;

public record CacheConfig
{
    public string? HostAndPort { get; init; }
    public bool Ssl { get; init; }
    public int Timeout { get; init; }
    public string? AccessKey { get; init; }
}