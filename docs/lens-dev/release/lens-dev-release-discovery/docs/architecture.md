# lens.core.release — Architecture

**Feature:** lens-dev-release-discovery
**Generated:** 2026-05-20

---

## Architectural Overview

`lens.core.release` is a **distribution-bundled, modular AI agent toolkit**. It is not a traditional software application — there is no runtime server, compiled binary, or database. Instead, it is a **collection of AI-readable prompt files, skill definitions, lifecycle contracts, and Python utility scripts** that instruct AI coding agents (Claude, GitHub Copilot, Cursor, Codex) how to orchestrate a disciplined feature-delivery workflow.

The architecture has three layers:

```
┌─────────────────────────────────────────────────────┐
│                 IDE Adapter Layer                    │
│   .github/  .claude/  .codex/  .cursor/  AGENTS.md │
│   (routes AI tool to prompts and skills)             │
└──────────────────────┬──────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────┐
│              Lens Module Layer                       │
│   _bmad/lens-work/                                  │
│   agents/ prompts/ skills/ scripts/ lifecycle.yaml  │
│   (the LENS lifecycle runtime)                      │
└──────────────────────┬──────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────┐
│            Supporting BMAD Modules Layer             │
│   _bmad/bmb/  _bmad/bmm/  _bmad/core/  etc.        │
│   (methodology and tooling skills invoked by Lens)  │
└─────────────────────────────────────────────────────┘
```

## Layer 1 — IDE Adapter Layer

Users interact with LENS through their AI coding tool of choice. The adapter layer makes every IDE a valid entry point.

### `.github/` — GitHub Copilot Adapter
```
.github/
├── agents/
│   └── bmad-agent-lens-work-lens.agent.md  # Copilot agent definition
├── instructions/
│   └── lens-control-repo.instructions.md   # Always-on context rules
├── prompts/                                 # 57 prompt stubs (one per command)
├── skills/                                  # Symlinked BMAD skill stubs
└── lens-work-instructions.md               # Module-level Copilot rules
```
Each `.github/prompts/lens-*.prompt.md` file is a thin stub that runs `light-preflight.py` then loads the real prompt from `_bmad/lens-work/prompts/`.

### `.claude/` — Claude Adapter
Native Claude project reference that points to the lens agent and skills.

### `.codex/` — Codex Adapter
Activated via `AGENTS.md` at the repo root. Codex reads `AGENTS.md` first, which directs it to load `lens.core/_bmad/lens-work/agents/lens.agent.md`.

### `.cursor/` — Cursor Adapter
Cursor rules pointing to the Lens agent and lifecycle contract.

## Layer 2 — Lens Module Layer (`_bmad/lens-work/`)

The core of the distribution. This is LENS Workbench v4.0.0.

### Module Identity Files

| File | Role |
| --- | --- |
| `module.yaml` | Canonical registry of all skills, prompts, installers, adapters |
| `lifecycle.yaml` | Lifecycle contract v4 — phases, milestones, gate semantics, axioms |
| `module-help.csv` | Command discovery and help registry |
| `bmadconfig.yaml` | Default configuration values for new Lens installs |

### Agent Shell (`agents/`)
```
agents/
├── lens.agent.md    # Thin-shell agent; routes commands to skills
└── lens.agent.yaml  # YAML representation of agent definition
```

The Lens agent is deliberately **thin** — it provides a compact command menu and delegates all real work to typed skills via `read_file` loads. This prevents the agent from accumulating stale logic.

### Prompt Entry Points (`prompts/`)
57 prompt files — one per user-facing command. Pattern:
```
lens-{command}.prompt.md
```
Each prompt follows the **stub + delegate** pattern:
1. Check/run `light-preflight.py`
2. Load the corresponding skill from `skills/bmad-lens-{command}/SKILL.md`
3. Follow the skill's instructions

### Skills (`skills/`)
41 skills organized by lifecycle function. Each skill directory contains:
- `SKILL.md` — Primary skill instructions (loaded by prompts)
- `scripts/` — Focused Python scripts for deterministic operations
- `assets/` — Templates, schemas, data files
- `tests/` — Skill-level tests (where present)

**Skill surface by category:**

| Category | Skills |
| --- | --- |
| Planning conductors | preplan, businessplan, techplan, expressplan, quickplan, finalizeplan, adversarial-review, devproposal |
| Execution | dev, complete |
| Lifecycle utilities | init-feature, target-repo, next, batch, switch, pause-resume, retrospective, move-feature, split-feature, lessons |
| Governance & reporting | constitution, sensing, audit, dashboard, approval-status, rollback, profile, theme, log-problem |
| Setup & migration | migrate, upgrade, document-project, module-management, onboard |
| Foundation | git-state, git-orchestration, feature-yaml, discover, help, bmad-skill |

### Scripts (`scripts/`)
Python and shell utilities for deterministic, repeatable operations:

| Script | Role |
| --- | --- |
| `install.py` | Copy module files into a control repo |
| `setup-control-repo.py` | Bootstrap new control repos (clone, config, governance) |
| `preflight.py` | Full preflight validation (lifecycle.yaml, feature state) |
| `light-preflight.py` | Fast preflight check (used by prompts) |
| `run-preflight-cached.py` | Preflight with caching to avoid redundant checks |
| `create-pr.py` | GitHub REST API PR creation (no `gh` CLI required) |
| `store-github-pat.py` | PAT setup via environment variables |
| `validate-lens-bmad-registry.py` | Validate skill/prompt registry consistency |
| `validate-lens-core-parity.sh` | Shell parity check between source and release |
| `validate-phase-artifacts.py` | Validate that phase transition artifacts are present |
| `validate-feature-move.py` | Validate feature move/rename operations |
| `derive-next-action.py` | Compute next recommended action from git state |
| `derive-initiative-status.py` | Compute initiative status from git + artifacts |
| `scan-active-initiatives.py` | Cross-repo active initiative scanning |
| `load-command-registry.py` | Load module-help.csv into structured form |
| `bootstrap-target-projects.py` | Scaffold TargetProjects/ for new repos |
| `plan-lifecycle-renames.py` | Compute rename plan for lifecycle schema migrations |

### Tests (`scripts/tests/` and `tests/`)

Two test tiers:

**Script tests** (`scripts/tests/` — 10 Python + README):
- `test-install.py` — Adapter generation and install validation
- `test-preflight.py` / `test-light-preflight.py` — Preflight behavior
- `test-setup-control-repo.py` — Control repo bootstrap
- `test-create-pr.py` — PR creation script
- `test-store-github-pat.py` — PAT storage
- `test-validate-phase-artifacts.py` — Phase artifact validation
- `test-adversarial-review-contracts.py` — Adversarial review contracts
- `test-batch-mode-contracts.py` — Batch mode contracts
- `test-phase-conductor-contracts.py` — Phase conductor contracts

**Contract tests** (`tests/contracts/` — 8 markdown files):
Declarative behavioral contracts verifying lifecycle invariants, branch topology, governance rules, sensing behavior.

### Module Installer (`_module-installer/`)
CI/CD adapter generator — produces the IDE adapter files (.github/, .claude/, .codex/, .cursor/) from templates. This is how the pre-generated adapters in the release artifact are built.

### Assets (`assets/`)
- `lens-bmad-skill-registry.json` — Machine-readable registry of all skills with metadata
- `templates/` — Reusable templates for skills, agents, prompts

## Layer 3 — Supporting BMAD Modules (`_bmad/`)

LENS depends on the BMAD framework. The release bundles all required modules alongside lens-work:

| Module | Skills | Purpose |
| --- | --- | --- |
| `core/` | ~10 | Foundation skills: brainstorming, elicitation, editorial review |
| `bmm/` | ~50+ | Product methodologies: PRD, UX design, epics, stories |
| `bmb/` | 2 | Builder skills: agent builder, workflow builder |
| `cis/` | ~15 | Creative innovation: problem solving, design thinking |
| `tea/` | ~10 | Test engineering: test design, automation, CI |
| `gds/` | ~20 | Game design: GDD, game architecture, narrative |
| `wds/` | ~10 | Web design: trigger mapping, UX scenarios |

## Architectural Axioms (from lifecycle.yaml)

| Axiom | Rule |
| --- | --- |
| A1 | Git is the only source of truth for shared workflow state |
| A2 | PRs are the only gating mechanism |
| A3 | Authority domains must be explicit |
| A4 | Sensing must be automatic at lifecycle gates |
| A5 | The control repo is an operational workspace, not a code repo |

## Data Flow — Lifecycle Execution

```
User types: /businessplan
         │
         ▼
IDE adapter stub (.github/prompts/lens-businessplan.prompt.md)
         │  runs light-preflight.py
         │  loads SKILL.md
         ▼
bmad-lens-businessplan/SKILL.md
         │  reads feature.yaml for current feature context
         │  checks lifecycle.yaml for phase gate rules
         │  reads constitution (org/domain/service/repo hierarchy)
         │  calls bmm skills as needed (bmad-create-prd, etc.)
         ▼
Feature artifacts committed to feature branch
         │
         ▼
PR opened to plan branch → reviewed → merged
         │
         ▼
Phase advances (git-derived: PR merged = phase complete)
```

## Distribution Architecture

The release is a **git-cloned module** — users clone it into their control repo:
```bash
git clone --branch beta https://github.com/crisweber2600/lens.core.git
```

After cloning, `install.py` copies the IDE adapters to `.github/`, `.claude/`, `.codex/`, `.cursor/` in the user's control repo and updates `bmadconfig.yaml` with the user's config.

There is no npm, pip, or brew install — the module is purely file-based.

---

_Generated by lens-bmad-document-project for feature lens-dev-release-discovery._
