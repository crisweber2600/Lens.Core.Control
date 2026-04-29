---
feature: lens-dev-new-codebase-preplan
doc_type: sprint-plan
status: draft
goal: "Sequence clean-room implementation of preplan command parity using shared utilities"
key_decisions:
  - Deliver command surface and parity test skeletons before implementation begins (fail-first discipline).
  - Keep baseline shared-utility prerequisites (stories 1-2, 1-3, 3-1) as explicit external blockers; do not start Sprint 3 until they are confirmed green in the new codebase.
  - Implement brainstorm-first orchestration before wiring the shared utilities, to lock in the ordering contract independently of the utility wiring.
  - Validate the no-governance-write invariant as a dedicated test, not as a by-product of other tests.
open_questions:
  - Whether bmad-lens-target-repo interruption-and-resume (PP-4.2) is accepted into scope for this sprint cycle.
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# Sprint Plan — Preplan Command

## Sprint Overview

| Sprint | Goal | Stories | Complexity Total | Risks |
|---|---|---|---|---|
| 1 | Establish command surface and parity test expectations | PP-1.1, PP-1.2, PP-1.3 | S + M + S | Test expectations may reveal gaps in shared utility APIs before implementation begins |
| 2 | Implement brainstorm-first conductor and artifact delegation | PP-2.1, PP-2.2, PP-2.3 | M + M + S | Brainstorm-first ordering must be enforced before any downstream BMAD wrapper is callable |
| 3 | Wire shared utilities (batch, review-ready, adversarial review) | PP-3.1, PP-3.2, PP-3.3 | M + M + M | Baseline stories 1-2, 1-3, and 3-1 must be confirmed green before this sprint starts |
| 4 | Phase completion, hardening, and release alignment | PP-4.1, PP-4.2 | M + S | No-governance-write invariant must pass before phase completion is declared done |

## External Prerequisites (Baseline Feature)

The following baseline stories must be complete in `TargetProjects/lens-dev/new-codebase/lens.core.src` before Sprint 3 can begin. They are owned by `lens-dev-new-codebase-baseline`, not by this feature.

| Baseline Story | What It Provides | Blocks |
|---|---|---|
| 1-2: validate-phase-artifacts shared utility | `validate-phase-artifacts.py` callable with `--phase preplan --contract review-ready` | PP-3.1 |
| 1-3: batch 2-pass contract | `bmad-lens-batch --target preplan` callable for pass 1 and pass 2 resume | PP-3.2 |
| 3-1: fix-constitution-partial-hierarchy | `bmad-lens-constitution` resolves partial hierarchy without panic | PP-2.1 (constitution load at activation), PP-3.3 |

## Sprint 1

**Goal:** Make `preplan` visible in the new codebase as a planned command surface and lock down parity expectations before any implementation begins.

| Story | Title | Estimate | Acceptance Criteria Summary | Risks |
|---|---|---:|---|---|
| PP-1.1 | Add command prompt surfaces | S | Installed stub runs light preflight and redirects to release prompt; release prompt loads `bmad-lens-preplan` and names the preplan phase outcomes | Path must stay relative to `lens.core/` in the stub redirect |
| PP-1.2 | Write SKILL.md thin conductor contract | M | SKILL.md documents analyst activation before brainstorm mode selection, user choice between `bmad-brainstorming` and `bmad-cis` modes, brainstorm-first ordering, batch delegation, review-ready delegation, no-governance-write invariant, phase completion gate, and all integration points | Contract must match the release SKILL.md behavioral specification without copying it verbatim |
| PP-1.3 | Add parity test skeletons | S | Tests fail red for analyst activation ordering, brainstorm mode choice (bmad-brainstorming path), brainstorm mode choice (bmad-cis path), brainstorm-first ordering, batch pass 1 stop, batch pass 2 resume, review-ready delegation, phase gate on fail, phase gate on pass, and no-governance-write invariant | Existing create-domain and init-feature tests must remain unchanged |

## Sprint 2

**Goal:** Implement the brainstorm-first conductor orchestration and artifact delegation wiring, before shared utilities are confirmed green.

| Story | Title | Estimate | Acceptance Criteria Summary | Risks |
|---|---|---:|---|---|
| PP-2.1 | Implement conductor activation and constitution load | M | Conductor resolves feature context, docs path, and governance mirror path; loads domain constitution via `bmad-lens-constitution`; handles partial hierarchy gracefully (relies on baseline story 3-1 being done) | Story 3-1 must be confirmed green before this story closes |
| PP-2.2 | Implement analyst activation and brainstorm mode selection | M | Interactive mode activates `bmad-agent-analyst` to frame requirements context before any authoring wrapper is invoked; after analyst framing, the conductor presents the user with a choice between `bmad-brainstorming` (divergent ideation) and `bmad-cis` (structured innovation); selected mode routes through `bmad-lens-bmad-skill`; brainstorm.md must exist regardless of mode before research or product-brief wrappers are offered | Both brainstorm modes must be tested; ordering invariant must be tested, not assumed |
| PP-2.3 | Implement research and product-brief delegation | S | After brainstorm.md exists, the conductor offers research and product-brief; routes research through the narrowest applicable canonical wrapper identifier (`bmad-domain-research`, `bmad-market-research`, or `bmad-technical-research`) and uses those exact names consistently in implementation and tests rather than shorthand aliases; routes product brief through `bmad-product-brief`; does not author artifacts directly | Research mode inference must not skip user clarification when ambiguous |

## Sprint 3

**Goal:** Wire the three shared utilities (review-ready, batch, adversarial review) so the conductor uses no inline logic for these operations.

> **Prerequisite gate:** Baseline stories 1-2, 1-3, and 3-1 must pass in `TargetProjects/lens-dev/new-codebase/lens.core.src` before this sprint starts.

| Story | Title | Estimate | Acceptance Criteria Summary | Risks |
|---|---|---:|---|---|
| PP-3.1 | Wire validate-phase-artifacts.py for review-ready | M | Conductor calls `validate-phase-artifacts.py --phase preplan --contract review-ready` on activation; `status=pass` triggers fast path to adversarial review; `status=fail` triggers normal authoring flow; no inline artifact checks remain | validate-phase-artifacts.py must be callable from the new codebase before this story can close |
| PP-3.2 | Wire bmad-lens-batch for batch mode | M | On batch pass 1, conductor calls `bmad-lens-batch --target preplan`, stops without writing lifecycle artifacts; on pass 2, pre-approved answers are loaded as context and authoring resumes; no inline `if mode == batch` logic in conductor | bmad-lens-batch (baseline story 1-3) must be callable from the new codebase before this story can close |
| PP-3.3 | Wire bmad-lens-adversarial-review for phase completion | M | At phase completion, conductor calls `bmad-lens-adversarial-review --phase preplan --source phase-complete`; `fail` verdict blocks `feature.yaml` update; `pass` or `pass-with-warnings` allows phase transition; review must run in party mode as specified by lifecycle.yaml | Constitution must be resolved before review can run (story 3-1 dependency) |

## Sprint 4

**Goal:** Complete phase transition, harden the release surface, and verify the no-governance-write invariant end to end.

| Story | Title | Estimate | Acceptance Criteria Summary | Risks |
|---|---|---:|---|---|
| PP-4.1 | Implement phase completion and feature.yaml update | M | After adversarial review passes, conductor updates `feature.yaml` phase to `preplan-complete` via `bmad-lens-feature-yaml`; no `publish-to-governance` call occurs; conductor reports "advance to `/businessplan`"; no-governance-write parity test passes | Phase update must not trigger any governance publication |
| PP-4.2 | Align help surfaces and resolve target-repo scope | S | `module-help.csv` and `lens.agent.md` include `preplan` in the 17-command surface; scope decision on `bmad-lens-target-repo` interruption-and-resume (in scope or deferred with a documented rationale) | Broader 17-command surface alignment sweep may own module-help.csv update instead |

## Cross-Sprint Dependencies

| Dependency | Blocks |
|---|---|
| PP-1.3 parity test skeletons | PP-2.1, PP-2.2, PP-2.3 (tests must fail red before implementation turns them green) |
| PP-2.1 activation + constitution load | PP-2.2, PP-2.3 (conductor must be activatable before authoring flows can run) |
| PP-2.2 brainstorm-first flow | PP-3.1 review-ready fast path (fast path only applies when artifacts already exist, which requires authoring flow to be wired) |
| Baseline story 1-2 (validate-phase-artifacts) | PP-3.1 |
| Baseline story 1-3 (bmad-lens-batch) | PP-3.2 |
| Baseline story 3-1 (constitution fix) | PP-2.1, PP-3.3 |
| PP-3.1 + PP-3.2 + PP-3.3 | PP-4.1 (all shared utility wiring must be green before phase completion is implemented) |

## Definition of Done

- The `preplan` command is available through the installed prompt stub and release prompt in `lens.core.src`.
- The SKILL.md conductor delegates to shared utilities for all non-authoring operations with no inline duplicated logic.
- Brainstorm-first ordering is enforced and covered by a passing parity test.
- Batch 2-pass contract delegates to `bmad-lens-batch` with a passing parity test.
- Review-ready fast path delegates to `validate-phase-artifacts.py` with a passing parity test.
- Phase completion gate enforces adversarial review (party mode) with a passing parity test.
- No governance writes during preplan — the invariant test passes.
- `module-help.csv` and `lens.agent.md` list `preplan` as one of the 17 retained commands.
- All existing init-feature, create-domain, and create-service parity tests remain green.
