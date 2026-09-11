# Summary and Retrospective

🌐 **Language:** [日本語](README_JA.md) | **English**

[Previous - Custom Agent](../9_CustomAgent/README_EN.md)

Well done! Let us organise the GitHub Copilot features you experienced in this workshop and the points that matter when applying them in real work.

---

## The features you learned in this workshop

| Step | Feature | What you can now do |
|------|---------|---------------------|
| 3 | **Agent mode** | Run project creation through build verification automatically with a single prompt |
| 4 | **Custom Instructions** | Apply project-wide rules automatically with `.github/copilot-instructions.md` |
| 4 | **File-scope instructions** | Apply rules per file type with `.instructions.md` + `applyTo` |
| 4 | **Reusable prompts** | Save and share templated instructions with `.prompt.md` |
| 5 | **Automatic error repair** | The Agent detects and fixes build errors |
| 5 | **/fix** | Fix errors in the editor with one command |
| 6 | **Vision** | Implement UI automatically from a wireframe image |
| 6 | **Using .prompt.md** | Generate consistent CRUD controllers with a reusable prompt |
| 7 | **Context management** | Pass the necessary information efficiently with `#file`, `#codebase` and `#terminalLastCommand` |
| 8 | **/tests** | Generate unit tests automatically from the file under test |
| 9 | **Custom Agent** | Build a project-specific AI assistant with `.agent.md` |
| 9 | **Skill** | Externalise knowledge with `SKILL.md` for the Agent to reference autonomously |

---

## Best practices for real work

### Recommended order for team adoption

```text
1. Put copilot-instructions.md in place (immediate effect, low cost)
   ↓
2. Add rules per file type with .instructions.md
   ↓
3. Templatise frequent tasks with .prompt.md
   ↓
4. Build specialist agents with .agent.md (standardise role, procedure and completion criteria)
   ↓
5. Externalise reference knowledge with SKILL.md (share guidelines, FAQs and design patterns)
```

### Tips

| Situation | Recommended technique |
|-----------|----------------------|
| Starting a new project | Scaffold it all at once in Agent mode |
| Introducing Copilot to an existing project | Generate the initial files with `/init`, review them, then start using them |
| Unifying coding conventions | `copilot-instructions.md` + grow it through review |
| Implementing from a design | Input the image with Vision |
| Repetitive tasks | Templatise with `.prompt.md` |
| Onboarding a new member | Make procedures and knowledge explicit with a Custom Agent + Skill |
| Handling errors | Leave it to the Agent, or use `#terminalLastCommand` + `/fix` |
| Saving tokens | `#file` pinpointing > `#codebase` |

---

## Points for the retrospective

### A structured 15 minutes

If you are facilitating, run it in this order rather than opening the floor. Open discussion in a mixed-skill room tends to be dominated by whoever had the worst tooling problem.

| Minutes | Prompt | Format |
|---------|--------|--------|
| 0-3 | "Which step saved you the most time, and roughly how much?" | Individually, in writing |
| 3-7 | "Where did you have to correct Copilot? What did the mistake have in common with the others?" | Pairs |
| 7-11 | "Which of these would you actually put in your own repository on Monday?" | Whole group |
| 11-15 | "What is one thing you would not use this for?" | Whole group |

The last prompt is the one worth protecting. A retrospective that produces only enthusiasm has not been critical enough to be useful, and the limits people name here are usually the most accurate content in the room.

### Take your measurements seriously

You recorded numbers in Steps 4, 7 and 9. Look at them together now:

- **Step 4** told you what the guardrails changed without you asking
- **Step 7** told you what context actually bought you, in accuracy rather than credits
- **Step 9** told you what a role definition and a completion contract bought you on top of the guardrails

If any of those came out flat — no visible difference — say so. Flat results are data. The most common honest finding is that Step 9's quality difference was small while its *reporting* difference was large, which is precisely the claim the step makes.

### Look back from the following angles:

### What was good to leave to Copilot
- Routine code generation (CRUD, tests)
- Fixing build errors
- Adhering to naming and comment conventions

### What humans should decide
- Deciding the requirements (what to build)
- Choosing the architecture (how to build it)
- The final call on security and performance
- Reviewing the code Copilot generated

### Things to watch out for
- Always verify Copilot's output (compile errors and logic mistakes are possible)
- Do not include confidential information in prompts
- Be careful about the license and copyright of generated code
- Check the content of a command before pressing `Continue`

---

## A two-week adoption plan

The recommended order above is a sequence, not a schedule. Here is a schedule. Adjust the dates; keep the order.

| When | Action | Done when |
|------|--------|-----------|
| **Day 1** | Run `/init` on one real repository and read what it generates | You have a draft `copilot-instructions.md` you did not write by hand |
| **Day 2** | Cut that draft down to rules your team actually enforces in review | It fits on one screen |
| **Day 3** | Commit it and tell the team it exists | It is on the default branch |
| **Day 5** | Collect the first round of "Copilot keeps doing X" complaints | You have 3 or more concrete items |
| **Day 6** | Turn each into a rule, or decide it is not worth one | The file grew by only what earned its place |
| **Week 2, Day 1** | Add one `.instructions.md` for your noisiest file type (tests, or views) | A generated file of that type follows the convention |
| **Week 2, Day 3** | Templatise your single most repeated task as a `.prompt.md` | A colleague used it without asking you how |
| **Week 2, Day 5** | Only now, consider a `.agent.md` | You can name its Definition of Done in one sentence |

> **Why `.agent.md` comes last.** A Custom Agent encodes a process. If your team's process is not yet written down anywhere, the agent will encode one person's habits and present them as a standard. Get the rules stable first, then automate them.
>
> The same applies to Skills. Externalise knowledge into a Skill when you have two agents that need it, or one body of knowledge large enough that loading it every time is wasteful. Before that, a `#file` reference is simpler and honest about what it is.

### What to be careful about in a real repository

The workshop is a safe sandbox. A production repository is not, and three of the workshop's conveniences become liabilities:

| Workshop practice | Why it is fine here | What to do instead at work |
|-------------------|--------------------|-----------------------------|
| Approving every terminal command quickly | Nothing here is destructive, and the repo is disposable | Read each one. Agent mode can run anything you can run |
| Letting the Agent fix failing tests on its own | The tests are throwaway | Review the fix. "Make the test pass" and "make the code correct" are different goals, and a model will sometimes achieve the first by weakening the second |
| Automatic migrations on startup | Convenient for a demo DB | Never in production. Migrations belong in a deliberate, reviewed deployment step |

The middle row is the one that bites people. A test the Agent rewrote to match broken behaviour is worse than a failing test, because it is silent.

---

## Next steps

Try these after the workshop:

1. **Use `/init` for the initial setup on an existing project**
   - Start by generating Copilot's initial file set with `/init`
   - Do not use the result as-is; review it against your team's conventions
   - A rough guide for organising it:
     - Shared rules go in `copilot-instructions.md`
     - Rules per file type go in `.instructions.md`
     - Role, procedure and completion criteria go in `.agent.md`
     - Reference knowledge (guidelines, FAQs) goes in `SKILL.md`

2. **Create a `copilot-instructions.md` for your own project**
   - Use today's template as a reference and customise it for your project

3. **Share `.prompt.md` files across the team**
   - Commit prompts for frequent tasks to the repository

4. **Grow your Custom Agent**
   - Put role, work procedure and completion criteria in `.agent.md`, and split development guidelines and FAQs out as Skills

5. **Keep up with new Copilot features**
   - [GitHub Copilot Changelog](https://github.blog/changelog/label/copilot/)
   - [VS Code Release Notes](https://code.visualstudio.com/updates)

---

## In closing

> **Copilot is a "co-pilot".**
>
> You are the one holding the controls. Deciding the destination and confirming safety is the human's job.
> But leave the boring routine work to Copilot,
> and concentrate on more creative work yourself.

May the cats of MeowWorld make your development life a little more fun 🐱

---

[Previous - Custom Agent](../9_CustomAgent/README_EN.md) | [Back to the top](../../README_EN.md)
