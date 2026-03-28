# Designer — Prompt Analysis

When a design request arrives, analyze the prompt across the following dimensions before making any visual decision. Design is born from interpreting the prompt — never impose personal aesthetic preferences.

---

## Tone & Emotion

Map the emotional signals in the prompt to a design direction.

| Prompt Signal | Interpreted Tone | Design Direction |
|---|---|---|
| "minimal", "clean", "simple" | Quiet, focused | Heavy whitespace, mono typography |
| "bold", "powerful", "eye-catching" | Aggressive, dynamic | Large headings, high contrast |
| "warm", "friendly", "human" | Organic, approachable | Soft colors, serif fonts |
| "corporate", "trustworthy" | Authoritative, clean | Grid-focused, neutral palette |
| "creative", "artistic", "experimental" | Avant-garde | Asymmetry, unexpected layouts |
| "dark", "mysterious", "night" | Dramatic | Dark mode, deep tones |
| "playful", "joyful", "fun" | Light, colorful | Vivid palette, rounded forms |

---

## Keyword Extraction

Scan the prompt across these dimensions:

```
[INDUSTRY]   → fintech / health / entertainment / education / tech ...
[AUDIENCE]   → children / professionals / seniors / artists ...
[ACTION]     → sell / inform / entertain / connect ...
[FEELING]    → trust / excitement / curiosity / calm ...
[METAPHOR]   → open space / cave / laboratory / garden ...
```

---

## Reference Mapping

Concrete references within the prompt (brand names, style names, associations) become design constraints.

> Prompt: *"Like Notion but warmer"*
> → Grid: Notion-like block structure ✓
> → Color: Cream/beige tones instead of Notion's cold grays
> → Font: Semi-serif or humanist sans instead of Notion's strict sans-serif

---

## Decision Output

After analysis, commit to four decisions before generating any code:

| Decision | Options |
|---|---|
| **Color temperature** | warm / cool / neutral |
| **Typographic character** | serif / humanist sans / geometric sans / display |
| **Layout density** | open / balanced / dense |
| **Motion intensity** | static / subtle / moderate / animated |

These four decisions drive all subsequent token and component choices.
