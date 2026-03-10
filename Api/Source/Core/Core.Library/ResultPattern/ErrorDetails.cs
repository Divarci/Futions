namespace Core.Library.ResultPattern;

/// <summary>
/// Represents structured error details associated with a failed operation,
/// containing a trace identifier and a list of error messages.
/// </summary>
public class ErrorDetails
{
    private ErrorDetails(string traceId, List<string> errors)
    {
        TraceId = traceId;
        Errors = errors;
    }

    public string TraceId { get; set; }
    public List<string> Errors { get; set; }

    /// <summary>
    /// Creates an <see cref="ErrorDetails"/> instance with a list of error messages.
    /// If <paramref name="traceId"/> is null or whitespace, a new <see cref="Guid"/> is generated.
    /// </summary>
    public static ErrorDetails Create(List<string> errors, string? traceId = null)
        => new(string.IsNullOrWhiteSpace(traceId)
            ? Guid.NewGuid().ToString()
            : traceId, errors);

    /// <summary>
    /// Creates an <see cref="ErrorDetails"/> instance with a single error message.
    /// If <paramref name="traceId"/> is null or whitespace, a new <see cref="Guid"/> is generated.
    /// </summary>
    public static ErrorDetails Create(string error, string? traceId = null)
        => new(string.IsNullOrWhiteSpace(traceId)
            ? Guid.NewGuid().ToString()
            : traceId, [error]);
}