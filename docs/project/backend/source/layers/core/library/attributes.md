# Attributes

## What It Does

Provides custom attributes that decorate enum fields and drive infrastructure behavior.
Specifically, marks which enum fields represent reference data that should be automatically
seeded into the database at application startup. The attributes themselves carry no runtime
logic — they are pure metadata consumed by an infrastructure seeder.

---

## Current Structure

### AutoSeedDataAttribute

Applied to individual enum fields ([AttributeUsage(AttributeTargets.Field)]). Signals to
the infrastructure seeder that this enum field corresponds to a reference record that must
exist in the database. The enum type must also implement IHaveAutoSeedData for the seeder
to discover and process it.

---

## How to Scale

Add a new attribute for each new cross-cutting concern that requires metadata-driven
infrastructure behavior. Each attribute should:

- Have a tightly scoped AttributeUsage target.
- Carry no runtime logic of its own.
- Be paired with a corresponding infrastructure component that reads the attribute and acts
  on it.

---

## Critical Rules

- Attributes in this folder must only drive infrastructure behavior, not application logic.
- AutoSeedDataAttribute must only be placed on enum fields. Applying it to a class, method,
  or property has no effect.
- The attribute alone seeds nothing. The infrastructure seeder must be registered and active.
- The owning enum type must also implement IHaveAutoSeedData for discovery to work.
