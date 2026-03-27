# App — Structure

The App layer maps directly to the Next.js App Router file-system conventions. Routes are split into two top-level route groups: `(protected)/` for authenticated routes and `(public)/` for unauthenticated access. Authentication is enforced globally by `middleware.ts`.

---

## Tree

```
src/
├── middleware.ts                    ← authentication guard (runs on every protected request)
└── app/
    ├── (protected)/                 ← all authenticated routes live here
    │   ├── globals.css              ← global styles (Tailwind, theme tokens)
    │   ├── layout.tsx               ← root layout (providers, global styles)
    │   ├── not-found.tsx            ← global 404 page
    │   ├── page.tsx                 ← root page (/)
    │   └── {domain}/               ← one folder per domain
    │       ├── page.tsx             ← collection page (list view)
    │       ├── loading.tsx          ← loading state for this route
    │       └── [id]/
    │           └── page.tsx         ← detail page
    └── (public)/                    ← unauthenticated routes live here
        └── api/
            └── auth/                ← provider-specific auth handler (e.g., NextAuth [...nextauth])
```

**Example — adding a new domain:**

```
app/
└── (protected)/
    ├── layout.tsx
    ├── page.tsx
    ├── tasks/
    │   ├── page.tsx
    │   ├── loading.tsx
    │   └── [taskId]/
    │       └── page.tsx
    └── tags/
        ├── page.tsx
        └── [tagId]/
            └── page.tsx             ← new domain folders go here (sibling of tasks/)
```

---

## Authentication — Route Groups and Middleware

All routes are protected by default via `middleware.ts`. The middleware runs on every request except the explicitly excluded paths (`api/auth`, `_next/static`, `_next/image`, `favicon.ico`).

**Route groups:**

| Group | Path prefix | Access |
|---|---|---|
| `(protected)/` | Everything except the exclusions below | Requires valid JWT |
| `(public)/` | `app/(public)/api/auth/` | Open — no token required |

**Middleware behaviour:**
- No token → redirect to `/api/auth/signin?callbackUrl={original path}`
- Token with refresh error → redirect to `/api/auth/signin?error=SessionExpired&callbackUrl={original path}`
- Valid token → request passes through; `x-user-id` and `x-user-email` headers are injected

**`(public)/api/auth/`** will contain the provider-specific auth route handler (e.g., NextAuth `[...nextauth]/route.ts`). Its exact shape depends on the chosen auth provider and is defined separately.

---

## Critical Rules

- `app/` files must never import from `infra/` — always go through `features/`.
- Page components are thin — no business logic, no data transformation, no direct API calls.
- `export default` is required for every page and layout file (Next.js convention).
- All other files in the project use named exports.
- Route segment folder names follow the domain naming from the backend API (e.g., `tasks/`, `tags/`).
- `error.tsx` must be a Client Component (`"use client"`).
- All new domain pages go inside `(protected)/` — never at the `app/` root.
- Never add business routes to `(public)/` — that group is reserved for the auth handler only.
