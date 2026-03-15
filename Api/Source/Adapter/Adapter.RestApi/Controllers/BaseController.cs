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
    /// <typeparam name="T">The type of the data returned by the operation.</typeparam>
    /// <typeparam name="M">The type of the mapped data.</typeparam>
    /// <param name="result">The result of the operation.</param>
    /// <param name="mapper">A function to map the data from type T to type M.</param>
    /// <returns>An ActionResult containing the mapped data or a ProblemDetails response.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected ActionResult HandleResult<T, M>(Result<T> result, Func<T, M> mapper)
    {
        if (result is null || result.StatusCode is null)
            throw new InvalidOperationException("Result cannot be null.");

        Result<M> mappedResult = result.MapTo(mapper);

        int statusCode = (int)mappedResult.StatusCode!;

        if (mappedResult.IsSuccess)
            return StatusCode(statusCode, mappedResult.Data);

        return StatusCode(statusCode, ApiResultHelper.Problem(result));
    }

    /// <summary>
    /// Handles a <see cref="PaginatedResult{T}"/> that carries a paginated collection.
    /// Returns the full paginated result (data + metadata) on success, or a ProblemDetails response on failure.
    /// </summary>
    /// <typeparam name="T">The type of the data returned by the operation.</typeparam>
    /// <typeparam name="M">The type of the mapped data.</typeparam>
    /// <param name="result">The result of the operation.</param>
    /// <param name="mapper">A function to map the data from type T to type M.</param>
    /// <returns>An ActionResult containing the mapped data or a ProblemDetails response.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected ActionResult HandleResult<T, M>(PaginatedResult<T> result, Func<T, M> mapper)
    {
        if (result is null || result.StatusCode is null)
            throw new InvalidOperationException("Result cannot be null.");

        PaginatedResult<M> mappedResult = result.MapTo(mapper);

        int statusCode = (int)mappedResult.StatusCode!;

        if (result.IsSuccess)
            return StatusCode(statusCode, result);

        return StatusCode(statusCode, ApiResultHelper.Problem(result));
    }
}

