# Validators

## What It Does

Provides a set of static extension methods for validating primitive types and value objects.
Validators are called inside value object constructors to enforce domain invariants before
an object can be constructed. All validators return Result — they never throw exceptions.

---

## Current Structure

### StringValidator

Validates string? values. Supports: required (null/empty) check, minimum length, maximum
length, and email format validation via a compiled regex. Both the nullable overload (for
optional fields) and a non-nullable overload (for always-present fields) are provided.

### GuidValidator

Validates Guid? values. Supports: required (null) check and an optional check for
Guid.Empty when an empty GUID is not acceptable.

### DateTimeValidator

Validates DateTime? values. Supports: required check, minValue/maxValue range bounds,
and a DateTimeRestriction flag that constrains the value to past-only or future-only. Both
nullable and non-nullable overloads are provided.

### DecimalValidator

Validates decimal? values. Supports: required check, negative value guard, and
minValue/maxValue range bounds. Both nullable and non-nullable overloads are provided.

### IntegerValidator

Validates int? values. Supports: required check, negative value guard, and
minValue/maxValue range bounds. Both nullable and non-nullable overloads are provided.

### EnumValidator

Validates TEnum? values where TEnum : struct, Enum. Supports: required (null) check and
an Enum.IsDefined membership check to guard against out-of-range integer casts. Both
nullable and non-nullable overloads are provided.

### ValueObjectValidator

Validates any reference-type value object. Performs a null check only — used to ensure a
nested value object field is not null before constructing the parent value object.

### Enums / DateTimeRestriction

Supporting enum with three values: None, OnlyPast, OnlyFuture. Passed to
DateTimeValidator to restrict what temporal direction the date may represent.

---

## How to Scale

- To validate a new primitive type, add a new static class following the same pattern:
  static class, Validate extension methods, returns Result.
- To add a new constraint to an existing validator (e.g., URL format on StringValidator),
  add a new optional parameter with a safe default value, or add a new overload.
- To add a new temporal restriction (e.g., business hours only), extend the
  DateTimeRestriction enum and add the corresponding branch in DateTimeValidator.
- Each validator file stays focused on a single type. Do not consolidate validators.

---

## Critical Rules

- Validators must return Result only. Throwing from inside a validator is not permitted.
- Validators are called inside value object constructors only. Application services and use
  cases must not call validators directly.
- Each validator is a static class with extension methods. Instance-based validators are
  not permitted.
- Validation errors (domain rule violations) use HttpStatusCode.UnprocessableEntity (422).
  Internal argument errors detected by the validator itself (e.g., empty fieldName) use
  HttpStatusCode.InternalServerError (500).
- Both nullable and non-nullable overloads must be provided for each validator: the nullable
  overload handles the absent-value case; the non-nullable handles presence-only validation.
