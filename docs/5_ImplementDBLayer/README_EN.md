# Implement the DB Layer

🌐 **Language:** [日本語](README_JA.md) | **English**

[Previous - Custom Instructions](../4_CustomInstructions/README_EN.md) | [Next - Implement MVC + Vision](../6_ImplementMVC/README_EN.md)

In this step you make use of Agent mode and Custom Instructions to implement the Entity Framework Core model, the DbContext and the migrations.

> **✨ This is the step where you feel the effect of Step 4:** the code the Agent generates here automatically picks up the Custom Instructions you configured in Step 4 (Japanese comments, file-scoped namespaces, primary constructors and so on). Confirm that "code following the conventions comes out even though I specified nothing".

---

## Goals

- Create the `Cat` entity and `AppDbContext`
- Configure the SQLite connection string
- Run the migrations and insert the seed data
- Experience the flow of the Agent repairing errors automatically when they occur

---

## Implement in Agent mode

### 1. Create the model and the DbContext

> 🕵️ **Agent mode**

**Original (Japanese):**

```
MeowWorld プロジェクトに以下を実装してください：

1. Models/Cat.cs - 猫エンティティ
   - Id (int, PK)
   - Name (string, 必須, 最大50文字)
   - Age (int, 0以上)
   - Breed (string, 必須, 最大50文字)
   - Description (string?, 任意, 最大500文字)
   - CreatedAt (DateTime, デフォルト=現在日時)

2. Data/AppDbContext.cs - DbContext
   - DbSet<Cat> Cats プロパティ
   - OnModelCreating でシードデータ5件を追加

3. appsettings.json に SQLite の接続文字列を追加
   - "ConnectionStrings": { "DefaultConnection": "Data Source=meowworld.db" }

4. Program.cs に DbContext の DI 登録を追加

ビルドが通ることを確認してください。
```

**English equivalent:**

```
Implement the following in the MeowWorld project:

1. Models/Cat.cs - the cat entity
   - Id (int, PK)
   - Name (string, required, max 50 characters)
   - Age (int, 0 or greater)
   - Breed (string, required, max 50 characters)
   - Description (string?, optional, max 500 characters)
   - CreatedAt (DateTime, default = current date and time)

2. Data/AppDbContext.cs - the DbContext
   - A DbSet<Cat> Cats property
   - Add 5 seed records in OnModelCreating

3. Add the SQLite connection string to appsettings.json
   - "ConnectionStrings": { "DefaultConnection": "Data Source=meowworld.db" }

4. Register the DbContext for DI in Program.cs

Confirm that the build passes.
```

> **Key point:** because the `.github/copilot-instructions.md` you configured in Step 4 is applied automatically, instructions such as "use a file-scoped namespace" or "write Japanese comments" are unnecessary.

### Observe how the Agent works

The Agent works roughly like this:

1. Creates and edits the necessary files
2. Runs `dotnet build`
3. Analyses and fixes automatically if there are errors
4. Builds again and confirms success

> **Note:** Build errors are a natural occurrence. Watch how the Agent fixes them automatically. This is a demonstration of Agent autonomy that "you cannot experience in Ask mode".

---

### 2. Run the migration

Once the build succeeds, create and apply the migration:

> 🕵️ **Agent mode**

**Original (Japanese):**

```
EF Core のマイグレーションを作成して適用してください。
マイグレーション名は "InitialCreate" で。
```

**English equivalent:**

```
Create and apply an EF Core migration.
Name the migration "InitialCreate".
```

The Agent runs the following:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> **If an error appears:** the `dotnet-ef` tool may not be installed. Leave it to the Agent and it will run `dotnet tool install --global dotnet-ef` for you.

---

### 3. Verify the behaviour

> 🕵️ **Agent mode**

**Original (Japanese):**

```
Program.cs にデータベースの自動マイグレーション適用コードを追加して、
アプリ起動時に DB が存在しなければ自動作成されるようにしてください。
その後 dotnet run で起動して動作確認してください。
```

**English equivalent:**

```
Add code to Program.cs that applies database migrations automatically,
so that the DB is created automatically at application startup if it does not exist.
Then start the app with dotnet run and verify it works.
```

---

## Expected deliverables

After this step the following files should exist:

```text
MeowWorld/
├── Models/
│   └── Cat.cs
├── Data/
│   └── AppDbContext.cs
├── Migrations/
│   ├── YYYYMMDDHHMMSS_InitialCreate.cs
│   └── AppDbContextModelSnapshot.cs
├── appsettings.json  (connection string added)
├── Program.cs        (DI registration + automatic migration added)
└── meowworld.db      (the SQLite file)
```

---

## Reference: an example of the expected code

Compare it with the code the Agent generated and check that the Custom Instructions were applied correctly:

<details>
<summary>Example of Models/Cat.cs (click to expand)</summary>

```csharp
namespace MeowWorld.Models;

/// <summary>
/// 猫のエンティティ
/// </summary>
public class Cat
{
    public int Id { get; set; }

    /// <summary>猫の名前</summary>
    [Required, MaxLength(50)]
    public required string Name { get; set; }

    /// <summary>年齢</summary>
    [Range(0, 30)]
    public int Age { get; set; }

    /// <summary>品種</summary>
    [Required, MaxLength(50)]
    public required string Breed { get; set; }

    /// <summary>説明（任意）</summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>登録日時</summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
```

The Japanese XML comments above mean: "The cat entity", "The cat's name", "Age", "Breed", "Description (optional)", "Registration date and time".

**What to check:**
- [x] File-scoped namespace
- [x] Japanese XML comments
- [x] The `required` keyword
- [x] Nullable types (`string?`)

</details>

<details>
<summary>Example of Data/AppDbContext.cs (click to expand)</summary>

```csharp
using Microsoft.EntityFrameworkCore;
using MeowWorld.Models;

namespace MeowWorld.Data;

/// <summary>
/// アプリケーションデータベースコンテキスト
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>猫テーブル</summary>
    public DbSet<Cat> Cats => Set<Cat>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // シードデータ
        modelBuilder.Entity<Cat>().HasData(
            new Cat { Id = 1, Name = "みけ", Age = 3, Breed = "三毛猫", Description = "おとなしい性格", CreatedAt = new DateTime(2024, 1, 1) },
            new Cat { Id = 2, Name = "くろ", Age = 5, Breed = "黒猫", Description = "甘えん坊", CreatedAt = new DateTime(2024, 1, 1) },
            new Cat { Id = 3, Name = "しろ", Age = 2, Breed = "白猫", Description = null, CreatedAt = new DateTime(2024, 1, 1) },
            new Cat { Id = 4, Name = "チャチャ", Age = 1, Breed = "茶トラ", Description = "元気いっぱい", CreatedAt = new DateTime(2024, 1, 1) },
            new Cat { Id = 5, Name = "ソラ", Age = 4, Breed = "ロシアンブルー", Description = "静かな環境が好き", CreatedAt = new DateTime(2024, 1, 1) }
        );
    }
}
```

The class summary means "Application database context", `猫テーブル` means "cat table" and `// シードデータ` means "// seed data".

**Seed data in English:**

| Id | Name (JA) | Name (EN) | Age | Breed (JA) | Breed (EN) | Description (JA) | Description (EN) |
|----|-----------|-----------|-----|------------|------------|------------------|------------------|
| 1 | みけ | Mike | 3 | 三毛猫 | Calico | おとなしい性格 | A quiet temperament |
| 2 | くろ | Kuro | 5 | 黒猫 | Black cat | 甘えん坊 | Loves being pampered |
| 3 | しろ | Shiro | 2 | 白猫 | White cat | (none) | (none) |
| 4 | チャチャ | Chacha | 1 | 茶トラ | Orange tabby | 元気いっぱい | Full of energy |
| 5 | ソラ | Sora | 4 | ロシアンブルー | Russian Blue | 静かな環境が好き | Likes a quiet environment |

> Keep the Japanese values as they are. Seed data is content, not UI chrome, so a language toggle normally does not translate it. If you do want the seed data to switch languages too, see the "Translating data, not just labels" section of [docs/Localization/README.md](../Localization/README.md).

**What to check:**
- [x] Primary constructor
- [x] Japanese seed data

</details>

---

## Fixing errors with /fix

Besides the automatic build-error repair in Agent mode, you can also use the **`/fix` command**.

Open a file that shows a red squiggle in the editor and type the following in Copilot Chat:

```
/fix
```

That alone gives you a proposal that fixes the errors in the current file.

> **When to use which:**
> - An error during Agent mode -> the Agent fixes it automatically (no intervention needed)
> - An error while editing manually -> `/fix` is convenient

---

[Previous - Custom Instructions](../4_CustomInstructions/README_EN.md) | [Next - Implement MVC + Vision](../6_ImplementMVC/README_EN.md)
