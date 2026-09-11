# Maintainer Copilot Instructions (English translation)

This is an English reading of [`.github/copilot-instructions.md`](../../.github/copilot-instructions.md) at the repository root.

**This is a translation for reference only.** The Japanese file is the live one. Copilot reads `.github/copilot-instructions.md` automatically; this file is never loaded and changing it has no effect on Copilot's behaviour. If you change the rules, change the Japanese file.

> **Do not confuse this with the workshop's own instruction file.** The repository root `.github/copilot-instructions.md` governs *maintaining the teaching material*. The `.github/copilot-instructions.md` that participants create in Step 4 lives inside `app/` and governs *the MeowWorld application*. They are different files with different audiences, and the Japanese original explicitly warns about mixing them up.

---

## Translation

### General

- Answer in Japanese
- This `.github/` directory is for maintaining the workshop material
- Do not casually agree with the requester's opinion on work instructions or in discussion; ask questions where necessary to confirm intent
- Always judge fairly without deferring to unstated expectations, and work to improve the participant's learning experience

### Assumptions about the repository

- The `docs/` directory at the repository root is the hands-on material
- Exclude anything that is not a deliverable of this repository, referring to `.gitignore`
  - Even if `app/` exists, it is there temporarily to verify the procedure. As a rule there is no need to keep it in sync or consistent with the material in `docs/`
  - Do not normally refer to anything under `app/`; refer to it only when you need to check the results of verifying a procedure, or to reflect feedback back into the material

### Priorities during maintenance

- Prioritise the participant's learning experience, consistency between chapters and reproducibility of the procedure over the implementation itself
- Although this hands-on is for learning, aim to convey value with a structure close to real practice, assuming a real project
  - Do not resort to easy workarounds that weaken the guardrails in order to manufacture a difference
- Leave room for participants to think for themselves, while providing the necessary information in the right amount

### Rules for paths and structure

- Write code paths in the material relative to the `app/` workspace that participants open
- Do not confuse the root `docs/` with the `app/docs/` created inside the material
- Likewise, do not confuse the root `.github/` with the `app/.github/` created inside the material

### Markdown / documentation rules

- When embedding a Markdown sample inside Markdown, use four backticks for the outer code fence
- Use a `text` fence for directory tree samples
- Keep Markdown notation for code fences, tables and lists correct, bearing in mind that it must not break in PDF conversion or preview

### Custom Agent / Skill rules

- In the `tools` list of an `.agent.md`, use `search/codebase` rather than `codebase`
- Do not duplicate reusable knowledge in the Agent; split it out into a Skill

---

## Notes on specific terms

Two lines are worth unpacking, because the standard dictionary rendering loses the point.

**忖度せず (line 4 of "General").** 忖度 (sontaku) means inferring what a superior wants without being told and acting on it, usually to their advantage. It carries a negative connotation in modern Japanese usage, associated with deference that overrides honest judgement. The instruction is not merely "be objective"; it is specifically "do not quietly adjust your answer to what you think the requester wants to hear". Paired with the preceding line about not casually agreeing and asking clarifying questions, this is an explicit instruction to push back.

**ガードレールを弱めて差を演出する (under "Priorities during maintenance").** Literally "weakening the guardrails to stage a difference". This is aimed at Step 9, where the workshop compares a plain Agent against a Custom Agent. The tempting way to make a Custom Agent look impressive is to remove the Custom Instructions from the "before" case, so the baseline produces visibly worse code. The instruction forbids that: keep the guardrails identical in both cases and measure the difference that remains. Step 9 states the same principle in its own opening, and the metrics it records (reproducibility, accountability, rework) are chosen because they survive an honest comparison.

---

## Applying these rules when editing this repository

If you are editing the workshop material, whether by hand or with Copilot, the operative points are:

1. **Four-backtick outer fences** wherever a Markdown sample contains its own fence. `docs/9_CustomAgent/README_JA.md` follows this; match it.
2. **`text` fences for directory trees**, not `bash` or an unlabelled fence.
3. **Code paths are relative to `app/`**, which is the participant's workspace, not to this repository root.
4. **`docs/` here is material; `app/docs/` is something the participant generates.** The same applies to the two `.github/` directories.
5. **Do not read from `app/`** unless you are specifically verifying a procedure. It is gitignored and disposable.
6. **Chapter-to-chapter consistency outranks local polish.** A change in Step 5 that contradicts Step 9 is worse than no change.
