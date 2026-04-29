---
feature: lens-dev-new-codebase-preplan
story_id: PP-3.3
epic: PP-E3
title: Wire bmad-lens-adversarial-review for phase completion
estimate: M
sprint: 3
status: not-started
depends_on: [PP-3.1, PP-3.2]
blocks: [PP-4.1]
prerequisite: "Sprint 3 gate verification command passes; baseline story 3-1 confirmed green in lens.core.src"
updated_at: 2026-04-28T00:00:00Z
---

# PP-3.3 — Wire bmad-lens-adversarial-review for phase completion

## Story

**As a** Lens user completing preplan,  
**I want** the phase completion adversarial review to run through the shared `bmad-lens-adversarial-review` skill in party mode,  
**so that** the phase gate is consistently enforced and the review report is produced before any feature.yaml update.

## Context

The phase completion gate is the final quality control step before preplan artifacts are considered complete. It must:
1. Call `bmad-lens-adversarial-review --phase preplan --source phase-complete` in party mode.
2. Treat `fail` as a hard blocker on the `feature.yaml` phase update.
3. Treat `pass` or `pass-with-warnings` as approval to proceed with the phase update.

**Critical invariant:** The adversarial review wiring must NOT introduce any governance write path. After this story is merged, the no-governance-write invariant parity test must still pass. This is an explicit acceptance criterion — the no-governance-write test is run as part of this story's verification, not deferred to PP-4.1.

**Sprint 3 Prerequisite Gate:** Same gate as PP-3.1 and PP-3.2.

**Hard Prerequisite:** Baseline story 3-1 (`bmad-lens-constitution` partial-hierarchy fix) confirmed green — the adversarial review calls the constitution at its entry.

## CLI Invocation (Exact)

```bash
bmad-lens-adversarial-review --phase preplan --source phase-complete
```

Mode: party (as specified by `lifecycle.yaml` phase definition for `preplan.completion_review.mode: party`).

## Acceptance Criteria

- [ ] At phase completion, conductor calls `bmad-lens-adversarial-review --phase preplan --source phase-complete`.
- [ ] Review runs in party mode as specified by `lifecycle.yaml`.
- [ ] `fail` verdict blocks `feature.yaml` phase update — no state mutation occurs.
- [ ] `pass` or `pass-with-warnings` allows the phase transition to proceed.
- [ ] **No-governance-write invariant test still passes after adversarial review wiring** — run `test_no_governance_write_invariant` explicitly as part of this story's PR verification; the adversarial review wiring introduces no governance write path.
- [ ] `test_phase_gate_on_fail` and `test_phase_gate_on_pass` parity tests turn green.
- [ ] Baseline story 3-1 confirmed green in `lens.core.src` (record in PR description).
- [ ] Sprint 3 gate verification command confirmed passing.
- [ ] No existing parity tests regress.

## Definition of Done

- Adversarial review wiring merged to feature branch.
- `test_phase_gate_on_fail`, `test_phase_gate_on_pass`, and `test_no_governance_write_invariant` all green.
- Baseline story 3-1 and Sprint 3 gate confirmation recorded in PR.
