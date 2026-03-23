# Scheduling — Overview

## Role

The `Scheduling/` folder is the application layer's home for all background work logic.
Any operation that must happen outside the request lifecycle — deferred processing,
periodic cleanup, async propagation of side-effects — is implemented here as pure
application code.

This folder contains **no infrastructure** and **no framework bindings**. It only defines
what the background work does, not how it is scheduled or triggered. Scheduling mechanics
(intervals, triggers, host configuration) live in the adapter layer.

---

## Current Structure

```
Scheduling/
└── DomainEvents/
    ├── Organisations/
    │   └── {Module}/
    │       └── {Entity}{Event}DomainEventHandler.cs
    ├── OutboxProcessor.cs
    └── OutboxProcessor.Private.cs
```

The domain events subfolder is the reference implementation for how background work is
structured in this layer. Any new background concern follows the same two-class model:
a **processor** and one or more **handlers**.

---

## The Pattern: Processor + Handlers

Every background concern is split into two responsibilities:

**Processor** — orchestrates the work unit. It knows how to fetch pending items, how to
dispatch them to handlers, and how to record the outcome (success or failure). It does
not know what any specific handler does. Real example:
`Api/Source/Application/App.UseCases/Scheduling/DomainEvents/OutboxProcessor.cs`

**Handler** — performs one specific reaction to one specific item type. It receives a
strongly-typed item and calls services to carry out the side-effect. It has no knowledge
of batching, scheduling, or persistence of processing state. Real example:
`Api/Source/Application/App.UseCases/Scheduling/DomainEvents/Organisations/`

This separation means the processor is stable and generic; only handlers change when new
reactions are needed.

---

## How to Add New Background Work

### 1 — Create a subfolder under `Scheduling/`

Name it after the concern, not the mechanism:

```
Scheduling/
└── {Concern}/               e.g. Cleanup/, Sync/, Notifications/
    ├── {Concern}Processor.cs
    └── {Module}/
        └── {Item}Handler.cs
```

### 2 — Define a processor

The processor follows the same structure as `OutboxProcessor`:

- `internal partial class {Concern}Processor` implementing `I{Concern}Processor`
- Constructor injects: unit of work, a service to fetch pending items, a logger
- The public method wraps the batch in a transaction, fetches pending items, iterates
  them, dispatches each to its handlers, and records the outcome
- A private partial file (`{Concern}Processor.Private.cs`) holds handler resolution and
  any other private helpers

The `I{Concern}Processor` interface is declared in
`Core.Domain/Entities/System/{Concern}/Interfaces/` following the same placement as
`IOutboxProcessor`.

### 3 — Define handlers

Each handler file is `internal sealed class {Item}Handler` implementing
`I{Item}Handler<T{Item}>`. Handlers:

- Receive a strongly-typed item
- Call only services — no repository or infrastructure calls directly
- Are discovered and registered automatically if the assembly scan convention from
  `ServiceRegistrar.cs` is extended to cover the new handler interface

### 4 — Register the processor in `ServiceRegistrar.cs`

Add `services.AddScoped<I{Concern}Processor, {Concern}Processor>()` in the
`RegisterUseCases` private method, alongside the existing use case registrations.

### 5 — Wire the trigger in the adapter layer

Once the processor exists in the application layer, the adapter layer
(`Adapter.Scheduler`) adds a job class that calls
`{concern}Processor.Process{Items}Async(...)` and registers a trigger for it. No
changes to the application layer are needed after step 4.

---

## Critical Rules

- Processors and handlers are `internal` — they are never exposed outside `App.UseCases`.
- Handlers may only call **services**. Direct repository access or infrastructure calls
  from a handler violate the layer boundary.
- Each handler handles **one item type**. If the same item requires multiple reactions,
  create multiple independent handler classes.
- The processor owns result recording (success / error). Handlers must not update
  processing state themselves.
- Processors wrap their work in a transaction via `ITransactionalUnitOfWork` — they
  never call `SaveChangesAsync` directly.
