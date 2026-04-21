# lens.core.src — Component Inventory

**Date:** 2026-04-21
**Feature:** lens-dev-old-codebase-discovery
**Module Version:** 4.0.0

## Overview

`lens.core.src` is composed of source assets rather than runtime services. The deep scan inventoried what is published, implemented, and validated.

## Module Core Components

| Component Area | Count | Key Paths | Purpose |
| --- | --- | --- | --- |
| Config bridge | 1 file | `_bmad/config.yaml` | Maps Lens wrapper expectations to the module |
| Module manifest | 1 file | `_bmad/lens-work/module.yaml` | Defines published skills, prompts, scripts, and metadata (v4.0.0) |
| Lifecycle contract | 1 file | `_bmad/lens-work/lifecycle.yaml` | Defines lifecycle phases, tracks, and promotion rules |
| Module config template | 1 file | `_bmad/lens-work/bmadconfig.yaml` | Canonical runtime config template |
| Help registry | 1 file | `_bmad/lens-work/module-help.csv` | Command/help metadata |
| Asset registry | 1 file | `_bmad/lens-work/assets/lens-bmad-skill-registry.json` | Published prompt-to-skill mapping |

## Published Behavior Surface

### Skills (41 directories)

Located under `_bmad/lens-work/skills/`. Includes lifecycle skills, utility skills, governance skills, and BMAD wrapper skills.

Representative examples:
- `bmad-lens-next` — Determine next lifecycle action
- `bmad-lens-status` — Show current lifecycle status
- `bmad-lens-switch` — Switch active feature context
- `bmad-lens-complete` — Complete a lifecycle phase
- `bmad-lens-document-project` — Document a project for AI context
- `bmad-lens-init-feature` — Initialize a new feature
- `bmad-lens-git-orchestration` — Git branch and PR orchestration
- `bmad-lens-migrate` — Schema migration support
- `bmad-lens-help` — Help and topic lookup
- `bmad-lens-sensing` — Repository sensing and state derivation

### Prompts (57 files)

Located under `_bmad/lens-work/prompts/`. Each file is the authoritative implementation for one user-invocable Lens command.

### Agents

- `_bmad/lens-work/agents/lens.agent.md` — Authored runtime agent definition
- `_bmad/lens-work/agents/lens.agent.yaml` — Agent manifest

## Automation and Validation Components

### Operational Scripts (19 files)

Located under `_bmad/lens-work/scripts/`. Main groups:

**Installation and setup**
- `install.py`
- `setup-control-repo.py`

**Preflight and validation**
- `preflight.py`
- `light-preflight.py`
- `validate-lens-bmad-registry.py`
- `validate-phase-artifacts.py`

**Git and PR automation**
- `create-pr.py`

**State derivation**
- `derive-next-action.py`

### Executable Test Surface (10 modules + README)

Located under `_bmad/lens-work/scripts/tests/`:

| Test Module | Coverage Area |
| --- | --- |
| `test-install.py` | Installer and adapter generation |
| `test-preflight.py` | Preflight health check |
| `test-light-preflight.py` | Lightweight preflight |
| `test-setup-control-repo.py` | Control repo bootstrap |
| `test-create-pr.py` | PR automation |
| `test-phase-conductor-contracts.py` | Phase conductor contract verification |
| `test-adversarial-review-contracts.py` | Adversarial review contract validation |
| `test-batch-mode-contracts.py` | Batch mode behavior contracts |
| `test-store-github-pat.py` | PAT storage logic |
| `test-validate-phase-artifacts.py` | Phase artifact validation |

### Contract Reference Surface (8 files)

Located under `_bmad/lens-work/tests/contracts/`:

| Contract | Coverage |
| --- | --- |
| `branch-parsing.md` | Git branch naming and parsing rules |
| `governance.md` | Governance repo integration |
| `provider-adapter.md` | Provider adapter behavior |
| `sensing.md` | Repository sensing logic |
| `two-branch-topology.md` | Control/target branch topology |
| `universal-planning-handoffs.md` | Planning phase handoffs |
| `init-initiative-follow-up.md` | Initiative initialization follow-up |
| `preplan-brainstorm-governance.md` | Pre-plan brainstorm governance |

## Documentation and Template Components

### Embedded Module Docs (24 files)

Located under `_bmad/lens-work/docs/`. Covers architecture, source tree, lifecycle, onboarding, development, adapter reference, pipeline flow, configuration, and version notes.

### Template Assets (11 files)

Located under `_bmad/lens-work/assets/templates/`. Supports planning and document generation workflows including architecture, PRD, epics, stories, product brief, readiness, user profile, and planning artifacts.

## Packaging Components

### IDE Adapter Installer

- `_bmad/lens-work/_module-installer/installer.js`
- Generates or refreshes IDE adapter surfaces during release packaging.

### Setup Skill

- `_bmad/lens-work/bmad-lens-work-setup/`
- Bootstrap skill for configuring new workspaces.

## Components Not Present by Design

- No browser UI component library
- No HTTP controller or API route layer
- No ORM or database model layer
- No frontend state management store
- No `.github/` adapter layer (this clone contains the module core only)

These omissions are expected because `lens.core.src` publishes workflow assets and lifecycle tooling, not an interactive application runtime.

## Extension Points

- Add or modify behavior under `_bmad/lens-work/skills/` with matching prompts.
- Extend validation with new scripts or tests under `_bmad/lens-work/scripts/`.
- Adjust packaging logic in `_module-installer/installer.js`.
- Update contracts in `_bmad/lens-work/tests/contracts/` when behavior boundaries change.

---

_Generated by lens-bmad-document-project for feature lens-dev-old-codebase-discovery._
