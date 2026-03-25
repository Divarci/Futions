# {ProjectName} — Backend Technical Design Document

<!--
  TEMPLATE INSTRUCTIONS
  ─────────────────────
  How to use this template:
  1. Write a rough draft or give the AI the filled Business Requirements Document.
  2. Give your draft + this template to the AI.
  3. AI will produce a filled, structured technical spec following this format exactly.

  This document is the single source of truth for backend developers and AI coding agents.
  It defines domain entities, business rules (with enforcement location), data models,
  database schema, and API contracts.

  Placeholders use {CurlyBrace} syntax.
  Comments explain what each section expects — remove them in the final doc.
-->

**Version:** {Version} | **Date:** {Date}

---

## 1. Overview

<!--
  One short paragraph: what system this is, what it does, what this document covers.
  Reference the BRD briefly.
-->

{Overview paragraph}

---

## 2. Domain

### 2.1 Entity Relationships

<!--
  Describe how entities relate to each other in plain language.
  Mention cardinalities (one-to-many, many-to-many).
  Example:
    - Task — the primary unit of work.
    - Tag  — a reusable label that categorises Tasks.
    A Task can have zero or many Tags. Tags exist independently.
-->

- **{Entity1}** — {primary role}
- **{Entity2}** — {primary role}

{Relationship description — e.g. "A {Entity1} can have zero or many {Entity2}s."}

### 2.2 Entities

<!--
  One table per entity.
  Columns: Property | Type | Required | Mutable | Default | Constraints
  Rules:
  - PKs are always Guid, Immutable, Default = Random.
  - CreatedUtc is always Immutable, Default = UTC now.
  - UpdatedUtc is always Mutable, "Updated on every change".
  - Use string? for nullable strings, DateOnly? for nullable dates.
-->

#### {Entity1}

| Property | Type | Required | Mutable | Default | Constraints |
|---|---|---|---|---|---|
| {Entity1Id} | Guid | Yes (PK) | Immutable | Random | — |
| {Property1} | string | Yes | Mutable | — | {e.g. 1–200 chars} |
| {Property2} | string? | No | Mutable | null | {e.g. 0–1000 chars} |
| {StatusProp} | {EnumType} | Yes | Mutable | {DefaultValue} | — |
| {DateProp} | DateOnly? | No | Mutable | null | {e.g. Optional; may be in the past} |
| CreatedUtc | DateTime | Yes | Immutable | UTC now | — |
| UpdatedUtc | DateTime | Yes | Mutable | UTC now | Updated on every change |

#### {Entity2}

| Property | Type | Required | Mutable | Default | Constraints |
|---|---|---|---|---|---|
| {Entity2Id} | Guid | Yes (PK) | Immutable | Random | — |
| {Property1} | string | Yes | Mutable | — | {e.g. 1–50 chars; must be unique} |
| CreatedUtc | DateTime | Yes | Immutable | UTC now | — |
| UpdatedUtc | DateTime | Yes | Mutable | UTC now | Updated on every change |

#### {JoinEntity} (Join Entity)

<!--
  Include only when there is a meaningful many-to-many relationship.
  Identified by composite key. May carry extra properties (e.g. AssignedUtc).
  Remove this section if no join entity exists.
-->

| Property | Type | Notes |
|---|---|---|
| {Entity1Id} | Guid | Composite PK; FK → {Entity1} |
| {Entity2Id} | Guid | Composite PK; FK → {Entity2} |
| {ExtraProperty} | DateTime | {e.g. UTC timestamp of assignment} |

---

## 3. Business Rules

<!--
  Extract every rule from the BRD and translate it to a technical constraint.
  The "Where to Apply" column tells developers exactly where to enforce the rule.
  Common locations:
    - "Validation on create/update"
    - "Service layer on create"
    - "Service layer on status change"
    - "DELETE removes record from DB"
    - "Filter applied in query/service layer"
    - "Validate on create/update; DB unique index"
    - "Cascade handled at DB level"
-->

| Concern | Rule | Where to Apply |
|---|---|---|
| {Entity1} {property} | {Constraint — e.g. Required; 1–200 characters} | {Validation on create/update} |
| {Entity1} status default | {Rule — e.g. Status defaults to Pending on creation} | {Service layer on create} |
| {Entity1} status transitions | {Rule — e.g. Pending → Completed only; Completed is terminal} | {Service layer on status change} |
| {Entity1} deletion | {Rule — e.g. Permanent hard delete; cannot be recovered} | {DELETE removes record from DB} |
| {Entity1} {relationship} | {Constraint on relationship cardinality} | {No restriction / cascade at DB level} |
| {Entity2} name uniqueness | {Rule — e.g. Must be unique (case-insensitive); duplicates → 409} | {Validate on create/update; DB unique index} |
| {Filter rule} | {Rule — e.g. Only Pending tasks in upcoming queries} | {Filter applied in query/service layer} |

---

## 4. Data Models

<!--
  Layered model design: RequestModels (API input) → DomainModels (internal) → ViewModels (API output).
  Request and domain models are parallel. ViewModels control what the API returns.
-->

### 4.1 Request Models

<!--
  Received and validated at the API layer. Mapped to domain models before passing inward.
  Use ? to denote optional fields.
-->

#### Create Models

- `{Entity1}CreateRequestModel` — {Field1}, {Field2?}, {Field3?}, {TagIds[]?}
- `{Entity2}CreateRequestModel` — {Field1}

#### Update Models

<!--
  All fields nullable. Partial update semantics — only non-null fields are applied.
-->

- `{Entity1}UpdateRequestModel` — {Field1?}, {Field2?}
- `{Entity2}UpdateRequestModel` — {Field1?}

#### Set Models

<!--
  Include only when a "replace full collection" operation exists (e.g. replacing all tags on a task).
  Remove this section if not applicable.
-->

- `{Entity1}{RelatedEntity}SetRequestModel` — used to replace the full {RelatedEntity} set on {Entity1} (list of {RelatedEntityId}s)

### 4.2 Domain Models

<!--
  Passed between layers. Not exposed externally. Mirror the request models.
-->

#### Create Models

- `{Entity1}CreateModel`
- `{Entity2}CreateModel`

#### Update Models

- `{Entity1}UpdateModel`
- `{Entity2}UpdateModel`

### 4.3 View Models

<!--
  Returned in API response bodies. One per entity.
  List all fields the consumer receives.
-->

- `{Entity1}ViewModel` — {Entity1Id}, {Field1}, {Field2}, {Status}, {DateField?}, {RelatedEntities[]}, CreatedUtc, UpdatedUtc
- `{Entity2}ViewModel` — {Entity2Id}, {Field1}, CreatedUtc, UpdatedUtc

---

## 5. Database

### 5.1 Tables

<!--
  One CREATE TABLE block per entity.
  Naming conventions:
    Tables:        dbo.{EntityName}
    PK constraint: PK_{EntityName}
    FK constraint: FK_{JoinTable}_{ReferencedTable}
    Index:         IX_{Table}_{Column}
    Unique index:  UX_{Table}_{Column}

  Rules:
  - PKs: UNIQUEIDENTIFIER NOT NULL
  - String with max length: NVARCHAR({n})
  - Enum stored as: NVARCHAR(30) NOT NULL
  - Optional date: DATE NULL
  - Timestamps: DATETIME2 NOT NULL (managed by application)
-->

#### {Entity1}

```sql
CREATE TABLE dbo.{Entity1}
(
    {Entity1Id}     UNIQUEIDENTIFIER NOT NULL,
    {Column1}       NVARCHAR({n})    NOT NULL,
    {Column2}       NVARCHAR({n})    NULL,
    {StatusColumn}  NVARCHAR(30)     NOT NULL,
    {DateColumn}    DATE             NULL,
    CreatedUtc      DATETIME2        NOT NULL,
    UpdatedUtc      DATETIME2        NOT NULL,
    CONSTRAINT PK_{Entity1} PRIMARY KEY ({Entity1Id})
);

-- Add indexes for every column used in WHERE clauses or ORDER BY
CREATE INDEX IX_{Entity1}_{Column} ON dbo.{Entity1} ({Column});
CREATE INDEX IX_{Entity1}_{Col1}_{Col2} ON dbo.{Entity1} ({Col1}, {Col2});
```

#### {Entity2}

```sql
CREATE TABLE dbo.{Entity2}
(
    {Entity2Id}     UNIQUEIDENTIFIER NOT NULL,
    {Column1}       NVARCHAR({n})    NOT NULL,
    CreatedUtc      DATETIME2        NOT NULL,
    UpdatedUtc      DATETIME2        NOT NULL,
    CONSTRAINT PK_{Entity2} PRIMARY KEY ({Entity2Id})
);

CREATE UNIQUE INDEX UX_{Entity2}_{Column} ON dbo.{Entity2} ({Column});
```

#### {JoinEntity} (if applicable)

```sql
CREATE TABLE dbo.{JoinEntity}
(
    {Entity1Id}     UNIQUEIDENTIFIER NOT NULL,
    {Entity2Id}     UNIQUEIDENTIFIER NOT NULL,
    AssignedUtc     DATETIME2        NOT NULL,
    CONSTRAINT PK_{JoinEntity} PRIMARY KEY ({Entity1Id}, {Entity2Id}),
    CONSTRAINT FK_{JoinEntity}_{Entity1} FOREIGN KEY ({Entity1Id})
        REFERENCES dbo.{Entity1} ({Entity1Id}) ON DELETE CASCADE,
    CONSTRAINT FK_{JoinEntity}_{Entity2} FOREIGN KEY ({Entity2Id})
        REFERENCES dbo.{Entity2} ({Entity2Id}) ON DELETE CASCADE
);

CREATE INDEX IX_{JoinEntity}_{Entity2Id} ON dbo.{JoinEntity} ({Entity2Id});
```

### 5.2 Design Notes

<!--
  Cross-cutting database design decisions. Keep to facts that affect implementation choices.
-->

- All entities use UNIQUEIDENTIFIER (Guid) primary keys, generated by the application layer.
- Timestamps are managed by the application layer — no database defaults.
- {Cascade note — e.g. "JoinEntity uses cascade delete on both FKs — deleting either parent removes its assignments."}
- {Uniqueness note — e.g. "Column uniqueness enforced at DB level via unique index; application layer enforces case-insensitive check."}

---

## 6. API

### 6.1 Query Parameters

<!--
  List all supported query parameters for collection endpoints.
  Include all filter, sort, and pagination parameters.
-->

| Parameter | Type | Description |
|---|---|---|
| sort | string | Sort field and direction. Format: `fieldNameAsc` or `fieldNameDesc` |
| skip | int | Pagination offset — skip the first n results |
| take | int | Pagination limit — maximum results returned |
| keyword | string | Keyword search across {field1} and {field2} |
| {filter1} | {Type} | {Description — e.g. Filter by status value} |
| {filter2} | {Type} | {Description — e.g. Filter by related entity ID} |
| {dateFrom} | DateOnly | Filter records with {dateField} on or after this date |
| {dateTo} | DateOnly | Filter records with {dateField} on or before this date |

### 6.2 Response Codes

| Code | Meaning |
|---|---|
| 200 OK | Successful GET request |
| 201 Created | Resource created. Response body contains the created resource. |
| 204 No Content | Successful update or delete. No body returned. |
| 400 Bad Request | Malformed request syntax |
| 404 Not Found | Resource does not exist |
| 409 Conflict | Uniqueness constraint violated |
| 422 Unprocessable Entity | Validation failure on request body |

### 6.3 Endpoints

<!--
  Business Reference column format:
    UC-XX  → references a Use Case from the BRD
    BR:    → summarises the enforced Business Rule

  One table per entity group.
  Include all CRUD operations plus any domain-specific operations (e.g. /complete, /tags).
-->

#### {Entity1}

| Method | Route | Request Body | Response Body | Codes | Notes | Business Reference |
|---|---|---|---|---|---|---|
| GET | `/{entity1s}` | — | `List<{Entity1}ViewModel>` | 200 | Supports filtering & pagination | {UC-XX} |
| GET | `/{entity1s}/{entity1Id}` | — | `{Entity1}ViewModel` | 200, 404 | — | {UC-XX} |
| POST | `/{entity1s}` | `{Entity1}CreateRequestModel` | `{Entity1}ViewModel` | 201, 422 | — | {UC-XX} · BR: {rule summary} |
| PATCH | `/{entity1s}/{entity1Id}` | `{Entity1}UpdateRequestModel` | — | 204, 404, 422 | All fields nullable; only non-null applied | {UC-XX} |
| DELETE | `/{entity1s}/{entity1Id}` | — | — | 204, 404 | {Hard/Soft} delete | {UC-XX} · BR: {rule summary} |
| PATCH | `/{entity1s}/{entity1Id}/{action}` | — | — | 204, 404 | {Domain-specific action — e.g. complete} | {UC-XX} · BR: {rule summary} |
| PUT | `/{entity1s}/{entity1Id}/{related}` | `{Entity1}{Related}SetRequestModel` | — | 204, 404, 422 | Replaces full {related} set | {UC-XX} |

#### {Entity2}

| Method | Route | Request Body | Response Body | Codes | Notes | Business Reference |
|---|---|---|---|---|---|---|
| GET | `/{entity2s}` | — | `List<{Entity2}ViewModel>` | 200 | — | {UC-XX} |
| GET | `/{entity2s}/{entity2Id}` | — | `{Entity2}ViewModel` | 200, 404 | — | {UC-XX} |
| POST | `/{entity2s}` | `{Entity2}CreateRequestModel` | `{Entity2}ViewModel` | 201, 409, 422 | 409 if uniqueness violated | BR: {rule summary} |
| PATCH | `/{entity2s}/{entity2Id}` | `{Entity2}UpdateRequestModel` | — | 204, 404, 409, 422 | — | BR: {rule summary} |
| DELETE | `/{entity2s}/{entity2Id}` | — | — | 204, 404 | Cascade removes related records | BR: {rule summary} |

### 6.4 Special Endpoint Behaviours

<!--
  Document non-standard behaviours for any endpoint.
  Example: PUT /tasks/{taskId}/tags uses intelligent diff (add new, remove absent, keep existing).
  Remove this section if all endpoints follow standard CRUD semantics.
-->

#### {Endpoint — e.g. PUT /{entity1s}/{entity1Id}/{related}}

{Description of the special behaviour.}

---

## 7. Enums

<!--
  List every enum used in the domain. Values must match exactly what is stored in the database.
-->

| Enum | Values |
|---|---|
| {EnumName} | `{Value1}`, `{Value2}` |
