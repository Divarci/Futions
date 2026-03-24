# Result Pattern

Replaces exceptions as the primary signaling mechanism for expected operation outcomes.
Every service method, use case, repository operation, and validator returns one of the
Result types instead of throwing. Exceptions are reserved exclusively for programmer errors
(see `{Solution}Exception`).

---

## What It Does

Provides a three-tier result hierarchy that carries success/failure state, an HTTP status
code, a human-readable message, optional structured error details, an optional typed data
payload, and optional pagination metadata — all in a single return value. This allows
callers to branch on outcomes using IsSuccess / IsFailure without try-catch blocks and
without losing any information needed for an HTTP response.

---

## Current Structure

### Result

Base class. Source: ResultPattern/Result.cs

Carries HttpStatusCode StatusCode, bool IsFailure, bool IsSuccess, string Message,
and ErrorDetails? ErrorDetails. StatusCode and IsFailure are tagged [JsonIgnore]
so they are never serialised into API responses — they are control-flow properties for
internal use only.

Exposes three factory groups:
- Success(message, statusCode) — command operations with no data
- Failure(message, statusCode) — failure with a message only
- Failure(message, errorDetails, statusCode) — failure with structured field-level errors
- CombineValidationErrors(List<Result>) — merges multiple validator results into a single
  outcome; returns success if all pass, or a 422 with all aggregated error strings if any fail

### Result\<T\>

Generic extension of Result. Source: ResultPattern/Result.cs

Adds a T? Data payload and the helper property IsFailureAndNoData (annotated with
[MemberNotNullWhen] so the compiler understands Data is non-null when this is false).
Used for all query operations that return a single entity or DTO.

Factory methods mirror Result but include a data parameter in the Success overload.

### PaginatedResult\<T\>

Extension of Result<T>. Source: ResultPattern/Result.cs

Adds a Metadata? Metadata property for pagination context. The Success factory
accepts raw pagination integers (pageNumber, pageSize, 	otalCount, pageCount) and
constructs the Metadata instance internally. The HTTP status code for success is always
200 and is not configurable at the call site. Used for all list/collection queries.

### ErrorDetails

Source: ResultPattern/ErrorDetails.cs

Wraps a string TraceId and a List<string> Errors. The TraceId is auto-generated
as a new Guid when not explicitly supplied, providing correlation across logs and
responses. Two Create factory overloads exist: one for a single error string, one for
a list. The constructor is private.

### Metadata

Source: ResultPattern/Metadata.cs

Carries PageNumber, PageSize, TotalCount, PageCount, and a computed TotalPages
(calculated as Math.Ceiling(totalCount / pageSize)). Throws `{Solution}Exception` if
pageSize is zero or negative — this is a programmer error, not a domain failure.

### ResultExtension

Source: ResultPattern/ResultExtension.cs

Static class with MapTo extension methods. Converts Result<TEntity> to
Result<TModel> and PaginatedResult<TEntity> to PaginatedResult<TModel> using a
caller-supplied mapping delegate. Throws `{Solution}Exception` if called on a failed result
with no data — mapping a failure is always a programmer error.

---

## How to Scale

The three-tier hierarchy (Result → Result<T> → PaginatedResult<T>) is intentionally
closed. Do not add a fourth tier or a sibling class.

If a new operation shape requires additional metadata on the response (e.g., cursor-based
pagination), extend PaginatedResult<T> or introduce a new property on Metadata.

If a new kind of mapping is needed in ResultExtension, add a new extension method.
Do not add instance methods or non-extension helpers.

---

## Critical Rules

- Every service, use case, and validator method must return Result, Result<T>, or
  PaginatedResult<T>. Throwing for expected failures is not permitted.
- Result.Failure with ErrorDetails is for field-level validation errors (422).
  Result.Failure without ErrorDetails is for single-message failures (404, 409, etc.).
- StatusCode, IsFailure, and IsSuccess are [JsonIgnore]. They must never appear
  in API responses — the adapter layer translates them into HTTP responses explicitly.
- CombineValidationErrors is the only correct way to merge multiple validator results.
  Do not manually aggregate errors outside of this method.
- ResultExtension.MapTo must never be called on a failed result. Always check
  IsSuccess before mapping.
- Metadata enforces pageSize > 0. If this throws, the call site passed a zero-page-size
  — that is a programmer error, not a domain validation failure.
- ErrorDetails.TraceId auto-generates a Guid when not supplied. Never pass an empty
  string intentionally — pass null or omit the parameter to get a generated ID.
