# Core Layer — Index

The Core layer is the foundation of the entire frontend. It has no layer dependencies — everything else depends on it. It contains three concerns: primitive UI building blocks, shared TypeScript types, and utility functions.

Use this index to navigate directly to the topic you need.

---

## Structure & Rules

| Document | What it covers |
|---|---|
| [Structure](core-structure.md) | Full folder tree with scale points and critical rules |

---

## Components

| Document | What it covers |
|---|---|
| [Components](components.md) | Primitive UI building blocks — folder pattern, rules, examples |

---

## Shared Primitives

| Document | What it covers |
|---|---|
| [Hooks](hooks.md) | Shared utility hooks — rules and promotion criteria |
| [Types](types.md) | Shared TypeScript types — `ApiResponse<T>`, `PaginatedResponse<T>` |
| [Utils](utils.md) | Pure utility functions — rules and conventions |

---

## Guidelines

- Read **Structure** before adding, moving, or renaming any file in `core/`.
- A component, hook, or utility is promoted to `core/` only when used by three or more unrelated features.
- Read **Types** before adding any new shared type — confirm it is not already defined.
