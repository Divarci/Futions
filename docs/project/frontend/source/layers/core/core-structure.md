# Core — Structure

The Core layer is the frontend foundation. It has zero layer dependencies — everything else depends on it.

---

## Tree

```
src/
└── core/
    ├── components/                  ← primitive, domain-agnostic UI components
    │   ├── {Component}/
    │   │   ├── {Component}.tsx      ← component implementation
    │   │   └── {Component}.types.ts ← Props type and any component-local types
    │   └── index.ts                 ← barrel: re-exports all core components
    ├── hooks/                       ← shared utility hooks (no domain logic)
    │   ├── useDebounce.ts
    │   ├── usePagination.ts
    │   └── index.ts
    ├── types/
    │   ├── common.types.ts          ← shared API response shapes (ApiResponse<T>, PaginatedResponse<T>)
    │   ├── theme.types.ts           ← Theme type ('light' | 'dark' | 'system')
    │   └── index.ts
    └── utils/
        ├── date.utils.ts
        └── index.ts                 ← new util files go here (sibling of date.utils.ts)
```

---

## Critical Rules

- `core/` files must never import from `features/`, `infra/`, or `app/`.
- A component, hook, or utility belongs in `core/` only when actively used by **three or more unrelated features**. Keep it in the owning feature folder until that threshold is reached.
- Every component in `core/components/` must be truly generic — if it references a domain type, it belongs in `features/` instead.
- Every component lives in its own sub-folder with an implementation file and a `Props` type file.
- All sub-folders (`components/`, `hooks/`, `types/`, `utils/`) export through their own `index.ts` barrel.
