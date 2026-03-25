# Core — Components

Primitive, reusable UI building blocks with no knowledge of any domain feature.

---

## Folder Pattern

```
core/components/
└── {Component}/
    ├── {Component}.tsx        ← implementation
    └── {Component}.types.ts  ← Props type
```

---

## Rules

- Accepts only generic props — no `TaskViewModel`, no domain types.
- Styled with Tailwind CSS.
- One component per folder.
- Each folder contains the implementation file and its `Props` type file.

---

## Examples

`Button`, `Input`, `Modal`, `Spinner`, `Badge`, `ErrorMessage`
