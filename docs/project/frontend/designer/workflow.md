# Designer — Workflow & Handbook

The complete prompt-to-delivery design workflow, plus the prompt template and pre-delivery checklist.

---

## Prompt → Design Decision Flow

```
1. PARSE  (read prompt-analysis.md)
   ├── Identify sector / context
   ├── List emotional tone keywords
   ├── Estimate target audience
   └── Check for explicit references or constraints

2. DECIDE  (commit before generating)
   ├── Color temperature   → warm / cool / neutral
   ├── Typographic character → serif / humanist sans / geometric / display
   ├── Layout density      → open / balanced / dense
   └── Motion intensity    → static / subtle / moderate / animated

3. GENERATE  (in this order)
   ├── Adjust ALL token values in globals.css for both :root and .dark  (read theme.md)
   ├── Establish layout skeleton — mobile-first                          (read responsive.md)
   ├── Build components with token classes                               (read components.md)
   ├── Apply typography scale                                            (read typography.md)
   ├── Apply spacing and sizing                                          (read spacing.md)
   └── Layer motion as the final step                                    (read motion.md)

4. VALIDATE  (complete before delivering)
   ├── Theme: all token values adjusted for both light and dark?
   ├── Contrast: all token pairs pass WCAG AA (≥ 4.5:1)?               (read accessibility.md)
   ├── Colors: all semantic surfaces using token classes only?
   ├── Responsive: mobile (<640px), tablet (768px), desktop (1280px+) tested?
   ├── Focus: keyboard navigation visible?
   ├── States: loading / error / empty states all designed?
   └── Motion: prefers-reduced-motion respected?
```

---

## Prompt Template

Use this template to gather design requirements before starting:

```
Project:        [What does this product do?]
Page/Component: [Which screen or component is being designed?]
Audience:       [Who will use it — professionals, consumers, children, etc.?]
Tone:           [3–5 adjectives — e.g., "calm, trustworthy, minimal"]
Reference:      [Similar product or design to draw from, if any]
Constraint:     [Existing brand color, font, or style to stay within]
Special note:   [Edge cases, accessibility concerns, or requirements]
```

---

## Pre-Delivery Checklist

Complete this checklist before marking any design as done.

### Theme

```
□ --color-primary and --color-primary-fg updated for both :root and .dark
□ --color-background reflects the design's warmth/coolness for both :root and .dark
□ --color-foreground contrasts ≥ 7:1 against background in both modes
□ --color-muted and --color-muted-fg are cohesive with the primary palette
□ All extended tokens (success, warning, info) defined in both :root and .dark
□ Tailwind config updated for any extended tokens
```

### Color & UI

```
□ All background colors: bg-background, bg-muted, bg-primary (no bg-white, bg-gray-*)
□ All text colors: text-foreground, text-muted-fg, text-primary-fg (no text-black, text-gray-*)
□ All borders: border-border (no border-gray-*)
□ All WCAG AA contrast pairs verified
```

### Responsive

```
□ Base styles target mobile — no unprefixed desktop-only layout classes
□ Tested at: mobile (<640px), tablet (768px), desktop (1280px+)
□ No horizontal overflow on mobile
□ Touch targets ≥ 44×44px on mobile
□ Navigation collapses correctly on mobile
```

### Components & States

```
□ All loading, error, and empty states designed
□ All interactive states present: hover, focus, active, disabled, loading
□ Max 1 primary button per page
□ Modals have focus-trap, ESC handler, aria-modal, aria-labelledby
□ All icon-only buttons have aria-label
□ All form inputs have associated label and aria-describedby
```

### Motion

```
□ All animations complete in ≤ 300ms
□ prefers-reduced-motion respected
□ No decorative-only animations
```
