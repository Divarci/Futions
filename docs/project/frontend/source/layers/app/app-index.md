# App Layer — Index

The App layer is the outermost layer of the frontend. It owns all routing, page rendering, and layout concerns. All data flows through `features/` hooks — no direct API calls. This layer maps directly to the Next.js App Router file-system conventions.

Use this index to navigate directly to the topic you need.

---

## Structure & Rules

| Document | What it covers |
|---|---|
| [Structure](app-structure.md) | Full folder tree with scale points and critical rules |

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

## Guidelines

- Read **Structure** before adding any new route segment or file.
- Read **Pages** before implementing any new page component.
- `app/` must never import from `infra/` — always go through `features/`.
- `export default` is required for page and layout files (Next.js convention).
