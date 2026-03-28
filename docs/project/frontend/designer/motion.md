# Designer — Motion & Animation

Animations must be purposeful, fast, and consistent. They communicate state changes and hierarchy — they never exist for decoration alone.

---

## Design Principles

| Principle | Rule |
|---|---|
| **Purposeful** | Every animation carries information (enter, exit, state change, feedback) |
| **Fast** | UI animations must complete in < 300ms; users should never wait for an animation |
| **Consistent** | The same action animates identically everywhere in the UI |
| **Reducible** | All animations must respect `prefers-reduced-motion` — wrap animations in a check |

---

## Duration & Easing Reference

```css
/* Duration */
--duration-instant: 50ms;   /* Press feedback, micro interactions */
--duration-fast:    150ms;  /* Hover effects, tooltips */
--duration-normal:  250ms;  /* Standard transitions */
--duration-slow:    400ms;  /* Complex state changes */
--duration-slower:  600ms;  /* Page transitions (use sparingly) */

/* Easing */
--ease-out:    cubic-bezier(0, 0, 0.2, 1);        /* Elements entering → decelerates */
--ease-in:     cubic-bezier(0.4, 0, 1, 1);        /* Elements leaving  → accelerates */
--ease-inout:  cubic-bezier(0.4, 0, 0.2, 1);      /* Position changes */
--ease-spring: cubic-bezier(0.34, 1.56, 0.64, 1); /* Playful, elastic feedback */
```

Tailwind equivalents: `transition-all duration-150 ease-out`, `duration-200`, `duration-300`

---

## Animation Type Mapping

| Trigger | Animation | Duration | Easing |
|---|---|---|---|
| Page load | Staggered fade-up (y: 16→0, opacity: 0→1) | 300–500ms per item | ease-out |
| Hover (card/button) | scale(1.02) + shadow increase | 150ms | ease-out |
| Click / press | scale(0.97) | 50ms | ease-in |
| Modal open | scale(0.95→1) + opacity(0→1) | 200ms | ease-out |
| Modal close | scale(1→0.95) + opacity(1→0) | 150ms | ease-in |
| Skeleton → content | opacity crossfade | 300ms | ease-inout |
| Error feedback | translateX ±6px × 3 shake | 400ms total | ease-inout |
| Success checkmark | stroke-dashoffset draw | 300ms | ease-out |

---

## Stagger Pattern

Use for lists of cards or items appearing on load:

```tsx
// Each item starts 70ms after the previous
items.map((item, i) => (
    <motion.div
        key={item.id}
        initial={{ opacity: 0, y: 16 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: i * 0.07, duration: 0.3, ease: [0, 0, 0.2, 1] }}
    >
        <ItemCard item={item} />
    </motion.div>
))

// Rules:
// - Apply stagger to max 8 visible items
// - For longer lists, stagger only the first 4 items (delay the rest to 0)
// - Never stagger table rows — use a single fade for the whole table
```

---

## Tailwind Transition Utilities

For simple animations that do not require a motion library:

```tsx
// Hover lift on cards
className="transition-all duration-150 ease-out hover:-translate-y-0.5 hover:shadow-lg"

// Button press feedback
className="transition-transform duration-[50ms] active:scale-[0.97]"

// Focus ring appearance
className="transition-shadow duration-150 focus-visible:ring-2 focus-visible:ring-primary"

// Color transitions on interactive elements
className="transition-colors duration-150 hover:bg-muted"
```

---

## Reduced Motion

Always wrap animations in a `prefers-reduced-motion` check:

```tsx
// CSS
@media (prefers-reduced-motion: reduce) {
    * {
        animation-duration: 0.01ms !important;
        transition-duration: 0.01ms !important;
    }
}

// With Framer Motion
const prefersReducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
const transition = prefersReducedMotion ? { duration: 0 } : { duration: 0.3 };
```

---

## Pre-Delivery Motion Checklist

```
□ All animations complete in < 300ms (600ms max for page transitions)
□ Entering elements use ease-out; leaving elements use ease-in
□ Stagger limited to max 8 items
□ prefers-reduced-motion respected — all animations suppressed when enabled
□ No purely decorative animations (every animation communicates state)
□ Tailwind transition utilities used for simple effects — motion library only for complex sequences
```
