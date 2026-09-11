# Unit Testing

🌐 **Language:** [日本語](README_JA.md) | **English**

[Previous - Token Savings and Context Management](../7_TokenManagement/README_EN.md) | [Next - Custom Agent](../9_CustomAgent/README_EN.md)

In this step you generate and run unit tests using Copilot's **`/tests` command** and Agent mode. You leave everything from creating the test project to implementing the tests to the Agent.

> **Time:** about 25 minutes
> **You will end with:** a `MeowWorld.Tests` project whose tests all pass
> **New Copilot skills:** `/tests`, and watching the Agent repair a failing test


---

## 1. Create the test project

Switch Copilot Chat to **Agent mode** and enter the following:

> 🕵️ **Agent mode**

**Original (Japanese):**

```
MeowWorld に対する xUnit テストプロジェクトを MeowWorld.Tests として作成してください。
InMemory データベースプロバイダー（Microsoft.EntityFrameworkCore.InMemory）を使います。
ソリューションファイルに追加し、ビルドが通ることを確認してください。
```

**English equivalent:**

```
Create an xUnit test project for MeowWorld named MeowWorld.Tests.
Use the InMemory database provider (Microsoft.EntityFrameworkCore.InMemory).
Add it to the solution file and confirm the build passes.
```

The Agent runs the following:
- `dotnet new xunit` to create the test project
- Adds the project reference
- Adds the InMemory package
- Adds it to the solution
- Confirms the build

---

## 2. Generate tests with the `/tests` command

### How to use it

Open the file under test in the editor (for example `CatsController.cs`) and type the following in Copilot Chat:

> 🕵️ **Agent mode**

```
/tests
```

That alone generates unit tests for that file.

### What to check

Confirm that the rules you set in `.instructions.md` (`applyTo: "**/*.Tests/**"`) have been applied:

- [ ] Test method names are in Japanese (for example `猫一覧が正しく取得できること`, "the cat list can be retrieved correctly")
- [ ] AAA pattern comment separators (`// Arrange`, `// Act`, `// Assert`)
- [ ] The InMemory database is used
- [ ] A unique DB name per test

> **Note:** `/tests` is a command you type in Copilot Chat (it is not run in the terminal). It also works in Ask mode, but in Agent mode it places the generated test file, builds and runs it automatically.

---

## 3. Enrich the tests in Agent mode

After `/tests` has generated the basic tests, ask the Agent for additional scenarios:

> 🕵️ **Agent mode**

**Original (Japanese):**

```
CatsController のテストに以下のケースを追加してください：
- 存在しない ID で Details を呼んだ場合に NotFound が返ること
- Name が空文字で Create した場合に ModelState エラーになること
- 正常に Edit した後、変更が DB に反映されていること

テストを実行して全件パスすることを確認してください。
```

**English equivalent:**

```
Add the following cases to the CatsController tests:
- Calling Details with a non-existent ID returns NotFound
- Creating with an empty string for Name produces a ModelState error
- After a successful Edit, the change is reflected in the DB

Run the tests and confirm that all of them pass.
```

The Agent does the following:
1. Adds the test cases
2. Runs `dotnet test`
3. Fixes anything that fails, automatically
4. Confirms that all tests pass

---

## 4. Experience automatic repair when a test fails

The real value of Agent mode is in "automatic repair when a test fails".

### Break a test deliberately

Make a small change, such as altering the order of the cats returned by the `Index` action in `CatsController.cs`, and then run the tests:

> 🕵️ **Agent mode**

**Original (Japanese):**

```
dotnet test を実行して。失敗したテストがあれば修正してください。
```

**English equivalent:**

```
Run dotnet test. If any test fails, fix it.
```

The Agent analyses the failure message and proposes a fix to either the test code or the production code.

> **Learning point:** feel how completely the error-fixing flow is automated.

---

## 5. Expected deliverables

```text
MeowWorld.Tests/
├── MeowWorld.Tests.csproj
├── CatsControllerTests.cs  (or Controllers/CatsControllerTests.cs)
└── GlobalUsings.cs
```

> **Note:** the test file name and subfolder structure vary depending on what Copilot generates (it may be generated into `UnitTest1.cs`, for example). What matters is not the file placement but that **the test content satisfies the requirements** and that **`dotnet test` passes completely**.

Run them yourself rather than trusting the Agent's summary:

```bash
# from the workspace root (app/)
dotnet test
```

Expected:

```text
Restore complete (0.4s)
  MeowWorld succeeded (1.2s)
  MeowWorld.Tests succeeded (0.9s)

Test summary: total: 5, failed: 0, succeeded: 5, skipped: 0, duration: 1.4s
Build succeeded in 3.1s
```

The older formatter prints it as:

```text
Passed!  - Failed:     0, Passed:     5, Skipped:     0, Total:     5
```

Either is fine. What matters is `failed: 0` and a total greater than zero.

> **`total: 0` is a failure, not a pass.** A run that discovers no tests exits 0 and looks green. If you see a total of zero, the test project is not referencing xUnit correctly or the test class is not `public`.

#### Common test failures and what they mean

| Failure | Cause | Fix |
|---------|-------|-----|
| `The type or namespace name 'CatsController' could not be found` | The test project has no reference to `MeowWorld` | `dotnet add MeowWorld.Tests reference MeowWorld` |
| `No suitable constructor found for CatsController` | The controller's constructor signature differs from what the test assumes (often a missing `ILogger`) | Open `CatsController.cs` and match the test helper to the real signature |
| Tests pass alone but fail together | Two tests share an InMemory database name | Give each a unique name; `nameof(TheTestMethod)` is the usual trick |
| `Sequence contains no elements` | The test asserts on seed data that only exists in the real SQLite DB | InMemory starts empty. The test must add its own data in Arrange |
| `Object reference not set` on `viewResult.Model` | The action returned a redirect or NotFound, not a view | Assert the actual result type first to see what came back |

Rows 3 and 5 are the ones worth internalising. A shared InMemory name is the classic flaky-test cause in EF Core, and it is exactly what the `tests.instructions.md` rule about unique DB names exists to prevent.

---

## Reference: an example test

<details>
<summary>Example of CatsControllerTests.cs (click to expand)</summary>

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MeowWorld.Controllers;
using MeowWorld.Data;
using MeowWorld.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeowWorld.Tests;

/// <summary>
/// CatsController のユニットテスト
/// </summary>
public class CatsControllerTests
{
    private static AppDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new AppDbContext(options);
    }

    private static CatsController CreateController(AppDbContext context)
    {
        return new CatsController(context, NullLogger<CatsController>.Instance);
    }

    [Fact]
    public async Task 猫一覧が正しく取得できること()
    {
        // Arrange
        using var context = CreateContext(nameof(猫一覧が正しく取得できること));
        context.Cats.AddRange(
            new Cat { Name = "みけ", Age = 3, Breed = "三毛猫" },
            new Cat { Name = "くろ", Age = 5, Breed = "黒猫" }
        );
        await context.SaveChangesAsync();
        var controller = CreateController(context);

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Cat>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task 存在しないIDでNotFoundが返ること()
    {
        // Arrange
        using var context = CreateContext(nameof(存在しないIDでNotFoundが返ること));
        var controller = CreateController(context);

        // Act
        var result = await controller.Details(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
```

**The Japanese identifiers above mean:**

| Japanese | English |
|----------|---------|
| `CatsController のユニットテスト` | Unit tests for CatsController |
| `猫一覧が正しく取得できること()` | `TheCatListCanBeRetrievedCorrectly()` |
| `存在しないIDでNotFoundが返ること()` | `NotFoundIsReturnedForANonExistentId()` |

> C# allows Unicode identifiers, so Japanese test method names compile and run fine. They keep the test report readable for a Japanese-speaking team. If your team reads English, switch the convention in `.github/instructions/tests.instructions.md` rather than renaming tests by hand.

</details>

---

## Step 8 completion checklist

- [ ] `MeowWorld.Tests/` exists and is referenced by the solution
- [ ] The test project references `MeowWorld` and `Microsoft.EntityFrameworkCore.InMemory`
- [ ] `dotnet test` reports `failed: 0` with a total greater than zero
- [ ] Test method names are Japanese
- [ ] Each test has `// Arrange`, `// Act` and `// Assert` comments
- [ ] Each test uses a unique InMemory database name
- [ ] You deliberately broke something and watched the Agent repair it

> **On skipping this step:** the test conventions here come from `.github/instructions/tests.instructions.md`, which is Step 4 content. If you skip Step 8 for time, you lose one demonstration of path-scoped instructions but nothing that Step 9 depends on structurally. Step 9's Custom Agent does require `dotnet test` to pass as part of its Definition of Done, so if you skip this, expect the Agent in Step 9 to create the test project itself.

---

## Summary

| Feature | What you experienced |
|---------|----------------------|
| `/tests` | Automatic test generation just by opening a file |
| Agent mode | Test project creation, execution and repair end to end |
| `.instructions.md` | The test naming convention (Japanese) and the AAA pattern applied automatically |
| Automatic repair | The Agent runs test failure -> cause analysis -> fix -> re-run autonomously |

---

[Previous - Token Savings and Context Management](../7_TokenManagement/README_EN.md) | [Next - Custom Agent](../9_CustomAgent/README_EN.md)
