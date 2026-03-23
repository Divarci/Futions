# Persistence — Configurations

EF Core `IEntityTypeConfiguration<T>` implementations. One class per entity, one file per
class. Applied automatically via `ApplyConfigurationsFromAssembly` in `AppDbContext`.

---

## Pattern

### Folder Structure

```
Configurations/
└── {Module}/
    ├── {Entity}Config.cs
    └── ...                   ← one config file per entity in this module
```

### {Entity}Config.cs — tenant-scoped entity (implements IHaveTenant + IHaveSoftDelete)

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class {Entity}Config : IEntityTypeConfiguration<{Entity}>
{
    public void Configure(EntityTypeBuilder<{Entity}> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.HasIndex(x => x.TenantId);

        builder.Property(x => x.IsDeleted)
            .IsRequired();
    }
}
```

### {Entity}Config.cs — owned value object (OwnsOne)

```csharp
public class {Entity}Config : IEntityTypeConfiguration<{Entity}>
{
    public void Configure(EntityTypeBuilder<{Entity}> builder)
    {
        builder.Property(x => x.RequiredField)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(a => a.LineOne).HasMaxLength(100);
            address.Property(a => a.LineTwo).HasMaxLength(100);
            address.Property(a => a.Postcode).HasMaxLength(20);
        });
    }
}
```

### {ChildEntity}Config.cs — one-to-many relationship (HasOne / WithMany / HasForeignKey)

Configure the relationship from the **many (child) side**. The parent entity does not need
to expose a collection navigation property.

```csharp
public class {ChildEntity}Config : IEntityTypeConfiguration<{ChildEntity}>
{
    public void Configure(EntityTypeBuilder<{ChildEntity}> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        // Many {ChildEntity} rows belong to one {ParentEntity}
        builder.HasOne(x => x.{ParentEntity})
            .WithMany()                          // no collection nav on parent
            .HasForeignKey(x => x.{ParentEntity}Id)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Property(x => x.TenantId).IsRequired();
        builder.HasIndex(x => x.TenantId);

        builder.Property(x => x.IsDeleted).IsRequired();
    }
}
```

If the parent **does** expose a collection navigation (`ICollection<{ChildEntity}>`), pass
it to `WithMany`:

```csharp
builder.HasOne(x => x.{ParentEntity})
    .WithMany(p => p.{ChildEntities})   // named collection nav on parent
    .HasForeignKey(x => x.{ParentEntity}Id)
    .OnDelete(DeleteBehavior.ClientCascade);
```

**Guidelines for this pattern:**
- Always configure from the **child (many) side** — never add `HasMany` on the parent
  config class for the same relationship.
- Prefer `WithMany()` (no argument) in line with this project's convention of not exposing
  collection navigations on domain entities.
- Use `OnDelete(DeleteBehavior.ClientCascade)` so EF Core handles cascading in-memory
  before issuing the `DELETE`, avoiding FK constraint violations.

---

### {JoinEntity}Config.cs — many-to-many via explicit join entity (HasOne / WithMany / HasForeignKey)

Use this pattern when a many-to-many relationship carries extra payload (columns beyond the
two foreign keys). Define a dedicated join entity and configure each side independently.

> Real example: `CompanyPersonConfig` — links `Company` ↔ `Person` and stores a `Title`.

```csharp
public class {JoinEntity}Config : IEntityTypeConfiguration<{JoinEntity}>
{
    public void Configure(EntityTypeBuilder<{JoinEntity}> builder)
    {
        // Extra payload on the join row
        builder.Property(x => x.Title)
            .HasMaxLength(100);

        // Side A  →  {EntityA}
        builder.HasOne(x => x.{EntityA})
            .WithMany()
            .HasForeignKey(x => x.{EntityA}Id)
            .OnDelete(DeleteBehavior.ClientCascade);

        // Side B  →  {EntityB}
        builder.HasOne(x => x.{EntityB})
            .WithMany()
            .HasForeignKey(x => x.{EntityB}Id)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
```

**Guidelines for this pattern:**
- Use `WithMany()` (no argument) when the parent entity does not expose a navigation
  collection back to the join entity. Avoids polluting the domain model with
  infrastructure-level collections.
- Always set `OnDelete(DeleteBehavior.ClientCascade)` for join entities so that EF Core
  deletes related join rows in memory before issuing the parent `DELETE`, preventing FK
  violations without relying on database-level cascade.
- The join entity must still have its own `Id` (inherited from `BaseEntity`) and must be
  registered as a `DbSet` in `AppDbContext`.
- If no extra payload is needed, prefer EF Core's implicit many-to-many (skip entity)
  over creating an explicit join entity.

---

## Critical Rules

- Every entity registered as a `DbSet` in `AppDbContext` must have a corresponding
  `{Entity}Config` class. Do not rely on EF Core conventions for column or length mapping.
- Config classes must be placed under `Configurations/{Module}/` matching the module
  structure of `Core.Domain`. No flat configurations folder.
- Never call `HasQueryFilter` in a config class. Global query filters (soft-delete, tenant)
  are applied centrally in `AppDbContext`, not per-config.
- Owned value objects must be configured with `OwnsOne` inside the owning entity's config
  class. Value objects do not have their own config file.
- `HasMaxLength` must always be applied to every `string` property. Do not leave string
  columns without an explicit length.
- Primary keys (`Id`) are configured globally via `BaseEntity` conventions. Do not redefine
  them in individual config classes.
- Config classes are `internal` and have no constructor parameters.
