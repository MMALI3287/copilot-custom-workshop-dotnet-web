# ArchitectureStacks.jpg - The Overall Program Stack

**Path:** `docs/2_BeforeGettingStarted/images/ArchitectureStacks.jpg`
**Format:** JPEG, 2635 × 2133, 330 DPI, baseline, 328 KB
**Used in:** [Step 2 - Before Getting Started](../2_BeforeGettingStarted/README_EN.md), as the big picture of what you build

---

## Summary

A two-tier architecture diagram on a white background, titled **Overall Program Stack**. It shows an application layer stacked above a data layer, each drawn as a dashed-border box containing a technology logo and a short caption. All text is English; there is no Japanese in this image.

The diagram is deliberately minimal. It communicates exactly two things: the app is ASP.NET Core MVC, and the database is a SQLite file.

## Layout structure

```text
                    Overall Program Stack                     ← title, large bold

     FRONTEND / BACKEND LAYER (ASP.NET Core MVC)              ← caption above the box
    ┌ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┐
    │                                                 │
    │    (.NET Core logo)                             │
    │         + MVC triad        VS Debug Server      │       ← blue dashed border
    │                                                 │
    └ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┘

                  DATA LAYER (SQLite)                          ← caption above the box
    ┌ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┐
    │                                                 │
    │    (SQLite logo)            File Base DB        │       ← black dashed border
    │                                                 │
    └ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┘
```

The two boxes are the same width and are vertically stacked with a gap between them. They are roughly equal in height, though the upper box's content is taller.

## Title

**Overall Program Stack** in large, bold, black sans-serif type, centred at the top.

## Upper tier: the application layer

**Caption above the box:** `FRONTEND / BACKEND LAYER (ASP.NET Core MVC)`

The box has a **blue dashed border**, distinguishing it from the lower box.

Contents, left to right:

1. **The .NET Core logo:** a purple circle with `.NET` in large white letters and `Core` beneath it in a lighter weight.

2. **The MVC triad:** three glossy spheres arranged in a triangle, connected by curved arrows, sitting to the lower right of the .NET circle:
   - A red sphere labelled **Model** at the top
   - A green sphere labelled **View** at the bottom left
   - An orange/amber sphere labelled **Controller** at the bottom right
   - Arrows run between them, indicating the MVC cycle. The arrowheads are small and the exact direction of flow is not clearly legible at this resolution, so do not read a precise data-flow claim into them. The intended meaning is the standard MVC relationship: the controller handles the request, works with the model, and selects the view.

3. **The caption `VS Debug Server`** in large plain black text, to the right of the logos.

That third element is the one worth pausing on. It says the application runs on the Visual Studio debug server (Kestrel, launched by `dotnet run` or by F5 in the IDE) rather than on IIS or in a container. It is a statement about the workshop's runtime environment, not about production architecture.

## Lower tier: the data layer

**Caption above the box:** `DATA LAYER (SQLite)`

The box has a **black dashed border**.

Contents, left to right:

1. **The SQLite logo:** the official mark, a blue page or book shape with a dark feather quill overlaid, followed by the wordmark `SQLite` in a dark teal serif face.

2. **The caption `File Base DB`** in large plain black text.

"File Base DB" is slightly non-idiomatic English for "file-based database". The point it makes is the important one: SQLite is a single file on disk (`meowworld.db` in this workshop) rather than a server process you connect to over a network. That is why the connection string is `Data Source=meowworld.db` and why there is nothing to install or start.

## What the diagram deliberately omits

- **Entity Framework Core.** EF Core sits between the application layer and the SQLite file and is the only thing the app talks to directly, yet it is not drawn. The `Microsoft.EntityFrameworkCore.Sqlite` package added in Step 3 is what bridges the two boxes.
- **The browser / client.** There is no user or browser element above the application layer, despite the layer being labelled "frontend".
- **Any arrow between the tiers.** The two boxes are stacked but not connected. The relationship is implied by the stacking, not drawn.
- **The test project.** `MeowWorld.Tests` has no place in this picture.
- **Migrations**, the seed data, and everything from Steps 4 onwards.

None of these omissions are errors for a Step 2 orientation diagram. They are worth naming because participants sometimes look for EF Core in this picture and cannot find it.

## A more complete mental model

For reference, the real runtime relationship in the finished workshop app:

```text
Browser
  │  HTTP
  ▼
Kestrel (the VS debug server)
  │
  ▼
ASP.NET Core MVC
  ├─ Controllers  (CatsController)
  ├─ Views        (Razor .cshtml)
  └─ Models       (Cat)
        │
        ▼
Entity Framework Core  (AppDbContext)
        │  Microsoft.EntityFrameworkCore.Sqlite
        ▼
meowworld.db  (a single SQLite file on disk)
```

## Suggested alt text

> An architecture diagram titled "Overall Program Stack", showing a frontend/backend layer of ASP.NET Core MVC running on the VS debug server, stacked above a data layer of SQLite as a file-based database.

## Language note

This image contains no Japanese text, so it needs no translation and no change if you add a JA/EN toggle to the application. It is the only image in the workshop with that property.
