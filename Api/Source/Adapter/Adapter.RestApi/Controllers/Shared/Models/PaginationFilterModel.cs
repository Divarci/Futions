using Microsoft.AspNetCore.Mvc;

namespace Adapter.RestApi.Controllers.Shared.Models;

public record PaginationFilterModel
{
    [FromQuery(Name = "page")]
    public int? Page { get; init; }

    [FromQuery(Name = "pageSize")]
    public int? PageSize { get; init; }

    [FromQuery(Name = "sortBy")]
    public string? SortBy { get; init; }

    [FromQuery(Name = "isAscending")]
    public bool? IsAscending { get; init; }

    [FromQuery(Name = "filter")]
    public string? Filter { get; init; }
}
