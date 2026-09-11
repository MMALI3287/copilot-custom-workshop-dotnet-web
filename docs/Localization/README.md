# Bilingual UI: Adding an Instant JA/EN Toggle to MeowWorld

This is an **extension to the workshop**, not part of it. The workshop deliberately builds a Japanese UI: the Custom Instructions in Step 4 instruct Copilot to write Japanese comments and Japanese UI labels, and that is the intended result. This document shows how to keep that Japanese baseline and add an English mode on top, switchable from a control in the header.

Nothing here changes any workshop step. Do it after Step 6, once views exist, or after Step 9 when the app is complete.

---

## Decide first: what "instant" has to mean

This is the decision that shapes everything else, so make it before writing code.

| Approach | How the switch works | Truly instant? | Covers server-side strings? | Complexity |
|----------|---------------------|----------------|----------------------------|------------|
| **A. Server-side (`.resx` + culture cookie)** | Sets a cookie, reloads the page | No, one round trip | Yes, everything | Low |
| **B. Client-side (JSON dictionary + JS)** | Swaps text nodes in the DOM | Yes, zero round trip | No | Medium |
| **C. Hybrid** | A renders both, JS swaps in place | Yes | Yes | Medium-high |

**Recommendation: start with A.**

The honest objection to A is that it is not literally instant. In practice, on localhost the reload is well under 100 ms, and on a deployed app a cached page is not much worse. Against that, A is the only approach where server-side content comes out in the right language: validation messages from DataAnnotations, `TempData` flash messages, exception pages, and any string a controller returns. With B, all of that stays in one language regardless of what the toggle says, which is a worse bug than a perceptible reload.

Do B only if you have a specific reason for zero reload, such as preserving unsaved form state or a live-updating dashboard. Do C only if you have measured A and found the reload genuinely unacceptable.

The rest of this document implements **A**, then shows what to add for **C** if you decide you need it.

> Sources: [Globalization and localization in ASP.NET Core](https://learn.microsoft.com/aspnet/core/fundamentals/localization), [Make an app's content localizable](https://learn.microsoft.com/aspnet/core/fundamentals/localization/make-content-localizable), [Provide localized resources](https://learn.microsoft.com/aspnet/core/fundamentals/localization/provide-resources).

---

## What gets translated and what does not

A language toggle should switch **chrome**, not **content**. Getting this boundary wrong is the most common mistake.

| Category | Example in MeowWorld | Translate? |
|----------|---------------------|------------|
| Navigation, headings, buttons | 猫一覧, 新しい猫を登録, 詳細, 編集, 削除 | **Yes** |
| Table column headers | 名前, 年齢, 品種, 説明, 操作 | **Yes** |
| Validation and error messages | "名前は必須です" | **Yes** |
| Page titles and the logo alt text | MeowWorld 猫管理システム | **Yes** |
| Cat names | みけ, くろ, しろ, チャチャ, ソラ | **No** |
| Cat descriptions | おとなしい性格 | **No** |
| Breed names | 三毛猫, ロシアンブルー | Borderline, see below |

Cat names and descriptions are user data. They live in the database, a user typed them, and there is no correct English for みけ beyond a transliteration. Translating them would mean either a translation column per language (a schema change that multiplies with every language) or machine translation at render time (slow and wrong). Leave them.

Breeds are the genuinely ambiguous case. They are a closed, finite vocabulary, so a lookup table is feasible, and a non-Japanese reader gets real value from "Russian Blue" over ロシアンブルー. See "Translating data, not just labels" at the end.

The full key-by-key list is in the [string catalog](string-catalog.md).

---

## Pseudocode first

Before the real code, the whole mechanism in eight lines:

```text
STARTUP:
  register localization services, resources live in /Resources
  supported cultures = [ja, en], default = ja
  install culture middleware BEFORE routing

EVERY REQUEST:
  middleware reads the culture, in order: ?culture= query -> cookie -> Accept-Language header
  sets CurrentCulture and CurrentUICulture for this request

RENDERING A VIEW:
  @Localizer["Nav_CatList"]  ->  looks up the key in SharedResource.<culture>.resx
                             ->  returns the value, or the key itself if missing

USER CLICKS THE TOGGLE:
  POST /Culture/Set { culture: "en", returnUrl: <current page> }
    -> write the .AspNetCore.Culture cookie
    -> LocalRedirect(returnUrl)
    -> the page re-renders, now in English
```

The two things to hold on to: **the cookie is the state**, and **the middleware must run before anything that needs the culture**.

---

## Implementation

### Step 1: register the services

In `Program.cs`, before `builder.Build()`:

```csharp
using System.Globalization;
using Microsoft.AspNetCore.Localization;

// ローカライズサービスの登録（リソースは Resources フォルダー配下）
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });

// サポートする言語（日本語を既定とする）
var supportedCultures = new[]
{
    new CultureInfo("ja"),
    new CultureInfo("en")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("ja");
    options.SupportedCultures = supportedCultures;      // 数値・日付の書式
    options.SupportedUICultures = supportedCultures;    // 文字列リソース
});
```

`SupportedCultures` and `SupportedUICultures` are separate on purpose. The first governs formatting of dates and numbers; the second governs which `.resx` is used. Here they are the same, but if you ever want English labels with Japanese date formatting, this is the lever.

### Step 2: install the middleware in the right place

Still in `Program.cs`, after `var app = builder.Build()`:

```csharp
// ❗ 順序が重要: カルチャを必要とするミドルウェアより前に置くこと
app.UseRequestLocalization(
    app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
```

**This ordering is not stylistic.** `UseRequestLocalization` sets `CultureInfo.CurrentUICulture` for the request. Any middleware that runs before it sees the default culture regardless of the cookie. Putting it after `UseRouting` is the single most common cause of "the toggle does nothing", and the symptom is indistinguishable from a broken cookie.

The middleware installs three culture providers by default, consulted in this order:

1. `QueryStringRequestCultureProvider` - reads `?culture=en&ui-culture=en`
2. `CookieRequestCultureProvider` - reads the `.AspNetCore.Culture` cookie
3. `AcceptLanguageHeaderRequestCultureProvider` - reads the browser's `Accept-Language` header

Query string beats cookie, which beats the browser's preference. That ordering is usually what you want: it makes a link like `?culture=en` shareable, while the cookie holds the durable choice. You can reorder them if you disagree.

### Step 3: create the shared resource class and the `.resx` files

`Resources/SharedResource.cs`:

```csharp
namespace MeowWorld.Resources;

/// <summary>
/// 共有リソースの型マーカー。実装は持たない。
/// </summary>
public class SharedResource
{
}
```

This empty class exists only as a type argument. `IStringLocalizer<SharedResource>` uses it to locate `SharedResource.<culture>.resx`.

Then create the resource files:

```text
Resources/
├── SharedResource.cs       <- the marker class
├── SharedResource.resx     <- the fallback (leave keys here in Japanese, or leave it empty)
├── SharedResource.ja.resx  <- Japanese
└── SharedResource.en.resx  <- English
```

Three naming rules that will cost you an afternoon if you get them wrong:

- **The culture suffix must match a registered culture exactly.** `SharedResource.ja.resx` matches `new CultureInfo("ja")`. If you name it `SharedResource.ja-JP.resx` and register only `ja`, it will not be found. Pick neutral cultures (`ja`, `en`) unless you specifically need regional variants.
- **The Build Action must be `Embedded Resource`.** The .NET SDK project format does this automatically for `.resx` under the project, but verify it if lookups silently fail.
- **`ResourcesPath` must match the real folder.** You set `"Resources"` in Step 1; the files must be under `Resources/`.

Populate them from the [string catalog](string-catalog.md). Every key appears in both files.

### Step 4: use the localizer in views

In `Views/_ViewImports.cshtml`:

```cshtml
@using Microsoft.AspNetCore.Mvc.Localization
@using MeowWorld.Resources
@inject IStringLocalizer<SharedResource> L
```

Then in any view:

```cshtml
<h1>@L["Page_CatList_Title"]</h1>

<a asp-action="Create" class="btn btn-primary">@L["Action_CreateCat"]</a>

<table class="table table-striped">
    <thead>
        <tr>
            <th>@L["Col_Id"]</th>
            <th>@L["Col_Name"]</th>
            <th>@L["Col_Age"]</th>
            <th>@L["Col_Breed"]</th>
            <th>@L["Col_Description"]</th>
            <th>@L["Col_Actions"]</th>
        </tr>
    </thead>
    <tbody>
    @foreach (var cat in Model)
    {
        <tr>
            <td>@cat.Id</td>
            <td>@cat.Name</td>          @* データなので翻訳しない *@
            <td>@cat.Age</td>
            <td>@cat.Breed</td>
            <td>@cat.Description</td>
            <td>
                <a asp-action="Details" asp-route-id="@cat.Id" class="btn btn-sm btn-info">@L["Action_Details"]</a>
                <a asp-action="Edit"    asp-route-id="@cat.Id" class="btn btn-sm btn-warning">@L["Action_Edit"]</a>
                <a asp-action="Delete"  asp-route-id="@cat.Id" class="btn btn-sm btn-danger">@L["Action_Delete"]</a>
            </td>
        </tr>
    }
    </tbody>
</table>
```

> **Why `IStringLocalizer<SharedResource>` and not `IViewLocalizer`?**
> `IViewLocalizer` resolves resources from the view's own file path, so `Views/Cats/Index.cshtml` needs `Resources/Views/Cats/Index.en.resx`. It has no built-in way to read a single global file. That means one `.resx` pair per view, and any string shared across views (詳細, 編集, 削除, every column header) gets duplicated into each one. For an app this size, one shared catalogue is much easier to keep consistent. Use `IViewLocalizer` if you have long, genuinely view-specific prose.

A missing key returns the key itself rather than throwing. So a button reading `Action_Details` means that key is absent from that language's `.resx`, not that the localizer is broken. This is convenient in development and easy to miss in review; grep your rendered output for `_` before shipping.

### Step 5: build the toggle

The controller, `Controllers/CultureController.cs`:

```csharp
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace MeowWorld.Controllers;

/// <summary>
/// 表示言語の切り替えを担当するコントローラー
/// </summary>
[Route("[controller]/[action]")]
public class CultureController : Controller
{
    /// <summary>
    /// 選択された言語をクッキーに保存し、元のページへ戻る
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Set(string culture, string returnUrl)
    {
        if (!string.IsNullOrEmpty(culture))
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    HttpOnly = false,   // クライアント側で現在の言語を読むため
                    SameSite = SameSiteMode.Lax
                });
        }

        // ❗ Redirect ではなく LocalRedirect を使う（オープンリダイレクト対策）
        return LocalRedirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
    }
}
```

Three deliberate choices:

- **`LocalRedirect`, never `Redirect`.** `returnUrl` comes from the request. `Redirect` would happily send the user to an attacker-supplied external site, which is an open redirect. `LocalRedirect` throws on a non-local URL.
- **`IsEssential = true`.** Under the GDPR cookie-consent features in ASP.NET Core, a non-essential cookie is withheld until consent is given. A language preference the user explicitly set is functionally essential; without this flag the toggle appears to do nothing for users who have not consented.
- **`ValidateAntiForgeryToken` with `[HttpPost]`.** A GET toggle is simpler but lets any page change a user's language with an `<img>` tag. Low stakes here, but there is no reason to accept it.

The partial view, `Views/Shared/_LanguageSwitcher.cshtml`:

```cshtml
@using System.Globalization
@{
    var current = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
    var returnUrl = $"{Context.Request.Path}{Context.Request.QueryString}";
}

<form asp-controller="Culture" asp-action="Set" method="post" class="d-flex align-items-center">
    <input type="hidden" name="returnUrl" value="@returnUrl" />
    <div class="btn-group btn-group-sm" role="group" aria-label="@(current == "ja" ? "表示言語" : "Display language")">
        <button type="submit" name="culture" value="ja"
                class="btn @(current == "ja" ? "btn-light" : "btn-outline-light")"
                aria-pressed="@(current == "ja" ? "true" : "false")"
                lang="ja">日本語</button>
        <button type="submit" name="culture" value="en"
                class="btn @(current == "en" ? "btn-light" : "btn-outline-light")"
                aria-pressed="@(current == "en" ? "true" : "false")"
                lang="en">English</button>
    </div>
</form>
```

Points worth keeping:

- Each button carries its own `lang` attribute so a screen reader pronounces 日本語 in Japanese and "English" in English.
- Each language is always written **in its own language**, never translated. A Japanese reader looking for their language scans for 日本語, not for "Japanese".
- `aria-pressed` communicates the current state to assistive technology, which colour alone does not.
- `returnUrl` preserves the query string, so the user returns to the same filtered or paged view.

Then render it in `_Layout.cshtml`, in the dark top bar on the right:

```cshtml
<header class="navbar navbar-dark bg-dark">
    <div class="container-fluid d-flex justify-content-between align-items-center">
        <a class="navbar-brand d-flex align-items-center" asp-controller="Home" asp-action="Index">
            <img src="~/images/logo.png" alt="@L["Logo_Alt"]" height="40" class="me-2 bg-white rounded" />
            <span>@L["App_Title"]</span>
        </a>
        <partial name="_LanguageSwitcher" />
    </div>
</header>
```

### Step 6: set `lang` on the html element

In `_Layout.cshtml`:

```cshtml
<html lang="@System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName">
```

Easy to forget and genuinely consequential. It drives screen-reader pronunciation, browser translation offers, and font selection for CJK text. A page serving Japanese under `lang="en"` can render with the wrong Han variants.

### Step 7: localise validation messages

DataAnnotations messages are server-generated, which is exactly the category approach B cannot reach.

```csharp
public class Cat
{
    /// <summary>猫の名前</summary>
    [Required(ErrorMessage = "Validation_Name_Required")]
    [MaxLength(50, ErrorMessage = "Validation_Name_MaxLength")]
    [Display(Name = "Col_Name")]
    public required string Name { get; set; }
}
```

The attribute values are now **resource keys, not messages**. `AddDataAnnotationsLocalization` (configured in Step 1 to use `SharedResource`) resolves them against the same catalogue. Add the `Validation_*` keys to both `.resx` files.

---

## Verifying it works

```bash
cd MeowWorld
dotnet run
```

1. Load the page. It should be Japanese, since `DefaultRequestCulture` is `ja`.
2. Click **English**. The page reloads in English. Column headers, buttons and the sidebar all switch; cat names and descriptions do not.
3. Reload. It stays English, because the cookie persists.
4. Open DevTools > Application > Cookies and confirm `.AspNetCore.Culture` holds `c=en|uic=en`.
5. Append `?culture=ja` to the URL. The page is Japanese for that request only, because the query-string provider outranks the cookie without overwriting it.
6. Submit the Create form with an empty name. The validation message should follow the current language.

Step 6 is the one people skip, and it is the one that distinguishes a real implementation from a cosmetic one.

---

## If you decide you need approach C (no reload)

Render both languages, then swap with CSS. No round trip, and server-rendered strings stay correct because the server produced both.

```cshtml
@* 両方の言語を出力し、表示は CSS で切り替える *@
<th><span lang="ja">@L_ja["Col_Name"]</span><span lang="en">@L_en["Col_Name"]</span></th>
```

```css
html[data-lang="ja"] [lang="en"] { display: none; }
html[data-lang="en"] [lang="ja"] { display: none; }
```

```js
// クリック時に即座に切り替え、選択はサーバーにも保存する
document.querySelectorAll('[data-set-lang]').forEach(btn => {
  btn.addEventListener('click', e => {
    e.preventDefault();
    const lang = btn.dataset.setLang;
    document.documentElement.dataset.lang = lang;            // 即時反映
    document.documentElement.lang = lang;
    fetch('/Culture/Set', {                                   // 次回以降のために保存
      method: 'POST',
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
      body: `culture=${encodeURIComponent(lang)}`
    });
  });
});
```

Be clear-eyed about the costs before choosing this:

- **Every localisable string is in the DOM twice.** Page weight and DOM size roughly double for text-heavy pages.
- **Hidden text is still in the accessibility tree unless you manage it.** `display: none` does remove it, but any approach using opacity or off-screen positioning will have a screen reader read both languages.
- **Content injected after load misses the mechanism.** Anything rendered by JavaScript, or returned by an AJAX partial, needs both variants too, or it will not switch.
- **Validation messages arriving from a POST** are single-language unless that response also renders both.

The two-render approach above is the least fragile variant of C, because the server remains the single source of truth. A pure JSON-dictionary variant of B is simpler to start and gets steadily worse as server-generated strings accumulate.

---

## Translating data, not just labels

If you want breed names to switch too, resist the urge to add `BreedEn` alongside `Breed`. That column pattern needs a new column for every language you ever add, and it puts translation data in the same table as the cat.

Two better options:

**A lookup table**, when the vocabulary is closed and shared:

```csharp
/// <summary>品種マスタ</summary>
public class Breed
{
    public int Id { get; set; }
    public required string NameJa { get; set; }
    public required string NameEn { get; set; }
}

// Cat は Breed への外部キーを持つ
public int BreedId { get; set; }
public Breed? Breed { get; set; }
```

Then pick the column by culture at render time. This normalises breeds properly, which is a good idea regardless of localisation, and it is a natural Step 9 exercise for `@meowworld-dev`.

**A translation table**, when the set is open-ended:

```text
CatTranslations(CatId, Culture, Description)
```

One row per cat per language. Adding a language adds rows, not columns.

For this workshop, the honest answer is that neither is necessary. Cat names and free-text descriptions are user content and should stay as entered. Breeds are the only column where translation clearly helps a reader, and the lookup table is the right shape if you want it.

---

## Effect on the workshop's Copilot configuration

If you add the toggle, update the instruction files so Copilot generates localised code from then on rather than hard-coded Japanese:

`.github/instructions/views.instructions.md` - add:

```markdown
- UI 文字列は直接記述せず、`@L["キー名"]` でリソースから取得すること
- リソースキーは `Resources/SharedResource.ja.resx` と `SharedResource.en.resx` の両方に追加すること
- 猫の名前・説明などのデータは翻訳対象外とすること
```

(In English: do not write UI strings inline, retrieve them from resources with `@L["Key"]`; add every key to both `.resx` files; do not treat data such as cat names and descriptions as translatable.)

`.github/skills/meowworld-patterns/SKILL.md` - add to the new-feature pattern:

```markdown
### 3. UI 文字列の追加
1. `Resources/SharedResource.ja.resx` にキーと日本語値を追加
2. `Resources/SharedResource.en.resx` に同じキーと英語値を追加
3. ビューでは `@L["キー名"]` で参照
4. 両方の言語で表示を確認
```

(In English: add the key and the Japanese value to the `ja` resource, the same key and the English value to the `en` resource, reference it in the view with `@L["Key"]`, and verify the display in both languages.)

This is a good demonstration of the Step 9 argument in its own right. The convention lives in one Skill file, and every agent that references that Skill picks it up without being told again.

---

## Reference

- [Globalization and localization in ASP.NET Core](https://learn.microsoft.com/aspnet/core/fundamentals/localization) - the overview
- [Make an app's content localizable](https://learn.microsoft.com/aspnet/core/fundamentals/localization/make-content-localizable) - `IStringLocalizer`, `IViewLocalizer`, shared resources
- [Provide localized resources](https://learn.microsoft.com/aspnet/core/fundamentals/localization/provide-resources) - `.resx` naming and lookup rules
- [Select a language/culture](https://learn.microsoft.com/aspnet/core/fundamentals/localization/select-language-culture) - culture providers and the middleware
- [String catalog](string-catalog.md) - every key used above, with JA and EN values
