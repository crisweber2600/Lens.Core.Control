---
feature: lens-dev-new-codebase-techplan
story_id: "TK-3.2"
doc_type: story
status: complete
title: "Parity Regressions and Dev-Complete Gate"
priority: P1
story_points: 3
epic: "Epic 3 — Shared Utility Delivery and Parity"
depends_on: ["TK-3.1"]
blocks: []
updated_at: 2026-04-29T00:00:00Z
---

# Story TK-3.2 — Parity Regressions and Dev-Complete Gate

**Feature:** `lens-dev-new-codebase-techplan`  
**Epic:** 3 — Shared Utility Delivery and Parity  
**Priority:** P1 | **Points:** 3 | **Status:** not-started

---

## Goal

Complete the parity regression suite for `lens-techplan`, remove all TK-3.1 skip annotations, confirm the full test harness passes, and advance `feature.yaml` to `dev-complete`.

---

## Context

This is the final story in the feature. When this story is complete, the `lens-techplan` command is fully implemented, tested, and governance-registered. The dev-complete gate confirms there are no skipped tests, no open TK-3.1 dependencies, and the governance state reflects the finished feature.

**Clean-room rule:** No old-codebase skill prose reproduced. Parity regressions validate behavioral contract from baseline rewrite docs only.

---

## Acceptance Criteria

**Parity Regression Suite (six tests):**
- [ ] **PR-1 — Publish gate:** Calling `lens-techplan` when governance is out-of-date (publish hook would fail) causes the command to stop before authoring begins.
- [ ] **PR-2 — PRD reference:** Calling `lens-techplan` without a locatable PRD causes the command to stop and report the missing reference.
- [ ] **PR-3 — Adversarial review gate:** Calling `lens-techplan` when `expressplan-adversarial-review.md` is absent or not `status: responses-recorded` causes the command to stop.
- [ ] **PR-4 — Constitution load:** The conductor skill successfully loads the applicable constitution for the `lens-dev/new-codebase` domain/service context.
- [ ] **PR-5 — Delegation only:** The conductor skill delegates to the BMAD wrapper for architecture authoring — it does not produce architecture output inline.
- [ ] **PR-6 — Clean-room:** A code scan confirms no prose from the old-codebase `bmad-lens-techplan` (if it exists) has been reproduced verbatim in the new skill. If no old-codebase skill exists, mark this AC as N/A and document the check.

**Test Harness Completion:**
- [ ] All TK-3.1 skip annotations are removed from the test file.
- [ ] All six parity regressions are in the focused test file confirmed in TK-2.1.
- [ ] `uv run --with pytest pytest <test-file-path> -q` (or equivalent) exits green with 0 failures and 0 skips.

**Governance Dev-Complete Gate:**
- [ ] `feature.yaml` phase is advanced to `dev-complete` via `feature-yaml-ops.py advance-phase`.
- [ ] Governance commit pushed to `TargetProjects/lens/lens-governance` on `main`.

---

## Dev Notes

- For PR-6, the old-codebase path to check is `TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md`. If the file does not exist, the AC is N/A.
- When advancing phase to `dev-complete`: run `uv run _bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/feature-yaml-ops.py advance-phase --feature-id lens-dev-new-codebase-techplan --phase dev-complete` from the workspace root.
- After advancing phase, commit + push governance repo.

---

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

- (Updated test file from TK-2.5)
- `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-techplan/feature.yaml` (updated)
