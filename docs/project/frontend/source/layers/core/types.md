# Core — Types

TypeScript types shared across all layers. Defines the shapes that `infra/` returns and `features/` consumes.

---

## Pattern

```typescript
// common.types.ts
export type ErrorDetails = {
    traceId: string;
    errors: string[];
};

export type Metadata = {
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    pageCount: number;
    totalPages: number;
};

export type ApiResponse<T> = {
    message: string;
    data: T;
    errorDetails: ErrorDetails | null;
};

export type PaginatedApiResponse<T> = ApiResponse<T> & {
    metadata: Metadata;
};
```

---

## Backend Alignment

These types mirror the backend's Result Pattern serialization contract:

| Backend property | Serialized | Frontend type |
|---|---|---|
| `Message` | ✓ | `message: string` |
| `Data` | ✓ | `data: T` |
| `ErrorDetails` | ✓ (null when absent) | `errorDetails: ErrorDetails \| null` |
| `Metadata` | ✓ when non-null (`WhenWritingNull`) | `metadata: Metadata` — present on `PaginatedApiResponse` only |
| `IsSuccess` | `[JsonIgnore]` | — not mapped |
| `IsFailure` | `[JsonIgnore]` | — not mapped |
| `StatusCode` | `[JsonIgnore]` | — not mapped |

`PaginatedResult<T>.Success(...)` always constructs a non-null `Metadata`, so `metadata` is always present in collection endpoint responses. Single-entity endpoints (`Result<T>`) never include `metadata`.

---

## Zod

`core/types/` uses plain TypeScript types — not Zod schemas. These are structural contracts consumed by Axios which provides its own generic typing.

Zod is used exclusively in `features/` for **form validation** with React Hook Form (`@hookform/resolvers`).

---

## Rules

- Only types that are genuinely shared across three or more layers or features live here.
- Domain-specific types (e.g., `{Entity}ViewModel`) belong in `features/{domain}/types/` — not here.
- Never add `isSuccess`, `isFailure`, `statusCode`, or any property that the backend marks `[JsonIgnore]`.
- Use `ApiResponse<T>` for single-entity endpoints (`Result<T>`).
- Use `PaginatedApiResponse<T>` for collection endpoints (`PaginatedResult<T>`).
