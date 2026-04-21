# `_bmad/lens-work/` — Deep Dive Documentation

**Target:** `TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/`
**Module version:** 4.0.0
**Scan date:** 2026-04-21
**Feature:** lens-dev-old-codebase-discovery

---

## 1. Module Identity and Architecture Overview

The `lens-work` module is the **LENS Workbench** — an AI-guided lifecycle orchestration system for software features. It provides a complete, git-derived, phase-gated workflow from idea inception through implemented delivery. The module is self-contained and framework-agnostic; it deploys as a BMAD module that can be installed into any project's `.github/` adapter layer.

### Design Axioms (from `lifecycle.yaml`)

| Axiom | Statement |
|-------|-----------|
| A1 | Git is the only source of truth for shared workflow state. No git-ignored runtime state. |
| A2 | PRs are the only gating mechanism. No side-channel approval. |
| A3 | Authority domains must be explicit. Every file belongs to exactly one authority. |
| A4 | Sensing must be automatic at lifecycle gates, not manual discovery. |
| A5 | The control repo is an operational workspace, not a code repo. |

### Repository Topology

```
control repo (feature branches)
  ├── {featureId}               ← base branch
  ├── {featureId}-plan          ← planning artifacts accumulate here
  └── {featureId}-dev-{user}    ← optional dev tracking branch

governance repo (always main)
  ├── feature-index.yaml        ← global registry
  ├── feature.yaml              ← per-feature canonical state
  ├── constitutions/            ← org → domain → service → repo rules
  └── features/{domain}/{service}/{featureId}/
       ├── summary.md, retrospective.md, problems.md
       └── docs/                ← planning artifacts mirror

target repo (code)
  └── feature/{featureId}[-{username}]  ← implementation branch
```

### Hierarchy

```
org → domain → service → repo
```

Constitution inheritance is **additive**; lower levels add constraints and cannot remove them.

---

## 2. Module Manifests

### `module.yaml`

| Field | Value |
|-------|-------|
| code | `lens` |
| name | LENS Workbench |
| module_version | `4.0.0` |
| type | standalone |
| global | false |
| lifecycle file | `lifecycle.yaml` (schema_version: 4) |
| dependencies | `core` |
| optional_dependencies | `cis`, `tea` |
| installer | `_module-installer/installer.js` |

**Declares 41 skills** across feature lifecycle, git operations, planning phases, review, migration, and utility.

**Key config questions at install time:**
- `target-projects-path` — where repos are cloned (default `../TargetProjects`)
- `default-git-remote` — `github`, `gitlab`, `azure-devops`
- `ides` — IDE adapters to install (`github-copilot`, `cursor`, `claude`, etc.)

---

### `lifecycle.yaml` (schema v4)

The **single source of truth** for all lifecycle behavior. Imported by the `@lens` agent and all skills. No lifecycle semantics are hardcoded elsewhere.

#### Schema-level concepts

| Concept | Description |
|---------|-------------|
| `schema_version: 4` | Current schema; breaking changes trigger migrations |
| `close_states` | `[completed, abandoned, superseded]` |
| `artifact_publication` | Governance artifacts written to `artifacts/`, enabled by default |
| `supported_languages` | 10 languages: typescript, python, go, java, csharp, rust, php, kotlin, swift, cpp |

#### Phases

| Phase | Display Name | Agent | Milestone | Auto-Advance |
|-------|-------------|-------|-----------|-------------|
| `preplan` | PrePlan | mary (Analyst) | techplan | `/businessplan` |
| `businessplan` | BusinessPlan | john (PM) + sally | techplan | `/techplan` |
| `techplan` | TechPlan | winston (Architect) | techplan | `/finalizeplan` |
| `finalizeplan` | FinalizePlan | lens (Planning Conductor) | finalizeplan | `/dev` |
| `expressplan` | ExpressPlan | lens (Express Planner) | finalizeplan | `/finalizeplan` |

- `phase_order` = `[preplan, businessplan, techplan, finalizeplan]`
- `expressplan` is standalone — not in phase_order, for the `express` track only
- `dev` is NOT a lifecycle phase — it is a delegation command that routes to target-project agents

#### Tracks

| Track | Phases | Use Case |
|-------|--------|----------|
| `full` | preplan → businessplan → techplan → finalizeplan | Complete lifecycle |
| `feature` | businessplan → techplan → finalizeplan | Known business context |
| `tech-change` | techplan → finalizeplan | Pure technical change |
| `hotfix` | techplan (only) | Urgent fix, minimal planning |
| `hotfix-express` | techplan (only) | Emergency, constitution gate informational |
| `spike` | preplan (only) | Research, no implementation |
| `quickdev` | finalizeplan | Rapid execution via target-project agents |
| `express` | expressplan → finalizeplan | Accelerated single-session planning |

All tracks use **2-branch topology** (`{featureId}` + `{featureId}-plan`) and `feature_yaml: true`.

#### Milestones

| Milestone | Role | Entry Gate |
|-----------|------|------------|
| `techplan` | IC creation work | — |
| `finalizeplan` | Planning consolidation | `adversarial-review` (party mode) |
| `dev-ready` | Ready for execution | `constitution-gate` |
| `dev-complete` | Execution complete (optional) | `dev-complete-validation` |

#### Adversarial Review — Party Mode

Phase completion reviews are **party-mode**: each artifact is reviewed by 2–3 named agents simultaneously.

| Artifact | Lead | Participants | Focus |
|----------|------|-------------|-------|
| product-brief | john | john, winston, sally | Actionable? Buildable? User-centered? |
| prd | winston | winston, mary, sally | Buildable? Well-researched? UX-aligned? |
| ux-design | john | john, winston, mary | Serves requirements? Technically feasible? |
| architecture | john | john, mary, bob | Meets spec? Practical? Sprintable? |
| epics-and-stories | winston | winston, bob, amelia | Buildable? Right-sized? Implementable? |

#### Artifact Validation

Enabled (`mode: pre-commit`, constitution-controlled). Each artifact has:
- `required_sections` — structural checks
- `min_word_count` — quality floor
- `must_reference` — cross-document dependency check

Relaxed variants exist for the `express` track (lower word counts, fewer required sections).

#### Supporting Contracts

| Contract | Behavior |
|----------|----------|
| `parallel_phases` | Disabled by default; constitution opt-in; allows businessplan + techplan parallel after PRD |
| `content_aware_sensing` | Cosine similarity (0.7 threshold) across epics, stories, architecture to catch true conflicts |
| `gate_collapsing` | Constitution opt-in; small features (≤3 stories) can collapse to 1 PR |
| `branch_cleanup` | Squash-and-delete after merge; never delete dev-ready or initiative root |
| `artifact_templates` | 11 templates in `assets/templates/`; constitution-overridable per domain |

---

## 3. Skills — Complete Inventory

### 3.1 Core Lifecycle Phase Conductors

#### `bmad-lens-preplan`
- **Purpose:** PrePlan phase conductor — brainstorm, research, product brief with Lens governance
- **Agents:** mary (Analyst), supporting: none
- **Artifacts:** product-brief, research, brainstorm
- **Gate:** adversarial-review (party mode) before advancing
- **Scripts:** none (pure AI instruction flow)

#### `bmad-lens-businessplan`
- **Purpose:** BusinessPlan phase conductor — PRD creation and UX design
- **Agents:** john (PM) + sally (UX)
- **Artifacts:** prd, ux-design
- **Gate:** adversarial-review before advancing to techplan
- **Scripts:** none

#### `bmad-lens-techplan`
- **Purpose:** TechPlan phase conductor — architecture and technical design
- **Agents:** winston (Architect)
- **Artifacts:** architecture
- **Gate:** adversarial-review before advancing to finalizeplan
- **Scripts:** none

#### `bmad-lens-finalizeplan`
- **Purpose:** Final planning consolidation — review, bundle, PR handoff
- **Agents:** lens (Planning Conductor) + john, bob, winston supporting
- **Artifacts:** review-report, epics, stories, implementation-readiness, sprint-status, story-files
- **3-step execution contract:**
  1. `review-and-push` — adversarial review + party challenge, commit and push plan branch
  2. `plan-pr-readiness` — create/verify `{featureId}-plan → {featureId}` PR
  3. `downstream-bundle-and-final-pr` — generate epics/stories, open final `{featureId} → main` PR
- **Scripts:** none

#### `bmad-lens-expressplan`
- **Purpose:** Accelerated single-session planning for the `express` track
- **Agents:** lens + winston, john, sally, bob supporting
- **3-step execution contract:**
  1. `quickplan-via-lens-wrapper` — delegate to `bmad-lens-quickplan` with Lens context
  2. `adversarial-review-party-mode` — reviews business-plan + tech-plan; `fail` halts
  3. `advance-to-finalizeplan` — update feature.yaml, signal advance
- **Scripts:** none

---

### 3.2 Core Orchestration Skills

#### `bmad-lens-next` — 344 LOC
- **Purpose:** Next-action routing — reads feature state, resolves the single best next action, auto-delegates when unblocked
- **Script:** `scripts/next-ops.py`
- **Key logic:**
  - Loads `feature.yaml` for phase, track, open problems, staleness flags
  - Maps phase → command using `lifecycle.yaml` `auto_advance_to` values
  - On `*-complete` suffix: delegates to the `auto_advance_to` from that phase
  - Hard gates: missing required artifacts, milestone not completed, open problems > 3
  - When unblocked: immediately delegates into the target skill rather than printing a menu
- **Entry milestone rules:**
  - `techplan` requires `businessplan` complete
  - `finalizeplan` requires `techplan` complete
  - `dev` requires `finalizeplan` complete
  - `complete` requires `dev-complete` milestone set
- **LEGACY_TRACK_ALIASES:** `quickplan → feature`
- **Dependencies:** `pyyaml>=6.0`, `lifecycle.yaml` (resolved via `Path(__file__).resolve().parents[3]`)

#### `bmad-lens-init-feature` — 1,832 LOC (largest skill script)
- **Purpose:** Feature initialization orchestrator — creates 2-branch topology, feature.yaml, feature-index.yaml entry, summary.md stub, PR
- **Script:** `scripts/init-feature-ops.py`
- **Progressive disclosure contract:**
  1. Ask only for name, domain, service
  2. Derive featureId = `{domain}-{service}-{featureSlug}` (lowercased, hyphenated)
  3. Confirm featureId + featureSlug before writing anything
  4. Present track choices; require explicit user selection
  5. Create all artifacts atomically
- **Key validations:**
  - `FEATURE_SLUG_PATTERN = ^[a-z0-9][a-z0-9-]{0,63}$`
  - `FEATURE_ID_PATTERN = ^[a-z0-9][a-z0-9-]*$`
  - Duplicate detection in `feature-index.yaml` before creation
  - Domain/service are path-constructing inputs — sanitized first
- **Outputs:**
  - Governance: `feature.yaml`, `feature-index.yaml` entry, `summary.md`, domain/service marker files
  - Control repo: `{featureId}` + `{featureId}-plan` branches created and pushed
  - Optionally: TargetProjects scaffold `.gitkeep`, `docs/{domain}/{service}/` scaffold
- **Options:** `--execute-governance-git` for the governance checkout/pull/add/commit/push sequence
- **AMBIGUOUS_SERVICE_NAMES:** warns on `api`, `auth`, `common`, `core`, `data`, `identity`
- **Dependencies:** `pyyaml>=6.0`, Python ≥ 3.10

#### `bmad-lens-quickplan` — 276 LOC
- **Purpose:** End-to-end planning pipeline from business plan through story creation
- **Script:** `scripts/quickplan-ops.py`
- **Two-document rule:** business-plan.md and tech-plan.md are always separate — never combined
- **Batch mode:** Two-pass contract (pass 1 writes `quickplan-batch-input.md`; pass 2 resumes with approved answers)
- **Phase fidelity:** Inherits contracts of `/businessplan`, `/techplan`, `/finalizeplan` rather than bypassing them
- **Args:** `--feature-id`, `--mode interactive|batch`

#### `bmad-lens-switch` — 732 LOC (largest utility script)
- **Purpose:** Feature context switcher — loads cross-feature context, lists available features
- **Script:** `scripts/switch-ops.py`
- **Dependencies:** `pyyaml>=6.0`

---

### 3.3 Git Skill Pair (Read/Write Split)

#### `bmad-lens-git-state` — 439 LOC
- **Purpose:** Read-only git queries for the Lens 2-branch feature model
- **Script:** `scripts/git-state-ops.py`
- **Strict invariant:** Never writes, never modifies state, never creates branches
- **Capabilities:**
  - `feature-state` — loads feature.yaml + git branch reality, surfaces discrepancies
  - `branches` — query branch existence, remote/local
  - `active-features` — workspace-wide scan
  - Context fallback when on a non-feature branch
- **State sources:** feature.yaml (lifecycle state) + git branches (ground truth for what work started)
- **Discrepancy handling:** Reports gap explicitly, never silently reconciles

#### `bmad-lens-git-orchestration` — 1,071 LOC
- **Purpose:** Git write operations for the Lens 2-branch model + target-repo dev branch preparation
- **Script:** `scripts/git-orchestration-ops.py`
- **Subcommands:**
  - `create-feature-branches` — creates `{featureId}` + `{featureId}-plan` from default branch
  - `commit-artifacts` — staged + structured commit with phase metadata
  - `create-dev-branch` — creates `{featureId}-dev-{username}` contributor branch
  - `prepare-dev-branch` — prepares target repo working branch (3 modes)
  - `merge-plan` — merges `{featureId}-plan` into `{featureId}`
  - `publish-to-governance` — copies staged planning docs to governance docs mirror
  - `push` — push current or named branch to remote
- **Target-repo dev branch modes:**
  - `direct-default` — code goes directly to target repo default branch
  - `feature-id` — `feature/{featureId}` branch (default)
  - `feature-id-username` — `feature/{featureId}-{username}` branch
- **2-branch invariant:** Every feature has exactly `{featureId}` + `{featureId}-plan` in the control repo; target-repo working branches are separate
- **No silent pushes:** Remote push only happens when explicitly requested or phase is complete
- **PHASE_ARTIFACTS dict:** Maps phase → list of artifact names for structured commits
- **Dependencies:** `PyYAML>=6.0`, Python ≥ 3.11

---

### 3.4 Feature YAML Skill

#### `bmad-lens-feature-yaml` — 836 LOC
- **Purpose:** Feature YAML lifecycle operations — create, read, update, validate, list feature.yaml files
- **Script:** `scripts/feature-yaml-ops.py`
- **feature.yaml is the single source of truth** for feature identity, lifecycle state, organizational hierarchy, and metadata
- **Schema validation constants:**
  - `VALID_PHASES` = 8 base + 5 completable variants (with `-complete` suffix) = up to 13 values
  - `VALID_TRACKS` = `[quickplan, full, feature, hotfix, hotfix-express, express, spike, tech-change]`
  - `VALID_PRIORITIES` = `[critical, high, medium, low]`
  - `VALID_ROLES` = `[lead, contributor, reviewer]`
  - `VALID_VISIBILITIES` = `[public, private, internal]`
  - `SAFE_ID_PATTERN = ^[a-z0-9][a-z0-9._-]{0,63}$`
- **TRACK_TRANSITIONS dict:** Encodes valid phase-to-phase transitions per track (e.g., `full.techplan → [finalizeplan, paused]`)
- **Dependencies:** `pyyaml>=6.0`, Python ≥ 3.10

---

### 3.5 Governance Skills

#### `bmad-lens-constitution` — 575 LOC
- **Purpose:** Resolves applicable governance rules using a 4-level hierarchy (org → domain → service → repo)
- **Script:** `scripts/constitution-ops.py`
- **Hierarchy location:** `{governance-repo}/constitutions/org/constitution.md` → domain → service → repo
- **Additive inheritance:** Lower levels add constraints; they cannot remove higher-level requirements
- **Progressive disclosure:** Only shows rules relevant to current phase + track
- **Compliance modes:** `hard gate` (blocks promotion) vs `informational gate` (noted but not blocking)
- **Never fabricates:** Missing constitution levels are reported as gaps, not filled from assumptions

#### `bmad-lens-sensing` — (script-less)
- **Purpose:** Cross-initiative overlap detection — scans branch topology for scope conflicts
- **Conflict types:** `SCOPE_OVERLAP`, `NAME_CONFLICT`, `RESOURCE_CLASH`
- **Advisory by default:** Hard gates require `--enforce` flag or constitution declaration
- **Content-aware sensing:** (from `lifecycle.yaml`) uses 0.7 cosine similarity threshold on epics, stories, architecture to reduce false positives
- **Read-only:** Never modifies initiatives, branches, or state

#### `bmad-lens-adversarial-review` — (script-less)
- **Purpose:** Lifecycle adversarial review gate with mandatory party-mode blind-spot challenge
- **Scope:** preplan, businessplan, techplan, finalizeplan, expressplan
- **Hard gate on phase-complete:** `fail` verdict blocks phase completion
- **Manual reruns:** Update the review artifact only; never advance lifecycle state
- **Party-mode requirement:** After adversarial findings, a multi-voice blind-spot challenge round is required
- **Verdict persistence:** Always writes `{phase}-adversarial-review.md` in control repo docs path
- **Args:** `--phase`, `--source`, `--feature-id`

---

### 3.6 Utility and Support Skills

#### `bmad-lens-help` — 174 LOC
- **Purpose:** Contextual help system — shows relevant commands for the current lifecycle state
- **Script:** `scripts/help-ops.py`
- **Assets:** `assets/help-topics.yaml` (topic index)
- **Sources:** `module-help.csv` + `help-topics.yaml`

#### `bmad-lens-onboard` — 282 LOC
- **Purpose:** Workspace bootstrap — runs shared preflight, stops on failures, ends with role-aware next steps
- **Script:** `scripts/onboard-ops.py`
- **Interactive contract:** Runs `preflight.py --caller onboard`; blocks on failures; no hidden auto-scaffold
- **Legacy subcommands retained:** `preflight`, `scaffold`, `write-config` (backward compat only)
- **Role-aware handoff:** `primary_role: dev` → `/switch` then `/dev`; others → `/switch` or `/new-*`

#### `bmad-lens-profile` — (script-less)
- **Purpose:** View and edit onboarding profile — displays current profile fields, supports interactive editing
- **State location:** `.lens/personal/profile.yaml`

#### `bmad-lens-dashboard` — 575 LOC
- **Purpose:** Cross-feature HTML dashboard generator
- **Script:** `scripts/dashboard-ops.py`
- **Output:** Single self-contained HTML file (no CDN, no external assets)
- **Data sources:** `feature-index.yaml` + governance `features/` deep content
- **Staleness threshold:** Active-phase feature with `lastUpdated` > 14 days ago
- **Graceful degradation:** Missing deep content reported as "unavailable", never treated as error
- **Args:** `collect`, `generate`, `dependency-data` + `--governance-repo`

#### `bmad-lens-batch` — (script-less)
- **Purpose:** Universal two-pass batch intake and resume flow for planning targets
- **Pass 1:** Writes/refreshes batch input file, stops
- **Pass 2:** Resumes pipeline only after approved answers are loaded
- **Principle:** Batch never means silent autonomous completion

#### `bmad-lens-bmad-skill` — (script-less)
- **Purpose:** Lens-aware BMAD skill wrapper — resolves feature context, governance, and write boundaries, then delegates to a registered BMAD skill
- **Registry:** `assets/lens-bmad-skill-registry.json`
- **Used by:** `expressplan` (delegates to `bmad-lens-quickplan`)

---

### 3.7 Feature Lifecycle Management Skills

#### `bmad-lens-pause-resume` — 286 LOC
- **Purpose:** Pause and resume features with state preservation
- **Script:** `scripts/pause-resume-ops.py`
- **State transitions:** Any phase → `paused`; `paused` → any base phase (per `TRACK_TRANSITIONS`)

#### `bmad-lens-switch` — 732 LOC
- **Purpose:** Feature context switcher — cross-feature context loading, feature listing
- **Script:** `scripts/switch-ops.py`

#### `bmad-lens-discover` — 342 LOC
- **Purpose:** Sync TargetProjects with governance repo inventory
- **Script:** `scripts/discover-ops.py`
- **Use case:** Reconcile local clones with what governance knows about

#### `bmad-lens-move-feature` — 440 LOC
- **Purpose:** Feature relocation to a new domain/service, updating all references and metadata
- **Script:** `scripts/move-feature-ops.py`
- **Interactive:** Requires user confirmation before any write

#### `bmad-lens-split-feature` — 565 LOC
- **Purpose:** Feature splitting — divides scope or stories into two features
- **Script:** `scripts/split-feature-ops.py`
- **Eligibility validation:** Checks split boundary before creating the new feature

#### `bmad-lens-rollback` — (script-less)
- **Purpose:** Safe phase rollback with confirmation gates and audit trail
- **Non-destructive:** Audit trail is required before any rollback executes

#### `bmad-lens-complete` — 450 LOC
- **Purpose:** Feature lifecycle endpoint — runs retrospective, delegates final project documentation, archives
- **Script:** `scripts/complete-ops.py`
- **Non-negotiable:** Final project state must be documented before closing; uses `bmad-lens-document-project`
- **Pre-conditions:**
  1. Feature exists in governance
  2. Phase is `dev` or `complete` (not planning phases or `paused`)
  3. `retrospective.md` exists (or user confirms skip)
- **Atomic archive:** feature-index + feature.yaml + final summary updated together
- **Irreversible:** Confirm before executing; complete cannot be undone

#### `bmad-lens-retrospective` — 494 LOC
- **Purpose:** Retrospective analyst — analyzes full problem log, identifies recurring patterns, generates root cause report
- **Script:** `scripts/retrospective-ops.py`
- **Output:** Feeds findings forward into user-level insights

#### `bmad-lens-log-problem` — 453 LOC
- **Purpose:** Problem capture and logging during any workflow phase
- **Script:** `scripts/log-problem-ops.py`
- **Features:** Inline fix recording, unresolved issue export to GitHub

---

### 3.8 Migration Skills

#### `bmad-lens-migrate` — 2,741 LOC (second-largest script)
- **Purpose:** Migration bridge from LENS v3 (`domain-service-feature[-milestone]` branches) to Lens Next 2-branch model
- **Script:** `scripts/migrate-ops.py`
- **Subcommands:** `scan`, `dry-run`, `migrate`, `verify`, `cleanup`
- **Non-negotiable:** Dry-run mandatory before execution; in-progress work must not be lost
- **Governance-first:** Every discovered legacy document mirrored to `docs/lens-work/migrations/...` as durable proof
- **Cleanup-receipt artifacts:** Both `cleanup-approval.yaml` and `cleanup-receipt.yaml` written as durable proof before/after destructive cleanup
- **Conflict detection:** Checks for existing `feature.yaml` at target path before any migration
- **Legacy path awareness:** Reads from `docs/{domain}/{service}/{legacyFeature}/` and `_bmad-output` paths

#### `bmad-lens-upgrade` — (script-less)
- **Purpose:** Schema migration between control repo versions
- **Route:** Explicit routing to legacy branch migration when schema is already current
- **Safety gates:** Dry-run support; never auto-upgrades

---

### 3.9 Dev Phase Skills

#### `bmad-lens-dev` — (script-less, AI instruction flow)
- **Purpose:** Dev phase conductor — epic-level story implementation loop with target-repo branch modes
- **Delegation model:** Every task implementation delegated via `runSubagent` — never inline
- **One target repo working branch per dev cycle** — all stories in an epic share one working branch
- **Branch mode memory:** `dev_branch_mode` stored per target repo in `repo-inventory.yaml` + `feature.yaml`
- **Dev branch modes:** `direct-default`, `feature-id` (default), `feature-id-username`
- **Single final PR:** One PR per dev cycle from working branch to target default branch
- **State checkpointing:** `dev-session.yaml` written after branch prep, after each story, after final review
- **Final gate required:** Full dev-closeout adversarial review + party-mode before final PR
- **Per-task commits:** Each task/subtask gets its own commit with Story/Task/Epic metadata

#### `bmad-lens-target-repo` — 486 LOC
- **Purpose:** Repository provisioning — verifies/creates remote, clones to canonical path, updates governance inventory and feature metadata
- **Script:** `scripts/target-repo-ops.py`
- **Canonical clone path:** `TargetProjects/{domain}/{service}/{repo}` (stored project-root-relative)
- **Governance alignment:** Every provisioned repo reflected in both `repo-inventory.yaml` and `feature.yaml.target_repos`
- **GitHub-first creation:** Auto-creation via `gh` for GitHub hosts; other providers get manual guidance
- **Idempotent:** Rerunning verifies and reconciles rather than duplicating entries

---

### 3.10 Deprecated Skills (Legacy Reference)

| Skill | Replacement |
|-------|-------------|
| `bmad-lens-devproposal` | FinalizePlan |
| `bmad-lens-sprintplan` | FinalizePlan |
| `bmad-lens-lessons` | Memory and review workflows |

These are retained only for migration reference and migration tooling that scans legacy patterns.

---

### 3.11 Remaining Skills

#### `bmad-lens-theme` — 464 LOC
- **Purpose:** Theme loading and persona overlay system
- **Scripts:** `scripts/theme-ops.py` + tests
- **Features:** Theme preference persistence, easter egg conditions, available theme listing

#### `bmad-lens-approval-status` — (script-less)
- **Purpose:** Aggregates pending promotion PR approval state across initiatives
- **Output:** Per-initiative PR approval status for planning or dev-ready gating

#### `bmad-lens-audit` — (script-less)
- **Purpose:** Cross-initiative compliance audit — scans all active initiatives for lifecycle compliance, artifact completeness, constitutional governance
- **Output:** Compliance dashboard

#### `bmad-lens-module-management` — (script-less)
- **Purpose:** Module version checking and self-service upgrade guidance
- **Clarifies:** Schema status does NOT clear legacy branch migration

#### `bmad-lens-document-project` — (script-less)
- **Purpose:** Document active feature/project with domain/service/feature-scoped output in both control and governance repos
- **Delegated from:** `bmad-lens-complete` for final project documentation
- **Output paths:** `docs/{domain}/{service}/{featureId}/` (control) + `features/{domain}/{service}/{featureId}/docs/` (governance)

---

## 4. Prompts — Complete Inventory (56 + 1 README)

Prompts are `.prompt.md` files in `prompts/`. They are the **user-facing command entry points** — thin shells that load and delegate to the corresponding SKILL.md. The prompt layer is the IDE adapter surface.

### Lens Workbench Command Prompts

| Prompt | Command |
|--------|---------|
| `lens-init-feature.prompt.md` | `/init-feature` — Feature initialization |
| `lens-next.prompt.md` | `/next` — Next-action routing |
| `lens-onboard.prompt.md` | `/onboard` — Workspace bootstrap |
| `lens-switch.prompt.md` | `/switch` — Context switch between features |
| `lens-help.prompt.md` | `/help` — Contextual help |
| `lens-complete.prompt.md` | `/complete` — Feature lifecycle endpoint |
| `lens-pause-resume.prompt.md` | `/pause-resume` — Pause or resume a feature |
| `lens-discover.prompt.md` | `/discover` — Sync TargetProjects inventory |
| `lens-dashboard.prompt.md` | `/dashboard` — Cross-feature HTML dashboard |
| `lens-sensing.prompt.md` | `/sensing` — Cross-initiative overlap detection |
| `lens-audit.prompt.md` | `/audit` — Compliance audit |
| `lens-approval-status.prompt.md` | `/approval-status` — PR approval aggregation |
| `lens-batch.prompt.md` | `/batch` — Batch intake flow |
| `lens-module-management.prompt.md` | `/module-management` — Version check |
| `lens-rollback.prompt.md` | `/rollback` — Safe phase rollback |
| `lens-upgrade.prompt.md` | `/upgrade` — Schema migration |
| `lens-profile.prompt.md` | `/profile` — Onboarding profile editor |
| `lens-theme.prompt.md` | `/theme` — Theme/persona overlay |
| `lens-log-problem.prompt.md` | `/log-problem` — Problem capture |
| `lens-retrospective.prompt.md` | `/retrospective` — Feature retrospective |
| `lens-migrate.prompt.md` | `/migrate` — v3→v4 migration |
| `lens-move-feature.prompt.md` | `/move-feature` — Feature relocation |
| `lens-split-feature.prompt.md` | `/split-feature` — Feature splitting |
| `lens-git-orchestration.prompt.md` | `/git-orchestration` — Git write ops |
| `lens-git-state.prompt.md` | `/git-state` — Git read-only queries |
| `lens-feature-yaml.prompt.md` | `/feature-yaml` — Feature YAML operations |
| `lens-constitution.prompt.md` | `/constitution` — Governance rule resolution |
| `lens-target-repo.prompt.md` | `/target-repo` — Repo provisioning |

### Planning Phase Prompts

| Prompt | Command |
|--------|---------|
| `lens-preplan.prompt.md` | `/preplan` — PrePlan phase |
| `lens-businessplan.prompt.md` | `/businessplan` — BusinessPlan phase |
| `lens-techplan.prompt.md` | `/techplan` — TechPlan phase |
| `lens-finalizeplan.prompt.md` | `/finalizeplan` — FinalizePlan phase |
| `lens-expressplan.prompt.md` | `/expressplan` — ExpressPlan (express track) |
| `lens-quickplan.prompt.md` | `/quickplan` — End-to-end planning pipeline |
| `lens-dev.prompt.md` | `/dev` — Dev phase (epic implementation) |

### BMAD Wrapper Prompts

These load a registered BMAD skill through the Lens context wrapper:

| Prompt | Wrapped BMAD Skill |
|--------|--------------------|
| `lens-bmad-brainstorming.prompt.md` | bmad-brainstorming |
| `lens-bmad-check-implementation-readiness.prompt.md` | bmad-check-implementation-readiness |
| `lens-bmad-code-review.prompt.md` | bmad-code-review |
| `lens-bmad-create-architecture.prompt.md` | bmad-create-architecture |
| `lens-bmad-create-epics-and-stories.prompt.md` | bmad-create-epics-and-stories |
| `lens-bmad-create-prd.prompt.md` | bmad-create-prd |
| `lens-bmad-create-story.prompt.md` | bmad-create-story |
| `lens-bmad-create-ux-design.prompt.md` | bmad-create-ux-design |
| `lens-bmad-document-project.prompt.md` | bmad-document-project |
| `lens-bmad-domain-research.prompt.md` | bmad-domain-research |
| `lens-bmad-market-research.prompt.md` | bmad-market-research |
| `lens-bmad-product-brief.prompt.md` | bmad-product-brief |
| `lens-bmad-quick-dev.prompt.md` | bmad-quick-dev |
| `lens-bmad-sprint-planning.prompt.md` | bmad-sprint-planning |
| `lens-bmad-technical-research.prompt.md` | bmad-technical-research |

### Utility Prompts

| Prompt | Purpose |
|--------|---------|
| `lens-new-domain.prompt.md` | Create a new domain in governance |
| `lens-new-service.prompt.md` | Create a new service in governance |
| `lens-new-project.prompt.md` | New project wizard |
| `lens-new-feature.prompt.md` | Alias/shorthand for init-feature |
| `lens-preflight.prompt.md` | Run preflight check directly |
| `lens-adversarial-review.prompt.md` | Adversarial review gate |
| `lens-businessplan.prompt.md` | BusinessPlan phase |

---

## 5. Operational Scripts — Complete Inventory

Located in `scripts/`. All use `uv run --script` with inline `/// script` metadata. No external CLI dependencies beyond `git` and optionally `gh`.

| Script | LOC | Purpose |
|--------|-----|---------|
| `preflight.py` | 829 | Synchronize authority repos and validate workspace state before workflow execution |
| `setup-control-repo.py` | 530 | Bootstrap a new control repo — clone governance + target repos, copy `.github` adapter |
| `install.py` | 511 | IDE adapter installer — installs GitHub Copilot, Cursor, Claude, Codex, OpenCode adapters |
| `light-preflight.py` | 495 | Lightweight preflight check (faster than full preflight, used for quick validation) |
| `create-pr.py` | 233 | PR creation via REST API (PAT auth) or manual URL; multi-provider support |
| `bootstrap-target-projects.py` | 161 | Bootstrap TargetProjects folder structure |
| `validate-phase-artifacts.py` | 204 | Validate presence and structure of phase artifacts |
| `scan-active-initiatives.py` | 115 | Scan all active initiative branches in the workspace |
| `run-preflight-cached.py` | 107 | Preflight with caching layer to avoid redundant checks |
| `load-command-registry.py` | 128 | Load and validate the BMAD command registry |
| `plan-lifecycle-renames.py` | 111 | Plan governance file renames for lifecycle migrations |
| `store-github-pat.py` | 133 | Secure PAT collection into environment variables (run outside AI context) |
| `validate-feature-move.py` | 110 | Validate feature move eligibility before executing |
| `validate-lens-bmad-registry.py` | 93 | Validate that registered BMAD skills exist in module manifests |
| `validate-lens-core-parity.sh` | — | Shell script: validate parity between source and installed module |
| `derive-initiative-status.py` | 87 | Derive initiative status from git branch + artifact state |
| `derive-next-action.py` | 91 | Derive the recommended next action from feature state |
| `bootstrap-target-projects.py` | 161 | Bootstrap TargetProjects directory tree |

### Key Script Details

#### `preflight.py` (829 LOC)
- **Entry:** `uv run ./lens.core/_bmad/lens-work/scripts/preflight.py --caller onboard`
- **Validates:** git availability, Python version, path safety, governance repo health, lifecycle.yaml schema version
- **Root resolution:** Finds nearest ancestor containing `lens.core/` directory
- **Known gotcha:** When run from `TargetProjects/lens.core/src/Lens.Core.Src`, resolves root to `TargetProjects/` not workspace root — so it looks for `lifecycle.yaml` at `TargetProjects/lens.core/_bmad/lens-work/lifecycle.yaml`
- **Caching:** `run-preflight-cached.py` provides a caching layer to skip redundant checks

#### `install.py` (511 LOC)
- **Supported IDEs:** `github-copilot`, `cursor`, `claude`, `codex`, `opencode`
- **Idempotent:** Skips existing files by default; `--update` to overwrite
- **Global state counters:** `_created`, `_skipped`, `_errors`, `_removed`
- **Dry-run support:** `--dry-run` prints what would happen without writing

#### `setup-control-repo.py` (530 LOC)
- **Purpose:** Bootstraps new control repos by cloning governance + target repos and copying `.github` adapter
- **Governance name derivation:** Preserves control repo root folder name verbatim + appends `.governance`
- **Auto-create:** Uses `gh repo create` before cloning when governance repo is missing
- **Dry-run support:** `--dry-run` available
- **Dependencies:** only `git`; no external Python packages

#### `create-pr.py` (233 LOC)
- **External dependency:** `requests>=2.31`
- **Multi-provider:** Parses GitHub HTTPS, GitHub SSH, Azure DevOps URLs
- **Auth:** `GITHUB_TOKEN` env var (PAT); falls back to manual URL if absent
- **PR creation:** REST API calls with JSON body; full error reporting

---

## 6. Test Suite — Complete Inventory

### Module-Level Tests (in `scripts/tests/`)

| Test File | LOC | What It Validates |
|-----------|-----|------------------|
| `test-preflight.py` | — | Full preflight behavior, root resolution, schema version checks |
| `test-light-preflight.py` | — | Lightweight preflight behavior |
| `test-install.py` | — | IDE adapter generation, file skipping, update mode |
| `test-setup-control-repo.py` | — | Control repo bootstrap, governance naming, gh auto-create |
| `test-create-pr.py` | — | PR creation, provider detection, auth fallback |
| `test-store-github-pat.py` | — | PAT collection, environment variable storage |
| `test-validate-phase-artifacts.py` | — | Phase artifact validation logic |
| `test-phase-conductor-contracts.py` | — | Phase conductor step contracts |
| `test-adversarial-review-contracts.py` | — | Adversarial review gate contracts |
| `test-batch-mode-contracts.py` | — | Batch intake two-pass contracts |

### Skill-Level Tests (in `skills/*/scripts/tests/`)

| Test File | Key Coverage |
|-----------|-------------|
| `test-init-feature-ops.py` | featureId composition, featureSlug derivation, governance naming, 2-branch topology |
| `test-feature-yaml-ops.py` | YAML validation, phase transitions, track-specific rules |
| `test-git-state-ops.py` | Branch existence checks, feature state derivation, discrepancy detection |
| `test-git-orchestration-ops.py` | Branch creation, artifact commits, dev branch modes, prepare-dev-branch |
| `test-next-ops.py` | Next-action routing, milestone gate enforcement, auto-delegate fallback |
| `test-constitution-ops.py` | 4-level hierarchy resolution, additive inheritance, progressive disclosure |
| `test-complete-ops.py` | Pre-condition checks, atomic archive, finalize contract |
| `test-retrospective-ops.py` | Problem log analysis, pattern detection, root cause report |
| `test-discover-ops.py` | Governance/TargetProjects sync, inventory reconciliation |
| `test-dashboard-ops.py` | HTML generation, staleness detection, graceful degradation |
| `test-move-feature-ops.py` | Move eligibility, reference updates, confirmation gates |
| `test-split-feature-ops.py` | Split boundary validation, new feature creation |
| `test-migrate-ops.py` | v3→v4 migration, dry-run, verify, cleanup receipt artifacts |
| `test-fix-feature-dirs.py` | Feature directory structure fixes during migration |
| `test-normalize-feature-ids.py` | featureId normalization and slug derivation |
| `test-quickplan-ops.py` | Planning pipeline orchestration, two-document rule |
| `test-pause-resume-ops.py` | State transitions, paused phase handling |
| `test-switch-ops.py` | Context switching, cross-feature context loading |
| `test-target-repo-ops.py` | Repo provisioning, inventory alignment, dev branch mode persistence |
| `test-log-problem-ops.py` | Problem capture, GitHub export |
| `test-onboard-ops.py` | Preflight delegation, scaffold, backward-compat subcommands |
| `test-help-ops.py` | Help topic loading, contextual filtering |
| `test-theme-ops.py` | Theme loading, preference persistence |

### Test Invocation Pattern

```bash
# From TargetProjects/lens.core/src/Lens.Core.Src:
uv run --with pytest --with pyyaml pytest _bmad/lens-work/scripts/tests/test-<name>.py -q
uv run --with pytest pytest _bmad/lens-work/skills/<skill>/scripts/tests/test-<name>.py -q
```

---

## 7. Asset Templates — Complete Inventory

Located in `assets/templates/`. All are YAML or Markdown starter scaffolds. Loaded by skills as document skeletons; constitution can override per domain.

| Template | Format | Used By |
|----------|--------|---------|
| `product-brief-template.md` | Markdown | preplan, quickplan |
| `prd-template.md` | Markdown | businessplan, quickplan |
| `ux-design-template.md` | Markdown | businessplan |
| `architecture-template.md` | Markdown | techplan, quickplan |
| `epics-template.md` | Markdown | finalizeplan |
| `stories-template.md` | Markdown | finalizeplan |
| `implementation-readiness-template.md` | Markdown | finalizeplan |
| `sprint-status-template.yaml` | YAML | finalizeplan |
| `problems-template.md` | Markdown | log-problem |
| `user-profile-template.md` | Markdown | onboard |
| `feature-yaml-template.yaml` | YAML | init-feature, feature-yaml skill |

Also: `assets/lens-bmad-skill-registry.json` — maps registered BMAD skill IDs to their module paths for the `bmad-lens-bmad-skill` wrapper.

---

## 8. Embedded Documentation — Complete Inventory

Located in `docs/`. Comprehensive reference documentation embedded in the module source. Not generated — shipped as curated guidance.

| Doc | Purpose |
|-----|---------|
| `GETTING-STARTED.md` | Quick start guide for new users |
| `architecture.md` | Module architectural overview |
| `component-inventory.md` | Full component listing |
| `configuration-examples.md` | bmadconfig.yaml example patterns |
| `copilot-adapter-reference.md` | GitHub Copilot adapter usage |
| `copilot-adapter-templates.md` | Adapter template content |
| `copilot-instructions.md` | Copilot `.github/instructions/` file content |
| `copilot-repo-instructions.md` | Copilot repo-scoped instructions |
| `development-guide.md` | Source contribution workflow |
| `index.md` | Documentation master index |
| `lex-persona.md` | Lens agent persona reference |
| `lifecycle-reference.md` | Lifecycle contract detailed reference |
| `lifecycle-visual-guide.md` | Lifecycle flowchart (visual) |
| `onboarding-checklist.md` | Step-by-step onboarding checklist |
| `pipeline-source-to-release.md` | CI/CD promotion pipeline |
| `preflight-strategy.md` | Preflight design rationale |
| `project-overview.md` | Module project overview |
| `project-scan-report.json` | Scan state (auto-updated by document-project) |
| `script-integration.md` | How AI skills invoke Python scripts |
| `source-tree-analysis.md` | Annotated directory tree |
| `understanding-your-workspace.md` | Workspace mental model guide |
| `v3.1-improvements.md` | v3.1 changelog reference |
| `whats-new.md` | What's new in current version |
| `source-project/` | Sub-folder for project source documentation |

---

## 9. Agents — `lens.agent.md` + `lens.agent.yaml`

### `lens.agent.md` — Dual-Section Structure

The agent file contains **two distinct definitions in one file**:

**Section 1: Thin Entry Shell**
- Loaded first by the IDE adapter
- Compact 6-item menu: Help (HP), Next (NX), Onboard (OB), Init Feature (IF), Chat (CH), Dismiss (DA)
- Each `exec` item points to the real SKILL.md — does not duplicate logic
- Activation: loads `bmadconfig.yaml`, `lifecycle.yaml`, `module-help.csv` → greets user → displays menu → waits
- Stops after menu display: does NOT auto-execute anything

**Section 2: Full Lifecycle Router**
- Loads all config fields as session variables
- Reads `feature-index.yaml` for active feature context
- Manages the full command surface via `/lens-{command}` routing

### `lens.agent.yaml`
- Separate YAML manifest declaring the agent's metadata for the BMAD module system

### Agent Activation Contract

```
Step 1: Load persona
Step 2: Load bmadconfig.yaml → store session variables
Step 3: Load lifecycle.yaml → ground routing
Step 4: Load module-help.csv → command discovery
Step 5: Greet user with name + language
Step 6: Display compact menu (6 items)
Step 7: Explain /lens-help and /lens-next
Step 8: STOP and WAIT for user input
```

---

## 10. Dependency Graph

### Skill → Script Dependencies

```
bmad-lens-next
  └── scripts/next-ops.py
        ├── lifecycle.yaml (read via Path(__file__).resolve().parents[3])
        └── feature.yaml (from governance repo)

bmad-lens-init-feature
  └── scripts/init-feature-ops.py
        ├── lifecycle.yaml (resolved via LIFECYCLE_PATH)
        ├── governance repo / feature-index.yaml
        ├── governance repo / features/.../feature.yaml
        └── governance repo / constitutions/

bmad-lens-git-orchestration
  └── scripts/git-orchestration-ops.py
        ├── feature.yaml (read-only reference for validation)
        └── git (subprocess, requires 2.28+)

bmad-lens-feature-yaml
  └── scripts/feature-yaml-ops.py
        ├── lifecycle.yaml (phase/track validation)
        └── governance repo / features/.../feature.yaml (read/write)

bmad-lens-constitution
  └── scripts/constitution-ops.py
        └── governance repo / constitutions/{org,domain,service,repo}/constitution.md

bmad-lens-complete
  └── scripts/complete-ops.py
        ├── bmad-lens-feature-yaml (via script delegation)
        ├── bmad-lens-document-project (delegated call)
        └── governance repo / features/.../feature-index.yaml
```

### Data Flow — Feature Creation

```
User: /init-feature
  → bmad-lens-init-feature SKILL.md loaded
  → scripts/init-feature-ops.py create
       ↓
  Governance repo (main):
    write feature.yaml → features/{domain}/{service}/{featureId}/
    write feature-index.yaml entry
    write summary.md stub
    write domain/service marker files
  Control repo:
    create {featureId} branch from default
    create {featureId}-plan branch from {featureId}
    push both branches to remote
  Return: featureId, branches, PR link
```

### Data Flow — Planning Phase

```
User: /expressplan
  → bmad-lens-expressplan SKILL.md
     → delegates to bmad-lens-bmad-skill
        → loads lens-bmad-skill-registry.json
        → delegates to bmad-lens-quickplan SKILL.md
           → scripts/quickplan-ops.py
              → businessplan phase (john + sally)
              → techplan phase (winston)
              → finalizeplan phase (lens conductor)
              → adversarial-review party mode
           ← returns planning artifacts staged in docs/{domain}/{service}/{featureId}/
        ← returns to expressplan
     → runs adversarial-review (fail halts)
     → updates feature.yaml phase to expressplan-complete
     → signals auto-advance to /finalizeplan
```

### Data Flow — Dev Phase

```
User: /dev --epic 1
  → bmad-lens-dev SKILL.md
     1. Publish staged FinalizePlan artifacts to governance docs mirror
        via bmad-lens-git-orchestration publish-to-governance --phase finalizeplan
     2. Load domain constitution via bmad-lens-constitution
     3. Load cross-feature context via bmad-lens-git-state
     4. Resolve target repo from feature.yaml.target_repos
     5. Present dev branch mode menu (if first run for this repo)
        → persist mode to repo-inventory.yaml + feature.yaml
     6. Prepare working branch via bmad-lens-git-orchestration prepare-dev-branch
     7. For each story in epic:
        → delegate to runSubagent (never inline)
        → per-task commits with Story/Task/Epic metadata
        → run adversarial code review after each story
        → write dev-session.yaml checkpoint
     8. Run dev-closeout adversarial review + party-mode challenge
     9. Create or reuse single final PR to target repo default branch
```

### Data Flow — Feature Completion

```
User: /complete
  → bmad-lens-complete SKILL.md
     → scripts/complete-ops.py check-preconditions
        ← validates: feature exists, phase is dev/complete, retrospective.md exists
     → runs bmad-lens-retrospective (if not already run)
     → delegates final docs to bmad-lens-document-project
        → writes to docs/{domain}/{service}/{featureId}/ (control repo)
        → mirrors to features/{domain}/{service}/{featureId}/docs/ (governance)
     → scripts/complete-ops.py finalize
        → updates feature.yaml phase to "complete"
        → updates feature-index.yaml status to "archived"
        → writes final summary.md
        → ATOMIC: all three updates committed together
```

---

## 11. Architectural Patterns and Design Decisions

### Pattern 1: AI-Skills as Intent + Script as Execution

Every skill that involves filesystem or git operations follows a strict split:
- **SKILL.md** = AI instructions (intent, communication style, vocabulary, decision rules)
- **`scripts/*.py`** = deterministic Python execution (validation, file writes, git operations)

Skills load then delegate: `"Run ./scripts/next-ops.py suggest first. Then: if status=fail..."`. The AI interprets results and decides next steps; the script provides ground truth.

### Pattern 2: Lifecycle-Derived Routing

No skill hardcodes phase-specific behavior. All routing derives from `lifecycle.yaml`:
- `auto_advance_to` — what command comes after a phase completes
- `completion_review` — which artifacts trigger the gate, which agents review
- `tracks[].phases` — which phases apply for this track
- `phases[].artifacts` — what artifacts are produced

This means adding a new phase requires only a `lifecycle.yaml` change; all skills that use `auto_advance_to` automatically pick it up.

### Pattern 3: Governance-First Atomicity

All feature state is on `main` in the governance repo at all times. The control repo holds only code branches. This means:
- `feature-index.yaml` always reflects the current feature set
- Governance artifacts are never in an intermediate state visible to other features
- "Atomic visibility" — a feature appears in `feature-index.yaml` the moment it is initialized, not after planning starts

### Pattern 4: 2-Branch Control Repo Invariant

Every feature has exactly `{featureId}` (base) + `{featureId}-plan` (planning) in the control repo. Target-repo working branches are separate and do not affect this invariant. The invariant is enforced in `git-orchestration-ops.py` before any branch creation.

### Pattern 5: Composable Skill Delegation

Skills delegate to each other rather than duplicating logic:
- `expressplan` → `bmad-lens-bmad-skill` → `bmad-lens-quickplan`
- `complete` → `bmad-lens-document-project` + `bmad-lens-retrospective`
- `dev` → `bmad-lens-git-orchestration` + `bmad-lens-constitution` + `bmad-lens-git-state`
- `onboard` → `preflight.py` (shared script, not a dedicated skill script)

### Pattern 6: Progressive Disclosure

Both `init-feature` and `constitution` implement progressive disclosure. `init-feature` asks only for name, domain, service before computing featureId. `constitution` shows only rules relevant to the current phase + track. This reduces cognitive load on users who are new to the system.

### Pattern 7: Idempotent Operations

Setup and provisioning scripts are designed to be re-run safely:
- `install.py` skips existing files unless `--update` is passed
- `target-repo-ops.py` verifies and reconciles rather than duplicating entries
- `init-feature-ops.py` detects duplicates in `feature-index.yaml` before creating anything

---

## 12. Contributor Checklist

### Before Adding or Modifying a Skill

- [ ] Does the skill's behavior derive from `lifecycle.yaml` (not hardcoded)?
- [ ] Is there a corresponding prompt file in `prompts/`?
- [ ] Is the skill registered in `module.yaml` under `skills`?
- [ ] Is the skill in `module-help.csv` (command discovery)?
- [ ] Is the skill in `agents/lens.agent.md` shell menu (if user-facing)?
- [ ] Does the skill have a test in `skills/{skill}/scripts/tests/`?

### Before Modifying a Script

- [ ] Does the script use `uv run --script` with inline metadata?
- [ ] Does the script use `argparse` and return structured output (JSON or exit code)?
- [ ] Does the script have `--dry-run` support if it writes files or branches?
- [ ] Do all path-constructing inputs validate against `SAFE_ID_PATTERN`?
- [ ] Do the corresponding tests cover the changed code path?

### Before Modifying `lifecycle.yaml`

- [ ] Does the change require a `schema_version` bump?
- [ ] Does the change affect `auto_advance_to`? (skills auto-pick this up — verify all phase conductors still route correctly)
- [ ] Does the change affect artifact validators? (check `validate-phase-artifacts.py` and `test-validate-phase-artifacts.py`)
- [ ] Is there a migration path for existing `feature.yaml` files already using old phase names?

### Risky Areas and Gotchas

| Area | Risk | Mitigation |
|------|------|-----------|
| `init-feature-ops.py` (1,832 LOC) | Most complex script; governs all feature creation | Run `test-init-feature-ops.py` + `test-normalize-feature-ids.py` after any change |
| `migrate-ops.py` (2,741 LOC) | Migration is irreversible if cleanup runs | Dry-run is mandatory; cleanup approval/receipt artifacts are required |
| `preflight.py` root resolution | Resolves to nearest `lens.core/` ancestor; breaks when run from inside `TargetProjects/` | Run targeted tests instead of preflight when validating source changes |
| `feature.yaml` TRACK_TRANSITIONS | Per-track phase transition rules; new tracks must add a full entry | Missing track key causes `KeyError` in `feature-yaml-ops.py` |
| `lifecycle.yaml` LIFECYCLE_PATH | Resolved as `Path(__file__).resolve().parents[3]` in script files | Breaks if script is moved more than 3 directories deep |
| `bmad-lens-dev` (no script) | Delegates all implementation via `runSubagent`; cannot be tested with unit tests | Relies on `test-phase-conductor-contracts.py` for contract coverage |
| Constitution hierarchy | Missing `org/constitution.md` breaks all resolution | Ensure org-level constitution exists in governance repo before any feature work |
| `create-pr.py` external dependency | `requests>=2.31` — not available in all environments | Always run with `uv run --script` which handles inline dependencies |

---

## 13. Version History Summary

| Version | Key Changes |
|---------|------------|
| v3.0 | Git-derived state replaces file-based state; no runtime YAML |
| v3.1 | Artifact validation hooks, parallel phases, content-aware sensing, dashboard, gate collapsing, artifact templates |
| v3.2 | Initiative root patterns added |
| v3.3 | Cross-feature visibility via `feature-index.yaml`, commit-and-publish, relationship conflict detection, required frontmatter |
| v3.4 | 2-branch topology, `featureId + featureId-plan` model, `feature.yaml` as optional state source, per-track topology config |
| v3.5 | FinalizePlan collapses DevProposal + SprintPlan into single post-TechPlan planning phase |
| v4.0 | Local personal state in `.lens/`, active generated docs under `docs/` (not legacy output folder), full Lens Next skill surface |
