# Domain Event Handlers

## Role

Handler classes are the side-effect layer for domain events. After a write operation
commits, the Outbox Processor picks up the persisted event and invokes every registered
handler for that event type. Handlers receive a strongly-typed `IDomainEvent` and perform
the downstream work that must happen as a consequence — such as syncing a read model,
sending a notification, or updating a related aggregate via a service call.

Handlers run **outside the original HTTP transaction**. They are invoked from a background
job (`Adapter.Scheduler`), not from the request pipeline.

---

## Rules

- Handlers may only call **services** (`I{Entity}Service`). Direct repository or DbContext
  calls are forbidden — all persistence must go through the service layer.
- Handlers must never start a new transaction themselves. If transactional behaviour is
  needed, it belongs in a UseCase, not a handler.
- Handlers are `internal sealed` — they are implementation details of the `App.UseCases`
  assembly and must not be exposed.
- One handler class per domain event type. If multiple side-effects share the same event,
  create multiple independent handler classes.

---

## Anatomy

Every handler inherits from `DomainEventHandler<TDomainEvent>` (defined in
`Api/Source/Core/Core.Library/Contracts/DomainEvents/Handle/DomainEventHandler.cs`).
The base class wires the non-generic `Handle(IDomainEvent)` dispatch to the strongly-typed
overload, so the handler only needs to implement the typed `Handle` method.

```csharp
using Core.Domain.Entities.{Module}.{Entities}.DomainEvents;
using Core.Library.Contracts.DomainEvents.Handle;

namespace App.UseCases.Scheduling.DomainEvents.{Module}.{Entities};

internal sealed class {Entity}{Event}DomainEventHandler : DomainEventHandler<{Entity}{Event}DomainEvent>
{
    public override Task Handle(
        {Entity}{Event}DomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // TODO: handle {Entity}{Event}DomainEvent here.
        // Only service calls are allowed in this method.
        // Example: await _{entity}Service.SyncReadModelAsync(domainEvent.{Entity}Id, cancellationToken);

        return Task.CompletedTask;
    }
}
```

Handlers that require a service dependency inject it via the constructor:

```csharp
internal sealed class {Entity}{Event}DomainEventHandler(
    I{Entity}Service {entity}Service) : DomainEventHandler<{Entity}{Event}DomainEvent>
{
    private readonly I{Entity}Service _{entity}Service = {entity}Service;

    public override async Task Handle(
        {Entity}{Event}DomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        // TODO: handle {Entity}{Event}DomainEvent here.
        // Only service calls are allowed in this method.
        // Example: await _{entity}Service.SyncReadModelAsync(domainEvent.{Entity}Id, cancellationToken);
    }
}
```

---

## DI Registration

Handlers are **not** manually registered. `ServiceRegistrar.AddDomainEventHandlers` scans
the `App.UseCases` assembly at startup, finds every type that implements
`IDomainEventHandler`, and registers each one as `TryAddScoped`.

**File:** `Api/Source/Application/App.UseCases/ServiceRegistrar.cs`

This means adding a new handler file is enough — no explicit registration step is
required.