---
feature: lens-dev-new-codebase-dogfood
doc_type: business-plan
status: draft
goal: "Plan clean-room dogfood parity for lens.core.src against the Lens 17-command baseline."
key_decisions:
  - Treat the baseline feature docs as the approved behavior contract and lens.core as a comparison reference, not a source to copy.
  - Prioritize lifecycle, configuration, feature state, git state, git orchestration, and Dev handoff foundations before command polish.
  - Integrate the bugfix backlog into the rebuild scope, especially branch topology, feature-index synchronization, base-branch validation, and closeout automation.
  - Replace the earlier flat-control-repo idea with a three-branch control topology: feature base, plan branch, and dev branch.
  - Keep governance flat on main while allowing target projects to choose flat, feature branch, or feature branch with username strategies.
  - Preserve lifecycle v4 compatibility, retained command behavior, prompt-start preflight, feature identity, and governance authority boundaries.
  - Treat ExpressPlanBugs.md as a dogfood defect intake source for workflow, tooling, publish, and environment issues observed during ExpressPlan execution.
open_questions:
  - Should QuickPlan remain internal-only in the clean-room rebuild, or preserve the reference module's standalone conductor behavior?
  - What is the exact persistent user config path for GitHub username and branch identity in the new codebase?
depends_on:
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/prd.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/docs/research.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/bugfixes.md
  - TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/ExpressPlanBugs.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-discover/retrospective.md
blocks:
  - Full feature parity is blocked until lifecycle/config/state foundations and the Dev conductor exist in lens.core.src.
updated_at: '2026-05-01T00:00:00Z'
---

# Business Plan - Dogfood Clean-Room Parity

## Executive Summary

The `lens-dev-new-codebase-dogfood` feature plans the next clean-room rebuild slice for `TargetProjects/lens-dev/new-codebase/lens.core.src`. The objective is to close the gap between the target codebase and the approved Lens baseline while respecting the clean-room rule: existing `lens.core` behavior may be studied and compared, but source files and prose must not be copied into the target. Success means the new codebase can reproduce retained Lens workflow outcomes, absorb known bugfix lessons, and use its own governed planning flow as evidence that the rebuild is stable enough to continue.

This is not a single-command patch. The target currently has a partial command surface and several missing foundations: no target lifecycle contract, no target module config, no `bmad-lens-feature-yaml`, no `bmad-lens-git-state`, no `bmad-lens-dev`, incomplete public-stub coverage, incomplete module registration, and a duplicate `lens-expressplan` entry in target `module.yaml`.

## Business Context

The approved baseline defines the rewrite as a 17-command stable surface, not a feature expansion. The retained commands are `preflight`, `new-domain`, `new-service`, `new-feature`, `switch`, `next`, `preplan`, `businessplan`, `techplan`, `finalizeplan`, `expressplan`, `dev`, `complete`, `split-feature`, `constitution`, `discover`, and `upgrade`.

The baseline also freezes several contracts: every public prompt starts with `light-preflight.py`, feature IDs use `{domain}-{service}-{featureSlug}`, planning artifacts publish through `publish-to-governance`, `next` handoffs are pre-confirmed, lifecycle schema remains v4-compatible, and `dev-session.yaml` remains backwards compatible.

Recent dogfood work adds required corrections that should be integrated now: BF-1 dev branch behavior, BF-2 username persistence, BF-3 `feature-index.yaml` synchronization, BF-4 phase-start branch verification, BF-5 base-branch validation, the BF-6 topology correction, missing Dev startup after FinalizePlan, and Complete automation for retrospective commit/push/merge.

`ExpressPlanBugs.md` adds a second dogfood bugfix stream from the actual ExpressPlan execution. The rebuild must fix the constitution resolver false negative that ignored `express` even when governance files permitted it, make command/config discovery deterministic when editor glob search or `rg` is unavailable, prevent Windows path-form mistakes that can create artifacts outside the workspace, correct express publish defaults so QuickPlan artifacts are mirrored without explicit overrides, provide a sanctioned `feature-index.yaml` sync operation, and treat dirty repo state as a commit/push responsibility rather than a blocker that strands generated artifacts. These are workflow reliability issues, not polish; they directly affect whether the new codebase can run its own lifecycle.

The updated topology decision is explicit. The governance repo is always flat on `main`. The control repo now uses a three-branch feature topology: `{featureId}`, `{featureId}-plan`, and `{featureId}-dev`. Planning items before FinalizePlan go to `{featureId}-plan`; FinalizePlan itself goes to `{featureId}`; FinalizePlan step 3 goes to `{featureId}-dev`. After each PR lands, local and remote branches must be cleaned up, and the workflow must switch to the correct next branch and pull before continuing. Target projects keep their own branch strategy choices: direct default branch writes, `feature/{featureStub}`, or `feature/{featureStub}-{username}`.

## Stakeholders

| Stakeholder | Need |
| --- | --- |
| Lens maintainers | A clean-room target that can replace the reference module without breaking active features. |
| Feature authors | A predictable 17-command surface with working prompt chains and no missing internal dependencies. |
| Governance reviewers | Evidence that authority boundaries are preserved and planning artifacts move through sanctioned paths. |
| Implementation agents | Clear stories that identify behavior to reproduce without copying source files. |
| Future dogfood sessions | Stable state, branch, and retrospective mechanics so follow-on work does not restart from ambiguity. |

## Success Criteria

1. `lens.core.src` contains the foundations needed by all retained commands: lifecycle contract, module config, feature-yaml operations, read-only git-state, and git-orchestration behavior aligned to the bugfix backlog.
2. The published prompt surface, release prompts, module metadata, help/discovery surfaces, and command inventory agree on the same 17 retained commands.
3. ExpressPlan can produce `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and an express review artifact in the feature docs path, then hand off to FinalizePlan without direct governance writes.
4. FinalizePlan can reach Dev, and Dev can resolve target repositories, persist compatible `dev-session.yaml`, execute story work in target repos only, and open an implementation PR.
5. Complete writes a retrospective before archive and supports the intended commit/push/merge automation without hand-edited governance state.
6. Bugfixes BF-1 through BF-6 are either implemented or explicitly split into tracked follow-up stories with acceptance criteria.
7. Validation uses behavior-level parity tests and command traces rather than copied implementation text.
8. ExpressPlanBugs.md issues are converted into implementation stories or acceptance criteria covering resolver parity, publish parity, Windows-safe paths, feature-index sync, deterministic discovery, and repo clean-state publishing.

## Scope

### In Scope

- Clean-room parity planning for `TargetProjects/lens-dev/new-codebase/lens.core.src` against the baseline and reference behavior.
- Integration of the bugfix backlog into the delivery plan.
- Integration of ExpressPlanBugs.md as execution-derived defect intake for business and technical planning.
- Restoration of missing P0 lifecycle, config, feature-state, git-state, and Dev orchestration capabilities.
- Prompt/module/discovery surface reconciliation for the retained 17 commands.
- Targeted regression strategy for prompt-start routing, phase gates, branch topology, publish-to-governance, and dev handoff.
- Retrospective lessons from the discover feature: Windows-safe pytest invocation, broad-suite noise, repo-inventory side effects, and the document-project gap.

### Out of Scope

- Copying files, code, or prose from `lens.core` into `lens.core.src`.
- Changing lifecycle schema beyond v4 compatibility.
- Hand-editing governance mirrors or `feature-index.yaml` as part of this planning packet.
- Rewriting unrelated BMad base modules outside the Lens wrapper behavior needed for parity.
- Declaring parity based on file counts alone; parity is behavioral and must be verified command by command.

## Risks and Mitigations

| Risk | Probability | Impact | Mitigation |
| --- | --- | --- | --- |
| The plan understates foundational gaps because some phase conductors already exist. | High | High | Treat P0 foundations as Sprint 1 and do not declare command parity until feature-yaml, git-state, lifecycle, config, and Dev conductor are present. |
| Topology requirements conflict: older baseline says 2-branch while dogfood now requires 3 control branches. | High | High | Implement the new 3-branch control topology, keep governance flat, and keep target-project branching strategy separate. |
| Clean-room boundary is blurred by comparing against `lens.core`. | Medium | High | Use comparison only to identify behavior and missing surfaces; write implementation from baseline contracts and acceptance criteria. |
| `feature-index.yaml` remains stale after phase transitions. | High | High | Add explicit feature-index sync behavior to sanctioned operations and test it. |
| Dev handoff remains broken because target lacks `bmad-lens-dev`. | High | High | Restore Dev conductor as a P1 capability, not later polish. |
| Windows test invocation produces false failures. | Medium | Medium | Standardize `uv run python -m pytest` for Windows validation and scope tests to touched skills first. |

## Timeline Expectations

This dogfood feature should be delivered as a staged parity recovery. Sprint 1 restores foundations, Sprint 2 absorbs git/topology bugfixes, Sprint 3 restores retained command surfaces, Sprint 4 completes Dev/Complete flow, and Sprint 5 hardens regression parity. Each sprint should leave the target codebase more executable than it started.
