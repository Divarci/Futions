# Domain-Driven Design

This document summarises the DDD decisions made in the domain layer.

---

## Bounded Context

The solution defines a single bounded context. Within it, the domain is divided into two sub-domains:

- **Business sub-domain** — models the core domain concepts: the entities and relationships that represent what the application is actually about.
- **System sub-domain** — hosts cross-cutting infrastructure entities (`AuditLog`, `OutboxMessage`) that are not part of the business model but must live in the domain layer because they follow the same entity lifecycle rules.

This separation prevents infrastructure concerns from polluting the business model while keeping both governed by the same domain rules.

---

## Entity Design Decisions

**Private constructor + static factory**
Entities cannot be constructed directly. All creation goes through a static `Create()` method. This ensures validation always runs before an entity instance is returned to any caller. An entity that fails validation never exists.

**Private setters**
All entity properties have private setters. State changes are only possible through explicit behaviour methods (`UpdateName`, `SoftDelete`, etc.). This makes it impossible to mutate an entity accidentally by assigning to a property directly.

**Partial class split**
Each entity is split across two files: one for state and behaviour, one for validation logic. This keeps the main entity file focused on what the entity does rather than how it validates itself.

**`BaseEntity` in Core.Library**
All entities inherit from `BaseEntity`, which provides a `Guid` identity and the domain event infrastructure. Placing `BaseEntity` in `Core.Library` rather than `Core.Domain` makes it reusable across solutions without pulling in any domain-specific code.

**GUID identity**
Entities use `Guid` as their identifier. This allows identity to be assigned by the application before the entity is persisted, which is required for the outbox pattern and for raising domain events that reference the entity id.

---

## Value Object Design Decisions

**`sealed record` as the base type**
Value objects are C# records, giving structural equality by default — two value objects with the same property values are considered equal. `sealed` prevents extension.

**Same private-constructor + static factory pattern as entities**
Value objects follow the same construction discipline as entities. They cannot be created with invalid state.

**`init`-only properties**
Value object properties are set only at construction. Once created, a value object is immutable. To "change" a value, a new instance is created.

**Separate validation partial file**
Value objects follow the same partial class split as entities for the same reason: to keep validation logic isolated and easy to locate.

---

## Aggregate Decisions

**No formal aggregate root pattern**
The codebase does not use a strict aggregate root hierarchy. Each entity manages its own consistency boundaries. Referential integrity between entities (e.g., a parent-child relationship) is enforced at the service layer before the entity is mutated, not inside the aggregate.

**Why this was chosen**
A strict aggregate root pattern adds significant complexity (nested hydration, cross-aggregate reference rules) and was not justified given the relatively flat entity relationships in this domain. Each entity is effectively its own aggregate root and is responsible only for its own invariants.

**Consistency rule**
Deleting or modifying an entity that other entities depend on requires a guard check in the service layer before the operation proceeds. The domain entity itself does not know about these dependencies.

---

## Domain Event Decisions

**One event per state change**
Every meaningful state transition in the domain raises a dedicated domain event. Events are named after the fact: `{Entity}CreatedDomainEvent`, `{Entity}NameUpdatedDomainEvent`.

**Events raised inside the entity**
Domain events are raised by the entity itself at the point of the state change, not by the service or use case. This ensures that if a state change happens, the corresponding event is always raised — there is no way to call `UpdateName` without `{Entity}NameUpdatedDomainEvent` being produced.

**Collected, not dispatched immediately**
Events are not dispatched inline. They accumulate in the entity's `DomainEvents` collection. The Unit of Work converts them to `OutboxMessage` rows on commit, enabling reliable asynchronous processing without distributed transactions.

---

## Repository Interface Decisions

**Interfaces defined in Core.Domain, not Core.Library**
Repository interfaces live next to the entity they belong to, inside `Core.Domain`. Generic base contracts (`IBaseRepository<T>`, `IGlobalRepository<T>`, `ITenantedRepository<T>`) live in `Core.Library` and are extended per entity.

**Tenanted vs global repositories**
Entities that carry a `TenantId` extend `ITenantedRepository<T>`, which requires a `tenantId` parameter on every read method. Infrastructure-level entities that are not tenant-scoped extend `IGlobalRepository<T>`. This distinction is enforced at the interface level so a tenanted entity can never be queried without a tenant filter.

---

## Model (DTO) Decisions

**`CreateModel` and `UpdateModel` belong to Core.Domain**
Models used to pass data into entity factory and update methods are defined in `Core.Domain` alongside the entity. They are part of the domain contract, not the API layer.

**Nullable properties on `UpdateModel`**
Update models use nullable properties throughout. A `null` value means "not supplied — do not change this field." This supports partial update semantics without requiring separate request types per field combination.

**No validation in models**
Models carry no validation logic or data annotations. Validation is the entity's responsibility. Models are simple data carriers.

---

## Multi-Tenancy Decisions

**Decided at the domain level via marker interfaces**
Tenant isolation starts in the domain. The `IHaveTenant` marker interface signals that an entity belongs to a tenant. This intent propagates upward: repositories that hold a tenanted entity automatically scope their queries; the API layer enforces that the caller's JWT tenant matches the route tenant.

**Tenant id is immutable after creation**
`TenantId` uses `private init`. Once an entity is created for a tenant, its tenant cannot be changed. This is a deliberate invariant.

**`SystemAdmin` as the escape hatch**
The only role that can bypass tenant isolation is `SystemAdmin`. This is enforced at the API layer, not the domain layer, since the domain has no knowledge of authentication concepts.

---

## Soft Delete Decisions

**Marker interface, not a base class**
Soft delete behaviour is opt-in via the `IHaveSoftDelete` interface. Not every entity supports soft delete — linking entities (e.g., a relationship entity) use hard delete instead.

**`IsDeleted` is an explicit domain state**
Soft deletion is a business operation with a domain event, not a technical flag silently set by infrastructure. Calling `SoftDelete()` on an entity raises a domain event, which can trigger downstream reactions (e.g., cache invalidation, audit logging).
