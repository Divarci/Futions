# Exceptions

## What It Does

Provides a single, project-wide exception type reserved exclusively for developer errors —
cases where a programming mistake, invalid configuration, or broken assumption is
encountered at runtime. This is not for domain failures or expected business errors.
All instances of this exception result in HTTP 500 Internal Server Error.

---

## Current Structure

### {Solution}Exception

A sealed class extending Exception. Requires four constructor arguments at the throw
site: assemblyName, className, methodName, and message. An optional innerException
is also supported. All four diagnostic fields are exposed as read-only string properties,
enabling precise location information in logs and monitoring tools without relying on stack
trace parsing.

---

## How to Scale

Do not add new exception types for new features. Domain failures and expected business
errors must use Result.Failure — never exceptions.

If new diagnostic metadata needs to be captured at throw sites (e.g., correlation IDs,
operation context), add properties to the existing type. Do not create sibling exception
classes.

---

## Critical Rules

- This exception type is for programmer errors only: invalid arguments, broken invariants,
  misconfigured dependencies, unexpected states.
- Never use this exception for domain failures, validation failures, or expected business
  errors. Use Result.Failure for those cases.
- Every throw site must supply assemblyName, className, and methodName explicitly.
  Do not rely on reflection or caller attributes — the caller provides context.
- This exception always produces HTTP 500. If a different status code is intended, this
  is the wrong type to use.
