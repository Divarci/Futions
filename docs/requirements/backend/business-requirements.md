# Todo App — Backend Business Requirements Document

**Product:** Todo App — RESTful API for task creation, management, and tracking
**Version:** v2.0 — Phase 2

---

## 1. Purpose

The Todo App backend provides a RESTful API that enables users to create, manage, and track their personal tasks. The system exposes structured endpoints consumed by the Next.js frontend, persisting all data in a relational database.

The backend exists to centralise task-management logic, enforce business rules, and serve as the single source of truth for task state — freeing the frontend from any persistence or validation responsibilities.

**Objectives:**
- Provide a complete CRUD API for tasks with robust input validation.
- Support status-based filtering and creation-date sorting on task listings.
- Enforce all domain business rules at the service and data layer.
- Deliver predictable, well-structured JSON responses for all operations.
- Authenticate all API consumers via Keycloak-issued JWT tokens.
- Isolate task data per tenant and per user — no cross-tenant or cross-user data leakage.

---

## 2. Scope

### In Scope

- Task entity: create, read, update, delete.
- Task listing with filtering by status (`active` / `completed`) and sorting by creation date.
- Completion toggle — marking a task done or reverting it to active.
- Input validation with structured error responses.
- Persistent storage via a relational database (SQL Server).
- RESTful JSON API built with ASP.NET Core.
- Authentication via Keycloak-issued JWT Bearer tokens.
- Multi-tenancy — tasks are isolated per tenant via `TenantId`.
- User-level isolation — each user sees and manages only their own tasks within a tenant.

### Out of Scope (v2)

- Categories, tags, and labels.
- Priority levels.
- Due dates and reminders.
- File attachments.
- Real-time notifications (WebSockets / SignalR).
- Soft-delete / task recovery.

---

## 3. Stakeholders

- Product Owner — defines requirements and acceptance criteria.
- Backend Developer — implements the ASP.NET Core API.
- Frontend Developer — consumes the API from the Next.js application.
- QA Engineer — writes and executes API-level tests.

---

## 4. Key Concepts

### Task

A Task is the central domain entity. It represents a single unit of work the user wants to track. Each task has a title, a completion status, and audit timestamps. Tasks are independent — they have no parent/child relationships in Phase 2. Every task belongs to exactly one tenant and exactly one user.

### Status

Status is a derived view over the Task entity representing whether a task is active (not yet completed) or completed. It is not stored as an independent column; it is derived from the `isCompleted` boolean field.

### Tenant

A Tenant is an isolated organisational unit. All tasks created within a tenant are invisible to users in other tenants. The tenant identity is carried in the caller's JWT token as the `tenantId` claim and must match the `tenantId` route parameter on every API call.

### User

A User is an individual authenticated caller identified by the `userId` claim in their JWT token. Within a tenant, tasks are further scoped per user — a user can only read and mutate their own tasks. There is no concept of shared tasks between users of the same tenant in Phase 2.

### Authentication

All API endpoints require a valid Keycloak-issued JWT Bearer token. Unauthenticated requests are rejected with `401 Unauthorized`. The token carries the caller's `tenantId`, `userId`, and role claims, which are used by the API to enforce tenant isolation and authorisation.

### Authorisation

Three roles are recognised: `User`, `Admin`, and `SystemAdmin`. All authenticated roles may perform full CRUD on their own tasks. `SystemAdmin` is a privileged role that can bypass tenant isolation — it is used for operational purposes only and is not a consumer-facing role.

---

## 5. Use Cases

### UC-01: Create Task

An authenticated user submits a task title. The system persists a new Task record scoped to the caller's tenant and user, with `isCompleted = false`, and returns the created resource with a generated ID and timestamps.

- Caller must be authenticated; unauthenticated requests return `401`.
- The `tenantId` in the route must match the `tenantId` claim in the JWT; mismatch returns `404`.
- The task is associated with the `userId` extracted from the JWT token.
- Default status: active (`isCompleted = false`).
- Title is required; must be between 1 and 200 characters.
- Leading and trailing whitespace is trimmed before persistence.

### UC-02: List Tasks

An authenticated user requests a list of their tasks. The system returns only tasks belonging to the caller's tenant and user, ordered by creation date (newest first). An optional filter parameter restricts results to active or completed tasks.

- Caller must be authenticated; unauthenticated requests return `401`.
- The `tenantId` in the route must match the `tenantId` claim in the JWT; mismatch returns `404`.
- Only tasks owned by the calling user (`userId`) within the tenant are returned.
- Supported filter values: `all` (default), `active`, `completed`.
- Sort order: `createdAt` descending.
- Returns an empty array — not a 404 — when no tasks match the filter.

### UC-03: Get Task by ID

An authenticated user requests a single task by its unique identifier. The system returns the full task resource if the task exists and belongs to the caller's tenant and user. Returns `404 Not Found` if the task does not exist or belongs to a different user or tenant.

- Caller must be authenticated; unauthenticated requests return `401`.
- The `tenantId` in the route must match the `tenantId` claim in the JWT; mismatch returns `404`.
- Tasks belonging to a different user within the same tenant also return `404` (no cross-user visibility).

### UC-04: Update Task

An authenticated user submits a new title for one of their existing tasks. The system validates and persists the change, updating the `updatedAt` timestamp.

- Caller must be authenticated; unauthenticated requests return `401`.
- The `tenantId` in the route must match the `tenantId` claim in the JWT; mismatch returns `404`.
- Only tasks owned by the calling user may be updated; tasks belonging to another user return `404`.
- Only the `title` field is updatable in Phase 2.
- The same validation rules as UC-01 apply.
- Returns `404` if the task ID does not exist or is not owned by the caller.

### UC-05: Toggle Completion

An authenticated user marks one of their active tasks as completed, or reverts a completed task to active. The system flips the `isCompleted` boolean and updates the `updatedAt` timestamp.

- Caller must be authenticated; unauthenticated requests return `401`.
- The `tenantId` in the route must match the `tenantId` claim in the JWT; mismatch returns `404`.
- Only tasks owned by the calling user may be toggled; tasks belonging to another user return `404`.
- This is a dedicated `PATCH` endpoint — not part of the general update.
- Returns `404` if the task ID does not exist or is not owned by the caller.

### UC-06: Delete Task

An authenticated user permanently removes one of their tasks by its ID. The operation is irreversible. The system returns `204 No Content` on success, or `404` if the task does not exist or does not belong to the caller.

- Caller must be authenticated; unauthenticated requests return `401`.
- The `tenantId` in the route must match the `tenantId` claim in the JWT; mismatch returns `404`.
- Only tasks owned by the calling user may be deleted; tasks belonging to another user return `404`.
- Deletion is permanent — no soft-delete or recovery in Phase 2.

---

## 6. Functional Requirements

### Authentication

- All endpoints must require a valid Keycloak-issued JWT Bearer token.
- Requests without a token or with an invalid token must return `401 Unauthorized`.
- The JWT must carry `tenantId`, `userId`, and `role` claims.

### Authorisation

- Every request to a tenant-scoped route must have its route `tenantId` verified against the `tenantId` JWT claim.
- If the two values do not match, the system must return `404 Not Found` (to avoid disclosing tenant existence to unauthorised callers).
- `SystemAdmin` role bypasses tenant isolation checks.
- All authenticated roles (`User`, `Admin`, `SystemAdmin`) may perform full CRUD on their own tasks.

### Task Management

- The system must support creating a task scoped to the caller's tenant and user.
- The system must support retrieving a single task by ID, visible only if it belongs to the caller.
- The system must support listing the caller's tasks within their tenant, with an optional status filter.
- The system must support updating the title of an existing task owned by the caller.
- The system must support toggling the completion status of a task owned by the caller.
- The system must support permanently deleting a task owned by the caller.

### Validation

- All write operations must validate input and return structured error details on failure.
- Title must be present and non-empty after trimming whitespace.
- Title must not exceed 200 characters.

### Error Handling

- Unauthenticated requests must return `401 Unauthorized`.
- Route `tenantId` mismatch with JWT claim must return `404 Not Found`.
- Operations on a non-existent task ID must return `404` with a descriptive message.
- Validation failures must return `422` with field-level error details.
- Unhandled exceptions must return `500` without leaking stack traces to the client.

---

## 7. Business Rules

| Entity / Operation | Property | Rule |
|---|---|---|
| Task | `title` | Required; 1–200 characters after trim |
| Task | `isCompleted` | Boolean; defaults to `false` on creation |
| Task | `id` | System-generated; immutable after creation |
| Task | `tenantId` | Extracted from JWT claim on creation; immutable; tasks never change tenant |
| Task | `userId` | Extracted from JWT claim on creation; immutable; tasks never change owner |
| Task | `createdAt` | Set by server on creation; immutable |
| Task | `updatedAt` | Set by server on every update or toggle |
| Create | status | New tasks always start as active (`isCompleted = false`) |
| Create | ownership | Task is automatically bound to the caller's `tenantId` and `userId` from JWT |
| Read / Write | access | A user may only access tasks where both `tenantId` and `userId` match their JWT claims |
| Tenant check | route param | Route `tenantId` must match JWT `tenantId`; mismatch returns `404` |
| SystemAdmin | bypass | `SystemAdmin` role bypasses tenant isolation check at the API layer |
| Update | `title` | Trimmed before save; same length constraints apply |
| Delete | permanence | Deletion is permanent; no recovery mechanism |
| Filter | filter param | Accepted values: `all`, `active`, `completed` — default: `all` |
| List | scope | List returns only tasks owned by the calling user within their tenant |
| List | sort order | Tasks returned newest-first by `createdAt` |

---

## 8. Non-Functional Requirements

### Performance

- All list and single-record read operations must complete in under 300 ms for datasets up to 1 000 tasks.
- Write operations (create, update, delete) must complete in under 500 ms.

### Reliability

- No silent data loss on write operations — every write must be confirmed or return an error.
- Database transactions must be used for any multi-step write operation.

### Maintainability

- The API must follow RESTful conventions consistently across all endpoints.
- Response shapes must be consistent — all success responses share a common envelope structure.

### Security

- All API endpoints must require JWT Bearer authentication issued by Keycloak.
- Tenant isolation must be enforced on every request — no cross-tenant data access is permitted.
- User-level isolation must be enforced — users can only access their own tasks.
- Stack traces and internal error details must never be returned to clients.
- JWT validation settings (issuer, audience, signing key) must be injected via environment configuration — never hardcoded.

---

## 9. Assumptions

- Multi-tenant system — every task belongs to exactly one tenant and one user.
- Keycloak is the sole identity provider; the backend does not issue or manage tokens directly.
- JWT tokens carry `tenantId`, `userId`, and `role` claims populated by Keycloak at login.
- All interaction occurs via the REST API; no server-rendered pages.
- The database schema is managed via EF Core migrations.
- The frontend is the sole consumer of this API in Phase 2.
- CORS is configured to allow requests from the Next.js development and production origins.

---

## 10. Future Enhancements

- Categories and tag system.
- Priority levels (low, medium, high, urgent).
- Due dates with optional reminders.
- Soft-delete with a 30-day recovery window.
- Real-time updates via SignalR.
- AI-powered task suggestions via Anthropic API (Phase 3).