---
feature: lens-dev-new-codebase-preplan
doc_type: epics
status: draft
goal: "Break the preplan clean-room rewrite into four epics aligned to sprint sequencing and shared-utility dependencies"
key_decisions:
  - Four epics map to four sprints; each epic owns its stories end-to-end
  - Epic PP-E3 (Shared Utility Wiring) is hard-gated on baseline stories 1-2, 1-3, and 3-1 passing in the new codebase
  - Epic PP-E4 (Phase Completion) begins only after all PP-E3 stories are green
  - No epic may introduce an owned script layer (ADR 1 — tests-only scripts/tests/ subdir is permitted)
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# Epics — Preplan Command

## Epic List

| Epic | Title | Stories | Sprint | Depends On |
|---|---|---|---|---|
| PP-E1 | Command Surface & Parity Test Expectations | PP-1.1, PP-1.2, PP-1.3 | Sprint 1 | — |
| PP-E2 | Brainstorm-First Conductor & Artifact Delegation | PP-2.1, PP-2.2, PP-2.3 | Sprint 2 | PP-E1 |
| PP-E3 | Shared Utility Wiring | PP-3.1, PP-3.2, PP-3.3 | Sprint 3 | PP-E2 + baseline 1-2, 1-3, 3-1 |
| PP-E4 | Phase Completion, Hardening & Release Alignment | PP-4.1, PP-4.2 | Sprint 4 | PP-E3 |

---

## PP-E1 — Command Surface & Parity Test Expectations

**Sprint:** 1
**Goal:** Make `preplan` visible in the new codebase as a planned command surface and lock down parity expectations before any implementation begins.

**Scope:**
- Add the `.github/prompts/lens-preplan.prompt.md` stub.
- Add the `_bmad/lens-work/prompts/lens-preplan.prompt.md` release redirect.
- Write the `bmad-lens-preplan` SKILL.md thin conductor contract (no implementation yet — contract only).
- Add all parity test skeletons in failing-red state.

**Acceptance Gate:** All PP-1.x stories done; parity tests fail red as expected (no implementation code merged yet).

**Dependencies:** None from this feature; lens.core module payload assumed present.

**Stories:** PP-1.1, PP-1.2, PP-1.3

---

## PP-E2 — Brainstorm-First Conductor & Artifact Delegation

**Sprint:** 2
**Goal:** Implement the brainstorm-first conductor orchestration and artifact delegation, enforcing analyst activation before brainstorm mode selection and `brainstorm.md` existence before downstream authoring.

**Scope:**
- Conductor activation sequence: feature context resolution, docs-path resolution, governance mirror path resolution.
- Domain constitution load via `bmad-lens-constitution` (requires baseline story 3-1).
- `bmad-agent-analyst` activation for requirements framing before any brainstorm mode is presented.
- User-facing brainstorm mode choice: `bmad-brainstorming` (divergent ideation) or `bmad-cis` (structured innovation) — both route through `bmad-lens-bmad-skill`.
- `brainstorm.md` existence gate before any research or product-brief wrapper is invoked.
- Research and product-brief delegation through `bmad-lens-bmad-skill` with canonical wrapper names.
- `/next` pre-confirmed handoff: no activation confirmation prompt when delegated from `/next`.

**Acceptance Gate:** All PP-2.x stories done; brainstorm-first ordering, analyst-first ordering, `/next` pre-confirmed handoff, and both brainstorm mode paths covered by passing parity tests.

**Dependencies:**
- PP-E1 (all parity test skeletons must fail red before this epic begins).
- Baseline story 3-1 (`bmad-lens-constitution` partial-hierarchy fix) confirmed green before PP-2.1 closes.

**Stories:** PP-2.1, PP-2.2, PP-2.3

---

## PP-E3 — Shared Utility Wiring

**Sprint:** 3
**Goal:** Wire all three shared utilities so the conductor uses zero inline logic for review-ready, batch, or adversarial review operations.

**Scope:**
- Wire `validate-phase-artifacts.py --phase preplan --contract review-ready` for the review-ready fast path (PP-3.1).
- Wire `bmad-lens-batch --target preplan` for the 2-pass batch contract (PP-3.2).
- Wire `bmad-lens-adversarial-review --phase preplan --source phase-complete` for the phase completion gate (PP-3.3).
- Confirm no-governance-write invariant passes after adversarial review wiring (PP-3.3 AC).

**Sprint 3 Prerequisite Gate:** The following baseline test suite must pass before any Sprint 3 story begins:
```bash
uv run --with pytest pytest \
  _bmad/lens-work/skills/bmad-lens-validate-phase-artifacts/scripts/tests/ \
  _bmad/lens-work/skills/bmad-lens-batch/scripts/tests/ \
  _bmad/lens-work/skills/bmad-lens-constitution/scripts/tests/ \
  -q
```
Sprint 2 Definition of Done includes confirming this gate is green.

**Acceptance Gate:** All PP-3.x stories done; Sprint 3 gate passes; no inline batch or review-ready logic remains in the conductor; no-governance-write invariant test passes.

**Dependencies:**
- PP-E2 (authoring flow wired before review-ready fast path is meaningful).
- Baseline story 1-2 (`validate-phase-artifacts.py`) — hard prerequisite for PP-3.1.
- Baseline story 1-3 (`bmad-lens-batch`) — hard prerequisite for PP-3.2.
- Baseline story 3-1 (constitution fix) — hard prerequisite for PP-3.3.

**Stories:** PP-3.1, PP-3.2, PP-3.3

---

## PP-E4 — Phase Completion, Hardening & Release Alignment

**Sprint:** 4
**Goal:** Complete the phase transition, harden the release surface, and verify the no-governance-write invariant end-to-end.

**Scope:**
- Phase completion: update `feature.yaml` to `preplan-complete` after adversarial review passes.
- Emit phase completion message naming `/businessplan` as the expected next command (message only — no live routing call; actual routing is the user's action).
- Confirm no `publish-to-governance` call is made at any point during preplan.
- Align `module-help.csv` and `lens.agent.md` with `preplan` as one of the 17 retained commands.
- Resolve the scope decision on `bmad-lens-target-repo` interruption-and-resume (in scope or documented deferral).

**Acceptance Gate:** All PP-4.x stories done; no-governance-write end-to-end parity test passes; help surfaces aligned; feature-level Definition of Done met.

**Dependencies:**
- PP-E3 (all shared utility wiring must be green before phase completion is implemented).

**Stories:** PP-4.1, PP-4.2
