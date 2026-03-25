# Features — Structure

The Features layer groups all business state by domain. Each domain is a self-contained vertical slice.

---

## Tree

```
src/
└── features/
    └── {domain}/                    ← one folder per domain
        ├── hooks/
        │   ├── useGet{Entities}.ts  ← paginated collection query
        │   ├── useGet{Entity}.ts    ← single entity query
        │   ├── useCreate{Entity}.ts ← create mutation
        │   ├── useUpdate{Entity}.ts ← update mutation
        │   └── useDelete{Entity}.ts ← delete mutation
        ├── components/
        │   ├── {Entity}List.tsx     ← list component
        │   ├── {Entity}Card.tsx     ← single-item component
        │   └── {Entity}Form.tsx     ← create / edit form
        ├── actions/                 ← optional — add only when Server Actions are needed
        │   └── {domain}.actions.ts
        ├── types/
        │   └── {domain}.types.ts    ← all types for this domain
        └── index.ts                 ← barrel: public API of the feature
```

**Example (Task domain):**

```
features/
└── tasks/
    ├── hooks/
    │   ├── useGetTasks.ts
    │   ├── useGetTask.ts
    │   ├── useCreateTask.ts
    │   ├── useUpdateTask.ts
    │   └── useDeleteTask.ts
    ├── components/
    │   ├── TaskList.tsx
    │   ├── TaskCard.tsx
    │   └── TaskForm.tsx
    ├── types/
    │   └── task.types.ts
    └── index.ts                     ← new domain folders go here (sibling of tasks/)
```

---

## Critical Rules

- Every new domain gets its own folder directly under `features/`. No nesting domains inside each other.
- Every domain folder must follow the exact layout: `hooks/`, `components/`, `types/`, `index.ts`. The `actions/` folder is optional — add it only when a Server Action is actually needed.
- Two features must never import from each other — not even through barrels.
- When a page needs data from two features, composition happens at the `app/` layer.
- If a type must be shared across three or more features, move it to `core/types/` — never reference another feature's types directly.
- All public symbols are exported through `index.ts`. External files must never import from internal paths.
