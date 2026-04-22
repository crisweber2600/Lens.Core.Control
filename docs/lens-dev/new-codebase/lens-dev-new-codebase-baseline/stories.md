---
feature: lens-dev-new-codebase-baseline
doc_type: stories
status: in-review
goal: "Regenerate the rewrite backlog as a clean 21-story implementation sequence derived from the current planning source documents."
key_decisions:
  - "Use the PRD, architecture, and FinalizePlan review as the only regeneration inputs."
  - "Preserve the 21-story inventory and existing story keys so the current `stories/` folder remains addressable."
  - "Make Story 3.1 the hard prerequisite for all planning-conductor work and assign release-delivery ownership to Story 5.5."
open_questions: []
depends_on:
  - epics.md
  - prd.md
  - architecture.md
  - finalizeplan-review.md
blocks: []
updated_at: 2026-04-22T23:59:00Z
---

# Stories: lens-dev-new-codebase-baseline

## Overview

This backlog restates the rewrite as a fresh 21-story sequence built from the current planning source of truth rather than the earlier reset-era backlog. It keeps the stable story IDs already used in the `stories/` folder, preserves the architecture's work-package ordering, and folds FinalizePlan carry-forward items into the release gate instead of leaving them as advisory notes.

## Sequencing Rules

- Stories 1.2, 1.3, and 1.4 establish the shared contracts that planning conductors consume.
- Stories 2.1 through 2.6 restore safe workflow entry, identity, and navigation before planning or execution rewrites depend on them.
- Story 3.1 must complete before Stories 4.1 through 4.5 begin.
- Story 4.4 depends on Story 4.3 because FinalizePlan bundles reviewed TechPlan output.
- Story 5.5 is the terminal release gate and owns upgrade notes, removed-command communication, and parity evidence.

## Backlog Summary

| Epic | Focus | Stories | Story Points |
|---|---|---:|---:|
| Epic 1 | Published surface and shared contracts | 4 | 23 |
| Epic 2 | Workflow entry, identity, and navigation | 6 | 25 |
| Epic 3 | Constitution resolution | 1 | 5 |
| Epic 4 | Planning conductors | 5 | 31 |
| Epic 5 | Delivery, closure, inventory, and release | 5 | 31 |
| Total | Full rewrite backlog | 21 | 115 |

## Story List

### Epic 1: Stable Published Surface and Shared Lifecycle Contracts

#### Story 1.1: Stable Published Surface and Discovery Parity

- Story Key: `1-1-scaffold-published-surface`
- Priority: Must
- Story Points: 8
- Depends On: None
- Outcome: Rebuild the public 17-command surface and every discovery surface so they agree everywhere the user can invoke Lens.

**Acceptance Focus:**

- Exactly 17 retained public commands remain published across prompt stubs, help surfaces, manifests, and adapters.
- Retained internal runtime dependencies stay present even when their public stubs are removed.
- `split-feature` remains visible on every published discovery surface.

#### Story 1.2: Shared Review-Ready Artifact Validation

- Story Key: `1-2-validate-phase-artifacts-shared-utility`
- Priority: Must
- Story Points: 5
- Depends On: `1-1-scaffold-published-surface`
- Outcome: Centralize review-ready artifact validation so all planning conductors use the same lifecycle gate.

**Acceptance Focus:**

- `validate-phase-artifacts.py` returns a clean success result for complete, reviewed artifacts.
- Missing or non-reviewed artifacts fail with explicit diagnostics.
- Planning conductors delegate to the shared validator instead of carrying local gate logic.

#### Story 1.3: Shared Two-Pass Batch Contract

- Story Key: `1-3-batch-two-pass-contract`
- Priority: Must
- Story Points: 5
- Depends On: `1-1-scaffold-published-surface`
- Outcome: Standardize planning batch intake and resume behavior behind `bmad-lens-batch`.

**Acceptance Focus:**

- Pass 1 writes the correct `{phase}-batch-input.md` file and stops.
- Pass 2 resumes with approved context and forwards it to downstream skills in order.
- Batch execution stops at the first failing delegated target.

#### Story 1.4: Canonical Publish-Before-Author Governance Hook

- Story Key: `1-4-publish-to-governance-entry-hook`
- Priority: Must
- Story Points: 5
- Depends On: `1-1-scaffold-published-surface`
- Outcome: Ensure reviewed predecessor artifacts are published through one hook before downstream authoring begins.

**Acceptance Focus:**

- `businessplan`, `techplan`, `finalizeplan`, and `dev` all use the shared publish hook.
- The hook is a safe no-op when nothing new needs publication.
- No planning conductor performs direct governance writes outside this path.

### Epic 2: Safe Feature Creation and Navigation

#### Story 2.1: Rewrite Preflight Entry and Workspace Validation

- Story Key: `2-1-rewrite-preflight`
- Priority: Must
- Story Points: 3
- Depends On: `1-1-scaffold-published-surface`
- Outcome: Keep `light-preflight.py` as the frozen entry gate and preserve safe workspace validation.

**Acceptance Focus:**

- Every retained public command fires `light-preflight.py` before prompt redirection.
- Healthy workspaces continue without feature-state mutation.
- Missing repos, config, or tooling fail clearly and non-destructively.

#### Story 2.2: Rewrite New Domain

- Story Key: `2-2-rewrite-new-domain`
- Priority: Must
- Story Points: 3
- Depends On: `2-1-rewrite-preflight`
- Outcome: Restore domain creation with stable naming, markers, and constitution scaffolding.

**Acceptance Focus:**

- Domain markers and constitution scaffolds land in the expected governance path.
- Duplicate-domain creation does not overwrite existing artifacts.
- The new domain becomes usable by downstream service and feature setup flows immediately.

#### Story 2.3: Rewrite New Service

- Story Key: `2-3-rewrite-new-service`
- Priority: Must
- Story Points: 3
- Depends On: `2-2-rewrite-new-domain`
- Outcome: Restore service creation under an existing domain while preserving inherited governance context.

**Acceptance Focus:**

- Service markers are written only when the parent domain exists.
- Domain-to-service inheritance remains intact.
- Duplicate or invalid service setup fails without partial writes.

#### Story 2.4: Rewrite New Feature

- Story Key: `2-4-rewrite-new-feature`
- Priority: Must
- Story Points: 8
- Depends On: `2-2-rewrite-new-domain`, `2-3-rewrite-new-service`
- Outcome: Rebuild canonical feature creation, feature-index registration, and the two-branch topology.

**Acceptance Focus:**

- `featureId` keeps the `{domain}-{service}-{featureSlug}` formula and persists `featureSlug` separately.
- Control-repo branch creation remains exactly `{featureId}` and `{featureId}-plan`.
- Target-repo metadata and branch mode remain compatible with downstream `/dev` work.

#### Story 2.5: Rewrite Switch

- Story Key: `2-5-rewrite-switch`
- Priority: Must
- Story Points: 3
- Depends On: `2-4-rewrite-new-feature`
- Outcome: Preserve safe, read-only feature context switching.

**Acceptance Focus:**

- Switching changes session context but does not write lifecycle or governance state.
- Invalid selections fail without altering the current active context.
- Downstream commands read the new feature context consistently after a successful switch.

#### Story 2.6: Rewrite Next

- Story Key: `2-6-rewrite-next`
- Priority: Must
- Story Points: 5
- Depends On: `2-1-rewrite-preflight`, `2-4-rewrite-new-feature`, `2-5-rewrite-switch`
- Outcome: Preserve blocker-first single-choice routing with the pre-confirmed handoff contract.

**Acceptance Focus:**

- `next` selects one unblocked action, not a menu of loosely ranked options.
- Delegated phase skills do not re-ask whether to proceed.
- Blockers stop delegation and are surfaced first.

### Epic 3: Reliable Constitution Resolution

#### Story 3.1: Fix Partial-Hierarchy Constitution Resolution

- Story Key: `3-1-fix-constitution-partial-hierarchy`
- Priority: Must
- Story Points: 5
- Depends On: `1-1-scaffold-published-surface`
- Outcome: Make constitution lookup additive and read-only across partial hierarchies so downstream planning can rely on it.

**Acceptance Focus:**

- Missing org-level constitutions no longer hard-fail resolution.
- Full-hierarchy merge behavior remains intact.
- The command stays read-only across all hierarchy combinations.

### Epic 4: Reviewed Planning Handoff

#### Story 4.1: Rewrite Preplan

- Story Key: `4-1-rewrite-preplan`
- Priority: Must
- Story Points: 8
- Depends On: `1-2-validate-phase-artifacts-shared-utility`, `1-3-batch-two-pass-contract`, `3-1-fix-constitution-partial-hierarchy`
- Outcome: Restore brainstorm-first discovery with shared review and batch behavior.

**Acceptance Focus:**

- Brainstorm work stays first in the preplan flow.
- Batch mode and review-ready checks delegate to shared contracts.
- Preplan remains a thin conductor over wrapper and review infrastructure.

#### Story 4.2: Rewrite Businessplan

- Story Key: `4-2-rewrite-businessplan`
- Priority: Must
- Story Points: 5
- Depends On: `1-4-publish-to-governance-entry-hook`, `3-1-fix-constitution-partial-hierarchy`, `4-1-rewrite-preplan`
- Outcome: Publish reviewed predecessor artifacts before PRD and any track-sensitive UX authoring.

**Acceptance Focus:**

- Businessplan publishes reviewed preplan artifacts before authoring.
- Track-sensitive UX behavior remains correct for non-UX tracks.
- No direct governance writes appear outside the publish hook.

#### Story 4.3: Rewrite Techplan

- Story Key: `4-3-rewrite-techplan`
- Priority: Must
- Story Points: 5
- Depends On: `1-4-publish-to-governance-entry-hook`, `3-1-fix-constitution-partial-hierarchy`, `4-2-rewrite-businessplan`
- Outcome: Preserve publish-before-author entry and architecture generation against the authoritative PRD.

**Acceptance Focus:**

- Reviewed businessplan artifacts publish before architecture work starts.
- Architecture output explicitly references the PRD.
- Shared validation and review gates are reused instead of forked.

#### Story 4.4: Rewrite FinalizePlan

- Story Key: `4-4-rewrite-finalizeplan`
- Priority: Must
- Story Points: 8
- Depends On: `1-2-validate-phase-artifacts-shared-utility`, `1-3-batch-two-pass-contract`, `1-4-publish-to-governance-entry-hook`, `3-1-fix-constitution-partial-hierarchy`, `4-3-rewrite-techplan`
- Outcome: Preserve the strict three-step ordering: review, plan PR, then downstream bundle plus final PR.

**Acceptance Focus:**

- Review outputs are committed and pushed before plan PR work begins.
- The planning PR path remains `{featureId}-plan` -> `{featureId}`.
- Bundle generation happens only after the planning PR merge and before the final `{featureId}` -> `main` PR.

#### Story 4.5: Rewrite ExpressPlan

- Story Key: `4-5-rewrite-expressplan`
- Priority: Must
- Story Points: 5
- Depends On: `1-3-batch-two-pass-contract`, `3-1-fix-constitution-partial-hierarchy`, `4-4-rewrite-finalizeplan`
- Outcome: Preserve express-only gating, internal QuickPlan delegation, and the hard review stop.

**Acceptance Focus:**

- Non-express features are blocked from express planning.
- Express-eligible work delegates through retained QuickPlan internals.
- Review failure halts the compressed flow before final bundling continues.

### Epic 5: Governed Delivery, Closure, and Release Compatibility

#### Story 5.1: Rewrite Dev

- Story Key: `5-1-rewrite-dev`
- Priority: Must
- Story Points: 8
- Depends On: `4-4-rewrite-finalizeplan`
- Outcome: Preserve target-repo-only implementation, resumable dev sessions, and per-task commit discipline.

**Acceptance Focus:**

- `/dev` prepares or selects the correct target-repo branch without writing code in the control repo.
- `dev-session.yaml` remains resumable and schema-compatible.
- The final implementation slice still closes through the retained PR model.

#### Story 5.2: Rewrite Complete

- Story Key: `5-2-rewrite-complete`
- Priority: Must
- Story Points: 5
- Depends On: `5-1-rewrite-dev`
- Outcome: Preserve retrospective, documentation, and archive ordering at feature closeout.

**Acceptance Focus:**

- Retrospective work happens before archival mutation.
- Final documentation lands before terminal archive state is applied.
- Completed features remain discoverable and unambiguously archived.

#### Story 5.3: Rewrite Split-Feature

- Story Key: `5-3-rewrite-split-feature`
- Priority: Must
- Story Points: 5
- Depends On: `2-4-rewrite-new-feature`, `5-1-rewrite-dev`
- Outcome: Keep validate-first feature splitting with safe governance creation and optional eligible story movement.

**Acceptance Focus:**

- In-progress or otherwise ineligible work blocks the split before mutation.
- New governance feature creation happens before optional story movement.
- The retained script surface remains visible and intact.

#### Story 5.4: Rewrite Discover

- Story Key: `5-4-rewrite-discover`
- Priority: Must
- Story Points: 5
- Depends On: `2-4-rewrite-new-feature`
- Outcome: Preserve explicit repo-inventory synchronization and the governance-main exception behavior.

**Acceptance Focus:**

- Repo inventory and local clone state synchronize bidirectionally.
- The governance-main auto-commit exception stays scoped only to `discover`.
- Post-sync inventory is clearer and more trustworthy than pre-sync state.

#### Story 5.5: Rewrite Upgrade and Release-Readiness Gate

- Story Key: `5-5-rewrite-upgrade-and-regression-gate`
- Priority: Must
- Story Points: 8
- Depends On: `1-1-scaffold-published-surface`, `4-4-rewrite-finalizeplan`, `5-1-rewrite-dev`, `5-2-rewrite-complete`, `5-3-rewrite-split-feature`, `5-4-rewrite-discover`
- Outcome: Preserve no-op upgrade behavior for current v4 users and produce the explicit evidence needed to release the rewrite safely.

**Acceptance Focus:**

- `upgrade` remains a no-op for v4-to-v4 cases and routes explicitly for future migrations.
- Release notes capture the delivery mechanism and the hard-cut behavior for the 37 removed public stubs.
- Final parity evidence covers all retained commands, required dependencies, and at least one integration-level coherence check.
