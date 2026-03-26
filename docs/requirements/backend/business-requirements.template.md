# Business Requirements Document (BRD)

<!--
  TEMPLATE INSTRUCTIONS
  ─────────────────────
  How to use this template:
  1. Write a rough draft describing your project (plain language, bullet points, anything).
  2. Give your draft + this template to the AI.
  3. AI will produce a filled, structured BRD following this format exactly.

  Placeholders use {CurlyBrace} syntax.
  Comments like this one explain what each section expects — remove them in the final doc.
-->

**Product:** {ProductName} — {One-sentence description of what the system does}
**Version:** {v1}

---

## 1. Purpose

<!--
  Why does this system exist? What problem does it solve?
  2–4 sentences. Keep it non-technical.
-->

{System purpose and context}

**Objectives:**
- {Primary objective — e.g. "Centralize task management"}
- {Secondary objective}
- {Add more as needed}

---

## 2. Scope

### In Scope

<!--
  What capabilities does this version include?
  List features and domains the backend must handle.
-->

- {Feature / capability 1}
- {Feature / capability 2}
- {Add more as needed}

### Out of Scope ({Version})

<!--
  What is explicitly NOT part of this version?
  Being explicit here prevents scope creep.
-->

- {Excluded capability 1 — e.g. "Authentication / user management"}
- {Excluded capability 2}
- {Add more as needed}

---

## 3. Stakeholders

<!--
  Who cares about this system? List roles, not names.
-->

- {Role 1 — e.g. Product Owner}
- {Role 2 — e.g. Backend Developer}
- {Role 3 — e.g. QA}

---

## 4. Key Concepts

<!--
  Define each core domain entity/concept in plain language.
  One subsection per concept. These become the domain entities in the technical spec.
  Example: ### Task / Represents a unit of work the user wants to track.
-->

### {ConceptName}

{Plain-language definition and its role in the system}

### {ConceptName}

{Plain-language definition}

---

## 5. Use Cases

<!--
  One UC entry per user-facing operation.
  Format: UC-XX: {Verb} {Noun}
  Include: actor (implicit = "User"), preconditions, notable rules, default values.
  Number sequentially. These become the Business Reference column in the API spec.
-->

### UC-01: {Action — e.g. Create Task}

{Description. Include default values and key constraints.}
- {Notable rule or behaviour — e.g. "Default status: Pending"}

### UC-02: {Action — e.g. Update Task}

{Description.}

### UC-03: {Action — e.g. Delete Task}

{Description. Note if permanent or recoverable.}

### UC-04: {Action — e.g. List Tasks}

{Description. Include supported filter/sort behaviour.}

<!--
  Add more UC entries as needed. Keep them numbered sequentially.
-->

---

## 6. Functional Requirements

<!--
  Group by domain entity or feature area.
  List must-have capabilities per group.
-->

### {Domain / Entity Group — e.g. Task Management}

- {Requirement — e.g. Create, update, delete, list tasks}
- {Requirement}

### {Domain / Entity Group — e.g. Filtering}

- {Requirement — e.g. By status}
- {Requirement — e.g. By date range}

---

## 7. Business Rules

<!--
  Specific, testable constraints that drive domain logic and validation.
  These map directly to entity validation, service checks, and DB constraints.
  Be precise — vague rules lead to incorrect implementations.
-->

- {Entity} {property}: {Constraint — e.g. "Title is required; 1–200 characters"}
- {Entity} {property}: {Constraint — e.g. "Status must be Pending or Completed"}
- {Entity} {relationship}: {Constraint — e.g. "A task can have zero or many tags"}
- {Operation}: {Rule — e.g. "Deleted tasks cannot be recovered"}
- {Uniqueness}: {Rule — e.g. "Tag names must be unique (case-insensitive)"}

---

## 8. Non-Functional Requirements

### Performance

- {Expectation — e.g. "Listing operations should complete under 500 ms for up to 10 000 records"}

### Usability

- {Expectation — e.g. "Operations should be completable in a single API call without chained requests"}

### Reliability

- {Expectation — e.g. "No silent data loss on write operations"}

---

## 9. Assumptions

<!--
  State assumptions that, if wrong, would change the design significantly.
-->

- {Assumption — e.g. "Single-user system — no multi-tenancy in v1"}
- {Assumption — e.g. "All interaction happens via the REST API"}

---

## 10. Future Enhancements

<!--
  Out-of-scope items noted for future versions. Not commitments.
-->

- {Enhancement — e.g. "Authentication and multi-user support"}
- {Enhancement}
