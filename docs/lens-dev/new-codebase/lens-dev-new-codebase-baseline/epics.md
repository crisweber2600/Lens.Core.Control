---
feature: lens-dev-new-codebase-baseline
doc_type: epics
status: complete
stepsCompleted: [step-01-validate-prerequisites, step-02-design-epics, step-03-create-stories]
inputDocuments:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/prd.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/architecture.md
updated_at: 2026-04-23T00:00:00Z
---

# lens-work Rewrite: 17-Command Stable Surface — Epic Breakdown

## Overview

This document provides the complete epic and story breakdown for the lens-work rewrite (feature: `lens-dev-new-codebase-baseline`), decomposing the requirements from the PRD and Architecture into implementable stories organized around the 17 work packages (WP-01 through WP-17).

**Key architectural constraint:** Epic 3 (WP-15, constitution bug fix) is a hard prerequisite for Epic 4 (WP-07–WP-11, planning conductors). Epic 4 stories must not begin until all Epic 3 stories are `done`.

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

### Non-Functional Requirements

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

- The rewrite must preserve the 17-command published surface exactly, including `split-feature`.
- `light-preflight.py` remains the prompt-start gate for every retained published command.
- The canonical feature identity formula remains `{domain}-{service}-{featureSlug}`.
- The control-repo planning topology remains `{featureId}-plan -> {featureId} -> main`.
- `publish-to-governance` remains the only valid planning-phase path for governance artifact publication.
- `next` handoff remains pre-confirmed; delegated phase skills must not re-ask launch confirmation.
- `dev-session.yaml` remains backwards compatible for in-progress dev work.
- `dev` remains a governed execution conductor entered after planning completion, not a planning lifecycle milestone.
- `split-feature` remains validate-first, blocks in-progress stories, and creates the new feature as a first-class governance citizen before modifying the source feature.
- Planning-phase commands must avoid direct governance writes except via `publish-to-governance`; `discover` retains its explicit auto-commit exception.
- All retained commands must continue to resolve: prompt stub → release prompt → owning `bmad-lens-*` skill.

### UX Design Requirements

This product has no rich visual UI. UX requirements are expressed through command-surface legibility, routing clarity, and onboarding behavior. The four user journeys in the PRD (Nina, Evan, Mara, Ian) capture the UX expectations:
- Nina: clear, small command surface, reliable prompt-start, predictable phase routing.
- Evan: zero schema changes, recovery-safe behavior, preserved identity and branch topology.
- Mara: shared utilities, explicit contracts, validation anchors, dogfooding through Lens workflows.
- Ian: exact agreement across prompts, help, manifests, installer, and tests; no silent capability loss.

### FR Coverage Map

| FR | Requirement Summary | Epic(s) | Stories |
|---|---|---|---|
| FR1 | 17-command published surface accessible | 2, 3, 4, 5 | 2.1–2.6, 3.1, 4.1–4.5, 5.1–5.5 |
| FR2 | Workspace validation before workflow | 2 | 2.1 |
| FR3 | Create governance domains | 2 | 2.2 |
| FR4 | Create services under existing domains | 2 | 2.3 |
| FR5 | Create features with canonical identity | 2 | 2.4 |
| FR6 | Switch context without lifecycle mutation | 2 | 2.5 |
| FR7 | Next unblocked action routing | 2 | 2.6 |
| FR8 | Constitutional guidance resolution | 3 | 3.1 |
| FR9 | Preplan artifacts | 4 | 4.1 |
| FR10 | Businessplan artifacts | 4 | 4.2 |
| FR11 | Techplan artifacts | 4 | 4.3 |
| FR12 | Finalizeplan consolidation | 4 | 4.4 |
| FR13 | Express planning path | 4 | 4.5 |
| FR14 | Dev conductor after planning | 5 | 5.1 |
| FR15 | Complete and archive a feature | 5 | 5.2 |
| FR16 | Split-feature governance integrity | 5 | 5.3 |
| FR17 | Existing features unbroken after rewrite | 1, 2, 3, 4, 5 | 1.1, all command stories |
| FR18 | Disrupted workflow state resumable | 2, 4, 5 | 2.4, 4.1–4.5, 5.1 |
| FR19 | Identity, branch topology, governance path preserved | 1, 2 | 1.1, 2.4 |
| FR20 | Dev session continuity preserved | 5 | 5.1 |
| FR21 | v4 drop-in for current Lens features | 5 | 5.5 |
| FR22 | Upgrade no-op for v4; explicit migration path retained | 5 | 5.5 |
| FR23 | Retained surface accessible through all install paths | 1 | 1.1 |
| FR24 | Prompts/help/manifests/installer agree on surface | 1 | 1.1 |
| FR25 | IDE and agent integrations available | 1 | 1.1 |
| FR26 | Internal skills/scripts retained behind commands | 2, 3, 4, 5 | all command stories |
| FR27 | Docs, examples, compatibility guidance accessible | 5 | 5.5 |
| FR28 | All dependencies treated as release obligations | 1, 2, 3, 4, 5 | all stories |
| FR29 | Reviewed phase artifacts published via publish-to-governance | 1 | 1.4 |
| FR30 | No direct governance writes outside approved exceptions | 1, 4 | 1.4, 4.1–4.5 |
| FR31 | Inventory sync between governance and local clones | 5 | 5.4 |
| FR32 | Discover auto-commit exception behavior preserved | 5 | 5.4 |
| FR33 | Release artifacts promotable without surface drift | 5 | 5.5 |
| FR34 | Authority boundaries clear | 1 | 1.1–1.4 |
| FR35 | Command traceability end-to-end | 1, 2, 3, 4, 5 | all stories |
| FR36 | Regression validation attached to retained commands | 2, 3, 4, 5 | all command stories |
| FR37 | Shared utilities centralize lifecycle behavior | 1 | 1.2–1.4 |
| FR38 | Release readiness assessable with explicit evidence | 5 | 5.5 |
| FR39 | Public vs internal distinction explicit | 1 | 1.1 |
| FR40 | All retained commands and deps verifiable before promotion | 5 | 5.5 |

## Epic List

1. **Epic 1: Codebase Foundation and Shared Infrastructure** — Scaffold the new source tree, reduce the published stub surface from 54 to 17, and implement the three shared utilities that all retained commands depend on.
2. **Epic 2: Identity and Navigation Command Surface** — Rewrite the six identity-root and navigation commands: `preflight`, `new-domain`, `new-service`, `new-feature`, `switch`, and `next`.
3. **Epic 3: Constitution Bug Fix and Governance Rules Engine** — Fix the org-level hard-fail bug in `constitution` and add parity regression coverage. **This epic is a prerequisite for Epic 4.**
4. **Epic 4: Planning Conductor Rewrite** — Rewrite the five planning-phase conductors (`preplan`, `businessplan`, `techplan`, `finalizeplan`, `expressplan`) using the shared utilities. **Blocked until Epic 3 is done.**
5. **Epic 5: Execution, Closure, and Maintenance Commands** — Rewrite `dev`, `complete`, `split-feature`, `discover`, and `upgrade`; run the end-to-end regression gate as the final release-readiness proof.

---

## Epic 1: Codebase Foundation and Shared Infrastructure

Establish the new source tree for the lens-work rewrite. This epic delivers the scaffolding that all subsequent epics depend on: a clean codebase layout that maps to the 17-command surface, reduced prompt stub count (54 → 17), module help and shell-menu alignment, and the three shared utilities extracted from duplicated lifecycle behavior (`validate-phase-artifacts.py`, `bmad-lens-batch`, and the `publish-to-governance` entry hook). Retained internal skills inventory is locked here.

This epic does not rewrite any individual published command; it creates the stable base that makes those rewrites safe.

### Story 1.1: New Codebase Scaffold, Install Surfaces, and Module Surface Reduction

As a lens-work maintainer,
I want the new source tree scaffolded with exactly 17 published prompt stubs, aligned module-help.csv, lens.agent.md shell menu, and up-to-date installer and manifest surfaces,
So that every subsequent command rewrite story starts from a consistent, auditable foundation with no legacy stub confusion.

**Acceptance Criteria:**

**Given** the new Lens.Core.Src repository is initialized
**When** setup.py (or equivalent install surface) is run
**Then** exactly 17 `.github/prompts/` stubs are present — no more, no fewer — matching the retained command inventory from the architecture

**Given** the new codebase is installed
**When** module-help.csv is inspected
**Then** it lists exactly the 17 retained commands and no deprecated stubs

**Given** the new codebase is installed
**When** lens.agent.md shell menu section is inspected
**Then** it contains exactly the 17 retained commands in the correct categories

**Given** `setup.py`, module-help.csv, lens.agent.md, and the prompt stubs are all inspected together
**When** their command inventories are compared
**Then** all four surfaces agree on the same 17-command set with no mismatches

**Given** the retained internal skills inventory is locked in the source tree
**When** the list is compared to the architecture's retained-vs-removed table
**Then** all retained internal skills (bmad-lens-onboard, bmad-lens-batch, bmad-lens-bmad-skill, bmad-lens-quickplan, bmad-lens-adversarial-review, bmad-lens-feature-yaml, bmad-lens-git-orchestration, bmad-lens-git-state, bmad-lens-target-repo, bmad-lens-document-project, bmad-lens-retrospective, bmad-lens-migrate) are present and all removed skills are absent

**Given** supported IDE and agent adapters (GitHub Copilot, Cursor, Claude) are regenerated or updated
**When** adapter command inventories are inspected
**Then** all adapters expose exactly the 17 retained commands

### Story 1.2: Implement validate-phase-artifacts.py Shared Utility

As a lens-work maintainer,
I want a single `validate-phase-artifacts.py` script that provides a review-ready fast path for all planning-phase commands,
So that gate-checking logic is not duplicated across preplan, businessplan, techplan, and finalizeplan, and a future fix to this path propagates to all callers without per-command patches.

**Acceptance Criteria:**

**Given** `validate-phase-artifacts.py` is invoked for a feature that has all required phase artifacts present and reviewed
**When** the script runs
**Then** it exits 0 and prints a review-ready confirmation

**Given** `validate-phase-artifacts.py` is invoked for a feature with missing required phase artifacts
**When** the script runs
**Then** it exits non-zero with a clear error identifying the missing artifact(s)

**Given** `validate-phase-artifacts.py` is invoked for a feature with artifacts present but not yet marked reviewed
**When** the script runs
**Then** it exits non-zero and reports the unreviewed artifact(s)

**Given** the shared utility is called from planning-phase commands (preplan, businessplan, techplan, finalizeplan)
**When** each command's gate check runs
**Then** it delegates to `validate-phase-artifacts.py` rather than reimplementing the same logic inline

**Given** a parity test exists for validate-phase-artifacts behavior
**When** the test is run
**Then** it passes and covers happy path, missing artifacts, and unreviewed artifacts cases

### Story 1.3: Implement bmad-lens-batch Shared 2-Pass Contract

As a lens-work maintainer,
I want `bmad-lens-batch` to provide a stable 2-pass batch intake contract (planning artifact collection + sequential BMAD skill invocation) that all planning-phase commands can delegate to,
So that batch mode behavior is consistent and a change to the batch contract only needs to be made in one place.

**Acceptance Criteria:**

**Given** a planning-phase command (preplan, businessplan, techplan, finalizeplan, expressplan) runs in batch mode
**When** the batch pass executes
**Then** it delegates to `bmad-lens-batch` rather than implementing its own intake loop

**Given** `bmad-lens-batch` is invoked with a list of planning artifacts and skill targets
**When** pass 1 (collection) runs
**Then** it gathers the required artifacts in the correct order without mutation

**Given** pass 1 completes
**When** pass 2 (sequential skill invocation) runs
**Then** it invokes each target skill in order and halts on first skill failure with a clear error

**Given** a wrapper-equivalence regression test exists for bmad-lens-batch
**When** the test is run
**Then** it passes and verifies that batch behavior matches the expected 2-pass contract

### Story 1.4: Implement publish-to-governance Entry Hook

As a lens-work maintainer,
I want a single `publish-to-governance` entry hook that planning-phase commands invoke before delegating to BMAD authoring skills,
So that the publish-before-author ordering is enforced consistently and no planning-phase command can accidentally write governance artifacts without going through the approved path.

**Acceptance Criteria:**

**Given** a planning-phase command (businessplan, techplan, finalizeplan) is entered
**When** the entry hook is invoked
**Then** `publish-to-governance` runs before any BMAD authoring skill is delegated to

**Given** `publish-to-governance` is invoked when no reviewed artifacts are pending publication
**When** the hook runs
**Then** it exits gracefully without error or mutation

**Given** `publish-to-governance` is invoked when reviewed planning artifacts are pending
**When** the hook runs
**Then** it publishes exactly those artifacts to governance and logs what was published

**Given** a planning-phase command attempts to write directly to governance outside of `publish-to-governance`
**When** the governance write audit is run
**Then** no direct governance writes are found for planning commands (except `discover`'s explicit auto-commit exception)

**Given** a governance-write audit test exists
**When** the test is run
**Then** it passes, confirming the publish-before-author entry hook is the only planning-phase governance write path

---

## Epic 2: Identity and Navigation Command Surface

Rewrite the six identity-root and navigation commands that form the entry layer of the Lens workflow: `preflight` (WP-01), `new-domain` (WP-02), `new-service` (WP-03), `new-feature` (WP-04), `switch` (WP-05), and `next` (WP-06). These commands are the first thing users interact with and the foundation for every downstream lifecycle operation.

Each story in this epic must preserve the existing behavioral contracts precisely. No user-visible behavior changes are permitted. Each story includes a parity regression anchor from the architecture.

### Story 2.1: Rewrite preflight Command (WP-01)

As a Lens user,
I want `preflight` to validate workspace and onboarding state exactly as it does today,
So that I can trust the prompt-start gate behavior is correct and existing onboarding flows are not disrupted.

**Acceptance Criteria:**

**Given** `preflight` is invoked on a correctly configured workspace
**When** the command runs
**Then** `light-preflight.py` runs as the prompt-start gate before any workspace checks

**Given** `preflight` is invoked on a correctly configured workspace
**When** workspace and onboarding validation complete
**Then** the command exits with a pass result and no lifecycle state is mutated

**Given** `preflight` is invoked on a workspace with a missing governance or install dependency
**When** validation runs
**Then** the command reports the missing dependency clearly and exits non-zero without mutating state

**Given** `test-setup-control-repo.py` is run against the rewritten preflight
**When** the test executes
**Then** it passes and confirms prompt-start path and workspace check parity with old-codebase behavior

**Given** the deprecated `onboard` prompt alias exists only as a legacy reference
**When** the published stub inventory is inspected
**Then** `onboard` is not present as a published command stub; only `preflight` is published

### Story 2.2: Rewrite new-domain Command (WP-02)

As a Lens user,
I want `new-domain` to create governance domain markers and a constitution scaffold exactly as it does today,
So that domain creation is governed and reproducible without naming-rule changes or governance-write surprises.

**Acceptance Criteria:**

**Given** `new-domain` is invoked with a valid domain name
**When** the command completes
**Then** governance domain markers are written under the stable naming rules defined in the architecture

**Given** `new-domain` is invoked with a valid domain name
**When** the command completes
**Then** a constitution scaffold is written for the new domain in the expected governance location

**Given** `new-domain` is invoked when the domain already exists
**When** the command runs
**Then** it exits without overwriting existing governance artifacts and reports the conflict clearly

**Given** the init-feature domain regressions are run against the rewritten new-domain
**When** the tests execute
**Then** they pass and confirm naming rules and scaffold creation match old-codebase behavior

### Story 2.3: Rewrite new-service Command (WP-03)

As a Lens user,
I want `new-service` to create a service under an existing domain with correct inheritance rules,
So that service creation is stable and does not silently change how services inherit domain-level governance.

**Acceptance Criteria:**

**Given** `new-service` is invoked with a valid service name under an existing domain
**When** the command completes
**Then** service markers are written in the correct location with the correct naming rules

**Given** `new-service` is invoked
**When** the command completes
**Then** service inheritance rules from the parent domain are preserved in the written service markers

**Given** `new-service` is invoked when the domain does not exist
**When** the command runs
**Then** it exits non-zero and reports that the domain must be created first

**Given** init-feature service regressions are run against the rewritten new-service
**When** the tests execute
**Then** they pass and confirm naming and inheritance match old-codebase behavior

### Story 2.4: Rewrite new-feature Command (WP-04)

As a Lens user,
I want `new-feature` to create a feature with the canonical featureId, feature-index registration, and 2-branch control-repo topology exactly as it does today,
So that active features created after the rewrite are governance-compatible with the existing schema and topology.

**Acceptance Criteria:**

**Given** `new-feature` is invoked with a valid domain, service, and featureSlug
**When** the command completes
**Then** the canonical featureId `{domain}-{service}-{featureSlug}` is computed and written to feature.yaml

**Given** `new-feature` completes
**When** the feature-index.yaml is inspected
**Then** the new feature is registered with its canonical featureId and featureSlug

**Given** `new-feature` completes
**When** the control-repo branches are inspected
**Then** exactly two branches exist: `{featureId}` and `{featureId}-plan`

**Given** `test-init-feature-ops.py` is run against the rewritten new-feature
**When** the tests execute
**Then** they pass and confirm featureId, index entry, and branch creation match old-codebase behavior

**Given** `test-git-orchestration-ops.py` is run against the rewritten new-feature branch topology
**When** the tests execute
**Then** they pass and confirm branch naming and topology match old-codebase behavior

**Given** the deprecated `init-feature` alias is not published
**When** the published stub inventory is inspected
**Then** `init-feature` is not present; only `new-feature` is published

### Story 2.5: Rewrite switch Command (WP-05)

As a Lens user,
I want `switch` to load a feature's context from feature-index.yaml without any lifecycle state mutation,
So that switching between features is safe and cannot accidentally alter governance or feature state.

**Acceptance Criteria:**

**Given** `switch` is invoked with the name of an existing feature
**When** the command completes
**Then** the active feature context is updated in the session to reflect the selected feature

**Given** `switch` is invoked
**When** the session context is inspected after the command
**Then** no changes were written to feature.yaml, feature-index.yaml, or any governance artifact

**Given** `switch` is invoked with a feature name that does not exist in feature-index.yaml
**When** the command runs
**Then** it exits non-zero and reports that the feature was not found without mutating state

**Given** a switch no-write regression test is run against the rewritten switch
**When** the test executes
**Then** it passes and confirms zero writes occurred to any lifecycle artifact

### Story 2.6: Rewrite next Command (WP-06)

As a Lens user,
I want `next` to route me to exactly the one unblocked next action for the active feature and auto-delegate without asking me again if I want to proceed,
So that the lifecycle feels intentional and I don't have to manually diagnose what to do next or confirm the same launch twice.

**Acceptance Criteria:**

**Given** `next` is invoked for a feature in a valid lifecycle phase
**When** routing logic runs
**Then** exactly one unblocked next action is identified and reported

**Given** `next` identifies the unblocked next action
**When** it delegates to the appropriate phase skill or command
**Then** no redundant launch confirmation is asked — the handoff is pre-confirmed

**Given** `next` is invoked for a feature that is blocked (e.g., all actions waiting on external dependency)
**When** routing logic runs
**Then** the blocker is reported clearly and no auto-delegation occurs

**Given** `test-next-ops.py` is run against the rewritten next command
**When** the tests execute
**Then** they pass and confirm blocker-first routing, single-choice, and auto-delegating behavior match old-codebase behavior

**Given** a handoff regression test is run
**When** the test executes
**Then** it confirms that delegated phase skills do not re-ask launch confirmation

---

## Epic 3: Constitution Bug Fix and Governance Rules Engine

Fix the org-level hard-fail bug in the `constitution` command (WP-15) and establish parity regression coverage for partial-hierarchy resolution. This epic is a **hard prerequisite for Epic 4**: the planning conductors in WP-07–WP-11 depend on correct constitution behavior, and their rewrite may not begin until this epic's story is `done`.

### Story 3.1: Fix Org-Level Constitution Hard-Fail Bug and Add Parity Tests (WP-15)

As a Lens user operating in a context that does not have an org-level constitution defined,
I want the `constitution` command to resolve constitutional guidance additively from whatever hierarchy levels are present without hard-failing on a missing org-level entry,
So that governance rules are applied correctly in partial-hierarchy environments without crashing or blocking downstream commands.

**Acceptance Criteria:**

**Given** `constitution` is invoked in a workspace where only domain-level and service-level constitutions exist (no org-level)
**When** the command runs
**Then** it resolves guidance additively from the available hierarchy levels and exits 0 without hard-failing

**Given** `constitution` is invoked in a workspace where all three levels (org, domain, service) exist
**When** the command runs
**Then** it resolves guidance from all three levels in the correct additive order

**Given** `constitution` is invoked in a workspace where no constitution levels exist
**When** the command runs
**Then** it reports that no governance rules were found and exits without mutating governance state

**Given** `constitution` is invoked at any hierarchy configuration
**When** the command runs
**Then** it does not write to any governance artifact (the command remains read-only)

**Given** a constitution partial-hierarchy regression test is run
**When** the test executes
**Then** it passes and covers missing-org-level, all-levels-present, and no-levels cases

**Given** the architecture WP-15 prerequisite constraint is documented
**When** Epic 4 story creation is attempted
**Then** each Epic 4 story's dev notes reference this story's completion as a prerequisite

---

## Epic 4: Planning Conductor Rewrite

Rewrite the five planning-phase conductor commands using the shared utilities from Epic 1. **This entire epic is blocked until Epic 3 (Story 3.1) is `done`.** The planning conductors are WP-07 (`preplan`), WP-08 (`businessplan`), WP-09 (`techplan`), WP-10 (`finalizeplan`), and WP-11 (`expressplan`).

Each story uses the shared utilities: `validate-phase-artifacts.py` for gate-checking, `bmad-lens-batch` for batch mode, and the `publish-to-governance` entry hook for publish-before-author ordering.

**Prerequisite:** Story 3.1 must be `done` before any story in this epic begins.

### Story 4.1: Rewrite preplan Command (WP-07)

As a Lens user,
I want `preplan` to orchestrate brainstorm, research, and product-brief artifact authoring through the shared batch contract and validate-phase-artifacts gate,
So that the brainstorm-first flow, batch contract, and review-ready fast path all behave as they do today.

**Acceptance Criteria:**

**Given** Epic 3 Story 3.1 is `done` before this story begins
**When** a developer starts work on this story
**Then** the prerequisite is confirmed to be complete

**Given** `preplan` is invoked for a feature in the correct phase
**When** the command starts
**Then** `light-preflight.py` runs as the prompt-start gate before any artifact authoring begins

**Given** `preplan` is invoked in non-batch mode
**When** the brainstorm-first flow runs
**Then** brainstorm artifact is authored before research or product-brief authoring begins

**Given** `preplan` is invoked in batch mode
**When** the batch pass runs
**Then** it delegates to `bmad-lens-batch` for intake and sequential skill invocation

**Given** `preplan`'s review gate is reached
**When** the gate check runs
**Then** it delegates to `validate-phase-artifacts.py` rather than implementing inline gate logic

**Given** phase-gate and wrapper-equivalence regression tests are run
**When** the tests execute
**Then** they pass and confirm brainstorm-first flow, batch contract, and review-ready gate match old-codebase behavior

### Story 4.2: Rewrite businessplan Command (WP-08)

As a Lens user,
I want `businessplan` to invoke the publish-to-governance entry hook before delegating PRD and UX authoring, with no direct governance writes,
So that publish-before-author ordering is enforced and the governance write boundary is preserved.

**Acceptance Criteria:**

**Given** Epic 3 Story 3.1 is `done` before this story begins
**When** a developer starts work on this story
**Then** the prerequisite is confirmed to be complete

**Given** `businessplan` is entered
**When** the entry hook runs
**Then** `publish-to-governance` is invoked before PRD or UX authoring is delegated to

**Given** `businessplan` runs PRD and UX skill delegation
**When** the governance write audit is run post-execution
**Then** no direct governance writes from businessplan are found; writes only occurred via `publish-to-governance`

**Given** wrapper-equivalence regression tests and governance write audit tests are run
**When** the tests execute
**Then** they pass and confirm publish-before-author ordering and no-direct-governance-write invariant

### Story 4.3: Rewrite techplan Command (WP-09)

As a Lens user,
I want `techplan` to generate architecture artifacts with the publish-before-author entry hook and the PRD reference rule enforced,
So that architecture generation is governed and the reference to the authoritative PRD remains intact.

**Acceptance Criteria:**

**Given** Epic 3 Story 3.1 is `done` before this story begins
**When** a developer starts work on this story
**Then** the prerequisite is confirmed to be complete

**Given** `techplan` is entered
**When** the entry hook runs
**Then** `publish-to-governance` is invoked before architecture authoring is delegated to

**Given** `techplan` generates the architecture artifact
**When** the artifact is inspected
**Then** it contains an explicit reference to the authoritative PRD

**Given** architecture-reference regression tests and wrapper-equivalence tests are run
**When** the tests execute
**Then** they pass and confirm publish-entry and PRD reference rule match old-codebase behavior

### Story 4.4: Rewrite finalizeplan Command (WP-10)

As a Lens user,
I want `finalizeplan` to execute its three steps in strict order (adversarial review → plan PR → downstream bundle) without skipping or reordering,
So that the plan PR topology and downstream bundle sequence are reliably produced and the finalizeplan step-order contract is preserved.

**Acceptance Criteria:**

**Given** Epic 3 Story 3.1 is `done` before this story begins
**When** a developer starts work on this story
**Then** the prerequisite is confirmed to be complete

**Given** `finalizeplan` is invoked
**When** Step 1 (adversarial review) runs
**Then** it must complete before Step 2 begins

**Given** Step 1 completes
**When** Step 2 (plan PR creation: `{featureId}-plan` → `{featureId}`) runs
**Then** the PR is created with the correct base and head branches

**Given** Step 2 completes
**When** Step 3 (downstream bundle: epics, impl-readiness, sprint-status, story files) runs
**Then** all bundle artifacts are generated and committed before the final PR (`{featureId}` → `main`) is created

**Given** finalizeplan step-order regression tests are run
**When** the tests execute
**Then** they pass and confirm three-step strict ordering, plan PR topology, and bundle sequence

### Story 4.5: Rewrite expressplan Command (WP-11)

As a Lens user,
I want `expressplan` to route only express-eligible features, delegate to the internal QuickPlan skill, and enforce a hard-stop adversarial review before the finalize bundle,
So that the express track compression stays intact and cannot be invoked for features that require the full planning track.

**Acceptance Criteria:**

**Given** Epic 3 Story 3.1 is `done` before this story begins
**When** a developer starts work on this story
**Then** the prerequisite is confirmed to be complete

**Given** `expressplan` is invoked for a feature that is not express-eligible
**When** the eligibility gate runs
**Then** it hard-stops and informs the user that the full planning track is required

**Given** `expressplan` is invoked for an express-eligible feature
**When** the internal delegation runs
**Then** `bmad-lens-quickplan` is invoked (not a planning-phase conductor)

**Given** `expressplan` delegation completes
**When** the hard-stop adversarial review gate runs
**Then** it must complete before the finalize bundle is produced

**Given** expressplan gating and quickplan retention regression tests are run
**When** the tests execute
**Then** they pass and confirm express-only gating, QuickPlan delegation, and hard-stop review behavior

---

## Epic 5: Execution, Closure, and Maintenance Commands

Rewrite the final five commands: `dev` (WP-12), `complete` (WP-13), `split-feature` (WP-14), `discover` (WP-16), and `upgrade` (WP-17). Story 5.5 (upgrade + E2E regression gate) is the final story in the entire rewrite and serves as the release-readiness proof gate. No release promotion should occur until Story 5.5 is `done`.

### Story 5.1: Rewrite dev Command (WP-12)

As a Lens user,
I want `dev` to continue executing governed development work in target repositories only, with per-task commits, resumable checkpoints, and a final PR, exactly as it does today,
So that my in-progress development sessions survive the rewrite and I can resume disrupted work without recreating state.

**Acceptance Criteria:**

**Given** `dev` is invoked for a feature that has completed planning
**When** the command starts
**Then** all code writes are routed to the target repository, never the control repo or release repo

**Given** `dev` executes a task
**When** the task completes
**Then** a commit is made in the target repo with the per-task commit message format

**Given** a `dev` session is interrupted mid-task
**When** the user re-invokes `dev`
**Then** the session resumes from the last checkpoint in `dev-session.yaml` without requiring schema migration or recreation

**Given** all tasks in the dev session are complete
**When** the final step runs
**Then** a PR is created in the target repository

**Given** `dev-session.yaml` compatibility regression tests are run
**When** the tests execute
**Then** they pass and confirm session file readability, per-task commits, resume behavior, and final PR semantics

### Story 5.2: Rewrite complete Command (WP-13)

As a Lens user,
I want `complete` to run retrospective and documentation steps before archiving a feature, with the archive being terminal and atomic,
So that the retrospective-before-archive ordering is preserved and closed features cannot be accidentally modified.

**Acceptance Criteria:**

**Given** `complete` is invoked for a feature that is eligible for closure
**When** the command starts
**Then** retrospective step runs before documentation step

**Given** retrospective completes
**When** documentation step runs
**Then** the documentation artifact is written before the archive step is triggered

**Given** documentation completes
**When** the archive step runs
**Then** the feature is marked archived in governance and no further lifecycle mutations are permitted

**Given** complete/archive atomicity regression tests are run
**When** the tests execute
**Then** they pass and confirm retrospective/document/archive ordering and terminal archive semantics

### Story 5.3: Rewrite split-feature Command (WP-14)

As a Lens user,
I want `split-feature` to validate before executing, block splits that include in-progress stories, create the new governance feature as a first-class citizen before modifying the source feature, and optionally move eligible stories,
So that split operations are safe, reversible at the validation stage, and never leave governance in a partially-split broken state.

**Acceptance Criteria:**

**Given** `split-feature` is invoked
**When** validation runs (via the `validate-split` subcommand surface)
**Then** the split is validated before any governance writes occur

**Given** validation passes and the split would include stories that are `in-progress`
**When** the split execution is attempted
**Then** it is blocked and the user is informed that in-progress stories must be resolved first

**Given** validation passes and no in-progress stories are present
**When** split execution runs via `create-split-feature`
**Then** the new governance feature (feature.yaml, feature-index entry, branches) is created first, before any modification to the source feature

**Given** the new feature is created
**When** eligible story movement runs via `move-stories`
**Then** stories are moved to the new feature and the source feature's story list is updated correctly

**Given** `test-split-feature-ops.py` and split dry-run regression tests are run
**When** the tests execute
**Then** they pass and confirm validate-first execution, in-progress blocking, feature-creation-before-modification, and story-move semantics

### Story 5.4: Rewrite discover Command (WP-16)

As a Lens maintainer,
I want `discover` to perform bidirectional inventory sync between governance and local clones and auto-commit its sync results to governance-main,
So that the inventory is always current and the explicit auto-commit exception that makes `discover` different from other planning commands is preserved.

**Acceptance Criteria:**

**Given** `discover` is invoked
**When** the bidirectional sync runs
**Then** both governance → local and local → governance inventory directions are synchronized

**Given** sync produces updates to the governance inventory
**When** the sync completes
**Then** the governance-main auto-commit is made automatically as the defined exception behavior

**Given** `discover` produces no inventory changes
**When** the sync completes
**Then** no commit is made and the no-op is reported clearly

**Given** discover bidirectional sync regression tests are run
**When** the tests execute
**Then** they pass and confirm bidirectional sync and governance-main auto-commit behavior match old-codebase behavior

### Story 5.5: Rewrite upgrade Command and Run E2E Regression Gate (WP-17)

As a Lens maintainer,
I want `upgrade` to remain a no-op for v4 features and retain an explicit migration path for future schema changes, and I want the end-to-end regression gate to confirm that all 17 retained commands and their dependencies are present and passing before release promotion is allowed,
So that the rewrite is certifiably safe to promote as a v4 drop-in and the release-readiness evidence is explicit rather than implied.

**Acceptance Criteria:**

**Given** `upgrade` is invoked on a feature at schema v4
**When** the command runs
**Then** it exits 0 reporting a no-op with no modifications to any artifact

**Given** `upgrade` is invoked on a feature at a schema version older than v4
**When** the command runs
**Then** it applies the explicit migration path appropriate to that version gap

**Given** `test-upgrade-ops.py` is run
**When** the tests execute
**Then** they pass and confirm v4 no-op behavior and migration-path retention

**Given** the E2E regression gate is run
**When** all focused regression tests execute
**Then** test-init-feature-ops.py, test-next-ops.py, test-setup-control-repo.py, test-split-feature-ops.py, test-upgrade-ops.py, and test-git-orchestration-ops.py all pass

**Given** the E2E regression gate completes
**When** the published prompt surface is counted
**Then** exactly 17 prompts are present and all 17 are reachable through supported install paths

**Given** the E2E regression gate completes
**When** all 17 retained commands are traced end to end
**Then** each command's prompt stub, release prompt, and owning bmad-lens-* skill are all present and linked

**Given** the E2E regression gate passes
**When** a release readiness report is produced
**Then** it confirms 0 broken active features, 0 schema changes, 17/17 retained prompts present, and all regression anchors green — and this report is the gate artifact for release promotion
