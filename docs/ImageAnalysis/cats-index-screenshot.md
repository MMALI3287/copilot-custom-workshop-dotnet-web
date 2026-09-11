# cats-index.png - The Finished Cat List Screen

**Path:** `docs/6_ImplementMVC/images/cats-index.png`
**Format:** PNG, 1035 × 1065, 8-bit RGB, non-interlaced, 60 KB
**Used in:** [Step 6 - Implement MVC + Vision](../6_ImplementMVC/README_EN.md), as "what the finished screen looks like"

---

## Summary

A browser screenshot of the running MeowWorld application at the cat list screen, captured after the Step 6 Vision exercise. This is the authoritative picture of what the workshop actually produces, as opposed to the wireframe, which is a pre-implementation mock.

It is a Bootstrap 5 dashboard: a dark top bar, a light left sidebar, and a white content area with a table of five cats.

## Layout structure

```text
┌────────────────────────────────────────────────────────────────┐
│ [logo] MeowWorld 猫管理システム                                  │  dark bar, full width
├──────────────┬─────────────────────────────────────────────────┤
│ ダッシュボード  │  猫一覧                                         │  h1
│ ▐ 猫一覧      │  [ 新しい猫を登録 ]                              │  primary button
│ 新しい猫を登録  │                                                 │
│ カテゴリ       │  ID 名前   年齢  品種      説明        操作      │  table header
│ 健康記録       │  ─────────────────────────────────────────────  │
│ 里親募集       │  1  みけ    3   三毛猫    おとなしい性格 詳細編集削除│  5 rows, striped
│ スケジュール    │  ...                                            │
│ レポート       │                                                 │
│              │                        (large empty area)        │
└──────────────┴─────────────────────────────────────────────────┘
```

| Region | Approximate extent |
|--------|--------------------|
| Top bar | full width, ~65 px tall |
| Left sidebar | ~170 px wide, full height below the bar |
| Content | the remainder |

A vertical scrollbar is visible on the right edge of the capture, and the content area is mostly empty below the table. The page was rendered in a tall viewport with only five rows to show.

## Top bar

- Dark, near-black background spanning the full width
- At the far left, a small white square tile containing the MeowWorld logo (the cat face with the wordmark beneath it, rendered small enough that the tagline text is illegible)
- Immediately to the right, in white: **MeowWorld 猫管理システム**

| Japanese | Romanisation | English |
|----------|--------------|---------|
| 猫管理システム | neko kanri shisutemu | Cat Management System |

Note the difference from the wireframe: the logo shrank to a small tile, moved from centre to left, and the tagline moved from below the wordmark into the bar as running text.

## Left sidebar

Seven items on a very light grey background. The second item (猫一覧) is active, shown as a solid blue filled block with white text. The remaining items are plain links in a dark colour.

| # | Japanese | Romanisation | English | State |
|---|----------|--------------|---------|-------|
| 1 | ダッシュボード | dasshubōdo | Dashboard | |
| 2 | 猫一覧 | neko ichiran | Cat List | **active, blue** |
| 3 | 新しい猫を登録 | atarashii neko o tōroku | Register a New Cat | |
| 4 | カテゴリ | kategori | Categories | |
| 5 | 健康記録 | kenkō kiroku | Health Records | |
| 6 | 里親募集 | satooya boshū | Adoption | |
| 7 | スケジュール | sukejūru | Schedule | |
| 8 | レポート | repōto | Reports | |

That is eight rows in the capture. The wireframe's ユーザー管理 (User Management) and 設定 (Settings) are absent. No icons were carried over from the wireframe; these are text-only links.

## Content area

### Page heading and action

- **猫一覧** as a large bold heading
- Below it, a blue filled button labelled **新しい猫を登録** ("Register a New Cat")

Unlike the wireframe, the button is stacked *under* the heading rather than pushed to the right of it.

### Table

Six columns, with a striped body (alternating white and very light grey rows) and no vertical borders. The header row is bold with a bottom rule. This is Bootstrap's `table table-striped`.

| Column | Header (JA) | Romanisation | English |
|--------|-------------|--------------|---------|
| 1 | ID | - | ID |
| 2 | 名前 | namae | Name |
| 3 | 年齢 | nenrei | Age |
| 4 | 品種 | hinshu | Breed |
| 5 | 説明 | setsumei | Description |
| 6 | 操作 | sōsa | Actions |

All headers are Japanese here, unlike the wireframe's mixed English/Japanese.

### Table data (transcribed in full)

This matches the Step 5 seed data exactly.

| ID | 名前 | Romanisation | 年齢 | 品種 | Breed (EN) | 説明 | Description (EN) |
|----|------|--------------|------|------|-----------|------|------------------|
| 1 | みけ | Mike | 3 | 三毛猫 | Calico | おとなしい性格 | A quiet temperament |
| 2 | くろ | Kuro | 5 | 黒猫 | Black cat | 甘えん坊 | Loves being pampered |
| 3 | しろ | Shiro | 2 | 白猫 | White cat | *(empty)* | *(empty)* |
| 4 | チャチャ | Chacha | 1 | 茶トラ | Orange tabby | 元気いっぱい | Full of energy |
| 5 | ソラ | Sora | 4 | ロシアンブルー | Russian Blue | 静かな環境が好き | Likes a quiet environment |

Row 3 has an empty Description cell, which is the visible result of `Description` being `string?` and the seed value being `null`. That is a useful detail: it confirms the nullable type from the Custom Instructions survived into the database and the view renders it without an error.

The ID and 年齢 columns render in a reddish-brown colour distinct from the rest of the text. That is most likely a link colour or a Bootstrap utility class picked up by the generated view; it is cosmetic and carries no meaning.

### Action buttons

Three small buttons per row, coloured:

| Order | Japanese | English | Colour | Bootstrap class |
|-------|----------|---------|--------|-----------------|
| 1 | 詳細 | Details | cyan | `btn-info` |
| 2 | 編集 | Edit | amber/yellow | `btn-warning` |
| 3 | 削除 | Delete | red | `btn-danger` |

The wireframe specified no colours, so this palette came from the model choosing conventional Bootstrap semantics: informational, cautionary, destructive. It is a reasonable default and worth pointing out during the workshop, because it is an example of the model filling a gap the design left open.

## What this screenshot tells you about the generated code

Reading backwards from the pixels, the view almost certainly contains:

- `<table class="table table-striped">` with a `<thead>` and a `@foreach` over `Model`
- `asp-action="Details|Edit|Delete"` with `asp-route-id="@item.Id"` on anchor tags styled as buttons
- A `_Layout.cshtml` with a `navbar` (dark) and a sidebar column, most likely `container-fluid` + `row` + two `col` elements
- The sidebar active state as a conditional class, or hard-coded on the 猫一覧 link

The last point matters. If the active state is hard-coded rather than derived from the current route, every sidebar link will look inactive except 猫一覧. Check this in your own output.

## Suggested alt text

> The finished cat list screen: a dark header with the MeowWorld logo, a left sidebar of Japanese navigation links with "Cat List" highlighted in blue, and a striped table of five cats with cyan, amber and red action buttons on each row.

## Relationship to the JA/EN toggle

Every string in this screenshot except the cat names, breeds and descriptions is UI chrome, and all of it is listed with a resource key in the [string catalog](../Localization/string-catalog.md). The three data columns are content and stay in Japanese by default. See [docs/Localization/README.md](../Localization/README.md) for the reasoning.
