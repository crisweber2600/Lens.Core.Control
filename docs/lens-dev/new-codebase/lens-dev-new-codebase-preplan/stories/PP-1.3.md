---
feature: lens-dev-new-codebase-preplan
story_id: PP-1.3
epic: PP-E1
title: Add parity test skeletons
estimate: S
sprint: 1
status: not-started
depends_on: [PP-1.2]
blocks: [PP-2.1, PP-2.2, PP-2.3]
updated_at: 2026-04-28T00:00:00Z
---

# PP-1.3 — Add parity test skeletons

## Story

**As a** developer implementing the preplan conductor,  
**I want** parity test skeletons that fail red before any implementation code is merged,  
**so that** the TDD discipline is enforced and no behavior is shipped without a corresponding green test.

## Context

Parity tests are the contract verifier for the SKILL.md behavioral spec. They must exist and fail red *before* implementation begins, which ensures each implementation PR is verified by a specific test turning green rather than tests being written after the fact.

The test file follows the `test-{skill}-ops.py` naming convention established by peer skills:
- `bmad-lens-switch` → `skills/bmad-lens-switch/scripts/tests/test-switch-ops.py`
- `bmad-lens-init-feature` → `skills/bmad-lens-init-feature/scripts/tests/test-init-feature-ops.py`
- **preplan** → `skills/bmad-lens-preplan/scripts/tests/test-preplan-ops.py`

This path is a `scripts/tests/` subpath permitted by ADR 1 (test-only fixtures, not implementation scripts).

## Implementation Target

`TargetProjects/lens-dev/new-codebase/lens.core.src/`

## File to Create

```
_bmad/lens-work/skills/bmad-lens-preplan/scripts/tests/test-preplan-ops.py
```

## Parity Test Categories (All Must Fail Red Initially)

| Test Category | What It Verifies |
|---|---|
| `test_analyst_activation_ordering` | `bmad-agent-analyst` is activated before any brainstorm wrapper is invoked |
| `test_brainstorm_mode_choice_brainstorming_path` | When user selects `bmad-brainstorming`, the wrapper is invoked via `bmad-lens-bmad-skill` |
| `test_brainstorm_mode_choice_cis_path` | When user selects `bmad-cis`, the wrapper is invoked via `bmad-lens-bmad-skill` |
| `test_brainstorm_first_ordering` | No research or product-brief wrapper is callable before `brainstorm.md` exists in docs path |
| `test_batch_pass1_stop` | On batch pass 1, conductor stops after writing `preplan-batch-input.md`; no lifecycle artifacts written |
| `test_batch_pass2_resume` | On batch pass 2, `batch_resume_context` is loaded and authoring resumes |
| `test_review_ready_delegation` | Conductor calls `validate-phase-artifacts.py --phase preplan --contract review-ready`; no inline artifact checks |
| `test_phase_gate_on_fail` | A `fail` adversarial review verdict blocks `feature.yaml` phase update |
| `test_phase_gate_on_pass` | A `pass` or `pass-with-warnings` verdict allows phase transition |
| `test_no_governance_write_invariant` | `publish-to-governance` is not invoked at any step of preplan execution |
| `test_next_pre_confirmed_handoff` | When activated via `/next` delegation, no launch confirmation prompt is shown; preplan begins immediately |
| `test_fetch_context_availability` | `bmad-lens-init-feature fetch-context` is callable in the new codebase (surfacing any implementation gap) |

## Acceptance Criteria

- [ ] Test file `_bmad/lens-work/skills/bmad-lens-preplan/scripts/tests/test-preplan-ops.py` exists in `lens.core.src`.
- [ ] All 12 parity test categories listed above fail red when no implementation code is present.
- [ ] Existing create-domain, init-feature, and create-service parity tests remain unchanged and continue to pass.
- [ ] Test file follows `test-{skill}-ops.py` naming convention.
- [ ] No implementation code is added in this story (test skeletons only).

## Definition of Done

- Test file committed; all tests fail red; existing tests green.
- PR merged to feature branch.
