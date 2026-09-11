# logo.png - The MeowWorld Logo

**Path:** `docs/assets/logo.png`
**Format:** PNG, 1536 × 1024, 8-bit RGB, non-interlaced, 276 KB
**Used in:** [Step 6 - Implement MVC + Vision](../6_ImplementMVC/README_EN.md), as the header logo (Option B: the bundled logo)

---

## Summary

The MeowWorld brand mark: a hand-drawn cat face flanked by two paw prints, above a handwritten wordmark and a Japanese tagline. Dark brown line art on a plain white background, in the same sketch style as the wireframe.

This is the same artwork that appears in the header of `Designer.png`. It was generated with an image tool (Microsoft Designer, per Step 6) rather than drawn by hand or produced in vector software.

## Composition

```text
                    ╭─────╮   ╭─────╮
                    │  ▲  │   │  ▲  │          ears with shaded inner triangles
                  ╭─┴─────┴───┴─────┴─╮
      ✿          │   ●           ●   │          ✿      eyes; paw prints either side
    (paw)   ────│──      ▼         ──│────   (paw)      whiskers; triangular nose
                 │        ‿‿          │                  smiling mouth
                  ╰──────────────────╯
 
                     MeowWorld                           wordmark, handwritten
                    猫管理システム                         tagline, spaced out
```

Vertical order, all centred on a common axis:

1. The cat face, with a paw print to its left and another to its right at roughly eye level
2. The wordmark **MeowWorld**
3. The tagline **猫管理システム**

The artwork occupies roughly the middle 70% of the canvas. There is substantial white margin on all four sides, most of it at the top and bottom.

## The cat face

- A wide, rounded head, slightly broader than it is tall
- Two triangular ears rising from the top, each containing a smaller shaded triangle drawn with fine hatching
- Two solid dark oval eyes, each with a small white highlight dot in the upper area
- A small rounded triangular nose at the centre
- A `‿‿` smiling mouth beneath the nose, drawn as two joined curves
- Three whiskers sweeping out from each side, extending well beyond the head outline

The line weight is even and moderately thick throughout, with the slight irregularity typical of a marker or brush pen. Nothing is filled except the eyes and the hatched ear interiors.

## The paw prints

Two mirrored paw prints, one either side of the head:

- Four separate oval toe beans arranged in an arc
- A larger triangular main pad beneath them, filled with diagonal hatching rather than solid colour

They sit at roughly the same height as the cat's eyes and are noticeably smaller than the head.

## The wordmark

**MeowWorld**, in a rounded handwritten face with capital `M` and `W`, set as one word with no space. It is the largest text element and sits directly beneath the cat face.

## The tagline

| Japanese | 猫管理システム |
|----------|---------------|
| Romanisation | neko kanri shisutemu |
| English | Cat Management System |
| Breakdown | 猫 (neko, cat) + 管理 (kanri, management) + システム (shisutemu, system) |

Set below the wordmark in a smaller weight with wide letter spacing, in a rounded gothic style consistent with the wordmark.

**This tagline is the only Japanese text in the logo**, and it is baked into the pixels. That has a direct consequence for the language toggle, covered below.

## Technical characteristics and how to use it

| Property | Value | Implication |
|----------|-------|-------------|
| Colour mode | RGB, **no alpha channel** | The background is opaque white, not transparent |
| Dimensions | 1536 × 1024 | Landscape 3:2, unusually wide for a logo |
| Size | 276 KB | Heavy for a header asset |
| Format | Raster PNG | No vector source; it will soften when scaled up |

Three practical consequences:

1. **It will show a white box on a dark header.** The finished screenshot (`cats-index.png`) has exactly this: the logo appears as a small white tile against the dark navbar. That is not a mistake in the implementation; it is the file having no transparency. If you want it to sit flush on the dark bar, remove the background yourself or place it on a deliberate white tile, as the screenshot does.

2. **The aspect ratio does not suit a navbar.** At 3:2 landscape with generous margins, constraining it to a navbar height leaves the cat face small and the tagline illegible. In the finished screenshot the tagline inside the image cannot be read at all, which is why the layout repeats 猫管理システム as live text beside it.

3. **276 KB is worth optimising.** For a header image rendered at roughly 40 px, a resized and compressed copy would be a fraction of that. Optimising it is a reasonable, if optional, improvement to suggest during the workshop.

## Interaction with the JA/EN toggle

The tagline is rasterised text. A resource file cannot reach it. If you implement the JA/EN toggle from [docs/Localization/README.md](../Localization/README.md), the logo's built-in 猫管理システム stays Japanese in both languages.

Options, in ascending order of effort:

| Approach | Effort | Result |
|----------|--------|--------|
| Crop the tagline off and render the subtitle as live text next to the mark | Low | One asset, fully translatable subtitle. **Recommended** |
| Ship two files, `logo.ja.png` and `logo.en.png`, and pick with the current culture | Medium | Both taglines look designed; two assets to maintain |
| Rebuild the mark as inline SVG with a `<text>` element | High | Fully scalable, themeable, translatable, and far smaller than 276 KB |

The first is what the finished screenshot effectively already does, since it shows the mark small and puts the readable subtitle in the navbar as text. Making that explicit costs almost nothing.

The `alt` attribute is separately translatable in every approach, and should be. See the `Logo_Alt` key in the [string catalog](../Localization/string-catalog.md).

## Suggested alt text

Language-neutral, for the logo as a whole:

> The MeowWorld logo: a hand-drawn cat face flanked by two paw prints, above the MeowWorld wordmark and the Japanese tagline "cat management system".

For use in the application header, shorter is better: `MeowWorld` in English, `MeowWorld 猫管理システム` in Japanese.

## Licensing note

Step 6 presents this file as pre-generated and bundled with the repository, with AI image generation (Microsoft Designer) as the recommended way to produce your own. Step 10 separately advises being careful about the license and copyright of generated content. If you intend to use a generated logo beyond this workshop, check the terms of the tool that produced it.
