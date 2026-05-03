---
story_id: "3.1"
epic: "Epic 3"
feature: lens-dev-new-codebase-bugbash
title: Main Entry Conductor (lens-bugbash)
priority: Medium
size: M
status: not-started
sprint: sprint-3
updated_at: 2026-05-03T23:45:00Z
---

# Story 3.1 — Main Entry Conductor (lens-bugbash)

## Context

`lens-bugbash` is the unified entry point for all bugbash operations. It routes to
`bug-reporter` or `bug-fixer` based on the supplied flag. It also exposes a `--status`
command for quick queue inspection.

**3-hop chain:**
```
.github/prompts/lens-bugbash.prompt.md  (stub)
  → lens.core/_bmad/lens-work/prompts/lens-bugbash.prompt.md  (release prompt)
    → skills/bmad-lens-bugbash/SKILL.md  (thin conductor — routing only)
      → scripts/bugbash-ops.py  (status-summary) or delegation to bug-reporter / bug-fixer
```

The SKILL.md does not implement bug operations inline — it routes by flag and delegates.

Depends on: All Sprint 1 and Sprint 2 stories complete (delegation targets must exist).

## Tasks

1. Create `lens.core/_bmad/lens-work/scripts/bugbash-ops.py` with `status-summary` command:
   - Count files in `bugs/New/`, `bugs/Inprogress/`, `bugs/Fixed/`
   - Return JSON: `{ "New": int, "Inprogress": int, "Fixed": int }`
   - Apply scope guard before scanning
2. Create `lens.core/_bmad/lens-work/skills/bmad-lens-bugbash/SKILL.md` via **bmad-module-builder** using the 7-section template from tech-plan Section 6.
3. Create `lens.core/_bmad/lens-work/prompts/lens-bugbash.prompt.md` (release prompt) via **bmad-workflow-builder**.
4. Create `.github/prompts/lens-bugbash.prompt.md` (stub) with invariant pattern:
   - Runs `light-preflight.py`; exits on non-zero
   - Loads `lens.core/_bmad/lens-work/prompts/lens-bugbash.prompt.md`
5. Wire flag routing in SKILL.md:
   - `--report` → delegate to `bmad-lens-bug-reporter`
   - `--fix-all-new` → delegate to `bmad-lens-bug-fixer --fix-all-new`
   - `--complete {featureId}` → delegate to `bmad-lens-bug-fixer --complete {featureId}`
   - `--status` → call `bugbash-ops.py status-summary`, print formatted summary
   - No flags → print help menu with all flags and descriptions
6. Commit with message: `[dev:3.1] lens-dev-new-codebase-bugbash — main entry conductor`.

## Acceptance Criteria

- [ ] `/lens-bugbash --report` delegates to bug-reporter workflow
- [ ] `/lens-bugbash --fix-all-new` delegates to bug-fixer batch workflow
- [ ] `/lens-bugbash --complete {featureId}` delegates to bug-fixer completion workflow
- [ ] `/lens-bugbash` with no flags displays help menu listing all flags and descriptions
- [ ] `/lens-bugbash --status` prints count of bugs in each status (New, Inprogress, Fixed)
- [ ] `.github/prompts/lens-bugbash.prompt.md` stub exists and invokes light-preflight.py
- [ ] `lens.core/_bmad/lens-work/prompts/lens-bugbash.prompt.md` release prompt exists and delegates to SKILL.md
- [ ] `lens.core/_bmad/lens-work/skills/bmad-lens-bugbash/SKILL.md` exists (via bmad-module-builder)
- [ ] `lens.core/_bmad/lens-work/scripts/bugbash-ops.py` status-summary command works

## Implementation Notes

- `bugbash-ops.py status-summary` output: `{ "New": int, "Inprogress": int, "Fixed": int }`
- BMB-First protocol: Python script authored directly; SKILL.md via bmad-module-builder; release prompt via bmad-workflow-builder
