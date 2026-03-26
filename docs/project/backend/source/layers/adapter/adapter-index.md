# Adapter Layer — Index

The Adapter layer is the outermost layer of the backend. It exposes the application to the outside world and has no business logic of its own. It is split into two projects: `Adapter.RestApi` (HTTP surface) and `Adapter.Scheduler` (background job host). Both depend on the Application layer; neither is referenced by any other layer.

Use this index to navigate directly to the topic you need.

---

## Adapter.RestApi

HTTP API surface: routing, controllers, request/response models, and mappers.

| Document | What it covers |
|---|---|
| [Structure](rest-api/api-structure.md) | Full folder tree of `Adapter.RestApi`, two-variant controller layout (flat vs nested), and critical structure rules |
| [AspNetCore Internals](rest-api/asp-net-core.md) | Every file and folder under `AspNetCore/` — filters, authentication helpers, versioning, and extension methods |
| [Endpoint Pattern](rest-api/endpoint-pattern.md) | Complete copy-ready pattern for a controller, request, response, and mapper with all critical rules |
| [Bruno Collection](rest-api/bruno.md) | Standards for the Bruno API collection — structure, request templates, variable scoping, and maintenance rules |

---

## Adapter.Scheduler

Background job host: job registration, trigger configuration, and scaling.

| Document | What it covers |
|---|---|
| [Scheduler Overview](scheduler/scheduler-info.md) | Description, structure tree, critical rules, `Program.cs` bootstrap order, and step-by-step guide for adding new jobs |

---

## Guidelines

- Read the **Structure** document before adding, moving, or renaming any file under `Adapter.RestApi`.
- Read the **Endpoint Pattern** before implementing any new controller — it is the single source of truth for controllers, requests, responses, and mappers.
- Read the **Scheduler Overview** before adding a new background job — it defines the exact four-step process and the rules every job must follow.
- No adapter file may reference another adapter project. `Adapter.RestApi` and `Adapter.Scheduler` are independent entry points.
- No business logic belongs in either adapter project. All logic is delegated to the Application layer through use case interfaces.
