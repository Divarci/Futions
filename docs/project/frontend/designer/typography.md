# Designer — Typography

Typography choices are derived from the prompt's emotional tone. Font selection, scale, and spacing all reflect the design's personality.

---

## Font Selection by Prompt Tone

| Tone | Display Font Suggestions | Body Font Suggestions |
|---|---|---|
| Luxury / Editorial | Playfair Display, Cormorant | Lato, Source Serif |
| Technical / SaaS | Space Grotesk, DM Sans | Inter, IBM Plex Sans |
| Creative / Artistic | Bebas Neue, Unbounded | Karla, Nunito |
| Warm / Friendly | Lora, Merriweather | Open Sans, Nunito Sans |
| Bold / Powerful | Anton, Oswald | Roboto, Barlow |

Load selected fonts via `next/font` in the root layout for performance.

---

## Type Scale (Major Third — 1.250 ratio)

Expressed as Tailwind `text-*` utilities:

```
text-xs:   0.64rem   (10.24px)
text-sm:   0.8rem    (12.8px)
text-base: 1rem      (16px)
text-lg:   1.25rem   (20px)
text-xl:   1.563rem  (25px)
text-2xl:  1.953rem  (31.25px)
text-3xl:  2.441rem  (39px)
text-4xl:  3.052rem  (48.8px)
text-5xl:  3.815rem  (61px)
```

---

## Line Height

```
leading-tight:   1.1   → display headings, hero text
leading-snug:    1.35  → card titles, subheadings
leading-normal:  1.5   → body text
leading-relaxed: 1.75  → long-form reading, article content
```

---

## Letter Spacing

```
tracking-tight:  -0.04em → large display headings
tracking-normal: 0em     → body text
tracking-wide:   0.06em  → labels, UI captions
tracking-widest: 0.15em  → all-caps micro labels
```

---

## Responsive Typography

Headings must scale across breakpoints. Body text stays consistent.

```tsx
// Page title — scales up on larger screens
<h1 className="text-2xl md:text-3xl lg:text-4xl font-bold leading-tight tracking-tight text-foreground">

// Section heading
<h2 className="text-xl md:text-2xl font-semibold leading-snug text-foreground">

// Body paragraph — max-width for readability
<p className="text-base leading-relaxed text-foreground max-w-prose">

// Caption / meta text
<span className="text-sm leading-normal text-muted-fg">

// Label
<label className="text-sm font-medium tracking-wide text-foreground">
```

---

## Hierarchy Rules

1. One `text-4xl`/`text-5xl` element maximum per page (the page's primary heading)
2. `text-foreground` for primary text; `text-muted-fg` for supporting/secondary text
3. Never use more than three font size levels in a single component
4. All text colors use token classes — never raw color utilities

---

## Pre-Delivery Typography Checklist

```
□ Font family reflects the prompt's emotional tone
□ Font loaded via next/font (no @import in CSS)
□ Heading sizes scale with md:/lg: breakpoint prefixes
□ Body text uses leading-normal or leading-relaxed
□ All text colors use token classes (text-foreground, text-muted-fg, text-primary-fg)
□ No more than 3 font size levels in any single component
□ All-caps strings use tracking-widest
```
