# Features — Types

All domain-specific TypeScript types live in `features/{domain}/types/{domain}.types.ts`.

---

## Pattern

```typescript
// features/{domain}/types/{domain}.types.ts

export type {Entity}Status = "Active" | "Inactive";

export type {Entity}ViewModel = {
    {entity}Id: string;
    name: string;
    status: {Entity}Status;
    createdUtc: string;
    updatedUtc: string;
};

export type {Entity}CreateModel = {
    name: string;
};

export type {Entity}UpdateModel = {
    name: string | null;
};

export type {Entity}FilterParams = {
    keyword?: string;
    status?: {Entity}Status;
    skip?: number;
    take?: number;
};
```

---

## Rules

- All types for one domain live in a single `{domain}.types.ts` file.
- Domain types never live in `core/types/` or `infra/` — they belong here.
- Types that must be shared across three or more unrelated features are moved to `core/types/`.
