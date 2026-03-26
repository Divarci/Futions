---
agent: agent
description: >
  Implements a complete backend feature end-to-end.
  Reads business and technical requirements by feature ID, implements all architectural layers
  (entity → controller), builds, creates the Bruno collection, writes and runs domain and
  service unit tests, performs a full compliance review, and finishes with a final clean build.
---

# Feature Implementation Pipeline

You are the **AI Orchestrator** executing a complete end-to-end backend feature implementation.

**First action:** Ask the user for the **Use Case ID** they want to implement (e.g. `UC-01`, `UC-02`). Wait for their answer, then refer to it as `FEATURE_ID` throughout all phases below.

This is a **sequential, gated pipeline**. Complete each phase fully before moving to the next. Never skip a phase. Never proceed with build errors or failing tests.

---

## Phase 0 — Requirements Discovery

### 0.1 — Business Requirements

Read `docs/requirements/backend/business-requirements.md`.

Search for and extract the full section matching **`FEATURE_ID`**.

Capture:
- Use case title and description
- All business rules and constraints
- Pre/post conditions and acceptance criteria

### 0.2 — Technical Requirements

Read `docs/requirements/backend/technical-requirements.md`.

Search for and extract the full section matching **`FEATURE_ID`**.

Capture:
- Entity definition (fields, types, mutability, constraints) — if a new entity is introduced
- API contract: HTTP method, route, request body, response body, status codes
- Data models: request models, domain models, view models
- Business rule enforcement locations
- Database schema (tables, indexes) — if applicable

**Checkpoint**: Both requirement sections for `FEATURE_ID` must be fully read and understood before any code is written. If no matching section is found, stop and report it to the user.

---

## Phase 1 — Backend Implementation

Read `.github/instructions/backend-coder.instructions.md` completely. Follow its professional workflow exactly — do not abbreviate, skip, or reorder steps.

The Backend Coder workflow requires:

1. **Read the Information Index** — `docs/project/backend/source/information/information-index.md`
2. **Read the Layer Index** — `docs/project/backend/source/layers/layer-index.md`
3. **Read each relevant Layer Index** for the layers touched by this feature
4. **Examine an existing, similar implementation** as a concrete reference before writing any code

Then implement in Clean Architecture dependency order:

### Layer 1 — Core / Library
Add new abstractions, contracts, result types, or exceptions **only if this feature requires them**.
Documentation: `docs/project/backend/source/layers/core/core-index.md`

### Layer 2 — Core / Domain
Add entity definition and value objects **only if this feature introduces a new domain entity**.
Documentation: `docs/project/backend/source/layers/core/core-index.md`

### Layer 3 — Application / Services
Add service interface and implementation covering all operations in `FEATURE_ID`.
Documentation: `docs/project/backend/source/layers/application/application-index.md`

### Layer 4 — Application / Use Cases
Add use case class **only if this feature requires orchestration beyond a single service call**.
Documentation: `docs/project/backend/source/layers/application/application-index.md`

### Layer 5 — Infrastructure / Persistence
Add repository implementation and EF Core entity configuration **only if this feature introduces a new entity or new query patterns**.
Documentation: `docs/project/backend/source/layers/infrastructure/infrastructure-index.md`

### Layer 6 — Adapter / Rest API
Add request/response models, mapper, controller action, route registration, and Bruno collection entries for all new endpoints.
Documentation: `docs/project/backend/source/layers/adapter/adapter-index.md` (includes Bruno guidelines)

**Non-negotiable rules throughout implementation:**
- Match namespaces, folder structures, and naming conventions exactly as documented
- Apply the Result pattern on every service operation — no exceptions for business logic
- Respect Clean Architecture dependency rules — no higher-layer references from lower layers
- Do not invent patterns not covered by the documentation — stop and ask if something is missing

---

## Phase 2 — Build Verification

Run the build:

```bash
dotnet build
```

- **Errors present**: Fix every compilation error. Re-run build until clean. Do not proceed until the build is green.
- **Build clean**: Proceed to Phase 3.

---

## Phase 3 — Domain Unit Tests

Read `.github/instructions/backend-tester.instructions.md` completely. Follow its guideline-driven workflow exactly.

For **domain tests**, follow these steps:

1. Read `docs/project/backend/tests/domain-tests/testing-index.md` completely
2. Identify all domain entity operations relevant to `FEATURE_ID`
3. For each operation, read the specific guideline file(s) referenced by the index completely
4. Extract all: test patterns, naming conventions, folder structure, required test count per operation
5. Implement domain entity unit tests matching the documented patterns character-by-character in naming, spacing, and structure
6. Run the tests:

```bash
dotnet test
```

Fix every failing test before continuing. All tests must be green.

---

## Phase 4 — Service Unit Tests

Continue the **Backend Testing Coder** workflow for the service layer.

1. Read `docs/project/backend/tests/service-tests/testing-index.md` completely
2. Identify all service operations for `FEATURE_ID`
3. For each operation, read the specific guideline file(s) referenced by the index completely
4. Extract all: test patterns, naming conventions, folder structure, required test count per operation
5. Implement service layer unit tests matching the documented patterns exactly
6. Run the tests:

```bash
dotnet test
```

Fix every failing test before continuing. All tests must be green.

---

## Phase 5 — Full Compliance Review

Re-engage the **Backend Coder**. Perform a full compliance review of everything implemented for `FEATURE_ID`.

Cross-reference the entire implementation against:

- `docs/project/backend/source/information/information-index.md` — cross-cutting concerns and coding style  
- `docs/project/backend/source/layers/core/core-index.md` — Domain and Library layer patterns  
- `docs/project/backend/source/layers/application/application-index.md` — Service and Use Case patterns  
- `docs/project/backend/source/layers/infrastructure/infrastructure-index.md` — Repository and persistence patterns  
- `docs/project/backend/source/layers/adapter/adapter-index.md` — REST API and Bruno patterns  
- `docs/requirements/backend/business-requirements.md` — `FEATURE_ID` section  
- `docs/requirements/backend/technical-requirements.md` — `FEATURE_ID` section  

For every gap found: fix it immediately. Apply zero tolerance for deviations from documented patterns.

---

## Phase 6 — Final Build & Test

Run the final combined verification:

```bash
dotnet build
dotnet test
```

Both commands must exit cleanly with no errors and no failing tests.

---

## Completion Criteria

Feature **`FEATURE_ID`** is fully complete when **all** of the following are satisfied:

| # | Criterion |
|---|---|
| 1 | Business and technical requirements for `FEATURE_ID` are fully implemented |
| 2 | All architectural layers implemented in correct Clean Architecture order |
| 3 | `dotnet build` exits with zero errors |
| 4 | Bruno `.bru` files created for every new endpoint (as part of Layer 6 / adapter-index) |
| 5 | Domain unit tests written, compliant with guidelines, and passing |
| 6 | Service unit tests written, compliant with guidelines, and passing |
| 7 | `dotnet test` exits with all tests green |
| 8 | Implementation reviewed and confirmed fully compliant with all layer documentation |
| 9 | Final `dotnet build` and `dotnet test` are both clean |
