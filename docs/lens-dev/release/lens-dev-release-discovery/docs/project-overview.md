# lens.core.release — Project Overview

**Feature:** lens-dev-release-discovery
**Generated:** 2026-05-20

---

## What Is This Project?

`lens.core.release` is the **distribution artifact** for LENS Workbench v4.0.0 — a skills-first, AI-guided lifecycle orchestration module built on the BMAD (Build, Measure, Analyze, Design) framework.

It is the release-ready bundle that users clone into their control repos. Unlike its source counterpart (`lens.core.src`), this repo contains no authoring scaffolding — it is the consumable payload: all skills, prompts, scripts, bundled BMAD modules, and pre-generated IDE adapters.

## Identity

| Field | Value |
| --- | --- |
| Module code | `lens` |
| Module name | LENS Workbench |
| Version | 4.0.0 |
| Schema version | 4 |
| Type | Standalone BMAD Module |
| Dependencies | `core`; optional: `cis`, `tea` |
| Distribution repo | https://github.com/crisweber2600/lens.core.git (beta branch) |

## Core Purpose

LENS Workbench provides an AI-agent workflow layer for **feature-first planning and delivery** in software projects. It introduces:

- A disciplined **two-branch git topology** (feature + feature-plan branches)
- **Git-derived state** — no git-ignored runtime state files; everything persists in committed artifacts and PRs
- **Phase-gated lifecycle** — PRs are the only gating mechanism; side-channel approval is explicitly rejected
- **Cross-initiative sensing** — automatic awareness of related features at lifecycle gates
- A **constitutional governance system** — org/domain/service/repo additive hierarchy

## Design Principles

From `lifecycle.yaml`:

| Principle | Statement |
| --- | --- |
| **Git is truth** | Git is the only source of truth for shared workflow state |
| **PRs gate everything** | PRs are the only gating mechanism; no side-channel approval |
| **Authority domains are explicit** | Every file belongs to exactly one authority |
| **Sensing is automatic** | Cross-initiative awareness checked at lifecycle gates, not manually |
| **Control repo is operational** | The control repo is a workspace, not a code repo |

## What's in the Release Bundle

The release artifact packages everything a consumer needs:

### Primary Module — `_bmad/lens-work/`
The LENS lifecycle module itself. Contains all 41 skills, 57 prompts, 19 operational scripts, lifecycle contract, and IDE adapters.

### Bundled BMAD Modules — `_bmad/`
All supporting BMAD modules distributed alongside lens-work:

| Module | Purpose |
| --- | --- |
| `bmb/` | BMAD Builder — agent and workflow builder skills |
| `bmm/` | BMAD Methodologies — analysis, planning, solutioning, implementation |
| `cis/` | Creative Innovation Studio — brainstorming, problem-solving, storytelling |
| `core/` | Core BMAD skills — brainstorming, distillation, editorial, party mode |
| `gds/` | Game Design System — game design and development workflows |
| `tea/` | Test Engineering Agent — test architecture and automation |
| `wds/` | Web Design System — UX design and frontend workflows |

### Pre-Generated IDE Adapters
The release ships with adapter stubs ready for each supported IDE:

| Adapter | Path | Loads prompts from |
| --- | --- | --- |
| GitHub Copilot | `.github/` | `.github/prompts/`, `.github/skills/`, `.github/agents/` |
| Claude | `.claude/` | Native Claude project reference |
| Codex | `.codex/` | Codex agent reference |
| Cursor | `.cursor/` | Cursor rules reference |

### Entry Point
- **`AGENTS.md`** — Codex agent activation file; points to `lens.core/_bmad/lens-work/agents/lens.agent.md`

## Lifecycle Phases

The LENS lifecycle (schema v4) defines these phases for each feature:

```
preplan → businessplan → techplan → [adversarial-review] → finalizeplan → dev → complete
```

Alternative paths:
- **expressplan** — Rapid planning track (collapses businessplan+techplan)
- **quickplan** — End-to-end pipeline conductor

Each phase transition requires a PR review. Phase state is derived entirely from git branch existence, PR status, and committed artifact presence — no external state stores.

## Key Files

| File | Role |
| --- | --- |
| `_bmad/lens-work/lifecycle.yaml` | Lifecycle contract — phases, milestones, gate semantics |
| `_bmad/lens-work/module.yaml` | Module identity, skill/prompt registry, installer adapters |
| `_bmad/lens-work/module-help.csv` | Command discovery registry |
| `_bmad/lens-work/agents/lens.agent.md` | Thin-shell Lens agent definition |
| `_bmad/lens-work/bmadconfig.yaml` | Source defaults for installed configuration |
| `_bmad/lens-work/README.md` | Human-readable module reference |
| `AGENTS.md` | Codex agent activation entry point |

## Relationship to Source

`lens.core.release` (this repo) is the **promoted output** of `lens.core.src` (old-codebase). The source is where skills, prompts, and scripts are authored and tested. CI/CD promotes stable source builds to the `beta` branch of this distribution repo.

Key differences:
- **No authoring path** — There is no `src/Lens.Core.Src` or `setup.py` here
- **Bundled modules** — BMAD module dependencies are co-distributed, not installed from separate repos
- **Pre-generated adapters** — IDE adapter stubs are pre-built rather than generated at install time
- **Total size** — ~6,761 files vs ~1,200 (lens-work only) in source

---

_Generated by lens-bmad-document-project for feature lens-dev-release-discovery._
