# Architecture

This document provides a high-level summary of the frontend architecture, covering structural patterns, layer boundaries, and scalability approach.

---

## Layered Architecture — Backend-Parallel Design

The frontend mirrors the backend's Clean Architecture in structure and intent.

**Layered Architecture** governs the macro structure: code is organized into four layers where dependencies always point inward. The core primitives sit at the center; infrastructure and adapters sit at the edges and depend on inner layers — never the reverse.

**Vertical Slice Architecture** governs the micro structure inside each layer: code is grouped by domain concept (e.g., `features/tasks/`), not by technical type. Every feature owns its folder with its hooks, components, and types co-located.

The result is a codebase where each feature is easy to locate (vertical slices), while cross-layer boundaries remain clean and predictable (layered architecture).

---

## Layers and Their Responsibilities

The frontend is divided into four layers, each with a distinct responsibility boundary.

```
┌──────────────────────────────────────────────────┐
│                    app/ Layer                     │
│              Next.js Pages & Layouts              │
├──────────────────────────────────────────────────┤
│                 features/ Layer                   │
│              SWR Hooks · Components                │
├──────────────────────────────────────────────────┤
│                   infra/ Layer                    │
│           HTTP Client · API Functions             │
├──────────────────────────────────────────────────┤
│                   core/ Layer                     │
│     Primitive Components · Types · Utilities      │
└──────────────────────────────────────────────────┘
```

| Layer | Folder | Responsibility |
|---|---|---|
| **Core** | `core/` | Foundation. Shared primitive UI components, common TypeScript types, utility functions, and shared hooks — zero business or domain logic. |
| **Infra** | `infra/` | Technical concerns. The HTTP client configuration and raw API call functions per domain. No React, no hooks, no components. |
| **Features** | `features/` | Business orchestration. SWR hooks that wrap `infra/` calls and manage server state. Feature-specific components that consume those hooks. |
| **App** | `app/` | Entry points. Next.js App Router pages, layouts, and route segments. No direct API calls — all data flows through `features/` hooks. |

---

## Layer Dependencies

Dependencies are strictly unidirectional — outer layers depend on inner layers, never the reverse.

```
app/ ──────────────► features/ ──────────────► infra/ ──────────────► core/
                         │                                                ▲
                         └────────────────────────────────────────────────┘
                                    (features also uses core/)
```

Key rules:

- `core/` has **zero** layer dependencies — it is fully self-contained.
- `infra/` depends only on `core/` for shared types — never imports from `features/` or `app/`.
- `features/` depends on `infra/` for API calls and `core/` for shared types — never imports from `app/`.
- `app/` depends on `features/` for data — never calls `infra/` directly.

---

## Scalability

The architecture is designed to grow by adding new feature folders, not by modifying existing layers.

**Features layer** accepts any number of new domain folders. Each follows the pattern `features/{domain}/` and owns its hooks, components, and types. Adding a new domain feature requires no changes to existing features.

**Infra layer** accepts any number of new domain API files. Each follows the pattern `infra/{domain}/{domain}.api.ts`. Adding a new API module requires no changes to existing infra files.

**Core layer** remains stable — only truly shared, domain-agnostic primitives live here.
