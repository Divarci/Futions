# Bruno API Testing Guidelines

Standards for maintaining the Bruno collection that covers all REST API endpoints.

---

## Collection Structure

All Bruno files live under `docs/bruno/`:

```
📁 docs/bruno/
├── bruno.json
├── collection.bru
├── 📁 environments/
│   ├── Local.bru
│   ├── Development.bru
│   ├── Staging.bru
│   └── Production.bru
└── 📁 <Resource>/          ← one folder per controller
    ├── folder.bru
    ├── Get <Resources>.bru
    ├── Get <Resource> by ID.bru
    ├── Create <Resource>.bru
    ├── Update <Resource>.bru
    └── Delete <Resource>.bru
```

**Required environment variables** (set in each `environments/*.bru`):
- `base_url` — base API URL including version, e.g. `https://localhost:7001/api/v1`
- `request_timeout` — max timeout in ms (default: `30000`)

---

## Response Shapes

All endpoints serialize `Result` types directly. The HTTP status code comes from `result.StatusCode`; `statusCode`, `isFailure`, and `isSuccess` are `[JsonIgnore]` — they never appear in the response body.

**Single-entity / command responses — `Result<T>` and `Result`:**
```json
// Result<T>  — GET by ID, POST, PATCH
{ "message": "...", "data": { "id": "...", "name": "..." } }

// Result     — PUT, PATCH, DELETE (no data payload)
{ "message": "..." }
```

**Collection response — `PaginatedResult<T>`:**
```json
{
  "message": "...",
  "data": [...],
  "metadata": {
    "pageNumber": 1,
    "pageSize": 25,
    "totalCount": 100,
    "pageCount": 4,
    "totalPages": 4
  }
}
```

**Error response — `ProblemDetails` (all 4xx / 5xx):**
```json
{
  "title": "Error message",
  "type": "https://datatracker.ietf.org/...",
  "status": 404,
  "extensions": { "errors": ["Field error 1"] }  // 422 only
}
```

---

## Request Requirements

Every `.bru` file MUST contain:

- **`meta`** block — descriptive name, `type: http`, sequential `seq`
- **HTTP method and URL** — always reference `{{base_url}}`
- **`auth`** — inherit from collection unless a specific override is needed
- **`body:json`** — for POST / PUT / PATCH operations
- **`params:query`** — for filterable / paginated GET operations
- **`tests {}`** — Chai assertions (status code, structure, response time)
- **`script:post-response {}`** — variable extraction / chaining logic (never mix with assertions)

Query parameter names match `PaginationFilterModel` exactly (all lowercase):
- `page`, `pageSize`, `sortBy`, `isAscending`, `filter`

Query parameter defaults:
- Strings → empty value: `filter: `
- Numbers → numeric literal: `page: 1`
- Booleans → boolean literal: `isAscending: true`

---

## Tests vs Post-Response Script

These two blocks are **strictly separate**:

| Block | Purpose |
|---|---|
| `tests {}` | Chai assertions only — status codes, response structure, timing |
| `script:post-response {}` | Side-effect logic — extract IDs, set variables for chaining |

```javascript
// tests block — assertions only
tests {
  test("Status is 200", function() {
    expect(res.getStatus()).to.equal(200);
  });
  test("Response has data.id", function() {
    expect(res.getBody().data).to.have.property('id');
  });
}

// script:post-response block — logic only
script:post-response {
  if (res.getStatus() === 200) {
    bru.setVar("resourceId", res.getBody().data.id);  // id is inside data, not root
  }
}
```

---

## Variables

Variable precedence (highest → lowest):

```
Runtime      bru.setVar / bru.getVar
Request      bru.setRequestVar / bru.getRequestVar
Folder       bru.setFolderVar / bru.getFolderVar
Environment  bru.setEnvVar / bru.getEnvVar
Collection   bru.getCollectionVar
Global       bru.setGlobalVar / bru.getGlobalVar
```

Rules:
- Never hardcode URLs, IDs, or secrets — always use variables
- Use `bru.setVar()` (runtime) for temporary chaining data
- Use `bru.setEnvVar()` only when the value must persist across runs
- Resource IDs extracted in post-response scripts are runtime variables

Available script libraries: `uuid`, `nanoid`, `moment`, `lodash (_)`, `crypto-js`

---

## Request Templates

#### GET (paginated)
```
meta {
  name: Get <Resources>
  type: http
  seq: 1
}

get {
  url: {{base_url}}/<resources>
}

params:query {
  page: 1
  pageSize: 25
  sortBy: id
  isAscending: true
  filter:
}

tests {
  test("Status is 200", function() {
    expect(res.getStatus()).to.equal(200);
  });
  test("Response has data array", function() {
    expect(res.getBody()).to.have.property('data').that.is.an('array');
  });
  test("Response has metadata", function() {
    const metadata = res.getBody().metadata;
    expect(metadata).to.have.property('pageNumber');
    expect(metadata).to.have.property('totalCount');
  });
}
```

#### GET by ID
```
meta {
  name: Get <Resource> by ID
  type: http
  seq: 2
}

get {
  url: {{base_url}}/<resources>/{{resourceId}}
}

tests {
  test("Status is 200", function() {
    expect(res.getStatus()).to.equal(200);
  });
  test("Response has data", function() {
    expect(res.getBody()).to.have.property('data');
    expect(res.getBody().data).to.have.property('id');
  });
}
```

#### POST (create)
```
meta {
  name: Create <Resource>
  type: http
  seq: 3
}

post {
  url: {{base_url}}/<resources>
}

body:json {
  {
    "name": "Example"
  }
}

script:post-response {
  // id lives inside data, not at root level
  if (res.getStatus() === 200) {
    bru.setVar("resourceId", res.getBody().data.id);
  }
}

tests {
  test("Status is 200", function() {
    expect(res.getStatus()).to.equal(200);
  });
  test("Response has data.id", function() {
    expect(res.getBody().data).to.have.property('id');
  });
}
```

#### PATCH (update)
```
meta {
  name: Update <Resource>
  type: http
  seq: 4
}

patch {
  url: {{base_url}}/<resources>/{{resourceId}}
}

body:json {
  {
    "name": "Updated Name"
  }
}

tests {
  test("Status is 200", function() {
    expect(res.getStatus()).to.equal(200);
  });
  test("Response has message", function() {
    expect(res.getBody()).to.have.property('message');
  });
}
```

#### DELETE
```
meta {
  name: Delete <Resource>
  type: http
  seq: 5
}

delete {
  url: {{base_url}}/<resources>/{{resourceId}}
}

tests {
  test("Status is 200", function() {
    expect(res.getStatus()).to.equal(200);
  });
  test("Response has message", function() {
    expect(res.getBody()).to.have.property('message');
  });
}
```

---

## folder.bru Template

Every resource folder MUST contain a `folder.bru`:

```
meta {
  name: <Resource>
  seq: 1
}

auth {
  mode: inherit
}

vars {
  resource_path: <resources>
}

docs {
  Operations for managing <resources>.
}
```

---

## Scripting Patterns

### Authentication Chaining

```javascript
// In Login.bru — script:post-response
if (res.getStatus() === 200) {
  bru.setEnvVar("authToken", res.getBody().access_token);
}

// In any subsequent request — headers block
headers {
  Authorization: Bearer {{authToken}}
}
```

### Pre-Request Scripts

```javascript
// Idempotency header
req.setHeader("X-Request-Id", uuid.v4());

// Timestamp header
req.setHeader("X-Request-Timestamp", Date.now().toString());
```

### Variable Debugging

```javascript
console.log(bru.getVar("resourceId"));         // runtime
console.log(bru.getEnvVar("authToken"));       // environment
console.log(bru.getFolderVar("resource_path")); // folder
```

---

## When Adding a New Endpoint

1. Create a `.bru` file in the matching resource folder
2. If the folder is new, create `folder.bru` using the template above
3. Add `tests {}` — validate status code and response structure
4. Add `script:post-response {}` if subsequent requests depend on a returned ID
5. Never hardcode IDs or environment-specific values
6. Commit the `.bru` file alongside the API change

---

## Troubleshooting

| Symptom | Check |
|---|---|
| 401 / auth failure | `authToken` env var set? Auth audience matches API config? |
| 404 Not Found | URL uses correct plural form? `base_url` points to correct version? `.bru` in correct folder? |
| Variable undefined | Variable set before it's read? Name matches exactly (case-sensitive)? Post-response script ran successfully? |
| Test assertion fails | `console.log(res.getBody())` to inspect actual shape. Run the request in isolation first. |
