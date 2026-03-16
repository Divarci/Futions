using Core.Library.Exceptions;
using System.Text.Json.Serialization;

namespace Core.Library.ResultPattern;

/// <summary>
/// Represents pagination metadata for a paginated result,
/// including page number, page size, total item count, and total page count.
/// </summary>
public class Metadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata"/> class.
    /// Automatically calculates <see cref="TotalPages"/> from <paramref name="totalCount"/> and <paramref name="pageSize"/>.
    /// </summary>
    [JsonConstructor]
    public Metadata(int pageNumber, int pageSize, int totalCount, int pageCount)
    {
        if (pageSize <= 0)
            throw new FutionsException(
                assemblyName: "Core.Library",
                className: nameof(Metadata),
                methodName: ".ctor",
                message: "PageSize must be greater than zero.");

        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        PageCount = pageCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int PageCount { get; set; }
    public int TotalPages { get; set; }
}