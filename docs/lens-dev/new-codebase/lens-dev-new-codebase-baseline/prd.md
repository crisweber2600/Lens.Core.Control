---
feature: lens-dev-new-codebase-baseline
doc_type: prd
status: draft
goal: "Define product requirements for the lens-work rewrite: 17-command stable surface with 100% backwards compatibility"
stepsCompleted: [step-01-init, step-02-discovery, step-02b-vision, step-02c-executive-summary, step-03-success, step-04-journeys, step-05-domain, step-06-innovation, step-07-project-type, step-08-scoping, step-09-functional, step-10-nonfunctional, step-11-polish, step-12-complete]
inputDocuments:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/product-brief.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/research.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/brainstorm.md
  - TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/deep-dive-lens-work-module.md
  - TargetProjects/lens/lens-governance/features/lens-dev/old-codebase/lens-dev-old-codebase-discovery/docs/dependency-mapping.md
workflowType: prd
classification:
  projectType: developer_tool
  domain: developer workflow orchestration
  complexity: high
  projectContext: brownfield
key_decisions:
  - Retain 17 published commands, explicitly including split-feature
  - Use the old-codebase discovery deep-dive and dependency mapping as the behavioral baseline
  - Treat retained-command traceability and command-by-command auditability as first-class product requirements
open_questions: []
depends_on: [product-brief, research, brainstorm]
blocks: []
updated_at: 2026-04-22T05:06:18Z
---

# Product Requirements Document — lens-work Rewrite: 17-Command Stable Surface

**Author:** CrisWeber
**Date:** 2026-04-22

## Executive Summary

`lens-work` is a brownfield rewrite of the LENS Workbench command surface and lifecycle module. The goal is not to invent a new product, but to make the existing one stable, trustworthy, and maintainable without breaking active features, governance state, branch topology, prompt contracts, or in-progress development sessions. The rewrite reduces the published prompt surface from 54 stubs to 17 retained commands, while preserving the underlying behavior, contracts, and operational boundaries that existing users already depend on.

The central product goal is stability. The current system became unstable because fixes were applied ad hoc, outside the governed Lens workflow the product is supposed to enforce. This rewrite is the opportunity to restore process integrity by rebuilding Lens through Lens itself. That makes the work both corrective and demonstrative: it hardens the product while proving that the workflow can successfully govern its own evolution.

The rewrite is therefore a command-surface preservation effort with internal consolidation, not a feature expansion project. User-visible behavior stays stable. Internal duplication is reduced by extracting shared utilities for review-ready gating, batch intake, and publish-before-author phase entry. The resulting product should be easier to trust, easier to navigate, and easier to maintain, while remaining fully compatible with the current lifecycle schema and all active features already tracked in governance.

### What Makes This Special

What makes this rewrite special is not just prompt reduction or code cleanup. It is deliberate dogfooding. Lens is being rebuilt using the same disciplined lifecycle, governance, and review model that it asks users to follow. That gives the rewrite a stronger value proposition than simplification alone: it is a proof that the workflow can produce stability when it is actually obeyed.

The differentiator is that stability is treated as the product outcome, not just an engineering side effect. The rewrite preserves published command behavior, keeps tested edge-case workflows such as `split-feature`, and explicitly maps every retained command end to end from prompt stub through skill, scripts, data contracts, outputs, and regression coverage. This shifts Lens from a system that was patched into working order to one whose behavior is specified, governed, and testable.

## Project Classification

- Project Type: developer tool
- Domain: developer workflow orchestration
- Complexity: high
- Project Context: brownfield

This classification reflects a source-authoritative lifecycle module with AI-facing prompts and skills, governance metadata, release-promotion tooling, and strict backwards-compatibility obligations across command behavior, schema, and session state.

## 1. Product Intent

Rewrite `lens-work` as a reduced but fully backwards-compatible published command surface. The rewrite now targets **17** published commands, not 16, because `split-feature` is still part of the shipped prompt surface, installer metadata, module help, and regression test suite. The old-codebase discovery artifacts remain the source baseline for retained-command behavior, especially the work-module deep dive and dependency map.

## 2. Scope and Invariants

### 2.1 Published Command Surface

The rewrite must preserve these 17 published commands:

| Command | Preserved user job |
|---|---|
| `preflight` | Validate workspace and onboarding state |
| `new-domain` | Create a new governance domain |
| `new-service` | Create a service under an existing domain |
| `new-feature` | Create a feature with canonical identity and 2-branch topology |
| `switch` | Switch active feature context |
| `next` | Route to the one unblocked next action |
| `preplan` | Produce brainstorm, research, and product brief artifacts |
| `businessplan` | Produce PRD and UX design artifacts |
| `techplan` | Produce architecture artifacts |
| `finalizeplan` | Consolidate planning, generate bundle, and open plan/final PRs |
| `expressplan` | Run compressed express-track planning |
| `dev` | Execute implementation in target repos |
| `complete` | Retrospective, document, and archive a feature |
| `split-feature` | Split an existing feature into a new initiative with eligible stories/scope |
| `constitution` | Resolve constitutional governance |
| `discover` | Sync repo inventory with local clones |
| `upgrade` | Apply lifecycle upgrades or report no-op |

### 2.2 Rewrite Invariants

- `light-preflight.py` remains the prompt-start gate for every retained published command.
- The canonical feature identity formula remains `{domain}-{service}-{featureSlug}`.
- The control-repo planning topology remains `{featureId}-plan -> {featureId} -> main`.
- `publish-to-governance` remains the only valid planning-phase path for governance artifact publication.
- `next` handoff remains pre-confirmed; delegated phase skills must not re-ask launch confirmation.
- `dev-session.yaml` remains backwards compatible for in-progress dev work.
- `dev` remains a governed execution conductor entered after planning completion, not a planning lifecycle milestone.
- `split-feature` remains validate-first, blocks in-progress stories, and creates the new feature as a first-class governance citizen before modifying the source feature.

### 2.3 Public Alias and Deprecation Clarifications

- `new-feature` is the canonical published command. `init-feature` remains a deprecated compatibility alias or historical reference and is not part of the retained 17-command surface.
- `preflight` is the canonical published command. The legacy `onboard` prompt is deprecated as a public surface even though onboarding behavior remains behind `preflight` and retained internal skills or scripts.
- `new-project` is removed from the published surface and has no direct day-1 replacement in this rewrite.

## 3. Compatibility & Parity Guardrails

### 3.1 R1 — Published Surface and Installer Parity

- Exactly 17 `.github/prompts/` stubs remain published on completion.
- `setup.py`, install surfaces, module help, prompt manifests, and the retained command inventory must agree on the same 17-command set.
- `split-feature` must remain published. It may not be silently demoted to an internal-only skill while installation or help surfaces continue to advertise it.

### 3.2 R2 — Retained Command Requirement Matrix

| Command | Must preserve end-to-end | Validation anchor |
|---|---|---|
| `preflight` | Stub runs `light-preflight.py`, then workspace/onboarding validation without mutating feature lifecycle state | `test-setup-control-repo.py` plus prompt-start regression |
| `new-domain` | Domain creation writes governance markers and constitution scaffold under stable naming rules | init-feature domain regressions |
| `new-service` | Service creation enforces existing domain and preserves inheritance rules | init-feature service regressions |
| `new-feature` | Feature creation preserves canonical featureId, feature-index registration, and 2-branch topology | `test-init-feature-ops.py` and git-orchestration branch regressions |
| `switch` | Switching remains read-only and loads feature context from `feature-index.yaml` | switch no-write regression |
| `next` | Routing remains blocker-first, single-choice, and auto-delegating | `test-next-ops.py` plus handoff regression |
| `preplan` | Brainstorm-first flow, batch contract, and review-ready fast path remain equivalent | phase gate and wrapper-equivalence regressions |
| `businessplan` | Publish-before-author ordering and PRD/UX delegation remain equivalent; no direct governance writes | wrapper-equivalence regressions plus governance write audit |
| `techplan` | Architecture generation preserves PRD reference rule and publish-before-author entry | architecture reference regression |
| `finalizeplan` | Three-step ordering, plan PR topology, and downstream bundle sequence remain intact | finalizeplan step-order regression |
| `expressplan` | Express-only gating, internal QuickPlan delegation, and hard-stop adversarial review remain intact | expressplan gating and quickplan retention regressions |
| `dev` | Target-repo-only code writes, per-task commits, resumable checkpoints, and final PR behavior remain intact | `dev-session.yaml` compatibility and task-commit regressions |
| `complete` | Retrospective/document-before-archive ordering and terminal archive semantics remain intact | complete/archive atomicity regression |
| `split-feature` | Validate-first split workflow blocks in-progress stories, creates new governance feature first, then optionally moves stories | `test-split-feature-ops.py` and split dry-run regression |
| `constitution` | Additive hierarchy resolution with missing org-level fallback remains read-only | constitution partial-hierarchy regression |
| `discover` | Bidirectional inventory sync and governance-main auto-commit behavior remain intact | discover bidirectional sync regression |
| `upgrade` | v4-to-v4 remains a no-op; migrations only execute when schema actually changes | `test-upgrade-ops.py` |

### 3.3 R3 — Internal Dependency Retention

- All retained commands must continue to resolve from prompt stub -> release prompt -> owning `bmad-lens-*` skill.
- Internal skills still required by retained commands must remain available even when their own published stubs are removed.
- `split-feature` must retain its dedicated script surface: `validate-split`, `create-split-feature`, and `move-stories`.

## 4. Acceptance Criteria

1. The installed published prompt surface is exactly 17 commands and includes `split-feature`.
2. The retained-command traceability matrix in `research.md` is complete for all 17 commands and is reflected in this PRD.
3. No retained command loses a documented old-codebase behavior without an explicit replacement requirement and validation plan.
4. Planning-phase commands continue to avoid direct governance writes except via `publish-to-governance`; `discover` retains its explicit auto-commit-to-main exception.
5. `split-feature` remains user-facing, validates before execution, blocks in-progress stories, and preserves feature-index/feature.yaml/summary creation semantics.
6. Regression coverage remains attached to every retained command, either through existing tests or rewrite-era parity tests.

## 5. Implementation Checklist

Use this checklist during TechPlan and implementation to audit the rewrite command by command.

| Command | Audit focus | Done when |
|---|---|---|
| `preflight` | Stub contract and onboarding scripts | Prompt-start path and workspace checks behave identically |
| `new-domain` | Governance scaffold creation | Domain markers and constitution scaffold are written correctly |
| `new-service` | Service inheritance and pathing | Service markers and inheritance rules are preserved |
| `new-feature` | Identity and branch creation | `featureId`, index entry, and branches match old behavior |
| `switch` | Read-only context loading | Feature selection updates session context without writes |
| `next` | Delegation semantics | Recommended next action auto-delegates without redundant confirmation |
| `preplan` | Artifact authoring contract | Brainstorm-first flow, batch mode, and review gate all match baseline |
| `businessplan` | Publish-entry hook | Reviewed preplan docs publish first; PRD/UX writes stay local |
| `techplan` | Architecture parity | Architecture generation keeps publish-entry and reference rules |
| `finalizeplan` | Step atomicity | Review, plan PR, and downstream bundle still execute in strict order |
| `expressplan` | Express compression | QuickPlan + review + finalize bundle remain a single express flow |
| `dev` | Target-repo execution | Resume state, per-task commits, and final PR remain intact |
| `complete` | Terminal archive workflow | Retrospective/document/archive ordering is unchanged |
| `split-feature` | Split validation and execution | Validate-first, in-progress blocking, feature creation, and story moves all match tested behavior |
| `constitution` | Hierarchy resolution | Partial hierarchy works without failure or mutation |
| `discover` | Inventory synchronization | Repo sync and governance-main auto-commit behavior remain explicit |
| `upgrade` | Version handling | v4 drop-in stays a no-op and migrations stay explicit |

## 6. Delivery Notes

This PRD intentionally folds the retained-command requirement map into a product-facing artifact so the rewrite can be reviewed as a command-surface preservation effort, not just a code cleanup. `split-feature` is retained because the current module still installs it, documents it, and tests it; removing it would require a separate deliberate surface-cut decision across code, help, docs, and validation.

## Success Criteria

### User Success

Existing Lens users experience the rewrite as a non-event operationally and a relief emotionally. The success moment is that they can continue active work without reinitializing features, repairing broken state, or learning a new workflow just to stay productive.

User success is achieved when:

- active features continue to work after the rewrite with no broken feature behavior
- in-progress development sessions remain resumable without schema migration or manual repair
- the retained 17-command surface behaves predictably enough that users encounter fewer workflow bugs and fewer surprise failures than before
- users can invoke every retained published prompt on day 1 and have its required dependencies available and functioning

### Business Success

Business success for this rewrite requires all three intended outcomes, not just one of them:

- the rewrite is completed through Lens workflows rather than through off-process fixes, proving the product can govern its own evolution
- future changes to Lens become safer and easier because retained behaviors, dependencies, and validation anchors are explicitly mapped end to end
- release confidence improves because the shipped surface is smaller, internally coherent, and backed by parity-focused validation instead of ad hoc patching

A successful outcome is not just cleaner code. It is a Lens release that is easier to trust, easier to promote, and less likely to require reactive repair.

### Technical Success

Technical success is strict:

- zero schema changes are required for lifecycle, governance, feature, or session artifacts
- zero existing features are broken by the rewrite
- all 17 retained prompts remain published and operational on day 1
- all required prompt dependencies remain present on day 1, including kept internal skills and script surfaces needed by retained commands
- frozen contracts remain intact, especially feature identity, branch topology, publish-to-governance boundaries, next handoff behavior, dev-session compatibility, and split-feature semantics
- focused regression coverage remains green and parity gaps are explicitly covered where old tests do not already exist

### Measurable Outcomes

The measurable outcomes for the rewrite are:

- post-rewrite bug reports for Lens workflow behavior trend down versus the pre-rewrite baseline
- 0 required schema migrations for existing Lens features and sessions
- 0 broken existing features at rewrite release
- 17 of 17 retained published prompts available at release
- 17 of 17 retained prompts have their required dependencies present and working on day 1
- 100% of retained prompts have end-to-end requirement mapping and validation anchors
- retained focused regression suites pass without relaxing backwards-compatibility expectations
- the rewrite is delivered through the governed Lens workflow rather than off-the-books fixes

## Product Scope

### MVP - Minimum Viable Product

Day 1 must include the full retained surface, not a partial landing:

- all 17 retained prompts are present
- all prompt dependencies required by those prompts are present
- zero schema changes are introduced
- zero existing features are broken
- parity is preserved for special cases such as split-feature, discover, next, and publish-before-author planning flows
- retained-command mapping and regression expectations are complete enough to defend the release as stable

### Growth Features (Post-MVP)

Post-MVP work can improve stability evidence and maintainability, but is not required to make the rewrite valid on day 1:

- broader parity automation beyond the current focused regression anchors
- better observability around prompt failures, dependency drift, and workflow regressions
- deeper internal consolidation once parity is already proven stable

### Vision (Future)

The long-term vision is a Lens module that is trusted because it changes through governed process rather than emergency repair. The rewrite becomes the proof case that Lens can rebuild itself without breaking user work, while giving future maintainers a stable command surface, explicit dependency map, and safer path for continued evolution.

## User Journeys

### Journey 1: Nina, New Lens User on the Success Path

Nina is new to Lens and arrives with a simple goal: start governed work without inventing process from scratch. In the old experience, her first emotion is overwhelm. She sees a crowded command surface, cannot tell which commands are core versus incidental, and does not know what the safe first move is.

In the rewrite, Nina meets a smaller, legible surface. She starts with `new-domain`, `new-service`, or `new-feature` as appropriate, then relies on `next` to move forward without having to understand every internal module. The rising action is not command archaeology. It is guided progression. Each retained prompt has the dependencies it needs, each handoff works, and the lifecycle feels intentional instead of improvised.

The climax is when `next` routes her into the correct next phase without redundant launch questions or broken delegation. Lens stops feeling like a framework she has to decode and starts feeling like an operating model she can trust. The resolution is orientation and confidence: Nina is no longer overwhelmed. She understands where to begin, how to proceed, and why the system is stable enough to rely on.

This journey reveals requirements for:

- a legible retained command surface
- reliable prompt-start behavior
- complete day-1 dependency availability for retained prompts
- predictable phase routing and handoff behavior
- onboarding that exposes the lifecycle without exposing internal clutter

### Journey 2: Evan, Existing Lens User Recovering Mid-Feature Without Schema Changes

Evan is already inside active work when the rewrite lands. He has real feature state, real branch topology, and partially completed lifecycle progress. His first emotion is anxiety, not curiosity. He is not asking whether the rewrite is cleaner. He is asking whether it is about to break the work he already has.

The situation is worse than a clean resume. Evan is recovering from a disrupted workflow state. A prior run stopped partway through, and he needs the rewritten Lens to recognize the existing state without asking for migration, manual repair, or schema edits. He re-enters through the retained prompts, expecting friction.

The rising action is disciplined compatibility. Lens reads the existing lifecycle artifacts and session state as they are. `feature.yaml`, `feature-index.yaml`, and `dev-session.yaml` remain readable because their schemas did not change. Re-entry happens through preserved command contracts, not through hidden conversion logic. The system does not figure it out somehow. It works because the rewrite preserved the boundaries that matter.

The climax is the recovery moment: Evan continues from disrupted but valid existing state without changing schemas, rewriting state, or recreating the feature. The resolution is relief. Stability is proven under pressure, not just in a demo. The rewrite earns trust because it preserves continuity when the user most needs it.

This journey reveals requirements for:

- zero schema changes across lifecycle, governance, and session artifacts
- continued readability of existing feature state
- recovery-safe behavior for partially completed work
- preserved feature identity and branch topology contracts
- validation focused on backward-compatible recovery, not only clean-start flows

### Journey 3: Mara, Lens Module Maintainer Improving the System Safely

Mara maintains Lens itself. In the old state, her first emotion is fatigue. A bug in a phase contract often means the same fix has to be made in several different places, each carrying similar logic with slightly different wording or behavior. The product works, but too much of that correctness depends on memory and repeated manual edits.

In the rewrite, Mara sees a system that is being rebuilt through Lens, not around Lens. That matters. The dogfooding is part of the product proof. As she reviews or changes a retained command, she can follow the full chain: prompt stub, redirect, owning skill, scripts, contracts, outputs, and validation. Shared lifecycle patterns are centralized instead of duplicated, so a fix to a gate or batch contract becomes a deliberate system change rather than a scavenger hunt.

The climax is operational safety: Mara makes a change once, validates it against known retained-command anchors, and does not have to wonder which duplicated implementation was missed. The resolution is confidence instead of fatigue. The rewrite becomes maintainable because behavior is explicit, shared, and governed rather than patched reactively.

This journey reveals requirements for:

- single-source shared utilities for repeated lifecycle behavior
- end-to-end retained-command traceability
- validation tied to preserved contracts
- architecture that reduces copy-paste drift without changing user-visible behavior
- dogfooding through Lens workflows as part of the product's credibility

### Journey 4: Ian, BMAD Work-System Integrator Seeking a Surface He Can Trust

Ian integrates Lens into a broader BMAD work system. His first emotion is uncertainty. He does not just need commands that appear to exist. He needs to know which prompts are truly public, which dependencies are required underneath them, and whether the visible surface actually matches the shipped implementation.

In the old state, too many wrappers and unclear boundaries make safe integration difficult. In the rewrite, Ian encounters a command surface whose public and internal boundaries are explicit. The published prompts are the retained prompts. The required internal skills remain available where needed. Help output, installer behavior, manifests, and validation evidence all align with the same surface definition.

The climax is when Ian can treat Lens as a stable integration surface rather than a shifting bundle of conventions. There is no silent removal of `split-feature`, no mismatch between installer and help, no hidden missing dependency on day 1. The resolution is trust. Ian can embed and reason about Lens because the prompt surface and dependency surface finally agree.

This journey reveals requirements for:

- exact agreement across prompts, installer, help, manifests, and tests
- explicit distinction between public prompts and retained internal modules
- no silent loss of shipped capabilities
- day-1 availability of all retained prompt dependencies
- documented surface boundaries that external integrators can rely on

### Journey Requirements Summary

These journeys expose four distinct requirement classes the rewrite must satisfy:

- onboarding clarity: new users need a smaller, legible command surface that still carries the full governed lifecycle
- recovery compatibility: existing users must be able to recover and continue work from disrupted state without schema changes or broken features
- maintainability: module maintainers need shared utilities, explicit contracts, and validation anchors so stability is preserved deliberately
- integration coherence: integrators need the published prompt surface and dependency surface to agree across prompts, help, installer metadata, manifests, and tests

Together, these journeys make the product promise concrete: Lens is being rebuilt through Lens, and the result must be stable on both the success path and the recovery path.

## Domain-Specific Requirements

### Compliance & Regulatory

- This domain is governed less by external regulation and more by internal operational compliance: planning-before-code, PR-only gating, explicit authority boundaries, and Git as the shared source of truth are product rules, not implementation preferences.
- The rewrite must preserve lifecycle compliance with the existing Lens operating model rather than reinterpret it.
- Governance publication boundaries are mandatory: planning-phase artifacts may only reach governance through `publish-to-governance`, with `discover` remaining the sole documented auto-commit exception.
- Release compatibility is part of the domain contract: the rewrite must remain a v4 drop-in and must not impose user migration work.

### Technical Constraints

- zero schema changes across `feature.yaml`, `feature-index.yaml`, `repo-inventory.yaml`, and `dev-session.yaml`
- zero broken existing features, including active governance-tracked features already in use
- all 17 retained prompts and all required dependencies must be present and operational on day 1
- frozen workflow contracts must remain intact:
  - `featureId = {domain}-{service}-{featureSlug}`
  - `{featureId}-plan -> {featureId} -> main` branch topology
  - `light-preflight.py` prompt-start behavior
  - `next` handoff without redundant confirmation
  - `split-feature` validate-first and in-progress-blocked semantics
  - `dev-session.yaml` resumability
- shared behavior extraction is allowed only if it preserves user-visible parity

### Integration Requirements

- published prompts, installer behavior, setup surfaces, module help, manifests, and tests must all agree on the same retained command surface
- the rewrite must continue to interoperate cleanly with the governance repo, the release promotion flow, and target repos without blurring authority boundaries
- old-codebase discovery artifacts remain the authoritative behavior baseline for retained-command mapping and parity decisions
- internal skills and scripts required by retained prompts must stay available even when their own published stubs are removed
- target-repo code writes must remain isolated to target repos, never the control repo or release repo

### Risk Mitigations

- schema drift risk:
  - mitigation: explicit v4 drop-in requirement, no-op upgrade behavior for v4, focused regression coverage on lifecycle artifacts
- silent surface mismatch risk:
  - mitigation: parity audit across prompts, installer, help, manifests, and tests
- recovery-path breakage risk:
  - mitigation: validate disrupted-workflow resume behavior, not just clean-start happy paths
- duplicated contract drift risk:
  - mitigation: shared utilities for review-ready, batch, and publish-entry behavior with parity validation
- hidden governance-write risk:
  - mitigation: preserve the CLI boundary and explicitly audit for direct governance mutations
- partial workflow ambiguity risk:
  - mitigation: preserve finalizeplan atomicity and dev-session checkpoint compatibility

## Innovation & Novel Patterns

### Detected Innovation Areas

- self-governed product evolution
  - The rewrite is not just using Lens as a build aid. It is using Lens as the governing mechanism for its own correction. That makes the work demonstrative as well as corrective.
- stability as a first-class product outcome
  - Stability is not framed as we refactored well. It is defined as preserved commands, preserved dependencies, zero schema churn, zero broken features, and recovery-safe compatibility.
- traceability as product surface
  - The retained 17-command surface is being mapped end to end from prompt stub through skill, scripts, contracts, outputs, governance touchpoints, and validation. That turns what is usually internal engineering hygiene into explicit product behavior.
- governed dogfooding as differentiation
  - The rewrite's claim is that the workflow can prove its own credibility by surviving its own rules. That is stronger than saying the code got cleaner.

### Market Context & Competitive Landscape

Most developer workflow tools differentiate on speed, convenience, or automation. Many AI-assisted workflow systems help users generate artifacts or route tasks, but they do not require their own evolution to obey the same governance model they impose on users.

Lens is different in a narrower but meaningful way. Its differentiation is not novelty for novelty's sake. It is that the system is being rebuilt under the same planning, review, lifecycle, and governance rules it expects users to follow. Combined with retained-command traceability and parity validation, that positions Lens less as a loose automation layer and more as a governed workflow system whose trustworthiness is part of the product.

The competitive fallback, if that innovation claim feels too strong, is still valuable: Lens becomes an unusually disciplined backwards-compatible rewrite of a developer workflow tool, with explicit command-surface parity and recovery-path guarantees that many comparable tools leave implicit.

### Validation Approach

- prove the rewrite is completed through Lens workflows rather than off-process fixes
- demonstrate that all 17 retained prompts and their required dependencies are available and working on day 1
- validate that every retained command has an end-to-end requirement map and attached validation anchor
- prove the v4 drop-in claim with zero schema changes across lifecycle, governance, and session artifacts
- validate the recovery path, not just clean-start flows:
  - existing features remain operable
  - disrupted workflow state remains recoverable
  - dev sessions remain resumable
- use focused regression evidence to show preserved behavior for load-bearing workflows such as `next`, `new-feature`, `split-feature`, `discover`, `upgrade`, and prompt-start preflight behavior

### Risk Mitigation

- innovation-overclaim risk
  - mitigation: position the novelty as a new operating-model pattern for developer workflow governance, not as an entirely unprecedented software category
- dogfooding fails to demonstrate credibility
  - mitigation: require the rewrite to be completed through Lens workflows with visible artifacts, review gates, and parity evidence
- traceability becomes documentation theater
  - mitigation: require each retained-command map to connect to executable validation, not just prose
- innovation framing expands scope
  - mitigation: keep the rewrite bounded to stability, parity, and retained-surface preservation; do not allow innovation to justify feature expansion
- fallback positioning needed
  - mitigation: if the self-governed-evolution story is not compelling enough, position the release as an excellent backwards-compatible rewrite with unusually strong recovery, compatibility, and audit guarantees

## Developer Tool Specific Requirements

### Project-Type Overview

`lens-work` is a Python-first developer workflow tool whose primary user-facing surface is the retained 17-command prompt interface. Unlike a narrow library or SDK, its usable product surface also includes installation and bootstrap paths, help and manifest inventories, adapter integrations, and the internal dependency chain that allows each retained prompt to function correctly.

For this rewrite, developer-tool quality is defined by operational trust: installation works, integrations remain available, published commands remain coherent, dependencies remain present, and existing users can continue work without migration or repair.

### Technical Architecture Considerations

The rewrite must preserve a layered architecture in which the published prompt surface remains stable while the internal dependency chain stays explicit and testable. Python remains the primary implementation language, but Markdown, YAML, CSV, prompt files, and shell-facing bootstrap surfaces are first-class operational artifacts because they participate directly in workflow execution, installation, governance, and agent integration.

The architecture must continue to distinguish clearly between:

- public user-facing prompt commands
- internal retained skills and scripts
- installation and manifest surfaces
- IDE and agent integration adapters
- governance and release-promotion boundaries

A dependency failure in any of those layers counts as a product regression, not just an implementation defect.

### Language Matrix

- Python is the authoritative implementation language for the rewrite.
- Supporting artifact languages and formats including Markdown, YAML, CSV, and prompt files remain first-class and in scope because they are part of the product's executable workflow surface.
- The rewrite must not introduce a second core implementation language as a new dependency for day-1 functionality.
- Python-based scripts, utilities, and validation anchors must remain compatible with the retained command surface and the v4 drop-in contract.

### Installation Methods

All currently shipped installation and acquisition paths are in scope for day 1.

That includes:

- setup and install surfaces already used by the module
- `uv`-based local execution flows
- Python packaging and installability where currently shipped
- repo-clone and bootstrap flows used in active development
- release-promotion outputs that land in the release repository

The rewrite must preserve parity across these paths so that installation method does not change the available prompt surface, integration behavior, or dependency completeness. A retained command is not considered shipped unless it is reachable through the supported install and bootstrap paths that users actually use.

### IDE & Agent Integration Surfaces

IDE and agent integrations are first-class supported surfaces, not hidden implementation details.

That includes support expectations for environments such as:

- GitHub Copilot
- Cursor
- Claude-oriented adapter surfaces
- generated or maintained adapters that expose Lens capabilities into agent tooling

The rewrite must preserve the compatibility and discoverability of these integrations where they are currently part of the shipped workflow. If a retained command depends on adapter generation, manifest data, or integration-specific routing, those dependencies are in scope and must remain operational on day 1.

### API Surface

The primary public API surface is the retained set of 17 published prompts and their frozen behavioral contracts.

However, the scoped technical interface for this rewrite is broader than the public prompt surface alone. It also includes:

- required internal skills retained behind those prompts
- required scripts and CLI entry points used by retained commands
- setup, installer, help, and manifest surfaces
- integration adapters and related support files
- dependency chains needed for prompt execution, routing, publication, and validation

In other words, all dependencies required to make the retained prompt surface real are also in scope. Internal dependencies are not automatically public APIs, but they are first-class rewrite obligations.

### Code Examples & Documentation

Day-1 documentation requirements include all of the following:

- installation and bootstrap guidance
- command examples for the retained prompt surface
- retained-command mapping and traceability references
- compatibility guidance for existing users
- troubleshooting and recovery examples
- documentation of integration surfaces and expected usage patterns

Because this is a stability-focused rewrite, examples should not only show happy-path usage. They should also show recovery-safe and compatibility-safe usage patterns where relevant.

### Migration Guide

The migration guidance for this rewrite is intentionally a compatibility guide, not a change-management playbook.

The day-1 migration section must explain:

- that the rewrite is a v4 drop-in
- that no schema migration is required
- that `upgrade` remains a no-op for v4-to-v4 scenarios
- which published prompts are retained
- which deprecated prompt stubs are removed from the published surface
- that `new-feature` is the canonical published command and `init-feature` is retained only as a deprecated compatibility alias or historical reference
- that `preflight` is the canonical published command and the legacy `onboard` prompt is removed from the public surface while its onboarding behavior remains behind retained internals
- that `new-project` is removed from the published surface with no direct day-1 replacement
- that retained dependencies remain present behind the supported command surface
- what existing users should expect when resuming features, sessions, and workflows after adopting the rewrite

The guide must reduce fear, not create work.

### Implementation Considerations

- Python-first implementation should remain consistent across scripts, validation tooling, and install surfaces.
- Public prompt parity must be verified alongside dependency parity.
- IDE and agent integration support must be validated as product behavior, not left as an implicit side effect.
- Documentation must be treated as a functional developer-tool requirement, not post-release polish.
- Old-codebase discovery outputs remain the baseline for command behavior, dependency expectations, and migration confidence.

## Project Scoping & Phased Development

### MVP Strategy & Philosophy

**MVP Approach:** Stability-first, compatibility-first full rewrite.

This MVP is not about proving a new feature. It is about proving Lens can be fully rewritten behind the retained 17-command surface without breaking active work, changing schemas, or leaving dependencies behind. The value of Phase 1 is safe cutover, operational trust, and parity you can defend.

**Resource Requirements:** Small core team.

The MVP assumes a small team that can cover:

- Python and workflow implementation
- Lens lifecycle and governance contract knowledge
- release and parity validation across prompts, dependencies, installers, help, manifests, and integrations

### MVP Feature Set (Phase 1)

**Core User Journeys Supported:**

- Nina: new-user onboarding through the retained command surface
- Evan: disrupted mid-feature recovery without schema changes
- Mara: maintainer-safe full rewrite with governed internal change
- Ian: integrator trust through public-surface and dependency-surface coherence

**Must-Have Capabilities:**

- full rewritten implementation behind all 17 retained published prompts
- full dependency parity across internal skills, scripts, manifests, installer surfaces, help output, adapters, and validation anchors
- zero schema changes across lifecycle, governance, and session artifacts
- zero broken existing features
- recovery-safe compatibility for in-flight work, partial workflow state, and dev sessions
- preserved special-case parity for `split-feature`, `discover`, `next`, publish-before-author flows, and finalizeplan atomicity
- day-1 documentation, examples, migration guidance, troubleshooting, and integration guidance
- release-ready artifact that can be promoted without prompt-surface or dependency-surface mismatch

### Post-MVP Features

**Phase 2 (Post-MVP):**

- broader automated parity coverage beyond the current focused regression anchors
- stronger observability for dependency drift, workflow regressions, and release confidence
- tooling and validation improvements that make future Lens changes safer and faster after the rewrite lands

**Phase 3 (Expansion):**

- new workflow capabilities beyond the retained surface
- richer self-governing workflow patterns built on the stable rewrite baseline
- expansion from parity-focused rewrite into additional productized workflow capabilities

### Risk Mitigation Strategy

**Technical Risks:** Dependency parity is the primary scoping limiter. The mitigation is to treat each retained prompt and its dependencies as a single release unit and gate cutover on parity evidence across prompts, installers, help, manifests, adapters, scripts, and tests.

**Market Risks:** The main product risk is that users experience the rewrite as churn rather than stability. The mitigation is a true drop-in release with zero schema change, zero broken features, and visible continuity for active work.

**Resource Risks:** A small team can get buried if the rewrite mixes parity work with feature expansion. The mitigation is to keep Phase 1 focused on the full retained-surface rewrite and defer non-essential expansion until the stable cutover is complete.

## Functional Requirements

### Command Surface & Workflow Entry

- FR1: Users can access the retained 17-command published workflow surface through supported Lens entry points.
- FR2: Users can validate workspace readiness before starting or resuming workflow activity.
- FR3: Users can create governance domains from the public workflow surface.
- FR4: Users can create services within existing governance domains.
- FR5: Users can create new features with canonical lifecycle identity and working context.
- FR6: Users can switch active feature context without unintended lifecycle mutation.
- FR7: Users can request the single next unblocked action for the active feature.
- FR8: Users can resolve applicable constitutional guidance for their current operating context.

### Lifecycle Planning & Delivery

- FR9: Users can create preplan artifacts for active features.
- FR10: Users can create businessplan artifacts for active features.
- FR11: Users can create techplan artifacts for active features.
- FR12: Users can consolidate planning outputs and advance planning completion through finalizeplan.
- FR13: Users can use the express planning path for eligible features.
- FR14: Users can execute governed development work in target repositories through the `dev` conductor after planning completion.
- FR15: Users can complete and archive a feature with retrospective and final documentation.
- FR16: Users can split an existing feature into a new first-class feature while preserving governance integrity and eligible work movement.

### Compatibility, Recovery & Lifecycle Continuity

- FR17: Existing users can continue active features after the rewrite without schema migration.
- FR18: Existing users can resume disrupted workflow state without recreating features or rewriting valid state.
- FR19: Existing users can preserve feature identity, branch topology, and governance path conventions through the rewrite.
- FR20: Existing users can preserve in-progress development session continuity through the rewrite.
- FR21: Maintainers can release the rewrite as a v4 drop-in for current Lens features.
- FR22: Users can invoke upgrade behavior that preserves no-op compatibility for current v4 features while retaining an explicit migration path for future schema divergence.

### Dependency, Installation & Integration Coherence

- FR23: Users can access the retained workflow surface through all supported installation and bootstrap paths.
- FR24: Integrators can rely on prompts, help, manifests, installer metadata, and setup surfaces to describe the same retained workflow surface.
- FR25: Users can use supported IDE and agent integrations to invoke or discover Lens workflows.
- FR26: Maintainers can keep required internal skills, scripts, and adapters available behind retained commands.
- FR27: Users can access examples, compatibility guidance, troubleshooting guidance, and migration guidance for the retained workflow surface.
- FR28: Integrators can treat all dependencies required by retained commands as supported release obligations.

### Governance, Publication & Release Safety

- FR29: Planning-phase users can publish reviewed phase artifacts to governance through the approved publication path.
- FR30: Planning-phase users can progress through lifecycle phases without direct governance writes outside approved exceptions.
- FR31: Maintainers can synchronize repository inventory between governance and local clones.
- FR32: Maintainers can preserve the defined governance exception behavior for direct repository-sync operations.
- FR33: Maintainers can promote release artifacts without prompt-surface or dependency-surface drift.
- FR34: Users can rely on clear authority boundaries between governance, control-repo planning artifacts, release artifacts, and target-repository code work.

### Traceability, Validation & Maintainer Operations

- FR35: Maintainers can trace each retained command end to end across prompt, skill, dependencies, outputs, and validation anchors.
- FR36: Maintainers can validate retained command behavior through attached regression or parity checks.
- FR37: Maintainers can centralize shared lifecycle behavior without changing retained user-facing outcomes.
- FR38: Small-team operators can assess release readiness using explicit parity and compatibility evidence.
- FR39: Maintainers can distinguish public prompt interfaces from retained internal modules when planning future changes.
- FR40: Maintainers can verify that every retained command and required dependency is present before release promotion.

## Non-Functional Requirements

### Reliability

- The rewrite must ship with 0 broken active features at release.
- The rewrite must require 0 schema migrations for current v4 Lens features and sessions.
- All 17 retained published prompts and their required dependencies must be present and operational on day 1.
- Retained commands must preserve recovery-safe behavior for disrupted but valid workflow state.
- Retained commands must preserve resumability for in-progress development sessions.
- Load-bearing retained workflows must remain backed by focused regression or parity validation before release promotion.
- Release promotion must be blocked if prompt-surface parity or dependency-surface parity is incomplete.

### Compatibility

- The rewrite must remain a v4 drop-in for existing Lens users.
- Existing lifecycle, governance, and session artifacts must remain readable without conversion.
- Frozen workflow contracts must remain intact across identity, branch topology, governance publication boundaries, handoff behavior, and retained special-case workflows.
- Deprecated public stubs may be removed only when their retained dependencies and supported replacement surface remain coherent.

### Integration

- Prompts, installer behavior, setup surfaces, help output, manifests, adapters, and validation inventory must resolve to the same retained command surface.
- Supported IDE and agent integration surfaces must remain discoverable and operational where they are part of the shipped workflow.
- Release artifacts must be promotable without divergence between public prompt contracts and dependency availability.

### Maintainability & Operability

- Each retained command must remain traceable end to end across prompt, skill, dependency chain, outputs, and validation anchors.
- Shared lifecycle behavior may be consolidated only when parity remains demonstrable.
- Small-team operators must be able to assess release readiness using explicit parity and compatibility evidence rather than manual intuition alone.
