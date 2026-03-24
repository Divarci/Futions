# Todo Backend System — Technical Design Document

**Version:** 1.0 | **Date:** March 2025

---

## 1. Overview

This document defines the technical design for the Todo Backend System — an API-first task management service. It captures domain model decisions, entity behaviours, business rules, API contracts, and data layer design so developers can build against a consistent specification.

The system allows users to create, organise, and track tasks with support for tagging and due date management.

---

## 2. Domain

### 2.1 Entity Relationships

The domain revolves around two core entities:

- **Task** — the primary unit of work.
- **Tag** — a reusable label that categorises one or more Tasks.

A Task can have zero or many Tags. Tags exist independently and are referenced by Tasks.

### 2.2 Entities

#### Task

Represents a unit of work to be tracked by the user.

| Property | Type | Required | Mutable | Default | Constraints |
|---|---|---|---|---|---|
| TaskId | Guid | Yes (PK) | Immutable | Random | — |
| Title | string | Yes | Mutable | — | 1–200 chars |
| Description | string? | No | Mutable | null | 0–1000 chars |
| Status | TaskStatus | Yes | Mutable | Pending | — |
| DueDate | DateOnly? | No | Mutable | null | Optional; may be in the past |
| CreatedUtc | DateTime | Yes | Immutable | UTC now | — |
| UpdatedUtc | DateTime | Yes | Mutable | UTC now | Updated on every change |

#### Tag

A reusable label used to categorise Tasks.

| Property | Type | Required | Mutable | Default | Constraints |
|---|---|---|---|---|---|
| TagId | Guid | Yes (PK) | Immutable | Random | — |
| Name | string | Yes | Mutable | — | 1–50 chars; must be unique |
| CreatedUtc | DateTime | Yes | Immutable | UTC now | — |

#### TaskTag (Join Entity)

Represents the assignment of a Tag to a Task. Identified by the composite key (TaskId, TagId).

| Property | Type | Notes |
|---|---|---|
| TaskId | Guid | Composite PK; FK → Task |
| TagId | Guid | Composite PK; FK → Tag |
| AssignedUtc | DateTime | UTC timestamp of assignment |

---

## 3. Business Rules

| Concern | Rule | Where to Apply |
|---|---|---|
| Task title | Title is required and must be 1–200 characters. | Validation on create/update |
| Task status default | Status defaults to Pending on creation. | Service layer on create |
| Task status transitions | Pending → Completed only. Completed is terminal. | Service layer on status change |
| Task deletion | Permanently deleted (hard delete). Cannot be recovered. | DELETE removes record from DB |
| Task tags | A Task may have zero or many Tags. A Tag may be assigned to many Tasks. | No restriction on assignment count |
| Upcoming tasks filter | Only Pending tasks are included in upcoming task queries. | Filter applied in query/service layer |
| Tag name uniqueness | Tag names must be unique (case-insensitive). Duplicates are rejected with 409 Conflict. | Validate on create/update; DB unique index |
| Tag name required | Tag name cannot be empty or whitespace. | Validation on create/update |
| Tag deletion | A Tag can be deleted regardless of Task assignments. Assignments are removed (cascade delete). | Cascade handled at DB level |
| Due date | Optional. No restriction on past/future dates. For informational and filtering use only. | No enforcement beyond format validation |
| Completed tasks in upcoming | Completed tasks must not appear in upcoming task queries. | Service/query layer filter |

---

## 4. Data Models

The service uses layered models to keep separation between API input/output and domain logic.

### 4.1 Request Models

Used to receive and validate input at the API layer. Mapped to domain models before being passed inward.

#### Create Models

- `TaskCreateRequestModel` — Title, Description?, DueDate?, TagIds[]
- `TagCreateRequestModel` — Name

#### Update Models

All fields are nullable. Only non-null fields are applied to the entity (partial update semantics).

- `TaskUpdateRequestModel` — Title?, Description?, DueDate?
- `TagUpdateRequestModel` — Name?

#### Set Models

- `TaskTagSetRequestModel` — used to replace the full Tag set on a Task (list of TagIds)

### 4.2 Domain Models

Used to pass data between layers. Not exposed externally.

#### Create Models

- `TaskCreateModel`
- `TagCreateModel`

#### Update Models

- `TaskUpdateModel`
- `TagUpdateModel`

### 4.3 View Models

Control what is returned in API response bodies. A single view model is defined per entity.

- `TaskViewModel` — TaskId, Title, Description, Status, DueDate, Tags[], CreatedUtc, UpdatedUtc
- `TagViewModel` — TagId, Name, CreatedUtc, UpdatedUtc

---

## 5. Database

### 5.1 Tables

#### Task

```sql
CREATE TABLE dbo.Task
(
    TaskId          UNIQUEIDENTIFIER NOT NULL,
    Title           NVARCHAR(200)    NOT NULL,
    Description     NVARCHAR(1000)   NULL,
    Status          NVARCHAR(30)     NOT NULL,
    DueDate         DATE             NULL,
    CreatedUtc      DATETIME2        NOT NULL,
    UpdatedUtc      DATETIME2        NOT NULL,
    CONSTRAINT PK_Task PRIMARY KEY (TaskId)
);

CREATE INDEX IX_Task_Status ON dbo.Task (Status);
CREATE INDEX IX_Task_DueDate ON dbo.Task (DueDate);
CREATE INDEX IX_Task_Status_DueDate ON dbo.Task (Status, DueDate);
```

#### Tag

```sql
CREATE TABLE dbo.Tag
(
    TagId           UNIQUEIDENTIFIER NOT NULL,
    Name            NVARCHAR(50)     NOT NULL,
    CreatedUtc      DATETIME2        NOT NULL,
    UpdatedUtc      DATETIME2        NOT NULL,
    CONSTRAINT PK_Tag PRIMARY KEY (TagId)
);

CREATE UNIQUE INDEX UX_Tag_Name ON dbo.Tag (Name);
```

#### TaskTag

```sql
CREATE TABLE dbo.TaskTag
(
    TaskId          UNIQUEIDENTIFIER NOT NULL,
    TagId           UNIQUEIDENTIFIER NOT NULL,
    AssignedUtc     DATETIME2        NOT NULL,
    CONSTRAINT PK_TaskTag PRIMARY KEY (TaskId, TagId),
    CONSTRAINT FK_TaskTag_Task FOREIGN KEY (TaskId)
        REFERENCES dbo.Task (TaskId) ON DELETE CASCADE,
    CONSTRAINT FK_TaskTag_Tag FOREIGN KEY (TagId)
        REFERENCES dbo.Tag (TagId) ON DELETE CASCADE
);

CREATE INDEX IX_TaskTag_TagId ON dbo.TaskTag (TagId);
```

### 5.2 Design Notes

- All entities use UNIQUEIDENTIFIER (Guid) primary keys, generated by the application layer.
- Timestamps are managed by the application layer, not database defaults.
- TaskTag uses cascade delete on both FKs — deleting a Task or Tag removes its assignments automatically.
- Tag name uniqueness is enforced at the DB level via a unique index (case-insensitive enforcement should be applied via collation or application layer).

---

## 6. API

### 6.1 Query Parameters

All collection endpoints support the following query parameters:

| Parameter | Type | Description |
|---|---|---|
| sort | string | Sort field and direction. Format: `fieldNameAsc` or `fieldNameDesc` (e.g. `dueDateAsc`) |
| skip | int | Skip the first n results (for pagination) |
| take | int | Limit the number of results returned |
| keyword | string | General keyword search across Title and Description |
| status | string | Filter by TaskStatus (`Pending` or `Completed`) |
| tagId | Guid | Filter tasks assigned to a specific Tag |
| dueDateFrom | DateOnly | Filter tasks with a DueDate on or after this date |
| dueDateTo | DateOnly | Filter tasks with a DueDate on or before this date |

### 6.2 Response Codes

All responses are wrapped in a typed response model. Standard codes used across the API:

| Code | Meaning |
|---|---|
| 200 OK | Successful GET request |
| 201 Created | Resource created successfully. Response body contains the created resource. |
| 204 No Content | Successful update or delete. No body returned. |
| 400 Bad Request | Malformed request syntax |
| 404 Not Found | Requested resource does not exist or has been deleted |
| 409 Conflict | Uniqueness constraint violated (e.g. duplicate Tag name) |
| 422 Unprocessable Entity | Validation failure on request body (e.g. missing title) |

### 6.3 Endpoints

Endpoint tablosundaki **Business Reference** kolonu her satır için ilgili BRD Use Case ve Business Rule'larını gösterir.
Referans formatı: `UC-XX` → Use Case, `BR:` → Business Rule özeti.

#### Tasks

| Method | Route | Request Body | Response Body | Codes | Notes | Business Reference |
|---|---|---|---|---|---|---|
| GET | `/tasks` | — | `List<TaskViewModel>` | 200 | All tasks; supports filtering & pagination | UC-04 · BR: Supports filter by status, tag, due date range |
| GET | `/tasks/upcoming` | — | `List<TaskViewModel>` | 200 | Pending tasks; supports `dueDateFrom` / `dueDateTo` filter | UC-08 · BR: Only Pending tasks included; Completed tasks excluded |
| GET | `/tasks/{taskId}` | — | `TaskViewModel` | 200, 404 | — | UC-04 |
| POST | `/tasks` | `TaskCreateRequestModel` | `TaskViewModel` | 201, 422 | — | UC-01 · BR: Title required (1–200 chars); Status defaults to Pending; DueDate optional |
| PATCH | `/tasks/{taskId}` | `TaskUpdateRequestModel` | — | 204, 404, 422 | All fields nullable; only non-null fields applied | UC-02 · BR: Title required if provided; DueDate optional, may be in the past |
| PATCH | `/tasks/{taskId}/complete` | — | — | 204, 404 | Transitions status to Completed | UC-02 · BR: Pending → Completed only; Completed is terminal |
| DELETE | `/tasks/{taskId}` | — | — | 204, 404 | Permanent hard delete | UC-03 · BR: Deleted tasks cannot be recovered |
| PUT | `/tasks/{taskId}/tags` | `TaskTagSetRequestModel` | — | 204, 404, 422 | Replaces full tag set; see §6.4 for diff behaviour | UC-05 · BR: A task can have zero or many tags |

#### Tags

| Method | Route | Request Body | Response Body | Codes | Notes | Business Reference |
|---|---|---|---|---|---|---|
| GET | `/tags` | — | `List<TagViewModel>` | 200 | Supports keyword filter | UC-04 |
| GET | `/tags/{tagId}` | — | `TagViewModel` | 200, 404 | — | UC-04 · UC-06 |
| POST | `/tags` | `TagCreateRequestModel` | `TagViewModel` | 201, 409, 422 | 409 if name already exists | BR: Tag name required; Tag name must be unique (case-insensitive) |
| PATCH | `/tags/{tagId}` | `TagUpdateRequestModel` | — | 204, 404, 409, 422 | All fields nullable; 409 if name conflict | BR: Tag name required if provided; Tag name must be unique (case-insensitive) |
| DELETE | `/tags/{tagId}` | — | — | 204, 404 | Cascade removes TaskTag assignments | BR: Tag deletion removes all related task assignments |

### 6.4 PUT Behaviour — Task Tags

`PUT /tasks/{taskId}/tags` replaces the complete tag set for a task. The implementation uses an intelligent diff approach:

- Tags present in the incoming list but not currently assigned are **added**.
- Tags currently assigned but absent from the incoming list are **removed**.
- Tags present in both lists are **left unchanged**.

Sending an empty array removes all tags from the task.

---

## 7. Enums

| Enum | Values |
|---|---|
| TaskStatus | `Pending`, `Completed` |