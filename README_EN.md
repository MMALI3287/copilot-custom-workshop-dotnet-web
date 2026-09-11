# Custom GitHub Copilot Workshop (ASP.NET Core & SQLite Stack)

🌐 **Language:** [日本語](README.md) | **English**

Welcome to the custom Copilot workshop prepared by the **GitHub Expert Services Team**!

In this workshop you will learn the latest GitHub Copilot capabilities hands-on, centred on **Agent mode**, **Custom Instructions** and **Custom Agents**, while building "MeowWorld", an ASP.NET Core MVC + SQLite web application.

> **Note:** Visual Studio 2026 + .NET 10 (LTS) is the primary environment. VS Code (C# Dev Kit) also works. If you use Visual Studio 2022 you can run the workshop on .NET 8 (LTS). See [Step 2](docs/2_BeforeGettingStarted/README_EN.md) for details.

## Table of Contents

| Step | Title | Main Learning Points |
|------|-------|----------------------|
| 1 | [The Story of Making Mona's Dream Come True](docs/1_Story/README_EN.md) | Background story of the workshop |
| 2 | [Before Getting Started](docs/2_BeforeGettingStarted/README_EN.md) | Environment setup, mode explanation, prerequisites |
| 3 | [Create the Project (Agent mode)](docs/3_CreateProject/README_EN.md) | Build the whole project in one shot with Agent mode |
| 4 | [Custom Instructions](docs/4_CustomInstructions/README_EN.md) | Three layers of instructions (repository / file scope / .prompt.md) |
| 5 | [Implement the DB Layer](docs/5_ImplementDBLayer/README_EN.md) | The Agent implements EF Core + SQLite autonomously |
| 6 | [Implement MVC + Vision](docs/6_ImplementMVC/README_EN.md) | Using .prompt.md + generating UI from an image |
| 7 | [Token Savings and Context Management](docs/7_TokenManagement/README_EN.md) | When to use #file, #codebase and #terminalLastCommand |
| 8 | [Unit Testing](docs/8_UnitTesting/README_EN.md) | The /tests command + automatic test generation by the Agent |
| 9 | [Custom Agent & Skill](docs/9_CustomAgent/README_EN.md) | Building a specialist agent with `.agent.md` + `SKILL.md` |
| 10 | [Summary and Retrospective](docs/10_LessonsLearned/README_EN.md) | Best practices and next steps |

**Appendices:**

| Appendix | Contents |
|----------|----------|
| [Troubleshooting Guide](docs/TroubleshootingGuide/README_EN.md) | Fixes for common Copilot and environment problems |
| [Image Analysis](docs/ImageAnalysis/README.md) | Detailed, text-only specifications for every image used in the workshop |
| [Bilingual UI (JA/EN toggle)](docs/Localization/README.md) | How to add an instant Japanese/English language toggle to MeowWorld |
| [String Catalog](docs/Localization/string-catalog.md) | Every UI string in the workshop with its resource key and JA/EN values |
| [Maintainer Copilot Instructions (EN)](docs/Reference/maintainer-copilot-instructions_EN.md) | English translation of `.github/copilot-instructions.md` |
| [Reference Implementation](app/README.md) | A complete, working build of MeowWorld including the JA/EN toggle |

> **⚠️ If you are running the workshop as written:** `app/` in this fork already contains the
> finished MeowWorld application. Step 3 assumes an empty `app/` that you create with `mkdir app`,
> so move or delete the folder before starting. If you only want to read the finished code, leave
> it as it is.

---

## Running this as a facilitated session

| Format | Duration | Steps to cover |
|--------|----------|----------------|
| Full workshop | ~4 hours | All of 1-10 |
| Half day | ~2.5 hours | 1-6, then 9 and 10. Drop 7 and 8 |
| Copilot-customisation focus | ~2 hours | 2, 4, 9, 10. Provide a pre-built app |
| Self-paced | Any | All, in order. Step 2's pre-session checklist matters most here |

**Send participants the [Step 2 prerequisites](docs/2_BeforeGettingStarted/README_EN.md#prerequisites) a day ahead.** Every command in it is verifiable in under a minute, and environment problems discovered at the start of a session cost the whole room, not just one person.

Three things worth knowing before you facilitate:

- **Step 4 is load-bearing.** Steps 5, 6 and 8 all demonstrate its effect. It is the one step that cannot be skipped for time
- **Step 9's comparison needs a clean baseline.** Participants who run the After case on top of the Before changes will measure nothing. The setup for this is spelled out at the top of Step 9
- **Outputs will differ between participants.** Model selection under Auto varies, so two people running an identical prompt get different code. Plan for that rather than treating it as an error, and use it: it is a genuine property of the tool worth discussing in the retrospective

---

## About this English edition

The workshop was originally authored in Japanese, and the MeowWorld application it builds has a Japanese UI by design. This English edition exists so that non-Japanese speakers can follow the same steps.

Two things to keep in mind:

- **Prompts are shown in both languages.** Each Copilot prompt block gives the original Japanese text and an English equivalent. Either one works. The Japanese prompts are what the workshop was tested with.
- **The generated UI stays Japanese unless you change it.** The Custom Instructions in Step 4 tell Copilot to write Japanese comments and Japanese UI labels. If you want an English or bilingual UI, follow [docs/Localization/README.md](docs/Localization/README.md), which adds a JA/EN toggle without breaking any of the workshop steps.
