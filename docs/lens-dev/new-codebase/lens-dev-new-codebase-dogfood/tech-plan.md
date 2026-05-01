---
feature: lens-dev-new-codebase-dogfood
doc_type: tech-plan
status: draft
goal: "Define clean-room implementation architecture for closing lens.core.src parity gaps."
key_decisions:
  - Rebuild behavior from baseline contracts and observed outcomes; do not copy implementation files from lens.core.
  - Restore P0 platform foundations before declaring any retained command complete.
  - Make topology policy explicit and configurable so BF-6 can coexist with existing 2-branch features.
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
| Config | Governance repo, control repo, topology, user defaults, output paths. | Missing in target lens-work. | Add `bmadconfig.yaml` and selected user config contract. |
| State operations | Feature YAML CRUD, feature-index sync, git state observation. | `feature-yaml` and `git-state` missing. | Implement behavior from baseline acceptance criteria. |
| Write orchestration | Branches, commits, pushes, PRs, publish-to-governance. | Present but needs bugfix parity. | Absorb BF-1 through BF-6 and artifact slug mapping. |
| Phase conductors | PrePlan, BusinessPlan, TechPlan, ExpressPlan, FinalizePlan. | Several present; foundations missing. | Reconnect through lifecycle/config/state abstractions. |
| Dev and Complete | Target-repo implementation loop and terminal closeout. | Dev missing; Complete partial. | Restore dev-session compatibility, retrospective-first closeout, and document-project gap handling. |

## Design Decisions (ADRs)

### ADR-1: Restore lifecycle/config/state foundations first

**Decision:** Sprint 1 restores `lifecycle.yaml`, `bmadconfig.yaml`, `bmad-lens-feature-yaml`, and `bmad-lens-git-state` before command parity work is declared complete.

**Rationale:** Most retained commands depend on feature identity, docs paths, phase transitions, artifact contracts, and branch state. Without these foundations, phase conductors can appear present while failing at runtime.

**Alternatives Rejected:** Continue filling individual command stubs first. This increases the number of entry points that fail deeper in the workflow.

### ADR-2: Treat topology as configuration, not a hardcoded invariant

**Decision:** Add an explicit topology setting with `flat` as the proposed default for new control repos and `2-branch` as a compatibility mode for existing features.

**Rationale:** The baseline preserved the 2-branch model, but BF-6 reports critical friction and requests flat default behavior. A configurable topology reconciles both without breaking existing feature branches.

**Alternatives Rejected:** Replace 2-branch behavior everywhere. This would break active features and contradict the baseline. Keep 2-branch only. This would ignore the highest-priority dogfood feedback.

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

`bmad-lens-git-state` must remain read-only and report topology, branch existence, active features, cross-feature context, and git-vs-yaml discrepancies. Breaking change: false.

### Git Orchestration Contract

`bmad-lens-git-orchestration` must support topology-aware branch creation, optional dev branch behavior, structured commit/push, publish-to-governance, express artifact mapping, feature-index sync, and target-repo dev branch preparation. Breaking change: conditional for new flat-default behavior only.

### Dev Handoff Contract

`bmad-lens-dev` must validate FinalizePlan artifacts, resolve the target repo, prepare a working branch, preserve `dev-session.yaml`, delegate story work within target repos only, run review, and open the implementation PR. Breaking change: false.

## Data Model Changes

No lifecycle schema migration is planned. The rebuild stays v4-compatible.

Potential additions are configuration-level, not feature schema breaking changes:

| Field | Location | Purpose |
| --- | --- | --- |
| `topology` | module or user config | Select `flat` or `2-branch` behavior. |
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
