# Futions — AI Orchestrator

> **This file is the single entry point for all AI-assisted work on this project.**
> It does not contain implementation rules or code patterns. Its sole responsibility is to **analyze incoming requests and delegate them to the correct Sub-Coder**.
> Sub-Coders hold the real rules. The Orchestrator is just the router.

---

## How the Orchestrator Works

1. **Receive** the user's request (feature, review, test, API collection, etc.)
2. **Analyze** — classify the request using the routing table below
3. **Locate** the matching Sub-Coder entry in the Active Coder Registry
4. **Load** that coder's instruction file completely before starting work
5. **Delegate** — follow that coder's rules exclusively for the duration of the task

If a request spans multiple domains (e.g., backend feature + tests), activate each relevant coder **in sequence**, completing one before starting the next.

---

## Active Coder Registry

Each entry defines **when** to engage the coder and **where** to find its rules.

---

### 1. Backend Coder
| Field | Value |
|---|---|
| **File** | `.github/instructions/backend-coder.instructions.md` |
| **Role** | Senior Backend Developer |
| **Status** | Active |

**Engage when the request involves:**
- Domain entities, value objects, aggregates
- Application services and business logic
- Use Cases and workflows
- Repository implementations
- Endpoints implementations
- EF Core configuration, migrations
- Any backend layer (Domain / Application / Infrastructure / Adapter)
- Backend code review or architecture compliance check

---

### 2. Backend Testing Coder
| Field | Value |
|---|---|
| **File** | `.github/instructions/backend-testing-coder.instructions.md` |
| **Role** | Senior Test Developer |
| **Status** | Active |

**Engage when the request involves:**
- Writing unit tests for service layer
- Writing unit tests for domain layer
- Test review or coverage gap analysis
- Test suite audit or guideline compliance check

---

### 3. Frontend Coder
| Field | Value |
|---|---|
| **File** | `.github/instructions/frontend-coder.instructions.md` |
| **Role** | Senior Frontend Developer |
| **Status** | Active |

**Engage when the request involves:**
- Primitive components, shared types, utility functions (`core/` layer)
- HTTP client configuration, API call functions (`infra/` layer)
- SWR hooks, feature components, domain types (`features/` layer)
- Next.js pages, layouts, route segments (`app/` layer)
- Frontend code review or architecture compliance check

---

### 4. Frontend Testing Coder
| Field | Value |
|---|---|
| **File** | `.github/instructions/frontend-tester.instructions.md` |
| **Role** | Senior Frontend Test Developer |
| **Status** | Active |

**Engage when the request involves:**
- Writing unit tests for SWR hooks (`features/` layer)
- Writing unit tests for feature components (`features/` layer)
- Writing unit tests for core utility functions (`core/utils/`)
- Frontend test review or coverage gap analysis

---

### 5. Design Decisions Coder
| Field | Value |
|---|---|
| **File** | `.github/instructions/design-decisions-coder.instructions.md` |
| **Role** | Design Decisions Recorder |
| **Status** | Active |

**Engage when the request involves:**
- Creating `docs/project/frontend/source/information/design-decisions.md` for the first time
- Updating design decisions after a design change (token values, typography, spacing, motion, etc.)
- Recording or refreshing the project's concrete design state from the designer docs + implementation files

---

## Request Routing Logic

Use this decision flow to determine which coder(s) to engage:

```
Is the request about writing or modifying backend source code?
  → YES → Backend Coder

Is the request about writing or reviewing backend tests?
  → YES → Backend Testing Coder

Is the request about writing or modifying frontend source code?
  → YES → Frontend Coder

Is the request about writing or reviewing frontend tests?
  → YES → Frontend Testing Coder

Does the request cover BOTH backend implementation AND backend tests?
  → YES → Backend Coder first, then Backend Testing Coder

Does the request cover BOTH frontend implementation AND frontend tests?
  → YES → Frontend Coder first, then Frontend Testing Coder

Is the request about recording or updating design decisions?
  → YES → Design Decisions Coder

Does the request cover BOTH backend AND frontend?
  → YES → Backend Coder first, then Frontend Coder

No match found?
  → State clearly what type of request this is and ask the user which coder scope applies
```

---

## Orchestrator Rules (Non-Negotiable)

1. **Always load the coder file before starting work.** Never rely on memory or assumptions about what a coder's rules say.
2. **Never mix coder rules.** Each coder governs a specific scope. Do not apply Backend Coder rules while acting as Frontend Coder.
3. **Never invent routing.** If the request does not clearly match an active coder, ask the user to clarify instead of guessing.
4. **Sequential multi-coder tasks.** Complete all work under Coder A before switching to Coder B.
5. **This file contains zero implementation rules.** If you find yourself looking here for code patterns, stop — you are in the wrong file.

---

## Skills

Skills are self-contained workflow prompt files. When a user invokes a skill command, load its file and follow it exactly as the complete operating procedure for that session.

### `/Feature` — Full Backend Feature Pipeline

| Field | Value |
|---|---|
| **File** | `.github/prompts/Feature.prompt.md` |
| **Trigger** | `/Feature` in Copilot Chat (Agent Mode) |
| **Scope** | Complete backend feature — from requirements to final build |

**What it does (in order):**
1. Asks for the **Use Case ID** (e.g. `UC-01`)
2. Reads the matching section from `business-requirements.md` and `technical-requirements.md`
3. Delegates implementation to **Backend Coder** — all layers, entity → controller
4. Runs **`dotnet build`** — must be clean before continuing
5. Creates the **Bruno collection** entries for all new endpoints
6. Delegates **domain unit tests** to **Backend Testing Coder** — runs and must be green
7. Delegates **service unit tests** to **Backend Testing Coder** — runs and must be green
8. Performs a full **compliance review** against all layer documentation
9. Runs final **`dotnet build` + `dotnet test`** — both must be clean

**Invoke when the user wants to implement a single backend use case end-to-end.**