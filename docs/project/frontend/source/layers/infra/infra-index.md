# Infra Layer — Index

The Infra layer is the communication boundary between the frontend and the backend REST API. It owns the HTTP client configuration and all raw API call functions. It has no React dependencies — no hooks, no components, no JSX.

Use this index to navigate directly to the topic you need.

---

## Structure & Rules

| Document | What it covers |
|---|---|
| [Structure](infra-structure.md) | Full folder tree with scale points and critical rules |

---

## HTTP

| Document | What it covers |
|---|---|
| [HTTP Client](http-client.md) | Axios instance configuration — base URL, interceptors, singleton rule |

---

## API Functions

| Document | What it covers |
|---|---|
| [API Functions](api-functions.md) | Domain `.api.ts` file pattern — one function per HTTP call |

---

## Authentication

| Document | What it covers |
|---|---|
| [Auth Provider](../app/auth-provider.md) | Current identity provider config — env vars, token flow, provider swap guide |

The `infra/auth/` module owns the NextAuth options (`authOptions`) and module augmentation. It is consumed by the route handler in `app/(public)/api/auth/[...nextauth]/route.ts`.

---

## Guidelines

- Read **Structure** before adding or moving any file in `infra/`.
- Read **HTTP Client** before touching `http/http-client.ts` — the Axios instance is a singleton.
- Read **API Functions** before implementing a new domain API file.
- To change the identity provider, read **Auth Provider** first — only `infra/auth/auth.config.ts` needs to change.
