# Create the Project (Agent mode)

🌐 **Language:** [日本語](README_JA.md) | **English**

[Previous - Before Getting Started](../2_BeforeGettingStarted/README_EN.md) | [Next - Custom Instructions](../4_CustomInstructions/README_EN.md)

In this step you use GitHub Copilot's Agent mode to go all the way from creating an ASP.NET Core MVC project, through introducing the SQLite packages, to confirming that it builds.

---

## Set up the project in Agent mode

### Prerequisites

- The .NET 10 SDK is installed
- Visual Studio 2026 or VS Code (C# Dev Kit) is installed
- "Agent" can be selected in the Copilot Chat mode switcher

### Procedure

1. Create an `app` folder at the repository root and **open the `app/` folder in your IDE as the workspace**.
   - **VS Code:** `File > Open Folder` -> select the `app` folder
   - **Visual Studio 2026:** "Open a local folder" -> select the `app` folder

   ```bash
   mkdir app
   ```

   > **Why make `app/` the workspace?**
   > - In real development, **project root = workspace root** is the standard
   > - The hands-on material (`docs/`) does not leak into Copilot's context, so completion accuracy improves
   > - `.github/copilot-instructions.md` ends up in its natural position directly under the workspace root
   > - Every file path you create in later steps matches the form used in real work

2. Open Copilot Chat and select **"Agent"** from the mode dropdown

3. Enter and send the following prompt:

> 🕵️ **Agent mode**

**Original (Japanese):**

```
.NET 10 の ASP.NET Core MVC プロジェクト "MeowWorld" を新規作成してください。
SQLite 関連の NuGet パッケージ（Microsoft.EntityFrameworkCore.Sqlite、
Microsoft.EntityFrameworkCore.Design、Microsoft.EntityFrameworkCore.Tools）を追加し、
ビルドが通ることを確認してください。
```

**English equivalent:**

```
Create a new .NET 10 ASP.NET Core MVC project named "MeowWorld".
Add the SQLite-related NuGet packages (Microsoft.EntityFrameworkCore.Sqlite,
Microsoft.EntityFrameworkCore.Design, Microsoft.EntityFrameworkCore.Tools)
and confirm that the build passes.
```

4. When the Agent asks for permission to run a terminal command, **check the content** and press "Continue"

5. The Agent runs the following in order:
   - `dotnet new mvc` to create the project
   - `dotnet add package` to add the NuGet packages
   - `dotnet build` to confirm the build

> **Tip:** If an error occurs while the Agent is running, the Agent automatically analyses it and tries to fix it. This is one of the big advantages of Agent mode.

### Verification

Once the build succeeds, confirm that the application starts with the following command (or by running the debugger):

```bash
cd MeowWorld
dotnet run
```

If a browser opens and the default ASP.NET Core page appears, you have succeeded.

![The default ASP.NET Core page at http://localhost:5244 showing the MeowWorld brand, Home and Privacy links, and a "Welcome" heading](./images/init-webapp.png)

> A full text description of this screenshot is in [Image Analysis - Initial Web App](../ImageAnalysis/init-webapp.md).

Stop the process once you have confirmed it starts.

---

## Learning point: the characteristics of Agent mode

Let us review the characteristics of Agent mode that you experienced in this step:

| Characteristic | What you experienced in this step |
|----------------|-----------------------------------|
| **Autonomous task execution** | One prompt completed everything from project creation to build verification |
| **Terminal operation** | The Agent ran the `dotnet` commands directly |
| **Automatic error recovery** | When a build error appears, the cause is analysed and fixed automatically |
| **Confirmation-based progress** | User approval is requested before each command runs |

> **Difference from Ask mode:** Ask mode only tells you "the steps for the commands"; you have to run them yourself. In Agent mode, Copilot handles the execution as well.

---

## Appendix: setting up manually

If you want to proceed manually without Agent mode, run the following commands in order:

<details>
<summary>Manual setup procedure (click to expand)</summary>

```bash
# Run with the app/ folder opened as the workspace

# 1. Create the project
dotnet new mvc -n MeowWorld

# 2. Add the NuGet packages
cd MeowWorld
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools

# 3. Confirm the build
dotnet build
```

</details>

---

[Previous - Before Getting Started](../2_BeforeGettingStarted/README_EN.md) | [Next - Custom Instructions](../4_CustomInstructions/README_EN.md)
