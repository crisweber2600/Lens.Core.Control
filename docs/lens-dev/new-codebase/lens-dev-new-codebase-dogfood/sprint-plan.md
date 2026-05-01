---
feature: lens-dev-new-codebase-dogfood
doc_type: sprint-plan
status: draft
goal: "Sequence clean-room parity work into executable rebuild slices for lens.core.src."
key_decisions:
  - Deliver foundations before retained command polish.
  - Treat three-branch control topology and feature-index synchronization as Sprint 2 blockers for reliable phase progression.
  - Reserve Dev and Complete restoration for a dedicated sprint because they define the implementation handoff and terminal closeout path.
  - Validate with focused parity tests before broad suite runs.
open_questions:
  - Does QuickPlan parity require a public command path?
  - Which Complete-flow documentation skill fills the document-project gap?
depends_on:
  - business-plan.md
  - tech-plan.md
blocks:
  - Dev execution for dogfood parity remains blocked until Sprint 4 restores the missing Dev conductor.
updated_at: '2026-05-01T00:00:00Z'
---

# Sprint Plan - Dogfood Clean-Room Parity

## Sprint Overview

| Sprint | Goal | Stories | Complexity | Primary risks |
| --- | --- | --- | --- | --- |
| 1 | Restore lifecycle, config, and state foundations. | S1.1-S1.5 | L | Missing foundations may expose additional target gaps. |
| 2 | Integrate git orchestration and topology bugfixes. | S2.1-S2.6 | L | Three-branch control topology must stay separate from target-project branch strategy. |
| 3 | Reconcile retained command surfaces and phase conductors. | S3.1-S3.5 | XL | Prompt/module/help/manifests can drift unless inventory is generated from one source. |
| 4 | Restore Dev and Complete handoff behavior. | S4.1-S4.5 | XL | Dev target-repo writes and retrospectives have high authority-boundary risk. |
| 5 | Prove parity through regression and workflow traces. | S5.1-S5.5 | L | Broad tests may include inherited failures unrelated to dogfood. |

## Sprint 1 - Foundation Restoration

**Goal:** Make the target module capable of resolving lifecycle, config, feature state, docs paths, and read-only branch context before deeper command work proceeds.

| Story | Type | Title | Estimate | Acceptance summary |
| --- | --- | --- | --- | --- |
| S1.1 | new | Build retained-command parity map in target docs/tests | S | The 17-command baseline is captured as a machine-checkable inventory and clean-room traceability document. |
| S1.2 | new | Restore v4-compatible lifecycle contract | M | Target has lifecycle phases, tracks, ExpressPlan review contract, artifact names, and validation fixtures. |
| S1.3 | new | Add target module config and user config contract | M | Governance repo, 3-branch control topology, target-project branch strategy, username, and output paths resolve without hardcoded workspace assumptions. |
| S1.4 | new | Implement feature-yaml state operations | L | Read/validate/update operations preserve v4 schema and update phase/docs fields surgically. |
| S1.5 | new | Implement read-only git-state operations | M | Branch topology, active features, and git-vs-yaml discrepancies are reported without writes. |

## Sprint 2 - Git Orchestration and Topology Bugfixes

**Goal:** Absorb the known bugfix backlog into the sanctioned write path so phase progression and branch setup are reliable.

| Story | Type | Title | Estimate | Acceptance summary |
| --- | --- | --- | --- | --- |
| S2.1 | fix | Implement 3-branch control topology | L | `{featureId}`, `{featureId}-plan`, and `{featureId}-dev` are created, tracked, and validated for each control feature. |
| S2.2 | fix | Route phase artifacts to the correct control branch | M | Planning before FinalizePlan writes to `{featureId}-plan`, FinalizePlan writes to `{featureId}`, and FinalizePlan step 3 writes to `{featureId}-dev`. |
| S2.3 | fix | Resolve BF-3 feature-index synchronization | M | A sanctioned operation syncs feature-index entries after feature.yaml phase transitions. |
| S2.4 | fix | Resolve BF-4 and BF-5 phase-start/base-branch validation | M | Planning phase start verifies required branches and fails on missing intended base branches instead of silently falling back. |
| S2.5 | fix | Add branch cleanup and branch-switch discipline | M | After each PR lands, local and remote branches are cleaned up, then the workflow switches to the correct next branch and pulls before continuing. |
| S2.6 | fix | Add express publish artifact mapping | M | `publish-to-governance` recognizes `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and both current and legacy express review filenames. **Additional AC (M3 — FinalizePlan review response A):** When resolving express track review artifact, the operation must recognize both `expressplan-adversarial-review.md` and legacy `expressplan-review.md` and must report which filename was matched in the output. |

## Sprint 3 - Retained Command Surface and Phase Conductors

**Goal:** Bring the target public and internal command surfaces into alignment with the 17-command baseline.

| Story | Type | Title | Estimate | Acceptance summary |
| --- | --- | --- | --- | --- |
| S3.1 | new | Reconcile public prompt stubs and release prompts | M | Public stubs, release prompts, and prompt-start behavior exist for all retained commands; producing an artifact without the public prompt chain is not parity-complete. |
| S3.2 | fix | Reconcile module.yaml, help CSVs, manifests, and discovery | M | The target no longer has duplicate `lens-expressplan`; retained commands agree across all metadata surfaces; duplicate/missing inventory validation prevents recurrence. |
| S3.3 | new | Restore missing command skills and internal dependencies | L | Missing retained commands such as `dev`, `split-feature`, `upgrade`, and public constitution support have owning skills or documented owners. |
| S3.4 | fix | Normalize Lens wrapper output-path precedence | L | BMad delegated skills write to feature docs paths, not global planning or implementation fallbacks. |
| S3.5 | new | Decide and implement QuickPlan parity shape | M | QuickPlan is either public-compatible or explicitly internal-only with tests proving ExpressPlan behavior is intact. |

## Sprint 4 - Dev and Complete Handoff Restoration

**Goal:** Restore the path from FinalizePlan into implementation and terminal closeout.

> **Governance coordination note (M2 — FinalizePlan review response A):** Sprint 4 Dev conductor stories are scoped as **parity-implementation work** against the existing reference module (`lens.core`). They must not duplicate or pre-empt the canonical feature `lens-dev-new-codebase-dev`, which is the first-class governance feature for the Dev command. Each S4 story file must include an `## Implementation Channel` section and a coordination note stating: _"This story reproduces reference behavior for clean-room parity testing. First-class Dev command authoring belongs to `lens-dev-new-codebase-dev`; any overlap with that feature's eventual PRD must be surfaced as an explicit acceptance criteria alignment before implementation proceeds."_

| Story | Type | Title | Estimate | Acceptance summary |
| --- | --- | --- | --- | --- |
| S4.1 | new | Implement Dev phase conductor | XL | Dev validates FinalizePlan artifacts, resolves registered target repos, prepares the selected target branch strategy, delegates stories, and opens final PR. Coordination note re `lens-dev-new-codebase-dev` required in story file. |
| S4.2 | new | Preserve dev-session.yaml compatibility | M | Existing checkpoint fields remain loadable; resume behavior is tested. Coordination note re `lens-dev-new-codebase-dev` required in story file. |
| S4.3 | new | Implement target-repo branch preparation | M | Target repo writes stay outside control/governance repos and branch mode is explicit. Coordination note re `lens-dev-new-codebase-dev` required in story file. |
| S4.4 | fix | Complete retrospective-first closeout automation | L | Complete writes retrospective, documents project state, commits/pushes, and archives only after documentation exists. Coordination note re `lens-dev-new-codebase-dev` required in story file. |
| S4.5 | fix | Fill document-project and discover retrospective lessons | M | `bmad-lens-document-project` gap is resolved or substituted; discover repo-inventory overwrite risk gets a create-only guard. Coordination note re `lens-dev-new-codebase-dev` required in story file. |

## Sprint 5 - Regression, Parity Proof, and Release Readiness

**Goal:** Prove output parity and document any remaining compatibility debt.

| Story | Type | Title | Estimate | Acceptance summary |
| --- | --- | --- | --- | --- |
| S5.1 | confirm | Run focused tests for touched skills | M | Skill-specific tests pass using `uv run python -m pytest` on Windows. |
| S5.2 | confirm | Run command trace validation for all 17 retained commands | M | Each command traces from public prompt to owner skill, dependencies, outputs, and authority boundary. |
| S5.3 | confirm | Run ExpressPlan-to-FinalizePlan dry run | M | Express artifacts validate review-ready and no direct governance writes occur. |
| S5.4 | confirm | Run FinalizePlan-to-Dev dry run | M | Dev handoff validates target repo resolution and session creation without executing unsafe writes. |
| S5.5 | confirm | Publish parity report and known compatibility debt | S | Report lists green contracts, accepted warnings, inherited failures, and follow-up backlog. |
| S5.6 | confirm | Document express review filename compatibility mapping | S | **New story (M3 — FinalizePlan review response A).** Parity report includes an explicit section documenting: (a) `expressplan-adversarial-review.md` is the current canonical filename per the active lifecycle contract; (b) `expressplan-review.md` is the legacy filename recognized for backward compatibility; (c) the implementation location in `publish-to-governance` where both names are resolved; (d) any remaining governance artifacts that still use the legacy name. |

## Cross-Sprint Dependencies

- S2 branch/topology work depends on S1 config and git-state.
- S3 phase conductor reconciliation depends on S1 feature-yaml and lifecycle validation.
- S4 Dev depends on S2 git-orchestration and S3 command surface reconciliation.
- S5 dry runs depend on S1-S4 foundations and should not be started as a substitute for implementation.

## Story Template Requirements (H2 — FinalizePlan review response D)

Every story file produced from this sprint plan must include an `## Implementation Channel` section. Stories touching any file under `lens.core.src/_bmad/lens-work/` must name the required BMB path:

- **SKILL.md changes:** use `.github/skills/bmad-module-builder` (BMB module builder skill)
- **Release prompts / workflows:** use `.github/skills/bmad-workflow-builder` (BMB workflow builder skill)
- **Stories that do NOT touch lens-work:** must explicitly declare the exception and identify what they touch instead

Stories that do not include this section are incomplete. Implementation agents must load the BMB documentation index (`TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md`) before authoring any skill, workflow, or prompt artifact in `lens.core.src`.

## Definition of Done

- The clean-room rule is documented in every implementation story that compares against `lens.core`.
- Every changed retained command has a prompt chain test or command trace.
- Every state-changing operation uses sanctioned feature-yaml or git-orchestration paths.
- Governance mirrors are updated only through publish-to-governance or explicitly sanctioned governance operations.
- Governance repo history remains flat on `main`; control repo work follows the 3-branch feature topology; target projects use their selected branch strategy independently.
- Windows validation uses `uv run python -m pytest` for pytest execution.
- Known naming drift such as `expressplan-review.md` versus `expressplan-adversarial-review.md` is documented and tested for compatibility where needed.
- Feature creation or phase readiness must require or warn on missing `target_repos` for implementation-impacting features so M4 does not recur.
- The target module can execute the planned workflow step or produces a structured blocker explaining exactly which contract remains missing.
