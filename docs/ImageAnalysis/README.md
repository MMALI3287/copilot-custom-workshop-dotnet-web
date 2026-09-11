# Image Analysis

Every image used in this workshop, described in full text. Nothing here replaces the image visually; the point is that each description is complete enough that you could rebuild the thing shown without seeing the picture.

## Why these files exist

- **Vision is not guaranteed.** Step 6 attaches a wireframe to Copilot and asks it to build a UI from it. Not every environment supports image attachment (VS2022 in particular), and the model's reading of an image is not deterministic. These files are the deterministic version of the same information.
- **Alt text has to come from somewhere.** The `alt` attributes in the English docs are short. The long descriptions live here.
- **The Japanese in the images is not searchable.** Labels drawn inside a PNG cannot be grepped, translated by a browser, or read by a screen reader. Every Japanese string visible in an image is transcribed here with its English meaning.

## Index

| File | Image | Used in |
|------|-------|---------|
| [designer-wireframe.md](designer-wireframe.md) | `docs/assets/Designer.png` | Step 6 (the Vision exercise) |
| [logo.md](logo.md) | `docs/assets/logo.png` | Step 6 (the header logo) |
| [architecture-stacks.md](architecture-stacks.md) | `docs/2_BeforeGettingStarted/images/ArchitectureStacks.jpg` | Step 2 |
| [init-webapp.md](init-webapp.md) | `docs/3_CreateProject/images/init-webapp.png` | Step 3 |
| [cats-index-screenshot.md](cats-index-screenshot.md) | `docs/6_ImplementMVC/images/cats-index.png` | Step 6 |
| [check-aicredit.md](check-aicredit.md) | `docs/7_TokenManagement/images/check-aicredit.png` | Step 7 |

## Inventory

| Image | Format | Dimensions | Size | Has Japanese text |
|-------|--------|-----------|------|-------------------|
| `Designer.png` | PNG, RGB | 1536 × 1024 | 584 KB | Yes, extensively |
| `logo.png` | PNG, RGB | 1536 × 1024 | 276 KB | Yes (tagline) |
| `ArchitectureStacks.jpg` | JPEG, 330 DPI | 2635 × 2133 | 328 KB | No |
| `init-webapp.png` | PNG, RGBA | 1306 × 370 | 36 KB | No |
| `cats-index.png` | PNG, RGB | 1035 × 1065 | 60 KB | Yes, extensively |
| `check-aicredit.png` | PNG, RGBA | 1191 × 302 | 36 KB | Yes (one sentence) |

## A note on discrepancies

The wireframe, the finished screenshot and the seed data in Step 5 do not agree with each other. That is not a bug in the workshop, but you should know about it before you spend time trying to reconcile them:

| | Wireframe (`Designer.png`) | Screenshot (`cats-index.png`) | Seed data (Step 5) |
|---|---|---|---|
| Number of cats | 8 | 5 | 5 |
| Cat names | モカ, レオ, ソラ, ミルク, クロ, ハル, ナナ, ココ | みけ, くろ, しろ, チャチャ, ソラ | Same as the screenshot |
| Table columns | ID, Name, Age, Breed, 操作 | ID, 名前, 年齢, 品種, 説明, 操作 | - |
| Column header language | English (except 操作) | Japanese | - |
| Sidebar items | 10 | 7 | - |

The wireframe is a design mock drawn before the data model existed. The screenshot is the real result. Where they conflict, **the screenshot and the seed data win**, because those are what the code actually produces. Details in each file.
