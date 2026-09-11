# Troubleshooting Guide

🌐 **Language:** [日本語](README_JA.md) | **English**

## Copilot issues

### I cannot select Agent mode
- Check that your Copilot Chat version is up to date
- On VS Code: update the "GitHub Copilot Chat" extension to the latest version
- On VS 2026: check for updates under Tools > Manage Extensions

### Custom Instructions are not being applied
- **VS Code:** check that `github.copilot.chat.codeGeneration.useInstructionFiles` is set to `true` in settings
- Check that the path of `.github/copilot-instructions.md` is correct (inside the `.github/` folder directly under the workspace root)
- Try again after opening a **new chat session** (an existing session may not pick it up)

### The `applyTo` in `.instructions.md` has no effect
- Check that the glob pattern is correct (for example `**/*.cshtml`, `**/*.Tests/**`)
- Check that the file is inside the workspace
- Check that you are **operating on the matching file as an edit target** (just viewing it does not apply the rules)

### The Custom Agent (`@agentname`) does not appear
- Check that a `.agent.md` file exists in the `.github/agents/` directory
- Check for syntax errors in the YAML front matter (the part enclosed by `---`)
- Try restarting Copilot Chat

### `/tests` or `/fix` does not work
- Check that the target file is open in the editor
- Check that the Copilot Chat extension is up to date

---

## Development environment issues

### The SQLite file is locked / inaccessible
- Check that you do not have several instances of the application running
- If you want to delete the DB file (`meowworld.db`) and recreate it, stop the application completely first

### An error appears during migration
- Check that the `dotnet-ef` tool is installed: `dotnet tool list --global`
- If it is not installed: `dotnet tool install --global dotnet-ef`
- Check that the package versions and the target framework match (.NET 10 -> EF Core 10.x, .NET 8 -> EF Core 8.x)

### Build errors
- Re-fetch the dependencies with `dotnet restore`
- Check `<TargetFramework>` in the `.csproj` (`net10.0` or `net8.0`)
- Check that the EF Core package versions are consistent

### Data is not displayed / not saved
- Check the connection string in `appsettings.json`: `"Data Source=meowworld.db"`
- Check that the automatic migration code is present in `Program.cs`
- Check that the `meowworld.db` file has been generated

---

## Bilingual UI (JA/EN toggle) issues

These apply only if you followed [docs/Localization/README.md](../Localization/README.md).

### The language does not change when I click the toggle
- Check that `app.UseRequestLocalization()` is called **before** `app.UseRouting()` in `Program.cs`
- Check that the browser actually received the `.AspNetCore.Culture` cookie (DevTools > Application > Cookies)
- Check that the culture you clicked is listed in `SupportedUICultures`; an unsupported culture silently falls back to the default

### Labels stay in the default language even though the cookie is set
- Check that the `.resx` files use the exact culture suffix: `SharedResource.ja.resx`, not `SharedResource.ja-JP.resx`, unless you also registered `ja-JP`
- Check that the resource file's Build Action is `Embedded Resource`
- Check that `ResourcesPath` in `AddLocalization` matches the folder where the `.resx` files actually live
- A missing key returns the key name itself rather than throwing, so a label showing up as `Nav_Dashboard` means that key is missing from that language's `.resx`

### Japanese characters appear as garbled text
- Save `.resx` and `.cshtml` files as UTF-8
- Check that the response has `charset=utf-8`; a `<meta charset="utf-8">` in `_Layout.cshtml` covers the browser side

---

## General advice

- **When in doubt, ask Copilot first:** paste the error message in Agent mode, or have it reference `#terminalLastCommand`
- **Reset the chat:** if the state gets strange, start a new chat session
- **Rebuild:** clean the state with `dotnet clean` -> `dotnet build`
