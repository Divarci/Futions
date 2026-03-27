# App Layer — Index

The App layer is the outermost layer of the frontend. It owns all routing, page rendering, layout concerns, and authentication enforcement. All data flows through `features/` hooks — no direct API calls. This layer maps directly to the Next.js App Router file-system conventions.

Use this index to navigate directly to the topic you need.

---

## Structure & Rules

| Document | What it covers |
|---|---|
| [Structure](app-structure.md) | Full folder tree with route groups, middleware, auth route, and critical rules |

---

## Pages & Layout

| Document | What it covers |
|---|---|
| [Pages](pages.md) | Collection and detail page patterns — thin wrappers, route params, rules |
| [Root Layout](layout.md) | Root layout pattern — global providers, `Providers` component |

---

## Error & Loading

| Document | What it covers |
|---|---|
| [Error Boundaries & Loading](error-boundaries.md) | `error.tsx`, `loading.tsx` conventions and coordination with feature-level handling |

---

## Authentication & Middleware

| Document | What it covers |
|---|---|
| [Middleware](middleware.md) | JWT-based authentication guard — token validation, redirect rules, header injection |
| [Auth Provider](auth-provider.md) | Current identity provider config (Keycloak) — env vars, token flow, and provider swap guide |

---

## Guidelines

- Read **Structure** before adding any new route segment or file.
- Read **Pages** before implementing any new page component.
- Read **Middleware** before touching `middleware.ts` or the auth route handler.
- `app/` must never import from `infra/` — always go through `features/`.
- `export default` is required for page and layout files (Next.js convention).
- All new domain pages go inside `(protected)/` — never at the `app/` root.
