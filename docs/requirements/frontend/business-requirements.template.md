# Frontend Business Requirements Document (BRD)

<!--
  TEMPLATE INSTRUCTIONS
  ─────────────────────
  How to use this template:
  1. Write a rough draft describing the pages, screens, and user interactions you want.
  2. Give your draft + this template to the AI.
  3. AI will produce a filled, structured frontend BRD following this format exactly.

  This document focuses on what the user sees and does — pages, flows, interactions,
  and display requirements. It does NOT contain TypeScript types or API details
  (those live in technical-requirements.md).

  Placeholders use {CurlyBrace} syntax.
  Comments explain what each section expects — remove them in the final doc.
-->

**Product:** {ProductName} — Frontend Application
**Version:** {Version}
**Corresponding Backend BRD:** `docs/requirements/backend/business-requirements.md`

---

## 1. Purpose

<!--
  What does the frontend application provide to the end user?
  What problem does it solve from the user's perspective?
  2–3 sentences. Keep it non-technical.
-->

{Frontend purpose and context}

---

## 2. Scope

### In Scope

<!--
  Which pages, features, and interactions are included in this version?
-->

- {Page / feature 1 — e.g. Task list page with filtering}
- {Page / feature 2 — e.g. Task detail page}
- {Add more as needed}

### Out of Scope ({Version})

<!--
  Explicitly excluded UI features.
-->

- {Excluded — e.g. Authentication screens}
- {Excluded — e.g. Mobile-specific layouts}

---

## 3. Stakeholders

- {Role 1 — e.g. Product Owner}
- {Role 2 — e.g. Frontend Developer}
- {Role 3 — e.g. UX Designer}

---

## 4. Pages

<!--
  One subsection per page. This is the core of the frontend BRD.
  
  For each page, document:
  - Route: the URL path
  - Purpose: what the user does here
  - Data displayed: what information is shown
  - User actions: what the user can do (buttons, forms, navigation)
  - UI states: how the page behaves in each state (loading, error, empty, data)
-->

### {PageName} — `{/route}`

<!--
  Example: Task List — /tasks
-->

**Purpose:** {What the user does on this page}

**Data displayed:**
- {Data item 1 — e.g. Task title}
- {Data item 2 — e.g. Task status badge}
- {Data item 3 — e.g. Due date}

**User actions:**
- {Action 1 — e.g. Click a task to navigate to the detail page}
- {Action 2 — e.g. Filter tasks by status}
- {Action 3 — e.g. Delete a task}
- {Action 4 — e.g. Open form to create a new task}

**UI states:**

| State | Display |
|---|---|
| Loading | {e.g. Spinner in place of the list} |
| Error | {e.g. Error message with retry button} |
| Empty | {e.g. "No tasks yet" message with a create button} |
| Data | {e.g. List of task cards} |

---

### {PageName} — `{/route/:id}`

**Purpose:** {What the user does on this page}

**Data displayed:**
- {Data item 1}
- {Data item 2}

**User actions:**
- {Action 1}
- {Action 2}

**UI states:**

| State | Display |
|---|---|
| Loading | {Display} |
| Error | {Display} |
| Not found | {e.g. 404 message with back navigation} |
| Data | {Display} |

---

<!--
  Add more page subsections as needed.
  Every distinct route should have its own subsection.
-->

---

## 5. User Flows

<!--
  Describe how users navigate between pages.
  Use simple numbered steps or a description.
  One subsection per key flow.
-->

### {Flow Name — e.g. Create Task Flow}

1. User is on {Page} → {Action — e.g. clicks "New Task" button}
2. {Form / modal appears / navigate to — describe what happens}
3. User fills in {fields} → submits
4. On success: {what happens — e.g. form closes, list refreshes, user navigated to detail}
5. On error: {what happens — e.g. validation errors shown inline}

### {Flow Name — e.g. Delete Task Flow}

1. {Step}
2. {Step}
3. {Outcome}

---

## 6. Global UI Requirements

<!--
  Requirements that apply across all pages rather than a single page.
-->

### Navigation

- {Requirement — e.g. Persistent navigation bar on all pages}
- {Requirement — e.g. Active route is visually highlighted}

### Loading & Error States

- {Requirement — e.g. All data-fetching operations show a loading indicator}
- {Requirement — e.g. All error states show a user-friendly message, not a raw error}
- {Requirement — e.g. Empty states are distinguishable from loading states}

### Forms

- {Requirement — e.g. Validation errors appear inline beside the relevant field}
- {Requirement — e.g. Submit button is disabled while a submission is in progress}

---

## 7. Non-Functional Requirements

### Performance

- {Expectation — e.g. "Pages should be interactive within 2 seconds on a standard connection"}

### Accessibility

- {Expectation — e.g. "All interactive elements must be keyboard-navigable"}

### Responsiveness

- {Expectation — e.g. "Layouts must work on screens from 375 px to 1440 px wide"}

---

## 8. Assumptions

- {Assumption — e.g. "Backend API is available and following the contract in technical-requirements.md"}
- {Assumption — e.g. "Authentication is out of scope — all pages are publicly accessible in v1"}

---

## 9. Future Enhancements

- {Enhancement — e.g. "Drag-and-drop task reordering"}
- {Enhancement}
