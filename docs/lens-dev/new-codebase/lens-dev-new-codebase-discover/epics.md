---
feature: lens-dev-new-codebase-discover
doc_type: epics
status: approved
goal: "Deliver a fully specified, regression-backed rewrite of the discover command that preserves bidirectional repo inventory sync and the governance-main auto-commit exception"
key_decisions:
  - Single epic (Epic 5) scoped to discover command; all stories are splits from baseline Story 5.4
  - BMB-first authoring applies to all lens-work file changes
  - Story 5.4.9 (integration smoke test) is a hard dev-complete gate per OQ-FP3 resolution
  - OQ-FP1 and OQ-FP2 deferred to follow-on features per finalizeplan review
depends_on: [business-plan, tech-plan, sprint-plan]
blocks: []
updated_at: 2026-04-29T00:00:00Z
---

# Epics — Discover Command

**Feature:** `lens-dev-new-codebase-discover`
**Author:** FinalizePlan bundle (lens-finalizeplan)
**Date:** 2026-04-29
**Track:** express

---

## Epic List

### Epic 5 — Discover Command Rewrite

**Goal:** Deliver the `bmad-lens-discover` command to `dev-complete` status with full behavioral specification, targeted script patches, regression test coverage, and a skill-level integration smoke test that validates the governance-main auto-commit chain end-to-end.

**Split from:** `lens-dev-new-codebase-baseline` Epic 5, Story 5.4

**Success Criteria:**
- All 8 stories (5.4.1–5.4.7 + 5.4.9) reach `dev-complete`
- Full T-series test suite passes: `uv run --with pytest pytest lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/ -q`
- Story 5.4.9 integration smoke test passes end-to-end (SKILL.md → discover-ops.py → governance-main commit)
- SKILL.md covers all behavioral paths with no gaps
- Architecture isolation audit confirms no other SKILL.md writes directly to governance main
- BMB-first authoring rules followed for all lens-work file changes

**Scope boundary:**
- In scope: SKILL.md spec, `discover-ops.py` patches, test authoring, prompt chain verification, isolation audit, integration smoke test
- Out of scope: no-remote edge case handling (deferred), config key stability test (deferred to upgrade feature review), publish-to-governance CLI changes

**Stories:**

| Story | Title | Priority | Blocking |
|---|---|---|---|
| 5.4.1 | Finalize SKILL.md behavioral spec | P0 | 5.4.2, 5.4.5, 5.4.9 |
| 5.4.2 | Patch SKILL.md: conditional auto-commit guard | P0 | 5.4.5, 5.4.9 |
| 5.4.3 | Patch discover-ops.py: path resolution via resolve() | P1 | 5.4.4 |
| 5.4.4 | Extend test suite: missing-from-disk and add-entry tests | P0 | — |
| 5.4.5 | Extend test suite: validate and no-op tests | P0 | — |
| 5.4.6 | Verify prompt stub and release prompt chain | P1 | — |
| 5.4.7 | Architecture isolation audit | P0 | — |
| 5.4.9 | Integration smoke test: full chain end-to-end | P0 | — (hard dev-complete gate) |

**Dependencies:**
- Depends on: `lens-dev-new-codebase-baseline` (baseline SKILL.md and script stubs must exist)
- Depended by: `lens-dev-new-codebase-upgrade` (config key resolution; watch for `bmadconfig.yaml` key renames at TechPlan)
- Known deferred gap: no-remote edge case (no git remote on local repo) — flagged for follow-on feature

**Estimated effort:** 2–3 focused development sessions
