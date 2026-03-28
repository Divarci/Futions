# Designer — Spacing & Sizing

All spacing follows an **8pt grid**. Use Tailwind's built-in spacing utilities — they align to this grid by default.

---

## Spacing Scale

```
p-1  / m-1  / gap-1  → 0.25rem  (4px)
p-2  / m-2  / gap-2  → 0.5rem   (8px)
p-3  / m-3  / gap-3  → 0.75rem  (12px)
p-4  / m-4  / gap-4  → 1rem     (16px)
p-6  / m-6  / gap-6  → 1.5rem   (24px)
p-8  / m-8  / gap-8  → 2rem     (32px)
p-12 / m-12 / gap-12 → 3rem     (48px)
p-16 / m-16 / gap-16 → 4rem     (64px)
p-20 / m-20 / gap-20 → 5rem     (80px)
p-24 / m-24 / gap-24 → 6rem     (96px)
p-32 / m-32 / gap-32 → 8rem     (128px)
```

---

## Border Radius

```
rounded-sm   → 4px    (subtle, data-dense UI)
rounded-md   → 8px    (standard inputs, buttons)
rounded-lg   → 12px   (cards)
rounded-xl   → 16px   (modals, panels)
rounded-2xl  → 24px   (feature cards, hero sections)
rounded-full → 9999px (pills, avatars, badges)
```

Choose a single radius level for the entire design and stay consistent. Mix only when there is a clear visual hierarchy reason (e.g., a button inside a card may use `rounded-md` while the card uses `rounded-lg`).

---

## Shadow Scale

```
shadow-sm  → 0 1px 2px rgba(0,0,0,0.05)           — subtle lift, active inputs
shadow-md  → 0 4px 6px rgba(0,0,0,0.07), ...      — cards, dropdowns
shadow-lg  → 0 10px 15px rgba(0,0,0,0.1), ...     — modals, popovers
shadow-xl  → 0 20px 25px rgba(0,0,0,0.1), ...     — elevated panels
shadow-2xl → 0 25px 50px rgba(0,0,0,0.25)         — floating overlays
```

---

## Spacing Application Guide

| Context | Recommended spacing |
|---|---|
| Inline icon + label gap | `gap-2` (8px) |
| Input padding | `px-3 py-2` |
| Button padding (md) | `px-4 py-2` |
| Card internal padding | `p-4` or `p-6` |
| Section vertical spacing | `py-12` or `py-16` |
| Page outer padding | `px-4 md:px-8 lg:px-16` |
| Between form fields | `space-y-4` |
| Between list cards | `space-y-3` or `gap-4` in grid |

---

## Pre-Delivery Spacing Checklist

```
□ All spacing values align to the 8pt grid (no arbitrary px values)
□ A single border-radius level is used consistently throughout the design
□ Page outer padding scales with breakpoints (px-4 md:px-8 lg:px-16)
□ Shadow levels match element elevation hierarchy
□ No margin-bottom fighting with flex/grid gap — choose one layout method
```
