using Adapter.RestApi.AspNetCore.Helpers;
using Core.Library.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.RestApi.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    /// <summary>
    /// Handles a non-data <see cref="Result"/> (e.g. update, delete).
    /// Returns 200 OK with the result message on success, or a ProblemDetails response on failure.
    /// </summary>
    protected ActionResult HandleResult(Result result)
    {
        if (result is null || result.StatusCode is null)
            throw new InvalidOperationException("Result cannot be null.");

        int statusCode = (int)result.StatusCode;

        if (result.IsSuccess)
            return StatusCode(statusCode, result);

        return StatusCode(statusCode, ApiResultHelper.Problem(result));
    }

    /// <summary>
    /// Handles a <see cref="Result{T}"/> that carries a single entity (e.g. get by id, create).
    /// Returns the entity data on success, or a ProblemDetails response on failure.
    /// </summary>
    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result is null || result.StatusCode is null)
            throw new InvalidOperationException("Result cannot be null.");

        int statusCode = (int)result.StatusCode;

        if (result.IsSuccess)
            return StatusCode(statusCode, result.Data);

        return StatusCode(statusCode, ApiResultHelper.Problem(result));
    }

    /// <summary>
    /// Handles a <see cref="PaginatedResult{T}"/> that carries a paginated collection.
    /// Returns the full paginated result (data + metadata) on success, or a ProblemDetails response on failure.
    /// </summary>
    protected ActionResult HandleResult<T>(PaginatedResult<T> result) where T : class
    {
        if (result is null || result.StatusCode is null)
            throw new InvalidOperationException("Result cannot be null.");

        int statusCode = (int)result.StatusCode;

        if (result.IsSuccess)
            return StatusCode(statusCode, result);

        return StatusCode(statusCode, ApiResultHelper.Problem(result));
    }
}

