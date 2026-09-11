# MeowWorld

The MeowWorld application: an ASP.NET Core MVC + SQLite cat register with an editorial
front page and an instant Japanese/English toggle.

Built alongside the workshop in [`docs/`](../docs/), which teaches you to create it with GitHub
Copilot. The localization design is documented in
[docs/Localization](../docs/Localization/README.md).

---

## Quick start

```bash
git clone https://github.com/MMALI3287/copilot-custom-workshop-dotnet-web.git
cd copilot-custom-workshop-dotnet-web/app/MeowWorld
dotnet run
```

Then open the `http://localhost:<port>` URL it prints. The database is created and seeded
automatically on first run; there is no setup step.

To stop it, press `Ctrl` + `C`.

---

## Requirements

| Requirement | Version | Notes |
|-------------|---------|-------|
| .NET SDK | **8.0** or later | `dotnet --version` to check |
| dotnet-ef | 8.x | Only needed if you change the model. Not needed to run |

Nothing else. No database server, no npm install, no container. SQLite is a single file and the
client-side libraries are vendored under `wwwroot/lib/`.

<details>
<summary>Installing the .NET 8 SDK</summary>

- **Windows / macOS:** download from <https://dotnet.microsoft.com/download/dotnet/8.0>
- **Ubuntu / Debian:** `sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0`
- **macOS with Homebrew:** `brew install dotnet@8`

After installing, open a **new** terminal so `PATH` picks it up.

</details>

<details>
<summary>Running on .NET 10 instead</summary>

This targets `net8.0` because that is what it was built and verified against. To move it to
.NET 10:

1. In `MeowWorld/MeowWorld.csproj` and `MeowWorld.Tests/MeowWorld.Tests.csproj`, change
   `<TargetFramework>net8.0</TargetFramework>` to `net10.0`
2. Bump every `Microsoft.EntityFrameworkCore.*` package from `8.0.11` to the matching `10.x`
3. `dotnet tool update --global dotnet-ef`
4. `dotnet build && dotnet test`

The application code itself needs no changes.

</details>

---

## Running it, step by step

### 1. Restore and build

```bash
cd app
dotnet build
```

Expected: `Build succeeded. 0 Warning(s) 0 Error(s)`

### 2. Run the tests

```bash
dotnet test
```

Expected:

```text
Passed!  - Failed: 0, Passed: 21, Skipped: 0, Total: 21
```

### 3. Start the app

```bash
cd MeowWorld
dotnet run
```

Expected:

```text
Now listening on: http://localhost:5228
Application started. Press Ctrl+C to shut down.
```

Your port will differ. Open whichever URL it prints.

> **If the browser warns about the certificate** you opened the HTTPS URL and the dev certificate
> is not trusted yet. Either run `dotnet dev-certs https --trust` once, or just use the HTTP URL.

> **If the port is already in use**, pass your own: `dotnet run --urls http://localhost:5300`

### 4. Try it

| Action | What to expect |
|--------|----------------|
| Open `/` | The editorial landing page, in Japanese |
| Click **猫一覧** | 5 seeded cats in a table |
| Click **English** in the header | Everything switches to English **instantly, with no page reload** |
| Look at the cat names | Still Japanese. Data is not translated, only the interface |
| Reload the page | Stays in English. The choice is remembered in a cookie |
| Add `?culture=ja` to the URL | Japanese for that one request, without changing your saved choice |
| Submit the Create form empty | Validation errors appear in the language you selected |
| Click the ☆ star | Toggles the favourite state |

---

## What is in here

```text
app/
├── MeowWorld/                      # the web application
│   ├── Controllers/
│   │   ├── CatsController.cs       # full CRUD + favourite toggle
│   │   ├── CultureController.cs    # language switching
│   │   └── HomeController.cs
│   ├── Data/AppDbContext.cs        # EF Core context + seed data
│   ├── Models/Cat.cs               # the entity
│   ├── Resources/                  # SharedResource.{ja,en}.resx
│   ├── SharedResource.cs           # resource type marker (project root, see below)
│   ├── Views/
│   ├── wwwroot/js/i18n.js          # instant language switching
│   └── Program.cs
├── MeowWorld.Tests/                # 20 xUnit tests
└── .github/                        # the Copilot customisation the workshop builds
    ├── copilot-instructions.md
    ├── instructions/  prompts/  agents/  skills/
```

The `.github/` folder is the finished version of what you build across Steps 4 and 9. Open the
folder in VS Code or Visual Studio and Copilot picks it up automatically.

---

## The design

A warm editorial layout rather than an admin editorial landing page: a serif display face with an italic
accent, generous whitespace, and a restrained palette of paper, forest green, clay and gold.

| Piece | Notes |
|-------|-------|
| Display type | Instrument Serif (Latin), Shippori Mincho (Japanese) |
| Body type | Inter, falling back to the system UI stack |
| Hero artwork | `wwwroot/images/cat-plate.svg`, drawn for this project. No photo dependency, ~8 KB |
| Palette | `--paper #FAF7F1`, `--ink #16140F`, `--clay #B4502C`, `--forest #1B3A32`, `--gold #D8C87A` |

Webfonts come from Google Fonts with a full fallback stack, so the page stays legible if that
request is blocked. Japanese gets its own serif stack, because a Latin display face has no
Japanese glyphs and would otherwise fall back to a mismatched gothic.

Type scale, letter-spacing and line-height all change under `html[lang="ja"]`. Japanese needs
looser line-height and no uppercase transforms; applying the Latin settings to Japanese looks
wrong, and this is the main reason the two languages are styled separately rather than sharing
one stylesheet.

## How the language toggle works

Three layers, so that switching is instant **and** the server still knows your language:

1. **Server renders the correct language on load.** `.resx` files are the single source of truth,
   read through `IStringLocalizer<SharedResource>`. This is what makes validation messages,
   `TempData` flash messages and page titles correct.
2. **The toggle swaps text in place, with no reload.** `wwwroot/js/i18n.js` fetches
   `/Culture/Strings?culture=en` once per language, caches it, and rewrites every element marked
   `data-i18n="Key"`.
3. **The choice is persisted in the `.AspNetCore.Culture` cookie**, so the next server render
   agrees with what you are looking at.

The default is Japanese for everyone. The browser's `Accept-Language` header is deliberately
ignored, so an English-locale browser still gets Japanese until the visitor chooses otherwise.
To honour the browser instead, delete the `RequestCultureProviders.Remove(...)` block in
`Program.cs`.

Full reasoning, including the approaches rejected and why, is in
[docs/Localization/README.md](../docs/Localization/README.md).

### Two mistakes worth knowing about

Both were hit while building this, and both produce confusing symptoms:

- **`SharedResource.cs` must sit at the project root**, not inside `Resources/`. Inside
  `Resources/` its namespace becomes `MeowWorld.Resources`, which combines with
  `ResourcesPath = "Resources"` to look for `MeowWorld.Resources.Resources.SharedResource.ja` and
  throw `MissingManifestResourceException`.
- **The toggle must write the cookie on every switch**, not only when it fetches. Caching the
  dictionary means a repeat switch skips the server, so a cookie set only by the server response
  goes stale and the next navigation reverts the language.

---

## Adding a UI string

1. Add the key and Japanese value to `Resources/SharedResource.ja.resx`
2. Add the **same key** and the English value to `Resources/SharedResource.en.resx`
3. Use it in a view: `<span data-i18n="Your_Key">@L["Your_Key"]</span>`
4. Run `dotnet test`

Step 4 is not optional. `LocalizationTests` compares the two resource files and fails if a key
exists in one language but not the other, which is the easiest mistake to make here.

---

## Tests

| File | Covers |
|------|--------|
| `MeowWorld.Tests/Controllers/CatsControllerTests.cs` | CRUD, NotFound paths, `[Required]` validation, ModelState failure, favourite toggle |
| `MeowWorld.Tests/LocalizationTests.cs` | Both resx load, keys match exactly, no empty values, key translations differ |
| `e2e/tests/*.spec.js` | Browser tests: the no-reload toggle, cookie persistence, accessible names, CRUD round trip, `CreatedAt` immutability, open-redirect rejection |

Unit tests:

```bash
cd app && dotnet test          # 21 tests
```

Browser tests (see [e2e/README.md](e2e/README.md)):

```bash
cd app/e2e && npm install && npx playwright install chromium && npm test   # 15 tests
```

All tests use a uniquely-named InMemory database, so they are order-independent and safe to run
in parallel.

---

## Known limitations

Being explicit about what this does **not** do, since it is a teaching artefact:

- **No authentication.** Anyone reaching the app can edit and delete any cat
- **Migrations run automatically at startup.** Convenient for a demo, wrong for production, where
  migrations belong in a reviewed deployment step
- **The header links cover only what exists.** The earlier wireframe showed a ten-item sidebar;
  the redesign drops the items with nothing behind them rather than showing dead links
- **Breeds are free text**, so the same breed can be spelled several ways. The lookup-table
  approach is sketched in the localization guide
- **No pagination.** Fine for 5 cats, not for 5,000
