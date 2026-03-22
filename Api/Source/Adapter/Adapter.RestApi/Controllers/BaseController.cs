using Adapter.RestApi.AspNetCore.Extensions;
using Adapter.RestApi.AspNetCore.Helpers;
using Core.Library.Exceptions;
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
        if (result is null)
            throw new FutionsException(
                assemblyName: "Adapter.RestApi",
                className: nameof(BaseController),
                methodName: nameof(HandleResult),
                message: "Result cannot be null.");

        if (result.IsFailure)
            return StatusCode((int)result.StatusCode, ApiResultHelper.Problem(result));

        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>
    /// Handles a <see cref="Result{T}"/> that is already mapped and ready to return.
    /// Returns the entity data on success, or a ProblemDetails response on failure.
    /// </summary>
    /// <typeparam name="T">The type of the data in the result.</typeparam>
    /// <param name="result">The already-mapped result.</param>
    /// <returns>An ActionResult containing the data or a ProblemDetails response.</returns>
    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result is null)
            throw new FutionsException(
                assemblyName: "Adapter.RestApi",
                className: nameof(BaseController),
                methodName: nameof(HandleResult),
                message: "Result cannot be null.");

        if (result.IsFailure)
            return StatusCode((int)result.StatusCode, ApiResultHelper.Problem(result));

        return StatusCode((int)result.StatusCode, result);
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
        if (result is null)
            throw new FutionsException(
                assemblyName: "Adapter.RestApi",
                className: nameof(BaseController),
                methodName: nameof(HandleResult),
                message: "Result cannot be null.");

        if (result.IsFailure)
            return StatusCode((int)result.StatusCode, ApiResultHelper.Problem(result));

        Result<M> mappedResult = result.MapTo(mapper);

        return StatusCode((int)result.StatusCode, mappedResult);
    }

    /// <summary>
    /// Handles a <see cref="PaginatedResult{T}"/> that is already mapped and ready to return.
    /// Returns the full paginated result (data + metadata) on success, or a ProblemDetails response on failure.
    /// </summary>
    /// <typeparam name="T">The type of the data in the paginated result.</typeparam>
    /// <param name="result">The already-mapped paginated result.</param>
    /// <returns>An ActionResult containing the paginated data or a ProblemDetails response.</returns>
    protected ActionResult HandleResult<T>(PaginatedResult<T> result)
    {
        if (result is null)
            throw new FutionsException(
                assemblyName: "Adapter.RestApi",
                className: nameof(BaseController),
                methodName: nameof(HandleResult),
                message: "Result cannot be null.");

        if (result.IsFailure)
            return StatusCode((int)result.StatusCode, ApiResultHelper.Problem(result));

        return StatusCode((int)result.StatusCode, result);
    }

    protected Guid GetCurrentUserId() => HttpContext.GetRequiredUserId();
    protected string GetCurrentUsername() => HttpContext.GetRequiredUserEmail();
}

