# {ProductName} — Frontend Technical Design Document

<!--
  TEMPLATE INSTRUCTIONS
  ─────────────────────
  How to use this template:
  1. Write a rough draft or give the AI the filled Frontend BRD + Backend Technical Requirements.
  2. Give your draft + this template to the AI.
  3. AI will produce a filled, structured frontend technical spec following this format exactly.

  This document is the single source of truth for frontend developers and AI coding agents.
  It defines:
    - TypeScript types that mirror the backend API contracts
    - API call function signatures per domain
    - SWR hook signatures per domain
    - Page-to-feature mapping
    - Form validation schemas

  The coding agent reads this document to know exactly what types, functions, hooks,
  and components to generate — without guessing or improvising.

  Placeholders use {CurlyBrace} syntax.
  Comments explain what each section expects — remove them in the final doc.
-->

**Version:** {Version} | **Date:** {Date}
**Corresponding Backend Tech Doc:** `docs/requirements/backend/technical-requirements.md`

---

## 1. Overview

<!--
  One short paragraph: what the frontend consumes and how it connects to the backend.
  Mention the base API URL environment variable.
-->

{Overview paragraph}

**API Base URL:** `process.env.NEXT_PUBLIC_API_BASE_URL` (configured per environment)

---

## 2. Domain Types

<!--
  One subsection per feature domain (maps to a features/{domain}/ folder).
  Each subsection defines ALL TypeScript types for that domain.

  Type categories:
  - ViewModel     → matches the backend response body (what GET returns)
  - CreateModel   → matches the request body for POST calls
  - UpdateModel   → matches the request body for PATCH calls (all fields optional)
  - FilterParams  → query parameters for collection endpoints
  - Enums         → string union types matching backend enum values

  Rules:
  - Use `type`, not `interface`.
  - Nullable fields use `T | null`.
  - Optional query params use `field?: T`.
  - Date fields from the API are typed as `string` (ISO 8601).
  - All field names must exactly match the backend JSON response keys (camelCase).
-->

### {Domain1} — `features/{domain1}/types/{domain1}.types.ts`

```typescript
// ------------------------------------------------------------
// Enums
// ------------------------------------------------------------

export type {EnumName} = "{Value1}" | "{Value2}";
// Example: export type TaskStatus = "Pending" | "Completed";

// ------------------------------------------------------------
// View Model — matches GET /api/v1/{domain1s} response item
// ------------------------------------------------------------

export type {Entity1}ViewModel = {
    {entity1Id}:     string;         // Guid comes from API as string
    {field1}:        string;
    {field2}:        string | null;  // nullable field
    {statusField}:   {EnumName};
    {dateField}:     string | null;  // DateOnly? from backend → ISO string
    {related}:       {Entity2}ViewModel[];
    createdUtc:      string;
    updatedUtc:      string;
};

// ------------------------------------------------------------
// Create Model — maps to POST /api/v1/{domain1s} request body
// ------------------------------------------------------------

export type {Entity1}CreateModel = {
    {field1}:        string;
    {field2}:        string | null;
    {dateField}:     string | null;
    {relatedIds}:    string[];
};

// ------------------------------------------------------------
// Update Model — maps to PATCH /api/v1/{domain1s}/{id} request body
// All fields optional (partial update semantics)
// ------------------------------------------------------------

export type {Entity1}UpdateModel = {
    {field1}:        string | null;
    {field2}:        string | null;
    {dateField}:     string | null;
};

// ------------------------------------------------------------
// Filter Params — query parameters for GET /api/v1/{domain1s}
// ------------------------------------------------------------

export type {Entity1}FilterParams = {
    keyword?:        string;
    {statusFilter}?: {EnumName};
    {relatedId}?:    string;
    {dateFrom}?:     string;
    {dateTo}?:       string;
    sort?:           string;
    skip?:           number;
    take?:           number;
};
```

---

### {Domain2} — `features/{domain2}/types/{domain2}.types.ts`

```typescript
export type {Entity2}ViewModel = {
    {entity2Id}:  string;
    {field1}:     string;
    createdUtc:   string;
    updatedUtc:   string;
};

export type {Entity2}CreateModel = {
    {field1}: string;
};

export type {Entity2}UpdateModel = {
    {field1}: string | null;
};

export type {Entity2}FilterParams = {
    keyword?: string;
    sort?:    string;
    skip?:    number;
    take?:    number;
};
```

---

<!--
  Add more domain subsections as needed.
  One subsection per features/{domain}/ folder.
-->

---

## 3. API Functions

<!--
  One subsection per domain. Maps directly to infra/{domain}/{domain}.api.ts.
  List every function with its signature.

  Rules:
  - One function per HTTP call.
  - Function name mirrors the HTTP operation: get, create, update, delete.
  - Return type matches the ViewModel or void.
  - Filter/query params are passed as a parameter typed to FilterParams.
  - Import types from features/{domain} barrel — infra is allowed to import from features types.
-->

### {Domain1} — `infra/{domain1}/{domain1}.api.ts`

```typescript
const BASE = "/api/v1/{domain1s}";

getAll{Entity1}s(filter: {Entity1}FilterParams): Promise<{Entity1}ViewModel[]>
get{Entity1}({entity1Id}: string):               Promise<{Entity1}ViewModel>
create{Entity1}(model: {Entity1}CreateModel):    Promise<{Entity1}ViewModel>
update{Entity1}({entity1Id}: string, model: {Entity1}UpdateModel): Promise<void>
delete{Entity1}({entity1Id}: string):            Promise<void>

// Include domain-specific operations if they exist:
// {action}{Entity1}({entity1Id}: string):        Promise<void>
// set{Entity1}{Related}({entity1Id}: string, {relatedId}s: string[]): Promise<void>
```

---

### {Domain2} — `infra/{domain2}/{domain2}.api.ts`

```typescript
const BASE = "/api/v1/{domain2s}";

getAll{Entity2}s(filter: {Entity2}FilterParams): Promise<{Entity2}ViewModel[]>
get{Entity2}({entity2Id}: string):               Promise<{Entity2}ViewModel>
create{Entity2}(model: {Entity2}CreateModel):    Promise<{Entity2}ViewModel>
update{Entity2}({entity2Id}: string, model: {Entity2}UpdateModel): Promise<void>
delete{Entity2}({entity2Id}: string):            Promise<void>
```

---

## 4. SWR Hooks

<!--
  One subsection per domain. Maps directly to features/{domain}/hooks/.
  List each hook with its signature and the infra function it wraps.

  Rules:
  - One hook per infra function — no combined calls.
  - Query hooks use useSWR; mutation hooks use useSWRMutation.
  - SWR key: array form ["{domain}", ...params].
  - Pass null as key to conditionally skip fetching (when ID is empty/undefined).
  - Mutation hooks revalidate related queries via global mutate in onSuccess.
-->

### {Domain1} — `features/{domain1}/hooks/`

| Hook | Wraps | SWR Key | Notes |
|---|---|---|---|
| `useGetAll{Entity1}s(filter: {Entity1}FilterParams)` | `getAll{Entity1}s` | `["{domain1}s", filter]` | Collection query |
| `useGet{Entity1}({entity1Id}: string)` | `get{Entity1}` | `["{domain1}s", {entity1Id}]` or `null` | Single query; null key when ID is empty |
| `useCreate{Entity1}()` | `create{Entity1}` | `["{domain1}s"]` | Mutation; revalidates `"{domain1}s"` on success |
| `useUpdate{Entity1}()` | `update{Entity1}` | `["{domain1}s", "update"]` | Mutation; revalidates `"{domain1}s"` on success |
| `useDelete{Entity1}()` | `delete{Entity1}` | `["{domain1}s", "delete"]` | Mutation; revalidates `"{domain1}s"` on success |
| `use{Action}{Entity1}()` | `{action}{Entity1}` | `["{domain1}s", "{action}"]` | Domain-specific mutation (if applicable) |

---

### {Domain2} — `features/{domain2}/hooks/`

| Hook | Wraps | SWR Key | Notes |
|---|---|---|---|
| `useGetAll{Entity2}s(filter: {Entity2}FilterParams)` | `getAll{Entity2}s` | `["{domain2}s", filter]` | Collection query |
| `useGet{Entity2}({entity2Id}: string)` | `get{Entity2}` | `["{domain2}s", {entity2Id}]` or `null` | Single query |
| `useCreate{Entity2}()` | `create{Entity2}` | `["{domain2}s"]` | Mutation |
| `useUpdate{Entity2}()` | `update{Entity2}` | `["{domain2}s", "update"]` | Mutation |
| `useDelete{Entity2}()` | `delete{Entity2}` | `["{domain2}s", "delete"]` | Mutation |

---

## 5. Pages & Routes

<!--
  Maps each Next.js App Router route segment to the feature components it renders.
  Rules:
  - Pages are thin wrappers — no business logic, no direct API calls.
  - All data flows through features/ hooks via feature components.
  - Route segment folder names follow the backend domain naming (e.g. tasks/, tags/).
-->

| Route | File | Feature Component(s) Rendered | Notes |
|---|---|---|---|
| `/{domain1s}` | `app/{domain1s}/page.tsx` | `{Entity1}List` | Collection page |
| `/{domain1s}/[{entity1Id}]` | `app/{domain1s}/[{entity1Id}]/page.tsx` | `{Entity1}Detail` | Detail page |
| `/{domain2s}` | `app/{domain2s}/page.tsx` | `{Entity2}List` | Collection page |
| `/{domain2s}/[{entity2Id}]` | `app/{domain2s}/[{entity2Id}]/page.tsx` | `{Entity2}Detail` | Detail page |

---

## 6. Form Schemas

<!--
  Zod schemas for every form in the application.
  One subsection per feature domain that has forms.
  Schema field names must match the CreateModel / UpdateModel field names exactly.
  Include validation messages that match the business rules.
-->

### {Domain1} Forms — `features/{domain1}/`

```typescript
// Create{Entity1}Schema — used in {Entity1}Form.tsx for creation

import { z } from "zod";

export const Create{Entity1}Schema = z.object({
    {field1}: z.string()
               .min(1, "{field1} is required")
               .max({n}, "{field1} must be {n} characters or fewer"),
    {field2}: z.string()
               .max({n}, "{field2} must be {n} characters or fewer")
               .nullable(),
    {dateField}: z.string().nullable(),
    {relatedIds}: z.array(z.string()),
});

export type Create{Entity1}FormValues = z.infer<typeof Create{Entity1}Schema>;

// Update{Entity1}Schema — used in {Entity1}Form.tsx for editing
// All fields optional (partial update)

export const Update{Entity1}Schema = z.object({
    {field1}: z.string()
               .min(1, "{field1} is required")
               .max({n}, "{field1} must be {n} characters or fewer")
               .nullable(),
    {field2}: z.string()
               .max({n})
               .nullable(),
    {dateField}: z.string().nullable(),
});

export type Update{Entity1}FormValues = z.infer<typeof Update{Entity1}Schema>;
```

---

### {Domain2} Forms — `features/{domain2}/`

```typescript
import { z } from "zod";

export const Create{Entity2}Schema = z.object({
    {field1}: z.string()
               .min(1, "{field1} is required")
               .max({n}, "{field1} must be {n} characters or fewer"),
});

export type Create{Entity2}FormValues = z.infer<typeof Create{Entity2}Schema>;
```
