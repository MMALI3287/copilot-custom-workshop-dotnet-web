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

Look back on today's experience from the following angles:

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
