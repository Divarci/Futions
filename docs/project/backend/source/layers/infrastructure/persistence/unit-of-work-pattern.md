# Unit of Work Pattern

`UnitOfWork` is the only component that calls `SaveChangesAsync`. It wraps an operation
in a database transaction and rolls back automatically on failure.

---

## Design

The pattern is split across three concerns:

**Contract** — `ITransactionalUnitOfWork`
`Api/Source/Core/Core.Library/Contracts/UnitOfWorks/ITransactionalUnitOfWork.cs`

Exposes two overloads of `ExecuteTransactionAsync`. Use the non-generic `Result` overload
for commands that produce no value (Update, Delete). Use the generic `Result<T>` overload
for commands that return a created entity (Create). The UseCase layer always calls through
this interface — never the concrete class.

**Implementation** — `UnitOfWork`
`Api/Source/Infrastructure/Infra.Persistence/UnitOfWorks/UnitOfWork.cs`

Each overload begins a transaction, awaits the caller's delegate, and branches on the
result. A failure result triggers `RollbackAsync` immediately. On success, `CommitAsync`
is called internally: it first writes all pending domain events as `OutboxMessage` rows
via `ChangeTracker`, then calls `SaveChangesAsync` and `transaction.CommitAsync` — both
business data and outbox rows land in the same transaction. Any unhandled exception is
caught, the transaction is rolled back, and an `InternalServerError` result is returned.

The only difference between the two overloads is the failure check: the non-generic
overload uses `result.IsFailure`; the generic overload uses `result.IsFailureAndNoData`,
which lets a partial-success result (data present, non-2xx status) still commit.

**DI Registration**
`Api/Source/Infrastructure/Infra.Persistence/ServiceRegistrar.cs`

Registered as `AddScoped<ITransactionalUnitOfWork, UnitOfWork>()`.

---

## Usage in the UseCase Layer

Every write operation in a UseCase is wrapped inside `ExecuteTransactionAsync`. Real
examples in the codebase:

- `Api/Source/Application/App.UseCases/UseCases/Organisations/People/PersonUseCase.Create.cs` — generic overload; creates the entity then writes the audit log inside one delegate. If either step fails, both writes roll back.
- `Api/Source/Application/App.UseCases/UseCases/Organisations/People/PersonUseCase.Update.cs` — non-generic overload.
- `Api/Source/Application/App.UseCases/UseCases/Organisations/People/PersonUseCase.Delete.cs` — non-generic overload.
- `Api/Source/Application/App.UseCases/Scheduling/DomainEvents/OutboxProcessor.cs` — non-UseCase consumer; outbox retrieval and status update are atomic.

---

## Critical Rules

- `SaveChangesAsync` is called **only** inside `UnitOfWork`. Repositories must never call
  it directly.
- Both `Result` (non-generic) and `Result<T>` (generic) overloads exist. Use the overload
  matching your operation's return type.
- A failure result from the operation triggers `RollbackAsync` before returning to the
  caller. No partial writes survive.
- Exceptions are caught, the transaction is rolled back, and a
  `HttpStatusCode.InternalServerError` result is returned. The exception is logged with
  a trace ID before being swallowed.
- Application services call `ITransactionalUnitOfWork.ExecuteTransactionAsync`, never
  `UnitOfWork` directly.
