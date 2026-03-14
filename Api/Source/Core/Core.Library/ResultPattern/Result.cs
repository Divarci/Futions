using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json.Serialization;

namespace Core.Library.ResultPattern;

/// <summary>
/// Represents the outcome of an operation, encapsulating success/failure state,
/// an HTTP status code, a descriptive message, and optional error details.
/// </summary>
public class Result
{
    protected Result(HttpStatusCode? statusCode,
        bool isFailure, string message, ErrorDetails? errorDetails)
    {
        StatusCode = statusCode;
        IsFailure = isFailure;
        Message = message;
        ErrorDetails = errorDetails;
    }

    [JsonIgnore]
    public HttpStatusCode? StatusCode { get; set; }

    [JsonIgnore]
    public bool IsFailure { get; set; }

    [JsonIgnore]
    public bool IsSuccess => !IsFailure;

    public string Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ErrorDetails? ErrorDetails { get; set; }

    /// <summary>
    /// Creates a successful result with the specified status code and message.
    /// </summary>
    public static Result Success(string message, HttpStatusCode? statusCode = null)
        => new(statusCode, false, message, null);

    /// <summary>
    /// Creates a failure result with the specified status code and message.
    /// </summary>
    public static Result Failure(string message, HttpStatusCode? statusCode = null)
        => new(statusCode, true, message, null);

    /// <summary>
    /// Creates a failure result with the specified status code, message, and error details.
    /// </summary>
    public static Result Failure(string message, ErrorDetails errorDetails,
        HttpStatusCode? statusCode = null)
        => new(statusCode, true, message, errorDetails);

    /// <summary>
    /// Combines multiple validation results into a single <see cref="Result"/>.
    /// Returns success if all pass, otherwise aggregates all errors into one failure.
    /// </summary>
    /// <param name="results">The list of validation results to combine.</param>
    /// <returns>
    /// A success result if all validations pass; otherwise, a failure result
    /// with all error details and a <see cref="HttpStatusCode.UnprocessableEntity"/> status code.
    /// </returns>
    public static Result CombineValidationErrors(List<Result> results)
    {
        List<Result> failures = [.. results.Where(r => r.IsFailure)];

        if (!(failures.Count > 0))
            return Success("Validation succeeded.");

        List<string> errors = [.. failures.SelectMany(r => r.ErrorDetails?.Errors ?? [])];

        return Failure(
            message: "Validation failed",
            errorDetails: ErrorDetails.Create(errors),
            statusCode: HttpStatusCode.UnprocessableEntity);
    }
}

/// <summary>
/// Represents the outcome of an operation that returns data of type <typeparamref name="T"/>.
/// Extends <see cref="Result"/> with an optional data payload.
/// </summary>
/// <typeparam name="T">The type of the data returned by the operation. Must be a reference type.</typeparam>
public class Result<T> : Result
{
    protected Result(HttpStatusCode? statusCode,
        bool isFailure, string message, ErrorDetails? errorDetails, T? data)
        : base(statusCode, isFailure, message, errorDetails)
    {
        Data = data;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; set; }

    [JsonIgnore]
    [MemberNotNullWhen(returnValue: false, nameof(Data))]
    public bool IsFailureAndNoData => IsFailure && Data is null;

    /// <summary>
    /// Creates a successful result with the specified data, message, and status code.
    /// </summary>      
    public static Result<T> Success(string message, T data, HttpStatusCode? statusCode = null)
        => new(statusCode, false, message, null, data);

    /// <summary>
    /// Creates a failure result with the specified status code and message.
    /// </summary>
    public static new Result<T> Failure(string message, HttpStatusCode? statusCode = null)
        => new(statusCode, true, message, null, default);

    /// <summary>
    /// Creates a failure result with the specified status code, message, and error details.
    /// </summary>
    public static new Result<T> Failure(string message, ErrorDetails errorDetails,
        HttpStatusCode? statusCode = null)
        => new(statusCode, true, message, errorDetails, default);
}

/// <summary>
/// Represents the outcome of a paginated operation that returns a collection of type <typeparamref name="T"/>.
/// Extends <see cref="Result{T}"/> with pagination metadata.
/// </summary>
/// <typeparam name="T">The type of the data returned by the operation. Must be a reference type.</typeparam>
public class PaginatedResult<T> : Result<T> where T : class
{
    private PaginatedResult(HttpStatusCode? statusCode, bool isFailure,
        string message, ErrorDetails? errorDetails, T? data, Metadata? metadata)
        : base(statusCode, isFailure, message, errorDetails, data)
    {
        Metadata = metadata;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Metadata? Metadata { get; set; }

    /// <summary>
    /// Creates a successful paginated result with data, message, and pagination details.
    /// The HTTP status code is automatically set to <see cref="HttpStatusCode.OK"/>.
    /// </summary>
    public static PaginatedResult<T> Success(T data, string message,
        int pageNumber, int pageSize, int totalCount, int pageCount)
        => new(HttpStatusCode.OK, false, message, null,
            data, new(pageNumber, pageSize, totalCount, pageCount));

    /// <summary>
    /// Creates a failed paginated result with the specified error message and optional HTTP status code.
    /// </summary>   
    public static new PaginatedResult<T> Failure(string message, HttpStatusCode? statusCode = null)
        => new(statusCode, true, message, null, default, null);
}