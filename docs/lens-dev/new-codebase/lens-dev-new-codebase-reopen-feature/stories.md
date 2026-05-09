---
feature: lens-dev-new-codebase-reopen-feature
doc_type: stories
status: approved
updated_at: '2026-05-08T00:00:00Z'
---

# Stories — lens-dev-new-codebase-reopen-feature

## Epic E1 — Implement Governed Feature Reopen Capability

### S1.1 — Add `reopen` subcommand to `feature-yaml-ops.py`

**As a** Lens operator,
**I want** to run `feature-yaml-ops.py reopen --feature-id <id> --governance-repo <path> --to-phase <phase>`,
**so that** I can restore a terminal feature to active express planning without manually editing `feature.yaml`.

**Acceptance Criteria:**
- Command accepts `--feature-id` (or `--feature-path`), `--governance-repo`, `--to-phase` (default: `expressplan`), and optional `--actor`.
- Rejects non-terminal features with a clear `reopen_not_allowed` error and non-zero exit code.
- On success, `feature.yaml` has: `phase=<to-phase>`, `status=active`, no `completed_at` key, and a new `phase_transitions` entry with timestamp and actor.
- `feature-index.yaml` status is set to `active` via the existing sync helper.
- `--to-phase` validation: rejected if target phase is itself terminal; null/missing track triggers a bypass-allowed warning rather than hard failure.

**Depends on:** None (first story in epic)
**Blocks:** S1.2, S1.3

---

### S1.2 — Add automated tests for the reopen subcommand

**As a** Lens developer,
**I want** automated tests that cover reopen success, guard rejection, and index sync correctness,
**so that** reopen behavior is regression-protected.

**Acceptance Criteria:**
- Tests use a temp-dir fake `feature.yaml` fixture so no live archived feature is required.
- Success test: a `complete`/`archived` feature reopens to `expressplan`; asserts correct `phase`, `status=active`, absence of `completed_at`, and presence of `phase_transitions` entry.
- Guard test: a non-terminal feature (e.g., `phase=dev`) returns `reopen_not_allowed` with non-zero exit code.
- Index sync test: after reopen, `feature-index.yaml` status field equals `active`.
- All tests pass via `uv run python -m pytest`.

**Depends on:** S1.1
**Blocks:** None

---

### S1.3 — Update `lens-feature-yaml` SKILL.md with reopen documentation

**As a** Lens contributor,
**I want** the `lens-feature-yaml` SKILL.md to document the `reopen` subcommand,
**so that** future users know reopen exists and how to invoke it.

**Acceptance Criteria:**
- SKILL.md includes a `reopen` command entry with: args table, terminal-state precondition, default `--to-phase`, and example invocation.
- Documents accepted deferral: no dedicated `/lens-reopen` conductor in this slice.
- Documentation is consistent with the implemented command contract in S1.1.

**Depends on:** S1.1
**Blocks:** None
