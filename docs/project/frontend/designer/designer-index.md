# Designer Documentation — Index

This folder contains the complete design system for the project. All visual design decisions live here.

> **For developers:** Read these docs only when a task requires visual or design decisions — creating a new component, designing a new page, or establishing a new layout. If the task is purely structural (wiring hooks, writing API functions), skip this folder entirely.

---

## Core Philosophy

- Design is born from interpreting the prompt — no imposed aesthetics.
- Every design must be **theme-aware**: adjust all token values for both light and dark.
- Every design must be **mobile-first responsive**: base styles for mobile, breakpoint prefixes for larger screens.

---

## Routing Table

Start with the workflow, then read only the topics your task requires.

| Topic | File | Read when |
|---|---|---|
| **Workflow & Checklist** | [workflow.md](workflow.md) | Always — start here for any design task |
| **Prompt Analysis** | [prompt-analysis.md](prompt-analysis.md) | Analyzing a new design prompt |
| **Theme System** | [theme.md](theme.md) | Setting or adjusting colors, dark/light mode, token values |
| **Responsive Design** | [responsive.md](responsive.md) | Designing layouts or adapting for different screen sizes |
| **Typography** | [typography.md](typography.md) | Choosing fonts, text sizes, line heights |
| **Spacing & Sizing** | [spacing.md](spacing.md) | Padding, margins, border radius, shadows |
| **Component Library** | [components.md](components.md) | Designing or referencing Button, Card, Input, Modal, Nav, etc. |
| **Motion & Animation** | [motion.md](motion.md) | Adding transitions, enter/exit animations, stagger patterns |
| **Accessibility** | [accessibility.md](accessibility.md) | Contrast verification, ARIA, focus styles, touch targets |

---

## Design Process Summary

```
1. Read workflow.md — commit to tone, color axis, layout density, motion level
2. Adjust ALL token values in globals.css (:root + .dark) — read theme.md
3. Establish layout skeleton mobile-first — read responsive.md
4. Build components with token classes — read components.md
5. Apply typography and spacing — read typography.md + spacing.md
6. Layer motion last — read motion.md
7. Verify accessibility — read accessibility.md
8. Run the Pre-Delivery Checklist in workflow.md before declaring done
```

---

## Non-Negotiables (apply to every design, always)

1. **Token classes only** on semantic surfaces — `bg-background`, `text-foreground`, `border-border`, etc. Never `bg-white`, `text-gray-*`, `text-black`.
2. **Both themes adjusted** — every design re-derives `:root` and `.dark` token values from the prompt. Default values are a starting point, not a final palette.
3. **Mobile-first** — base styles for mobile. `md:`, `lg:` prefixes for larger screens. No unprefixed desktop-only layouts.
4. **All data-fetching components** have loading, error, and empty states designed.
5. **WCAG AA contrast** verified for all token pairs before delivery.
