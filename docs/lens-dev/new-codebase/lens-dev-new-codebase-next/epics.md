---
feature: lens-dev-new-codebase-next
doc_type: epics
status: draft
goal: "Decompose the Next command rewrite into reviewable epics aligned with the three implementation slices."
key_decisions:
  - Slice 1 (express planning alignment) is complete — epics start at Slice 2.
  - Epic 2 requires the paused-state behavior decision before Slice 3 fixtures are written (M1 gate).
  - Epic 1 includes a precondition: trueup dev agent must finish discovery-surface writes before Next's Slice 2 begins (M2/C response).
  - Epic 3 requires the constitution resolver dependency owner to be confirmed before Slice 4 closes (H1/A response).
depends_on:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-next/finalizeplan-review.md
blocks: []
updated_at: 2026-04-30T22:15:00Z
---

# Epics — Next Command Rewrite

## Epic 1 — Prompt Chain and Discovery

**Goal:** Create the full entry-point surface for the `next` command: public stub, release
prompt, skill shell, and discovery registration. This is the minimum for the command to be
invokable.

**Scope:**
- Create `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-next.prompt.md`
  following the shared prompt-start preflight pattern (run `light-preflight.py`, stop on
  non-zero, then load the release prompt).
- Create `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-next.prompt.md`
  as a thin redirect to the owning skill.
- Scaffold `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-next/SKILL.md`
  as a thin conductor: load config, resolve governance repo and feature id, run
  `next-ops.py suggest`, apply result (fail → error, blockers → report, unblocked → delegate).
- Register `next` in the retained discovery surfaces (`module-help.csv`, `module.yaml`,
  `lens.agent.md`, or equivalent) following the same pattern as other retained commands.

**Precondition (M2):** Before starting Slice 2 discovery wiring, confirm that
`lens-dev-new-codebase-trueup` has completed its discovery-surface writes. Serialize: Next
appends after trueup finishes. Document this check in E1-S4.

**Exit Criteria:**
- Public stub runs light preflight before loading the release prompt.
- Release prompt has no inline routing logic.
- SKILL.md shell has the `next-ops.py suggest` invocation contract in place.
- `next` is discoverable as one of the retained commands.
- E1-S4 confirms trueup precondition was checked.

---

## Epic 2 — Routing Engine Parity

**Goal:** Implement `next-ops.py` with deterministic routing logic and full parity fixture
coverage including express-track and edge cases.

**Scope:**
- Implement `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-next/scripts/next-ops.py`
  reading `feature.yaml` and `lifecycle.yaml` (live file, not stubbed) to produce JSON:
  `{ phase, track, recommendation, blockers, warnings }`.
- Add parity fixtures for all routing rules in the tech plan:
  - Full-track `preplan` → `/preplan`
  - Express-track `expressplan` → `/expressplan`
  - `expressplan-complete` → `/finalizeplan` (from `auto_advance_to` in lifecycle.yaml)
  - Missing phase → track start phase
  - Blockers present → no delegation
  - Warnings only → delegation with warning surfaced
  - Unknown state → fail clearly

**Paused-state gate (M1):** Before writing Slice 3 fixtures, the Slice 2 story file
(E2-S1) must document the selected paused-state behavior. No fixture for paused state
until the behavior is chosen.

**Express-track parity (H1 note):** Parity fixtures for express routing must load the
actual installed `lifecycle.yaml`. Do not stub lifecycle contents in fixtures (Blind Spot 2).

**Exit Criteria:**
- `next-ops.py suggest` is deterministic and script-testable.
- Fixtures cover full-track, express-track, complete, missing-phase, blocker, warning, and unknown states.
- Paused-state fixture exists and matches the selected behavior documented in E2-S1.
- All fixtures load the live `lifecycle.yaml`.
- `next` remains read-only — no governance or control-doc writes from `next-ops.py`.

---

## Epic 3 — Delegation and Release Hardening

**Goal:** Complete pre-confirmed handoff, verify no-write behavior, close the constitution
resolver dependency gate, and confirm the feature is release-ready.

**Scope:**
- Implement pre-confirmed delegation in `bmad-lens-next/SKILL.md`: an unblocked `/next`
  invocation loads the target phase skill immediately, passing current feature context as
  handoff state. No second launch confirmation prompt.
- Add a negative test confirming `next` produces no governance or control-doc writes (Blind
  Spot 1 from party-mode round).
- Resolve H1: confirm `lens-dev-new-codebase-constitution` has reached at least
  `expressplan-complete` OR scope the resolver express-track allow-list fix into this Slice.
  Document the resolution in E3-S3.
- Confirm `next` remains in the retained command discovery surface after all slices are
  complete.
- Update `feature.yaml.target_repos` to include `lens.core.src`.

**Exit Criteria:**
- Unblocked recommendations load the target skill without a redundant confirmation prompt.
- Blocked recommendations do not load downstream skills.
- Negative test for no-write behavior passes.
- Constitution resolver dependency has a documented owner and regression expectation.
- Feature declared release-ready.
