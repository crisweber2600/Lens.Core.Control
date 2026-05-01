---
feature: lens-dev-new-codebase-dogfood
doc_type: stories
status: draft
goal: "Story list with acceptance criteria for clean-room parity rebuild of lens.core.src across 5 epics and 27 stories."
key_decisions:
  - Stories align to the five sprint epics; acceptance criteria are observable at the skill/command boundary.
  - Every story touching lens-work files must include an Implementation Channel section naming the required BMB path (H2 response D).
  - Defect traceability from ExpressPlanBugs.md is explicit — each defect maps to a story in the implementation-readiness artifact.
open_questions:
  - Does QuickPlan require a public prompt path or remain strictly internal?
depends_on:
  - epics.md
  - sprint-plan.md
  - tech-plan.md
blocks: []
updated_at: '2026-05-01T14:30:00Z'
---

# Stories — Dogfood Clean-Room Parity

---

## Epic 1 — Foundation Restoration

---

### E1-S1 — Build retained-command parity map

**Type:** [new] | **Points:** S | **Story ID:** S1.1

**Story:** As a Lens maintainer, I want the 17-command baseline captured as a machine-checkable inventory and clean-room traceability document so that every subsequent sprint starts from a verified foundation.

**Acceptance Criteria:**
- [ ] A parity map document lists all 17 retained commands with their public stub path, release prompt path, owning skill, and current target status (present/missing/partial).
- [ ] The clean-room traceability statement is explicit: no files were copied from `lens.core`; all behavior is reproduced from baseline acceptance criteria and observed outputs.
- [ ] The document is committed to `lens.core.src` under a known docs path.
- [ ] A validation script or check can compare the parity map against the live file tree and report drift.

---

### E1-S2 — Restore v4-compatible lifecycle contract

**Type:** [new] | **Points:** M | **Story ID:** S1.2

**Story:** As a Lens maintainer, I want the target module to have a v4-compatible `lifecycle.yaml` so that phase conductors, track validation, and artifact contracts resolve from a single source rather than hardcoded assumptions.

**Acceptance Criteria:**
- [ ] Target `lens.core.src` contains `_bmad/lens-work/lifecycle.yaml` at v4-compatible schema.
- [ ] Lifecycle covers all retained phases: `preplan`, `businessplan`, `techplan`, `expressplan`, `finalizeplan`, `dev`, `complete`.
- [ ] Lifecycle covers all retained tracks: `standard`, `express`, `quickdev`, `hotfix-express`, `spike`.
- [ ] The `express` track lists `finalizeplan` in its phases array.
- [ ] The express review artifact contract names `expressplan-adversarial-review.md` and recognizes `expressplan-review.md` as a legacy alias (ADR-5 / Defect 7 partial).
- [ ] Constitution resolver uses the lifecycle to determine track eligibility; the `express` track is permitted for `lens-dev/new-codebase` (Defect 1 / BF regression fixture).
- [ ] Focused regression test: constitution resolver returns permitted for `express` track on `lens-dev/new-codebase`.

---

### E1-S3 — Add target module config and user config contract

**Type:** [new] | **Points:** M | **Story ID:** S1.3

**Story:** As a Lens maintainer, I want `bmadconfig.yaml` and a user config contract in the target module so that governance repo, control topology, target-project branch strategy, username, and output paths resolve without hardcoded workspace assumptions.

**Acceptance Criteria:**
- [ ] Target has `_bmad/lens-work/bmadconfig.yaml` with `governance_repo_path`, `control_topology: 3-branch`, `target_projects_path`, `default_git_remote`, and `lifecycle_contract` fields.
- [ ] User config contract documents which fields are user-overridable via `config.user.yaml` (at minimum: `github_username`, `default_branch`, `target_branch_strategy`).
- [ ] Feature and config discovery work without `rg` or any specific editor search provider; fallback path is tested (Defect 2).
- [ ] File write and publish operations use OS-normalized absolute paths; no artifact is written to `C:/d/...` or similar on Windows (Defect 3).
- [ ] Focused tests cover config loading, fallback discovery, and Windows path normalization.

---

### E1-S4 — Implement feature-yaml state operations

**Type:** [new] | **Points:** L | **Story ID:** S1.4

**Story:** As a Lens maintainer, I want `bmad-lens-feature-yaml` to read, validate, and update `feature.yaml` entries so that phase conductors can progress features without direct governance file manipulation.

**Acceptance Criteria:**
- [ ] `bmad-lens-feature-yaml` skill exists in target `_bmad/lens-work/skills/`.
- [ ] Read operation: loads feature.yaml and returns identity fields, phase, track, docs paths, target_repos, dependencies, and transition history.
- [ ] Validate operation: rejects invalid phase transitions, warns on missing target_repos for implementation-impacting features.
- [ ] Update operation: surgically updates `phase`, `docs.path`, `docs.governance_docs_path`, and `target_repos`; preserves all other fields unchanged.
- [ ] Dirty-state handling: pull, stage, commit, push relevant changes, report SHA before continuing — does not block on uncommitted changes (Defect 6).
- [ ] Focused tests cover read/validate/update/dirty-state scenarios.

---

### E1-S5 — Implement read-only git-state operations

**Type:** [new] | **Points:** M | **Story ID:** S1.5

**Story:** As a Lens maintainer, I want `bmad-lens-git-state` to report the control-repo branch topology, active features, and git-vs-yaml discrepancies without performing any writes.

**Acceptance Criteria:**
- [ ] `bmad-lens-git-state` skill exists in target `_bmad/lens-work/skills/`.
- [ ] Reports: current branch, all feature branches present, which features have plan/dev branches open.
- [ ] Reports: active features from governance feature-index, phase per feature.
- [ ] Reports: discrepancies between `feature.yaml` phase and branch state.
- [ ] Strictly read-only: no git writes, no file mutations.
- [ ] Focused tests cover branch detection, discrepancy reporting, and read-only constraint.

---

## Epic 2 — Git Orchestration and Topology Bugfixes

---

### E2-S1 — Implement 3-branch control topology

**Type:** [fix] | **Points:** L | **Story ID:** S2.1

**Story:** As a Lens maintainer, I want git orchestration to create and manage `{featureId}`, `{featureId}-plan`, and `{featureId}-dev` branches so that planning, review, and implementation phases have distinct, tracked branch targets.

**Acceptance Criteria:**
- [ ] `create-feature-branches` operation creates all three branches from the correct base.
- [ ] Each branch is validated for existence before phase operations that require it.
- [ ] Missing branch triggers a structured error routing back to `init-feature`, not silent fallback.
- [ ] Target-project branch strategy (`flat`, `feature/{featureStub}`, `feature/{featureStub}-{username}`) is independent and configurable separately.
- [ ] Focused tests cover branch creation, validation, missing-branch error, and target-project strategy.

---

### E2-S2 — Route phase artifacts to the correct control branch

**Type:** [fix] | **Points:** M | **Story ID:** S2.2

**Story:** As a Lens maintainer, I want planning artifacts to be committed to the correct branch by phase step so that governance authority boundaries are maintained across the feature lifecycle.

**Acceptance Criteria:**
- [ ] Planning artifacts before FinalizePlan: routed to `{featureId}-plan`.
- [ ] FinalizePlan step 1 and step 2 artifacts: routed to `{featureId}-plan`, then merged to `{featureId}`.
- [ ] FinalizePlan step 3 bundle artifacts: routed to `{featureId}` (or `{featureId}-dev` if present).
- [ ] Dev implementation artifacts: written only to the target repo (never control or governance).
- [ ] Phase-routing decisions are logged so deviations are detectable.
- [ ] Focused tests cover correct branch routing for each phase step.

---

### E2-S3 — Resolve BF-3 feature-index synchronization

**Type:** [fix] | **Points:** M | **Story ID:** S2.3

**Story:** As a Lens maintainer, I want a sanctioned operation to sync `feature-index.yaml` after feature.yaml phase transitions so that `next`, `switch`, dashboards, and cross-feature context always read current state.

**Acceptance Criteria:**
- [ ] `sync-feature-index` operation reads `feature.yaml` for the target feature and updates the matching entry in `feature-index.yaml` in the governance repo.
- [ ] Operation is called automatically after every phase transition in `bmad-lens-feature-yaml update`.
- [ ] Governance writes route through the publish CLI or sanctioned operation — no direct file edits by agents.
- [ ] Fixture test: start with stale `feature-index.yaml` entry, run phase transition, confirm entry is updated.
- [ ] Defect 5 / BF-3 regression test passes.

---

### E2-S4 — Resolve BF-4 and BF-5 phase-start and base-branch validation

**Type:** [fix] | **Points:** M | **Story ID:** S2.4

**Story:** As a Lens maintainer, I want phase-start operations to verify that required branches exist and that the intended base branch is correct so that phases never begin from the wrong state.

**Acceptance Criteria:**
- [ ] Phase-start check verifies `{featureId}` and `{featureId}-plan` exist before any phase writing begins.
- [ ] Base-branch validation confirms the current branch is the intended base; fails explicitly on mismatch.
- [ ] Constitution resolver accepts `express` track for `lens-dev/new-codebase`; false-negative from Defect 1 does not recur.
- [ ] Focused tests cover missing branch, wrong base, and constitution resolver express-track pass scenarios.

---

### E2-S5 — Add branch cleanup and branch-switch discipline

**Type:** [fix] | **Points:** M | **Story ID:** S2.5

**Story:** As a Lens maintainer, I want git orchestration to clean up merged branches and switch to the correct next branch after each PR merge so that the workspace never accumulates stale branches.

**Acceptance Criteria:**
- [ ] After a PR merges, local and remote branches for that feature step are deleted.
- [ ] Workflow switches to the correct next branch and pulls before continuing.
- [ ] Branch-switch includes a pull; working directory is clean before any write.
- [ ] Cleanup is idempotent: running twice does not fail.
- [ ] Focused tests cover cleanup, switch, and idempotent re-run.

---

### E2-S6 — Add express publish artifact mapping

**Type:** [fix] | **Points:** M | **Story ID:** S2.6

**Story:** As a Lens maintainer, I want `publish-to-governance --phase expressplan` to copy all required QuickPlan artifacts and recognize both current and legacy express review filenames so that governance mirrors are complete by default.

**Acceptance Criteria:**
- [ ] `publish-to-governance --phase expressplan` copies: `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and the express review artifact.
- [ ] Both `expressplan-adversarial-review.md` (current) and `expressplan-review.md` (legacy) are recognized; the matched filename is reported in output.
- [ ] Express publish does not require explicit operator override to include all artifacts (Defect 4).
- [ ] Windows path normalization is applied before file writes and publish operations (Defect 3).
- [ ] Focused tests cover full artifact set, legacy filename fallback, and Windows path normalization.
- [ ] **Additional AC (M3 / ADR-5):** Output reports which express review filename was matched.

---

## Epic 3 — Retained Command Surface and Phase Conductors

---

### E3-S1 — Reconcile public prompt stubs and release prompts

**Type:** [new] | **Points:** M | **Story ID:** S3.1

**Story:** As a Lens maintainer, I want public stubs, release prompts, and prompt-start behavior to exist for all 17 retained commands so that no command is marked parity-complete while its public prompt chain is missing (Defect 7).

**Acceptance Criteria:**
- [ ] All 17 retained commands have a `.github/prompts/lens-{command}.prompt.md` that runs `light-preflight.py` and delegates to the release prompt.
- [ ] All 17 retained commands have a `_bmad/lens-work/prompts/lens-{command}.prompt.md` that delegates to the owning SKILL.md.
- [ ] Any command without a complete public prompt chain is explicitly listed as not-parity-complete.
- [ ] Inventory validation script flags missing or broken prompt chain entries.
- [ ] Defect 7 regression test: no retained command passes parity without a working public prompt chain.

**Implementation Channel:** `.github/skills/bmad-workflow-builder` for release prompts; `.github/skills/bmad-module-builder` for SKILL.md owners.

---

### E3-S2 — Reconcile module.yaml, help CSVs, manifests, and discovery

**Type:** [fix] | **Points:** M | **Story ID:** S3.2

**Story:** As a Lens maintainer, I want `module.yaml`, help CSVs, and manifests to agree on the retained 17-command set so that duplicate entries and drift do not cause loading failures or incorrect discovery.

**Acceptance Criteria:**
- [ ] Duplicate `lens-expressplan` entry in `module.yaml` is removed.
- [ ] `module.yaml`, `module-help.csv`, manifests, and adapter surfaces all list exactly the 17 retained commands.
- [ ] An inventory validation check prevents re-introduction of duplicates.
- [ ] Setup (`setup.py` or equivalent) agrees with the retained command inventory.
- [ ] Focused tests cover inventory consistency across all metadata surfaces.

**Implementation Channel:** `.github/skills/bmad-module-builder` for `module.yaml` and skill inventory changes.

---

### E3-S3 — Restore missing command skills and internal dependencies

**Type:** [new] | **Points:** L | **Story ID:** S3.3

**Story:** As a Lens maintainer, I want missing retained command skills (`dev`, `split-feature`, `upgrade`, `constitution`) and their internal dependencies to exist in the target module so that all retained commands have owning skill files.

**Acceptance Criteria:**
- [ ] `bmad-lens-dev` skill exists in target `_bmad/lens-work/skills/` (stub acceptable; behavior gated by Epic 4).
- [ ] `bmad-lens-split-feature`, `bmad-lens-upgrade`, and `bmad-lens-constitution` skills exist or are documented with their owning skill paths.
- [ ] Internal dependencies required by retained conductors are present or documented as gaps.
- [ ] Parity map from E1-S1 is updated with final ownership status for each command.

**Implementation Channel:** `.github/skills/bmad-module-builder` for SKILL.md creation.

---

### E3-S4 — Normalize Lens wrapper output-path precedence

**Type:** [fix] | **Points:** L | **Story ID:** S3.4

**Story:** As a Lens maintainer, I want `bmad-lens-bmad-skill` to resolve feature docs paths from `feature.yaml` as the authoritative write scope so that delegated BMAD skills never write to the global planning fallback.

**Acceptance Criteria:**
- [ ] When feature context exists, `bmad-lens-bmad-skill` sets `planning_artifacts = feature.yaml.docs.path`.
- [ ] Global `docs/planning-artifacts` fallback is only used when feature context is genuinely absent.
- [ ] `bmad-lens-bmad-skill` blocks writes to `{release_repo_root}/`, `.github/`, and governance repo paths.
- [ ] Focused tests cover path-precedence enforcement and write-scope blocking.

**Implementation Channel:** `.github/skills/bmad-module-builder` for SKILL.md changes.

---

### E3-S5 — Decide and implement QuickPlan parity shape

**Type:** [new] | **Points:** M | **Story ID:** S3.5

**Story:** As a Lens maintainer, I want QuickPlan's public compatibility or internal-only status to be explicitly decided and tested so that ExpressPlan behavior is provably intact regardless of the decision.

**Acceptance Criteria:**
- [ ] Decision is recorded: QuickPlan is either (a) public-compatible with a `.github/prompts/lens-quickplan.prompt.md` stub or (b) explicitly internal-only with a documented note in `module.yaml`.
- [ ] If internal-only: a test confirms ExpressPlan invokes QuickPlan via `bmad-lens-bmad-skill` and the behavior is intact.
- [ ] If public-compatible: a public stub exists and follows the prompt-start preflight pattern.
- [ ] Open question from business-plan and tech-plan is closed and recorded in this story's completion notes.

**Implementation Channel:** `.github/skills/bmad-module-builder` if SKILL.md is modified; `.github/skills/bmad-workflow-builder` if a public prompt stub is created.

---

## Epic 4 — Dev and Complete Handoff Restoration

> **Governance coordination note:** All Sprint 4 stories are parity-implementation work against the existing reference module (`lens.core`). First-class Dev command authoring belongs to `lens-dev-new-codebase-dev`. Any overlap with that feature's eventual PRD must be surfaced as an explicit acceptance criteria alignment before implementation proceeds.

---

### E4-S1 — Implement Dev phase conductor

**Type:** [new] | **Points:** XL | **Story ID:** S4.1

**Story:** As a Lens maintainer, I want a Dev phase conductor in the target module that validates FinalizePlan artifacts, resolves target repos, prepares the selected target branch, delegates story work, and opens the implementation PR.

**Acceptance Criteria:**
- [ ] `bmad-lens-dev` SKILL.md exists with: artifact validation, target repo resolution, branch preparation, story delegation, and final PR creation.
- [ ] Dev validates that FinalizePlan bundle artifacts (epics, stories, implementation-readiness, sprint-status) exist before starting.
- [ ] Dev resolves `feature.yaml.target_repos` and fails explicitly if empty.
- [ ] Dev writes only to the target repo; control and governance repo writes are blocked.
- [ ] Dev dry-run test: validates artifacts and resolves target repo without executing unsafe writes.

**Implementation Channel:** `.github/skills/bmad-module-builder` for SKILL.md creation.

**Governance coordination note:** This story reproduces reference behavior for clean-room parity testing. First-class Dev command authoring belongs to `lens-dev-new-codebase-dev`; any overlap with that feature's eventual PRD must be surfaced as explicit acceptance criteria alignment before implementation proceeds.

---

### E4-S2 — Preserve dev-session.yaml compatibility

**Type:** [new] | **Points:** M | **Story ID:** S4.2

**Story:** As a Lens maintainer, I want `dev-session.yaml` checkpoint fields to remain loadable and resume behavior to be tested so that interrupted Dev sessions can resume without data loss.

**Acceptance Criteria:**
- [ ] `dev-session.yaml` schema is documented in the target module.
- [ ] Existing checkpoint fields from the reference module are supported.
- [ ] Resume behavior is tested: loading a saved session produces the expected state.
- [ ] New fields are additive; existing fields are never removed or renamed in a breaking way.

**Implementation Channel:** `.github/skills/bmad-module-builder` if session handling is inside a SKILL.md.

**Governance coordination note:** This story reproduces reference behavior for clean-room parity testing. First-class Dev command authoring belongs to `lens-dev-new-codebase-dev`; any overlap with that feature's eventual PRD must be surfaced as explicit acceptance criteria alignment before implementation proceeds.

---

### E4-S3 — Implement target-repo branch preparation

**Type:** [new] | **Points:** M | **Story ID:** S4.3

**Story:** As a Lens maintainer, I want the Dev conductor to prepare a working branch in the target repo using the configured target-project branch strategy so that implementation writes are isolated and never bleed into the control or governance repos.

**Acceptance Criteria:**
- [ ] Dev resolves `target_branch_strategy` from feature or repo config: `flat`, `feature/{featureStub}`, or `feature/{featureStub}-{username}`.
- [ ] Working branch is created in the target repo before any implementation writes.
- [ ] Target repo write scope is enforced: no writes to `lens.core.src/_bmad/lens-work/` (that path is the module source; implementation goes to the feature scope).
- [ ] Focused tests cover each branch strategy and write-scope enforcement.

**Implementation Channel:** `.github/skills/bmad-module-builder` for SKILL.md; target-repo branch prep is implementation, not a lens-work artifact change.

**Governance coordination note:** This story reproduces reference behavior for clean-room parity testing. First-class Dev command authoring belongs to `lens-dev-new-codebase-dev`; any overlap with that feature's eventual PRD must be surfaced as explicit acceptance criteria alignment before implementation proceeds.

---

### E4-S4 — Complete retrospective-first closeout automation

**Type:** [fix] | **Points:** L | **Story ID:** S4.4

**Story:** As a Lens maintainer, I want the Complete conductor to write the retrospective, document project state, commit/push, and archive only after documentation exists so that no feature is archived silently.

**Acceptance Criteria:**
- [ ] Complete checks that a retrospective exists before archiving.
- [ ] If retrospective is missing: writes it before continuing.
- [ ] Document-project step runs after retrospective and before archive.
- [ ] Commit, push, and merge happen in that order; archive is the final step.
- [ ] Automation is tested with a fixture that exercises retrospective-before-archive ordering.

**Implementation Channel:** `.github/skills/bmad-module-builder` for SKILL.md changes.

**Governance coordination note:** This story reproduces reference behavior for clean-room parity testing. First-class Dev command authoring belongs to `lens-dev-new-codebase-dev`; any overlap with that feature's eventual PRD must be surfaced as explicit acceptance criteria alignment before implementation proceeds.

---

### E4-S5 — Fill document-project and discover retrospective gaps

**Type:** [fix] | **Points:** M | **Story ID:** S4.5

**Story:** As a Lens maintainer, I want the document-project gap and the discover repo-inventory overwrite risk resolved so that Complete automation does not leave undefined behavior for these two known gaps.

**Acceptance Criteria:**
- [ ] `bmad-lens-document-project` gap is either resolved (skill exists and is tested) or documented as an explicit substitution with an alternative path.
- [ ] Discover repo-inventory overwrite risk gets a create-only guard: if `repo-inventory.yaml` exists, the operation appends or updates rather than overwriting.
- [ ] Retrospective lessons from the discover feature are captured in a completion note on this story.

**Implementation Channel:** `.github/skills/bmad-module-builder` for any SKILL.md changes.

**Governance coordination note:** This story reproduces reference behavior for clean-room parity testing. First-class Dev command authoring belongs to `lens-dev-new-codebase-dev`; any overlap with that feature's eventual PRD must be surfaced as explicit acceptance criteria alignment before implementation proceeds.

---

## Epic 5 — Regression, Parity Proof, and Release Readiness

---

### E5-S1 — Run focused tests for touched skills

**Type:** [confirm] | **Points:** M | **Story ID:** S5.1

**Story:** As a Lens maintainer, I want focused pytest tests to pass for all skills touched in Sprints 1–4 so that the rebuild has a tested safety net before broader validation.

**Acceptance Criteria:**
- [ ] `uv run python -m pytest` runs successfully on Windows for all focused test files.
- [ ] All tests written in Sprints 1–4 pass.
- [ ] Any inherited failures (unrelated to dogfood) are explicitly documented with test IDs.
- [ ] No test regresses from a previously passing state.

---

### E5-S2 — Run command trace validation for all 17 retained commands

**Type:** [confirm] | **Points:** M | **Story ID:** S5.2

**Story:** As a Lens maintainer, I want each retained command traced from public stub to skill, including dependencies, outputs, and authority boundary, so that behavioral parity is documented evidence rather than assumed.

**Acceptance Criteria:**
- [ ] Trace document exists for all 17 commands: `preflight`, `new-domain`, `new-service`, `new-feature`, `switch`, `next`, `preplan`, `businessplan`, `techplan`, `finalizeplan`, `expressplan`, `dev`, `complete`, `split-feature`, `constitution`, `discover`, `upgrade`.
- [ ] Each trace covers: public prompt path → release prompt → owning skill → internal dependencies → output contract → authority boundary.
- [ ] Any command with a missing or broken chain is listed as a known gap with an action item.

---

### E5-S3 — Run ExpressPlan-to-FinalizePlan dry run

**Type:** [confirm] | **Points:** M | **Story ID:** S5.3

**Story:** As a Lens maintainer, I want an ExpressPlan-to-FinalizePlan dry run to confirm that express artifacts validate review-ready and no direct governance writes occur during the sequence.

**Acceptance Criteria:**
- [ ] Dry run executes from ExpressPlan activation through FinalizePlan review artifact validation.
- [ ] No direct governance file writes occur during the sequence.
- [ ] Express review artifact (both `expressplan-adversarial-review.md` and `expressplan-review.md`) is recognized at the appropriate gate.
- [ ] Result is documented in this story's completion notes.

---

### E5-S4 — Run FinalizePlan-to-Dev dry run

**Type:** [confirm] | **Points:** M | **Story ID:** S5.4

**Story:** As a Lens maintainer, I want a FinalizePlan-to-Dev dry run to confirm that the Dev handoff validates target repo resolution and session creation without executing unsafe writes.

**Acceptance Criteria:**
- [ ] Dry run executes Dev activation with a fixture FinalizePlan bundle.
- [ ] Dev resolves `feature.yaml.target_repos` and reports the result.
- [ ] No writes to governance or control repos occur during the dry run.
- [ ] Dev-session.yaml creation step is exercised but the file is written to a temp/test path.
- [ ] Result is documented in this story's completion notes.

---

### E5-S5 — Publish parity report and known compatibility debt

**Type:** [confirm] | **Points:** S | **Story ID:** S5.5

**Story:** As a Lens maintainer, I want a published parity report that lists green contracts, accepted warnings, inherited failures, and a follow-up backlog so that the dogfood feature closes with a complete evidence record.

**Acceptance Criteria:**
- [ ] Parity report is committed to the target repo docs path.
- [ ] Report includes: green contracts (all passing), accepted risks (ADR-5, M3 compatibility debt), inherited failures (with test IDs), follow-up backlog.
- [ ] Clean-room compliance checkpoint: external hash comparison result for all touched `lens.core.src` files against `lens.core` counterparts is included as evidence (Defect 8).
- [ ] Report is legible without reading individual sprint test output.

---

### E5-S6 — Document express review filename compatibility mapping

**Type:** [confirm] | **Points:** S | **Story ID:** S5.6

**Story:** As a Lens maintainer, I want the express review filename compatibility mapping explicitly documented so that the ADR-5 tech debt is a named, traceable record rather than a silent assumption.

**Acceptance Criteria:**
- [ ] Document specifies: (a) `expressplan-adversarial-review.md` is the current canonical filename per the active lifecycle contract; (b) `expressplan-review.md` is the legacy filename recognized for backward compatibility; (c) the implementation location in `publish-to-governance` where both names are resolved; (d) any governance artifacts that still use the legacy name.
- [ ] Sprint-status accepted-risks entry for ADR-5 exists.
- [ ] Document is linked from the parity report (E5-S5).
