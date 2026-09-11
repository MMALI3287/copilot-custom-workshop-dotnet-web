# check-aicredit.png - Checking Model and Credit Consumption

**Path:** `docs/7_TokenManagement/images/check-aicredit.png`
**Format:** PNG, 1191 × 302, 8-bit RGBA, non-interlaced, 36 KB
**Used in:** [Step 7 - Token Savings and Context Management](../7_TokenManagement/README_EN.md), to show where the model name and credit cost appear

---

## Summary

A cropped screenshot of the tail end of a Copilot Chat response in VS Code, on a dark theme. It shows the last code block of an answer, a closing sentence, the response action icons, and the metadata footer that names the model used and the credits consumed.

The whole point of the image is the bottom-right corner. Everything else is incidental context.

## Layout structure

```text
┌──────────────────────────────────────────────────────────────────┐
│  {                                                               │
│    "ConnectionStrings": {                                        │  syntax-highlighted
│      "DefaultConnection": "Data Source=meowworld.db"             │  JSON code block
│    }                                                             │
│  }                                                               │
├──────────────────────────────────────────────────────────────────┤
│  必要ならこの内容で [◉ Program.cs] へ反映する差分も作成します。      │  closing sentence
│                                                                  │
│  ⟳  ⧉  👍  👎                    GPT-5.3-Codex • 76.9 credits    │  actions / metadata
└──────────────────────────────────────────────────────────────────┘
```

A short red horizontal bar appears at the very bottom right of the capture, partially cut off by the crop. It is an annotation drawn on top of the screenshot to point at the credit figure, not part of the VS Code interface.

## The code block

Syntax-highlighted JSON on a dark background:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=meowworld.db"
  }
}
```

This is the `appsettings.json` connection string from Step 5. Its presence tells you the screenshot was taken during a conversation about the database configuration, which is consistent with Step 7's experiment prompts.

## The closing sentence

The one piece of Japanese text in the image:

| Japanese | 必要ならこの内容で `Program.cs` へ反映する差分も作成します。 |
|----------|--------------------------------------------------------|
| Romanisation | Hitsuyō nara kono naiyō de Program.cs e han'ei suru sabun mo sakusei shimasu. |
| English | "If you need it, I can also create a diff that applies this content to `Program.cs`." |

`Program.cs` is rendered as an inline file chip with a small C# file icon, which is how VS Code displays a file reference inside a chat response, not as plain text.

This sentence is itself evidence of the Custom Instructions working. The `.github/copilot-instructions.md` from Step 4 contains 日本語で回答すること ("answer in Japanese"), and here the model is answering in Japanese without being asked to.

## The response actions

Four icons in a row at the bottom left, in this order:

| Icon | Meaning |
|------|---------|
| ⟳ Circular arrow | Regenerate / retry the response |
| ⧉ Two overlapping squares | Copy the response |
| 👍 Thumbs up | Positive feedback |
| 👎 Thumbs down | Negative feedback |

## The metadata footer (the important part)

At the bottom right, in a muted grey:

```text
GPT-5.3-Codex • 76.9 credits
```

Two separate facts, separated by a bullet:

| Part | Value in this capture | What it means |
|------|----------------------|---------------|
| Model name | `GPT-5.3-Codex` | Which model actually served this response |
| Credit cost | `76.9 credits` | What this single response consumed from the quota |

### Why the model name matters here

Step 2 tells participants to set the model selector to **Auto**. Under Auto, Copilot picks a model per request based on the complexity of the input. So this footer is the only place you can find out *which* model Auto chose for a given turn.

That is exactly what Step 7's experiment needs. When you run the same question in patterns A, B and C, the footer is how you observe that a clearer, tighter prompt can be routed to a cheaper model, while a vague one may be escalated.

### Reading the credit figure honestly

Two cautions, and Step 7 makes the second one itself:

1. **`76.9` is not a target.** It is one response in one conversation on one account. Your figures will differ, and comparing your absolute number against this screenshot tells you nothing.

2. **A single-turn credit difference is weak evidence.** Step 7 states plainly that on a small one-off task the difference between patterns can come out small, and that the real saving shows up as reduced rework across a whole task rather than as a lower number on one response. Treat this footer as an observation instrument, not as a score to optimise.

### Where to find it in your own session

The footer sits at the bottom right of each completed response in the Copilot Chat panel. If you do not see it:

- The response may still be streaming; it appears when the turn completes
- Your Copilot Chat extension may predate the feature, so update it
- Individual (non-Business/Enterprise) plans may not surface a credit figure at all

## Version note

`GPT-5.3-Codex` is the model this particular capture happened to use. Model lineups change frequently. The name in your own footer will very likely differ, and that is not a problem with your setup.

## Suggested alt text

> A Copilot Chat response in VS Code ending with a JSON snippet for the DefaultConnection connection string, a Japanese closing sentence, and a footer showing the model name GPT-5.3-Codex and a cost of 76.9 credits.
