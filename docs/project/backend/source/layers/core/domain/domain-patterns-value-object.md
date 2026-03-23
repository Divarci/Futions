# Domain Patterns — Value Object

Defines the complete structure a value object must follow. Value objects are immutable,
identity-free, and validated at construction. They are owned by entities and represented
as `sealed partial record` types split across two files.

---

## Pattern — Value Object

### Folder Structure

```
ValueObjects/{ValueObject}ValueObject/
├── {ValueObject}.cs
├── {ValueObject}.Validation.cs
└── {ValueObject}Model.cs
```

### {ValueObject}.cs

```csharp
using Core.Library.Exceptions;
using Core.Library.ResultPattern;
using System.Net;

public sealed partial record {ValueObject}
{
    // ── Constructor ───────────────────────────────────────────────────────────
    private {ValueObject}(string requiredField, string? optionalField)
    {
        RequiredField = requiredField;
        OptionalField = optionalField;
    }

    // ── Properties ────────────────────────────────────────────────────────────
    public string  RequiredField { get; init; }
    public string? OptionalField { get; init; }

    // ── Factory ───────────────────────────────────────────────────────────────
    public static Result<{ValueObject}> Create({ValueObject}Model model)
    {
        if (model is null)
            throw new {Solution}Exception(
                assemblyName: "Core.Domain",
                className:    nameof({ValueObject}),
                methodName:   nameof(Create),
                message:      "{ValueObject} create model cannot be null.");

        {ValueObject} valueObject = new(model.RequiredField!, model.OptionalField);

        Result validationResult = Validate(valueObject);

        if (validationResult.IsFailure)
            return Result<{ValueObject}>.Failure(
                message:      validationResult.Message,
                errorDetails: validationResult.ErrorDetails!,
                statusCode:   validationResult.StatusCode);

        return Result<{ValueObject}>.Success(
            message:    "{ValueObject} created successfully.",
            data:       valueObject,
            statusCode: HttpStatusCode.OK);
    }
}
```

### {ValueObject}.Validation.cs

The validation file is the `partial` counterpart. It follows the same two-method pattern
as entity validation: `ValidateProperties` for field-level rules and `ValidateBusiness`
for cross-field or domain-rule checks.

```csharp
using Core.Library.ResultPattern;
using Core.Library.Validators;
using System.Net;

public sealed partial record {ValueObject}
{
    private static Result Validate({ValueObject} valueObject)
    {
        List<Result> results = [];

        ValidateProperties(results, valueObject);
        ValidateBusiness(results, valueObject);

        return Result.CombineValidationErrors(results);
    }

    private static void ValidateProperties(List<Result> results, {ValueObject} valueObject)
        => results.AddRange(
            [
                valueObject.RequiredField.Validate(
                    fieldName: nameof(valueObject.RequiredField),
                    maxLength: 100),

                valueObject.OptionalField.Validate(
                    fieldName:  nameof(valueObject.OptionalField),
                    maxLength:  100,
                    isRequired: false),
            ]);

    private static void ValidateBusiness(List<Result> results, {ValueObject} valueObject)
    {
        if (valueObject.RequiredField.StartsWith("_"))
            results.Add(Result.Failure(
                message:      "Validation failed",
                errorDetails: ErrorDetails.Create(["RequiredField cannot start with underscore."]),
                statusCode:   HttpStatusCode.UnprocessableEntity));
    }
}
```

### {ValueObject}Model.cs

The model is a `sealed record` carrying the raw input. Optional fields use nullable types.

```csharp
public sealed record {ValueObject}Model
{
    public required string  RequiredField { get; init; }
    public required string? OptionalField { get; init; }
}
```

---

## Critical Rules

- Value objects must be declared as `sealed partial record`. No class, no struct.
- The constructor is always `private`. It is only called from the `Create` factory.
- Properties use `init`-only setters. No property may have a `set` accessor.
- `null` model input in `Create` is a programmer error — throw `{Solution}Exception`.
- All validation runs inside `Validate` before the value object is returned. A value object
  that fails validation is never returned to the caller.
- Validation follows the same `ValidateProperties` + `ValidateBusiness` +
  `CombineValidationErrors` structure as entities.
- Value objects have no domain events. They do not extend `BaseEntity`.
- A value object is never updated in place. When a value changes, the owning entity creates
  a new instance via `Create` and replaces the old one.
- Value objects are owned by entities. They must never be referenced directly as aggregate
  roots or stored as independent database rows.
- The model (`{ValueObject}Model`) is the only input accepted by `Create`. Overloads that
  accept individual primitive parameters are not permitted.
