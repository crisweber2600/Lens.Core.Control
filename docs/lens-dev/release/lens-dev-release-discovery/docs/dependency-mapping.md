# `_bmad/lens-work/` — Dependency Mapping and IPO Catalog

**Target:** `TargetProjects/lens-dev/release/lens.core.release/_bmad/lens-work/`
**Module version:** 4.0.0
**Scan date:** 2026-04-22
**Feature:** lens-dev-release-discovery

> **Note:** This dependency mapping documents the release distribution artifact. Because the release and source contain identical module logic, the component dependencies and IPO contracts are the same. Key differences are noted where the release differs structurally (pre-generated IDE adapters, bundled BMAD modules, no authoring scaffolding).

---

## Contents

1. [Component Dependency Overview](#1-component-dependency-overview)
2. [Shared Resource Map](#2-shared-resource-map)
3. [Cross-Component Interaction Matrix](#3-cross-component-interaction-matrix)
4. [IPO Catalog — Lifecycle Phase Conductors](#4-ipo-catalog--lifecycle-phase-conductors)
5. [IPO Catalog — Orchestration Skills](#5-ipo-catalog--orchestration-skills)
6. [IPO Catalog — Git Skills](#6-ipo-catalog--git-skills)
7. [IPO Catalog — Governance Skills](#7-ipo-catalog--governance-skills)
8. [IPO Catalog — Operational Scripts](#8-ipo-catalog--operational-scripts)
9. [IPO Catalog — Feature Lifecycle Management](#9-ipo-catalog--feature-lifecycle-management)
10. [IPO Catalog — End-to-End User Journeys](#10-ipo-catalog--end-to-end-user-journeys)

---

## 1. Component Dependency Overview

The diagram below shows which components call or delegate to which other components. Arrows indicate direction of dependency (`A → B` = A depends on / calls B).

```
USER COMMAND SURFACE (prompts/*.prompt.md)
            │
            ▼
    ┌───────────────────────────────────────────────────┐
    │          AGENT: lens.agent.md                     │
    │  (activation, routing, session-variable loading)  │
    └───────────────────┬───────────────────────────────┘
                        │ delegates to skill
                        ▼
    ╔══════════════════════════════════════════════════╗
    ║          ORCHESTRATION LAYER                     ║
    ║  bmad-lens-next ─────────────────────────────►  ║
    ║    (routing)         bmad-lens-init-feature      ║
    ║                      bmad-lens-switch            ║
    ║                      bmad-lens-quickplan         ║
    ║                      bmad-lens-batch             ║
    ╚══════════════════════════════════════════════════╝
                        │
          ┌─────────────┼──────────────┐
          ▼             ▼              ▼
  ┌─────────────┐ ┌──────────┐ ┌────────────────┐
  │ PHASE       │ │ GIT      │ │ GOVERNANCE     │
  │ CONDUCTORS  │ │ SKILLS   │ │ SKILLS         │
  │             │ │          │ │                │
  │ preplan     │ │ git-state│ │ constitution   │
  │ businessplan│ │ git-     │ │ sensing        │
  │ techplan    │ │ orchest- │ │ adversarial-   │
  │ finalizeplan│ │ ration   │ │   review       │
  │ expressplan │ │          │ │ feature-yaml   │
  └──────┬──────┘ └────┬─────┘ └───────┬────────┘
         │             │               │
         └─────────────┼───────────────┘
                       │ invoke scripts
                       ▼
    ╔══════════════════════════════════════════════════╗
    ║            SCRIPT LAYER (scripts/*.py)           ║
    ║  next-ops       git-orchestration-ops            ║
    ║  init-feature-ops  feature-yaml-ops              ║
    ║  constitution-ops  complete-ops                  ║
    ║  quickplan-ops  switch-ops  migrate-ops ...      ║
    ╚══════════════════════════════════════════════════╝
                        │ reads / writes
                        ▼
    ╔══════════════════════════════════════════════════╗
    ║             SHARED DATA LAYER                    ║
    ║  lifecycle.yaml   feature.yaml                   ║
    ║  feature-index.yaml  constitutions/              ║
    ║  Git branches (control + governance + target)    ║
    ╚══════════════════════════════════════════════════╝
```

### Delegation Chains

| Entry Point | → First Delegate | → Second Delegate | → Third |
|-------------|-----------------|-------------------|---------|
| `/expressplan` | `bmad-lens-expressplan` | `bmad-lens-bmad-skill` | `bmad-lens-quickplan` → `quickplan-ops.py` |
| `/complete` | `bmad-lens-complete` | `bmad-lens-document-project` + `bmad-lens-retrospective` | `complete-ops.py` |
| `/dev` | `bmad-lens-dev` | `bmad-lens-git-orchestration` + `bmad-lens-constitution` + `bmad-lens-git-state` | `git-orchestration-ops.py` + `constitution-ops.py` |
| `/next` | `bmad-lens-next` | `next-ops.py` → auto-delegates to target skill | target skill → its scripts |
| `/onboard` | `bmad-lens-onboard` | `preflight.py` | (subprocess) |
| `/migrate` | `bmad-lens-migrate` | `migrate-ops.py` | (git subprocess + feature-yaml-ops patterns) |

---

## 2. Shared Resource Map

Files and data stores that are read or written by more than one component. Read access is denoted `R`, write access `W`.

### `lifecycle.yaml`

| Component | Access | What It Uses |
|-----------|--------|-------------|
| `lens.agent.md` | R | Phase list, track list, auto_advance_to, help routing |
| `bmad-lens-next` / `next-ops.py` | R | Phase routing table, auto_advance_to per phase, track definitions |
| `bmad-lens-feature-yaml` / `feature-yaml-ops.py` | R | VALID_PHASES, VALID_TRACKS, TRACK_TRANSITIONS dict |
| `bmad-lens-expressplan` | R | expressplan phase definition, finalizeplan auto-advance |
| `bmad-lens-finalizeplan` | R | finalizeplan step contract, dev-ready milestone definition |
| `bmad-lens-migrate` / `migrate-ops.py` | R | schema_version for detecting version mismatch |
| `bmad-lens-upgrade` | R | schema_version, breaking-change flag, migration paths |
| `validate-phase-artifacts.py` | R | artifact lists per phase, required_sections, min_word_count |
| `bmad-lens-adversarial-review` | R | completion_review config, party-mode participants per artifact |

### `feature.yaml` (per-feature, in governance repo)

| Component | Access | What It Reads/Writes |
|-----------|--------|---------------------|
| `bmad-lens-init-feature` / `init-feature-ops.py` | W(create) | Creates the full feature.yaml skeleton |
| `bmad-lens-feature-yaml` / `feature-yaml-ops.py` | R+W | Full CRUD on feature.yaml; phase transitions; team/repo updates |
| `bmad-lens-next` / `next-ops.py` | R | phase, track, open_problems, staleness flags, last_updated |
| `bmad-lens-git-orchestration` / `git-orchestration-ops.py` | R+W | Reads for branch validation; writes target_repos, dev_branch_mode |
| `bmad-lens-complete` / `complete-ops.py` | R+W | Reads phase/milestone; writes phase=complete, archived flag |
| `bmad-lens-pause-resume` / `pause-resume-ops.py` | R+W | Reads current phase; writes paused/resumed state |
| `bmad-lens-sensing` | R | featureId, domain, service, scope fields for overlap detection |
| `bmad-lens-switch` / `switch-ops.py` | R | featureId, phase, last_updated for context display |
| `bmad-lens-dev` | R+W | Reads target_repos + dev_branch_mode; writes dev-session.yaml via stage |
| `bmad-lens-target-repo` / `target-repo-ops.py` | R+W | Reads target_repos list; appends newly provisioned repo entry |
| `bmad-lens-move-feature` / `move-feature-ops.py` | R+W | Reads domain/service; rewrites path fields and updates governance paths |
| `bmad-lens-split-feature` / `split-feature-ops.py` | R | Reads scope, epics, stories to identify split boundary |

### `feature-index.yaml` (in governance repo root)

| Component | Access | What It Uses |
|-----------|--------|-------------|
| `bmad-lens-init-feature` / `init-feature-ops.py` | R+W | Reads to check for duplicates; appends new entry |
| `bmad-lens-complete` / `complete-ops.py` | R+W | Reads feature entry; updates status to `archived` |
| `bmad-lens-switch` / `switch-ops.py` | R | Lists all features; resolves featureId from partial name |
| `bmad-lens-dashboard` / `dashboard-ops.py` | R | Reads all entries for dashboard generation |
| `bmad-lens-sensing` | R | Reads all active features for overlap detection |
| `lens.agent.md` | R | Loads active feature context at session start |
| `bmad-lens-split-feature` / `split-feature-ops.py` | R+W | Reads parent; appends child feature entry |
| `bmad-lens-move-feature` / `move-feature-ops.py` | R+W | Reads entry; rewrites domain/service/path fields |
| `bmad-lens-discover` / `discover-ops.py` | R+W | Syncs local TargetProjects against governance inventory |

### Git Branches (Control Repo)

| Component | Branches Touched | Operation |
|-----------|-----------------|-----------|
| `bmad-lens-init-feature` / `init-feature-ops.py` | `{featureId}`, `{featureId}-plan` | Create and push |
| `bmad-lens-git-orchestration` / `git-orchestration-ops.py` | `{featureId}`, `{featureId}-plan`, `{featureId}-dev-{user}` | Create, commit, push, merge |
| `bmad-lens-git-state` / `git-state-ops.py` | all feature branches | Read-only query |
| `bmad-lens-sensing` | all active branches | Read-only scan |
| `bmad-lens-migrate` / `migrate-ops.py` | legacy v3 branches, new `{featureId}` + `{featureId}-plan` | Rename, create |
| `bmad-lens-finalizeplan` | `{featureId}-plan → {featureId}` (PR), `{featureId} → main` (PR) | PR creation |

### Git Branches (Target Repo)

| Component | Branch Touched | Operation |
|-----------|---------------|-----------|
| `bmad-lens-git-orchestration` / `prepare-dev-branch` | `feature/{featureId}[-{username}]` or default | Create, checkout |
| `bmad-lens-dev` | inherits from git-orchestration | Write via delegated git-orchestration |
| `bmad-lens-target-repo` / `target-repo-ops.py` | clones default branch | Clone, inventory update |

### `constitutions/` Hierarchy (Governance Repo)

| Component | Access | What It Reads |
|-----------|--------|--------------|
| `bmad-lens-constitution` / `constitution-ops.py` | R | org, domain, service, repo constitution files |
| `bmad-lens-dev` | R (delegated) | Via bmad-lens-constitution |
| `bmad-lens-init-feature` / `init-feature-ops.py` | R | Service constitution for validation rules at init |
| `bmad-lens-adversarial-review` | R (via constitution) | Phase-specific gate rules |

### `assets/templates/` (Module Source)

| Template | Read By |
|----------|---------|
| `product-brief-template.md` | `bmad-lens-preplan`, `bmad-lens-quickplan` |
| `prd-template.md` | `bmad-lens-businessplan`, `bmad-lens-quickplan` |
| `ux-design-template.md` | `bmad-lens-businessplan` |
| `architecture-template.md` | `bmad-lens-techplan`, `bmad-lens-quickplan` |
| `epics-template.md` | `bmad-lens-finalizeplan` |
| `stories-template.md` | `bmad-lens-finalizeplan` |
| `feature-yaml-template.yaml` | `init-feature-ops.py` |
| `problems-template.md` | `bmad-lens-log-problem` / `log-problem-ops.py` |
| `user-profile-template.md` | `bmad-lens-onboard` / `onboard-ops.py` |
| `sprint-status-template.yaml` | `bmad-lens-finalizeplan` |
| `implementation-readiness-template.md` | `bmad-lens-finalizeplan` |

---

## 3. Cross-Component Interaction Matrix

Rows = caller; columns = callee. `D` = delegates (skill → skill), `S` = invokes script, `R` = reads file, `W` = writes file.

| Caller ↓ / Callee → | `init-feature-ops` | `git-orch-ops` | `git-state-ops` | `feature-yaml-ops` | `constitution-ops` | `next-ops` | `quickplan-ops` | `complete-ops` | `migrate-ops` | `lifecycle.yaml` | `feature.yaml` | `feature-index` |
|---|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
| `bmad-lens-next` | | | | | | S | | | | R | R | |
| `bmad-lens-init-feature` | S | | | | R | | | | | R | W | W |
| `bmad-lens-expressplan` | | | | | | | D | | | R | R | |
| `bmad-lens-quickplan` | | | | | | | S | | | R | R | |
| `bmad-lens-finalizeplan` | | S | S | S | | | | | | R | W | |
| `bmad-lens-dev` | | D | D | | D | | | | | R | W | |
| `bmad-lens-complete` | | | | D | | | | S | | R | W | W |
| `bmad-lens-git-orchestration` | | S | | | | | | | | R | W | |
| `bmad-lens-git-state` | | | S | | | | | | | R | R | |
| `bmad-lens-feature-yaml` | | | | S | | | | | | R | R+W | |
| `bmad-lens-constitution` | | | | | S | | | | | R | R | |
| `bmad-lens-sensing` | | | | | | | | | | R | R | R |
| `bmad-lens-adversarial-review` | | | | | R | | | | | R | W | |
| `bmad-lens-migrate` | | | | | | | | | S | R | W | W |
| `bmad-lens-switch` | | | | | | | | | | R | R | R |
| `bmad-lens-dashboard` | | | | | | | | | | R | R | R |
| `bmad-lens-pause-resume` | | | | D | | | | | | R | W | |
| `bmad-lens-target-repo` | | | | | | | | | | R | W | |
| `bmad-lens-log-problem` | | | | | | | | | | R | W | |

---

## 4. IPO Catalog — Lifecycle Phase Conductors

### `bmad-lens-preplan`

| | |
|--|--|
| **Input** | • User command `/preplan` <br> • `feature.yaml` (current phase = `preplan` or unset) <br> • `lifecycle.yaml` (phase definition, artifact list, template references) <br> • `assets/templates/product-brief-template.md` <br> • `constitutions/` (org → domain rules for this phase) |
| **Process** | 1. Confirm feature context (load `feature.yaml`) <br> 2. Load constitution rules for `preplan` phase <br> 3. Delegate to **mary** (Analyst agent) for brainstorming → research → product-brief <br> 4. On user signal: invoke adversarial review (party mode: john + winston + sally) <br> 5. On `pass`: update feature.yaml phase to `preplan-complete`; surface `/businessplan` |
| **Output** | • `docs/{domain}/{service}/{featureId}/product-brief.md` (staged in control repo) <br> • `docs/{domain}/{service}/{featureId}/research.md` (optional) <br> • `docs/{domain}/{service}/{featureId}/brainstorm.md` (optional) <br> • `feature.yaml` phase = `preplan-complete` <br> • Adversarial review artifact: `preplan-adversarial-review.md` |
| **Hard Gates** | Adversarial review verdict `fail` → blocks phase completion |
| **Calls** | `bmad-lens-adversarial-review` (review gate), `bmad-lens-feature-yaml` (phase update) |

---

### `bmad-lens-businessplan`

| | |
|--|--|
| **Input** | • User command `/businessplan` <br> • `feature.yaml` (phase must be `preplan-complete` or track start) <br> • `lifecycle.yaml` (phase definition, artifact validator settings) <br> • `assets/templates/prd-template.md` <br> • `assets/templates/ux-design-template.md` <br> • Prior artifacts: `product-brief.md` (if preplan was run) |
| **Process** | 1. Confirm phase gate (previous phase complete per track) <br> 2. Load constitution rules for `businessplan` <br> 3. Delegate to **john** (PM) for PRD creation; **sally** (UX) for UX design <br> 4. `must_reference` check: prd must reference product-brief; ux-design must reference prd <br> 5. Adversarial review party mode: prd (winston lead), ux-design (john lead) <br> 6. On `pass`: update `feature.yaml` phase → `businessplan-complete`; signal `/techplan` |
| **Output** | • `prd.md` staged in control repo docs path <br> • `ux-design.md` staged in control repo docs path <br> • `businessplan-adversarial-review.md` <br> • `feature.yaml` phase = `businessplan-complete` |
| **Hard Gates** | Missing prd OR ux-design → blocks completion. Adversarial `fail` → blocks completion |
| **Calls** | `bmad-lens-adversarial-review`, `bmad-lens-feature-yaml`, `validate-phase-artifacts.py` |

---

### `bmad-lens-techplan`

| | |
|--|--|
| **Input** | • User command `/techplan` <br> • `feature.yaml` (phase gate) <br> • `lifecycle.yaml` (techplan phase definition) <br> • `assets/templates/architecture-template.md` <br> • Prior artifacts: `prd.md`, `ux-design.md` |
| **Process** | 1. Phase gate: businessplan-complete (or track start for `tech-change`) <br> 2. Constitution rules for techplan <br> 3. Delegate to **winston** (Architect) for architecture document <br> 4. `must_reference` check: architecture must reference prd <br> 5. Adversarial review (john lead, mary + bob supporting) <br> 6. On `pass`: `feature.yaml` → `techplan-complete`; signal `/finalizeplan` |
| **Output** | • `architecture.md` staged in control repo <br> • `techplan-adversarial-review.md` <br> • `feature.yaml` phase = `techplan-complete` |
| **Hard Gates** | Adversarial `fail` blocks. Missing cross-reference in architecture blocks |
| **Calls** | `bmad-lens-adversarial-review`, `bmad-lens-feature-yaml`, `validate-phase-artifacts.py` |

---

### `bmad-lens-finalizeplan`

| | |
|--|--|
| **Input** | • User command `/finalizeplan` <br> • `feature.yaml` (techplan-complete required) <br> • `lifecycle.yaml` (finalizeplan 3-step contract) <br> • Staged artifacts: `prd.md`, `ux-design.md`, `architecture.md` <br> • `assets/templates/` (epics, stories, implementation-readiness, sprint-status) <br> • `git-orchestration-ops.py` (for commit + PR creation) <br> • `create-pr.py` (for PR via GitHub REST) |
| **Process** | **Step 1 — review-and-push:** <br> 1a. Adversarial review of all planning artifacts (party mode) <br> 1b. Commit all staged artifacts to `{featureId}-plan` branch <br> 1c. Push branch to remote <br> **Step 2 — plan-pr-readiness:** <br> 2a. Create/verify PR: `{featureId}-plan → {featureId}` <br> 2b. Confirm PR URL, update feature.yaml with PR reference <br> **Step 3 — downstream-bundle-and-final-pr:** <br> 3a. Generate epics.md + stories.md + story files from architecture <br> 3b. Generate implementation-readiness.md + sprint-status.yaml <br> 3c. Commit bundle to `{featureId}-plan` <br> 3d. Create final PR: `{featureId} → main` <br> 3e. Set `dev-ready` milestone in feature.yaml |
| **Output** | • `epics.md`, `stories.md`, `story-{n}.md` files <br> • `implementation-readiness.md`, `sprint-status.yaml` <br> • `finalizeplan-adversarial-review.md` <br> • PR: `{featureId}-plan → {featureId}` with URL in feature.yaml <br> • PR: `{featureId} → main` with URL in feature.yaml <br> • `feature.yaml` milestone = `dev-ready` |
| **Hard Gates** | Adversarial review fail on any artifact blocks. Missing PRD/architecture cross-reference blocks |
| **Calls** | `bmad-lens-adversarial-review`, `git-orchestration-ops.py`, `create-pr.py`, `feature-yaml-ops.py`, `validate-phase-artifacts.py` |

---

### `bmad-lens-expressplan`

| | |
|--|--|
| **Input** | • User command `/expressplan` <br> • `feature.yaml` (track must be `express`) <br> • `lifecycle.yaml` (expressplan 3-step contract) <br> • `assets/lens-bmad-skill-registry.json` (maps skill id → module path) |
| **Process** | **Step 1 — quickplan-via-lens-wrapper:** <br> 1a. Load feature context + governance rules <br> 1b. Delegate to `bmad-lens-bmad-skill` with target = `bmad-lens-quickplan` <br> 1c. `quickplan-ops.py` runs full planning pipeline (business + tech + finalize) <br> **Step 2 — adversarial-review-party-mode:** <br> 2a. Run adversarial review on quickplan output; `fail` → halt <br> **Step 3 — advance-to-finalizeplan:** <br> 3a. Update `feature.yaml` phase = `expressplan-complete` <br> 3b. Signal auto-advance to `/finalizeplan` |
| **Output** | • All quickplan artifacts (product-brief, prd, ux-design, architecture, epics, stories) <br> • `expressplan-adversarial-review.md` <br> • `feature.yaml` phase = `expressplan-complete` |
| **Hard Gates** | Adversarial `fail` in Step 2 → halt (no advance) |
| **Calls** | `bmad-lens-bmad-skill` → `bmad-lens-quickplan` → `quickplan-ops.py`; `bmad-lens-adversarial-review`; `feature-yaml-ops.py` |

---

## 5. IPO Catalog — Orchestration Skills

### `bmad-lens-next`

| | |
|--|--|
| **Input** | • Current `feature.yaml` (phase, track, milestones, open_problems, staleness) <br> • `lifecycle.yaml` (auto_advance_to per phase, phase_order, track definitions) <br> • (optional) `feature-index.yaml` for feature list when no active feature |
| **Process** | 1. `next-ops.py suggest` — reads feature.yaml + lifecycle.yaml <br> 2. Evaluates hard gates: open_problems > 3, missing required artifacts, milestone not set <br> 3. Maps phase + track → recommended next command via `auto_advance_to` <br> 4. If unblocked: immediately delegates to the target skill (no menu printed) <br> 5. If blocked: displays blocking reason + unblock instructions |
| **Output** | • If unblocked: delegation to the next skill in the lifecycle chain <br> • If blocked: structured block report (reason, unblock path) <br> • No file writes |
| **Hard Gates** | open_problems > 3, stale feature (no update in > 14 days), missing milestone |
| **Calls** | `next-ops.py`; delegates to any lifecycle skill based on routing result |

---

### `bmad-lens-init-feature`

| | |
|--|--|
| **Input** | • User command `/init-feature` with name, domain, service inputs <br> • `lifecycle.yaml` (track options, phase definitions) <br> • `feature-index.yaml` (duplicate detection) <br> • Governance `constitutions/` (service constitution for validation rules) <br> • `assets/templates/feature-yaml-template.yaml` |
| **Process** | 1. Collect: feature name, domain, service (progressive disclosure — 3 fields only first) <br> 2. Derive: `featureId = {domain}-{service}-{featureSlug}` (lowercase, hyphenated) <br> 3. Confirm featureId + featureSlug with user <br> 4. Present track options from `lifecycle.yaml`; require explicit user selection <br> 5. Check `feature-index.yaml` for duplicates; abort if found <br> 6. Validate: `featureSlug` matches `^[a-z0-9][a-z0-9-]{0,63}$` <br> 7. Write governance artifacts atomically: `feature.yaml`, `feature-index.yaml` entry, `summary.md` stub, domain/service marker files <br> 8. Create control repo branches: `{featureId}` from default, `{featureId}-plan` from `{featureId}` <br> 9. Push both branches to remote |
| **Output** | • `governance/features/{domain}/{service}/{featureId}/feature.yaml` (created) <br> • `governance/feature-index.yaml` (entry appended) <br> • `governance/features/{domain}/{service}/{featureId}/summary.md` (stub) <br> • Governance domain/service marker files (if new) <br> • Control repo: `{featureId}` branch pushed <br> • Control repo: `{featureId}-plan` branch pushed <br> • (Optional) TargetProjects scaffold `.gitkeep` files |
| **Hard Gates** | Duplicate featureId in feature-index → abort. Invalid featureSlug pattern → abort. Missing governance repo → abort |
| **Calls** | `init-feature-ops.py`; `constitution-ops.py` (service constitution); git subprocess |

---

### `bmad-lens-quickplan`

| | |
|--|--|
| **Input** | • `feature.yaml` (featureId, domain, service, track, phase) <br> • `lifecycle.yaml` (phase definitions for businessplan, techplan, finalizeplan) <br> • `assets/templates/` (product-brief, prd, ux-design, architecture, epics, stories) <br> • `--mode interactive|batch` flag |
| **Process** | **Interactive mode:** <br> 1. Two-document rule: always creates business-plan.md + tech-plan.md as separate files <br> 2. Runs businessplan, techplan, finalizeplan phase contracts sequentially <br> 3. Each phase: template → AI fill → validate → adversarial review <br> **Batch mode:** <br> 1. Pass 1: writes `quickplan-batch-input.md` with all prompts, stops <br> 2. Pass 2: resumes only after user-approved answers are loaded <br> 3. Continues pipeline with approved answers |
| **Output** | • `business-plan.md` (always separate from tech-plan.md) <br> • `tech-plan.md` <br> • All standard finalizeplan artifacts (epics, stories, implementation-readiness) <br> • Review artifacts per phase |
| **Hard Gates** | Adversarial review `fail` at any phase blocks continuation |
| **Calls** | `quickplan-ops.py`; inherits phase conductor contracts (does NOT bypass them) |

---

### `bmad-lens-switch`

| | |
|--|--|
| **Input** | • `feature-index.yaml` (full feature list) <br> • `feature.yaml` for each candidate feature (phase, last_updated, domain/service) <br> • Optional `--feature-id` to switch directly |
| **Process** | 1. `switch-ops.py switch` — loads feature-index <br> 2. Presents features grouped by phase/status <br> 3. On selection: loads full context from `feature.yaml` + governance docs <br> 4. Sets active feature context for current session |
| **Output** | • Active feature context for current agent session (in-memory) <br> • No file writes (read-only operation) |
| **Hard Gates** | None |
| **Calls** | `switch-ops.py`; reads `feature-index.yaml` + `feature.yaml` |

---

## 6. IPO Catalog — Git Skills

### `bmad-lens-git-state`

| | |
|--|--|
| **Input** | • Feature context (featureId from `feature.yaml` or session) <br> • Git repository state (branches, remote tracking, commit history) <br> • `feature.yaml` (lifecycle state for comparison against git reality) |
| **Process** | 1. `git-state-ops.py feature-state` — reads feature.yaml AND git branch list <br> 2. Compares declared lifecycle state vs actual branch existence <br> 3. Detects discrepancies (e.g., feature.yaml says `dev` but no dev branch exists) <br> 4. Reports discrepancy explicitly — never silently reconciles <br> 5. `git-state-ops.py branches` — existence queries by name or pattern <br> 6. `git-state-ops.py active-features` — workspace-wide branch scan |
| **Output** | • Structured state report (JSON or display) <br> • Discrepancy report if lifecycle state ≠ git reality <br> • No writes — strictly read-only |
| **Hard Gates** | None (advisory output only; callers decide what to do with discrepancies) |
| **Calls** | `git-state-ops.py`; git subprocess (read-only) |

---

### `bmad-lens-git-orchestration`

| | |
|--|--|
| **Input** | • `feature.yaml` (featureId, phase, target_repos, dev_branch_mode) <br> • `lifecycle.yaml` (PHASE_ARTIFACTS dict for structured commit messages) <br> • Staged control repo docs (for `commit-artifacts`) <br> • `--feature-id`, `--phase`, `--username`, `--mode` flags per subcommand |
| **Process** | **`create-feature-branches`:** <br> → Verify 2-branch invariant (abort if branches already exist) <br> → Create `{featureId}` from default branch <br> → Create `{featureId}-plan` from `{featureId}` <br> → Push both to remote <br><br> **`commit-artifacts`:** <br> → Stage specified docs files <br> → Build commit message with phase/artifact metadata <br> → Commit to current branch (must be `{featureId}-plan`) <br><br> **`create-dev-branch`:** <br> → Create `{featureId}-dev-{username}` from `{featureId}` <br> → Push to remote <br><br> **`prepare-dev-branch`:** <br> → Read `dev_branch_mode` from feature.yaml (or ask user if first run) <br> → Mode `direct-default`: use target repo default branch <br> → Mode `feature-id`: create `feature/{featureId}` in target repo <br> → Mode `feature-id-username`: create `feature/{featureId}-{username}` in target repo <br> → Persist mode to `repo-inventory.yaml` + `feature.yaml` <br><br> **`merge-plan`:** <br> → Merge `{featureId}-plan` into `{featureId}` <br><br> **`publish-to-governance`:** <br> → Copy staged planning docs to governance `features/.../docs/` mirror |
| **Output** | • Control repo branches created/pushed (per subcommand) <br> • Commits with structured metadata <br> • Target repo working branch created (prepare-dev-branch) <br> • Governance docs mirror updated (publish-to-governance) <br> • `feature.yaml` `dev_branch_mode` field updated |
| **Hard Gates** | 2-branch invariant check: aborts if `{featureId}` or `{featureId}-plan` already exist when creating. Branch must exist before committing. |
| **Calls** | `git-orchestration-ops.py`; git subprocess (2.28+); `feature-yaml-ops.py` (for mode persistence) |

---

## 7. IPO Catalog — Governance Skills

### `bmad-lens-constitution`

| | |
|--|--|
| **Input** | • `feature.yaml` (domain, service, org for path construction) <br> • `governance/constitutions/{org}/constitution.md` <br> • `governance/constitutions/{org}/{domain}/constitution.md` <br> • `governance/constitutions/{org}/{domain}/{service}/constitution.md` <br> • `governance/constitutions/{org}/{domain}/{service}/{repo}/constitution.md` <br> • Current phase + track (for progressive disclosure filtering) |
| **Process** | 1. `constitution-ops.py` resolves all 4 levels of the hierarchy <br> 2. Merges rules additively (lower levels add, never remove) <br> 3. Filters to rules relevant to current phase + track only <br> 4. Classifies each rule: `hard gate` (blocking) vs `informational gate` (advisory) <br> 5. Reports missing levels as gaps — does not fabricate defaults |
| **Output** | • Merged constitution rules for current context (in-memory, presented to agent) <br> • Gap report if any constitution level file is missing <br> • Classification: which rules are hard gates vs informational |
| **Hard Gates** | Missing org-level constitution → reports critical gap (callers must decide to proceed) |
| **Calls** | `constitution-ops.py` |

---

### `bmad-lens-adversarial-review`

| | |
|--|--|
| **Input** | • Staged planning artifacts for the current phase (from control repo docs path) <br> • `lifecycle.yaml` (completion_review config: lead agent, participants, focus questions per artifact) <br> • `constitutions/` (phase-specific gate rules) <br> • `--phase`, `--source`, `--feature-id` flags |
| **Process** | 1. Load artifact list for the phase from `lifecycle.yaml` completion_review config <br> 2. For each artifact: assign lead + participant reviewers per party-mode table <br> 3. Each reviewer evaluates independently (blind to others' results) <br> 4. Aggregate: `pass` requires all participants to pass; any `fail` → overall `fail` <br> 5. Mandatory blind-spot challenge round after aggregate result <br> 6. Write review artifact to control repo <br> 7. If `pass`: signal phase completion (caller can advance) <br> 8. If `fail`: halt; do NOT advance lifecycle state |
| **Output** | • `{phase}-adversarial-review.md` written to control repo docs path <br> • Verdict: `pass` or `fail` (structured, parseable by calling skill) |
| **Hard Gates** | Any participant `fail` → overall `fail` → caller MUST NOT advance phase |
| **Calls** | `validate-phase-artifacts.py` (optional structural check before review); reads `lifecycle.yaml` constitution; reads staged artifacts |

---

### `bmad-lens-sensing`

| | |
|--|--|
| **Input** | • `feature-index.yaml` (all active features) <br> • `feature.yaml` files for all active features (scope, domain, service, epics) <br> • `lifecycle.yaml` (content_aware_sensing settings: cosine threshold = 0.7) <br> • Active git branches in control repo (for scope conflict detection) <br> • `--enforce` flag (optional) |
| **Process** | 1. Scan all active initiatives from feature-index + git branches <br> 2. Structural overlap: same domain+service+scope signals SCOPE_OVERLAP <br> 3. Name conflict: two features with similar featureId signals NAME_CONFLICT <br> 4. Content-aware: cosine similarity (0.7 threshold) across epics, stories, architecture docs — triggers if similarity > threshold <br> 5. Resource clash: same target repo + overlapping development windows → RESOURCE_CLASH <br> 6. Report: advisory by default; hard gate only with `--enforce` or constitution declaration |
| **Output** | • Conflict report: `[SCOPE_OVERLAP | NAME_CONFLICT | RESOURCE_CLASH]` with affected featureIds <br> • No file writes |
| **Hard Gates** | Only when `--enforce` flag is set or constitution declares sensing as mandatory gate |
| **Calls** | `git-state-ops.py` (branch scan); reads `feature-index.yaml` + `feature.yaml` per feature |

---

### `bmad-lens-feature-yaml`

| | |
|--|--|
| **Input** | • `feature.yaml` file path (derived from featureId + governance repo path) <br> • Subcommand: `create`, `read`, `update`, `validate`, `list`, `transition` <br> • `lifecycle.yaml` (VALID_PHASES, VALID_TRACKS, TRACK_TRANSITIONS) |
| **Process** | **`create`:** Builds feature.yaml from template + user inputs; validates all fields <br> **`read`:** Loads and displays feature.yaml in structured format <br> **`update`:** Applies field updates with schema validation before writing <br> **`validate`:** Checks all required fields, phase validity for track, cross-field consistency <br> **`transition`:** Validates transition is allowed per TRACK_TRANSITIONS dict; applies new phase <br> **`list`:** Reads feature-index.yaml; presents all features with current phase |
| **Output** | • `feature.yaml` created or updated (write operations) <br> • Structured display of feature state (read operations) <br> • Validation report with specific field errors |
| **Hard Gates** | Invalid phase transition per TRACK_TRANSITIONS → abort. Missing required fields → abort |
| **Calls** | `feature-yaml-ops.py`; reads `lifecycle.yaml` for schema constants |

---

## 8. IPO Catalog — Operational Scripts

### `preflight.py`

| | |
|--|--|
| **Input** | • `--caller {onboard|light|check}` flag <br> • Workspace root (nearest ancestor containing `lens.core/`) <br> • `lifecycle.yaml` (schema_version field) <br> • Governance repo: existence, main branch, connectivity <br> • System: git availability, Python version |
| **Process** | 1. Resolve workspace root (nearest `lens.core/` ancestor — **gotcha**: resolves to `TargetProjects/` when run from inside it) <br> 2. Validate: git >= 2.28, Python >= 3.10 <br> 3. Validate: `lifecycle.yaml` schema_version matches expected <br> 4. Validate: governance repo exists + accessible <br> 5. Validate: no uncommitted dangerous changes in control repo <br> 6. Run `run-preflight-cached.py` to skip redundant checks |
| **Output** | • Exit code 0 (all checks pass) or non-zero (failure) <br> • Structured failure report listing all failed checks <br> • No file writes |
| **Hard Gates** | Any failed check → non-zero exit; `bmad-lens-onboard` displays failure and stops |
| **Calls** | git subprocess; `run-preflight-cached.py` |

---

### `install.py`

| | |
|--|--|
| **Input** | • `--ide {github-copilot|cursor|claude|codex|opencode}` flag <br> • `--dry-run` flag (optional) <br> • `--update` flag (optional, allows overwriting existing files) <br> • Module source files in `_bmad/lens-work/` |
| **Process** | 1. Load IDE-specific adapter manifest from `ides/` config <br> 2. For each file in adapter manifest: <br>   a. Check if target exists; skip if `--update` not set <br>   b. Copy or template-render the file to the target location (`.github/` or IDE-specific path) <br> 3. Track: `_created`, `_skipped`, `_errors`, `_removed` counters <br> 4. If `--dry-run`: print what would happen, exit 0 with no writes |
| **Output** | • IDE adapter files installed in target locations (e.g., `.github/instructions/`, `.github/prompts/`) <br> • Summary: files created/skipped/errored/removed <br> • Dry-run: plan report only |
| **Hard Gates** | None (errors are counted but do not abort; reported at end) |
| **Calls** | File I/O only; no subprocess |

---

### `setup-control-repo.py`

| | |
|--|--|
| **Input** | • Control repo root folder name (verbatim — used to derive governance repo name) <br> • `--dry-run` flag (optional) <br> • git and `gh` CLI availability |
| **Process** | 1. Derive governance repo name: `{control-repo-root-name}.governance` (verbatim, no transformation) <br> 2. Check if governance repo exists on remote; if not: `gh repo create` to auto-create <br> 3. Clone governance repo to `TargetProjects/lens/` <br> 4. Clone target repos listed in `repo-inventory.yaml` to canonical `TargetProjects/{domain}/{service}/` paths <br> 5. Run `install.py` to deploy IDE adapters <br> 6. Copy `.github/` adapter to the new control repo |
| **Output** | • Governance repo cloned to `TargetProjects/lens/{governance-repo-name}/` <br> • Target repos cloned to canonical paths <br> • `.github/` adapter installed <br> • Dry-run: plan report only |
| **Hard Gates** | `gh` unavailable + governance repo doesn't exist → error (cannot auto-create without `gh`) |
| **Calls** | `install.py`; git subprocess; `gh` subprocess |

---

### `create-pr.py`

| | |
|--|--|
| **Input** | • `--head-branch`, `--base-branch`, `--title`, `--body` flags <br> • `GITHUB_TOKEN` env var (PAT for REST API auth) <br> • Remote URL (parsed to detect: GitHub HTTPS, GitHub SSH, Azure DevOps) |
| **Process** | 1. Parse remote URL to detect provider + org + repo <br> 2. If `GITHUB_TOKEN` available: POST to GitHub REST `/repos/{owner}/{repo}/pulls` <br> 3. If no token: construct manual URL for PR creation and display to user <br> 4. Full error reporting: HTTP status, response body, rate limit headers |
| **Output** | • PR URL (created or manual) printed to stdout <br> • PR object (title, number, URL) in structured output |
| **Hard Gates** | Invalid remote URL → error. API error → display response + manual fallback URL |
| **Calls** | `requests` HTTP library; git subprocess (to read remote URL) |

---

### `validate-phase-artifacts.py`

| | |
|--|--|
| **Input** | • `--phase` flag <br> • `--feature-docs-path` (control repo docs path for this feature) <br> • `lifecycle.yaml` (artifact list per phase, required_sections, min_word_count, must_reference) <br> • Actual artifact files at the specified path |
| **Process** | 1. Load artifact list for the phase from `lifecycle.yaml` <br> 2. For each required artifact: check file existence <br> 3. Check `required_sections` (heading presence) <br> 4. Check `min_word_count` (word count of the document) <br> 5. Check `must_reference` (does this doc contain a reference to the upstream artifact?) <br> 6. Apply track-specific relaxation (express track: lower word counts) |
| **Output** | • Validation report: pass/fail per artifact per check <br> • Overall pass/fail exit code <br> • No file writes |
| **Hard Gates** | Missing required artifact → fail. Missing required section → fail. Word count below minimum → fail |
| **Calls** | Reads `lifecycle.yaml`; reads artifact files |

---

### `migrate-ops.py`

| | |
|--|--|
| **Input** | • `--subcommand {scan|dry-run|migrate|verify|cleanup}` <br> • Legacy v3 branches (pattern: `{domain}-{service}-{feature}[-{milestone}]`) <br> • Legacy docs in `docs/{domain}/{service}/{legacyFeature}/` <br> • `feature-index.yaml` (duplicate detection) <br> • `--governance-repo` path |
| **Process** | **`scan`:** Identifies all legacy v3 branches + their docs <br> **`dry-run`:** Plans migration steps without writing; reports what would change <br> **`migrate`:** <br>   1. Validate: no existing `feature.yaml` at target path (conflict detection) <br>   2. Create new `{featureId}` + `{featureId}-plan` branches <br>   3. Mirror legacy docs to governance `docs/lens-work/migrations/...` (durable proof) <br>   4. Create `feature.yaml` for migrated feature <br>   5. Add to `feature-index.yaml` <br> **`verify`:** Checks migration output against expected state <br> **`cleanup`:** Writes `cleanup-approval.yaml` → user approves → writes `cleanup-receipt.yaml` → deletes legacy branches |
| **Output** | • New 2-branch topology in control repo <br> • `feature.yaml` + `feature-index.yaml` entries <br> • Governance migration docs mirror <br> • `cleanup-approval.yaml` + `cleanup-receipt.yaml` (durable proof for cleanup) |
| **Hard Gates** | Dry-run mandatory before `migrate`. Existing `feature.yaml` at target → conflict abort. Cleanup requires `cleanup-approval.yaml` present |
| **Calls** | git subprocess; `feature-yaml-ops.py` patterns; `git-orchestration-ops.py` patterns |

---

### `dashboard-ops.py`

| | |
|--|--|
| **Input** | • `feature-index.yaml` (all features) <br> • `governance/features/{domain}/{service}/{featureId}/` deep content (feature.yaml, docs) <br> • `--governance-repo` path <br> • Staleness threshold: 14 days since `lastUpdated` |
| **Process** | **`collect`:** <br>   1. Load feature-index.yaml <br>   2. For each feature: load feature.yaml + summary.md + problems.md <br>   3. Detect stale features (> 14 days without update) <br>   4. Graceful degradation: missing content reported as "unavailable" <br> **`generate`:** <br>   1. Build self-contained HTML (no CDN, no external assets) <br>   2. Embed all data inline (CSS + JS + data in a single file) <br> **`dependency-data`:** Output cross-feature dependency graph as JSON |
| **Output** | • Single self-contained HTML file (all assets inline) <br> • JSON dependency data (for `dependency-data` subcommand) <br> • No modifications to source data |
| **Hard Gates** | None (graceful degradation on missing content) |
| **Calls** | Reads `feature-index.yaml` + `feature.yaml` files; file I/O only |

---

### `complete-ops.py`

| | |
|--|--|
| **Input** | • `feature.yaml` (phase must be `dev` or `complete`) <br> • `governance/features/.../retrospective.md` (must exist or user confirms skip) <br> • `feature-index.yaml` (for atomic archive update) |
| **Process** | **`check-preconditions`:** <br>   1. Verify feature exists in governance <br>   2. Verify phase is `dev` or `complete` (not planning, not paused) <br>   3. Verify `retrospective.md` exists (or user confirms skip) <br> **`finalize`:** <br>   1. Delegate to `bmad-lens-document-project` (final docs generation) <br>   2. Update `feature.yaml` phase = `complete` <br>   3. Update `feature-index.yaml` status = `archived` <br>   4. Write `final-summary.md` <br>   5. ATOMIC: all three updates committed together as one governance commit <br>   6. Irreversible — user must confirm before execution |
| **Output** | • `feature.yaml` phase = `complete` <br> • `feature-index.yaml` status = `archived` <br> • `final-summary.md` in governance feature folder <br> • Governance commit containing all three changes atomically |
| **Hard Gates** | Phase not `dev` or `complete` → abort. Missing retrospective (without skip confirmation) → abort. User must confirm — no silent execution |
| **Calls** | `bmad-lens-document-project` (delegated); `feature-yaml-ops.py`; git subprocess |

---

## 9. IPO Catalog — Feature Lifecycle Management

### `bmad-lens-pause-resume`

| | |
|--|--|
| **Input** | • `feature.yaml` (current phase + track) <br> • `lifecycle.yaml` (TRACK_TRANSITIONS) <br> • `--action {pause|resume}` flag |
| **Process** | **`pause`:** <br>   1. Validate any active phase is pausable (not in `close_states`) <br>   2. Update feature.yaml phase = `paused`, store `paused_from_phase` <br> **`resume`:** <br>   1. Validate `paused` state exists <br>   2. Read `paused_from_phase` from feature.yaml <br>   3. Restore phase to `paused_from_phase` per TRACK_TRANSITIONS |
| **Output** | • `feature.yaml` updated (phase = `paused` or restored) |
| **Hard Gates** | Resume without `paused` state → abort. Pause from `close_state` → abort |
| **Calls** | `pause-resume-ops.py`; `feature-yaml-ops.py` |

---

### `bmad-lens-complete`

(See [complete-ops.py IPO](#complete-opspy) above for the script-level detail.)

| | |
|--|--|
| **Input** | • User command `/complete` <br> • `feature.yaml`, `feature-index.yaml`, `retrospective.md` |
| **Process** | Runs precondition check → triggers retrospective if missing → delegates final documentation → atomic archive |
| **Output** | • Feature archived in governance. Docs finalized. |
| **Calls** | `complete-ops.py`, `bmad-lens-retrospective`, `bmad-lens-document-project` |

---

### `bmad-lens-log-problem`

| | |
|--|--|
| **Input** | • Problem description from user <br> • `feature.yaml` (featureId, phase for context) <br> • `assets/templates/problems-template.md` <br> • `--export-to-github` flag (optional) |
| **Process** | 1. Load existing `problems.md` from governance or create from template <br> 2. Append structured problem entry with timestamp + phase context <br> 3. Update `feature.yaml` open_problems count <br> 4. If `--export-to-github`: create GitHub issue via `create-pr.py` patterns (REST API) |
| **Output** | • `governance/features/.../problems.md` updated <br> • `feature.yaml` open_problems counter incremented <br> • (Optional) GitHub issue created + URL returned |
| **Hard Gates** | None |
| **Calls** | `log-problem-ops.py`; `feature-yaml-ops.py`; optionally GitHub REST API |

---

### `bmad-lens-target-repo`

| | |
|--|--|
| **Input** | • `--repo-url`, `--domain`, `--service`, `--repo-name` flags <br> • `feature.yaml` (target_repos list) <br> • `repo-inventory.yaml` (workspace-wide repo registry) <br> • `gh` CLI (for GitHub auto-creation) |
| **Process** | 1. Derive canonical clone path: `TargetProjects/{domain}/{service}/{repo}` <br> 2. If repo doesn't exist on remote + GitHub host: `gh repo create` <br> 3. Clone to canonical path <br> 4. Update `repo-inventory.yaml` (append entry if not present) <br> 5. Update `feature.yaml` `target_repos` list (append if not present) <br> 6. Idempotent: rerun verifies and reconciles |
| **Output** | • Repo cloned at `TargetProjects/{domain}/{service}/{repo-name}/` <br> • `repo-inventory.yaml` updated <br> • `feature.yaml` `target_repos` updated |
| **Hard Gates** | Non-GitHub host + repo missing → no auto-create; provide manual guidance |
| **Calls** | `target-repo-ops.py`; git subprocess; `gh` subprocess; `feature-yaml-ops.py` |

---

## 10. IPO Catalog — End-to-End User Journeys

### Journey A: `full` Track — New Feature from Scratch

```
INPUT: User idea (domain, service, feature name)
  │
  ▼
/init-feature
  INPUT:  user inputs (name, domain, service)
  PROCESS: derive featureId, validate, create branches + governance artifacts
  OUTPUT: feature.yaml + 2 branches + feature-index.yaml entry
  │
  ▼
/preplan
  INPUT:  feature.yaml, lifecycle.yaml, product-brief template, constitution
  PROCESS: mary (Analyst) brainstorms + researches → adversarial review
  OUTPUT: product-brief.md, research.md → feature.yaml phase=preplan-complete
  │
  ▼
/businessplan
  INPUT:  feature.yaml, product-brief.md, prd + ux templates, constitution
  PROCESS: john (PM) → prd; sally (UX) → ux-design; adversarial review
  OUTPUT: prd.md, ux-design.md → feature.yaml phase=businessplan-complete
  │
  ▼
/techplan
  INPUT:  feature.yaml, prd.md, ux-design.md, architecture template, constitution
  PROCESS: winston (Architect) → architecture; adversarial review
  OUTPUT: architecture.md → feature.yaml phase=techplan-complete
  │
  ▼
/finalizeplan
  INPUT:  all prior artifacts, epics/stories/impl-readiness templates, git-orchestration
  PROCESS: 3-step: adversarial review → commit + push plan branch → PR creation + epics/stories bundle
  OUTPUT: epics.md, stories.md, story files, 2 PRs → feature.yaml milestone=dev-ready
  │
  ▼
/dev
  INPUT:  feature.yaml (dev-ready), constitution, target repo inventory, dev-branch mode
  PROCESS: per-epic story implementation loop via delegated runSubagent; per-task commits; final PR
  OUTPUT: code in target repo, dev-session.yaml checkpoints, final PR to target default branch
  │
  ▼
/complete
  INPUT:  feature.yaml (phase=dev), retrospective.md, feature-index.yaml
  PROCESS: retrospective analysis → final docs → atomic archive commit
  OUTPUT: feature archived, final-summary.md, feature-index.yaml status=archived
```

### Journey B: `hotfix` Track — Urgent Fix

```
INPUT: Urgent bug + target repo details
  │
  ▼
/init-feature (track=hotfix)
  OUTPUT: feature.yaml (track=hotfix) + 2 branches
  │
  ▼
/techplan (skips preplan + businessplan)
  INPUT:  feature.yaml, architecture template, issue description
  OUTPUT: hotfix-architecture.md (minimal)
  │
  ▼
/finalizeplan (abbreviated)
  OUTPUT: minimal story file + plan PR
  │
  ▼
/dev
  OUTPUT: hotfix commit, PR to target repo
  │
  ▼
/complete
  OUTPUT: archived
```

### Journey C: `expressplan` Track — Rapid Planning

```
INPUT: Feature idea + feature.yaml (track=express)
  │
  ▼
/expressplan (3-step conductor)
  Step 1: quickplan-ops.py → all planning artifacts in one pass
  Step 2: adversarial review (party mode) — fail halts
  Step 3: advance → finalizeplan
  │
  ▼
/finalizeplan → /dev → /complete
```

---

_Generated by lens-bmad-document-project for feature lens-dev-release-discovery._
