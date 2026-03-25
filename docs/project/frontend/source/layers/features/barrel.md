# Features — Barrel Export

Every feature exposes its public API through a single `index.ts` barrel file. Files outside the feature always import from this barrel — never from internal paths.

---

## Pattern

```typescript
// features/{domain}/index.ts
export { {Entity}List }       from "./components/{Entity}List";
export { {Entity}Card }       from "./components/{Entity}Card";
export { {Entity}Form }       from "./components/{Entity}Form";
export { useGet{Entities} }   from "./hooks/useGet{Entities}";
export { useGet{Entity} }     from "./hooks/useGet{Entity}";
export { useCreate{Entity} }  from "./hooks/useCreate{Entity}";
export { useUpdate{Entity} }  from "./hooks/useUpdate{Entity}";
export { useDelete{Entity} }  from "./hooks/useDelete{Entity}";
export type {
    {Entity}ViewModel,
    {Entity}CreateModel,
    {Entity}UpdateModel,
    {Entity}FilterParams,
    {Entity}Status,
}                             from "./types/{domain}.types";
```

---

## Rules

- Every public symbol (component, hook, type, action) must be re-exported from `index.ts`.
- Files outside the feature import from `@/features/{domain}` — never from deep paths like `@/features/{domain}/hooks/useGet{Entities}`.
- Internal files within the same feature may use relative imports.
