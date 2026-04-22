---
feature: lens-dev-new-codebase-baseline
doc_type: epics
status: in-review
goal: "Regenerate the epic and story breakdown for the lens-work rewrite from the current PRD, architecture, and FinalizePlan review."
stepsCompleted: [step-01-validate-prerequisites, step-02-design-epics, step-03-create-stories]
inputDocuments:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/prd.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/architecture.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/finalizeplan-review.md
key_decisions:
  - "Use the PRD and architecture as the authoritative planning inputs for regeneration."
  - "Carry FinalizePlan review findings into Story 5.5 instead of leaving release-delivery and upgrade communication unowned."
  - "Preserve the 21-story inventory and stable story numbering so downstream story-file paths remain addressable."
open_questions:
  - "Should the final release-delivery note for Story 5.5 live inside the upgrade artifact or in a companion release-ops note?"
depends_on: [prd.md, architecture.md, finalizeplan-review.md]
blocks: []
updated_at: 2026-04-22T23:59:00Z
---

# lens-work Rewrite: 17-Command Stable Surface - Epic Breakdown

## Overview

This document regenerates the backlog for `lens-dev-new-codebase-baseline` from the current planning source of truth: the rewrite PRD, the architecture, and the FinalizePlan review. It replaces the stale backlog-reset framing with a fresh 5-epic, 21-story plan that preserves the architecture's work-package structure, keeps stable story numbering, and explicitly assigns the release-delivery and upgrade-communication work that FinalizePlan called out.

The epic structure is user-value-first for a developer workflow product: it starts by making the published command surface trustworthy, then restores safe feature creation and navigation, then fixes governance resolution, then rebuilds planning conductors, and finally closes the loop with governed delivery, closure, inventory sync, and release compatibility.

## Requirements Inventory

### Functional Requirements

FR1: Users can access the retained 17-command published workflow surface through supported Lens entry points.
FR2: Users can validate workspace readiness before starting or resuming workflow activity.
FR3: Users can create governance domains from the public workflow surface.
FR4: Users can create services within existing governance domains.
FR5: Users can create new features with canonical lifecycle identity and working context.
FR6: Users can switch active feature context without unintended lifecycle mutation.
FR7: Users can request the single next unblocked action for the active feature.
FR8: Users can resolve applicable constitutional guidance for their current operating context.
FR9: Users can create preplan artifacts for active features.
FR10: Users can create businessplan artifacts for active features.
FR11: Users can create techplan artifacts for active features.
FR12: Users can consolidate planning outputs and advance planning completion through finalizeplan.
FR13: Users can use the express planning path for eligible features.
FR14: Users can execute governed development work in target repositories through the `dev` conductor after planning completion.
FR15: Users can complete and archive a feature with retrospective and final documentation.
FR16: Users can split an existing feature into a new first-class feature while preserving governance integrity and eligible work movement.
FR17: Existing users can continue active features after the rewrite without schema migration.
FR18: Existing users can resume disrupted workflow state without recreating features or rewriting valid state.
FR19: Existing users can preserve feature identity, branch topology, and governance path conventions through the rewrite.
FR20: Existing users can preserve in-progress development session continuity through the rewrite.
FR21: Maintainers can release the rewrite as a v4 drop-in for current Lens features.
FR22: Users can invoke upgrade behavior that preserves no-op compatibility for current v4 features while retaining an explicit migration path for future schema divergence.
FR23: Users can access the retained workflow surface through all supported installation and bootstrap paths.
FR24: Integrators can rely on prompts, help, manifests, installer metadata, and setup surfaces to describe the same retained workflow surface.
FR25: Users can use supported IDE and agent integrations to invoke or discover Lens workflows.
FR26: Maintainers can keep required internal skills, scripts, and adapters available behind retained commands.
FR27: Users can access examples, compatibility guidance, troubleshooting guidance, and migration guidance for the retained workflow surface.
FR28: Integrators can treat all dependencies required by retained commands as supported release obligations.
FR29: Planning-phase users can publish reviewed phase artifacts to governance through the approved publication path.
FR30: Planning-phase users can progress through lifecycle phases without direct governance writes outside approved exceptions.
FR31: Maintainers can synchronize repository inventory between governance and local clones.
FR32: Maintainers can preserve the defined governance exception behavior for direct repository-sync operations.
FR33: Maintainers can promote release artifacts without prompt-surface or dependency-surface drift.
FR34: Users can rely on clear authority boundaries between governance, control-repo planning artifacts, release artifacts, and target-repository code work.
FR35: Maintainers can trace each retained command end to end across prompt, skill, dependencies, outputs, and validation anchors.
FR36: Maintainers can validate retained command behavior through attached regression or parity checks.
FR37: Maintainers can centralize shared lifecycle behavior without changing retained user-facing outcomes.
FR38: Small-team operators can assess release readiness using explicit parity and compatibility evidence.
FR39: Maintainers can distinguish public prompt interfaces from retained internal modules when planning future changes.
FR40: Maintainers can verify that every retained command and required dependency is present before release promotion.

### NonFunctional Requirements

NFR1: The rewrite must ship with 0 broken active features at release.
NFR2: The rewrite must require 0 schema migrations for current v4 Lens features and sessions.
NFR3: All 17 retained published prompts and their required dependencies must be present and operational on day 1.
NFR4: Retained commands must preserve recovery-safe behavior for disrupted but valid workflow state.
NFR5: Retained commands must preserve resumability for in-progress development sessions.
NFR6: Load-bearing retained workflows must remain backed by focused regression or parity validation before release promotion.
NFR7: Release promotion must be blocked if prompt-surface parity or dependency-surface parity is incomplete.
NFR8: The rewrite must remain a v4 drop-in for existing Lens users.
NFR9: Existing lifecycle, governance, and session artifacts must remain readable without conversion.
NFR10: Frozen workflow contracts must remain intact across identity, branch topology, governance publication boundaries, handoff behavior, and retained special-case workflows.
NFR11: Deprecated public stubs may be removed only when their retained dependencies and supported replacement surface remain coherent.
NFR12: Prompts, installer behavior, setup surfaces, help output, manifests, adapters, and validation inventory must resolve to the same retained command surface.
NFR13: Supported IDE and agent integration surfaces must remain discoverable and operational where they are part of the shipped workflow.
NFR14: Release artifacts must be promotable without divergence between public prompt contracts and dependency availability.
NFR15: Each retained command must remain traceable end to end across prompt, skill, dependency chain, outputs, and validation anchors.
NFR16: Shared lifecycle behavior may be consolidated only when parity remains demonstrable.
NFR17: Small-team operators must be able to assess release readiness using explicit parity and compatibility evidence rather than manual intuition alone.

### Additional Requirements

- Preserve the 3-hop command-resolution chain: public prompt stub -> release prompt -> owning `bmad-lens-*` skill -> scripts or delegated sub-skills.
- Keep `light-preflight.py` as the prompt-start gate for every retained command, with no change to its exit-code contract.
- Consolidate review-ready artifact validation into `validate-phase-artifacts.py`; no planning conductor may reimplement that logic inline.
- Consolidate batch behavior into `bmad-lens-batch`; planning conductors must use the shared 2-pass contract instead of local intake flows.
- Consolidate publish-before-author behavior into `publish-to-governance`; planning conductors may not write governance files directly.
- Freeze schema v4 contracts for `feature.yaml`, `feature-index.yaml`, `dev-session.yaml`, lifecycle topology, governance path conventions, and the canonical feature identity formula.
- Treat `split-feature` as a retained published command with its dedicated `validate-split`, `create-split-feature`, and `move-stories` script surface intact.
- Preserve `discover` as the only retained command with an explicit governance-main auto-commit exception outside the normal publish-before-author path.
- Rewrite `constitution` so partial hierarchies resolve additively and read-only even when org-level files are absent.
- Enforce the architecture prerequisite that the `constitution` bug fix lands before any planning-conductor rewrite begins.
- Preserve `dev` target-repo-only writes, per-task commits, resumable `dev-session.yaml` checkpoints, and the single final PR model.
- Preserve `upgrade` as a v4-to-v4 no-op for current features, while keeping the explicit migration router for future schema changes.
- Treat the 37 removed public stubs as a hard cut at release time: no redirect shim, explicit upgrade communication required.
- Assign the release-delivery note, removed-command communication, and final release-readiness evidence to Story 5.5 so they are not left as advisory carry-forward work.

### UX Design Requirements

No standalone UX specification was used for this regeneration. The feature is on the `tech-change` track, so UX requirements are limited to command-surface legibility, predictable routing, clear failure messages, and stable handoff behavior already captured in the PRD and architecture.

### FR Coverage Map

FR1: Epic 1 - Stable published surface and discovery parity.
FR2: Epic 2 - Preflight entry and workspace validation.
FR3: Epic 2 - Domain creation.
FR4: Epic 2 - Service creation.
FR5: Epic 2 - Canonical feature creation.
FR6: Epic 2 - Read-only feature switching.
FR7: Epic 2 - Next-action routing.
FR8: Epic 3 - Constitution resolution.
FR9: Epic 4 - Preplan conductor rewrite.
FR10: Epic 4 - Businessplan conductor rewrite.
FR11: Epic 4 - Techplan conductor rewrite.
FR12: Epic 4 - FinalizePlan conductor rewrite.
FR13: Epic 4 - ExpressPlan rewrite.
FR14: Epic 5 - Dev conductor rewrite.
FR15: Epic 5 - Complete conductor rewrite.
FR16: Epic 5 - Split-feature rewrite.
FR17: Epic 2 - Backwards-compatible feature creation and navigation.
FR18: Epic 2 - Recovery-safe command routing and resumed state handling.
FR19: Epic 2 - Identity and branch-topology preservation.
FR20: Epic 5 - Dev session continuity.
FR21: Epic 5 - Release as a v4 drop-in.
FR22: Epic 5 - Upgrade no-op and migration routing.
FR23: Epic 1 - Install-surface parity.
FR24: Epic 1 - Prompt/help/manifest alignment.
FR25: Epic 1 - IDE and agent integration parity.
FR26: Epic 1 - Retained internal dependency inventory.
FR27: Epic 5 - Compatibility and upgrade guidance.
FR28: Epic 1 - Retained dependencies treated as release obligations.
FR29: Epic 1 - Publish reviewed planning artifacts through the approved path.
FR30: Epic 1 - No direct governance writes outside approved exceptions.
FR31: Epic 5 - Repo-inventory synchronization.
FR32: Epic 5 - Discover exception behavior.
FR33: Epic 5 - Release promotion without surface drift.
FR34: Epic 1 - Clear authority boundaries.
FR35: Epic 1 - End-to-end command traceability.
FR36: Epic 1 - Validation anchors for retained behavior.
FR37: Epic 1 - Shared-lifecycle consolidation with parity.
FR38: Epic 5 - Explicit release-readiness evidence.
FR39: Epic 1 - Public versus internal command distinction.
FR40: Epic 5 - Final retained-command and dependency verification.

## Epic List

### Epic 1: Stable Published Surface and Shared Lifecycle Contracts

Rebuild the published command surface, discovery surfaces, and shared lifecycle primitives so maintainers and users can trust that every retained command resolves through one coherent, testable contract.

**FRs covered:** FR1, FR23, FR24, FR25, FR26, FR28, FR29, FR30, FR34, FR35, FR36, FR37, FR39, FR40

### Epic 2: Safe Feature Creation and Navigation

Restore the workflow entry, identity, and navigation commands so users can validate a workspace, create governed structures, establish canonical features, and move through feature context without corrupting lifecycle state.

**FRs covered:** FR2, FR3, FR4, FR5, FR6, FR7, FR17, FR18, FR19

### Epic 3: Reliable Constitution Resolution

Fix governance resolution so missing hierarchy levels do not block valid work and every downstream planning or execution command can rely on additive, read-only constitution behavior.

**FRs covered:** FR8, FR17, FR18, FR34, FR36

### Epic 4: Reviewed Planning Handoff

Rebuild the planning conductors so users can move from preplan through FinalizePlan with shared validation, shared batch handling, publish-before-author governance, and the correct review and PR sequence.

**FRs covered:** FR9, FR10, FR11, FR12, FR13, FR29, FR30, FR35, FR36, FR37

### Epic 5: Governed Delivery, Closure, and Release Compatibility

Restore execution, closure, feature splitting, inventory synchronization, and upgrade behavior so the rewrite can ship as a trustworthy v4 drop-in with explicit release-readiness evidence.

**FRs covered:** FR14, FR15, FR16, FR20, FR21, FR22, FR27, FR31, FR32, FR33, FR38, FR40

## Epic 1: Stable Published Surface and Shared Lifecycle Contracts

This epic establishes the stable runtime foundation for the rewrite. It makes the public surface auditable, removes stale stub drift, and centralizes the three lifecycle contracts that planning conductors depend on: review-ready validation, batch intake, and publish-before-author governance.

### Story 1.1: Stable Published Surface and Discovery Parity

As a lens-work maintainer,
I want the published prompt surface, help surfaces, and adapter metadata rebuilt around the retained 17 commands,
So that installation and discovery stay coherent for every supported entry point.

**Acceptance Criteria:**

**Given** the rewrite source tree is installed
**When** the public prompt directory, module help, shell menu, setup surfaces, and manifests are compared
**Then** exactly 17 retained commands are present everywhere
**And** `split-feature` remains on every published surface.

**Given** removed public stubs are deleted
**When** the internal skill inventory is compared with the architecture retention matrix
**Then** every required retained dependency remains available behind the kept commands
**And** no removed public stub is still advertised.

**Given** supported IDE and agent adapters are generated
**When** users discover Lens commands through those adapters
**Then** the same 17-command set is exposed
**And** command aliases resolve to the same public names as the prompt stubs.

### Story 1.2: Shared Review-Ready Artifact Validation

As a planning-phase maintainer,
I want a single review-ready artifact validator,
So that every planning conductor enforces the same lifecycle gate without copy-pasted logic.

**Acceptance Criteria:**

**Given** a feature has the required reviewed artifacts for its phase and track
**When** `validate-phase-artifacts.py` runs in review-ready mode
**Then** it exits successfully with machine-readable confirmation
**And** the caller can treat the phase gate as satisfied.

**Given** required artifacts are missing or not review-ready
**When** the validator runs
**Then** it exits non-zero
**And** the output identifies the missing or invalid artifacts clearly enough for the caller to stop.

**Given** `preplan`, `businessplan`, `techplan`, `finalizeplan`, or `expressplan` need a review-ready check
**When** their gate logic executes
**Then** they delegate to the shared validator
**And** none of those conductors reimplement the contract inline.

### Story 1.3: Shared Two-Pass Batch Contract

As a planning-phase maintainer,
I want a single two-pass batch intake contract,
So that batch mode behaves the same way across all planning conductors.

**Acceptance Criteria:**

**Given** a planning conductor runs in batch mode without resume context
**When** `bmad-lens-batch` pass 1 executes
**Then** it writes the correct `{phase}-batch-input.md` artifact and stops
**And** it does not start downstream authoring.

**Given** a planning conductor runs in batch mode with approved resume context
**When** `bmad-lens-batch` pass 2 executes
**Then** it forwards the batch context to downstream skills in order
**And** treats the answers as pre-approved offline input.

**Given** any delegated batch target fails
**When** the shared batch contract is executing sequential work
**Then** it halts on the first failure
**And** surfaces which target stopped the batch sequence.

### Story 1.4: Canonical Publish-Before-Author Governance Hook

As a planning-phase maintainer,
I want prior-phase artifact publication routed through one hook,
So that governance mirroring happens consistently without direct writes from planning conductors.

**Acceptance Criteria:**

**Given** `businessplan`, `techplan`, `finalizeplan`, or `dev` starts with reviewed predecessor artifacts available
**When** the phase entry logic runs
**Then** it calls `publish-to-governance` for the prior phase before local authoring starts
**And** the governance mirror becomes the published snapshot for cross-feature consumers.

**Given** nothing new needs to be published
**When** the hook executes
**Then** it returns a safe no-op result
**And** the caller continues without manufacturing governance churn.

**Given** planning conductors are audited for governance writes
**When** their mutation paths are inspected
**Then** publish-before-author is the only planning-phase governance write path
**And** `discover` remains the sole explicit exception.

## Epic 2: Safe Feature Creation and Navigation

This epic restores the commands that let a user enter, set up, and navigate the workflow. It preserves identity invariants, two-branch topology, and read-only navigation behavior so the rest of the rewrite has a safe operational base.

### Story 2.1: Rewrite Preflight Entry and Workspace Validation

As a Lens user,
I want every retained command to start with the same preflight discipline,
So that misconfigured workspaces fail before any lifecycle or governance state can change.

**Acceptance Criteria:**

**Given** a retained public command is invoked
**When** the prompt stub starts execution
**Then** `light-preflight.py` runs before the redirect to the release prompt
**And** its exit-code contract remains unchanged.

**Given** the workspace, governance repo, and module install are healthy
**When** preflight runs
**Then** it reports readiness without mutating feature lifecycle state
**And** the command can continue into its owning skill.

**Given** required repos, config, or tooling are missing
**When** preflight runs
**Then** it fails fast with a clear reason
**And** no feature, governance, or session files are modified.

### Story 2.2: Rewrite New Domain

As a Lens user,
I want to create a new governance domain through the retained surface,
So that domain-level constitutions and markers are established under stable naming rules.

**Acceptance Criteria:**

**Given** a valid new domain slug is supplied
**When** `new-domain` runs
**Then** the governance markers and constitution scaffold are created in the correct domain path
**And** the path naming matches the retained conventions.

**Given** a domain already exists
**When** `new-domain` is invoked for the same identifier
**Then** the command fails safely
**And** it does not overwrite existing domain artifacts.

**Given** a new domain is created successfully
**When** downstream service or feature setup relies on it
**Then** the domain is discoverable through the same governance structure as before the rewrite
**And** inherited constitution behavior remains available.

### Story 2.3: Rewrite New Service

As a Lens user,
I want to create a service inside an existing domain,
So that service-level governance artifacts inherit the right domain context without manual setup.

**Acceptance Criteria:**

**Given** the parent domain exists
**When** `new-service` runs with a valid service slug
**Then** service governance markers are created in the expected location
**And** the service inherits domain-level constitutional context.

**Given** the parent domain does not exist
**When** `new-service` runs
**Then** it fails with a clear domain-missing message
**And** no partial service artifacts are written.

**Given** a service already exists
**When** the same service is requested again
**Then** the command preserves existing artifacts
**And** duplicate creation does not overwrite service state.

### Story 2.4: Rewrite New Feature

As a Lens user,
I want feature creation to preserve canonical identity and branch topology,
So that every new initiative starts with a correct governance record and working branch model.

**Acceptance Criteria:**

**Given** a valid domain, service, and feature slug are supplied
**When** `new-feature` runs
**Then** `feature.yaml` records the canonical `featureId` formula `{domain}-{service}-{featureSlug}`
**And** `featureSlug` remains separately persisted.

**Given** a new feature is created successfully
**When** control-repo branches are prepared
**Then** exactly `{featureId}` and `{featureId}-plan` are created
**And** the feature-index entry points at the correct plan branch.

**Given** target-repo context is needed for later `/dev` work
**When** feature creation completes
**Then** the feature keeps its target-repo metadata and branch mode intact
**And** no schema changes are introduced to support that handoff.

### Story 2.5: Rewrite Switch

As a Lens user,
I want to change active feature context without changing lifecycle state,
So that navigation remains safe and reversible.

**Acceptance Criteria:**

**Given** a valid feature is selected
**When** `switch` runs
**Then** the active session context changes to that feature
**And** no governance or lifecycle files are mutated.

**Given** an invalid or archived target is selected
**When** `switch` runs
**Then** it surfaces the selection problem clearly
**And** the current active feature context remains unchanged.

**Given** `switch` completes successfully
**When** downstream commands read feature context
**Then** they resolve the selected feature's docs, governance path, and metadata consistently
**And** no hidden side effects appear in git state.

### Story 2.6: Rewrite Next

As a Lens user,
I want one clear next action with pre-confirmed handoff,
So that the workflow advances without redundant prompts or ambiguous branching.

**Acceptance Criteria:**

**Given** an active feature has exactly one unblocked next lifecycle action
**When** `next` runs
**Then** it recommends that single action
**And** the rationale reflects blockers, phase, and track state.

**Given** `next` delegates to a downstream phase skill
**When** the handoff occurs
**Then** the delegated skill does not re-ask for launch confirmation
**And** the pre-confirmed contract remains intact.

**Given** blockers prevent forward movement
**When** `next` runs
**Then** it stops on the blocking condition
**And** it reports the blocker before suggesting any action.

## Epic 3: Reliable Constitution Resolution

This epic isolates the governance bug fix that the architecture made a prerequisite for planning-conductor work. It exists to keep downstream planning and execution from inheriting undefined behavior whenever part of the constitution hierarchy is absent.

### Story 3.1: Fix Partial-Hierarchy Constitution Resolution

As a Lens user working in environments with partial constitution hierarchies,
I want constitutional guidance to resolve additively from whatever levels exist,
So that valid work does not fail just because an org-level file is absent.

**Acceptance Criteria:**

**Given** only repo-, service-, or domain-level constitutions exist
**When** `constitution` resolves guidance
**Then** it returns additive guidance from the available hierarchy levels
**And** it does not hard-fail because the org level is missing.

**Given** a full hierarchy exists
**When** constitution resolution runs
**Then** the merge order remains deterministic and additive
**And** previously correct full-hierarchy behavior is preserved.

**Given** any hierarchy combination is resolved
**When** the command completes
**Then** the operation remains read-only
**And** no constitutions, feature records, or governance metadata are modified during lookup.

## Epic 4: Reviewed Planning Handoff

This epic rebuilds the five planning conductors on top of the shared contracts from Epic 1 and the governance fix from Epic 3. The goal is a planning sequence that writes the right artifacts in the right order, mirrors reviewed outputs before downstream authoring, and preserves the existing PR topology.

### Story 4.1: Rewrite Preplan

As a Lens user,
I want preplan to preserve brainstorm-first discovery and shared gating behavior,
So that early planning remains structured, resumable, and reviewable.

**Acceptance Criteria:**

**Given** the constitution prerequisite is satisfied
**When** `preplan` runs in interactive mode
**Then** it stages brainstorm work before research or product brief generation
**And** the artifact sequence matches the retained planning contract.

**Given** `preplan` runs in batch mode
**When** it needs offline intake or resume behavior
**Then** it delegates to `bmad-lens-batch`
**And** it does not reimplement batch flow locally.

**Given** review-ready checking or final review is needed
**When** `preplan` evaluates readiness
**Then** it uses the shared validation and adversarial-review paths
**And** the conductor stays a thin orchestrator over shared primitives.

### Story 4.2: Rewrite Businessplan

As a Lens user,
I want businessplan to publish reviewed predecessor artifacts before authoring new outputs,
So that PRD and any track-sensitive UX work start from the correct published planning snapshot.

**Acceptance Criteria:**

**Given** reviewed preplan artifacts exist
**When** `businessplan` starts
**Then** it publishes the reviewed predecessor set before delegating PRD or UX authoring
**And** governance consumers see the published snapshot rather than local staged-only files.

**Given** a feature track does not require UX output
**When** `businessplan` delegates downstream work
**Then** the command remains track-aware
**And** it does not manufacture a UX artifact for tracks that do not own one.

**Given** governance mutation paths are audited
**When** `businessplan` completes
**Then** no direct governance writes exist outside the publish hook
**And** local planning artifacts remain the control-repo source of truth.

### Story 4.3: Rewrite Techplan

As a Lens user,
I want techplan to publish businessplan outputs and generate architecture against the approved PRD,
So that technical design stays grounded in the current planning contract.

**Acceptance Criteria:**

**Given** businessplan artifacts are reviewed
**When** `techplan` starts
**Then** it publishes the reviewed predecessor set before architecture generation
**And** the same governance boundary rules as businessplan are preserved.

**Given** architecture generation runs
**When** the output is written
**Then** the artifact explicitly references the authoritative PRD
**And** the retained `must_reference` behavior stays intact.

**Given** techplan reaches review or completion checks
**When** its validation flow runs
**Then** it delegates to shared validation and review infrastructure
**And** the conductor does not fork those contracts locally.

### Story 4.4: Rewrite FinalizePlan

As a Lens user,
I want FinalizePlan to preserve strict review, PR, and bundle ordering,
So that planning closes with reviewed artifacts, the correct branch topology, and a clean handoff into `/dev`.

**Acceptance Criteria:**

**Given** reviewed techplan artifacts exist on `{featureId}-plan`
**When** FinalizePlan step 1 executes
**Then** adversarial review outputs are written, committed, and pushed before PR-readiness work starts
**And** the pushed review commit is reported.

**Given** step 2 evaluates merge readiness
**When** the planning PR path is created or validated
**Then** the PR targets `{featureId}` from `{featureId}-plan`
**And** blockers or branch-protection failures stop progression cleanly.

**Given** the planning PR has merged into `{featureId}`
**When** step 3 runs
**Then** the downstream planning bundle generates `epics.md`, `stories.md`, `implementation-readiness.md`, `sprint-status.yaml`, and story files before the final `{featureId}` -> `main` PR is opened
**And** the feature phase can advance without skipping the bundle.

### Story 4.5: Rewrite ExpressPlan

As a Lens user on an express-eligible feature,
I want a compressed planning path that still enforces the hard review stop,
So that express features move quickly without bypassing governance or quality gates.

**Acceptance Criteria:**

**Given** a feature is not express-eligible
**When** `expressplan` is invoked
**Then** the command blocks entry
**And** it explains why the feature must use the standard planning path instead.

**Given** an express-eligible feature starts express planning
**When** the conductor runs
**Then** it delegates the compressed planning sequence through `bmad-lens-quickplan`
**And** QuickPlan remains an internal retained dependency even though its public stub is removed.

**Given** the adversarial review gate fails during express planning
**When** the command reaches the hard-stop review point
**Then** express planning halts before FinalizePlan-style bundling continues
**And** the failure is surfaced as a blocker, not downgraded to advisory output.

## Epic 5: Governed Delivery, Closure, and Release Compatibility

This epic closes the loop from implementation to release. It preserves target-repo execution discipline, feature completion semantics, split-feature behavior, discover's special sync path, and the final upgrade and release-readiness gate that makes the rewrite safe to ship.

### Story 5.1: Rewrite Dev

As a Lens user ready to implement approved work,
I want `/dev` to preserve target-repo-only execution and resumable checkpoints,
So that implementation stays governed without corrupting planning or governance state.

**Acceptance Criteria:**

**Given** planning is complete and target-repo metadata is available
**When** `/dev` prepares work
**Then** it creates or selects the correct target-repo development branch
**And** code writes remain confined to the target repo rather than the control or governance repos.

**Given** implementation work proceeds through dev tasks
**When** checkpoints are recorded
**Then** `dev-session.yaml` preserves resumable state without schema changes
**And** per-task commit behavior remains intact.

**Given** dev reaches review completion
**When** the workflow closes the implementation slice
**Then** it preserves the single final PR model
**And** the control-repo planning artifacts stay unchanged except for governed phase updates.

### Story 5.2: Rewrite Complete

As a Lens user finishing a feature,
I want completion to preserve retrospective, documentation, and archive ordering,
So that finished work lands in a clean terminal state with the right closeout record.

**Acceptance Criteria:**

**Given** a feature is ready for closeout
**When** `complete` runs
**Then** retrospective work happens before archival mutation
**And** closeout documentation is written before the terminal archive step.

**Given** completion reaches the archive action
**When** the feature is transitioned
**Then** the archive move is atomic
**And** terminal-state semantics match the retained lifecycle rules.

**Given** a completed feature is later inspected
**When** governance and docs are read
**Then** the final retrospective and documentation outputs remain discoverable
**And** archived state is unambiguous.

### Story 5.3: Rewrite Split-Feature

As a Lens user reshaping a large initiative,
I want split-feature to validate before mutating anything,
So that new features are created safely and only eligible work moves.

**Acceptance Criteria:**

**Given** a source feature has in-progress or otherwise ineligible stories
**When** `split-feature` validation runs
**Then** the split is blocked before mutation
**And** the reason is surfaced explicitly.

**Given** a split is valid
**When** execution begins
**Then** the new governance feature, summary stub, and feature metadata are created before any optional story movement occurs
**And** the new feature becomes a first-class governance citizen immediately.

**Given** story movement is requested
**When** `move-stories` executes
**Then** only eligible stories are moved
**And** the retained `validate-split`, `create-split-feature`, and `move-stories` command surface remains intact.

### Story 5.4: Rewrite Discover

As a Lens maintainer,
I want inventory synchronization to remain explicit and trustworthy,
So that governance inventory and local clones stay aligned without blurring write-boundary rules.

**Acceptance Criteria:**

**Given** governance inventory and local clone state differ
**When** `discover` runs
**Then** repo-inventory data is synchronized bidirectionally according to the retained rules
**And** missing or stale local clone information is reflected accurately.

**Given** `discover` needs to write inventory updates
**When** it performs that work
**Then** the command preserves its explicit governance-main auto-commit exception
**And** that exception does not leak into other planning or execution commands.

**Given** operators inspect inventory after synchronization
**When** they compare governance and local state
**Then** repo provenance is clearer, not less clear
**And** the command remains distinguishable from publish-to-governance behavior.

### Story 5.5: Rewrite Upgrade and Release-Readiness Gate

As a Lens maintainer,
I want upgrade behavior and release-readiness evidence to be explicit,
So that the rewrite ships as a trustworthy v4 drop-in instead of a best-effort assumption.

**Acceptance Criteria:**

**Given** current v4 features and sessions are already on the active lifecycle schema
**When** `upgrade` runs
**Then** the command reports a no-op outcome for v4-to-v4 cases
**And** it does not mutate feature, governance, or session data.

**Given** a future schema divergence exists
**When** `upgrade` detects that migration is required
**Then** it routes explicitly into the migration engine instead of applying silent inline changes
**And** the migration path remains auditable.

**Given** the rewrite is being prepared for release
**When** Story 5.5 completes
**Then** the release-delivery mechanism, removed-stub communication, and upgrade notes for the 37 hard-cut commands are written as explicit deliverables
**And** the FinalizePlan advisory findings about release ownership are no longer unowned.

**Given** release readiness is reviewed
**When** maintainers assemble the final evidence set
**Then** all 17 retained commands and their required dependencies have explicit validation anchors
**And** at least one integration-level check confirms shared state remains coherent across command families rather than only within isolated work packages.
