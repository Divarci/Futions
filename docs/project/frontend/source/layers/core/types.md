# Core — Types

TypeScript types shared across all layers. Defines the shapes that `infra/` returns and `features/` consumes.

---

## Pattern

```typescript
// common.types.ts
export type ApiResponse<T> = {
    data:      T;
    isSuccess: boolean;
};

export type PaginatedResponse<T> = {
    items:      T[];
    totalCount: number;
    skip:       number;
    take:       number;
};
```

---

## Rules

- Only types that are genuinely shared across three or more layers or features live here.
- Domain-specific types (e.g., `TaskViewModel`) belong in `features/{domain}/types/` — not here.
