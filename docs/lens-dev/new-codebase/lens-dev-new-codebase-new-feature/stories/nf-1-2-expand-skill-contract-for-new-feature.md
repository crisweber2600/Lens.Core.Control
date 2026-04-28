# Story NF-1.2: Expand Skill Contract for new-feature

**Feature:** lens-dev-new-codebase-new-feature  
**Epic:** 1 — Command Surface and Parity Foundation  
**Estimate:** M  
**Sprint:** 1  
**Status:** backlog  
**Depends on:** NF-1.1 (prompt surfaces exist)  
**Blocks:** NF-2.1 (spec baseline for implementation)  
**Updated:** 2026-04-27

---

## Goal

Update `bmad-lens-init-feature/SKILL.md` in the new codebase to document the full `new-feature` interaction flow. This is the contract the AI agent reads when invoked — it must be accurate, complete, and not promise anything the script does not yet deliver.

---

## Acceptance Criteria

- [ ] SKILL.md documents the **progressive disclosure flow**:
  1. Collect feature name, domain, and service from user
  2. Require explicit track selection (`full`, `feature`/`quickplan`, `express`)
  3. Invoke `init-feature-ops.py create` with all resolved arguments
- [ ] SKILL.md documents all required **`create` output fields**:
  - `featureId`, `featureSlug`
  - `starting_phase`, `recommended_command`, `router_command`
  - `governance_git_commands`, `governance_git_executed`, `governance_commit_sha`
  - `control_repo_git_commands`, `control_repo_activation_commands`
  - `remaining_git_commands`, `remaining_commands`
  - `gh_commands`, `planning_pr_created`
- [ ] SKILL.md references **`fetch-context`** with:
  - When to invoke it: at feature init to load related summaries and dependency docs for LLM context
  - The subcommand call signature: `init-feature-ops.py fetch-context --governance-repo ... --feature-id ... --depth summaries`
  - What to do with the returned paths (load them into context before presenting recommended command to user)
- [ ] SKILL.md documents **failure behavior**:
  - Missing `--track`: explicit error, user prompted to select track
  - Invalid domain/service/slug: `status: fail` + message, no file writes
  - Duplicate feature ID: `status: fail` before writes
  - Dirty governance repo with `--execute-governance-git`: `status: fail` before writes
- [ ] No SKILL.md section promises behavior not in the current script contract (no aspirational content)
- [ ] SKILL.md is consistent with ADR 6 (fetch-context) in tech-plan.md

---

## Technical Context

### SKILL.md Location in New Codebase

```
TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md
```

### Old Codebase Reference

```
TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md
```

Read the old SKILL.md for structural reference and existing section headings. The new SKILL.md must add:
- The `new-feature` command sections (not present in old codebase — old codebase had `init-feature`)
- The `fetch-context` reference (ADR 6, new as of this feature)
- Accurate output field documentation reflecting the new codebase script contract

### Auto-Context Pull Reference

The old codebase has `./references/auto-context-pull.md` that documents the fetch-context-then-load pattern. Check if this file exists in the new codebase. If yes, reference it from SKILL.md. If no, document the pattern inline.

```
TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/references/auto-context-pull.md
```

### Track Names

| Track | Alias | Start Phase |
|---|---|---|
| `full` | — | `preplan` |
| `feature` | `quickplan` | `businessplan` |
| `express` | — | `expressplan` |

The `quickplan` alias maps to `feature` track. SKILL.md should document both names.

### Express Track Behavior

When track is `express`:
- `gh_commands` is `[]`
- `planning_pr_created` is `false`
- SKILL.md must NOT prompt the user to create a planning PR after express creation

---

## Test Requirements

No automated tests for SKILL.md content. Validation is performed during code review:
1. Reviewer checks SKILL.md against the script contract in NF-2.1
2. Reviewer confirms fetch-context reference matches ADR 6 exactly
3. Reviewer confirms no aspirational content (nothing promised that the script does not yet deliver at the time of the review)

SKILL.md will be re-validated as NF-2.1 through NF-4.3 complete. If the script adds capabilities that SKILL.md did not document, update SKILL.md in the relevant story.

---

## Definition of Done

- `bmad-lens-init-feature/SKILL.md` updated in `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- All acceptance criteria above checked
- Auto-context pull pattern documented (inline or by reference)
- SKILL.md reviewed against tech-plan.md ADR 2–6 for consistency
- No promises of unimplemented behavior
