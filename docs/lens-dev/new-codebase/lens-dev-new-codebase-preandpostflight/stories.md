---
feature: lens-dev-new-codebase-preandpostflight
doc_type: stories
status: approved
goal: "Provide an implementation-ready story set for the express-track preflight lifecycle redesign across cadence split, mutable sync policy, and validation hardening."
key_decisions:
  - Story PF-1.1 starts by reconciling the live target-repo preflight surface because the checked-in implementation currently exposes only `bmad-lens-preflight/scripts/light-preflight.py`.
  - Story PF-2.1 makes explicit request classification authoritative, with touched-repo detection as fallback only.
  - Stories PF-2.2 and PF-2.3 make the failure taxonomy explicit in acceptance criteria so readiness does not reopen warning-versus-hard-stop policy.
  - Story PF-3.1 creates the missing focused preflight test surface and extends git orchestration tests where mutation coverage belongs.
open_questions:
  - Should the full request-lifecycle entrypoint be introduced as `preflight.py` under `bmad-lens-preflight/scripts/`, or should another name be chosen to avoid confusion with older planning assumptions?
depends_on:
  - epics.md
  - sprint-plan.md
  - finalizeplan-review.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-preandpostflight/feature.yaml
blocks: []
updated_at: 2026-05-04T00:00:00Z
---

# Stories - PreAndPostFlight

## Story Summary

| Story ID | Title | Points | Status | Depends On |
| --- | --- | --- | --- | --- |
| PF-1.1 | Reconcile live preflight surface and preserve prompt-start contract | 2 | Ready | - |
| PF-1.2 | Add layered request-lifecycle entrypoint and cadence state | 5 | Ready | PF-1.1 |
| PF-1.3 | Implement develop-sensitive release refresh and mirror boundary | 3 | Ready | PF-1.2 |
| PF-2.1 | Add explicit request classification and sync intent resolution | 3 | Ready | PF-1.3 |
| PF-2.2 | Implement pre-request control and governance sync policy | 5 | Ready | PF-2.1 |
| PF-2.3 | Implement post-request touched-repo publish policy | 5 | Ready | PF-2.2 |
| PF-3.1 | Add focused regression coverage for cadence and sync policy | 5 | Ready | PF-2.3 |
| PF-3.2 | Align the docs contract and carry the failure taxonomy into readiness handoff | 2 | Ready | PF-3.1 |

**Total story points:** 30

---

## Story PF-1.1 - Reconcile live preflight surface and preserve prompt-start contract

**Status:** Ready
**Story Points:** 2
**Epic:** Epic 1
**Depends on:** None

**Story:**
As a Lens maintainer,
I want the live preflight implementation surface in `lens.core.src` reconciled with the
planning packet while keeping `light-preflight.py` as the frozen prompt-start gate,
so that the cadence redesign starts from the real target repo and does not break published
commands.

**Acceptance Criteria:**
1. The implementation owner is confirmed as `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/`, and story notes explicitly call out the path drift from the older `skills/lens-preflight` wording in predecessor docs.
2. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/scripts/light-preflight.py` continues to return `0` to proceed and non-zero to halt, and remains limited to cheap root, Python, and required-path validation.
3. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/SKILL.md` documents the same invocation path and supported arguments that the script actually accepts.
4. The full request-lifecycle entrypoint file under `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/scripts/` is named and reserved before cadence work begins; prompt stubs continue to call only `light-preflight.py`.
5. No control or governance repo mutation is added directly to the prompt-start gate.

**Implementation Notes:**
- The live target repo currently contains `bmad-lens-preflight/SKILL.md` and `scripts/light-preflight.py`, but no focused preflight tests or full preflight script yet.

---

## Story PF-1.2 - Add layered request-lifecycle entrypoint and cadence state

**Status:** Ready
**Story Points:** 5
**Epic:** Epic 1
**Depends on:** PF-1.1

**Story:**
As a Lens maintainer,
I want a full request-lifecycle entrypoint under `bmad-lens-preflight/scripts/` that
separates every-request gates from cadence-owned work,
so that prompt-start behavior stays cheap and the heavier lifecycle becomes explicit.

**Acceptance Criteria:**
1. A full lifecycle script is added under `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/scripts/` plus supporting helper modules for cadence state as needed.
2. The implementation separates four internal layers: every-request gates, branch-sensitive release refresh, daily hygiene, and weekly hygiene.
3. Timestamp freshness can suppress only cadence-owned layers; it never suppresses root, Python, `LENS_VERSION`, or required repo and path checks.
4. The lifecycle entrypoint records which layers ran and why in the existing preflight log stream.
5. When the lifecycle entrypoint is unavailable or fails before mutable work begins, the prompt-start gate still halts cleanly with a diagnosable error.
6. No new request-status UX surface is introduced.

**Implementation Notes:**
- Keep prompt stubs and SKILL delegation thin; lifecycle logic lives under `bmad-lens-preflight/scripts/`.

---

## Story PF-1.3 - Implement develop-sensitive release refresh and mirror boundary

**Status:** Ready
**Story Points:** 3
**Epic:** Epic 1
**Depends on:** PF-1.2

**Story:**
As a Lens maintainer,
I want release-derived refresh to be branch-sensitive to `lens.core`,
so that `develop` stays fresh on every request while stable branches avoid unnecessary
refresh churn.

**Acceptance Criteria:**
1. The current `lens.core` branch is resolved before cadence decisions are finalized.
2. If the branch is `develop`, release-derived assets refresh on every request even when timestamps are otherwise fresh.
3. If the branch is not `develop`, the refresh automatically downgrades to cadence-managed execution with no extra user setting.
4. Any retained hard-reset behavior is explicitly documented and confined to the `lens.core` mirror path only; it is never reused for control or governance repos.
5. The preflight log stream distinguishes no-op, refresh-only, and mutable-sync paths without changing the user-visible surface.

**Implementation Notes:**
- Release-derived assets include prompt and mirror outputs already sourced from `lens.core`; do not mix this story with control or governance mutation rules.

---

## Story PF-2.1 - Add explicit request classification and sync intent resolution

**Status:** Ready
**Story Points:** 3
**Epic:** Epic 2
**Depends on:** PF-1.3

**Story:**
As a Lens maintainer,
I want request classification to be explicit and available throughout the lifecycle,
so that mutable sync policy is auditable instead of inferred from side effects.

**Acceptance Criteria:**
1. The implementation exposes explicit request classes: `read-only`, `control-write`, `governance-write`, and `mixed`.
2. Classification is resolved before pre-request sync begins and remains available to post-request handling and tests.
3. Touched-repo detection is used only as the fallback execution check when explicit classification is absent or insufficient.
4. Requests that only read staged docs, governance data, or release-derived assets do not trigger mutable sync by default.
5. Classification decisions are emitted in logs or structured results in a way tests can assert.

**Implementation Notes:**
- Put classification logic under `bmad-lens-preflight/scripts/`; do not spread policy inference across prompt files or git orchestration docs.

---

## Story PF-2.2 - Implement pre-request control and governance sync policy

**Status:** Ready
**Story Points:** 5
**Epic:** Epic 2
**Depends on:** PF-2.1

**Story:**
As a Lens maintainer,
I want deterministic pre-request sync rules for the control and governance repos,
so that the system can distinguish safe no-op and warning paths from true blockers before
work begins.

**Acceptance Criteria:**
1. Pre-request policy returns explicit per-repo outcomes: `no-op`, `warn`, `pull-only`, or `block`.
2. Read-only requests treat governance freshness as warning-only unless the command explicitly requires mutable governance state before execution.
3. The hard-stop taxonomy is explicit and testable: interrupted git state, detached or wrong branch when mutation is required, auth or permission failure for required mutable sync, unresolved merge or rebase conflict, policy-blocked sync, and missing required repo context.
4. Warning-only taxonomy is explicit and testable: governance freshness on read-only requests, optional publish lag, and other non-required freshness gaps that do not block safe execution.
5. Any actual pull or reconciliation work reuses `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py` or a shared helper it owns; preflight does not embed ad hoc git write sequences.
6. The preflight log stream states separately whether control and governance pre-request sync were skipped, warned, pulled, or blocked.

**Implementation Notes:**
- Preserve the separation between `lens.core` mirror refresh and mutable control or governance sync; they are not the same policy surface.

---

## Story PF-2.3 - Implement post-request touched-repo publish policy

**Status:** Ready
**Story Points:** 5
**Epic:** Epic 2
**Depends on:** PF-2.2

**Story:**
As a Lens maintainer,
I want post-request repo publication to be explicit and touched-repo driven,
so that qualifying work pushes or publishes by default while untouched repos remain
unchanged.

**Acceptance Criteria:**
1. Post-request handling inspects touched control and governance repos explicitly and no-ops untouched repos.
2. For qualifying touched repos, the default outcome is commit plus push or publish through the git orchestration surface, not local staging only.
3. Post-request failure taxonomy is explicit and testable: auth or push failure, branch-policy failure, reconcile conflict, publish CLI failure, and other errors that occur after user-visible work completes.
4. Post-request failures surface as a distinct "work completed but sync requires reconciliation" outcome; they do not silently downgrade to success.
5. Read-only requests never auto-publish when no eligible repo was touched.
6. The post-request path preserves the current log surface and does not add a second status channel.

**Implementation Notes:**
- Use `git-orchestration-ops.py publish-to-governance` for governance mirror updates; do not hand-copy governance files.

---

## Story PF-3.1 - Add focused regression coverage for cadence and sync policy

**Status:** Ready
**Story Points:** 5
**Epic:** Epic 3
**Depends on:** PF-2.3

**Story:**
As a Lens maintainer,
I want focused regression coverage for cadence, sync policy, and failure outcomes,
so that the new layered lifecycle is provable instead of being carried only by prose.

**Acceptance Criteria:**
1. A focused preflight test module is created under `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/scripts/tests/`.
2. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-git-orchestration/scripts/tests/test-git-orchestration-ops.py` is extended where mutation-path assertions belong.
3. Tests cover: root or Python gate behavior, `lens.core` `develop` every-request refresh, non-`develop` cadence downgrade, fresh timestamps not suppressing required `develop` refresh, explicit classification precedence over touched fallback, read-only governance warning, untouched repo no-op, and touched repo default publish behavior.
4. Tests assert the hard-stop versus warning taxonomy from PF-2.2 and PF-2.3 rather than reopening the policy.
5. Windows-safe path handling and repo-root detection remain covered.
6. The existing preflight log stream remains the authoritative user-visible signal in tests.

**Implementation Notes:**
- The live target repo currently has no focused preflight test file; this story creates that surface.

---

## Story PF-3.2 - Align the docs contract and carry the failure taxonomy into readiness handoff

**Status:** Ready
**Story Points:** 2
**Epic:** Epic 3
**Depends on:** PF-3.1

**Story:**
As a Lens implementer,
I want the live preflight docs contract and downstream readiness inputs aligned to the same
policy,
so that `implementation-readiness.md` and future story files inherit the finalized
taxonomy instead of rediscovering it.

**Acceptance Criteria:**
1. `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-preflight/SKILL.md` and any related preflight references describe the live target repo paths and supported invocation contract accurately.
2. `implementation-readiness.md` copies the hard-stop versus warning taxonomy from PF-2.2 and PF-2.3 without reopening the policy question.
3. Story files generated after this bundle inherit the same failure taxonomy in their acceptance criteria.
4. Any remaining mismatch between predecessor plan wording and the live target repo layout is recorded explicitly as a handoff note.
5. FinalizePlan can proceed to readiness and story-file generation without revisiting the accepted express-plan decisions.