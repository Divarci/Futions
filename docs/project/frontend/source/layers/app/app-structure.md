# App — Structure

The App layer maps directly to the Next.js App Router file-system conventions. One domain folder per route group.

---

## Tree

```
src/
└── app/
    ├── layout.tsx                   ← root layout (providers, global styles)
    ├── page.tsx                     ← root page (/)
    ├── error.tsx                    ← root error boundary
    ├── loading.tsx                  ← root loading state
    └── {domain}/                    ← one folder per domain
        ├── page.tsx                 ← collection page (list view)
        ├── loading.tsx              ← loading state for this route
        └── [id]/
            └── page.tsx             ← detail page
```

**Example:**

```
app/
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
        └── page.tsx                 ← new domain folders go here (sibling of tasks/)
```

---

## Critical Rules

- `app/` files must never import from `infra/` — always go through `features/`.
- Page components are thin — no business logic, no data transformation, no direct API calls.
- `export default` is required for every page and layout file (Next.js convention).
- All other files in the project use named exports.
- Route segment folder names follow the domain naming from the backend API (e.g., `tasks/`, `tags/`).
- `error.tsx` must be a Client Component (`"use client"`).
