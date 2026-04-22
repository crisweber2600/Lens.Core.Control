# lens.core.release — Deep Dive: LENS Workbench Module

**Feature:** lens-dev-release-discovery
**Generated:** 2026-05-20

---

## Overview

LENS Workbench (v4.0.0, schema 4) is a structured AI-agent lifecycle governance module for software feature delivery. It sits on top of BMAD (AI methodology framework) and provides opinionated, git-anchored lifecycle management for features from ideation through code delivery.

The core insight: **git state is the only source of truth**. Branch existence, PR metadata, and committed artifacts replace all legacy file-based state. There is no external database, no CI state store, no side-channel.

---

## Lifecycle Contract v4 (lifecycle.yaml)

### Design Philosophy

The lifecycle contract encodes five design axioms:

| Axiom | Statement |
| --- | --- |
| A1 | Git is the only source of truth for shared workflow state. No git-ignored runtime state. |
| A2 | PRs are the only gating mechanism. No side-channel approval. |
| A3 | Authority domains must be explicit. Every file belongs to exactly one authority. |
| A4 | Sensing must be automatic at lifecycle gates, not manual discovery. |
| A5 | The control repo is an operational workspace, not a code repo. |

And three fundamental truths:

| Truth | Statement |
| --- | --- |
| FT1 | Planning artifacts must exist and be reviewed before code is written. |
| FT2 | AI agents must work within disciplined constraints, not freestyle. |
| FT3 | Multi-service initiatives must have coordinated lifecycle governance. |

### Phases

The canonical phase sequence is:

```
preplan → businessplan → techplan → [adversarial-review gate] → finalizeplan → dev → complete
```

Additionally:
- `expressplan` — standalone single-session track (business + tech + finalize in one pass)
- `quickplan` — conductor pipeline across all phases

| Phase | Agent | Artifacts | Auto-advance to |
| --- | --- | --- | --- |
| preplan | mary (Analyst) | product-brief, research, brainstorm | /businessplan |
| businessplan | john (PM) + sally | prd, ux-design | /techplan |
| techplan | winston (Architect) | architecture | /finalizeplan |
| finalizeplan | lens (Conductor) | review-report, epics, stories, sprint-status, story-files | /dev |
| expressplan | lens | business-plan, tech-plan, sprint-plan, review | /finalizeplan |

Each phase has an adversarial review gate (party mode) with outcomes: `pass`, `pass-with-warnings`, `fail`.

### Milestones

| Milestone | Role | Phases |
| --- | --- | --- |
| techplan | IC creation work | preplan, businessplan, techplan |
| finalizeplan | Planning consolidation | finalizeplan |
| dev-ready | Ready for execution | (constitution gate) |
| dev-complete | Execution complete | (optional, dev-complete-validation) |

### Tracks

| Track | Description | Start Phase |
| --- | --- | --- |
| full | All phases, all milestones | preplan |
| feature | Known business context, skip research | businessplan |
| tech-change | Pure technical change | techplan |
| hotfix | Urgent fix, minimal planning | techplan |
| hotfix-express | Emergency fix | techplan |

All tracks use the **2-branch topology**: `{featureId}-plan` (planning) + `{featureId}` (dev/code).

### Constitution System

Four-level additive constitution hierarchy:

```
org-level constitution
  └── domain-level constitution
        └── service-level constitution
              └── repo-level constitution
                    └── language-specific constitution
```

Language-specific constitutions apply only to repos matching the declared language. Supported languages: typescript, python, go, java, csharp, rust, php, kotlin, swift, cpp.

Language detection order: explicit config → `.bmad/language` file → build files → source analysis → GitHub primary language → "unknown".

---

## Two-Branch Topology

Every feature has exactly two git branches:

```
governance-repo/
├── {featureId}-plan    ← All planning artifacts
│   ├── businessplan.md
│   ├── techplan.md
│   ├── architecture.md
│   └── ...
└── {featureId}         ← Planning → main PR target
```

Target repos create short `feature/{featureSlug}` working branches independent of the governance branch naming. This keeps target-repo branches short even when governance uses canonical IDs.

**Feature ID format:** `{normalized-domain}-{normalized-service}-{featureSlug}`

**Feature YAML** (`feature.yaml`) tracks feature identity and phase state. It is the optional state source — git state is still authoritative.

---

## Skills In-Depth (41 skills)

### Planning Conductors

#### `bmad-lens-preplan` (PL: `preplan`)
Runs PrePlan phase. Agent: mary (Analyst). Outputs: product-brief.md, research.md, brainstorm.md.
Completion gate: adversarial review (party mode) on all three artifacts.
Auto-advances to businessplan.

#### `bmad-lens-businessplan` (LB: `businessplan`)
Runs BusinessPlan phase. Agent: john (PM) + sally. Outputs: prd.md, ux-design.md.
Publishes preplan context before handing off to BMAD PRD or UX sessions.
Completion gate: adversarial review on prd + ux-design.

#### `bmad-lens-techplan` (LT: `techplan`)
Runs TechPlan phase. Agent: winston (Architect). Output: architecture.md.
Publishes businessplan context before handing off to native architecture session.
Completion gate: adversarial review on architecture.

#### `bmad-lens-adversarial-review` (RV: `adversarial-review`)
Runs adversarial review with mandatory party-mode blind-spot challenge round.
Can target current phase or specific prior phases.
Outcomes: pass, pass-with-warnings, fail (fail halts progression).

#### `bmad-lens-finalizeplan` (FZ: `finalizeplan`)
Planning consolidation conductor. Three-step execution contract:
1. Final adversarial review + party challenge → commit and push `{featureId}-plan`
2. Create/verify `{featureId}-plan → {featureId}` PR, confirm auto-merge readiness
3. After PR lands: generate epics, stories, implementation-readiness, sprint-status, story files → open final `{featureId} → main` PR

#### `bmad-lens-expressplan` (LE: `expressplan`)
Express planning track — full planning in one session. Three-step:
1. Invoke quickplan pipeline (business → tech → sprint)
2. Adversarial review with party mode — fail verdict halts
3. Advance to finalizeplan

#### `bmad-lens-quickplan` (PL: `plan`)
Conductor for full planning pipeline with adversarial review in sequence.

#### `bmad-lens-devproposal` (`devproposal`)
Generates a dev proposal from a finalized plan.

---

### Execution Skills

#### `bmad-lens-dev` (DV: `dev`)
Launches Dev phase — epic-level implementation loop with story management.
This is a **delegation** command — it hands off to implementation agents in target repos.
It is NOT a lifecycle phase (not in phase_order). Story management and epic tracking happen in target repos.

#### `bmad-lens-complete` (FF: `finalize-feature`)
Feature completion gating. Subcommands:
- `check-preconditions` (CK) — wrapped project doc gate before archival
- `finalize` (FF) — confirm gate and archive feature
- `archive-status` (AR) — check archive status

---

### Lifecycle Utilities

#### `bmad-lens-init-feature` (IF: `init-feature`)
Initializes a new feature: creates 2-branch topology, feature.yaml, governance entries, featureId, featureSlug.
Creates domain/service marker files if missing (does NOT create constitutions — use create-domain/create-service for those).
Subcommand `pull-context` (PC) fetches cross-feature context (summaries, deps, constitution).

#### `bmad-lens-target-repo` (TR: `target-repo`)
Provisions or registers a feature target repo. Persists to repo-inventory.yaml and feature metadata.
Supports `--create-remote` for GitHub repo creation.

#### `bmad-lens-next` (NA: `next-action`)
Context-aware next-action routing. Reads git state, feature.yaml, PR status → resolves the single unblocked next command → delegates into it automatically.
Dependencies: `derive-next-action.py` script for computation.

#### `bmad-lens-batch` (BT: `batch`)
Two-pass batch intake for multi-feature operations. Supports targets: current, preplan, businessplan, techplan, finalizeplan, expressplan, quickplan.

#### `bmad-lens-switch` (FE: `switch-feature` / LF: `list-features`)
Switches active feature context. Loads paths and related deps.
`list` subcommand shows all features with status and staleness.

#### `bmad-lens-pause-resume` (PA/RS/PT)
Pause: preserves feature state with reason.
Resume: restores context from paused state.
Status: checks pause state and reason.

#### `bmad-lens-retrospective` (AP/GR/UI)
Analyze: scans problem log patterns and frequencies.
Generate-report: narrative retrospective.
Update-insights: appends lessons to governance insights store.

#### `bmad-lens-move-feature` (MF/LR/PH)
Relocates feature to new domain/service with full reference patching.
list-references: finds all cross-references before move.
patch-references: updates references after move.

#### `bmad-lens-split-feature` (VS/SL/MS)
Creates a new feature from a subset of stories.
validate-split: blocks on in-progress stories.
create-split-feature: creates new feature.yaml and index entry.
move-stories: relocates story files.

#### `bmad-lens-lessons` (`lessons`)
Captures and surfaces lessons learned.

#### `bmad-lens-discover` (DR: `discover`)
Syncs TargetProjects with governance repo inventory. The entry point for repo inventory discovery.

---

### Governance and Reporting

#### `bmad-lens-constitution` (CO: `load-constitution`)
Resolves the full constitution stack (org → domain → service → repo → language).
Returns constitution YAML with merged rules.

#### `bmad-lens-sensing` (SN: `sensing`)
Cross-initiative overlap detection. Scans all active features for:
- Conflicting file modifications
- Shared dependency versions
- Timeline overlaps
Automatic at lifecycle gates (Axiom A4).

#### `bmad-lens-audit` (AA: `audit`)
Cross-initiative compliance audit dashboard.
Output: `docs/reports/lens-work/quality-scan/`.

#### `bmad-lens-dashboard` (CD/DG/GD)
collect-data: pulls feature-index.yaml cross-feature data.
dependency-graph: builds dependency graph.
generate-dashboard: produces self-contained HTML dashboard.

#### `bmad-lens-approval-status` (AS: `approval-status`)
Shows pending promotion PR approval state and review status.

#### `bmad-lens-rollback` (RB: `rollback`)
Safely rolls back to a previous lifecycle phase.
Supports `--target-phase` and `--dry-run`.

#### `bmad-lens-profile` (PF: `profile`)
View and edit onboarding profile stored in `.lens/personal/profile.yaml`.

#### `bmad-lens-theme` (TH: `apply-theme`)
Applies visual theme to Lens output.

#### `bmad-lens-log-problem` (LG/RP/LL)
log: captures a problem or blocker in the feature problem log.
resolve: marks a problem as resolved.
list: lists problems by status filter.

---

### Setup and Migration

#### `bmad-lens-migrate` (SC/MI/CC)
Schema migration with dry-run/apply/verify pattern.
scan-legacy: finds v3 branches and builds migration plan.
migrate-feature: migrates single feature to 2-branch topology.
check-conflicts: detects naming conflicts before migration.

#### `bmad-lens-upgrade` (UG: `upgrade`)
Migrates control repo schema to current version. Routes legacy branches to lens-migrate.
Declared in lifecycle.yaml under schema migrations.

#### `bmad-lens-document-project` (DC: `document-project`)
Runs BMAD document-project with Lens feature docs scope and governance mirroring.
Outputs: project-overview.md, index.md, and full docs suite.
This session is an instance of this skill running exhaustive mode.

#### `bmad-lens-module-management` (MM: `module-management`)
Checks module version, clarifies legacy branch migration status.

#### `bmad-lens-onboard` (PK/NB/WC/PO)
Deprecated onboard bridge retained for compatibility.
Provides: preflight check, next-step guidance, config writing.

---

### Foundation / Low-Level

#### `bmad-lens-git-state` (GS: `read-git-state`)
Read-only git queries for 2-branch feature model.
Returns: git state JSON (branch existence, PR status, artifact presence).

#### `bmad-lens-git-orchestration` (GO: `orchestrate-git`)
Executes multi-step git workflows atomically.
Subcommand: `create-feature-branches` for 2-branch topology initialization.
All write operations are here; git-state is read-only.

#### `bmad-lens-feature-yaml` (FY: `manage-feature-yaml`)
Reads and writes feature.yaml files atomically.
Subcommands: read, write with `--dry-run` support.

#### `bmad-lens-help` (HE/SH/AH)
contextual-help: phase-filtered help for current lifecycle state.
search-help: full-text search across help topics.
all-help: complete help table grouped by category.

#### `bmad-lens-bmad-skill` (BBS/BPF/BDR/BMR/BTR/BPR/BUX/BAR/BES/BIR/BSP/BST/BQD/BCR)
Wraps BMAD methodology skills with Lens governance context.
Each invocation writes output to `feature.yaml.docs.path`, keeping all artifacts in the feature's governance docs path.
BMAD skills covered: brainstorming, product-brief, domain-research, market-research, technical-research, create-prd, create-ux-design, create-architecture, create-epics-and-stories, check-implementation-readiness, sprint-planning, create-story, quick-dev, code-review.

#### `bmad-lens-sprintplan` (`sprintplan`)
Sprint planning conductor.

---

## Prompts System (57 prompts)

### Stub Pattern
Every `lens-*.prompt.md` follows the same two-step pattern:
1. Run `light-preflight.py` — validates environment and active feature context
2. Load SKILL.md and follow instructions

This ensures every command entry point validates state before loading skill instructions. Users can never accidentally run a phase skill without preflight context.

### Prompt Categories

| Category | Count | Examples |
| --- | --- | --- |
| Native Lens commands | ~41 | lens-preplan, lens-businessplan, lens-next |
| Convenience aliases | ~4 | lens-new-feature, lens-new-domain, lens-new-project, lens-new-service |
| BMAD bridge | ~13 | lens-bmad-create-prd, lens-bmad-code-review |

---

## Scripts System (19 scripts)

### Script Architecture

All scripts use `uv run` with inline PEP 723 dependency declarations:
```python
# /// script
# dependencies = ["pyyaml>=6.0"]
# ///
```

No global environment, no pip install, no virtualenv required. Each script runs in a clean isolated environment.

### Key Script Interactions

```
user invokes /lens-*
  → prompt stub runs light-preflight.py
    → loads SKILL.md
      → SKILL.md may call derive-next-action.py
        → reads git-state via git commands
          → returns next-action recommendation
```

```
/lens-init-feature
  → init-feature-ops.py
    → creates feature.yaml
    → git-orchestration-ops.py creates 2 branches
    → feature-index.yaml updated
```

```
/lens-discover
  → syncs TargetProjects/ entries with repo-inventory.yaml
    → validates local_path for each repo
      → reports sync status
```

```
/lens-migrate (scan)
  → migrate-ops.py --mode dry-run
    → finds legacy v3 branches
      → returns migration plan JSON
```

### Test Targets (verified from prior sessions)

```bash
# Setup and bootstrap tests (from TargetProjects/lens.core/src/Lens.Core.Src):
uv run --with pytest --with pyyaml pytest _bmad/lens-work/scripts/tests/test-setup-control-repo.py -q

# Install + next-action (adapter generation + routing):
uv run --with pytest --with pyyaml pytest _bmad/lens-work/scripts/tests/test-install.py _bmad/lens-work/skills/bmad-lens-next/scripts/tests/test-next-ops.py -q

# Git orchestration:
uv run --with pytest --with pyyaml pytest _bmad/lens-work/skills/bmad-lens-git-orchestration/scripts/tests/test-git-orchestration-ops.py -q

# Full test suite:
uv run --with pytest pytest _bmad/lens-work/scripts/tests -q
```

---

## IDE Adapter Generation

The `_module-installer/` directory contains the adapter generator that produces all IDE-specific adapter stubs. It reads `module.yaml` (skill and prompt registry) and emits:

| Adapter | Path | Format |
| --- | --- | --- |
| GitHub Copilot | `.github/prompts/` | `.prompt.md` stubs |
| GitHub Copilot agent | `.github/agents/` | `.agent.md` |
| GitHub Copilot instructions | `.github/instructions/` | `.instructions.md` |
| Claude | `.claude/` | Command entries |
| Codex | `.codex/` | `AGENTS.md` entries |
| Cursor | `.cursor/` | Cursor rules |

Adapters are idempotent — regenerating overwrites but does not corrupt. After a module upgrade: run `install.py` to refresh all adapters.

---

## Comparison: Release vs. Source

| Dimension | lens.core.release | lens.core.src |
| --- | --- | --- |
| Skill count | 41 | 41 |
| Prompt count | 57 | 57 |
| Script count | 19 | 19 |
| Test count | 10 + README | 10 + README |
| Contract tests | 8 | 8 |
| Bundled BMAD modules | Yes (bmb, bmm, cis, core, gds, tea, wds) | No (separate module repos) |
| Pre-generated IDE adapters | Yes | No (generated at install) |
| Authoring scaffolding | No | Yes |
| Read-only | Yes | No |
| Distribution method | git clone beta branch | source control, dev branch |

The release artifact is larger (6,761 total files) due to bundled BMAD modules. The source artifact is the same lens-work content but without bundled modules and with authoring-time scaffolding for new skills and prompts.

---

## Governance Hierarchy

```
Lens.Core.Governance/
├── constitutions/
│   ├── org.constitution.md         ← level 1: org-wide rules
│   └── ...
└── features/
    └── {domain}/
        └── {service}/
            ├── domain.constitution.md   ← level 2: domain rules
            ├── service.constitution.md  ← level 3: service rules
            └── {feature}/
                ├── feature.yaml          ← feature identity + phase state
                ├── businessplan.md
                ├── techplan.md
                ├── architecture.md
                └── ...
```

The `bmad-lens-constitution` skill resolves this 4-level (+ language) stack on demand, producing merged governance rules that constrain planning artifacts and phase gates.

---

## Sensing System

The `bmad-lens-sensing` skill implements cross-initiative overlap detection:

1. Scans all active features via `scan-active-initiatives.py`
2. Computes overlap risk: shared files, shared dependencies, timeline conflicts
3. Surfaces a sensing report at lifecycle gates
4. Blocked by Axiom A4 — sensing is automatic, not manual

Sensing is invoked as part of the finalizeplan execution contract (step 1 includes governance cross-check).

---

_Generated by lens-bmad-document-project for feature lens-dev-release-discovery._
