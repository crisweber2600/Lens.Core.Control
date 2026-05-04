---
feature: lens-dev-new-codebase-preandpostflight
doc_type: epics
status: approved
goal: "Convert the reviewed express-track preflight packet into three implementation epics that preserve the frozen prompt-start contract while making request-lifecycle sync explicit."
key_decisions:
  - Keep the three sprint slices from sprint-plan and turn each into a reviewable development epic.
  - Treat the live target repo path `_bmad/lens-work/skills/lens-preflight/` as the implementation authority, even where predecessor prose referenced older `bmad-lens-*` nesting.
  - Preserve `light-preflight.py` as the cheap prompt-start gate and move heavier request-lifecycle work into the full preflight path plus helpers under the same target-repo surface.
  - Route mutable control and governance repo writes through `skills/lens-git-orchestration` and its shared repo-sync helper instead of ad hoc preflight git commands.
  - Encode warning-versus-hard-stop behavior directly in story acceptance criteria so downstream readiness can inherit the same taxonomy without reopening policy.
open_questions:
  - Resolved: `preflight.py` remains the full request-lifecycle entrypoint under `_bmad/lens-work/skills/lens-preflight/scripts/`, with shared git sync primitives owned by `_bmad/lens-work/skills/lens-git-orchestration/scripts/repo_sync.py`.
depends_on:
  - business-plan.md
  - tech-plan.md
  - sprint-plan.md
  - expressplan-adversarial-review.md
  - finalizeplan-review.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-preandpostflight/feature.yaml
blocks: []
updated_at: 2026-05-04T00:00:00Z
---

# Epics - PreAndPostFlight

Express-track downstream note: these epics convert the reviewed express packet into the
internal FinalizePlan bundle for implementation in
`TargetProjects/lens-dev/new-codebase/lens.core.src`. They preserve the public prompt
contract and focus on the request-lifecycle behavior behind preflight.

## Epic Overview

| Epic | Slice | Title | Status | Primary target surfaces |
| --- | --- | --- | --- | --- |
| Epic 1 | PF-1.1 | Cadence Split and Branch-Sensitive Refresh | Done | `_bmad/lens-work/skills/lens-preflight/SKILL.md`, `_bmad/lens-work/skills/lens-preflight/scripts/light-preflight.py`, `_bmad/lens-work/skills/lens-preflight/scripts/preflight.py` |
| Epic 2 | PF-2.1 | Explicit Request Lifecycle Sync Policy | Done | `_bmad/lens-work/skills/lens-preflight/scripts/`, `_bmad/lens-work/skills/lens-git-orchestration/scripts/git-orchestration-ops.py`, `_bmad/lens-work/skills/lens-git-orchestration/scripts/repo_sync.py` |
| Epic 3 | PF-3.1 | Validation Hardening and Readiness Handoff | Done | `_bmad/lens-work/skills/lens-preflight/scripts/tests/`, `_bmad/lens-work/skills/lens-git-orchestration/scripts/tests/test-git-orchestration-ops.py`, preflight skill docs |

---

## Epic 1 - Cadence Split and Branch-Sensitive Refresh (PF-1.1)

**Status:** Done.

**Goal:** Restructure the target-repo preflight implementation into explicit request-time
layers without breaking the frozen prompt-start gate used by the express-track command
surface.

**Scope:**
- Reconcile the planning packet with the live target repo layout under
  `_bmad/lens-work/skills/lens-preflight/`.
- Preserve `light-preflight.py` as the cheap gate and align `SKILL.md` with the real
  invocation contract.
- Add the full lifecycle entrypoint plus cadence helpers needed to separate every-request
  checks, branch-sensitive release refresh, and daily or weekly hygiene.
- Implement the `lens.core` `develop` refresh rule and the automatic downgrade back to
  slower cadence when the mirror is no longer on `develop`.
- Keep the existing preflight log stream as the only user-visible state signal.

**Exit Criteria:**
- Prompt-start gating remains cheap, deterministic, and backward-compatible for published
  stubs.
- Heavy request-lifecycle work lives behind an explicit full preflight entrypoint in the
  target repo.
- Release-derived assets refresh on every request when `lens.core` tracks `develop`.
- Non-`develop` branches automatically fall back to cadence-managed refresh without extra
  settings.
- No mutable control or governance sync is hidden inside the prompt-start gate.

---

## Epic 2 - Explicit Request Lifecycle Sync Policy (PF-2.1)

**Status:** Done.

**Goal:** Make control and governance repo mutation an explicit request-lifecycle policy
with auditable no-op, warning, pull, commit, and publish outcomes.

**Scope:**
- Introduce explicit request classification (`read-only`, `control-write`,
  `governance-write`, `mixed`) with touched-repo detection only as the fallback.
- Define pre-request policy for control and governance freshness, branch safety, auth, and
  clean-state checks.
- Define post-request touched-repo policy using `git-orchestration-ops.py` for actual
  writes and publication.
- Make the failure taxonomy explicit across hard-stop, warning, and reconcile-later
  outcomes.
- Preserve the `lens.core` mirror policy as separate from control or governance sync.

**Exit Criteria:**
- Read-only requests warn on governance freshness instead of hard-blocking.
- Qualifying touched repos default to post-request commit, push, or publish.
- Untouched repos no-op.
- Hard-stop and warning classes are explicit and testable.
- Control and governance writes route through existing git orchestration surfaces, not ad
  hoc shell or script commands.

---

## Epic 3 - Validation Hardening and Readiness Handoff (PF-3.1)

**Status:** Done.

**Goal:** Prove cadence and sync policy with focused tests and carry the same failure
taxonomy into downstream readiness artifacts.

**Scope:**
- Add focused tests for gate behavior, cadence, branch-sensitive refresh, explicit
  classification, touched-repo handling, and failure outcomes.
- Reuse or extend `test-git-orchestration-ops.py` for write-path assertions that belong on
  the shared git orchestration surface.
- Align `skills/lens-preflight/SKILL.md` and related notes with the live target repo paths
  and invocation contract.
- Treat the story acceptance criteria as the source for the readiness failure matrix to be
  echoed in `implementation-readiness.md` and future story files.
- Capture any target-repo path drift or helper-file naming decision as a handoff note, not
  as hidden implementation trivia.

**Exit Criteria:**
- Focused regression coverage exists for cadence, sync policy, and warning-versus-hard-stop
  behavior.
- The current preflight log stream remains the authoritative user-visible signal.
- Readiness handoff can lift the failure taxonomy directly from the story set without
  reopening scope.
- Remaining risks are reduced to execution details, not policy ambiguity.