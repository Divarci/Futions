# Abstractions

## What It Does

Provides the foundational base class that every domain entity must extend, and a set of
marker interfaces used to opt an entity into cross-cutting behavioral contracts. Nothing
here is infrastructure-aware — these are pure domain primitives.

---

## Current Structure

### BaseEntity

The root class for all entities. Holds a Guid identity that is set at construction
(init-only) and never mutated. Manages an internal domain event list: Raise() appends
an event, DomainEvents is a read-only view over that list, and ClearDomainEvents() is
called by the infrastructure after event dispatch. Inheriting this class is mandatory for
all entities.

### IHaveSoftDelete

Opt-in interface for soft-deletable entities. Requires the entity to expose
bool IsDeleted { get; } and implement Result SoftDelete(). The infrastructure layer
uses this interface to apply automatic query filters and to route deletes to a soft-delete
path instead of a hard-delete.

### IHaveTenant

Opt-in interface for tenant-scoped entities. Requires Guid TenantId { get; }. The
infrastructure layer uses this interface to apply automatic per-tenant query filters.

### IHaveAutoSeedData

Pure marker interface. Signals to the infrastructure that the implementing enum has fields
decorated with AutoSeedDataAttribute, and that seed data should be applied to the
database at migration.

### IOutboxOptions

Configuration contract for the outbox background processor. Defines int IntervalInSeconds
and int BatchSize. Implemented as a record and consumed by the background scheduler job
to control polling frequency and throughput.

---

## How to Scale

When a new cross-cutting concern needs to apply to a subset of entities (e.g., auditing,
optimistic concurrency, archiving), define a new marker or behavioral interface here. The
infrastructure layer is responsible for scanning for that interface and applying the
appropriate behavior through query filters, interceptors, or persistence configuration.

Do not add concern-specific logic inside the interface itself. Business logic for the
behavior belongs in the entity implementation; infrastructure logic belongs in the
Infrastructure layer.

---

## Critical Rules

- Every entity must extend BaseEntity. There are no exceptions to this rule.
- Marker interfaces are strictly opt-in. Do not add them to BaseEntity.
- IHaveSoftDelete.SoftDelete() must return Result. It must never throw.
- BaseEntity.Id is init-only. It is set once at construction and is never mutated.
- ClearDomainEvents() is called exclusively by the infrastructure after publishing. It
  must never be called from domain or application code.
