# Event-Driven Design

This document explains how the backend communicates internally using domain events, including publishing, persistence, and asynchronous processing.

---

## Overview

The system implements the **Transactional Outbox** pattern. Domain events raised by entities are:

1. Collected in-memory during the request.
2. Serialised into `OutboxMessage` rows within the same database transaction.
3. Processed asynchronously by a background scheduler that dispatches them to registered handlers.

This guarantees that domain events are **never lost** (they share the same transaction as the business data) and **eventually processed** (the scheduler picks them up).

---

## Domain Event Contract

All domain events implement `IDomainEvent` (from `Core.Library`). The abstract base record provides a `Guid` identity and a UTC timestamp. Domain events are immutable C# **records** — they cannot be modified after creation.

---

## Event Structure

Each entity defines one event per state change. All events carry only the entity id — the handler is responsible for loading any additional data it needs.

Events follow the naming pattern `{Entity}{Verb}DomainEvent` and are placed in a `DomainEvents/` folder under the entity's folder:

| Naming Pattern | Example |
|---|---|
| `{Entity}CreatedDomainEvent` | Raised by the static `Create()` factory method |
| `{Entity}DeletedDomainEvent` | Raised by `SoftDelete()` or `Delete()` |
| `{Entity}{Property}UpdatedDomainEvent` | Raised by each individual update method |

---

## Publishing — How Events Are Raised

Entities raise events via the `Raise()` method inherited from `BaseEntity`. Events accumulate in the entity's `DomainEvents` collection during the request. They are **not dispatched inline** — nothing happens to them until the Unit of Work commits.

---

## Persistence — Transactional Outbox

When `UnitOfWork.CommitAsync()` is called at the end of a use case, it performs the following steps inside the active database transaction:

1. Iterates all tracked `BaseEntity` instances via the EF Core `ChangeTracker`.
2. For each entity, reads its `DomainEvents` collection and calls `ClearDomainEvents()` to prevent re-processing.
3. Serialises each event to JSON and creates an `OutboxMessage` row storing:
   - `Type` — the .NET assembly-qualified type name (used for deserialisation).
   - `Content` — the full JSON payload.
   - `OccurredOnUtc` — the current UTC timestamp.
4. Calls `SaveChangesAsync` and `CommitAsync` — business data changes and outbox messages are persisted in the **same transaction**. If anything fails, both roll back — no ghost events, no lost events.

### OutboxMessage Fields

| Column | Type | Purpose |
|---|---|---|
| `Id` | `Guid` | Primary key |
| `Type` | `string` (max 2000) | Assembly-qualified .NET type name |
| `Content` | `string` (max 100000) | Serialised JSON of the domain event |
| `OccurredOnUtc` | `DateTime` | When the event was raised |
| `ProcessedOnUtc` | `DateTime?` | When the message was processed (`null` = unprocessed) |
| `Error` | `string?` | Error details if the handler failed |

---

## Processing — Background Scheduler

The `Adapter.Scheduler` project runs a Quartz.NET job (`ProcessOutboxJob`) on a configurable interval (`Outbox:IntervalInSeconds`, `Outbox:BatchSize`).

### Job Execution Flow

1. Fetch unprocessed `OutboxMessage` rows ordered by `OccurredOnUtc`, limited to `BatchSize`.
2. For each message:
   - Resolve the .NET `Type` from `OutboxMessage.Type`.
   - Deserialise `OutboxMessage.Content` into the concrete domain event object.
   - Find all registered `IDomainEventHandler<T>` implementations for that event type.
   - Call each handler's `Handle()` method.
   - Mark the message as processed (`ProcessedOnUtc = now`).
   - If a handler throws, log the error and store it in `OutboxMessage.Error`.
3. `SaveChanges` + `Commit` — processing is also wrapped in a transaction.

### Handler Discovery

Handlers are discovered via reflection at startup and registered as `Scoped` services. At runtime, each handler is resolved from a new `IServiceScope` to ensure proper DI lifetime management.

### Concurrency Safety

`[DisallowConcurrentExecution]` on the job class ensures only one instance of the processor runs at a time, preventing duplicate event processing without requiring distributed locks.

---

## Domain Event Handler Contract

Handlers extend `DomainEventHandler<T>` (from `Core.Library`), which implements both the generic and non-generic `IDomainEventHandler` interfaces. Each handler implements a single `Handle(TDomainEvent, CancellationToken)` method.

Handlers are placed in `App.UseCases/Scheduling/DomainEvents/` and follow the naming pattern `{Entity}{Verb}DomainEventHandler`.

---

## Error Handling

- If a handler throws, the error is stored in `OutboxMessage.Error` and the message is marked as processed — this prevents infinite retry loops.
- If the batch fetch itself fails, the transaction rolls back and the job logs a `FutionsException`.
- If event serialisation fails during the UoW commit, the entire transaction (business data + outbox) is rolled back.

---

## Key Design Decisions

| Decision | Rationale |
|---|---|
| Outbox in same database | Eliminates distributed transaction complexity; guarantees at-least-once delivery |
| JSON serialisation | Human-readable, easy to debug, sufficient for event payloads |
| Assembly-qualified type name | Enables polymorphic deserialisation without a separate type registry |
| Configurable batch size | Prevents overloading the system under high event volume |
| Separate scheduler process | Decouples event processing from API request latency |
| `DisallowConcurrentExecution` | Prevents duplicate processing without distributed locks |


