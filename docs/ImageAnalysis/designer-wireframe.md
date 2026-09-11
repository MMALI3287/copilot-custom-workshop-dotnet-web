# Designer.png - The MeowWorld Wireframe

**Path:** `docs/assets/Designer.png`
**Format:** PNG, 1536 × 1024, 8-bit RGB, non-interlaced, 584 KB
**Used in:** [Step 6 - Implement MVC + Vision](../6_ImplementMVC/README_EN.md), as the image attached to Copilot for the Vision exercise

---

## Summary

A hand-drawn, sketch-style wireframe of the MeowWorld cat list screen. The whole image is black line art on a white background, drawn in a deliberately rough "paper sketch" style with slightly wobbly rectangles and a handwritten-looking font. Every label is in Japanese except the three table column headers ID, Name and Age, plus Breed.

It is a single screen, a classic admin dashboard: a centred brand header across the top, a narrow left sidebar of navigation links, and a wide main content area holding a data table.

## Layout structure

```text
┌─────────────────────────────────────────────────────────────────┐
│                    [paw] [cat face] MeowWorld [paw]             │  header, centred
│                          猫管理システム                          │
├──────────────────┬──────────────────────────────────────────────┤
│  ⌂ ダッシュボード   │  猫一覧                    [ + 新しい猫を登録 ] │  page title + primary action
│ ▓▓ 猫一覧 ▓▓▓▓▓▓ │  ┌────┬──────┬─────┬───────────┬─────────┐   │  (active item is shaded)
│  + 新しい猫を登録  │  │ ID │ Name │ Age │   Breed   │   操作   │   │  table header
│  ◇ カテゴリ       │  ├────┼──────┼─────┼───────────┼─────────┤   │
│  ♡ 健康記録       │  │  1 │ モカ  │  2  │ アメリカン… │ 詳細編集削除│   │  8 data rows
│  ⚭ 里親募集       │  │ ...│ ...  │ ... │    ...    │   ...   │   │
│  ▦ スケジュール    │  └────┴──────┴─────┴───────────┴─────────┘   │
│  ▥ レポート       │                                              │
│  ○ ユーザー管理    │                                              │
│  ⚙ 設定          │                                              │
└──────────────────┴──────────────────────────────────────────────┘
```

Proportions, measured off the image:

| Region | Approximate extent | Share of width |
|--------|--------------------|----------------|
| Header band | full width, top ~14% of the image | 100% |
| Left sidebar | from the left edge to ~18% of the width | ~18% |
| Main content | the remaining width | ~82% |

The sidebar and the main content are separated by a single vertical rule. The header is separated from both by a single horizontal rule. There is a thin outer border around the whole frame, which reads as the browser viewport rather than as part of the page.

## Header

Centred as one group:

1. A left paw print (four toe beans above a shaded triangular pad)
2. A line-art cat face: round head, two triangular ears with shaded inner triangles, two solid round eyes, a small triangular nose, a curved smiling mouth, three whiskers on each side
3. The wordmark **MeowWorld** in a large handwritten style, with `M` and `W` capitalised
4. A right paw print, mirroring the left one

Directly beneath the wordmark, in a smaller size and letter-spaced:

| Japanese | Romanisation | English |
|----------|--------------|---------|
| 猫管理システム | neko kanri shisutemu | Cat Management System |

The header artwork is the same drawing as `logo.png` (see [logo.md](logo.md)); the wireframe embeds it rather than referencing a separate asset.

## Left sidebar navigation

Ten items, each an icon on the left and a Japanese label on the right, stacked vertically. The second item (猫一覧) is the active one, shown with a shaded/hatched background band spanning the sidebar width.

| # | Icon drawn | Japanese | Romanisation | English | State |
|---|-----------|----------|--------------|---------|-------|
| 1 | House outline | ダッシュボード | dasshubōdo | Dashboard | |
| 2 | Cat face in a rounded square | 猫一覧 | neko ichiran | Cat List | **active** |
| 3 | Plus sign | 新しい猫を登録 | atarashii neko o tōroku | Register a New Cat | |
| 4 | Tag / label with a dot | カテゴリ | kategori | Categories | |
| 5 | Heart outline | 健康記録 | kenkō kiroku | Health Records | |
| 6 | Two people / handover figure | 里親募集 | satooya boshū | Adoption (foster recruitment) | |
| 7 | Calendar grid | スケジュール | sukejūru | Schedule | |
| 8 | Bar chart | レポート | repōto | Reports | |
| 9 | Single person bust | ユーザー管理 | yūzā kanri | User Management | |
| 10 | Gear / cog | 設定 | settei | Settings | |

A visible gap separates item 3 from item 4, which groups the sidebar into "the two list/create actions for cats" and "everything else". Whether that grouping was intentional is unclear, and the finished implementation does not reproduce it.

Note that only items 1, 2 and 3 correspond to anything the workshop actually builds. Items 4 through 10 are aspirational and lead nowhere.

## Main content area

### Page header row

- Left: the page title **猫一覧** (neko ichiran, "Cat List"), larger than body text
- Right: a bordered rectangular button containing a `+` and the label **新しい猫を登録** (atarashii neko o tōroku, "Register a New Cat")

The title and the button sit on the same baseline, pushed to opposite edges. This maps to a flexbox row with `justify-content: space-between`.

### Table

A five-column table with a full grid of borders around every cell.

| Column | Header as drawn | Language of the header | Alignment of the data |
|--------|-----------------|------------------------|-----------------------|
| 1 | ID | English | centred |
| 2 | Name | English | centred |
| 3 | Age | English | centred |
| 4 | Breed | English | left |
| 5 | 操作 (sōsa, "Actions") | Japanese | centred |

The mixed-language headers are a quirk of the mock. The finished implementation uses Japanese for all of them; see the discrepancy table below.

### Table data (transcribed in full)

| ID | Name (JA) | Romanisation | Age | Breed (JA) | Breed (EN) |
|----|-----------|--------------|-----|------------|------------|
| 1 | モカ | Mocha | 2 | アメリカンショートヘア | American Shorthair |
| 2 | レオ | Leo | 1 | マンチカン | Munchkin |
| 3 | ソラ | Sora | 3 | スコティッシュフォールド | Scottish Fold |
| 4 | ミルク | Milk | 2 | ラグドール | Ragdoll |
| 5 | クロ | Kuro | 4 | 黒猫（雑種） | Black cat (mixed breed) |
| 6 | ハル | Haru | 1 | ベンガル | Bengal |
| 7 | ナナ | Nana | 2 | 三毛猫（雑種） | Calico (mixed breed) |
| 8 | ココ | Coco | 3 | ブリティッシュショートヘア | British Shorthair |

All eight names are katakana, which is the normal way to write a pet's name in Japanese. Six of the breeds are katakana transliterations of Western breed names; two (黒猫, 三毛猫) are native Japanese words for coat patterns rather than pedigree breeds, and both are annotated 雑種 (zasshu, "mixed breed").

### Action buttons

Every row ends with three small bordered buttons, in this order:

| Order | Japanese | Romanisation | English | Maps to |
|-------|----------|--------------|---------|---------|
| 1 | 詳細 | shōsai | Details | `asp-action="Details"` |
| 2 | 編集 | henshū | Edit | `asp-action="Edit"` |
| 3 | 削除 | sakujo | Delete | `asp-action="Delete"` |

They are drawn as plain outlined rectangles with no fill, so the wireframe says nothing about colour. The finished implementation colours them cyan, amber and red respectively.

## What the wireframe does not specify

Worth knowing before you judge Copilot's output against it:

- **No colours.** Everything is black on white. Any colour in the result came from Bootstrap defaults or from the model's own choice, not from the image.
- **No Description column.** The `Cat` entity from Step 5 has a `Description` property. The wireframe has no column for it; the finished screenshot does.
- **No responsive behaviour.** There is one width, and nothing indicates what happens on a narrow screen.
- **No empty, loading, or error states.**
- **No pagination**, despite eight rows implying a list that could grow.
- **No indication of which element is a link versus a button.**
- **No favourites control**, which is the feature added in Step 9.

## Discrepancies with the finished implementation

| Aspect | Wireframe | Finished screenshot (`cats-index.png`) |
|--------|-----------|---------------------------------------|
| Rows | 8 cats | 5 cats (the Step 5 seed data) |
| Names | モカ, レオ, ソラ, ミルク, クロ, ハル, ナナ, ココ | みけ, くろ, しろ, チャチャ, ソラ |
| Columns | ID, Name, Age, Breed, 操作 | ID, 名前, 年齢, 品種, 説明, 操作 |
| Header language | Mixed English and Japanese | All Japanese |
| Sidebar items | 10 | 7 (ユーザー管理 and 設定 dropped, plus one more) |
| Logo placement | Centred, large, with the tagline | Small, top-left, inside a dark bar |
| Header style | Light, centred, no background | Dark bar, left-aligned, title beside the logo |

Only ソラ appears in both, and with a different age (3 in the wireframe, 4 in the seed data).

**Which is correct:** the screenshot and the Step 5 seed data. The wireframe was drawn before the data model was settled. Do not "fix" your implementation to match the wireframe's data.

## Suggested alt text

Short, for use in the docs:

> A hand-drawn wireframe of the MeowWorld cat list screen, with a centred cat logo header, a left sidebar of ten Japanese navigation links, and a table of eight cats with Details, Edit and Delete buttons on each row.

## If you cannot use Vision

Step 6 already gives a text-based alternative prompt. If you want one that matches this image more exactly, including the mixed-language headers and the eight sample rows, use the table above to write it. The Step 6 fallback prompt is deliberately aligned with the *finished* implementation rather than with this wireframe, which is why its sidebar list and columns differ.
