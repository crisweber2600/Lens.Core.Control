---
feature: lens-dev-new-codebase-reopen-feature
doc_type: epics
status: approved
updated_at: '2026-05-08T00:00:00Z'
---

# Epics — lens-dev-new-codebase-reopen-feature

## E1 — Implement Governed Feature Reopen Capability

**Goal:** Add a `reopen` subcommand to `feature-yaml-ops.py` that allows terminal features (`complete`/`archived`) to be safely returned to an active express-planning phase, with feature-index synchronization and auditability.

**Scope:**
- New `reopen` subcommand with terminal-state guard
- Phase validation against track (with null-track warning bypass)
- `feature.yaml` mutation: phase, status, `completed_at` removal, `phase_transitions` append
- `feature-index.yaml` sync via existing helper
- Automated tests with temp-dir fixtures
- `lens-feature-yaml/SKILL.md` documentation update

**Stories:**
- S1.1 — Add `reopen` subcommand to `feature-yaml-ops.py`
- S1.2 — Add automated tests for the reopen subcommand
- S1.3 — Update `lens-feature-yaml` SKILL.md with reopen command documentation

**Exit Criteria:**
- `reopen` command executes cleanly on a temp-dir archived feature and produces correct `feature.yaml` and `feature-index.yaml` state
- All tests pass via `uv run python -m pytest`
- SKILL.md includes accurate `reopen` command reference
