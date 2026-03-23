# Architecture

This document provides a high-level summary of the backend architecture, covering structural patterns, layer boundaries, scalability approach, and containerisation.

---

## Hybrid Architecture — Clean Architecture + Vertical Slice

The backend combines two complementary patterns.

**Clean Architecture** governs the macro structure: code is organized into concentric layers where dependencies always point inward. The domain and shared kernel sit at the center; infrastructure and adapters sit at the edges and depend on inner layers — never the reverse.

**Vertical Slice Architecture** governs the micro structure inside each layer: code is grouped by domain concept (e.g., `Organisations/Companies/`), not by technical type. Every entity owns its folder with its interfaces, models, domain events, and configuration co-located.

The result is a codebase where each feature is easy to locate (vertical slices), while cross-layer boundaries remain clean and testable (clean architecture).

---

## Layers and Their Responsibilities

The solution is divided into four layers, each with a distinct responsibility boundary.

```
┌──────────────────────────────────────────────────┐
│                   Adapter Layer                   │
│   Adapter.RestApi  ·  Adapter.Scheduler           │
├──────────────────────────────────────────────────┤
│               Application Layer                   │
│     App.UseCases   ·   App.Services               │
├──────────────────────────────────────────────────┤
│              Infrastructure Layer                  │
│   Infra.Persistence  ·  Infra.Caching             │
├──────────────────────────────────────────────────┤
│                   Core Layer                      │
│     Core.Domain    ·   Core.Library               │
└──────────────────────────────────────────────────┘
```

| Layer | Projects | Responsibility |
|---|---|---|
| **Core** | `Core.Library`, `Core.Domain` | Foundation. `Core.Library` holds shared abstractions, the result pattern, and validators — zero business logic. `Core.Domain` holds entities, value objects, domain events, and the interfaces that upper layers must implement. |
| **Application** | `App.Services`, `App.UseCases` | Business logic. `App.Services` implements entity services (repository calls + cache invalidation). `App.UseCases` wraps service calls in transactions, writes audit logs, and manages cache-aside reads. |
| **Infrastructure** | `Infra.Persistence`, `Infra.Caching` | Technical concerns. `Infra.Persistence` provides EF Core repositories, the unit of work, and outbox persistence. `Infra.Caching` provides Redis-backed caching. |
| **Adapter** | `Adapter.RestApi`, `Adapter.Scheduler` | Entry points. `Adapter.RestApi` is the HTTP host (authentication, routing, validation, error handling). `Adapter.Scheduler` is the background worker that processes outbox messages via Quartz.NET. |

---

## Layer Dependencies

Dependencies are strictly unidirectional — outer layers depend on inner layers, never the reverse.

```
Adapter.RestApi ──────┐
                      ├──► App.UseCases ───► Core.Domain ───► Core.Library
Adapter.Scheduler ────┤   App.Services ───► Core.Domain ───► Core.Library
                      ├──► Infra.Persistence ─► Core.Domain
                      └──► Infra.Caching ─────► Core.Domain
```

Key rules:
- `Core.Library` has **zero** project references — it is fully self-contained.
- `Core.Domain` depends only on `Core.Library`.
- `App.Services` and `App.UseCases` depend only on `Core.Domain` — they are unaware of infrastructure.
- `Infra.Persistence` and `Infra.Caching` depend only on `Core.Domain` — they implement its contracts.
- Adapter projects reference all layers — they are the composition roots.

---

## Scalability

The architecture is designed to grow horizontally without modifying existing projects.

**Adapter layer** accepts any number of new entry-point projects. Each follows the naming pattern `Adapter.{ChannelName}` for synchronous channels or `Adapter.{WorkerName}` for background workers. A new adapter adds a project, wires DI, and delegates all logic inward — no existing adapter or application code is touched.

| Pattern | Example additions |
|---|---|
| `Adapter.{ChannelName}` | `Adapter.Grpc`, `Adapter.GraphQL` |
| `Adapter.{WorkerName}` | `Adapter.EventConsumer`, `Adapter.DataSync` |

**Infrastructure layer** accepts any number of new provider projects. Each follows the naming pattern `Infra.{ProviderName}`, depends only on `Core.Domain`, and registers through its own `ServiceRegistrar`. Adding a new provider requires no changes to any existing project.

| Pattern | Example additions |
|---|---|
| `Infra.{ProviderName}` | `Infra.Email`, `Infra.Storage`, `Infra.Search` |

**Core and Application layers** remain untouched when new adapters or infrastructure providers are added — they define contracts, not delivery mechanisms.

---

## Docker Compose

The project ships two Compose files that Docker always merges, plus a `.env` file for production secrets.

| File | Purpose |
|---|---|
| `docker-compose.yml` | Production-grade service definitions. All values injected from `.env` via `${VAR_NAME}` substitution — nothing hardcoded. |
| `docker-compose.override.yml` | Development overrides. Values hardcoded inline for local use; adds the `sqlserver` container; mounts user-secrets and HTTPS certificates. |
| `.env` | Single source of truth for all production secrets and environment-specific settings. Never committed — provisioned per environment at deploy time. |

### Services

| Service | Image | Purpose |
|---|---|---|
| `backend` | custom (`Adapter.RestApi`) | REST API host |
| `scheduler` | custom (`Adapter.Scheduler`) | Background outbox processor |
| `redis` | `redis:latest` | Distributed cache |
| `serilog` | `datalust/seq:latest` | Structured log aggregation |
| `sqlserver` *(dev only)* | `mcr.microsoft.com/mssql/server:2022` | SQL Server — development only; production uses a managed instance via `.env` connection string |

All services share a bridge network. Redis and SQL Server use named volumes for data persistence.

Keycloak is not containerised. In development, the override hardcodes `host.docker.internal` addresses to reach a locally running Keycloak instance. In production, real issuer URLs are supplied via `.env`.
