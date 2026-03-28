# Designer — Responsive Design

Every design is **mobile-first**. Base styles target the smallest screen; larger screens are enhanced with breakpoint prefixes. This applies to every component, every page, every layout — no exceptions.

---

## Breakpoint System (Tailwind)

```
sm:    640px   → Large phone
md:    768px   → Tablet
lg:    1024px  → Small laptop
xl:    1280px  → Laptop / desktop
2xl:   1536px  → Wide screen
```

---

## Mobile-First Rule

Always write base styles for mobile, then layer up. Never write desktop-first styles and scale down.

```tsx
// CORRECT — mobile-first
className="text-sm md:text-base lg:text-lg"
className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3"
className="px-4 md:px-8 lg:px-16"
className="flex-col md:flex-row"

// INCORRECT — desktop-first
className="text-lg sm:text-base xs:text-sm"
className="grid-cols-3 md:grid-cols-2 sm:grid-cols-1"
```

---

## Grid Structure

```
Mobile (< 640px):  single column, 16px side padding
Tablet (768px):    2–4 columns, 20px gutter, 32px outer margin
Laptop (1024px):   up to 12 columns, 24px gutter, 48px outer margin
Desktop (1280px+): 12 columns, 32px gutter, 80px outer margin
```

---

## Page Templates by Screen Size

### Collection / List Page

```
[Mobile]
  Sticky header
  Search / filter bar (collapsed by default)
  Vertical card stack (full-width cards)
  Pagination (simple prev/next)

[Tablet]
  Sticky header
  Filter bar (expanded)
  2-column card grid
  Pagination

[Desktop]
  Sticky header
  [Sidebar filters (240px) | 3-column card grid]
  Pagination with page numbers
```

### Detail Page

```
[Mobile]
  Back button + breadcrumb
  Hero (full-width)
  Stacked content sections
  Related items (horizontal scroll)

[Desktop]
  Breadcrumb
  Hero
  [Main content (8 col) | Sticky sidebar (4 col)]
  Related items grid
```

### Dashboard

```
[Mobile]
  Top bar (logo + user menu)
  Bottom tab navigation
  Stats stacked vertically
  Single chart
  Scrollable table

[Desktop]
  Top bar
  Fixed sidebar (240–280px, collapsible)
  Stats row (3–4 columns)
  Charts row
  Full table with sorting
```

### Auth Pages

```
[Mobile]
  Centered card form (full-width with padding)
  Logo at top

[Desktop]
  Split layout: [Visual/brand area (6 col) | Form area (6 col)]
```

---

## Navigation Responsiveness

| Nav Type | Desktop | Mobile |
|---|---|---|
| Sidebar | Fixed 240–280px panel | Hidden — opens as drawer overlay |
| Topbar | Full horizontal bar | Hamburger → full-screen menu |
| Bottom Tab | Not used | Fixed bottom — 4–5 icons |

```tsx
// Sidebar example — hidden on mobile, visible md+
<aside className="hidden md:block w-60 shrink-0">...</aside>

// Mobile sidebar drawer — visible only on mobile
<div className="fixed inset-0 z-50 md:hidden">...</div>
```

---

## Image & Media Responsiveness

```tsx
// Always use aspect-ratio containers — never fixed pixel heights
<div className="aspect-video w-full overflow-hidden rounded-lg">
    <img className="w-full h-full object-cover" src={src} alt={alt} />
</div>

// Responsive hero images
<div className="h-48 md:h-64 lg:h-80 relative overflow-hidden">
    <img className="absolute inset-0 w-full h-full object-cover" ... />
</div>
```

---

## Typography Responsiveness

Heading sizes must scale across breakpoints:

```tsx
// Page title
<h1 className="text-2xl md:text-3xl lg:text-4xl font-bold text-foreground">

// Section heading
<h2 className="text-xl md:text-2xl font-semibold text-foreground">

// Body text stays consistent — only adjust line-length containers
<p className="text-base leading-relaxed text-foreground max-w-prose">
```

---

## Pre-Delivery Responsive Checklist

```
□ Base styles target mobile (no unprefixed desktop-only layout classes)
□ All breakpoints tested: mobile (<640px), tablet (768px), desktop (1280px)
□ Navigation collapses/transforms correctly on mobile
□ Images use aspect-ratio containers — no fixed pixel heights
□ Typography scales with md: and lg: prefixes for headings
□ Touch targets are at least 44×44px on mobile
□ No horizontal overflow on mobile (no fixed-width elements wider than the viewport)
□ Tables are scrollable or restructured on mobile
```
