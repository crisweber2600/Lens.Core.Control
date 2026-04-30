---
feature: lens-dev-new-codebase-dev
doc_type: sprint-plan
status: draft
goal: "Organize the dev command rewrite into three slices: foundation validation, core behavioral rewrite, and handoff readiness."
key_decisions:
  - "Three slices parallel the expressplan sprint structure used by other single-command rewrites in the baseline program."
  - "Slice 2 is the critical slice: it contains all three behavioral acceptance criteria from Story 5.1."
  - "Slice 3 is gated: discovery registration and all regression tests must pass before the planning PR merges."
  - "Foundation layer validation (all six internal dependencies) is a Slice 1 exit gate, not a Slice 2 assumption."
open_questions:
  - "Should E2-S1 (discovery registration) be pulled earlier — before regression tests that may require discovery to locate the command?"
depends_on:
  - lens-dev-new-codebase-finalizeplan
blocks: []
updated_at: '2026-04-30T00:00:00Z'
---

# Sprint Plan — Dev Command Rewrite

## Slice 1 — Foundation Authoring and Validation

**Goal:** Author the `bmad-lens-dev` SKILL.md conductor, then confirm the dev prompt chain,
SKILL.md conductor contract, module.yaml registration, and all six internal dependency slots
are present and correct before any behavioral work begins.

**Stories:**

- **E1-S1 — Validate dev prompt stubs**
  - Confirm `.github/prompts/lens-dev.prompt.md` exists with correct preflight invocation.
  - Confirm `lens.core/_bmad/lens-work/prompts/lens-dev.prompt.md` exists as a thin redirect.
  - Confirm both files are tracked in git in the target source repo.

- **E1-S2 — Validate `bmad-lens-dev` SKILL.md conductor contract** *(runs after E1-S5 authors the SKILL.md)*
  - Confirm publish-before-author entry hook is present.
  - Confirm conductor chain covers: entry hook → dev-session load/create → branch prep →
    constitution load → task loop → per-task commit → final PR.
  - Confirm dev-session.yaml is read and written (not bypassed).
  - Confirm write isolation: no direct file writes outside bmad-lens-git-orchestration.

- **E1-S3 — Validate `module.yaml` registration**
  - Confirm `lens-dev.prompt.md` is listed in `module.yaml` prompts section.
  - Confirm entry shape matches other retained-command entries.

- **E1-S4 — Foundation layer validation**
  - Confirm all six internal dependencies are present in the target project:
    `bmad-lens-git-orchestration`, `bmad-lens-constitution`, `bmad-lens-adversarial-review`,
    `bmad-lens-feature-yaml`, `bmad-lens-git-state`, `bmad-lens-target-repo`.
  - Confirm `dev-session.yaml` schema definition is present and matches old-codebase shape.
  - Confirm sprint-status and story file discovery paths are operational.
  - Document any gaps as blocking items; do not proceed to Slice 2 if gaps exist.

- **E1-S5 — Author `bmad-lens-dev` SKILL.md conductor**
  - Author the full conductor SKILL.md from scratch (new-codebase rewrite; no direct copy).
  - Load BMad Builder docs index (`externaldocs/bmad-builder-docs/llms-full/index.md`) and the
    domain constitution via `bmad-lens-constitution` before authoring begins (BMB-first rule).
  - Implement the full conductor chain: entry hook → dev-session load/create → branch prep →
    constitution load → task loop → per-task commit → final PR.
  - Implement write isolation: all writes routed exclusively through `bmad-lens-git-orchestration`.
  - Commit the authored SKILL.md to the target source repo.
  - Note: E1-S2 must run after this story to validate the SKILL.md contract shape.

**Exit Criteria:**
- `bmad-lens-dev` SKILL.md authored and committed (E1-S5).
- All prompt chain files present and valid.
- SKILL.md conductor contract confirmed against all behavioral requirements (E1-S2, run after E1-S5).
- `module.yaml` registration confirmed correct.
- All six internal dependency slots confirmed present or gaps explicitly flagged as blockers.
- No Slice 2 work begins until all Slice 1 exit criteria are met.

---

## Slice 2 — Core Behavioral Rewrite

**Goal:** Implement and validate all three acceptance criteria from Story 5.1, confirm
dev-session.yaml backward compatibility, and register the command in the discovery surface.

**Stories:**

- **E2-S1 — Register `lens-dev` in the retained command discovery surface**
  - Identify the discovery file used by other retained commands (same file as `lens-techplan`,
    `lens-expressplan`, etc.).
  - Register `lens-dev` following the identical pattern.
  - Commit the registration to the target source repo.
  - Note: This story is the first registration story in Slice 2 because discovery registration
    may be a prerequisite for regression tooling to locate the command.
  - **Dependency rule:** If E2-S1 fails because the discovery surface does not yet exist,
    E2-S2 through E2-S5 may proceed independently provided the regression tests do not require
    discovery to locate the command. E2-S1 failure is flagged as a dependency gap, not a
    Slice 2 blocker. The implementation agent must confirm this at the start of Slice 2.

- **E2-S2 — Regression: Target-repo-only write isolation (AC1)**
  - Verify that code changes during a dev session land only in the target repo.
  - Verify that no writes occur in the control repo or release repo during the implementation loop.
  - Test passes confirming AC1.

- **E2-S3 — Regression: Per-task commit semantics (AC2)**
  - Verify that each completed task produces exactly one atomic commit to the target branch.
  - Verify that commit scope is bounded to that task's changes.
  - Verify that per-task commits are not squashed or rebased during the session.
  - Test passes confirming AC2.

- **E2-S4 — Regression: dev-session.yaml resume and checkpoint (AC3)**
  - Verify that an interrupted session can be resumed from the last checkpoint.
  - Verify that schema remains backward-compatible (no new required fields).
  - Verify that a completed session closes with status: complete and final PR reference.
  - Test passes confirming AC3.

- **E2-S5 — Confirm publish-before-author entry hook**
  - Verify that dev stops with a clear error if feature phase is not `finalizeplan-complete`.
  - Verify that dev stops if `epics.md` or `stories.md` are absent in the governance docs path.
  - Verify that a valid finalizeplan-complete feature passes the gate and enters the task loop.

**Exit Criteria:**
- `lens-dev` registered in discovery surface (E2-S1), or gap flagged if discovery surface absent.
- All three acceptance criteria have passing regression tests (E2-S2, E2-S3, E2-S4).
- Publish-before-author hook confirmed via E2-S5.
- No unresolved fail-level gaps remain from Slice 1 foundation validation.

---

## Slice 3 — Handoff Readiness

**Goal:** Confirm the feature is dev-ready and the planning PR can be merged.

**Stories:**

- **E3-S1 — Merge planning PR and signal /dev handoff**
  - Confirm no unresolved fail-level findings in `expressplan-review.md`.
  - Confirm all Slice 1 and Slice 2 exit criteria are met.
  - Merge planning PR.
  - Signal `/dev` handoff.

**Exit Criteria:**
- Planning PR is merged.
- Feature phase is `finalizeplan-complete`.
- Dev handoff is signalled.

---

## Dependency Notes

- Slice 1 exit criteria are a hard gate for Slice 2. No regression work begins on an
  unvalidated foundation.
- Slice 2 is gated on `lens-dev-new-codebase-finalizeplan` being `finalizeplan-complete`
  before any implementation loop runs (enforced by the publish-before-author entry hook).
- Slice 3 depends on all Slice 2 regression tests passing and E2-S5 confirmed.
- If any Slice 1 foundation gap is found, a remediation pass is required before Slice 2 begins.
