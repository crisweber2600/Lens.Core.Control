---
feature: lens-dev-new-codebase-baseline
doc_type: architecture
status: draft
goal: "Define the technical architecture for the lens-work rewrite: 17-command stable surface with shared utilities and 100% backwards compatibility"
key_decisions:
  - Shared utility extraction replaces per-phase copy-pasted implementations
  - BMB-first implementation channel for all lens.core.src changes
  - Prompt-stub-to-script chain topology preserved verbatim across all 17 commands
  - All data contracts frozen at schema v4
open_questions: []
depends_on: [prd]
blocks: []
updated_at: 2026-04-22T12:00:00Z
---

# Architecture — lens-work Rewrite: 17-Command Stable Surface

**Author:** Winston (Architect)
**Feature:** lens-dev-new-codebase-baseline
**Date:** 2026-04-22

> References: [PRD — lens-work Rewrite: 17-Command Stable Surface](../prd.md)

## Overview

The `lens-work` rewrite replaces a 54-stub command surface with a 17-command stable surface while preserving 100% of user-observable behavior. The architectural problem is not a greenfield design challenge — it is a constrained brownfield rewrite. The structure of the new codebase is defined by the rewrite contracts in this document and the supporting PRD — not derived from the old codebase. What changes is:

1. The number of published `.github/prompts/` stubs (54 → 17)
2. Three cross-phase copy-pasted patterns extracted into shared utilities
3. Internal skill implementations reorganized to reflect the explicit retained-vs-removed inventory

Nothing changes about `feature.yaml`, `feature-index.yaml`, branch topology, lifecycle schema, governance path conventions, or `dev-session.yaml`. This architecture is the technical answer to the PRD's preservation mandate.

The implementation channel for all source-level changes is **BMB-first**: every edit to `lens.core.src` is routed through `lens.core/_bmad/bmb`, not applied directly. This is a service constitution requirement and a governance axiom for the `lens-dev/new-codebase` service.

---

## System Design

### 1. Module Topology

The `lens-work` module lives at `lens.core/_bmad/lens-work/`. The rewrite preserves this root and all relative path conventions. The changes are contained to which skills and stub files exist, not where the module lives. All topology decisions are derived from the rewrite contracts in this document and the PRD. Old-codebase path conventions may be consulted as verification references but do not govern the implementation.

```
lens.core/_bmad/lens-work/
├── skills/
│   ├── bmad-lens-preplan/          # retained — full track
│   ├── bmad-lens-businessplan/     # retained — full track
│   ├── bmad-lens-techplan/         # retained — full track
│   ├── bmad-lens-finalizeplan/     # retained — full track
│   ├── bmad-lens-expressplan/      # retained — express track
│   ├── bmad-lens-dev/              # retained — execution
│   ├── bmad-lens-complete/         # retained — closure
│   ├── bmad-lens-switch/           # retained — navigation
│   ├── bmad-lens-next/             # retained — navigation
│   ├── bmad-lens-init-feature/     # retained — backing skill for new-feature
│   ├── bmad-lens-discover/         # retained — backing skill for discover
│   ├── bmad-lens-constitution/     # retained — governance
│   ├── bmad-lens-git-orchestration/ # retained — git workflow orchestration
│   ├── bmad-lens-upgrade/          # retained — maintenance
│   ├── bmad-lens-split-feature/    # retained — published; feature reshaping
│   ├── bmad-lens-bmad-skill/       # retained — wrapper delegation router
│   ├── bmad-lens-batch/            # retained — shared batch 2-pass contract
│   ├── bmad-lens-adversarial-review/ # retained — shared review gate
│   └── [internal-only skills]      # stubs removed, skill directories retained
│                                   #   where required by retained commands
├── scripts/
│   ├── light-preflight.py          # prompt-start gate — frozen interface
│   ├── validate-phase-artifacts.py # review-ready fast path — single implementation
│   ├── preflight.py                # workspace/onboarding validation
│   └── [per-skill scripts]         # owned by their skill directories
├── agents/
│   └── lens.agent.md               # shell menu — exactly 17 commands
├── module-help.csv                  # discovery surface — exactly 17 commands
├── lifecycle.yaml                   # schema v4 — frozen
├── module.yaml
└── bmadconfig.yaml
```

The `agents/lens.agent.md` shell menu and `module-help.csv` are kept in sync with the 17-command surface. Adding or removing a command requires updating both surfaces plus the stub directory simultaneously.

### 2. Command Resolution Chain

Every published command follows an identical 3-hop chain. This chain is invariant across all 17 commands:

```
.github/prompts/lens-{command}.prompt.md       (stub — user entry point)
  → lens.core/_bmad/lens-work/prompts/lens-{command}.prompt.md  (full prompt — loads skill)
    → skills/bmad-lens-{command}/SKILL.md  (owning skill — orchestrates execution)
      → scripts/ and sub-skill delegates as needed
```

`light-preflight.py` fires at the stub level before the redirect on every command invocation. Its exit-code interface is frozen and may not be altered.

The stub for `new-feature` resolves to `bmad-lens-init-feature` (not a skill named `bmad-lens-new-feature`). The public alias mapping is owned in the stub and documented in the skill.

### 3. Shared Utilities

Three patterns currently copy-pasted across phase skills are extracted into single implementations. This is the primary structural change in the rewrite.

#### 3.1 Review-Ready Fast Path

**Single implementation:** `scripts/validate-phase-artifacts.py`

This script already exists and is the authoritative implementation. Per-phase inline review-ready logic in SKILL.md files is replaced by a single invocation pattern:

```
uv run scripts/validate-phase-artifacts.py \
  --phase {phase} --contract review-ready \
  --lifecycle-path {lifecycle-path} \
  --docs-root {staged-docs-path} --json
```

All phase skills (preplan, businessplan, techplan, expressplan) delegate to this script. No phase skill duplicates this logic.

#### 3.2 Batch Mode 2-Pass Contract

**Single implementation:** `skills/bmad-lens-batch/`

The 2-pass contract — pass 1 writes `{phase}-batch-input.md` and stops; pass 2 resumes with answers as pre-approved context — is owned by `bmad-lens-batch`. Phase skills delegate via:

```
bmad-lens-batch --target {phase}
```

No phase skill contains inline batch-intake logic. This replaces the copy-pasted `if mode is batch and batch_resume_context is absent...` blocks in each SKILL.md.

#### 3.3 Publish-Before-Author Entry Hook

**Single implementation:** `skills/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py` (`publish-to-governance` subcommand)

The contract — publish reviewed prior-phase artifacts to governance before authoring new-phase outputs — is enforced through a single CLI call:

```
uv run .../git-orchestration-ops.py publish-to-governance \
  --phase {prior-phase} --feature-id {featureId} \
  --governance-repo {path} --control-repo {path}
```

Used by businessplan, techplan, finalizeplan, and dev. No direct governance file writes from phase skills. `discover` retains its explicit auto-commit-to-main exception per PRD R3.

### 4. Rewrite Dependency Layers

The rewrite unit for a retained command is not a single prompt file. Every surviving command carries a dependency stack that must stay coherent across six layers. A command is not considered rewritten until all impacted layers below are either rewritten or explicitly retained unchanged.

| Layer | Rewrite surface | Load-bearing dependency |
|---|---|---|
| Entry stub | `.github/prompts/lens-{command}.prompt.md` | `light-preflight.py` exit-code contract and public command alias |
| Release prompt | `lens.core/_bmad/lens-work/prompts/{command}.md` | Redirect path from public command to owning skill |
| Owning skill | `skills/bmad-lens-*/SKILL.md` | Lifecycle orchestration, delegation rules, and user-facing behavior |
| Script / ops layer | `scripts/*.py` or `skills/**/scripts/*-ops.py` | Actual file mutation, git orchestration, state reads, and governance I/O |
| Shared contracts | `lifecycle.yaml`, `feature.yaml`, `feature-index.yaml`, `dev-session.yaml`, constitutions, `repo-inventory.yaml` | Frozen schemas, branch topology, and governance boundaries |
| Discovery / install surfaces | `module-help.csv`, `agents/lens.agent.md`, `setup.py`, manifests, IDE adapters | The command must remain installable, discoverable, and named consistently everywhere |

This is the key rewrite implication from the PRD and research corpus: the new codebase is not only rewriting phase conductors. It is rewriting the dependency graph behind each retained command, including internal skills that lose their public stubs but remain required at runtime.

### 5. Retained Command Dependency Rewrite Inventory

The tables below turn the retained-command traceability work into an implementation inventory. They answer the architectural question the rewrite must solve: if a public command survives, what else has to survive and be rewritten with it?

#### 5.1 Scaffolding and Navigation Commands

| Command | Owning chain | Must-retain dependencies | Rewrite-critical implication |
|---|---|---|---|
| `preflight` | `bmad-lens-onboard` -> `preflight.py` / `light-preflight.py` | `bmadconfig.yaml`, `lifecycle.yaml`, governance repo shape, tooling checks | Consolidate internal preflight behavior if desired, but keep the stub-start `light-preflight.py` contract and zero feature-state mutation semantics unchanged |
| `new-domain` | `bmad-lens-init-feature` -> `init-feature-ops.py create-domain` | domain scaffold templates, `bmad-lens-constitution`, governance path rules | The alias may change internally, but the rewrite must preserve domain slugging, constitution scaffold creation, and stable domain path naming |
| `new-service` | `bmad-lens-init-feature` -> `init-feature-ops.py create-service` | domain existence checks, service scaffold templates, inherited constitution rules | Service creation cannot be rewritten in isolation from constitution inheritance and governance path validation |
| `new-feature` | `bmad-lens-init-feature` -> `init-feature-ops.py create` -> `bmad-lens-git-orchestration` -> optional `bmad-lens-target-repo` | frozen `featureId` formula, `feature.yaml`, `feature-index.yaml`, 2-branch topology, target repo inventory | This is the identity root of the system. Rewrite only after the featureId, branch, and governance path contracts are frozen and test-backed |
| `switch` | `bmad-lens-switch` -> `switch-ops.py` | `feature-index.yaml`, feature summaries, constitution load, session context conventions | Must remain read-only. Any rewrite that adds caching or shortcuts cannot introduce git-visible state changes |
| `next` | `bmad-lens-next` -> `next-ops.py suggest` -> delegated owning skill | `lifecycle.yaml`, `feature.yaml`, blocker rules, next-handoff pre-confirmed contract | `next` depends on every downstream phase entry contract. It should be rewritten only after phase command entry semantics are stable |

#### 5.2 Planning Commands

| Command | Owning chain | Must-retain dependencies | Rewrite-critical implication |
|---|---|---|---|
| `preplan` | `bmad-lens-preplan` -> `bmad-lens-bmad-skill` -> brainstorming / research / product-brief flows -> adversarial review | `validate-phase-artifacts.py`, `bmad-lens-batch`, `bmad-lens-adversarial-review`, `bmad-lens-constitution`, `bmad-lens-git-orchestration`, `bmad-lens-feature-yaml` | Rewrite as a thin conductor. Shared batch, review-ready, and review-gate behavior must be extracted rather than recopied |
| `businessplan` | `bmad-lens-businessplan` -> `publish-to-governance --phase preplan` -> `bmad-lens-bmad-skill` -> `bmad-create-prd` / `bmad-create-ux-design` -> adversarial review | `bmad-lens-git-orchestration`, `bmad-lens-bmad-skill`, `bmad-lens-batch`, `bmad-lens-adversarial-review`, `bmad-lens-constitution` | Cannot be rewritten independently from the publish-before-author hook. Direct governance writes remain prohibited |
| `techplan` | `bmad-lens-techplan` -> `publish-to-governance --phase businessplan` -> `bmad-lens-bmad-skill` -> `bmad-create-architecture` -> adversarial review | `prd.md`, `ux-design.md`, `validate-phase-artifacts.py`, `bmad-lens-git-orchestration`, `bmad-lens-batch`, `bmad-lens-adversarial-review`, `bmad-lens-constitution` | The architecture generator, publish-entry hook, and phase completion behavior are one dependency bundle and must be rewritten together |
| `finalizeplan` | `bmad-lens-finalizeplan` -> publish techplan artifacts -> adversarial review -> `bmad-lens-git-orchestration` commit/push/PR -> `bmad-lens-bmad-skill` bundle generation | `bmad-lens-git-orchestration`, `bmad-lens-adversarial-review`, `bmad-lens-bmad-skill`, `bmad-lens-feature-yaml`, milestone updates, plan/final PR topology | Highest dependency fan-out in the planning family. Rewrite last among planning commands after bundle generators and PR primitives are stable |
| `expressplan` | `bmad-lens-expressplan` -> `bmad-lens-bmad-skill` -> `bmad-lens-quickplan` -> adversarial review -> finalizeplan bundle reuse | `bmad-lens-quickplan`, `bmad-lens-adversarial-review`, `bmad-lens-finalizeplan`, `bmad-lens-git-orchestration`, express-track gating in `feature.yaml` | The silent-break risk is internal QuickPlan removal. ExpressPlan survives only if the deprecated prompt is removed but the QuickPlan skill remains callable |

#### 5.3 Execution, Closure, Governance, and Maintenance Commands

| Command | Owning chain | Must-retain dependencies | Rewrite-critical implication |
|---|---|---|---|
| `dev` | `bmad-lens-dev` -> publish finalizeplan artifacts -> `prepare-dev-branch` -> constitution load -> subagent execution -> review -> final PR | `bmad-lens-git-orchestration`, `dev-session.yaml`, target repo inventory, `bmad-lens-adversarial-review`, `bmad-lens-constitution`, `bmad-lens-feature-yaml` | Rewrite must preserve target-repo-only code writes, per-task commits, resumability, and the final single-PR model |
| `complete` | `bmad-lens-complete` -> `bmad-lens-retrospective` -> `bmad-lens-document-project` -> archive / finalize ops | `feature.yaml`, `feature-index.yaml`, retrospective outputs, document-project outputs, terminal archive semantics | Closure remains a strict ordered flow: document before archive, then transition to terminal archived state |
| `split-feature` | `bmad-lens-split-feature` -> `split-feature-ops.py validate-split` -> `create-split-feature` -> optional `move-stories` | source and target `feature.yaml`, `feature-index.yaml`, story files, summary stub creation, validate-first contract | The three-subcommand script surface is load-bearing. Prompt reduction cannot collapse this into a single opaque implementation |
| `constitution` | `bmad-lens-constitution` -> constitution resolution ops | org/domain/service/repo constitutions, additive merge rules, `gate_mode`, missing-level fallback behavior | The rewrite must treat partial hierarchies as valid inputs. The current org-level hard-fail is evidence that dependency resolution needs to be rewritten, not preserved blindly |
| `discover` | `bmad-lens-discover` -> discover sync ops | `repo-inventory.yaml`, user profile, `target_projects_path`, local clone scan, governance-main auto-commit exception | This command has its own write boundary and cannot be folded into publish-to-governance. Keep the explicit inventory-sync exception path intact |
| `upgrade` | `bmad-lens-upgrade` -> migration detection -> `bmad-lens-migrate` when needed | `lifecycle.yaml` migration table, `module.yaml`, schema version contract, `[LENS:UPGRADE]` commit semantics | Preserve v4 -> v4 no-op behavior. Only the migration router should change when the schema actually changes |

### 6. Rewrite Sequencing by Dependency Tier

The dependency inventory above implies the rewrite order. Rewriting commands in a different order increases the chance of false parity because higher-level conductors will end up targeting unstable internal modules.

1. Cross-cutting entry and discovery surfaces: prompt stubs, release prompts, `module-help.csv`, `agents/lens.agent.md`, install/manifests/adapters.
2. Shared runtime primitives: `light-preflight.py`, `validate-phase-artifacts.py`, `bmad-lens-feature-yaml`, `bmad-lens-git-orchestration`, `bmad-lens-constitution`, `bmad-lens-batch`, `bmad-lens-adversarial-review`, `bmad-lens-bmad-skill`.
3. Identity and navigation roots: `preflight`, `new-domain`, `new-service`, `new-feature`, `switch`, `next`.
4. Planning conductors: `preplan`, `businessplan`, `techplan`, `finalizeplan`, `expressplan`.
5. Execution and terminal workflows: `dev`, `complete`, `split-feature`, `discover`, `upgrade`.

**Prerequisite constraint (post-TechPlan review):** `bmad-lens-constitution` has a known org-level hard-fail bug. WP-15 must deliver the constitution resolution bug fix **before** any planning conductor work package (WP-07 through WP-11) begins. The `constitution` public command rewrite is tier 5, but the shared resolution engine it provides is called by all planning conductors during the rewrite. This is an intra-tier ordering constraint: WP-15 bug fix precedes planning tier execution even though WP-15's full work package sits in tier 5.

This sequencing is part of the architecture, not just implementation advice. It is how the rewrite avoids treating public commands as isolated wrappers when the real work happens in the internal dependency graph.

### 7. Internal Skill Retention Matrix

This matrix is the concrete keep/remove decision set requested by ADR-004. It is derived from the current `skills/` and `prompts/` inventories in `lens.core/_bmad/lens-work/`, the retained 17-command surface defined in the PRD, and the rewrite contract matrix in this architecture. Old-codebase discovery artifacts (`dependency-mapping.md`, `deep-dive-lens-work-module.md`) are approved verification references for outcome checks — they are not the derivation source for this matrix.

Decision rule:

1. If the rewrite contract matrix shows a retained command requiring the skill as a runtime dependency, remove the public stub but keep the skill as an internal runtime dependency.
2. If the skill is not on any retained-command dependency path in the rewrite contract, remove it from the stable-surface rewrite scope.
3. If a skill currently carries discovery or theme behavior that is still wanted, extract that behavior into a non-command surface before deleting the skill.

#### 7.1 Keep as Internal Runtime Dependencies

| Skill | Current prompt surface | Dependency-mapping evidence | Decision | Why |
|---|---|---|---|---|
| `bmad-lens-onboard` | `lens-onboard.prompt.md` | `/onboard` delegates to `preflight.py`; `bmad-lens-onboard` owns the preflight stop/go path and profile template loading | Keep skill, remove public stub | `preflight` remains public; `onboard` becomes the internal engine behind it |
| `bmad-lens-batch` | `lens-batch.prompt.md` | Listed in the orchestration layer; batch 2-pass contract remains shared across planning flows | Keep skill, remove public stub | Planning parity requires one shared batch contract instead of per-phase copies |
| `bmad-lens-bmad-skill` | `lens-bmad-*.prompt.md` family | `/expressplan` delegates through `bmad-lens-bmad-skill`; planning wrappers route through it to BMAD generators | Keep router, remove wrapper prompts | Wrapper prompts leave the public surface, but the router remains load-bearing for planning delegation |
| `bmad-lens-quickplan` | `lens-quickplan.prompt.md` | `/expressplan` -> `bmad-lens-bmad-skill` -> `bmad-lens-quickplan` -> `quickplan-ops.py` | Keep skill, remove public stub | ExpressPlan breaks silently if QuickPlan is deleted with its prompt |
| `bmad-lens-adversarial-review` | `lens-adversarial-review.prompt.md` | Called by phase conductors; reads `completion_review` and constitution-gated party-mode rules | Keep skill, remove public stub | Review is mandatory lifecycle infrastructure, not a day-1 public command |
| `bmad-lens-feature-yaml` | `lens-feature-yaml.prompt.md` | Shared resource map and interaction matrix show full `feature.yaml` CRUD and phase transitions | Keep skill, remove public stub | Central schema enforcement must stay reusable across the retained graph |
| `bmad-lens-git-orchestration` | `lens-git-orchestration.prompt.md` | Branch creation, publish-to-governance, prepare-dev-branch, finalizeplan PR flow, dev branch prep | Keep skill, remove public stub | It is the shared git/governance mutation primitive behind multiple retained commands |
| `bmad-lens-git-state` | `lens-git-state.prompt.md` | `/dev` depends on `bmad-lens-git-state`; shared branch discrepancy and state-reading contract | Keep skill, remove public stub | Dev still needs a read-only git truth source even when the standalone command is removed |
| `bmad-lens-target-repo` | `lens-target-repo.prompt.md` | Shared resource map and IPO catalog show target repo provisioning feeding `feature.yaml` and `repo-inventory.yaml` | Keep skill, remove public stub | `new-feature` and `dev` still need target repo provisioning and idempotent repo registration |
| `bmad-lens-document-project` | `lens-bmad-document-project.prompt.md` | `/complete` delegates to `bmad-lens-document-project` before archive | Keep skill, remove public stub | Closeout parity depends on final documentation before archive |
| `bmad-lens-retrospective` | `lens-retrospective.prompt.md` | `/complete` calls `bmad-lens-retrospective` before final docs and archive | Keep skill, remove public stub | Retrospective-before-archive remains a load-bearing terminal workflow step |
| `bmad-lens-migrate` | `lens-migrate.prompt.md` | `/upgrade` routes to `migrate-ops.py` for actual migrations and verification | Keep skill, remove public stub | Upgrade keeps the migration engine even while the direct migrate surface disappears |

#### 7.2 Remove from the Stable-Surface Rewrite Scope

| Skill | Dependency-mapping status | Decision | Rewrite note |
|---|---|---|---|
| `bmad-lens-help` | Not required by any retained command in the rewrite contract | Remove | Discovery responsibility moves to `module-help.csv`, install surfaces, and `lens.agent.md` |
| `bmad-lens-theme` | Not required by any retained command in the rewrite contract | Extract assets if still needed, then remove command skill | Do not keep a standalone command just to preserve theme overlays |
| `bmad-lens-approval-status` | Not required by retained command paths | Remove | Promotion diagnostics are not part of day-1 parity for the stable surface |
| `bmad-lens-audit` | Not required by retained command paths | Remove | Compliance dashboarding becomes a follow-on feature, not a parity blocker |
| `bmad-lens-dashboard` | Reads shared data but is not called by retained commands | Remove | Reporting is not on the day-1 runtime dependency path |
| `bmad-lens-devproposal` | Superseded by FinalizePlan in the v4 lifecycle | Remove | The old phase family is explicitly replaced |
| `bmad-lens-lessons` | Not on retained-command dependency paths | Remove | Lessons capture is out of the stable-surface rewrite scope |
| `bmad-lens-log-problem` | Writes governance state but is not needed by retained commands | Remove | Readers of historical `open_problems` state remain; the standalone authoring surface does not |
| `bmad-lens-module-management` | Not on retained-command dependency paths | Remove | Version/status diagnostics fold into upgrade and release validation |
| `bmad-lens-move-feature` | Not on retained-command dependency paths | Remove | Feature relocation is a separate capability, not part of day-1 parity |
| `bmad-lens-pause-resume` | Writes `feature.yaml` state but is not on retained-command paths | Remove | Retained readers must still tolerate historical paused state, but no rewrite package is allocated to the standalone command |
| `bmad-lens-profile` | Not on retained-command dependency paths | Remove | Profile setup folds into preflight/onboarding support rather than a public command |
| `bmad-lens-rollback` | Not on retained-command dependency paths | Remove | Rollback semantics need a separate design if reintroduced later |
| `bmad-lens-sensing` | Advisory overlap detection only; not called by retained commands | Remove | Cross-initiative sensing is deferred beyond the parity release |
| `bmad-lens-sprintplan` | Superseded by FinalizePlan bundle generation | Remove | Sprint status generation remains, but the old standalone command does not |

The removal decisions above apply to the stable-surface rewrite scope, not to schema compatibility. Retained commands must continue to read historical fields such as paused state or open problem counts where those fields already exist in governance data.

### 8. Command Work Packages, Owners, and Parity Gates

The dependency inventory becomes executable only when each retained command is assigned a rewrite package with a single owner lane and a mandatory parity gate. Owners below are workstream owners, not necessarily individual people. No package is considered done until its parity gate passes.

#### 8.1 Scaffolding and Navigation Work Packages

| Package | Command | Owner lane | Rewrite scope | Parity gate |
|---|---|---|---|---|
| `WP-01` | `preflight` | Workspace Runtime | Preserve `light-preflight.py` entry semantics, fold `onboard` behind `preflight`, and keep zero lifecycle mutation | `test-setup-control-repo.py` plus prompt-start exit-code regression |
| `WP-02` | `new-domain` | Governance Identity | Keep `create-domain` inside `bmad-lens-init-feature`; preserve domain scaffold naming and constitution bootstrap | Domain scaffold regression against current init-feature behavior |
| `WP-03` | `new-service` | Governance Identity | Keep `create-service` inside `bmad-lens-init-feature`; preserve domain existence checks and service inheritance | Service inheritance/path regression against current init-feature behavior |
| `WP-04` | `new-feature` | Governance Identity | Freeze feature identity, 2-branch creation, `feature-index.yaml` registration, and target repo handoff | `test-init-feature-ops.py` plus `test-git-orchestration-ops.py` branch parity |
| `WP-05` | `switch` | Navigation and Session | Preserve feature discovery, read-only context loading, and no-write semantics | Switch no-write regression with branch and file mutation assertions |
| `WP-06` | `next` | Navigation and Session | Preserve blocker-first single-choice routing and pre-confirmed handoff into downstream skills | `test-next-ops.py` plus delegated-no-reconfirm regression |

#### 8.2 Planning Work Packages

| Package | Command | Owner lane | Rewrite scope | Parity gate |
|---|---|---|---|---|
| `WP-07` | `preplan` | Planning Orchestration | Rewrite as a thin conductor over `bmad-lens-batch`, `validate-phase-artifacts.py`, `bmad-lens-bmad-skill`, and adversarial review | Review-ready fast-path regression, batch pass-1/pass-2 regression, brainstorm-first sequencing regression |
| `WP-08` | `businessplan` | Planning Orchestration | Preserve publish-before-author ordering, PRD/UX delegation, and no direct governance writes | Publish-before-author regression, PRD/UX wrapper-equivalence checks, governance-write audit |
| `WP-09` | `techplan` | Planning Orchestration | Preserve businessplan publication, architecture generation, PRD reference rule, and adversarial review gate | Architecture `must_reference` regression plus publish-before-author parity |
| `WP-10` | `finalizeplan` | Planning Orchestration | Preserve the 3-step contract, plan PR topology, downstream bundle generation, and `dev-ready` milestone stamping | Finalizeplan step-order regression, PR-topology regression, bundle-output parity |
| `WP-11` | `expressplan` | Planning Orchestration | Preserve express-track gate, internal QuickPlan delegation, review hard-stop, and finalizeplan bundle reuse | Express-track gating regression, QuickPlan retention regression, hard-stop review regression |

#### 8.3 Execution, Governance, and Maintenance Work Packages

| Package | Command | Owner lane | Rewrite scope | Parity gate |
|---|---|---|---|---|
| `WP-12` | `dev` | Execution Runtime | Preserve target-repo-only code writes, per-task commits, dev branch prep, constitution load, and `dev-session.yaml` resumability | `dev-session.yaml` compatibility regression, per-task commit regression, target-repo-only write regression |
| `WP-13` | `complete` | Closeout | Preserve retrospective -> final documentation -> atomic archive ordering | Archive atomicity regression plus retrospective/document-before-archive regression |
| `WP-14` | `split-feature` | Feature Reshaping | Preserve validate-first split, in-progress blockers, new feature creation, summary stub, and optional story moves | `test-split-feature-ops.py` plus split dry-run regression |
| `WP-15` | `constitution` | Governance Rules | **Bug fix prerequisite: must deliver org-level hard-fail fix before WP-07 begins (see §6 prerequisite constraint).** Rewrite constitution resolution to preserve additive merge semantics while tolerating partial hierarchy gaps | Partial-hierarchy regression with missing org-level constitution and additive-merge assertions |
| `WP-16` | `discover` | Inventory Sync | Preserve repo-inventory synchronization and the explicit governance-main auto-commit exception | Bidirectional sync regression plus governance-main auto-commit regression |
| `WP-17` | `upgrade` | Compatibility and Migration | Preserve v4 -> v4 no-op behavior while retaining explicit routing into `migrate-ops.py` for real schema changes | `test-upgrade-ops.py` plus migration-router regression |

The work-package graph follows the old dependency mapping directly: identity and navigation must stabilize before planning conductors, planning conductors must stabilize before `dev`, and terminal/governance commands must preserve their special write boundaries rather than being folded into generic utilities.

### 9. Deprecated Stub Removal Strategy

The rewrite removes 37 `.github/prompts/` stubs (54 → 17). The removal decision for each deprecated stub has two independent dimensions:

| Dimension | Options |
|---|---|
| Stub | Deleted from `.github/prompts/` |
| Owning skill — required by retained command | Retained in `skills/` with no stub |
| Owning skill — not required by any retained command | Removed from `skills/` |

A **retained-vs-removed skill inventory** is produced as a first-class TechPlan/FinalizePlan deliverable and reviewed before any deletion occurs. No skill directory is deleted until the inventory is confirmed. Deleting a skill that is a silent dependency of a retained command breaks the resolution chain at runtime.

**Post-upgrade stub behavior:** Removed stubs are hard-deleted with no redirect or deprecation shim. After upgrade, invoking a removed command (e.g. `/pause`, `/theme`) produces no output — the stub simply does not exist. Users who relied on removed commands must be informed at release time via upgrade notes.

### 10. Implementation Channel: Lens Workflow Dogfooding

All source-level modifications to `lens.core.src` during this rewrite are authored through the Lens workflow process (init-feature → plan → dev → complete). Direct edits that bypass this workflow are a `lens-dev/new-codebase` service constitution violation and will be flagged in compliance review.

The BMB module (`lens.core/_bmad/bmb`) is the **bmad workflow builder** — it is a tool for authoring and validating bmad workflow skills and agents. BMB can be used where appropriate for creating or editing skill files, but it is not a source-promotion pipeline for all `lens.core.src` changes. The governance constraint here is Lens dogfooding: every source change follows the same Lens workflow that users follow.

This is not tooling overhead — it is the proof that Lens can govern its own evolution by following the same process it enforces for users.

---

## Data Model

All data contracts are frozen at schema v4. No field additions, removals, or renames are permitted in any of the following. This is non-negotiable per PRD G2 and G5.

### feature.yaml (frozen, schema v4)

```yaml
featureId: {domain}-{service}-{featureSlug}   # identity formula — immutable
featureSlug: {slug}
domain: {domain}
service: {service}
phase: {lifecycle-phase}
track: {track}
milestones:
  businessplan: null | {timestamp}
  techplan: null | {timestamp}
  finalizeplan: null | {timestamp}
  dev-ready: null | {timestamp}
  dev-complete: null | {timestamp}
team:
  - username: {user}
    role: {role}
dependencies:
  depends_on: []
  depended_by: []
target_repos: []
docs:
  path: docs/{domain}/{service}/{featureId}
  governance_docs_path: features/{domain}/{service}/{featureId}/docs
phase_transitions:
  - phase: {phase}
    timestamp: {ISO}
    user: {username}
```

### feature-index.yaml (frozen)

Structure and field names unchanged. The rewrite must not alter the index entry shape for any existing or new feature. Existing entries created before the rewrite must remain valid after.

### dev-session.yaml (frozen)

All checkpoint fields preserved verbatim. The rewrite must not add, rename, or reorder any field. In-progress dev sessions created before the rewrite must be resumable with zero migration after upgrade.

### Artifact File Conventions (frozen)

All planning artifacts resolve to a predictable filename under the staged docs path. No renames or path changes:

| Artifact key | Filename | Path |
|---|---|---|
| `product-brief` | `product-brief.md` | `docs/{domain}/{service}/{featureId}/` |
| `prd` | `prd.md` | `docs/{domain}/{service}/{featureId}/` |
| `ux-design` | `ux-design.md` | `docs/{domain}/{service}/{featureId}/` |
| `architecture` | `architecture.md` | `docs/{domain}/{service}/{featureId}/` |
| `brainstorm` | `brainstorm.md` | `docs/{domain}/{service}/{featureId}/` |
| `research` | `research.md` | `docs/{domain}/{service}/{featureId}/` |

**Track qualifier:** `ux-design.md` applies to the `full` track only. The `tech-change` and `express` tracks do not produce a `ux-design` artifact.

---

## API Design

### light-preflight.py (frozen exit-code interface)

```
uv run scripts/light-preflight.py
  Exit 0  → workspace valid; stub continues to full prompt
  Exit 1  → validation failure; stub halts with no lifecycle change
```

No arguments. No output format changes. Called identically in every prompt stub, before the redirect.

### validate-phase-artifacts.py (review-ready gate)

```
uv run scripts/validate-phase-artifacts.py \
  --phase {preplan|businessplan|techplan|finalizeplan|expressplan} \
  --contract review-ready \
  --lifecycle-path {path}/lifecycle.yaml \
  --docs-root {staged-docs-path} \
  [--json]

  Exit 0  → {status: "pass"} — all required artifacts present
  Exit 1  → {status: "fail", missing: [...], failure_reason: "..."} — artifacts missing
```

Single source of truth across all phases. No per-phase duplicate implementations.

### git-orchestration-ops.py: publish-to-governance

```
uv run skills/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py \
  publish-to-governance \
  --phase {prior-phase} \
  --feature-id {featureId} \
  --governance-repo {governance-repo-path} \
  --control-repo {control-repo-path} \
  [--artifact {specific-artifact}] \
  [--dry-run]
```

The only valid governance write path for planning-phase artifacts. Returns JSON with `copied_from`, `published_files`, and `missing_artifacts`. `discover`'s auto-commit-to-main behavior is a separate code path and is explicitly exempt per PRD R3.

### bmad-lens-batch delegation contract

```
Phase skill delegates to:  bmad-lens-batch --target {phase}

  Pass 1 (batch_resume_context absent):
    → writes {phase}-batch-input.md with discovery questions
    → stops; returns control to user

  Pass 2 (batch_resume_context present):
    → loads answered batch input as pre-approved context
    → proceeds with phase execution; no separate confirmation prompt
```

### Published stub contract (frozen for all 17 commands)

```
.github/prompts/{command}.prompt.md:
  1. Invoke light-preflight.py; exit on failure
  2. Load full prompt from lens.core/_bmad/lens-work/prompts/{command}.md
  3. Contain no lifecycle logic inline
```

---

## Architecture Decision Records

### ADR-001 — Shared Utility Extraction via Delegation
**Decision:** Extract review-ready fast path, batch 2-pass contract, and publish-entry hook into single script/skill implementations. Phase SKILL.md files delegate to these instead of duplicating logic inline.
**Rationale:** Copy-pasted patterns diverge silently over time. A fix applied to one phase's inline implementation does not propagate. Single implementations are the only reliable mechanism for cross-phase behavioral consistency.
**Consequence:** Phase skills become structurally thinner orchestrators. All cross-phase behavioral guarantees are owned at the shared utility level.

### ADR-002 — Frozen Data Contracts
**Decision:** `feature.yaml`, `feature-index.yaml`, and `dev-session.yaml` schemas are read-only for the duration of this rewrite. No field changes.
**Rationale:** Any schema field change during a brownfield rewrite creates migration risk for existing users' in-progress features and dev sessions. Stability is the primary product goal; schema stability is a prerequisite.
**Consequence:** All architectural goals must be achievable within the current v4 schema. Any future schema evolution is a separate initiative requiring an explicit upgrade path.

### ADR-003 — Lens Workflow as Implementation Channel
**Decision:** All modifications to `lens.core.src` are authored through the Lens workflow process (init-feature → plan → dev → complete), not applied directly.
**Rationale:** Service constitution requirement for `lens-dev/new-codebase`. Direct edits bypass governance and make the rewrite non-compliant with the process it is rebuilding. Dogfooding the workflow is a stated product goal (PRD Executive Summary).
**Consequence:** Every source change is executed as a Lens dev session. The BMB module (bmad workflow builder) is available for authoring workflow skill files but is not the sole implementation channel — it is a tool within the process, not the process itself.

### ADR-004 — Retained-vs-Removed Inventory as First-Class Deliverable
**Decision:** A definitive inventory distinguishing retained-as-internal from fully-removed `bmad-lens-*` skills is produced during TechPlan and reviewed before any deletion.
**Rationale:** Silently deleting a skill that is a dependency of a retained command breaks the resolution chain at runtime with no compile-time signal. The inventory is the only mechanism to catch this before it becomes a regression.
**Consequence:** No skill directory is deleted until the inventory is confirmed. The §7 inventory is confirmed as of the TechPlan adversarial review (2026-04-23). Any post-TechPlan modifications to keep/remove decisions require a FinalizePlan gate item.

### ADR-005 — Stub Surface and Help Surface Must Stay in Sync
**Decision:** Any addition or removal of a published command requires simultaneous updates to `.github/prompts/`, `agents/lens.agent.md`, and `module-help.csv`.
**Rationale:** Command discovery is split across three surfaces. Updating only one creates stale discovery that misleads users into invoking commands that no longer exist or missing commands that do.
**Consequence:** Command surface changes are atomic across all three surfaces, enforced during code review.

---

## Validation Strategy

Regression anchors per PRD Table 3.2. These tests must pass unchanged before the rewrite is considered complete:

| Command | Regression anchor | Pass condition |
|---|---|---|
| `new-feature` | `test-init-feature-ops.py` | featureId, index entry, and 2-branch topology match old behavior |
| `switch` | switch no-write regression | Context load produces no commits, no file mutations |
| `next` | `test-next-ops.py` | Single-choice, blocker-first, auto-delegate without re-confirmation |
| `preflight` | `test-setup-control-repo.py` | Workspace and onboarding checks produce identical outcomes |
| `split-feature` | `test-split-feature-ops.py` | Validate-first, in-progress story blocking, feature creation, story moves all match |
| `upgrade` | `test-upgrade-ops.py` | v4 → v4 is a confirmed no-op |
| `discover` | bidirectional sync regression | Repo sync and governance-main auto-commit behavior unchanged |
| git-orchestration | `test-git-orchestration-ops.py` | Branch naming and publish-to-governance path unchanged |

Architecture is complete when:

1. All 17 retained commands have their full stub → prompt → skill → script chain verified first against rewrite contracts and then checked against approved legacy reference outcomes for parity
2. All three shared utility implementations exist and all phase skills delegate to them (no inline duplicates remain)
3. The retained-vs-removed skill inventory is confirmed and signed off
4. Every retained command has an assigned work package, owner lane, and parity gate
5. The regression suite above passes unchanged
