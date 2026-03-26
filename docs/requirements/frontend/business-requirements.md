# Todo App — Frontend Business Requirements Document

**Product:** Todo App — Frontend Application
**Version:** v1.0 — Phase 1
**Corresponding Backend BRD:** `todo-backend-brd-phase1.md`

---

## 1. Purpose

The Todo App frontend provides a clean, responsive web interface built with Next.js that allows users to manage their tasks without friction. The application communicates exclusively with the ASP.NET Core backend API and holds no persistent data on the client side in Phase 1.

The goal is to deliver a fast, intuitive experience where users can add, complete, edit, and delete tasks — and immediately see an accurate reflection of their data.

---

## 2. Scope

### In Scope

- Task List Page — the primary view showing all tasks with filter tabs.
- Inline task creation via an input field on the list page.
- Inline task editing — edit a task title directly in the list without a separate page.
- Task completion toggle from the list.
- Task deletion from the list with a confirmation step.
- Filter tabs: All, Active, Completed — each showing its task count.
- Progress summary: completed count, total count, and completion percentage.
- Loading, error, and empty UI states for all data operations.
- Responsive layout supporting mobile (375 px) through desktop (1440 px).

### Out of Scope (v1)

- Authentication screens (login, register, password reset).
- Task detail page — all interactions occur inline on the list.
- Categories, tags, priority levels, due dates.
- Drag-and-drop reordering.
- Push notifications or reminders.
- Dark mode toggle (theme is fixed in Phase 1).
- Bulk operations (select all, bulk delete).

---

## 3. Stakeholders

- Product Owner — defines requirements and signs off on UX flows.
- Frontend Developer — implements the Next.js application.
- UX Designer — approves interaction patterns and visual states.
- QA Engineer — validates UI behaviour and state transitions.

---

## 4. Pages

### Task List — `/`

**Purpose:** The user views, creates, edits, completes, and deletes tasks — all without navigating away from this single page.

**Data displayed:**
- Application name / page title.
- Progress bar showing completed vs total tasks and completion percentage.
- Summary statistics: total task count, active count, completed count.
- Filter tabs (All / Active / Completed) each labelled with their respective count.
- Task list: one card per task showing title, creation date, completion checkbox, edit icon, and delete icon.

**User actions:**
- Type a task title in the input field and press Enter or click the Add button to create a task.
- Click the circle/checkbox on a task card to toggle its completion status.
- Click the edit (pencil) icon to enter inline edit mode; press Enter or click the Save button to confirm; press Escape or click Cancel to discard.
- Click the delete (trash) icon to remove a task — a confirmation prompt appears before deletion executes.
- Click a filter tab to switch the visible subset of tasks.
- Click "Clear completed" to bulk-delete all completed tasks (visible only when at least one completed task exists).

**UI states:**

| State | Display |
|---|---|
| Initial load | Skeleton placeholder or spinner in the list area while the first API call resolves. |
| Error | Error banner with a human-readable message and a Retry button. |
| Empty — no tasks | Illustration or icon with the message "Add your first task". |
| Empty — filtered | Contextual message such as "No active tasks" or "No completed tasks yet" — visually distinct from the no-tasks state. |
| Data | List of task cards rendered newest-first. |
| Submitting — add | Add button is disabled and shows a loading indicator; input is locked. |
| Submitting — toggle | Checkbox animates; card is briefly dimmed. Reverts to previous state on API error. |
| Submitting — delete | Card fades out optimistically; reappears on API error. |
| Edit mode | Task title is replaced by a pre-filled text input. Delete and edit icons are hidden; Save and Cancel controls appear instead. |

---

## 5. User Flows

### Flow 1: Create a Task

1. User types a task title (1–200 characters) in the input field at the top of the page.
2. User presses Enter or clicks the Add (+) button.
3. The Add button is disabled and shows a spinner; the input is locked.
4. On success: the new task card appears at the top of the list with a slide-in animation; the input clears; statistics and progress bar update immediately.
5. On error: an inline error message appears below the input; the input retains its value so the user does not lose their work.
6. Submitting an empty or whitespace-only title has no effect; the input shakes to signal the invalid state.

### Flow 2: Complete / Reactivate a Task

1. User clicks the circle/checkbox on a task card.
2. The checkbox animates to a filled checkmark; the card transitions to a muted, strikethrough visual (optimistic update).
3. The API `PATCH` call is made in the background.
4. On success: filter count badges and the progress bar update.
5. On error: the card reverts to its previous visual state; an error toast is displayed.

### Flow 3: Edit a Task Title

1. User clicks the pencil icon on a task card.
2. The title is replaced by a text input pre-filled with the current title; edit and delete icons are replaced by Save (✓) and Cancel (✕).
3. User edits the title.
4. User presses Enter or clicks Save: the API `PUT` call is made; the card returns to read mode showing the updated title.
5. User presses Escape or clicks Cancel: the edit input is dismissed with no API call; the original title is restored.
6. On API error: an error message appears beneath the input; edit mode remains open so the user can retry.

### Flow 4: Delete a Task

1. User clicks the trash icon on a task card.
2. A confirmation prompt appears inline or as a small dialog: "Delete this task?" with Confirm and Cancel options.
3. User clicks Confirm: the card fades out; the API `DELETE` call is made; the list and statistics update.
4. User clicks Cancel: the prompt closes; the card remains unchanged.
5. On API error: the card reappears; an error toast is displayed.

### Flow 5: Filter Tasks

1. User clicks a filter tab (All / Active / Completed).
2. The selected tab becomes visually highlighted.
3. The task list instantly reflects the filter — client-side filtering in Phase 1, no additional API call.
4. If the filtered result is empty, the appropriate empty state message is shown.

---

## 6. Global UI Requirements

### Navigation

- Phase 1 has a single page — no persistent navigation bar is required.
- The application title / logo is a link to `/` as a standard home affordance.

### Loading and Error States

- Every API operation must show a visual loading indicator for its duration.
- Every API error must surface a user-readable message — raw error codes or exception details must never be shown.
- Loading and empty states must be visually distinct so the user never mistakes "loading" for "no data".

### Forms and Input

- Validation errors appear inline — no full-page redirects or modal alerts for input errors.
- The Add button is disabled when the input is empty or contains only whitespace.
- The Save button in edit mode is disabled when the input is empty.
- Submit controls are disabled while a network request is in flight to prevent duplicate submissions.

### Feedback and Confirmation

- Destructive operations (delete) require an explicit confirmation step before execution.
- Successful create and delete operations produce brief visual feedback (animation, toast, or card transition).
- Failed operations revert the UI to its pre-operation state.

---

## 7. Non-Functional Requirements

### Performance

- The page must be interactive within 2 seconds on a standard broadband connection.
- Filter switching must be instantaneous (client-side, < 16 ms).
- Task card animations must run at 60 fps with no visible jank.

### Accessibility

- All interactive elements (buttons, inputs, checkboxes) must be keyboard-navigable.
- A visible focus ring must appear on all focused elements.
- Icon-only buttons must carry an `aria-label` for screen reader users.
- Colour contrast must meet WCAG AA for all text and interactive elements.

### Responsiveness

- Layouts must be functional and readable from 375 px (iPhone SE) to 1440 px (desktop).
- Touch targets must be at least 44 × 44 px on mobile viewports.

---

## 8. Assumptions

- The backend API is available and implements the contract defined in `todo-backend-brd-phase1.md`.
- Authentication is out of scope — all pages are publicly accessible in Phase 1.
- The Next.js application communicates with the ASP.NET Core API via an environment-variable-configured base URL.
- CORS is enabled on the backend for the frontend origin.
- Client-side filtering is acceptable in Phase 1 — the frontend fetches the full unfiltered task list and applies filters locally.

---

## 9. Future Enhancements

- Task detail page with extended metadata (description, due date, attachments).
- Drag-and-drop manual reordering.
- Dark mode toggle.
- Category and tag filter chips.
- Priority level badge on task cards.
- Due date display with overdue highlighting.
- AI-powered task suggestions panel (Phase 3).
- Progressive Web App (PWA) installation prompt.