# Frontend Layers — Index

The frontend is built on a layered architecture that mirrors the backend's Clean Architecture. Layers form a strict inward-only dependency graph: `app/` → `features/` → `infra/` → `core/`. No layer may import from a layer further out than itself.

| Layer | Folder | Index | Responsibility |
|---|---|---|---|
| Core | `core/` | [core/core-index.md](core/core-index.md) | Primitive UI components, shared types, utility functions — zero layer dependencies |
| Infra | `infra/` | [infra/infra-index.md](infra/infra-index.md) | HTTP client configuration and raw API call functions — no React, no hooks |
| Features | `features/` | [features/features-index.md](features/features-index.md) | SWR hooks and feature-specific components — business state orchestration |
| App | `app/` | [app/app-index.md](app/app-index.md) | Next.js App Router pages, layouts, and route segments — no direct API calls |
