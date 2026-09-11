# Implement MVC + Vision

🌐 **Language:** [日本語](README_JA.md) | **English**

[Previous - Implement the DB Layer](../5_ImplementDBLayer/README_EN.md) | [Next - Token Savings and Context Management](../7_TokenManagement/README_EN.md)

In this step you implement the controller and the views. You then use Copilot's **Vision feature** (image recognition) to generate UI from a wireframe image.

> **Time:** about 40 minutes
> **You will end with:** a working CRUD screen for cats, styled from a wireframe
> **New Copilot skills:** invoking a `.prompt.md`, and Vision (attaching an image)
>
> **The longest step.** If you are running short, the Vision exercise (section 2) is the part with a text-based fallback.


---

## 1. Generate the CRUD controller (using .prompt.md)

Use the reusable prompt you created in Step 4.

> 🕵️ **Agent mode** - type `/` and select `create-crud-controller`

**Original (Japanese):**

```
/create-crud-controller Cat エンティティ（Id, Name, Age, Breed, Description, CreatedAt）
```

**English equivalent:**

```
/create-crud-controller the Cat entity (Id, Name, Age, Breed, Description, CreatedAt)
```

> **Key point:** using `.prompt.md` makes the structure of your CRUD controllers consistent every time. You can reuse the same prompt when you add another entity.

### Let the Agent handle the rest

Once the controller is generated, give the follow-up instruction:

> 🕵️ **Agent mode**

**Original (Japanese):**

```
CatsController の全ビュー（Index, Details, Create, Edit, Delete）も作成して、
ナビゲーションメニューに「猫一覧」リンクを追加してください。
ビルドと動作確認もお願いします。
```

**English equivalent:**

```
Also create all the views for CatsController (Index, Details, Create, Edit, Delete)
and add a "猫一覧" (cat list) link to the navigation menu.
Please build and verify it works as well.
```

---

## 2. Implementing UI with Vision

### What the Vision feature is

Copilot can **take an image as input**, understand its contents and generate code. Hand it a design comp or a wireframe and it implements the corresponding HTML/CSS for you.

### Preparing the logo image

The wireframe includes a cat logo in the header, but **GitHub Copilot is a code generation tool and cannot generate image files**. You have to provide the logo image separately.

#### Option A: generate it with M365 Copilot (Microsoft Designer) (recommended)

Generate an image at [Microsoft Designer](https://designer.microsoft.com/) with a prompt such as:

**Original (Japanese):**

```
シンプルでかわいい猫のロゴを作成してください。
```

**English equivalent:**

```
Create a simple, cute cat logo.
```

Save the generated image as `wwwroot/images/logo.png`.

> **Key point:** GitHub Copilot handles code generation and M365 Copilot handles image generation. This is a practical workflow that combines the Microsoft Copilot ecosystem.

#### Option B: use the logo bundled with the repository

This repository includes a pre-generated logo. Copy it from the terminal in the workspace (`app/`):

```bash
mkdir -p MeowWorld/wwwroot/images
cp ../docs/assets/logo.png MeowWorld/wwwroot/images/logo.png
```

> **Note:** `../docs/assets/` points at `docs/assets/` inside the cloned repository (one level above the workspace).

#### Option C: substitute an icon font or emoji

- **Bootstrap Icons:** use an icon font such as `<i class="bi bi-emoji-smile"></i>`
- **emoji:** place `🐱` directly in the header

Either option is fine for the exercises that follow.

---

### Exercise: re-implement the view from the wireframe

1. Switch Copilot Chat to Agent mode

2. Attach the wireframe image to the chat. There are three ways, in order of reliability:

   | Method | How | Notes |
   |--------|-----|-------|
   | **Paperclip** | Click 📎 in the chat input box, then browse to the file | Most reliable. In VS Code the menu item is "Attach Context" or "Add Context" depending on version |
   | **Drag and drop** | Drag the PNG from your file explorer onto the chat input | Fast, but some VS Code versions insert a path rather than the image |
   | **Paste** | Copy the image to the clipboard, then `Ctrl` + `V` in the chat box | Works well when you have just taken a screenshot |

   The file to attach:

   ```text
   copilot-custom-workshop-dotnet-web/docs/assets/Designer.png
   ```

   Remember this sits **one level above your workspace**, since your workspace is `app/`. From the workspace it is `../docs/assets/Designer.png`.

   **Confirm the attachment worked before sending.** You should see a thumbnail or a chip naming the image in the input box. If you only see a file path as text, the image was not attached and Copilot will be guessing.

   > **If you cannot attach an image at all**, Vision is unavailable in your setup. This is expected on Visual Studio 2022 and on some older Copilot Chat versions. Skip to the [text-based alternative](#text-based-alternative-for-vs2022-or-when-you-have-no-image) below. You will get an equivalent UI; you just will not experience the image-to-code workflow.

3. Enter the following prompt:

> 🕵️ **Agent mode** + 🖼️ **Vision (image attached)**

**Original (Japanese):**

```
添付したワイヤーフレームを参考に、Views/Cats/Index.cshtml を
このデザインに近づけてください。左サイドバーのナビゲーションは
_Layout.cshtml に実装してください。
ヘッダーのロゴには wwwroot/images/logo.png を使用してください。
```

**English equivalent:**

```
Using the attached wireframe as a reference, bring Views/Cats/Index.cshtml
closer to this design. Implement the left sidebar navigation in
_Layout.cshtml.
Use wwwroot/images/logo.png for the header logo.
```

4. Copilot analyses the image, recognises the following and implements it:
   - The title in the header area
   - The navigation structure of the left sidebar
   - The cat list table in the main content
   - The action buttons (details, edit, delete)

### What to check

| Element in the image | Expected implementation |
|----------------------|-------------------------|
| Left sidebar | A Bootstrap sidebar navigation in `_Layout.cshtml` |
| Cat list table | `<table class="table">` with `asp-action` tag helpers |
| Action buttons | Details / Edit / Delete link buttons |
| Responsive | Bootstrap `container-fluid` + `row` + `col` |

### How close should you expect it to be?

Set expectations honestly, because "it does not look identical" is the most common reaction here and it is the wrong conclusion.

| Aspect | Expect | Why |
|--------|--------|-----|
| Overall layout (header, sidebar, table) | **Close** | Structure is what Vision reads most reliably |
| Presence of the right columns and buttons | **Close** | These are explicit in the image |
| Exact spacing, font sizes, border weights | **Not close** | The wireframe is a hand sketch with no measurements |
| Colours | **Invented** | The wireframe is black and white. Any colour came from Bootstrap or from the model |
| The eight sample cats in the wireframe | **Will not appear** | Your data comes from the Step 5 seed, which has five different cats |
| Sidebar items beyond Dashboard / Cat list / Register | **Dead links** | Nothing behind them exists |

The last two are the ones that confuse people. The wireframe was drawn before the data model existed, so its sample rows and your real rows deliberately disagree. Full detail is in [Image Analysis - Designer Wireframe](../ImageAnalysis/designer-wireframe.md).

### If the result is wrong, iterate rather than restart

Vision output is a starting point. Refine it in the same session, where the image is still in context:

```
サイドバーが本文の上に重なっています。Bootstrap の grid で
サイドバーと本文を横並びにしてください。
```

English: `The sidebar is overlapping the main content. Use the Bootstrap grid to place the sidebar and the content side by side.`

```
テーブルに「説明」列が抜けています。Cat.Description を表示する列を追加してください。
```

English: `The table is missing the Description column. Add a column that displays Cat.Description.`

> **Learning point:** using Vision dramatically streamlines the "implement the screen the designer made" workflow. You no longer have to describe the layout in fine detail in the prompt. What it does not do is remove the review step: you still read the result and correct it, exactly as you would a junior developer's first pass.

> **A full text description of the wireframe**, including every label, the table contents and an English/Japanese label mapping, is in [Image Analysis - Designer Wireframe](../ImageAnalysis/designer-wireframe.md). It is worth reading even if Vision works for you, because it tells you exactly what the model was supposed to see.

---

### Text-based alternative (for VS2022 or when you have no image)

If the Vision feature is unavailable or you have no image, the following prompt implements an equivalent UI:

<details>
<summary>Text-based prompt (click to expand)</summary>

> 🕵️ **Agent mode**

**Original (Japanese):**

```
Views/Cats/Index.cshtml と _Layout.cshtml を以下の仕様で実装してください：

【_Layout.cshtml】
- ヘッダー: "MeowWorld 猫管理システム" のタイトル
- 左サイドバーナビゲーション:
  - ダッシュボード
  - 猫一覧（アクティブ）
  - 新しい猫を登録
  - カテゴリ
  - 健康記録
  - 里親募集
  - スケジュール
  - レポート
  - ユーザー管理
  - 設定
- メインコンテンツ: @RenderBody()
- Bootstrap 5 のダッシュボード風レイアウト

【Views/Cats/Index.cshtml】
- テーブルに猫一覧を表示（ID, 名前, 年齢, 品種）
- 各行に「詳細」「編集」「削除」ボタン
- 「新しい猫を登録」ボタン（テーブル上部）
```

**English equivalent:**

```
Implement Views/Cats/Index.cshtml and _Layout.cshtml to the following specification:

[_Layout.cshtml]
- Header: the title "MeowWorld 猫管理システム" (MeowWorld Cat Management System)
- Left sidebar navigation:
  - ダッシュボード (Dashboard)
  - 猫一覧 (Cat list) (active)
  - 新しい猫を登録 (Register a new cat)
  - カテゴリ (Categories)
  - 健康記録 (Health records)
  - 里親募集 (Adoption)
  - スケジュール (Schedule)
  - レポート (Reports)
  - ユーザー管理 (User management)
  - 設定 (Settings)
- Main content: @RenderBody()
- A Bootstrap 5 dashboard-style layout

[Views/Cats/Index.cshtml]
- Display the cat list in a table (ID, name, age, breed)
- "詳細" (Details), "編集" (Edit) and "削除" (Delete) buttons on each row
- A "新しい猫を登録" (Register a new cat) button above the table
```

> Keep the Japanese labels if you want the workshop's intended result. If you prefer an English or bilingual UI, implement the toggle from [docs/Localization/README.md](../Localization/README.md) rather than hard-coding English labels here; the sidebar keys are already listed in the [string catalog](../Localization/string-catalog.md).

</details>

---

## 3. Verify the behaviour

```bash
cd MeowWorld
dotnet run
```

Check the display in the browser and confirm the following work:

### What the finished screen looks like

![The finished cat list screen: a dark header with the MeowWorld logo, a left sidebar of Japanese navigation links, and a five-row cat table with Details, Edit and Delete buttons](images/cats-index.png)

> A full text description of this screenshot is in [Image Analysis - Cats Index Screenshot](../ImageAnalysis/cats-index-screenshot.md).

- [ ] The cat list is displayed in table form
- [ ] The 5 seed records are displayed
- [ ] You can add a cat with Create
- [ ] You can change a cat's information with Edit
- [ ] You can delete a cat with Delete
- [ ] You can check a cat's information with Details

---

### Step 6 completion checklist

- [ ] `Controllers/CatsController.cs` exists with all five actions, each `async`
- [ ] `Views/Cats/` contains Index, Details, Create, Edit and Delete
- [ ] `_Layout.cshtml` has the sidebar navigation
- [ ] The header shows the logo, or a deliberate emoji/icon substitute
- [ ] `dotnet build` succeeds
- [ ] The list screen shows all 5 seed cats
- [ ] Create adds a cat and it appears in the list
- [ ] Edit changes a cat and the change persists after a reload
- [ ] Details shows one cat
- [ ] Delete removes a cat, with a confirmation screen first
- [ ] Row 3 (しろ) shows an empty Description cell without erroring

That last box confirms the nullable `string?` from Step 5 survives all the way to the view.

> **Before moving on:** if you deleted a cat while testing, you now have fewer than 5 rows. That is fine. If you would rather reset, delete `meowworld.db` and run `dotnet run` again; the automatic migration from Step 5 recreates it with the original seed data. Stop the app first or the file will be locked.

---

## Summary

The Copilot features you experienced in this step:

| Feature | What you experienced |
|---------|----------------------|
| `.prompt.md` | Consistent CRUD generation through a reusable prompt |
| **Vision** | Automatic UI implementation from a wireframe image |
| Agent mode | Controller + views + build verification end to end |
| `.instructions.md` | `applyTo: "**/*.cshtml"` applied Bootstrap 5 + Japanese automatically |

---

[Previous - Implement the DB Layer](../5_ImplementDBLayer/README_EN.md) | [Next - Token Savings and Context Management](../7_TokenManagement/README_EN.md)
