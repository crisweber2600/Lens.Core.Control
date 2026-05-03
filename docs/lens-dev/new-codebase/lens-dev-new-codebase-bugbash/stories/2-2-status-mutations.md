---
story_id: "2.2"
epic: "Epic 2"
feature: lens-dev-new-codebase-bugbash
title: Status Mutations — New → Inprogress → Fixed
priority: High
size: M
status: not-started
sprint: sprint-2
updated_at: 2026-05-03T23:45:00Z
---

# Story 2.2 — Status Mutations — New → Inprogress → Fixed

## Context

Bug status transitions happen atomically at phase boundaries with git-traceable commits.
The three-commit lifecycle model (from tech-plan Section 2.3 and 3.2):

1. **Feature-created commit** — after `init-feature-ops.py create` succeeds (Phase 2)
2. **→Inprogress commit** — after all bugs moved to Inprogress (Phase 3): `[BUGBASH] Batch {featureId} moved to Inprogress`
3. **→Fixed commit** — after `--complete {featureId}` (via Story 2.4): `[BUGBASH] Batch {featureId} completed`

`fix-all-new` uses commits 1 and 2 only. The Fixed commit is reserved for `--complete`.

**Per-item failure isolation (Phase 3):**
- Failed bugs remain in New; successfully moved bugs proceed to Inprogress
- Commit only lands after all attempted moves complete (success or per-item failure recorded)
- Status=Inprogress is queryable; supports manual recovery if crash between phases

File moves are **atomic**: write new file, verify, delete old file. No partial-state writes.

Depends on: Story 2.1 (feature generation must succeed before this story executes).

## Tasks

1. Add `move-to-inprogress` command to `bug-fixer-ops.py`:
   - For each slug in batch: move `bugs/New/{slug}.md` → `bugs/Inprogress/{slug}.md`
   - Update frontmatter: status=Inprogress, featureId={featureId}, updated_at=now
   - Atomic move: write destination, verify, then delete source
   - Per-item failure: record error for that slug; continue with remaining slugs
   - Return JSON: `{ "moved": [str], "failed": [{ "slug": str, "error": str }] }`
2. Wire Phase 3 into `bmad-lens-bug-fixer` SKILL.md:
   - Call `move-to-inprogress` after feature creation succeeds
   - Create git commit with exact template: `[BUGBASH] Batch {featureId} moved to Inprogress`
   - Surface per-item outcome report after commit
3. Write tests for Phase 2 failure (no Inprogress commit should be created).
4. Write tests for per-bug failure isolation during Phase 3.
5. Commit with message: `[dev:2.2] lens-dev-new-codebase-bugbash — status mutations New to Inprogress`.

## Acceptance Criteria

- [ ] After feature creation: each bug moved from `bugs/New/{slug}.md` to `bugs/Inprogress/{slug}.md` with status=Inprogress, featureId set
- [ ] Git commit created: `[BUGBASH] Batch {featureId} moved to Inprogress`
- [ ] Phase 2 failure (feature creation): all bugs remain in New; no Inprogress commit made
- [ ] Per-bug Phase 3 failure: failed bug remains in New; successfully moved bugs proceed to Inprogress; explicit per-bug error in report
- [ ] File moves are atomic (write-verify-delete); no partial states written
- [ ] `move-to-inprogress` applies scope guard before any file operation (Story 1.3)
- [ ] Schema validation applied to updated frontmatter before write (Story 1.2)

## Implementation Notes

- `move-to-inprogress` output: `{ "moved": [str], "failed": [{ "slug": str, "error": str }] }`
- Commit message template is exact — do not vary the format
- The `move-to-fixed` command (for Story 2.4 completion path) is separate from this story
