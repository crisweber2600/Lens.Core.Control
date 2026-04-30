---
feature: lens-dev-new-codebase-discover
story_id: "5.4.2"
doc_type: story
status: not-started
title: "Patch SKILL.md: conditional auto-commit guard"
priority: P0
story_points: 2
epic: "Epic 5 — Discover Command Rewrite"
depends_on: ["5.4.1"]
blocks: ["5.4.5", "5.4.9"]
updated_at: 2026-04-29T00:00:00Z
---

# Story 5.4.2 — Patch SKILL.md: Conditional Auto-Commit Guard

**Feature:** `lens-dev-new-codebase-discover`
**Epic:** 5 — Discover Command Rewrite
**Priority:** P0 | **Points:** 2 | **Status:** not-started

---

## Goal

The skill's auto-commit logic fires only when `repo-inventory.yaml` has actually changed. No empty commit is produced on a no-op run.

---

## Context

The auto-commit exception is the governance-critical behavior that makes `discover` unique among Lens commands. Without a pre/post hash guard, a no-op run could produce an empty commit on governance `main`, polluting git history and confusing consumers.

**Pre-work item (M1 — from adversarial review):** Before starting this story, resolve the hash comparison ownership question: Is the pre/post hash comparison logic implemented inline in the SKILL.md orchestration section, or as a script subcommand in `discover-ops.py`? The tech-plan decision is **inline in SKILL.md** (not in the script). Confirm this is still the accepted approach before coding begins.

The hash comparison flow in SKILL.md:
1. Capture SHA-256 hash of `repo-inventory.yaml` before any `add-entry` calls (pre-hash)
2. Run all `add-entry` calls
3. Capture SHA-256 hash after all `add-entry` calls complete (post-hash)
4. If pre-hash ≠ post-hash: execute `git add repo-inventory.yaml && git commit -m "[discover] Sync repo-inventory.yaml" && git push`
5. If pre-hash = post-hash: skip commit entirely

---

## Acceptance Criteria

- [ ] **Pre-work resolved:** Hash comparison ownership confirmed as inline skill logic (not script subcommand) before any edits
- [ ] Pre-mutation SHA-256 hash of `repo-inventory.yaml` is captured before any `add-entry` calls
- [ ] Post-mutation hash is captured after all `add-entry` calls complete
- [ ] `git commit` is called only when pre-hash ≠ post-hash
- [ ] No empty commit is produced on a no-op run
- [ ] SKILL.md documents the hash comparison approach in the `## Auto-Commit (Governance-Main Exception)` section

---

## Implementation Notes

- File to edit: `lens.core/_bmad/lens-work/skills/bmad-lens-discover/SKILL.md`
- Use EW via BMB for edits
- The hash comparison should use Python's `hashlib.sha256` pattern, expressed in the SKILL.md as a pseudocode or instructional step that the agent executes
- This section is the one being validated by Story 5.4.9 (integration smoke test)

---

## Definition of Done

- [ ] M1 pre-work ownership decision documented
- [ ] SKILL.md auto-commit section updated with pre/post hash guard
- [ ] All 5 acceptance criteria checked off
- [ ] Committed via BMB
