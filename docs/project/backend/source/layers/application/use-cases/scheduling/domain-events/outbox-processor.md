# Outbox Processor

## What Problem It Solves

When a write operation commits (e.g., creating a company), the entity raises one or more
domain events in memory. These events represent facts that other parts of the system must
react to. However, handling side-effects synchronously inside the original HTTP request
creates coupling, increases latency, and risks partial failure.

The **Transactional Outbox pattern** solves this by persisting domain events as
`OutboxMessage` rows in the same database transaction as the write. This guarantees
atomicity: either both the entity change and the event record are saved, or neither is.
A background job then processes these rows asynchronously and independently of the
original request.

---

## How Domain Events Reach the Outbox

This happens automatically inside `UnitOfWork.CommitAsync` before `SaveChangesAsync` is
called. The method `AddDomainEventsAsOutboxMessages` (`UnitOfWork.cs`) iterates every
`BaseEntity` currently tracked by the persistence context's change tracker, drains its in-memory
`DomainEvents` collection, and converts each event into a serialized `OutboxMessage` row.

The `OutboxMessage` stores:
- `Type` — assembly-qualified type name used to deserialise back to the correct CLR type
- `Content` — JSON-serialized event payload
- `OccurredOnUtc` — UTC timestamp of when the event was raised
- `ProcessedOnUtc` — set by the processor once handling completes
- `Error` — exception details if handling failed

These rows are inserted atomically with the main entity change. If `SaveChangesAsync`
fails, no `OutboxMessage` rows exist and no handlers will fire.

**Source:** `Api/Source/Infrastructure/Infra.Persistence/UnitOfWorks/UnitOfWork.cs`

---

## Processing Flow

The `OutboxProcessor` is a partial class implementing `IOutboxProcessor`. It is invoked
by a scheduled job in `Adapter.Scheduler` on a configurable interval.

**Public entry point:** `ProcessOutboxMessagesAsync(batchSize, jsonSerializerOptions, ct)`

The method wraps the entire batch in a single transaction
(`_unitOfWork.ExecuteTransactionAsync`) and follows this sequence:

1. **Fetch a batch** of unprocessed messages via `IOutboxMessageService
   .GetUnprocessedMessagesAsync(batchSize, ct)`. If this fails, log a warning and return
   a failure result — the transaction rolls back.

2. **For each message:**
   - Resolve the CLR type from `OutboxMessage.Type` using `Type.GetType(...)`. If the
     type cannot be resolved, a `{Solution}Exception` is thrown.
   - Deserialise the JSON payload back to the concrete `IDomainEvent` instance.
   - Create a **new DI scope** via `IServiceScopeFactory.CreateScope()`. Handlers are
     scoped services — a fresh scope prevents cross-message contamination.
   - Resolve all registered `IDomainEventHandler<TDomainEvent>` implementations from
     the scope using a `ConcurrentDictionary`-backed cache (`HandlersDictionary`) to
     avoid repeated reflection on every tick.
   - Call `handler.Handle(domainEvent, ct)` for each handler in sequence.
   - If any handler throws, catch the exception and store it — do **not** abort the
     batch. The remaining messages continue processing.

3. **Mark the message** as processed by calling `OutboxMessage.Update(exception?.ToString())`.
   If processing succeeded, `ProcessedOnUtc` is set and `Error` remains null. If it
   failed, `Error` contains the exception details for inspection and retry.

4. Log completion and return `Result.Success`.

**Sources:**  
`Api/Source/Application/App.UseCases/Scheduling/DomainEvents/OutboxProcessor.cs`  
`Api/Source/Application/App.UseCases/Scheduling/DomainEvents/OutboxProcessor.Private.cs`

---

## Handler Resolution and Caching

`HandlersDictionary` is a static `ConcurrentDictionary<string, Type[]>`. The key is
`"{AssemblyName}{EventTypeName}"`. On first encounter the processor reflects over the
`App.UseCases` assembly to find every type assignable to
`IDomainEventHandler<TDomainEvent>` for the given event, then caches the result. All
subsequent calls for the same event type pay zero reflection cost.

Handlers are resolved via `serviceProvider.GetRequiredService(handlerType)` from a
scoped DI container created per message, not per batch.

---

## Critical Rules

- The processor **never calls handlers directly**. Resolution always goes through DI so
  that handler constructor dependencies (services, etc.) are properly injected.
- A single failed handler does not abort the batch. Each message's exception is caught
  individually and recorded on its `OutboxMessage` row.
- The `ConcurrentDictionary` cache is static — it survives the lifetime of the host
  process. Handler types must not change after startup.
- Do not add new handler resolution logic elsewhere. All discovery is centralised in
  `OutboxProcessor.Private.cs`.
