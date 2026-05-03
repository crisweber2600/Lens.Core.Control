---
feature: lens-dev-new-codebase-bugfix-commands-should-have-run-via-lens-4
title: "Bugbash: Commands Should Have Run Via Lens 4F788235"
doc_type: business-plan
status: draft
track: express
phase: expressplan
created_at: 2026-05-03
---

# Business Plan — Commands Should Have Run Via Lens

## Problem

When `/lens-bug-fixer --fix-all-new` reaches Phase 4 (expressplan execution), it attempts
to delegate to expressplan as a shell command (`lens-expressplan plan ...`). No such shell
command exists. This causes Phase 4 to fail with `command not found`, leaving bugs stuck
in Inprogress and the expressplan lifecycle stage incomplete.

## Root Cause

The `bmad-lens-bug-fixer` SKILL.md Phase 4 instruction says "Delegate to `bmad-lens-expressplan`
skill." The phrasing is ambiguous — a conductor (LLM) can read "delegate" as "run a shell
command" rather than "load the SKILL.md and execute it in-context." There is no registered
Lens shell command named `lens-expressplan`.

## Target Users

- LENS workbench users running `/lens-bug-fixer` to batch-fix governance bugs in the
  `new-codebase` scope.

## Business Goals

1. Phase 4 of the bug-fixer lifecycle completes end-to-end without conductor error.
2. Express planning artifacts are produced for every bugfix batch feature.
3. Bugs advance from Inprogress to the expressplan-complete stage as designed.

## Scope

**In scope:**
- Update `bmad-lens-bug-fixer/SKILL.md` Phase 4 instructions to unambiguously describe
  skill delegation (load SKILL.md, follow it) rather than a shell command.

**Out of scope:**
- Changes to `bug-fixer-ops.py` or any other script.
- Changes to the `bmad-lens-expressplan` skill itself.
- Retroactive repair of bugs stuck in Inprogress from prior failed runs.

## Dependencies

- `bmad-lens-expressplan/SKILL.md` — the target skill being delegated to.
- `lens.core.src` source repo on branch `fix/preflight-old-patterns` or a new branch.

## Risks

| Risk | Mitigation |
|------|-----------|
| Other conductors may have the same ambiguity | Audit other skills that say "delegate to X skill" and apply consistent phrasing |
| Fix ships in source but not in release until promotion | Covered by the promote-to-release CI/CD workflow |

## Success Criteria

- `/lens-bug-fixer --fix-all-new` executes Phase 4 without `command not found`.
- `expressplan-adversarial-review.md` is produced after a successful run.
- `feature.yaml.phase` advances to `expressplan-complete`.
