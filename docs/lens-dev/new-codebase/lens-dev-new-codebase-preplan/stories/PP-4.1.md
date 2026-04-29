---
feature: lens-dev-new-codebase-preplan
story_id: PP-4.1
epic: PP-E4
title: Implement phase completion and feature.yaml update
estimate: M
sprint: 4
status: not-started
depends_on: [PP-3.1, PP-3.2, PP-3.3]
blocks: [PP-4.2]
updated_at: 2026-04-28T00:00:00Z
---

# PP-4.1 — Implement phase completion and feature.yaml update

## Story

**As a** Lens user finishing the preplan phase,  
**I want** the conductor to update `feature.yaml` to `preplan-complete` and emit a clear completion message naming `/businessplan`,  
**so that** the feature state is governed and the next action is obvious without creating a runtime dependency on businessplan availability.

## Context

Phase completion is the terminal step for the preplan conductor. After the adversarial review passes, the conductor:

1. Updates `feature.yaml` phase to `preplan-complete` via `bmad-lens-feature-yaml`.
2. Emits a phase completion message naming `/businessplan` as the expected next command. **This is a message only — no live routing call is made.** The actual routing to `/businessplan` is the user's action. (Resolves F-5 from finalizeplan-review via option B.)
3. Does NOT call `publish-to-governance` at any point.

The no-governance-write invariant end-to-end test (`test_no_governance_write_invariant`) verifies the full execution path, including phase completion, produces zero governance writes.

An independent end-to-end run of the full preplan flow must be recorded before this story closes. This means a reviewer (not the implementing developer) runs the complete preplan flow from scratch against a test feature and confirms behavioral parity.

## Acceptance Criteria

- [ ] After adversarial review passes (`pass` or `pass-with-warnings`), conductor updates `feature.yaml` phase to `preplan-complete` via `bmad-lens-feature-yaml`.
- [ ] Conductor emits a phase completion message; the message names `/businessplan` as the expected next command; it is phrased as a message/recommendation — no live routing call is issued.
- [ ] **No `publish-to-governance` call** occurs at any point during the preplan phase, including phase completion.
- [ ] `test_no_governance_write_invariant` passes end-to-end (full execution path, not just phase completion step).
- [ ] An independent end-to-end run of the full preplan flow is completed and recorded in the PR description or story closure notes.
- [ ] No parity tests regress.

## Note on `/businessplan` Forward Dependency

The businessplan command (`lens-dev-new-codebase-businessplan`) is currently at `expressplan-complete` — not yet dev-ready. Preplan's phase completion must not create a runtime dependency on businessplan being available. The message-only approach (option B from F-5 resolution) ensures preplan completes successfully regardless of businessplan availability.

## Definition of Done

- Phase completion merged to feature branch.
- `test_no_governance_write_invariant` green end-to-end.
- Independent end-to-end flow run recorded in PR.
- PR merged.
