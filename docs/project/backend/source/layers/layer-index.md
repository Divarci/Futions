# Backend Layers — Index

The backend is built on Clean Architecture. Layers form a strict inward-only dependency graph: Adapter → Application → Core ← Infrastructure. No layer may reference a layer further out than itself.

| Layer | Index | Responsibility |
|---|---|---|
| Core | [core-index.md](core/core-index.md) | Domain entities, value objects, shared primitives, contracts — zero outward dependencies |
| Application | [application-index.md](application/application-index.md) | Business orchestration: services, use cases, background processors |
| Infrastructure | [infrastructure-index.md](infrastructure/infrastructure-index.md) | Technology implementations: persistence and caching |
| Adapter | [adapter-index.md](adapter/adapter-index.md) | Entry points: HTTP API and background job host |
