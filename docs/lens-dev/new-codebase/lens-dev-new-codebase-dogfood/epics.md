---
feature: lens-dev-new-codebase-dogfood
doc_type: epics
status: draft
goal: "Decompose clean-room parity rebuild for lens.core.src into five reviewable epics aligned with five sprint slices."
key_decisions:
  - Organize work around Foundation Restoration, Git Orchestration, Command Surface, Dev/Complete Handoff, and Parity Proof epics.
  - Foundations must land before git orchestration; orchestration must land before command surface reconciliation.
  - Dev and Complete restoration get a dedicated epic due to high authority-boundary risk.
  - Sprint 5 is confirmation-only — no net-new implementation.
open_questions:
  - Does QuickPlan require a public prompt path or remain strictly internal?
  - Which config file is the single source of truth for persisted github_username?
depends_on:
  - business-plan.md
  - tech-plan.md
  - sprint-plan.md
  - finalizeplan-review.md
blocks: []
updated_at: '2026-05-01T14:30:00Z'
---

# Epics — Dogfood Clean-Room Parity

## Epic 1 — Foundation Restoration

**Goal:** Make the target module capable of resolving lifecycle, config, feature state, docs paths, and read-only branch context before any deeper command work proceeds.

**Scope:**
- Build and publish a machine-checkable 17-command parity map and clean-room traceability document.
- Recreate a v4-compatible `lifecycle.yaml` from baseline acceptance behavior, covering phases, tracks, ExpressPlan review contract, and artifact names.
- Add `bmadconfig.yaml` and `config.user.yaml` contract covering governance repo, 3-branch control topology, target-project branch strategy, username, and output paths.
- Implement `bmad-lens-feature-yaml`: read, validate, update phase and docs fields surgically (v4 schema, preserve unknown fields, reject invalid transitions).
- Implement `bmad-lens-git-state`: read-only branch topology, active features, and git-vs-yaml discrepancy reporting.

**Exit Criteria:**
- All 5 stories in Sprint 1 pass their acceptance criteria.
- Focused tests cover lifecycle/config/feature-yaml/git-state contracts.
- No foundation story leaves a failing test.
- Clean-room traceability map is committed to `lens.core.src`.

---

## Epic 2 — Git Orchestration and Topology Bugfixes

**Goal:** Absorb the known bugfix backlog (BF-1 through BF-6) into the sanctioned write path so phase progression and branch setup are reliable.

**Scope:**
- Implement the 3-branch control topology: `{featureId}`, `{featureId}-plan`, and `{featureId}-dev` creation, tracking, and validation.
- Route phase artifacts to the correct control branch by phase step.
- Add a sanctioned feature-index sync operation for post-transition stale-state resolution (BF-3 / Defect 5).
- Add phase-start branch verification and base-branch validation (BF-4, BF-5 / Defect 1).
- Add branch cleanup and branch-switch discipline after each PR merge.
- Add express publish artifact mapping for all QuickPlan artifacts and both review filename variants (BF-6 / Defects 3, 4).

**Exit Criteria:**
- All 6 stories in Sprint 2 pass their acceptance criteria.
- BF-1 through BF-6 defect fixtures pass.
- `publish-to-governance --phase expressplan` produces all required artifacts.
- Feature-index sync runs after phase transitions and passes the stale-entry fixture.

---

## Epic 3 — Retained Command Surface and Phase Conductors

**Goal:** Bring the target public and internal command surfaces into alignment with the 17-command baseline.

**Scope:**
- Reconcile public prompt stubs and release prompts for all 17 retained commands; enforce public prompt chain as a parity requirement.
- Reconcile `module.yaml`, help CSVs, manifests, and command discovery; eliminate the duplicate `lens-expressplan` entry.
- Restore missing command skills (dev, split-feature, upgrade) and internal dependencies.
- Normalize Lens wrapper output-path precedence so delegated skills write to feature docs paths.
- Decide and implement QuickPlan parity shape (public-compatible or explicitly internal-only with regression tests).

**Exit Criteria:**
- All 5 stories in Sprint 3 pass their acceptance criteria.
- No duplicate entries in `module.yaml`.
- All 17 retained commands have a working public prompt chain.
- Lens wrapper writes to feature-scoped paths, not global fallbacks.

---

## Epic 4 — Dev and Complete Handoff Restoration

**Goal:** Restore the implementation handoff path from FinalizePlan into target-repo execution and terminal closeout.

**Scope:**
- Implement Dev phase conductor: validates FinalizePlan artifacts, resolves registered target repos, prepares target branch, delegates stories, opens final PR.
- Preserve `dev-session.yaml` checkpoint compatibility and test resume behavior.
- Implement target-repo branch preparation; target repo writes stay outside control/governance repos.
- Complete retrospective-first closeout: writes retrospective, documents project state, commits/pushes, archives only after documentation exists.
- Fill document-project and discover retrospective gaps.

> **Governance coordination note:** Sprint 4 Dev conductor work is scoped as parity-implementation against the existing reference module. First-class Dev command authoring belongs to `lens-dev-new-codebase-dev`. Sprint 4 stories must not duplicate or pre-empt that feature's eventual PRD.

**Exit Criteria:**
- All 5 stories in Sprint 4 pass their acceptance criteria.
- Dev conductor validates a FinalizePlan artifact set without executing unsafe writes (dry-run test).
- Complete conductor runs retrospective-before-archive path.
- All story files include `## Implementation Channel` section per H2 response D.

---

## Epic 5 — Regression, Parity Proof, and Release Readiness

**Goal:** Prove output parity against the 17-command baseline and document any remaining compatibility debt.

**Scope:**
- Run focused tests for all skills touched in Sprints 1–4.
- Run command trace validation for all 17 retained commands (public stub → skill → dependencies → outputs → authority boundary).
- Run ExpressPlan-to-FinalizePlan dry run; confirm no direct governance writes.
- Run FinalizePlan-to-Dev dry run; confirm target repo resolution and session creation.
- Publish parity report with green contracts, accepted warnings, inherited failures, and follow-up backlog.
- Document express review filename compatibility mapping (ADR-5 / M3 accepted risk).

**Exit Criteria:**
- All 6 stories in Sprint 5 pass their acceptance criteria.
- Parity report committed; no undocumented inherited failures.
- Express review filename mapping documented with implementation location named.
- Feature is release-ready for `/dev`.
