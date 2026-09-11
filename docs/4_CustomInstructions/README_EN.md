# Defining Project Rules with Custom Instructions

🌐 **Language:** [日本語](README_JA.md) | **English**

[Previous - Create the Project](../3_CreateProject/README_EN.md) | [Next - Implement the DB Layer](../5_ImplementDBLayer/README_EN.md)

In this step you learn GitHub Copilot's **Custom Instructions** feature in three stages. Copilot will follow your project rules automatically, without you having to write them into every prompt.

> **Note on language:** the instruction files below are written in Japanese on purpose. They are what makes the MeowWorld UI and its code comments Japanese, which is the workshop's intended result. English translations are given next to each file so you can read what you are pasting. If you want a bilingual UI with a JA/EN toggle, do not change these files here; follow [docs/Localization/README.md](../Localization/README.md) instead, which adds the toggle on top of the Japanese baseline.

---

## The big picture of Custom Instructions

Copilot custom instructions come at three levels:

| Level | File | Scope | Purpose |
|-------|------|-------|---------|
| **Whole repository** | `.github/copilot-instructions.md` | Every chat | Rules shared across the project |
| **File scope** | `.instructions.md` (with applyTo) | When operating on files matching a pattern | Rules per file type |
| **Reusable prompt** | `.prompt.md` | Only when invoked | Instructions turned into a template |

---

## 1. Repository-level instructions (`.github/copilot-instructions.md`)

### Before: check the output without any settings

First, look at the state without Custom Instructions.

> 💬 **Ask mode**

**Original (Japanese):**

```
ASP.NET Core MVC で猫の情報（名前・年齢・品種）を表す DTO クラスを作成してください。
```

**English equivalent:**

```
Create a DTO class in ASP.NET Core MVC that represents cat information (name, age, breed).
```

Observe the output:
- What language are the comments written in?
- Is the namespace in `namespace X { }` form or `namespace X;` form?
- Are the `required` keyword and nullable types used?

### Procedure: create the file

Create `.github/copilot-instructions.md` at the workspace root:

```markdown
# MeowWorld プロジェクト - Copilot カスタム指示

## 全般
- 日本語で回答すること

## コメント規約
- コード内のコメントは日本語で記述すること
- XML ドキュメントコメント（`///`）も日本語で記述すること

## C# コーディング規約
- file-scoped namespace（`namespace X;` 形式）を使用すること
- nullable reference types を考慮し、必要に応じて `string?` や `required` キーワードを使用すること
- プライマリコンストラクタが使える場面では積極的に使うこと

## ASP.NET Core / Entity Framework Core
- 非同期メソッド（async/await）を優先すること
- DbContext はコンストラクタインジェクションで受け取ること
- LINQ メソッド構文を優先すること

## プロジェクト構成
- OS: Windows
- フレームワーク: .NET 10, ASP.NET Core MVC
- データベース: SQLite（Entity Framework Core 経由）
- テスト: xUnit + Moq
```

<details>
<summary>English translation of the file above (for reading, not for pasting)</summary>

```markdown
# MeowWorld project - Copilot custom instructions

## General
- Answer in Japanese

## Comment conventions
- Write comments inside code in Japanese
- Write XML documentation comments (`///`) in Japanese as well

## C# coding conventions
- Use file-scoped namespaces (the `namespace X;` form)
- Take nullable reference types into account and use `string?` or the `required` keyword where appropriate
- Use primary constructors wherever they apply

## ASP.NET Core / Entity Framework Core
- Prefer asynchronous methods (async/await)
- Receive the DbContext through constructor injection
- Prefer LINQ method syntax

## Project setup
- OS: Windows
- Framework: .NET 10, ASP.NET Core MVC
- Database: SQLite (through Entity Framework Core)
- Testing: xUnit + Moq
```

</details>

> **Checking the settings:**
> - **VS Code:** confirm that `github.copilot.chat.codeGeneration.useInstructionFiles` is enabled
> - **Visual Studio 2026:** confirm the reference is enabled under Tools > Options > GitHub Copilot > Custom Instructions

### After: check the output with the settings applied

Open a **new chat session** and enter the same prompt again:

> 💬 **Ask mode**

```
ASP.NET Core MVC で猫の情報（名前・年齢・品種）を表す DTO クラスを作成してください。
```

### What to check

| Aspect | Before | After |
|--------|--------|-------|
| Comment language | Mostly English | Japanese |
| Namespace form | Block form | File-scoped |
| Null safety | Not considered | `required` / `string?` used |

---

## 2. File-scope instructions (`.instructions.md` + `applyTo`)

On top of the repository-wide rules, you can apply **different rules per file type**.

### Overall structure of the `.github/` folder

All the Copilot customisation files you create in this hands-on go into the `.github/` folder. The final structure looks like this:

```text
.github/
├── copilot-instructions.md              <- Step 4-1: repository-wide instructions
├── instructions/
│   ├── views.instructions.md            <- Step 4-2: file scope (for views)
│   └── tests.instructions.md            <- Step 4-2: file scope (for tests)
├── prompts/
│   └── create-crud-controller.prompt.md <- Step 4-3: reusable prompt
├── agents/
│   └── *.agent.md                       <- Step 9: custom agents
└── skills/
    └── */SKILL.md                       <- Step 9: skills (knowledge the Agent refers to)
```

> **Why this structure?** The official GitHub documentation specifies that path-specific instructions go in the `.github/instructions/` directory. VS Code detects them anywhere in the workspace, but the Copilot features on GitHub.com (cloud agent, code review) require them under `.github/instructions/`. It also lines up with the same "subdirectory per type" pattern as `prompts/`, `agents/` and `skills/`, which makes the structure intuitive.

### Create the instructions for views

Create `.github/instructions/views.instructions.md`:

```markdown
---
applyTo: "**/*.cshtml"
---
- Bootstrap 5 のクラスを使用すること
- レスポンシブデザインを意識すること（`container`、`row`、`col-*` を活用）
- 日本語の UI ラベルを使用すること
- `asp-*` タグヘルパーを積極的に使用すること
```

<details>
<summary>English translation of the file above</summary>

```markdown
---
applyTo: "**/*.cshtml"
---
- Use Bootstrap 5 classes
- Keep responsive design in mind (make use of `container`, `row` and `col-*`)
- Use Japanese UI labels
- Make active use of `asp-*` tag helpers
```

</details>

### Create the instructions for tests

Create `.github/instructions/tests.instructions.md`:

```markdown
---
applyTo: "**/*.Tests/**"
---
- テストメソッド名は日本語で「何をテストしているか」を明記すること（例: `猫一覧が正しく取得できること`）
- AAA パターン（Arrange / Act / Assert）を必ず使用すること
- 各セクションをコメントで区切ること（`// Arrange` など）
- InMemory データベースを使用し、テストごとに一意のDB名を付けること
```

<details>
<summary>English translation of the file above</summary>

```markdown
---
applyTo: "**/*.Tests/**"
---
- Name test methods in Japanese, stating clearly what is being tested (for example `猫一覧が正しく取得できること`, "the cat list can be retrieved correctly")
- Always use the AAA pattern (Arrange / Act / Assert)
- Separate each section with a comment (`// Arrange` and so on)
- Use the InMemory database and give every test a unique DB name
```

</details>

> **Note:** the file name is free as long as it matches the `*.instructions.md` pattern. Here the `views` and `tests` prefixes make the purpose clear.

### Check the effect

Try the following (no files will be changed):

> 💬 **Ask mode**

**Original (Japanese):**

```
Views/Cats/Index.cshtml に猫の一覧テーブルを表示するビューを作成してください。
```

**English equivalent:**

```
Create a view at Views/Cats/Index.cshtml that displays a table listing the cats.
```

The `applyTo: "**/*.cshtml"` of `views.instructions.md` is applied automatically, so the output should be a view using Bootstrap 5 and Japanese labels. You will create the actual file in Step 6.

> **Note:** `applyTo` takes a glob pattern. You can place multiple `.instructions.md` files, and they are applied automatically when you operate on files matching the pattern.

---

## 3. Reusable prompts (`.prompt.md`)

Instead of writing the same instructions over and over, you can save a **templated prompt**.

### Create a CRUD controller generation prompt

Create `.github/prompts/create-crud-controller.prompt.md`:

```markdown
---
description: "Entity に対する CRUD コントローラーを生成する"
---
# CRUD コントローラー生成

以下の Entity に対して、ASP.NET Core MVC の CRUD コントローラーを作成してください。

## 対象 Entity
{{input}}

## 要件
- AppDbContext を DI で受け取る
- 全アクションを async/await で実装する
- Index（一覧）、Details（詳細）、Create（作成 GET/POST）、Edit（編集 GET/POST）、Delete（削除 GET/POST）を実装する
- POST アクションでは ModelState.IsValid を検証する
- 例外処理は try-catch で囲み、エラー時はログ出力する
```

<details>
<summary>English translation of the file above</summary>

```markdown
---
description: "Generate a CRUD controller for an entity"
---
# CRUD controller generation

Create an ASP.NET Core MVC CRUD controller for the following entity.

## Target entity
{{input}}

## Requirements
- Receive AppDbContext through DI
- Implement every action with async/await
- Implement Index (list), Details, Create (GET/POST), Edit (GET/POST) and Delete (GET/POST)
- Validate ModelState.IsValid in the POST actions
- Wrap exception handling in try-catch and log on error
```

</details>

### How to use it

Type `/` in Copilot Chat and your saved prompts appear as candidates. Select `create-crud-controller`, enter the entity name, and you get a consistent controller:

> **Note:** If the prompt you created does not appear when you type `/`, confirm that the `.github/prompts` folder sits directly under the `app/` folder. In this workshop `app/` is the workspace root. If you create `.github/prompts` under a C# project folder such as `MeowWorld/`, the prompt may not show up as a candidate.

> 💬 **Ask mode** (verification only. The real generation happens in Step 6)

```
/create-crud-controller Cat（Id, Name, Age, Breed, Description, CreatedAt プロパティを持つ）
```

English equivalent:

```
/create-crud-controller Cat (has the properties Id, Name, Age, Breed, Description, CreatedAt)
```

> **Tip:** `.prompt.md` files can be shared across the whole team, enabling a practice of "use this prompt when adding this kind of feature". You will put this prompt to real use in Step 6.

---

## Summary

| Level | File | When it applies |
|-------|------|-----------------|
| Whole repository | `.github/copilot-instructions.md` | Always, automatically |
| File scope | `.instructions.md` (applyTo) | Automatically when operating on files matching the pattern |
| Reusable prompt | `.prompt.md` | When the user invokes it explicitly |

Combining these means:
- **No more effort spent writing the rules into every prompt**
- **Everyone on the team gets code generation of the same quality**
- **Token savings too** (covered in detail in Step 7)

---

[Previous - Create the Project](../3_CreateProject/README_EN.md) | [Next - Implement the DB Layer](../5_ImplementDBLayer/README_EN.md)
