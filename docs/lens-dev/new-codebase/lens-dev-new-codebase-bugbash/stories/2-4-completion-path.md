---
story_id: "2.4"
epic: "Epic 2"
feature: lens-dev-new-codebase-bugbash
title: Completion Path — Inprogress → Fixed
priority: High
size: S
status: not-started
sprint: sprint-2
updated_at: 2026-05-03T23:45:00Z
---

# Story 2.4 — Completion Path — Inprogress → Fixed

## Context

The `--complete {featureId}` flag triggers the final status transition: Inprogress → Fixed.
This is the third and final git commit in the three-commit lifecycle model.

Resolution lookup: scan `bugs/Inprogress/` for artifacts where frontmatter `featureId`
matches the supplied `{featureId}`. Any match triggers a Fixed promotion.

Guard: status must be Inprogress before promoting to Fixed. Fixed is a terminal state.

Idempotency: if bugs are already Fixed when `--complete` runs, the command is a no-op
(no double-commit, no error).

Depends on: Story 2.2 (move-to-fixed command; schema state machine).

## Tasks

1. Add `resolve-bugs` command to `bug-fixer-ops.py`:
   - Scan `bugs/Inprogress/` for artifacts where frontmatter `featureId == {featureId}`
   - If none found: return error; do not promote
   - Return JSON: `{ "resolved": [str], "not_found": [str], "already_fixed": [str] }`
2. Add `move-to-fixed` command to `bug-fixer-ops.py`:
   - Move each resolved bug from `bugs/Inprogress/{slug}.md` → `bugs/Fixed/{slug}.md`
   - Update frontmatter: status=Fixed, updated_at=now
   - Atomic move: write destination, verify, delete source
   - Return JSON: `{ "moved": [str], "failed": [{ "slug": str, "error": str }] }`
3. Wire `--complete {featureId}` into `bmad-lens-bug-fixer` SKILL.md:
   - Call `resolve-bugs`, then `move-to-fixed`
   - Create git commit: `[BUGBASH] Batch {featureId} completed`
   - Handle "none found": block Fixed promotion; explicit error report
4. Implement idempotency guard: if bugs already in Fixed, return `already_fixed`; no commit.
5. Write test: completion for same featureId twice — confirm no double-commit.
6. Commit with message: `[dev:2.4] lens-dev-new-codebase-bugbash — completion path Inprogress to Fixed`.

## Acceptance Criteria

- [ ] `/lens-bug-fixer --complete {featureId}` resolves bug artifacts linked to featureId
- [ ] Each linked bug moved from `bugs/Inprogress/{slug}.md` to `bugs/Fixed/{slug}.md`; frontmatter status=Fixed
- [ ] Git commit created: `[BUGBASH] Batch {featureId} completed`
- [ ] Bug not resolvable by featureId: Fixed promotion is blocked; explicit error report with unresolved bug slugs
- [ ] Idempotent: running completion twice for same featureId returns `already_fixed`; no double-commit

## Implementation Notes

- `resolve-bugs` output: `{ "resolved": [str], "not_found": [str], "already_fixed": [str] }`
- Guard: status must be Inprogress before promoting — validated via schema state machine (Story 1.2)
- Commit message template is exact — do not vary the format
