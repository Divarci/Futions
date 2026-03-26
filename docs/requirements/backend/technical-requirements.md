# Todo App — Backend Technical Design Document

**Version:** v2.0 | **Date:** 2026-03-26

---

## 1. Overview

This document defines the technical design for the Todo App backend — a RESTful API built with ASP.NET Core that allows authenticated users to create, manage, and track their personal tasks within a multi-tenant environment. It covers the domain entity, authentication and authorisation strategy, business rule enforcement locations, the relational database schema, layered data models, and the full API contract. This document is derived from the Backend Business Requirements Document (BRD v2.0) and serves as the single source of truth for backend implementation and AI coding agents.

---

## 2. Domain

### 2.1 Entity Relationships

- **Task** — the primary and only business domain entity in Phase 2. Represents a single unit of work the user wants to track. Implements `IHaveTenant` to enforce tenant isolation at the repository level.

There are no relationships between business entities in Phase 2. Tasks are independent records with no parent/child structure, no tags, and no categories.

### 2.2 Entities

#### Task

| Property | Type | Required | Mutable | Default | Constraints |
|---|---|---|---|---|—|
| TaskId | Guid | Yes (PK) | Immutable | Random | — |
| TenantId | Guid | Yes (FK) | Immutable | From JWT claim | Enforced by `IHaveTenant`; set by service layer on create |
| UserId | Guid | Yes | Immutable | From JWT claim | Set by service layer on create; cannot be changed |
| Title | string | Yes | Mutable | — | 1–200 characters after trim |
| IsCompleted | bool | Yes | Mutable | false | — |
| CreatedUtc | DateTime | Yes | Immutable | UTC now | — |
| UpdatedUtc | DateTime | Yes | Mutable | UTC now | Updated on every change |

---

## 3. Business Rules

| Concern | Rule | Where to Apply |
|---|---|---|
| Task `Title` | Required; 1–200 characters after whitespace trim | Validation on create/update |
| Task `Title` | Leading and trailing whitespace trimmed before persistence | Service layer on create/update |
| Task `IsCompleted` | Defaults to `false` on creation | Service layer on create |
| Task `IsCompleted` toggle | Flips the current boolean value; no terminal state | Service layer on toggle |
| Task `TaskId` | System-generated; immutable after creation | Set by service layer on create |
| Task `TenantId` | Extracted from caller's JWT `tenantId` claim; immutable after creation | Service layer on create; `IHaveTenant` enforces query scoping |
| Task `UserId` | Extracted from caller's JWT `userId` claim; immutable after creation | Service layer on create; query filter on every read/write |
| Task `CreatedUtc` | Set by server on creation; immutable | Set by service layer on create |
| Task `UpdatedUtc` | Updated on every mutation (title update or toggle) | Service layer on update/toggle |
| Tenant isolation | Route `tenantId` must match JWT `tenantId` claim; mismatch returns `404` | `TenantAuthorizationAttribute` at API layer |
| User isolation | All queries are scoped by `UserId` from JWT; users cannot access other users' tasks | Service/query layer on every read/write |
| `SystemAdmin` bypass | `SystemAdmin` role skips tenant check; can access any tenant's data | `TenantAuthorizationAttribute` at API layer |
| Authentication | All endpoints require a valid JWT Bearer token | `[Authorize]` attribute at controller level |
| Authorisation | All authenticated roles (`User`, `Admin`, `SystemAdmin`) may perform full CRUD on their own tasks | `[Authorize(Policy = PolicyNames.AllRoles)]` on all endpoints |
| Task deletion | Permanent hard delete; no recovery mechanism | DELETE removes record from DB |
| List filter | Accepted values: `all`, `active`, `completed`; default: `all` | Filter applied in query/service layer |
| List sort order | Tasks returned newest-first by `CreatedUtc` | Order applied in query/service layer |

---

## 4. Data Models

### 4.1 Request Models

#### Create Models

- `TaskCreateRequestModel` — `Title`
  - `TenantId` and `UserId` are **not** part of the request body — they are extracted from the JWT via `HttpContext` in the controller and passed into the mapper.

#### Update Models

- `TaskUpdateRequestModel` — `Title?` (nullable; only non-null fields are applied)

### 4.2 Domain Models

#### Create Models

- `TaskCreateModel` — `Title`, `TenantId`, `UserId`

#### Update Models

- `TaskUpdateModel` — `Title?`

### 4.3 View Models

- `TaskViewModel` — `TaskId`, `Title`, `IsCompleted`, `CreatedUtc`, `UpdatedUtc`
  - `TenantId` and `UserId` are intentionally excluded from the view model (caller already knows their identity).

---

## 5. Database

### 5.1 Tables

#### Task

```sql
CREATE TABLE dbo.Task
(
    TaskId          UNIQUEIDENTIFIER NOT NULL,
    TenantId        UNIQUEIDENTIFIER NOT NULL,
    UserId          UNIQUEIDENTIFIER NOT NULL,
    Title           NVARCHAR(200)    NOT NULL,
    IsCompleted     BIT              NOT NULL,
    CreatedUtc      DATETIME2        NOT NULL,
    UpdatedUtc      DATETIME2        NOT NULL,
    CONSTRAINT PK_Task PRIMARY KEY (TaskId)
);

-- Tenant isolation: all queries are scoped by TenantId
CREATE INDEX IX_Task_TenantId ON dbo.Task (TenantId);

-- User isolation: compound index covering the primary access pattern
CREATE INDEX IX_Task_TenantId_UserId ON dbo.Task (TenantId, UserId);

-- Filter by completion status within tenant+user scope
CREATE INDEX IX_Task_TenantId_UserId_IsCompleted ON dbo.Task (TenantId, UserId, IsCompleted);

-- Sort by creation date (newest first) within tenant+user scope
CREATE INDEX IX_Task_TenantId_UserId_CreatedUtc ON dbo.Task (TenantId, UserId, CreatedUtc);
```

### 5.2 Design Notes

- All entities use UNIQUEIDENTIFIER (Guid) primary keys, generated by the application layer.
- Timestamps are managed by the application layer — no database defaults or triggers.
- `TenantId` is a non-nullable FK-style column. No foreign key constraint is added — the tenant record lives in the identity system (Keycloak), not in this database.
- `UserId` is a non-nullable column referencing the authenticated user's identity (sourced from the JWT claim). No FK constraint is added for the same reason.
- `IsCompleted` is stored as a BIT column; status filtering (`active` / `completed`) is derived from this value at the query/service layer.
- Composite indexes are used for the primary access pattern `(TenantId, UserId)` to ensure all queries are efficiently tenant- and user-scoped.
- `Task` implements `IHaveTenant`, which causes the repository infrastructure to automatically apply a `TenantId` filter on all queries. The `UserId` filter is applied explicitly in the service/query layer.

---

## 6. API

### 6.1 Query Parameters

Collection endpoint: `GET /api/v1/tenants/{tenantId}/tasks`

| Parameter | Type | Description |
|---|---|---|
| status | string | Filter by completion status. Accepted values: `all` (default), `active`, `completed` |

### 6.2 Response Codes

| Code | Meaning |
|---|---|
| 200 OK | Successful GET request |
| 201 Created | Resource created. Response body contains the created resource. |
| 204 No Content | Successful update, toggle, or delete. No body returned. |
| 400 Bad Request | Malformed request syntax |
| 401 Unauthorized | Missing or invalid JWT token |
| 403 Forbidden | Valid token but insufficient role for the requested policy |
| 404 Not Found | Resource does not exist, or route `tenantId` does not match JWT `tenantId` claim |
| 422 Unprocessable Entity | Validation failure on request body |
| 500 Internal Server Error | Unhandled exception. Stack trace must never be exposed to the client. |

### 6.3 Endpoints

**Base route:** `api/v1/tenants/{tenantId:guid}/tasks`  
**Controller-level auth:** `[Authorize(Policy = PolicyNames.AllRoles)]` + `[TenantAuthorization]`  
**TenantAuthorization behaviour:** verifies JWT `tenantId` == route `tenantId`; `SystemAdmin` bypasses this check; mismatch returns `404`.

#### Task

| Method | Route | Request Body | Response Body | Codes | Auth Policy | Notes | Business Reference |
|---|---|---|---|---|---|---|---|
| GET | `/api/v1/tenants/{tenantId}/tasks` | — | `List<TaskViewModel>` | 200, 401, 403 | `AllRoles` | Filter by `status`; scoped to caller's `TenantId`+`UserId`; sorted newest-first; returns empty array (not 404) when no match | UC-02 |
| GET | `/api/v1/tenants/{tenantId}/tasks/{taskId}` | — | `TaskViewModel` | 200, 401, 403, 404 | `AllRoles` | Returns 404 if task does not exist or does not belong to caller | UC-03 |
| POST | `/api/v1/tenants/{tenantId}/tasks` | `TaskCreateRequestModel` | `TaskViewModel` | 201, 401, 403, 422 | `AllRoles` | `TenantId`+`UserId` sourced from JWT; `IsCompleted` defaults to `false`; title trimmed | UC-01 · BR: title 1–200 chars after trim |
| PATCH | `/api/v1/tenants/{tenantId}/tasks/{taskId}` | `TaskUpdateRequestModel` | — | 204, 401, 403, 404, 422 | `AllRoles` | Only `title` is updatable; `UpdatedUtc` refreshed; 404 if not owned by caller | UC-04 · BR: title 1–200 chars after trim |
| PATCH | `/api/v1/tenants/{tenantId}/tasks/{taskId}/toggle` | — | — | 204, 401, 403, 404 | `AllRoles` | Flips `IsCompleted` boolean; `UpdatedUtc` refreshed; 404 if not owned by caller | UC-05 |
| DELETE | `/api/v1/tenants/{tenantId}/tasks/{taskId}` | — | — | 204, 401, 403, 404 | `AllRoles` | Hard delete; no recovery; 404 if not owned by caller | UC-06 · BR: permanent deletion |

---

## 7. Enums

| Enum | Values |
|---|---|
| `TaskStatusFilter` | `All`, `Active`, `Completed` |
