# Features Layer — Index

The Features layer contains all business state orchestration: SWR hooks that wrap `infra/` API calls, and feature-specific components that consume those hooks. It depends on `infra/` for data and `core/` for shared primitives — it is never referenced by `infra/`.

Use this index to navigate directly to the topic you need.

---

## Structure & Rules

| Document | What it covers |
|---|---|
| [Structure](features-structure.md) | Full folder tree with scale points and critical rules |

---

## Hooks

| Document | What it covers |
|---|---|
| [Hooks](hooks.md) | Query and mutation hook patterns — naming, SWR keys, revalidation |

---

## Components

| Document | What it covers |
|---|---|
| [Components](components.md) | Component pattern, `"use client"` rendering model, rules |

---

## Types

| Document | What it covers |
|---|---|
| [Types](types.md) | Domain type file pattern — ViewModels, CreateModels, FilterParams |

---

## Barrel & Server-Side Concerns

| Document | What it covers |
|---|---|
| [Barrel Export](barrel.md) | `index.ts` barrel pattern — what to export and how to import |
| [Server Actions](actions.md) | `"use server"` functions — placement, pattern, rules |
| [Error Handling](error-handling.md) | SWR error/loading states and coordination with `app/` |

---

## Guidelines

- Read **Structure** before adding any new domain folder or file.
- Read **Hooks** before implementing any SWR hook.
- Read **Barrel Export** before adding any new public symbol — every export goes through `index.ts`.
- Two features must never import from each other. Composition happens at the `app/` layer.
