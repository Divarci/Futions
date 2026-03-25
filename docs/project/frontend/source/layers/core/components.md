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
- Styled with Tailwind CSS using **semantic theme token classes** (`bg-background`, `text-foreground`, `border-border`, etc.) — never raw colour utilities (`bg-white`, `text-gray-*`).
- **Mobile-first responsive**: base styles target mobile; use `sm:`, `md:`, `lg:` prefixes to enhance for larger screens.
- One component per folder.
- Each folder contains the implementation file and its `Props` type file.

---

## Examples

`Button`, `Input`, `Modal`, `Spinner`, `Badge`, `ErrorMessage`, `ThemeToggle`
