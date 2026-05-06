---
feature: lens-dev-new-codebase-quickdev-expressplan
doc_type: epics
status: approved
stepsCompleted: [step-01-validate-prerequisites, step-02-design-epics, step-03-create-stories, step-04-final-validation]
inputDocuments:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-quickdev-expressplan/business-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-quickdev-expressplan/tech-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-quickdev-expressplan/sprint-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-quickdev-expressplan/expressplan-adversarial-review.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-quickdev-expressplan/finalizeplan-review.md
goal: "Decompose the lens-quickdev wrapper into user-value epics and development-ready stories."
key_decisions:
  - Use the current feature packet as the authoritative requirement source in the absence of standalone PRD, architecture, and UX documents.
  - Treat business-plan.md as the primary requirement source, tech-plan.md as the architecture source, and sprint-plan.md plus the review artifacts as supporting constraint inputs.
  - Organize work as safe quickdev entry, safe execution, and safe audit/publication outcomes rather than technical layers.
  - Seed the downstream bundle with one ready-for-dev story while keeping the remaining approved stories in backlog order.
open_questions: []
depends_on:
  - business-plan.md
  - tech-plan.md
  - sprint-plan.md
  - expressplan-adversarial-review.md
  - finalizeplan-review.md
blocks: []
updated_at: '2026-05-06T21:05:00Z'
---

# lens-quickdev Wrapper - Epic Breakdown

## Overview

This document provides the complete epic and story breakdown for the `lens-quickdev` wrapper using the current feature packet as the authoritative analysis set. No standalone PRD, architecture, or UX specification was present in the feature folder, so the business plan, tech plan, sprint plan, and both review artifacts are treated as the approved requirement sources for epic and story design.

## Requirements Inventory

### Functional Requirements

FR1: Users can invoke a public `lens-quickdev` command for a scoped implementation ask.
FR2: The wrapper can resolve the active feature automatically and can honor an explicit `--feature-id` override.
FR3: The wrapper can block execution unless the feature is dev-ready.
FR4: The wrapper can resolve the target repository from `feature.yaml.target_repos[0]` and block without guessing when it is absent.
FR5: The wrapper can resolve the staged docs path and governance docs path for the active feature.
FR6: The wrapper can inspect current branch state, worktree cleanliness, relevant implementation surfaces, and likely validation paths before implementation begins.
FR7: The wrapper can create a new versioned quickdev evidence artifact for each run under `quickdev/quickdev-[summaryofrequeststub]-vNNN.md`.
FR8: The wrapper can record request, assessment, assumptions, validation plan, and implementation plan in the versioned quickdev artifact before implementation proceeds.
FR9: The wrapper can delegate implementation through the existing `bmad-quick-dev` workflow instead of creating a second implementation engine.
FR10: The wrapper can commit directly to an active in-progress feature branch when that branch already exists for the feature.
FR11: The wrapper can create or verify a working branch and PR through Lens git orchestration when no active in-progress feature branch exists.
FR12: The wrapper can run focused validation selected during assessment before completion.
FR13: The wrapper can create a conventional commit for implementation changes when the run produces non-empty changes.
FR14: The wrapper can record a no-op outcome without creating an empty commit when no implementation changes are produced.
FR15: The wrapper can stop before commit creation and mark the quickdev artifact `blocked` when validation fails before commit creation.
FR16: The wrapper can keep a failing post-commit result local, avoid push or PR creation, and request fix-forward or revert direction when validation fails after a local commit.
FR17: The wrapper can avoid rewriting shared history and can record blocked or fix-forward recovery when validation fails after push or PR creation.
FR18: The wrapper can publish the exact versioned quickdev artifact to governance after local evidence is finalized.
FR19: The wrapper can update feature-associated control-repo docs when command contract or operator guidance changes.
FR20: The workflow can warn the user and record an approved override before expanding beyond feature-associated control-repo docs into broader control-repo or packaging/discovery surfaces.
FR21: The implementation can preserve `/lens-bug-quickdev` as an unchanged bug-specific route.

### NonFunctional Requirements

NFR1: The wrapper must remain conductor-only and must not duplicate the implementation behavior of `bmad-quick-dev`.
NFR2: The wrapper must not guess target repositories or bypass FinalizePlan metadata gates.
NFR3: Quickdev evidence must be durable, versioned, and non-destructive across reruns of the same ask.
NFR4: Governance publication must use the sanctioned Lens publication path rather than direct governance file authoring.
NFR5: The workflow must preserve write-boundary safety by avoiding direct writes to `lens.core/`, unauthorized governance paths, or unrelated control-repo surfaces.
NFR6: Branch and PR handling must avoid rewriting shared history after push.
NFR7: Command discovery surfaces must expose `lens-quickdev` exactly once where owned by the implementation slice.
NFR8: Validation and regression coverage must include dev-ready gating, missing target repo blocking, branch policy, validation-failure behavior, rerun versioning, governance publication, and bug quickdev non-regression.
NFR9: Quickdev evidence must remain traceable to branch, commit, validation command, changed files, and PR state.
NFR10: If required non-source work expands beyond feature docs, the scope increase must be surfaced explicitly before implementation continues.

### Additional Requirements

- `lens-quickdev` must start with prompt preflight using `--caller lens-quickdev`.
- The public command must live in `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-quickdev.prompt.md` and delegate to `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-quickdev/SKILL.md`.
- Registration updates are expected in `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/module.yaml` and `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/module-help.csv`.
- FinalizePlan owns registration of `lens.core.src` in `feature.yaml.target_repos` before strict handoff validation.
- Quickdev evidence artifacts live under `feature.yaml.docs.path/quickdev/` and publish to `feature.yaml.docs.governance_docs_path/quickdev/`.
- Feature-associated control-repo docs are in scope by default; broader documentation or packaging/discovery work requires a scope-creep warning and recorded override.
- The implementation must preserve the branch policy decision: direct commit on active feature branches, branch-and-PR flow otherwise.
- The implementation must preserve the staged validation-failure contract for pre-commit, local post-commit, and pushed-or-PR failures.

### UX Design Requirements

UX-DR1: Command help and operator-facing docs must state clearly that `lens-quickdev` is a governed dev-ready-only wrapper, not a shortcut around planning.
UX-DR2: The command surface must make branch policy and validation outcomes legible enough for maintainers to understand whether the run produced a commit, a local-only failure, a PR, or a blocked result.
UX-DR3: Versioned quickdev evidence should keep reruns separate and easy to inspect by storing artifacts under a dedicated `quickdev/` folder.
UX-DR4: If implementation discovers broader non-source work, the user experience must include a scope-creep warning and a recorded override before those edits happen.

### FR Coverage Map

FR1: Epic 1 / Story 1.1 - Expose `lens-quickdev` as a public command surface.
FR2: Epic 1 / Story 1.3 - Resolve active feature context and explicit feature overrides.
FR3: Epic 1 / Story 1.3 - Enforce the dev-ready gate before any implementation work.
FR4: Epic 1 / Story 1.3 and Epic 3 / Story 3.2 - Resolve and register target repo metadata safely.
FR5: Epic 1 / Story 1.3 - Resolve staged docs and governance docs paths.
FR6: Epic 1 / Story 1.4 - Capture branch, worktree, and focused validation assessment.
FR7: Epic 1 / Story 1.4 - Create versioned quickdev artifacts in `quickdev/`.
FR8: Epic 1 / Story 1.4 - Record request, assumptions, validation plan, and implementation plan before delegation.
FR9: Epic 2 / Story 2.1 - Delegate implementation through the existing `bmad-quick-dev` engine.
FR10: Epic 2 / Story 2.2 - Commit directly to active in-progress feature branches.
FR11: Epic 2 / Story 2.2 - Create or verify working branches and PRs when direct commit is not allowed.
FR12: Epic 2 / Story 2.3 - Run focused validation selected during assessment.
FR13: Epic 2 / Story 2.3 - Create conventional commits for non-empty implementation changes.
FR14: Epic 2 / Story 2.3 - Record no-op outcomes without empty commits.
FR15: Epic 2 / Story 2.4 - Mark the run blocked before commit when validation fails early.
FR16: Epic 2 / Story 2.4 - Keep failing local commits unpushed and request fix-forward or revert direction.
FR17: Epic 2 / Story 2.4 - Preserve shared history after push or PR creation and record recovery action.
FR18: Epic 3 / Story 3.1 - Publish the exact versioned quickdev artifact to governance.
FR19: Epic 3 / Story 3.3 - Update feature-associated control-repo docs when operator guidance changes.
FR20: Epic 3 / Story 3.3 - Warn and record overrides before broader non-source scope expansion.
FR21: Epic 2 / Story 2.4 - Preserve `/lens-bug-quickdev` routing behavior.

## Epic List

### Epic 1: Governed Quickdev Entry and Planning Gate
Users can start a `lens-quickdev` run safely, with the command resolving feature context, enforcing dev-ready and target-repo gates, assessing the codebase, and creating a versioned evidence scaffold before any implementation begins.
**FRs covered:** FR1, FR2, FR3, FR4, FR5, FR6, FR7, FR8

### Epic 2: Scoped Implementation Execution and Branch Control
Users can execute the ask through the existing `bmad-quick-dev` engine with correct branch policy, validation, commit and no-op handling, and safe recovery behavior across all failure timings.
**FRs covered:** FR9, FR10, FR11, FR12, FR13, FR14, FR15, FR16, FR17, FR21

### Epic 3: Audit Trail, Publication, and Safe Surface Expansion
Users can rely on durable published quickdev evidence, FinalizePlan-owned metadata reconciliation, and explicit scope-creep handling for broader non-source documentation or packaging work.
**FRs covered:** FR18, FR19, FR20, and the handoff portion of FR4

## Epic 1: Governed Quickdev Entry and Planning Gate

Deliver a safe, discoverable starting point for `lens-quickdev`. This epic makes the wrapper visible to users, enforces the lifecycle and metadata gates that keep it governed, and creates the versioned quickdev evidence scaffold before any code changes are delegated.

### Story 1.1: Add Public Prompt and Skill Surfaces

As a Lens maintainer,
I want `lens-quickdev` to exist as a public prompt and owned skill surface,
So that users can invoke the wrapper without manually routing through `lens-bmad-skill`.

**Acceptance Criteria:**

**Given** the source module command surfaces are inspected
**When** `lens-quickdev` is implemented
**Then** `_bmad/lens-work/prompts/lens-quickdev.prompt.md` exists as a redirect-only prompt
**And** `_bmad/lens-work/skills/lens-quickdev/SKILL.md` exists as the owning conductor contract.

**Given** the prompt is opened for review
**When** the wrapper contract is checked
**Then** the prompt contains no business logic beyond preflight and skill loading
**And** the implementation behavior is owned by the skill.

**Given** the wrapper is reviewed against the existing quick-dev engine
**When** the handoff path is traced
**Then** there is exactly one implementation engine: `bmad-quick-dev`
**And** `lens-quickdev` adds only governance, context, and evidence orchestration.

### Story 1.2: Register Command Discovery and Operator Help

As a Lens maintainer,
I want `lens-quickdev` to be discoverable and clearly documented as dev-ready only,
So that users can find the command and understand its lifecycle boundary before running it.

**Acceptance Criteria:**

**Given** the retained command discovery surfaces are reviewed
**When** `lens-quickdev` is registered
**Then** `module.yaml` and `module-help.csv` expose the command exactly once
**And** no duplicate discovery entry is introduced.

**Given** command help and feature-associated docs are updated
**When** a user reads the operator guidance
**Then** the guidance states that `lens-quickdev` is allowed only for dev-ready features
**And** it describes the wrapper as governed implementation, not a planning shortcut.

**Given** broader non-source discovery or packaging changes are discovered
**When** they would expand scope beyond feature-associated control-repo docs
**Then** the implementation records a scope-creep warning
**And** no broader edits happen unless an override is explicitly documented.

### Story 1.3: Implement Feature Resolution and Dev-Ready Gate

As a Lens maintainer,
I want the wrapper to resolve feature context, docs paths, and target repo metadata before delegation,
So that quickdev runs can fail safely when the lifecycle or metadata preconditions are not met.

**Acceptance Criteria:**

**Given** a user runs `lens-quickdev`
**When** no explicit `--feature-id` is supplied
**Then** the wrapper resolves the active feature from Lens feature context
**And** it supports an explicit feature override when one is provided.

**Given** the resolved feature is not dev-ready
**When** the gate check runs
**Then** the wrapper blocks before target-repo assessment
**And** it returns an actionable dev-ready blocker.

**Given** `feature.yaml.target_repos` is empty or unresolved
**When** the wrapper tries to prepare implementation
**Then** it blocks without guessing a write target
**And** it instructs operators to repair feature metadata through the sanctioned FinalizePlan path.

### Story 1.4: Create Versioned Quickdev Evidence Scaffold

As a Lens maintainer,
I want every quickdev run to start with a versioned evidence artifact and codebase assessment,
So that the implementation plan, assumptions, and validation path are captured before any code changes occur.

**Acceptance Criteria:**

**Given** a quickdev request is accepted
**When** the wrapper prepares the run
**Then** it creates `quickdev/quickdev-[summaryofrequeststub]-vNNN.md`
**And** the artifact version increments on reruns instead of overwriting prior artifacts.

**Given** the evidence file is created
**When** the pre-implementation assessment completes
**Then** the file records request, branch state, dirty-worktree outcome, relevant surfaces, assumptions, validation plan, and implementation plan
**And** the assessment is complete before delegation starts.

**Given** the same ask is run again
**When** a prior quickdev artifact already exists
**Then** the wrapper chooses the next available `vNNN` suffix
**And** earlier artifacts remain unchanged for auditability.

## Epic 2: Scoped Implementation Execution and Branch Control

Deliver the governed execution path. This epic delegates the actual implementation to the existing quick-dev engine, applies the agreed branch policy, captures validation and commit outcomes, and protects the bug-specific quickdev path from regression.

### Story 2.1: Delegate Implementation Through bmad-quick-dev

As a Lens maintainer,
I want `lens-quickdev` to delegate code changes through the existing `bmad-quick-dev` engine,
So that the wrapper remains conductor-only and all implementation behavior stays in one place.

**Acceptance Criteria:**

**Given** the wrapper reaches the implementation phase
**When** the delegate path is resolved
**Then** it invokes the sanctioned quick-dev engine through Lens context
**And** it does not create a second implementation workflow.

**Given** a dedicated script facade is absent
**When** the wrapper still needs to hand off
**Then** it loads the registered `bmad-quick-dev` skill directly
**And** it forwards the same Lens context and write scope.

**Given** the implementation target is inspected after delegation
**When** code changes are made
**Then** those changes occur only in the resolved target repo
**And** no governance or release-clone code paths are edited directly.

### Story 2.2: Apply Branch Policy and PR Orchestration

As a Lens maintainer,
I want branch and PR behavior to follow the agreed policy automatically,
So that quickdev runs end up on the right branch without ad hoc git decisions.

**Acceptance Criteria:**

**Given** the target repo is already on an active in-progress feature branch
**When** the wrapper prepares to commit
**Then** it commits directly to that branch
**And** it does not create a new PR branch unless one already exists.

**Given** no active in-progress feature branch exists
**When** the wrapper needs a writable branch
**Then** it prepares a working branch through Lens git orchestration
**And** it pushes or verifies a PR through the sanctioned PR helper.

**Given** branch state is ambiguous or dirty due to unrelated work
**When** the branch policy check runs
**Then** the wrapper blocks before implementation
**And** it records the blocker in the quickdev artifact.

### Story 2.3: Record Validation, Commit, and No-Op Outcomes

As a Lens maintainer,
I want validation and commit outcomes written back into the versioned quickdev artifact,
So that each run ends with a durable, inspectable implementation record.

**Acceptance Criteria:**

**Given** focused validation has been selected during assessment
**When** the implementation run completes
**Then** the wrapper records the validation command and result in the versioned quickdev artifact
**And** it updates branch, commit, changed-files, and PR fields as applicable.

**Given** implementation changes are produced
**When** the wrapper commits them
**Then** it creates a conventional commit
**And** it records the commit hash in the versioned quickdev artifact.

**Given** the run produces no code changes
**When** the wrapper completes
**Then** the quickdev artifact records a `no-op` outcome
**And** no empty commit is created.

### Story 2.4: Handle Validation-Failure Branches and Preserve Bug Quickdev

As a Lens maintainer,
I want failure handling and regression coverage to match the approved branch-by-timing contract,
So that quickdev failures stay recoverable and the bug-specific quickdev route remains intact.

**Acceptance Criteria:**

**Given** validation fails before a commit is created
**When** the wrapper stops
**Then** no commit is created
**And** the versioned quickdev artifact is marked `blocked` with the failing validation summary.

**Given** validation fails after a local commit but before push or PR creation
**When** the wrapper stops
**Then** the commit remains local
**And** the wrapper does not push or open a PR and records a `validation-failed` outcome with fix-forward or revert guidance.

**Given** validation fails after push or PR creation
**When** the wrapper recovers
**Then** it does not rewrite shared history
**And** it records either a fix-forward recovery or blocked PR state while preserving `/lens-bug-quickdev` behavior through explicit non-regression coverage.

## Epic 3: Audit Trail, Publication, and Safe Surface Expansion

Deliver the audit and handoff obligations. This epic publishes the exact quickdev evidence file to governance, reconciles FinalizePlan-owned metadata, and protects the feature from silent scope expansion into broader control-repo or packaging work.

### Story 3.1: Publish Versioned Quickdev Evidence to Governance

As a Lens maintainer,
I want the exact versioned quickdev artifact published to governance after each completed run,
So that the implementation record survives beyond the local feature branch.

**Acceptance Criteria:**

**Given** a quickdev artifact is finalized locally
**When** governance publication runs
**Then** the exact versioned file is copied to `feature.yaml.docs.governance_docs_path/quickdev/`
**And** the artifact records governance publication success.

**Given** governance publication is attempted
**When** the publication path is reviewed
**Then** it uses the sanctioned Lens publication path
**And** it does not hand-author governance files.

**Given** multiple versioned quickdev artifacts exist
**When** publication runs again
**Then** each published artifact preserves its original version suffix
**And** prior governance artifacts are not overwritten.

### Story 3.2: Reconcile Target Repos and Dev Handoff Metadata

As a Lens governance operator,
I want FinalizePlan to register and validate the feature's implementation target metadata,
So that `/dev` can resolve `lens.core.src` without reopening planning questions.

**Acceptance Criteria:**

**Given** FinalizePlan Step 3 is performing metadata reconciliation
**When** it updates `feature.yaml`
**Then** `target_repos` includes `lens.core.src`
**And** the update is made through the sanctioned `feature-yaml` helper.

**Given** the downstream bundle is reviewed for dev readiness
**When** implementation-readiness is generated
**Then** it explicitly carries the versioned quickdev artifact rule and sanctioned governance publication path
**And** it confirms the metadata needed for `/dev` is present.

**Given** strict handoff validation runs
**When** the bundle is checked
**Then** the docs, sprint tracking, and metadata are internally consistent
**And** the first ready-for-dev story aligns with `sprint-status.yaml`.

### Story 3.3: Guard Scope Expansion and Final Audit Readiness

As a Lens maintainer,
I want broader non-source work to be explicitly gated and documented,
So that the feature can expand safely when needed without silently swallowing scope creep.

**Acceptance Criteria:**

**Given** implementation reveals a required change outside feature-associated control-repo docs
**When** that work would materially expand scope
**Then** the wrapper warns the user before editing those broader surfaces
**And** any approved override is recorded in the feature docs.

**Given** the feature docs are reviewed at handoff time
**When** audit readiness is checked
**Then** command-surface scope, versioned evidence rules, and publication boundaries are all documented in one place
**And** remaining deferred risks are explicit rather than implicit.

**Given** the final downstream bundle is inspected
**When** operators prepare to enter `/dev`
**Then** the story and sprint artifacts provide an ordered, reviewable starting point
**And** the handoff no longer depends on undocumented assumptions.