# Infra — Structure

The Infra layer owns the HTTP client and all raw API call functions. No React, no hooks, no JSX.

---

## Tree

```
src/
└── infra/
    ├── http/
    │   ├── http-client.ts           ← Axios instance, base URL, interceptors
    │   └── index.ts
    ├── auth/
    │   ├── auth.config.ts           ← NextAuth options (provider, callbacks, refresh logic)
    │   ├── auth.types.ts            ← NextAuth module augmentation (Session, JWT)
    │   └── index.ts
    ├── {domain}/                    ← one folder per domain
    │   ├── {domain}.api.ts          ← raw API functions for one domain
    │   └── index.ts
    └── index.ts                     ← barrel: re-exports all modules
```

**Example:**

```
infra/
├── http/
│   └── http-client.ts
├── auth/
│   ├── auth.config.ts
│   ├── auth.types.ts
│   └── index.ts
├── tasks/
│   ├── task.api.ts
│   └── index.ts
└── tags/
    ├── tag.api.ts
    └── index.ts                     ← new domain folders go here (sibling of tasks/)
```

> `auth/` is a fixed infrastructure module — not a domain API module. Do not apply the `{domain}.api.ts` pattern to it. See [Auth Provider](../app/auth-provider.md) for configuration details.

---

## Critical Rules

- `infra/` files must never import from `app/`.
- `infra/` files must never import React, use hooks, or return JSX.
- One `.api.ts` file per domain — do not merge multiple domains into one file.
- All API endpoint URLs are defined inside the `.api.ts` file — no URL strings anywhere else.
- Error handling (HTTP status codes, retry, toast notifications) belongs in `features/` hooks, not here.
- The `httpClient` instance is created once in `http/http-client.ts` — never create a second instance.
