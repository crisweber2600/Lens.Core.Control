---
feature: lens-dev-new-codebase-discover
story_id: "5.4.7"
doc_type: story
status: not-started
title: "Architecture isolation audit"
priority: P0
story_points: 2
epic: "Epic 5 — Discover Command Rewrite"
depends_on: []
blocks: []
updated_at: 2026-04-29T00:00:00Z
---

# Story 5.4.7 — Architecture Isolation Audit

**Feature:** `lens-dev-new-codebase-discover`
**Epic:** 5 — Discover Command Rewrite
**Priority:** P0 | **Points:** 2 | **Status:** not-started

---

## Goal

Confirm that the governance-main direct-commit pattern exists only in `bmad-lens-discover/SKILL.md` and nowhere else in the lens-work skill surface.

---

## Context

The discover command is the **only** Lens command permitted to commit directly to governance `main` outside the `publish-to-governance` CLI. This exception must be contained. If any other SKILL.md has acquired a similar direct commit pattern (accidentally or by copy-paste), it must be surfaced.

**This story is audit-only — no remediation here.** Findings are documented in `arch-review-note.md` and surfaced as issues. Remediation of any other-skill findings happens in a separate follow-on story or feature.

**Pre-work item (M2 — from adversarial review):** Confirm the scope of Story 5.4.7: does SC-5 (success criterion 5 — confirm auto-commit isolation) require audit-only or active remediation? The accepted scope is **audit-only** per this story.

Recommended to run last in the sprint, after all spec and patch work is complete, so the audit reflects final state.

---

## Audit Method

1. Search all SKILL.md files under `lens.core/_bmad/lens-work/skills/` for:
   - Direct `git push` to `origin main` (or similar) references
   - Direct `git commit` references (excluding `bmad-lens-git-orchestration` which orchestrates its own flows)
2. For each hit: determine if it is a legitimate orchestration commit (within `bmad-lens-git-orchestration`) or an out-of-scope direct governance write
3. Document all findings in `arch-review-note.md`

---

## Acceptance Criteria

- [ ] Search completed across all SKILL.md files under `lens.core/_bmad/lens-work/skills/`
- [ ] No SKILL.md other than `bmad-lens-discover/SKILL.md` contains a direct `git push` to governance `main` outside of the `publish-to-governance` CLI
- [ ] Findings (any direct-commit references found outside expected files) are documented in `docs/lens-dev/new-codebase/lens-dev-new-codebase-discover/arch-review-note.md`
- [ ] Audit confirms `bmad-lens-git-orchestration` direct commit references are scoped to its own orchestration contract (not governance writes)
- [ ] `arch-review-note.md` includes: verdict, files searched, any findings with file path and line reference, and recommended follow-up action for each finding

---

## Output Artifact

**File:** `docs/lens-dev/new-codebase/lens-dev-new-codebase-discover/arch-review-note.md`

**Contents:**
- Audit scope and method
- Files searched count
- Verdict: `ISOLATED` (no violations) or `FINDINGS` (violations found)
- Any findings: file, pattern matched, recommended action
- Sign-off note: audit complete for sprint DoD

---

## Definition of Done

- [ ] `arch-review-note.md` written and staged in docs path
- [ ] All 5 acceptance criteria checked off
- [ ] Committed to feature branch
