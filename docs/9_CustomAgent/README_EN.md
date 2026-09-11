# Custom Agent & Skill

🌐 **Language:** [日本語](README_JA.md) | **English**

[Previous - Unit Testing](../8_UnitTesting/README_EN.md) | [Next - Summary](../10_LessonsLearned/README_EN.md)

In this step you create a **Custom Agent (`.agent.md`)** and a **Skill (`SKILL.md`)** to build an AI assistant dedicated to your project.

The comparison in this hands-on is not about "can it follow the conventions"; the aim is to experience **the differences that matter in real work**.

- Keep the same guardrails (the Custom Instructions from Step 4) in place during the comparison
- Observe the aspects where a difference still appears: reproducibility, accountability, less rework
- Confirm quantitatively that it is "easier to improve", not that it "always gets better"

---

## What a Custom Agent is

A Custom Agent is a "specialised AI assistant" defined in a `.agent.md` file.

| Item | Custom Instructions | Custom Agent | Skill |
|------|--------------------|--------------|-------|
| Definition file | `.github/copilot-instructions.md` | `.github/agents/xxx.agent.md` | `.github/skills/xxx/SKILL.md` |
| Scope | Every chat | Only when invoked with `@agentname` | When the Agent judges it necessary |
| Purpose | Coding conventions and project information | Role, work process, completion criteria | A package of domain knowledge and reference information |
| How it is referenced | Automatically | Explicitly invoked with `@agentname` | The Agent decides autonomously by looking at the description |

**When to use which:**
- Custom Instructions = "rules everyone must follow" (always applied)
- Custom Agent = "a team member with a defined role and way of working" (explicitly invoked)
- Skill = "a bookshelf of specialist books" (the Agent pulls one out when it needs to)

---

## 1. Generate the architecture guide

First, have Copilot generate the project information that the Custom Agent will refer to.

In Copilot Chat (Agent mode):

> 🕵️ **Agent mode**

**Original (Japanese):**

```
#codebase を分析して、docs/architecture-guide.md を作成してください。
以下を含めてください：
- プロジェクト構成（ディレクトリ・ファイル構造）
- 使用技術スタック
- 命名規約（実際のコードから読み取ったもの）
- データモデルの関係
- 新機能追加時の標準パターン（コントローラー、ビュー、テストの配置場所）
```

**English equivalent:**

```
Analyse #codebase and create docs/architecture-guide.md.
Include the following:
- Project structure (directory and file layout)
- The technology stack in use
- Naming conventions (read from the actual code)
- Relationships in the data model
- The standard pattern for adding a new feature (where controllers, views and tests go)
```

This file becomes the knowledge base for the Custom Agent.

---

## 2. Before: measure the baseline (without a Custom Agent)

First implement **without using a Custom Agent** and measure the baseline.

> 🕵️ **Agent mode**

**Original (Japanese):**

```
猫のお気に入り登録機能を追加してください。

要件:
- Cat に対してお気に入り状態（IsFavorite: bool）を管理できること
- 一覧画面（Views/Cats/Index.cshtml）でお気に入りの切り替え操作ができること
- 既存の CRUD 機能を壊さないこと
- テストを追加し、dotnet test が通ること

最後に以下を出力してください:
1. 変更したファイル一覧
2. 実行した確認コマンドと結果
3. 既知のリスク（あれば）
```

**English equivalent:**

```
Add a favourites feature for cats.

Requirements:
- A favourite state (IsFavorite: bool) can be managed for a Cat
- The favourite state can be toggled on the list screen (Views/Cats/Index.cshtml)
- The existing CRUD features are not broken
- Tests are added and dotnet test passes

At the end, output the following:
1. The list of files you changed
2. The verification commands you ran and their results
3. Known risks (if any)
```

### Metrics to record for the baseline

| Metric | How to record it |
|--------|------------------|
| First-pass acceptance rate | Did it meet the requirements with no additional instructions? (Yes/No) |
| Number of additional inputs | How many extra prompts until completion |
| Explicitness of verification | Are the build/test results reported? |
| Change traceability | Is the list of changed files stated explicitly? |
| Explicitness of risk | Are known constraints or unresolved points reported? |

> **Important:** the aim here is not "to look for failures" but **to take measurements for comparison**.

### Comparison sheet (recommended)

To compare Before and After on the same basis, this sheet makes the change visible.

| Metric | Before (Agent only) | After (Custom Agent) | Criterion |
|--------|--------------------|--------------------|-----------|
| First-pass acceptance rate | Y / N | Y / N | Requirements met with no additional prompts, and `dotnet build` and `dotnet test` succeed |
| Number of additional inputs | count | count | Number of extra prompts sent (+1 per correction request) |
| Explicitness of verification report | ○ / ✕ | ○ / ✕ | Both the commands run and their results are reported |
| Change traceability | ○ / ✕ | ○ / ✕ | The list of changed files is presented |
| Explicitness of risk | ○ / ✕ | ○ / ✕ | Outstanding items and known risks are stated (or "none") |

---

## 3. Create the Custom Agent (writing down role and completion criteria)

### Create `.github/agents/meowworld-dev.agent.md`

````markdown
---
description: "MeowWorld プロジェクトの機能開発を担当するエージェント。新機能の設計・実装・テストを一貫して行う。"
tools:
  - search/codebase
  - terminal
  - file
---
# MeowWorld 開発エージェント

あなたは MeowWorld（猫カフェ管理システム）の専任開発者です。

## プロジェクト知識

#file:docs/architecture-guide.md を必ず参照して、プロジェクトの構造と規約を理解した上で作業してください。

## 新機能の開発プロセス

新機能のリクエストを受けたら、以下の順序で作業してください：

### Step 1: 設計
- 必要なモデル変更を特定する
- 既存のモデル（Cat.cs）との関係を明確にする
- マイグレーションが必要か判断する

### Step 2: 実装
- モデル → DbContext → コントローラー → ビュー の順で実装する
- 既存コードのパターンに厳密に従う：
  - コントローラーは `Controllers/` 直下
  - ビューは `Views/{ControllerName}/` 配下
  - file-scoped namespace を使用
  - 非同期メソッドを使用
  - 日本語の XML ドキュメントコメント

### Step 3: テスト
- テストクラスは `MeowWorld.Tests/Controllers/` に配置
- テストメソッド名は日本語
- AAA パターン + InMemory DB
- 正常系 + 異常系（最低 3 ケース）を含める

### Step 4: 確認
- `dotnet build` でビルド確認
- `dotnet test` で全テストパス確認
- 既存テストが壊れていないか確認

## 制約
- 既存機能を壊さないこと
- 不必要なパッケージを追加しないこと
- 一度に複数のマイグレーションを作らないこと

## 完了条件（Definition of Done）
- ビルド成功（`dotnet build`）
- テスト成功（`dotnet test`）
- 変更ファイル一覧を提示
- 要件ごとの対応状況を提示（満たした/未対応）

## 返信フォーマット
最終返信は以下の順序で簡潔に報告すること：
1. 実装サマリ（3-5行）
2. 変更ファイル一覧
3. 実行した検証コマンドと結果
4. 未対応事項またはリスク（なければ「なし」）
````

<details>
<summary>English translation of the agent file above (for reading, not for pasting)</summary>

````markdown
---
description: "The agent responsible for feature development on the MeowWorld project. It handles design, implementation and testing of new features end to end."
tools:
  - search/codebase
  - terminal
  - file
---
# MeowWorld development agent

You are the dedicated developer for MeowWorld (a cat cafe management system).

## Project knowledge

Always refer to #file:docs/architecture-guide.md and work with an understanding of the project structure and conventions.

## The development process for a new feature

When you receive a request for a new feature, work in the following order:

### Step 1: Design
- Identify the model changes that are needed
- Clarify the relationship with the existing model (Cat.cs)
- Decide whether a migration is needed

### Step 2: Implementation
- Implement in the order model -> DbContext -> controller -> view
- Follow the patterns of the existing code strictly:
  - Controllers go directly under `Controllers/`
  - Views go under `Views/{ControllerName}/`
  - Use file-scoped namespaces
  - Use asynchronous methods
  - Japanese XML documentation comments

### Step 3: Tests
- Place test classes in `MeowWorld.Tests/Controllers/`
- Test method names in Japanese
- AAA pattern + InMemory DB
- Include both happy path and error cases (at least 3 cases)

### Step 4: Verification
- Confirm the build with `dotnet build`
- Confirm all tests pass with `dotnet test`
- Confirm no existing test is broken

## Constraints
- Do not break existing features
- Do not add unnecessary packages
- Do not create multiple migrations at once

## Definition of Done
- The build succeeds (`dotnet build`)
- The tests succeed (`dotnet test`)
- The list of changed files is presented
- The status of each requirement is presented (met / not addressed)

## Reply format
Report the final reply concisely in the following order:
1. Implementation summary (3-5 lines)
2. List of changed files
3. The verification commands run and their results
4. Outstanding items or risks (write "none" if there are none)
````

</details>

---

## 4. After: implement the same feature with the Custom Agent and compare

Either revert the earlier change or work on a separate branch, then add the same feature using the Custom Agent.

In Copilot Chat, type `@meowworld-dev` to invoke the Custom Agent:

> 🕵️ **Agent mode** - `@meowworld-dev`

**Original (Japanese):**

```
@meowworld-dev 猫のお気に入り登録機能を追加してください。
ユーザーがお気に入りボタンを押すと、その猫がお気に入りリストに追加されます。

要件:
- Cat に対してお気に入り状態（IsFavorite: bool）を管理できること
- 一覧画面（Views/Cats/Index.cshtml）でお気に入りの切り替え操作ができること
- 既存の CRUD 機能を壊さないこと
- テストを追加し、dotnet test が通ること
```

**English equivalent:**

```
@meowworld-dev Add a favourites feature for cats.
When the user presses the favourite button, that cat is added to the favourites list.

Requirements:
- A favourite state (IsFavorite: bool) can be managed for a Cat
- The favourite state can be toggled on the list screen (Views/Cats/Index.cshtml)
- The existing CRUD features are not broken
- Tests are added and dotnet test passes
```

> **Key point for the comparison:** keep the implementation requirements identical to Before. In contrast, do not write the list of changed files, the verification results or the risk report into the prompt; leave those to the Custom Agent's completion criteria and reply format.

### Comparing Before and After (the difference to look for in this chapter)

| Aspect | Before (Agent only) | After (Custom Agent) |
|--------|--------------------|--------------------|
| First-pass acceptance rate | Varies easily with the instruction | Tends to be stable because completion criteria exist |
| Number of additional inputs | Additional confirmations tend to increase | The reply format tends to reduce round trips |
| Quality of the verification report | Whether it ran things tends to be reported vaguely | Build/test results are easy to state explicitly |
| Change traceability | The granularity of change descriptions varies | Easy to trace with the list of changed files |
| Fit for team operation | Tends to depend on the individual | The DoD and the report format can be shared |

> **Learning point:** even with the guardrails kept in place, a Custom Agent can fix the "role", the "completion criteria" and the "report format", which makes a difference to **the reproducibility and accountability you need in real work**.

---

## 5. Externalise knowledge with a Skill

### The problem with `#file` references

The `.agent.md` you created in section 3 refers to the architecture guide with `#file:docs/architecture-guide.md`. That approach has the following problems:

| Problem | Explanation |
|---------|-------------|
| **Always loaded** | Every time you invoke the Agent, the referenced file is read into the context in full |
| **Token consumption** | Even for a simple question, unnecessary knowledge consumes tokens |
| **Scalability** | The more reference files there are, the higher the Agent's startup cost |

A **Skill** solves this problem. The Agent looks at the `description` field and **decides autonomously whether this knowledge is needed for the current question**, loading the content only when it is.

### What a Skill is

```text
.github/skills/
└── meowworld-patterns/
    └── SKILL.md          <- description + the knowledge itself
```

- **description (YAML front matter)**: the clue the Agent uses to decide whether to use this Skill
- **Body**: the actual knowledge (the content the Agent refers to)

### Create the Skill

Create `.github/skills/meowworld-patterns/SKILL.md`:

````markdown
---
description: "MeowWorld プロジェクトのアーキテクチャパターン、ファイル配置規約、命名規則、新機能追加手順を提供する。新しいコントローラー・モデル・ビュー・テストの追加時に参照する。"
---
# MeowWorld アーキテクチャパターン

## プロジェクト構成

```text
MeowWorld/
├── Controllers/       ← コントローラー（{Entity}Controller.cs）
├── Models/            ← エンティティ・ビューモデル
├── Data/              ← DbContext・マイグレーション
├── Views/
│   ├── {Controller}/  ← コントローラー対応ビュー
│   └── Shared/        ← レイアウト・パーシャル
└── wwwroot/           ← 静的ファイル
```

## 技術スタック
- .NET 10 / ASP.NET Core MVC
- Entity Framework Core + SQLite
- xUnit + Moq（テスト）
- Bootstrap 5（UI）

## 命名規約
- エンティティ: PascalCase 単数形（`Cat`, `Favorite`）
- コントローラー: `{Entity}Controller`（複数形でない）
- ビューフォルダ: コントローラー名から `Controller` を除いた名前
- テストクラス: `{Controller名}Tests`
- テストメソッド: 日本語（「猫一覧が正しく取得できること」）

## 新機能追加パターン

### 1. 新エンティティの追加
1. `Models/{Entity}.cs` を作成
2. `Data/AppDbContext.cs` に `DbSet<{Entity}>` を追加
3. マイグレーション作成・適用
4. `Controllers/{Entity}Controller.cs` に CRUD 実装
5. `Views/{Entity}/` に Index, Details, Create, Edit, Delete ビュー作成
6. `MeowWorld.Tests/Controllers/{Entity}ControllerTests.cs` にテスト

### 2. 既存エンティティへのリレーション追加
1. モデルにナビゲーションプロパティ追加
2. `OnModelCreating` でリレーション設定
3. マイグレーション作成・適用
4. 関連コントローラーにアクション追加

## コーディング規約
- file-scoped namespace
- 非同期メソッド（async/await）
- プライマリコンストラクタ（DI 用）
- 日本語 XML ドキュメントコメント
- `required` / nullable 型の適切な使用
````

<details>
<summary>English translation of the Skill file above (for reading, not for pasting)</summary>

````markdown
---
description: "Provides the architecture patterns, file placement conventions, naming rules and new-feature procedure for the MeowWorld project. Refer to it when adding a new controller, model, view or test."
---
# MeowWorld architecture patterns

## Project structure

```text
MeowWorld/
├── Controllers/       <- controllers ({Entity}Controller.cs)
├── Models/            <- entities and view models
├── Data/              <- DbContext and migrations
├── Views/
│   ├── {Controller}/  <- views matching a controller
│   └── Shared/        <- layouts and partials
└── wwwroot/           <- static files
```

## Technology stack
- .NET 10 / ASP.NET Core MVC
- Entity Framework Core + SQLite
- xUnit + Moq (testing)
- Bootstrap 5 (UI)

## Naming conventions
- Entity: PascalCase singular (`Cat`, `Favorite`)
- Controller: `{Entity}Controller` (not plural)
- View folder: the controller name with `Controller` removed
- Test class: `{ControllerName}Tests`
- Test methods: Japanese (for example 「猫一覧が正しく取得できること」)

## Patterns for adding a feature

### 1. Adding a new entity
1. Create `Models/{Entity}.cs`
2. Add `DbSet<{Entity}>` to `Data/AppDbContext.cs`
3. Create and apply the migration
4. Implement CRUD in `Controllers/{Entity}Controller.cs`
5. Create the Index, Details, Create, Edit and Delete views in `Views/{Entity}/`
6. Add tests in `MeowWorld.Tests/Controllers/{Entity}ControllerTests.cs`

### 2. Adding a relationship to an existing entity
1. Add the navigation property to the model
2. Configure the relationship in `OnModelCreating`
3. Create and apply the migration
4. Add the action to the related controller

## Coding conventions
- File-scoped namespaces
- Asynchronous methods (async/await)
- Primary constructors (for DI)
- Japanese XML documentation comments
- Appropriate use of `required` and nullable types
````

</details>

---

## 6. Update the Agent to use the Skill

Remove the inline `#file` from `.agent.md` and update it to reference the Skill instead.

### Update `.github/agents/meowworld-dev.agent.md`

````markdown
---
description: "MeowWorld プロジェクトの機能開発を担当するエージェント。新機能の設計・実装・テストを一貫して行う。"
tools:
  - search/codebase
  - terminal
  - file
skills:
  - meowworld-patterns
---
# MeowWorld 開発エージェント

あなたは MeowWorld（猫カフェ管理システム）の専任開発者です。

## 新機能の開発プロセス

新機能のリクエストを受けたら、以下の順序で作業してください：

### Step 1: 設計
- 必要なモデル変更を特定する
- 既存のモデル（Cat.cs）との関係を明確にする
- マイグレーションが必要か判断する

### Step 2: 実装
- モデル → DbContext → コントローラー → ビュー の順で実装する
- プロジェクトのアーキテクチャパターンに厳密に従うこと

### Step 3: テスト
- 正常系 + 異常系（最低 3 ケース）を含める
- 既存テストが壊れていないか確認する

### Step 4: 確認
- `dotnet build` でビルド確認
- `dotnet test` で全テストパス確認

## 制約
- 既存機能を壊さないこと
- 不必要なパッケージを追加しないこと
- 一度に複数のマイグレーションを作らないこと

## 完了条件（Definition of Done）
- ビルド成功（`dotnet build`）
- テスト成功（`dotnet test`）
- 変更ファイル一覧を提示
- 要件ごとの対応状況を提示（満たした/未対応）

## 返信フォーマット
最終返信は以下の順序で簡潔に報告すること：
1. 実装サマリ（3-5行）
2. 変更ファイル一覧
3. 実行した検証コマンドと結果
4. 未対応事項またはリスク（なければ「なし」）
````

> The English reading of this file is the same as the translation in section 3, minus the "Project knowledge" section (replaced by the `skills:` front matter entry) and with Step 2 shortened to "follow the project's architecture patterns strictly".

### The difference from an inline `#file`

| Comparison item | `#file` reference | Skill |
|-----------------|-------------------|-------|
| Context loading | **In full, every time** | **Only when needed** |
| Size of the Agent itself | Bloats by the size of the knowledge | Stays slim |
| Managing several knowledge sources | The `#file` list keeps growing | Just add another Skill |
| Reusability | Specific to that Agent | **Shareable across multiple Agents** |
| Maintenance | Changing the Agent = changing the knowledge | **Updatable independently** |

> **Key point:** let `.agent.md` focus on "what to do (the process)" and separate "what it knows (the knowledge)" into a Skill. This is separation of concerns (SoC) itself.

---

## 7. Verify with the Skill-enabled Agent

Try the same feature addition again with the updated Agent:

> 🕵️ **Agent mode** - `@meowworld-dev`

**Original (Japanese):**

```
@meowworld-dev 猫のお気に入り登録機能を追加してください。
ユーザーがお気に入りボタンを押すと、その猫がお気に入りリストに追加されます。

要件:
- Cat に対してお気に入り状態（IsFavorite: bool）を管理できること
- 一覧画面（Views/Cats/Index.cshtml）でお気に入りの切り替え操作ができること
- 既存の CRUD 機能を壊さないこと
- テストを追加し、dotnet test が通ること
```

**English equivalent:**

```
@meowworld-dev Add a favourites feature for cats.
When the user presses the favourite button, that cat is added to the favourites list.

Requirements:
- A favourite state (IsFavorite: bool) can be managed for a Cat
- The favourite state can be toggled on the list screen (Views/Cats/Index.cshtml)
- The existing CRUD features are not broken
- Tests are added and dotnet test passes
```

**What to check:**
- Is the Agent following the contents of the Skill (file placement, naming conventions)?
- Has `.agent.md` itself become slimmer while the output quality is maintained?
- Is it implementing along the procedure in the Skill's knowledge (the new-feature pattern)?

### Overall comparison

| Aspect | Plain Agent | Custom Agent (`#file`) | Custom Agent + Skill |
|--------|-------------|------------------------|----------------------|
| Consistency of the work process | Depends on the instruction | ◎ (fixed in the Agent) | ◎ (fixed in the Agent) |
| Handling of project knowledge | Searched as needed | △ (`#file` referenced every time) | ◎ (Skill referenced when needed) |
| Token efficiency | Depends on the task | △ (read in full every time) | ◎ (only when needed) |
| Maintainability of the knowledge | - | △ (Agent and knowledge coupled) | ◎ (Skill updated on its own) |
| Knowledge sharing across Agents | - | ✕ (`#file` per agent) | ◎ (the same Skill is referenced) |

---

## 8. Complete the feature using the Custom Agent

Once the Before/After comparison is done, adopt the "favourites feature" generated by the Custom Agent and finish the implementation:

> 🕵️ **Agent mode** - `@meowworld-dev`

**Original (Japanese):**

```
@meowworld-dev ビルドとテストが通ることを確認して、
何か問題があれば修正してください。
```

**English equivalent:**

```
@meowworld-dev Confirm that the build and the tests pass,
and fix anything that is wrong.
```

---

## Going further: ideas for other agents and skills

You can build all sorts of specialist agents and skills with the same mechanism:

| Agent name | Purpose | Skill to combine with it |
|------------|---------|--------------------------|
| `@reviewer` | Feedback from a code review perspective | `review-checklist` |
| `@security` | Checks from a security perspective | `owasp-patterns` |
| `@docs` | Generating API documentation and README files | `documentation-standards` |
| `@migration` | Specialist in DB migrations | `meowworld-patterns` (shared) |

> **Key point:** note that `@migration` shares the `meowworld-patterns` Skill. Once you externalise knowledge as a Skill, several Agents can reference the same knowledge base and maintenance happens in one place.

---

## Summary

| Feature | What you experienced |
|---------|----------------------|
| Custom Agent (`.agent.md`) | Defining an AI assistant dedicated to the project |
| Skill (`SKILL.md`) | Externalising knowledge that the Agent references autonomously |
| `#file` reference vs Skill | Choosing between always-loaded and loaded-when-needed |
| Before/After comparison | Comparing reproducibility, accountability and reduced rework |
| Standardising the development process | Sharing the design -> implement -> test -> verify workflow |

### Looking back at the whole picture from Step 4

```text
Step 4: Custom Instructions    -> applies "rules" automatically to every chat
Step 4: .instructions.md       -> applies "rules per file type" automatically
Step 4: .prompt.md             -> a "template" invoked explicitly
Step 9: .agent.md              -> "a specialist's operating guidelines", invoked explicitly
Step 9: SKILL.md               -> "a bookshelf of expert knowledge" the Agent references autonomously
```

You can see that as the level of abstraction rises, **the instructions humans write decrease and the AI's autonomous judgement increases**.

---

[Previous - Unit Testing](../8_UnitTesting/README_EN.md) | [Next - Summary](../10_LessonsLearned/README_EN.md)
