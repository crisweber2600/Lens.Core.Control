---
feature: lens-dev-new-codebase-dogfood
doc_type: tech-plan
status: draft
goal: "Define clean-room implementation architecture for closing lens.core.src parity gaps."
key_decisions:
  - Rebuild behavior from baseline contracts and observed outcomes; do not copy implementation files from lens.core.
  - Restore P0 platform foundations before declaring any retained command complete.
  - Implement the control repo as a three-branch topology while keeping governance flat and target-project branching strategy separate.
  - Treat feature-index synchronization and publish-to-governance mapping as first-class CLI contracts.
  - Use focused parity tests and workflow traces as the acceptance mechanism.
open_questions:
  - Should QuickPlan expose a public prompt-compatible conductor or remain internal-only for ExpressPlan?
  - Which config file is authoritative for persisted username and default topology?
  - Which review filename should automation canonicalize for express track while preserving compatibility?
depends_on:
  - business-plan.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/research.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/bugfixes.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/ExpressPlanBugs.md
blocks:
  - Implementation cannot reach parity until lifecycle/config/state foundations and dev handoff are restored.
updated_at: '2026-05-01T00:00:00Z'
---

# Tech Plan - Dogfood Clean-Room Parity

## Technical Summary

The target module at `TargetProjects/lens-dev/new-codebase/lens.core.src` is a partial clean-room rebuild of the Lens Workbench module. The reference module has a mature lifecycle contract, config, 40+ lens-work skills, 50+ prompt files, helper scripts, tests, and governance operations. The target currently has a narrower set: 18 lens-work skill directories, 12 release prompts, 10 public prompt stubs, one top-level script, no target `lifecycle.yaml`, no target `bmadconfig.yaml`, and missing foundational skills such as `bmad-lens-feature-yaml`, `bmad-lens-git-state`, and `bmad-lens-dev`.

The technical plan is to close behavioral parity in layers: restore lifecycle/config/state foundations, fix git orchestration and topology semantics, reconcile the retained command surface, restore Dev/Complete orchestration, and run parity-focused regression suites. The implementation must reproduce outputs and contracts, not source structure for its own sake.

## Architecture Overview

| Layer | Responsibility | Current target status | Required action |
| --- | --- | --- | --- |
| Prompt entry | Public prompt stubs run prompt-start preflight and delegate to release prompts. | Partial: 10 public stubs observed. | Restore all retained command stubs and prompt-start regression coverage. |
| Release prompts | `_bmad/lens-work/prompts/lens-*.prompt.md` redirects to owning skill. | Partial: 12 release prompts observed. | Align with 17-command inventory. |
| Module metadata | `module.yaml`, help CSVs, manifests, command discovery. | Partial: duplicate `lens-expressplan` and incomplete retained surface. | Generate or validate from a single command inventory. |
| Lifecycle contract | Central phase, track, artifact, and review definitions. | Missing in target lens-work. | Recreate v4-compatible lifecycle contract from baseline behavior. |
| Config | Governance repo, control repo topology, target-project branch strategy, user defaults, output paths. | Missing in target lens-work. | Add `bmadconfig.yaml` and selected user config contract. |
| State operations | Feature YAML CRUD, feature-index sync, git state observation. | `feature-yaml` and `git-state` missing. | Implement behavior from baseline acceptance criteria. |
| Write orchestration | Branches, commits, pushes, PRs, publish-to-governance. | Present but needs bugfix parity. | Absorb BF-1 through BF-6 and artifact slug mapping. |
| Phase conductors | PrePlan, BusinessPlan, TechPlan, ExpressPlan, FinalizePlan. | Several present; foundations missing. | Reconnect through lifecycle/config/state abstractions. |
| Dev and Complete | Target-repo implementation loop and terminal closeout. | Dev missing; Complete partial. | Restore dev-session compatibility, retrospective-first closeout, and document-project gap handling. |

## ExpressPlanBugs.md Defect Intake

The ExpressPlan dogfood transcript records defects that must become implementation work rather than conversation-only lessons:

| Defect | Impact | Required fix |
| --- | --- | --- |
| Constitution resolver reported `express` as ignored while the governance constitution files permitted it. | ExpressPlan can false-block valid express-track features. | Update constitution resolution to merge and validate express-track permissions from the actual hierarchy, with a regression fixture for `lens-dev/new-codebase`. |
| Editor glob search and `rg` assumptions failed during setup. | Feature/config discovery becomes environment-dependent and brittle. | Add deterministic repository discovery helpers and fallbacks that do not depend on one editor search provider or `rg` availability. |
| Git Bash `/d/...` paths used with file-edit tooling created files under `C:/d/...`. | Planning artifacts can be written outside the workspace and validators cannot find them. | Add Windows-safe path normalization or explicit absolute-path validation before file writes and publish operations. |
| Default ExpressPlan publish mapping copied only the review artifact. | Governance mirrors can miss `business-plan.md`, `tech-plan.md`, and `sprint-plan.md` unless an operator knows the override. | Fix `publish-to-governance` phase artifact mapping so express publishes all required QuickPlan artifacts plus the review artifact by default. |
| Feature phase advanced but `feature-index.yaml` stayed stale because no sanctioned sync command exists. | `switch`, `next`, dashboards, and cross-feature context can read outdated status. | Add a sanctioned feature-index sync operation and call it after phase transitions; do not rely on hand edits. |
| Dirty governance state initially blocked phase advancement. | Required publishes can stall despite the user's expectation that repo changes are always published. | Make dirty-state handling explicit: pull, stage the relevant dirty files, commit, push, and report the SHA before continuing. |
| The public prompt chain gap was identified after initial planning. | Internal-only skills may appear complete while public user commands fail. | Treat public stubs, release prompts, module metadata, and skill owners as a single inventory contract. |
| Clean-room file hash verification happens outside VS Code. | In-editor validation cannot be the only clean-room evidence. | Emit traceable behavior/parity artifacts and keep source-copy avoidance explicit so external hash checks can run independently. |

### Defect-to-Story Traceability (H1 — FinalizePlan review response D)

All 8 defects must map to a story ID and acceptance criterion before Sprint 1 implementation begins. This table is the governance evidence that the defect intake was fully absorbed. A complete per-defect traceability table (with assigned story IDs) must appear in `implementation-readiness.md`; the mapping below is the planning-phase source from which that table is populated.

| # | Defect summary | Story | Acceptance criterion |
| --- | --- | --- | --- |
| 1 | Constitution resolver false negative for `express` track | S1.2 (lifecycle contract) + S2.4 (phase-start validation) | Constitution resolver correctly permits `express` track for `lens-dev/new-codebase`; regression fixture for this domain/service combination passes. |
| 2 | Editor glob / `rg` discovery assumptions | S1.3 (module config) | Config and feature discovery work without `rg` or any specific editor search provider; fallback path is tested. |
| 3 | Windows `/d/...` path form creates files outside workspace | S1.3 (module config) + S2.6 (express publish) | File write and publish operations use OS-normalized absolute paths; no artifact is written to `C:/d/...` or similar on Windows. |
| 4 | Express publish mapping copies only review artifact | S2.6 (express publish artifact mapping) | `publish-to-governance --phase expressplan` copies `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and the express review artifact; both `expressplan-adversarial-review.md` and legacy `expressplan-review.md` are recognized and reported. |
| 5 | `feature-index.yaml` goes stale after phase transitions | S2.3 (feature-index sync) | A sanctioned sync operation updates `feature-index.yaml` after every `feature.yaml` phase change; tested with a fixture that exercises the stale-entry scenario. |
| 6 | Dirty governance state blocks phase advancement | S1.4 (feature-yaml operations) + S2.4 (phase-start validation) | Dirty-state detection pulls, stages, commits, and pushes relevant changes, then reports the SHA before continuing; blocking on uncommitted changes is not valid behavior. |
| 7 | Public prompt chain gap discovered after planning | S3.1 (prompt stubs + release prompts) | Public stubs, release prompts, module metadata, and skill owners are validated as a single inventory; no retained command is parity-complete without a working public prompt chain. |
| 8 | Clean-room file hash verification outside VS Code | S5.5 (parity report) | Parity report includes a clean-room compliance checkpoint: external hash comparison result for all touched `lens.core.src` files against `lens.core` counterparts is included as evidence. |

## Design Decisions (ADRs)

### ADR-1: Restore lifecycle/config/state foundations first

**Decision:** Sprint 1 restores `lifecycle.yaml`, `bmadconfig.yaml`, `bmad-lens-feature-yaml`, and `bmad-lens-git-state` before command parity work is declared complete.

**Rationale:** Most retained commands depend on feature identity, docs paths, phase transitions, artifact contracts, and branch state. Without these foundations, phase conductors can appear present while failing at runtime.

**Alternatives Rejected:** Continue filling individual command stubs first. This increases the number of entry points that fail deeper in the workflow.

### ADR-2: Split governance, control, and target-project topology rules

**Decision:** Governance remains flat on `main`. The control repo uses a three-branch feature topology: `{featureId}`, `{featureId}-plan`, and `{featureId}-dev`. Target projects have independent branch strategy choices: direct writes to the default branch, `feature/{featureStub}`, or `feature/{featureStub}-{username}`.

**Rationale:** The workflow needs a stable control-repo promotion path without confusing that path with target-repo implementation branches. Planning before FinalizePlan belongs on `{featureId}-plan`; FinalizePlan belongs on `{featureId}`; FinalizePlan step 3 belongs on `{featureId}-dev`. Branch cleanup after PR merge is part of the topology contract.

**Alternatives Rejected:** Flat control topology is invalid for this workflow. Reusing target-project branch strategy for the control repo was rejected because target code branching and planning governance are separate concerns.

### ADR-3: Feature-index synchronization becomes an operation contract

**Decision:** Add or extend a sanctioned operation that synchronizes `feature-index.yaml` from `feature.yaml` after phase transitions.

**Rationale:** Direct governance edits are prohibited, but stale feature-index entries break routing, `next`, switch context, and dashboard-style views.

**Alternatives Rejected:** Allow hand edits of `feature-index.yaml`. This violates authority boundaries and creates more divergence.

### ADR-4: Validate parity through command traces and focused tests

**Decision:** Each retained command gets a trace from public stub to skill, dependencies, outputs, authority boundaries, and focused regression tests.

**Rationale:** File counts are diagnostics, not proof of parity. Behavior equivalence needs workflow traces and testable contracts.

**Alternatives Rejected:** Full-suite validation as the only gate. The discover retrospective showed broad-suite noise from unrelated skills; focused tests should lead, full suites should follow.

### ADR-5: Preserve express review compatibility while following current lifecycle

**Decision:** The current dogfood review artifact is `expressplan-adversarial-review.md` because the active lifecycle contract names that file. The implementation should also document compatibility debt around older `expressplan-review.md` references.

**Rationale:** The active prompt and lifecycle define the current gate, while baseline docs and prior artifacts contain naming drift. Tooling must be explicit rather than assume one hidden convention.

**Alternatives Rejected:** Rename artifacts ad hoc in planning docs. That would create another drift point.

## API Contracts

### Prompt Chain Contract

```text
.github/prompts/lens-{command}.prompt.md
  -> uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py
  -> _bmad/lens-work/prompts/lens-{command}.prompt.md
  -> owning _bmad/lens-work/skills/bmad-lens-{command}/SKILL.md or documented owner
```

Breaking change: false.

### Feature State Contract

`bmad-lens-feature-yaml` must read, validate, and surgically update `feature.yaml`, including identity fields, `phase`, `track`, milestones, docs paths, `target_repos`, dependencies, and transition history. Breaking change: false.

### Git State Contract

`bmad-lens-git-state` must remain read-only and report the three control branches, target-project branch strategy, active features, cross-feature context, and git-vs-yaml discrepancies. Breaking change: false.

### Git Orchestration Contract

`bmad-lens-git-orchestration` must create and manage `{featureId}`, `{featureId}-plan`, and `{featureId}-dev`, route artifacts to the correct branch by phase step, switch to the correct branch and pull before continuing, clean up local and remote branches after their PRs merge, publish to governance, map express artifacts, sync feature-index state, and prepare target-project branches using the selected target strategy. Breaking change: yes for the former 2-branch control model; governance remains flat and target-project branch strategy remains separate.

### Dev Handoff Contract

`bmad-lens-dev` must validate FinalizePlan artifacts, resolve the target repo, prepare a working branch, preserve `dev-session.yaml`, delegate story work within target repos only, run review, and open the implementation PR. Breaking change: false.

## Data Model Changes

No lifecycle schema migration is planned. The rebuild stays v4-compatible.

Potential additions are configuration-level, not feature schema breaking changes:

| Field | Location | Purpose |
| --- | --- | --- |
| `control_topology` | module config | Fixed value `3-branch` for `{featureId}`, `{featureId}-plan`, and `{featureId}-dev`. |
| `target_branch_strategy` | feature or repo config | Select `flat`, `feature/{featureStub}`, or `feature/{featureStub}-{username}` for target project writes. |
| `username` or `github_username` | user config | Persist branch identity for dev branches and target repo branch modes. |
| `default_branch` | module or repo config | Avoid silent fallback to `main` when `develop` is expected. |

Existing feature YAML files should continue to load without modification. `target_repos` should be populated by features that impact implementation repos, but missing values should be reported as warnings before becoming hard failures.

## Dependencies

- `uv` for validation and test scripts.
- `git` and optionally `gh` for branch/PR operations.
- YAML parsing for feature/config/index operations.
- Existing governance repo layout under `TargetProjects/lens/lens-governance`.
- BMad base skills invoked through the Lens wrapper, with output-path precedence enforced by Lens context.

## Rollout Strategy

1. Land foundations behind focused tests in `lens.core.src` without publishing the target as a replacement.
2. Reconcile command inventory and prompt surfaces to the retained 17 commands.
3. Run focused traces for `new-feature`, `next`, `expressplan`, `finalizeplan`, `dev`, `complete`, and `discover`.
4. Run scoped pytest commands with `uv run python -m pytest` on Windows.
5. Run broader parity suites only after focused tests pass, documenting inherited failures separately.

Rollback plan: revert or disable target module registration for any restored command whose focused trace fails. Do not mutate governance state manually to recover.

## Testing Strategy

| Test area | Required coverage |
| --- | --- |
| Prompt-start preflight | Every retained public stub runs `light-preflight.py` and stops on non-zero exit. |
| Inventory parity | Public stubs, release prompts, `module.yaml`, help CSVs, and manifests agree on the retained 17-command set. |
| Feature YAML | Read, validate, update phase, update docs paths, preserve unknown fields, reject invalid transitions. |
| Git state | Read-only branch detection, topology reporting, discrepancy surfacing, active feature listing. |
| Git orchestration | BF-1 through BF-6, base branch validation, feature-index sync, publish-to-governance mapping. |
| ExpressPlan dogfood defects | Constitution resolver express-track regression, deterministic discovery fallback, Windows path normalization, express artifact publish mapping, dirty-state commit/push handling, and external clean-room hash-check readiness. |
| Phase gates | Review-ready fast path, missing artifact failure, no advance on review fail. |
| Wrapper output paths | Lens context overrides global planning/implementation fallback paths. |
| Dev handoff | Target repo only writes, dev-session compatibility, per-story commits, final PR. |
| Complete | Retrospective-before-archive, document-project gap handling, commit/push/merge automation. |

## Observability

The rebuild should expose structured output for preflight, phase artifact validation, feature-yaml operations, git-state discrepancies, git-orchestration operations, Dev session progress, and command inventory parity checks. Each lifecycle operation should identify feature ID, phase, track, docs path, governance repo path, and topology mode.

## Open Questions

1. Should `bmad-lens-quickplan` in the target stay internal-only, or does parity require broader QuickPlan conductor semantics?
2. Which config path is authoritative for persisted username and topology?
3. Should express review tooling emit only `expressplan-adversarial-review.md`, or also recognize legacy `expressplan-review.md` when publishing old artifacts?
4. Should `bmad-lens-document-project` be rebuilt inside this dogfood feature or tracked as a separate Complete-flow hardening feature?

## Retrospective Tech Debt Notes

- Express review artifact compatibility is accepted as tech debt: current tooling should canonicalize on `expressplan-adversarial-review.md` while recognizing older `expressplan-review.md` files during reads and publication.
- The topology correction supersedes the earlier flat-control-repo idea. Any remaining references to flat topology must be scoped only to target-project branch strategy.
