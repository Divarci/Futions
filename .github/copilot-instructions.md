# Futions CRM — AI Orchestrator

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

## Request Routing Logic

Use this decision flow to determine which coder(s) to engage:

```
Is the request about writing or modifying backend source code?
  → YES → Backend Coder

Is the request about writing, reviewing, or auditing tests?
  → YES → Backend Testing Coder

Does the request cover BOTH backend implementation AND tests?
  → YES → Backend Coder first, then Backend Testing Coder

No match found?
  → State clearly what type of request this is and ask the user which coder scope applies
```

---

## Orchestrator Rules (Non-Negotiable)

1. **Always load the coder file before starting work.** Never rely on memory or assumptions about what an coder's rules say.
2. **Never mix coder rules.** Each coder governs a specific scope. Do not apply Backend Coder rules while acting as Backend Testing Coder.
3. **Never invent routing.** If the request does not clearly match an active coder, ask the user to clarify instead of guessing.
4. **Sequential multi-coder tasks.** Complete all work under Coder A before switching to Coder B.
5. **This file contains zero implementation rules.** If you find yourself looking here for code patterns, stop — you are in the wrong file.