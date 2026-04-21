# lens.core.src — Source Tree Analysis

**Date:** 2026-04-21
**Feature:** lens-dev-old-codebase-discovery

## Overview

`lens.core.src` has a compact parent tree and a dense module subtree. Almost all product logic lives under `_bmad/lens-work/`, making it the primary entry point for contributors.

## Annotated Directory Structure

```text
lens.core.src/
├── _bmad/
│   ├── config.yaml                           # Lens config bridge
│   └── lens-work/                            # Authoritative module source (v4.0.0)
│       ├── agents/
│       │   ├── lens.agent.md                 # Primary agent implementation
│       │   └── lens.agent.yaml               # Agent manifest
│       ├── assets/
│       │   ├── lens-bmad-skill-registry.json # Published prompt-to-skill mapping
│       │   └── templates/                    # 11 authored planning/doc templates
│       ├── bmad-lens-work-setup/             # Setup skill for new workspaces
│       ├── docs/                             # 24 embedded reference docs
│       ├── prompts/                          # 57 module prompt files
│       ├── scripts/                          # 19 top-level operational scripts
│       │   └── tests/                        # 10 Python test modules + README
│       │       ├── test-install.py
│       │       ├── test-preflight.py
│       │       ├── test-setup-control-repo.py
│       │       ├── test-create-pr.py
│       │       ├── test-phase-conductor-contracts.py
│       │       ├── test-adversarial-review-contracts.py
│       │       ├── test-batch-mode-contracts.py
│       │       ├── test-light-preflight.py
│       │       ├── test-store-github-pat.py
│       │       └── test-validate-phase-artifacts.py
│       ├── skills/                           # 41 skill directories
│       ├── tests/
│       │   └── contracts/                    # 8 behavior contract references
│       │       ├── branch-parsing.md
│       │       ├── governance.md
│       │       ├── provider-adapter.md
│       │       ├── sensing.md
│       │       ├── two-branch-topology.md
│       │       ├── universal-planning-handoffs.md
│       │       ├── init-initiative-follow-up.md
│       │       └── preplan-brainstorm-governance.md
│       ├── _module-installer/
│       │   └── installer.js                  # IDE adapter generator
│       ├── README.md
│       ├── TODO.md
│       ├── bmadconfig.yaml                   # Canonical module config template
│       ├── lifecycle.yaml                    # Lifecycle contract and phase model
│       ├── module-help.csv                   # Command/help registry
│       └── module.yaml                       # Module identity and published surface
└── docs/                                     # Source-project documentation (this scan)
    └── *.md / project-scan-report.json
```

## Critical Directories

### `_bmad/`

**Purpose:** Source root for the module and bridge configuration.

**Contains:** `config.yaml` (bridges Lens wrapper expectations) plus the full `lens-work` module directory.

### `_bmad/lens-work/`

**Purpose:** Core product implementation.

**Entry Points:** `module.yaml`, `lifecycle.yaml`, `scripts/install.py`, `agents/lens.agent.md`, `_module-installer/installer.js`.

**Why it matters:** Everything the framework publishes originates here.

### `_bmad/lens-work/skills/`

**Purpose:** User-facing behavior model — 41 skill directories.

**Pattern:** Each skill has a `SKILL.md` and typically a `scripts/` subdirectory with implementation and test files.

### `_bmad/lens-work/prompts/`

**Purpose:** 57 authoritative prompt implementations, one per user-invocable Lens command.

### `_bmad/lens-work/scripts/`

**Purpose:** 19 operational automation scripts.

**Key scripts:**
- `install.py` — Install Lens module into a control repo
- `preflight.py` — Pre-session health check
- `setup-control-repo.py` — Bootstrap a new control repo
- `create-pr.py` — Automated PR creation and management
- `validate-lens-bmad-registry.py` — Cross-check registry alignment
- `derive-next-action.py` — Determine next lifecycle action
- `validate-phase-artifacts.py` — Validate lifecycle artifact completeness
- `light-preflight.py` — Lightweight cache-aware preflight

### `_bmad/lens-work/tests/contracts/`

**Purpose:** 8 markdown contract references defining expected behavior for branch topology, provider adapters, governance, sensing, and planning handoffs.

### `_bmad/lens-work/assets/templates/`

**Purpose:** 11 authored templates supporting planning and document generation workflows (architecture, PRD, epics, stories, product brief, readiness, user profile, planning artifacts).

### `_bmad/lens-work/docs/`

**Purpose:** 24 embedded reference docs covering architecture, source tree, lifecycle, onboarding, development, adapter reference, pipeline flow, configuration, and version notes.

## File Organization Patterns

- Module-level files under `_bmad/lens-work/` hold all real logic and contracts.
- Embedded technical documentation is concentrated under `_bmad/lens-work/docs/`.
- Operational tests are split between executable pytest files in `scripts/tests/` and reference contracts in `tests/contracts/`.
- Template assets live under `_bmad/lens-work/assets/templates/`.

## Configuration and Registry Files

| File | Role |
| --- | --- |
| `_bmad/config.yaml` | Bridge config for Lens wrapper flows |
| `_bmad/lens-work/bmadconfig.yaml` | Authoritative module config template |
| `_bmad/lens-work/module.yaml` | Skill, prompt, script, and dependency registry |
| `_bmad/lens-work/module-help.csv` | Command/help registry |
| `_bmad/lens-work/assets/lens-bmad-skill-registry.json` | Published prompt-to-skill mapping |

---

_Generated by lens-bmad-document-project for feature lens-dev-old-codebase-discovery._
