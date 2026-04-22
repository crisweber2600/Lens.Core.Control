---
feature: lens-dev-new-codebase-baseline
doc_type: stories
status: draft
goal: "Stage the full implementation backlog for the lens-work rewrite with ordered, estimated, dev-ready stories."
key_decisions:
  - "Use epics.md as the authoritative story inventory and keep the 21-story decomposition unchanged."
  - "Preserve the Epic 3 to Epic 4 prerequisite in both dependencies and sprint sequencing."
  - "Treat the old-codebase discovery docs as verification-only references for outcome checks; they are not parity-reference inputs or implementation baselines."
open_questions:
  - "Should the proposed seven-sprint sequence be compressed once team capacity is confirmed?"
  - "Should /dev formally record the target repo path before the first implementation story begins?"
depends_on:
  - epics.md
  - prd.md
  - architecture.md
  - finalizeplan-review.md
blocks:
  - implementation-readiness.md
  - sprint-status.yaml
  - stories/
updated_at: 2026-04-22T00:00:00Z
updated_at: 2026-04-22T18:02:56Z
---

# Stories: lens-dev-new-codebase-baseline

## Overview

This backlog stages the approved 21-story implementation sequence for the lens-work rewrite. It follows the existing epic breakdown in epics.md, preserves the hard prerequisite that Story 3.1 must complete before any Epic 4 work starts, and uses old-codebase discovery artifacts as verification-only references for outcome checks and dependency coverage after contract design.

## Story List

### EPIC-1: Codebase Foundation and Shared Infrastructure

#### Story 1.1 (EPIC-1-S1): New Codebase Scaffold, Install Surfaces, and Module Surface Reduction

- Epic: EPIC-1
- Priority: Must
- Story Points: 8
- Dependencies: None
- Description: As a lens-work maintainer, I want the source tree scaffolded with exactly 17 published prompt stubs, aligned help and shell surfaces, and updated installer and adapter metadata so every rewrite starts from a consistent foundation.

**Acceptance Criteria:**
- [ ] The published prompt surface is reduced to exactly 17 retained commands.
- [ ] setup.py, module-help.csv, lens.agent.md, and supported adapters agree on the same command inventory.
- [ ] The retained internal skill inventory matches the approved keep/remove matrix.

**Technical Notes:**
- Use the old-codebase discovery inventory and dependency mapping as verification references for published surface coverage and retained internal dependency checks — not as implementation baselines.

---

#### Story 1.2 (EPIC-1-S2): Implement validate-phase-artifacts.py Shared Utility

- Epic: EPIC-1
- Priority: Must
- Story Points: 5
- Dependencies: 1.1
- Description: As a maintainer, I want a single review-ready validation script shared by all planning conductors so lifecycle gate logic is not duplicated.

**Acceptance Criteria:**
- [ ] The script reports success for reviewed, complete phase artifacts.
- [ ] Missing or unreviewed artifacts fail with specific diagnostics.
- [ ] preplan, businessplan, techplan, and finalizeplan delegate to the shared script.

**Technical Notes:**
- Regression coverage must include happy path, missing artifact, and unreviewed artifact cases.

---

#### Story 1.3 (EPIC-1-S3): Implement bmad-lens-batch Shared 2-Pass Contract

- Epic: EPIC-1
- Priority: Must
- Story Points: 5
- Dependencies: 1.1
- Description: As a maintainer, I want a single two-pass batch contract so every planning conductor handles batch intake and resume consistently.

**Acceptance Criteria:**
- [ ] Planning conductors delegate batch intake and resume behavior to bmad-lens-batch.
- [ ] Pass 1 collects the required artifacts in order without mutation.
- [ ] Pass 2 invokes downstream skills sequentially and stops cleanly on first failure.

**Technical Notes:**
- Preserve wrapper-equivalence behavior from the old orchestration chain.

---

#### Story 1.4 (EPIC-1-S4): Implement publish-to-governance Entry Hook

- Epic: EPIC-1
- Priority: Must
- Story Points: 5
- Dependencies: 1.1
- Description: As a maintainer, I want a single publish-before-author entry hook so planning conductors can mirror reviewed docs to governance without direct governance writes.

**Acceptance Criteria:**
- [ ] businessplan, techplan, and finalizeplan invoke publish-to-governance before authoring work begins.
- [ ] No-op entry is graceful when nothing is pending publication.
- [ ] Governance write audits show the entry hook is the only planning-phase write path.

**Technical Notes:**
- discover remains the only explicit auto-commit exception.

### EPIC-2: Identity and Navigation Command Surface

#### Story 2.1 (EPIC-2-S1): Rewrite preflight Command (WP-01)

- Epic: EPIC-2
- Priority: Must
- Story Points: 5
- Dependencies: 1.1
- Description: As a Lens user, I want preflight to preserve prompt-start sync and workspace validation exactly as it works today.

**Acceptance Criteria:**
- [ ] light-preflight.py runs before any workspace validation.
- [ ] Correctly configured workspaces pass without lifecycle mutation.
- [ ] Missing governance or install dependencies fail clearly and non-destructively.

**Technical Notes:**
- Keep parity with test-setup-control-repo.py and remove onboard as a published stub.

---

#### Story 2.2 (EPIC-2-S2): Rewrite new-domain Command (WP-02)

- Epic: EPIC-2
- Priority: Must
- Story Points: 3
- Dependencies: 1.1
- Description: As a Lens user, I want new-domain to create governance markers and a constitution scaffold with the existing naming rules.

**Acceptance Criteria:**
- [ ] Governance domain markers are written under the approved naming convention.
- [ ] The domain constitution scaffold lands in the expected location.
- [ ] Duplicate-domain attempts fail without overwriting artifacts.

**Technical Notes:**
- Preserve init-feature domain regression behavior.

---

#### Story 2.3 (EPIC-2-S3): Rewrite new-service Command (WP-03)

- Epic: EPIC-2
- Priority: Must
- Story Points: 3
- Dependencies: 2.2
- Description: As a Lens user, I want new-service to create service governance artifacts under an existing domain while preserving inherited rules.

**Acceptance Criteria:**
- [ ] Service markers use the correct location and naming rules.
- [ ] Domain-to-service inheritance remains intact.
- [ ] Missing parent domains fail fast with a clear message.

**Technical Notes:**
- Preserve init-feature service regression coverage.

---

#### Story 2.4 (EPIC-2-S4): Rewrite new-feature Command (WP-04)

- Epic: EPIC-2
- Priority: Must
- Story Points: 8
- Dependencies: 2.2, 2.3
- Description: As a Lens user, I want new-feature to keep the canonical featureId formula, feature-index registration, and 2-branch topology intact.

**Acceptance Criteria:**
- [ ] The canonical featureId `{domain}-{service}-{featureSlug}` is written to feature.yaml.
- [ ] feature-index.yaml records both featureId and featureSlug.
- [ ] The control repo creates exactly `{featureId}` and `{featureId}-plan`.

**Technical Notes:**
- Preserve parity with test-init-feature-ops.py and test-git-orchestration-ops.py.

---

#### Story 2.5 (EPIC-2-S5): Rewrite switch Command (WP-05)

- Epic: EPIC-2
- Priority: Must
- Story Points: 3
- Dependencies: 2.4
- Description: As a Lens user, I want switch to update active feature context without mutating lifecycle state.

**Acceptance Criteria:**
- [ ] Session context changes to the selected feature.
- [ ] No governance or lifecycle artifact is written during a switch.
- [ ] Invalid feature selection fails cleanly.

**Technical Notes:**
- Keep the switch no-write regression in place.

---

#### Story 2.6 (EPIC-2-S6): Rewrite next Command (WP-06)

- Epic: EPIC-2
- Priority: Must
- Story Points: 5
- Dependencies: 2.1, 2.4, 2.5
- Description: As a Lens user, I want next to identify the one unblocked action and auto-delegate without redundant confirmation.

**Acceptance Criteria:**
- [ ] Exactly one unblocked next action is selected.
- [ ] Delegation is pre-confirmed and does not re-ask to proceed.
- [ ] Blockers stop delegation and are surfaced clearly.

**Technical Notes:**
- Preserve blocker-first routing and handoff regression behavior.

### EPIC-3: Constitution Bug Fix and Governance Rules Engine

#### Story 3.1 (EPIC-3-S1): Fix Org-Level Constitution Hard-Fail Bug and Add Parity Tests (WP-15)

- Epic: EPIC-3
- Priority: Must
- Story Points: 5
- Dependencies: 1.1
- Description: As a Lens user working without an org-level constitution, I want additive resolution from available hierarchy levels without a hard failure.

**Acceptance Criteria:**
- [ ] Partial-hierarchy environments resolve constitution guidance without crashing.
- [ ] Full hierarchy resolution remains additive and ordered.
- [ ] The command remains read-only across all hierarchy combinations.

**Technical Notes:**
- This story is a hard prerequisite for every Epic 4 story.

### EPIC-4: Planning Conductor Rewrite

#### Story 4.1 (EPIC-4-S1): Rewrite preplan Command (WP-07)

- Epic: EPIC-4
- Priority: Must
- Story Points: 8
- Dependencies: 1.2, 1.3, 3.1
- Description: As a Lens user, I want preplan to preserve brainstorm-first behavior, shared batch semantics, and the review-ready fast path.

**Acceptance Criteria:**
- [ ] The Epic 3 prerequisite is enforced before work begins.
- [ ] Non-batch runs author brainstorm before research or product brief.
- [ ] Batch and review gates delegate to shared utilities instead of inline logic.

**Technical Notes:**
- Preserve wrapper-equivalence and phase-gate regression anchors.

---

#### Story 4.2 (EPIC-4-S2): Rewrite businessplan Command (WP-08)

- Epic: EPIC-4
- Priority: Must
- Story Points: 5
- Dependencies: 1.4, 3.1, 4.1
- Description: As a Lens user, I want businessplan to publish reviewed predecessors before PRD and UX authoring, with no direct governance writes.

**Acceptance Criteria:**
- [ ] publish-to-governance runs before BMAD authoring delegation.
- [ ] Governance write audits show no direct writes from businessplan.
- [ ] Wrapper-equivalence and governance-audit regressions stay green.

**Technical Notes:**
- Preserve the published predecessor snapshot as authoring context.

---

#### Story 4.3 (EPIC-4-S3): Rewrite techplan Command (WP-09)

- Epic: EPIC-4
- Priority: Must
- Story Points: 5
- Dependencies: 1.4, 3.1, 4.2
- Description: As a Lens user, I want techplan to generate architecture through the shared publish-before-author hook while enforcing the PRD reference rule.

**Acceptance Criteria:**
- [ ] The publish entry hook runs before architecture authoring.
- [ ] The architecture artifact explicitly references the authoritative PRD.
- [ ] Architecture-reference and wrapper-equivalence regressions remain green.

**Technical Notes:**
- Preserve the architecture-to-PRD linkage defined in lifecycle validation.

---

#### Story 4.4 (EPIC-4-S4): Rewrite finalizeplan Command (WP-10)

- Epic: EPIC-4
- Priority: Must
- Story Points: 8
- Dependencies: 1.2, 1.3, 1.4, 3.1, 4.3
- Description: As a Lens user, I want finalizeplan to preserve strict three-step ordering: review, plan PR, then downstream bundle plus final PR.

**Acceptance Criteria:**
- [ ] Step 1 review completes before plan PR work begins.
- [ ] Step 2 creates the correct `{featureId}-plan` to `{featureId}` PR.
- [ ] Step 3 generates epics, stories, readiness, sprint status, and story files before the final PR is opened.

**Technical Notes:**
- Preserve the finalizeplan step-order regression and plan/final PR topology.

---

#### Story 4.5 (EPIC-4-S5): Rewrite expressplan Command (WP-11)

- Epic: EPIC-4
- Priority: Must
- Story Points: 5
- Dependencies: 1.3, 3.1, 4.4
- Description: As a Lens user, I want expressplan to run only for express-eligible features, delegate to QuickPlan, and stop on an adversarial-review failure.

**Acceptance Criteria:**
- [ ] Non-express features are blocked from entering expressplan.
- [ ] Express-eligible features delegate to bmad-lens-quickplan.
- [ ] The hard-stop adversarial review completes before finalize bundling.

**Technical Notes:**
- Preserve express-only gating and quickplan retention behavior.

### EPIC-5: Execution, Closure, and Maintenance Commands

#### Story 5.1 (EPIC-5-S1): Rewrite dev Command (WP-12)

- Epic: EPIC-5
- Priority: Must
- Story Points: 8
- Dependencies: 4.4
- Description: As a Lens user, I want dev to keep target-repo-only writes, per-task commits, resumable checkpoints, and a final PR.

**Acceptance Criteria:**
- [ ] All code writes stay in the target repo, never the control or release repo.
- [ ] Each completed task produces a per-task commit.
- [ ] dev-session.yaml resumes interrupted work without schema changes and ends with a final PR.

**Technical Notes:**
- Preserve dev-session compatibility and repo-scoped branch mode behavior.

---

#### Story 5.2 (EPIC-5-S2): Rewrite complete Command (WP-13)

- Epic: EPIC-5
- Priority: Must
- Story Points: 5
- Dependencies: 5.1
- Description: As a Lens user, I want complete to preserve retrospective, documentation, and archive ordering with terminal archive semantics.

**Acceptance Criteria:**
- [ ] Retrospective runs before documentation.
- [ ] Documentation completes before archive.
- [ ] Archive is atomic and prevents future lifecycle mutation.

**Technical Notes:**
- Preserve complete/archive atomicity regression behavior.

---

#### Story 5.3 (EPIC-5-S3): Rewrite split-feature Command (WP-14)

- Epic: EPIC-5
- Priority: Must
- Story Points: 8
- Dependencies: 4.4
- Description: As a Lens user, I want split-feature to validate first, block in-progress stories, create the new feature before modifying the source, and move eligible stories safely.

**Acceptance Criteria:**
- [ ] validate-split runs before any governance mutation.
- [ ] In-progress stories hard-stop split execution.
- [ ] create-split-feature creates the child feature before source modification, and move-stories preserves file integrity.

**Technical Notes:**
- Preserve test-split-feature-ops.py and dry-run regression semantics.

---

#### Story 5.4 (EPIC-5-S4): Rewrite discover Command (WP-16)

- Epic: EPIC-5
- Priority: Must
- Story Points: 5
- Dependencies: 1.4
- Description: As a maintainer, I want discover to preserve bidirectional inventory sync and the governance-main auto-commit exception.

**Acceptance Criteria:**
- [ ] Governance-to-local and local-to-governance inventory sync both execute.
- [ ] Governance-main auto-commit occurs only when inventory changes exist.
- [ ] No-op runs report cleanly without creating a commit.

**Technical Notes:**
- Preserve discover bidirectional sync and auto-commit regression behavior.

---

#### Story 5.5 (EPIC-5-S5): Rewrite upgrade Command and Run E2E Regression Gate (WP-17)

- Epic: EPIC-5
- Priority: Must
- Story Points: 8
- Dependencies: 2.1, 2.4, 2.6, 3.1, 4.4, 5.1, 5.3, 5.4
- Description: As a maintainer, I want upgrade to stay a v4 no-op while the final regression gate proves the rewritten 17-command surface is release-ready.

**Acceptance Criteria:**
- [ ] upgrade remains a no-op for v4 features and retains explicit migration paths for older versions.
- [ ] The focused regression anchor set passes: init-feature, next, setup-control-repo, split-feature, upgrade, and git-orchestration.
- [ ] Release readiness explicitly confirms 17 reachable prompts, end-to-end traceability, and zero broken active features.

**Technical Notes:**
- This is the final release-promotion gate and should remain the last story in the program order.

## Story Summary

| Epic | Story | Points | Priority | Dependencies |
|------|-------|--------|----------|--------------|
| EPIC-1 | 1.1 | 8 | Must | None |
| EPIC-1 | 1.2 | 5 | Must | 1.1 |
| EPIC-1 | 1.3 | 5 | Must | 1.1 |
| EPIC-1 | 1.4 | 5 | Must | 1.1 |
| EPIC-2 | 2.1 | 5 | Must | 1.1 |
| EPIC-2 | 2.2 | 3 | Must | 1.1 |
| EPIC-2 | 2.3 | 3 | Must | 2.2 |
| EPIC-2 | 2.4 | 8 | Must | 2.2, 2.3 |
| EPIC-2 | 2.5 | 3 | Must | 2.4 |
| EPIC-2 | 2.6 | 5 | Must | 2.1, 2.4, 2.5 |
| EPIC-3 | 3.1 | 5 | Must | 1.1 |
| EPIC-4 | 4.1 | 8 | Must | 1.2, 1.3, 3.1 |
| EPIC-4 | 4.2 | 5 | Must | 1.4, 3.1, 4.1 |
| EPIC-4 | 4.3 | 5 | Must | 1.4, 3.1, 4.2 |
| EPIC-4 | 4.4 | 8 | Must | 1.2, 1.3, 1.4, 3.1, 4.3 |
| EPIC-4 | 4.5 | 5 | Must | 1.3, 3.1, 4.4 |
| EPIC-5 | 5.1 | 8 | Must | 4.4 |
| EPIC-5 | 5.2 | 5 | Must | 5.1 |
| EPIC-5 | 5.3 | 8 | Must | 4.4 |
| EPIC-5 | 5.4 | 5 | Must | 1.4 |
| EPIC-5 | 5.5 | 8 | Must | 2.1, 2.4, 2.6, 3.1, 4.4, 5.1, 5.3, 5.4 |

## Velocity Estimate

| Metric | Value |
|--------|-------|
| Total stories | 21 |
| Total story points | 120 |
| Estimated sprints | 7 |
| Sprint sequencing rule | Epic 3 must complete before Epic 4 begins |
