# init-webapp.png - The Default ASP.NET Core Page

**Path:** `docs/3_CreateProject/images/init-webapp.png`
**Format:** PNG, 1306 × 370, 8-bit RGBA, non-interlaced, 36 KB
**Used in:** [Step 3 - Create the Project](../3_CreateProject/README_EN.md), as proof that the scaffolded project runs

---

## Summary

A cropped browser screenshot showing the unmodified default page of a freshly scaffolded ASP.NET Core MVC project. This is the "it works" checkpoint at the end of Step 3, before any MeowWorld-specific code exists.

Everything visible here comes from the `dotnet new mvc` template. Nothing on this page was written by Copilot or by you.

## Layout structure

```text
┌────────────────────────────────────────────────────────────────┐
│ ← → ⟳ │ http://localhost:5244/            │ ⧉ ⓘ  ▭ ⌗ ⋯       │  browser chrome, dark
├────────────────────────────────────────────────────────────────┤
│  MeowWorld      Home   Privacy                                 │  navbar, white
├────────────────────────────────────────────────────────────────┤
│                                                                │
│                        Welcome                                 │  h1, centred
│                                                                │
│         Learn about building Web apps with ASP.NET Core.       │  centred, with a link
│                                                                │
└────────────────────────────────────────────────────────────────┘
```

The capture is cropped to roughly the top third of the viewport. Everything below the welcome text, including the template's footer, is outside the frame.

## Browser chrome

A dark toolbar at the top containing, left to right:

- Back arrow, forward arrow, reload icon
- The address bar showing **`http://localhost:5244/`**
- On the right, a small cluster of browser icons: split-screen, info, a device/cast icon with a chevron, a developer-tools icon, and an overflow (`⋯`) menu

The chrome style and icon set are consistent with Microsoft Edge on Windows, which matches the workshop's stated `OS: Windows` in the Custom Instructions.

### About the port

`5244` is a **randomly assigned port**, written into `Properties/launchSettings.json` when the project is created. Yours will almost certainly be a different number. Do not treat 5244 as a value to match, and do not "fix" your project to use it.

Note also that the URL is plain **`http`**, not `https`. The MVC template configures both an HTTP and an HTTPS profile; this capture was taken on the HTTP one. If your browser opens `https://localhost:7xxx` instead, that is equally correct, and you may see a certificate warning until you run `dotnet dev-certs https --trust`.

## Page content

### Navigation bar

A white bar with a light bottom border:

- **`MeowWorld`** at the left, as the brand. This is the one piece of the page that reflects your project name; it comes from the project name you passed to `dotnet new mvc -n MeowWorld` and appears in `_Layout.cshtml`.
- **`Home`** and **`Privacy`** as the two nav links, both from the template.

There is no sidebar, no cat logo, and no Japanese anywhere. All of that arrives in Step 6.

### Body

- A large, light-weight centred heading: **`Welcome`**
- Beneath it, centred: **`Learn about `** followed by the blue underlined link text **`building Web apps with ASP.NET Core`** and a full stop

The link points at the ASP.NET Core documentation. This is `Views/Home/Index.cshtml` from the template, unedited.

## What to verify against this image

When you reach the end of Step 3, you have succeeded if:

- [ ] A browser opened on its own, or you opened the printed localhost URL
- [ ] The page loads without an error page or a connection refusal
- [ ] The brand in the navbar reads **MeowWorld**, confirming the project name took effect
- [ ] The heading reads **Welcome**

What does **not** need to match: the port number, http versus https, the browser, and the exact chrome icons.

## Language note

This image contains no Japanese text. The Custom Instructions that make the UI Japanese are not created until Step 4, and no MeowWorld view exists until Step 6, so the default template renders entirely in English.

If you add the JA/EN toggle from [docs/Localization/README.md](../Localization/README.md), this page is replaced long before the toggle exists, so nothing here needs a resource key.

## Suggested alt text

> The default ASP.NET Core page at http://localhost:5244, showing the MeowWorld brand with Home and Privacy links in the navbar, and a centred "Welcome" heading above a link to the ASP.NET Core documentation.
