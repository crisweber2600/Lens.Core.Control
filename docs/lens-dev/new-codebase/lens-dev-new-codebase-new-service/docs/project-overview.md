# Project Overview: bmad-lens-init-feature / create-service

**Feature:** lens-dev-new-codebase-new-service  
**Generated:** 2026-04-27  
**Project type:** Python CLI — uv-runnable inline-dependency script  
**Language:** Python 3.13  
**Dependencies (inline):** `pyyaml>=6.0`

---

## Purpose

`init-feature-ops.py` is the core governance container initialization script for Lens Workbench. It provisions domain and service containers in a governance repository — creating marker YAML files, constitution markdown files, workspace scaffold `.gitkeep` files, and optionally executing governance git operations.

This feature added the `create-service` subcommand (analogous to the existing `create-domain`) along with its SKILL.md intent flow, release prompt, and module-help registration.

---

## Architecture

### CLI Entry Point

```
init-feature-ops.py <subcommand> [args]

Subcommands:
  create-domain   — Create a governance domain container
  create-service  — Create a governance service container within a domain (new)
```

The script is uv-runnable with inline dependencies:

```python
# /// script
# requires-python = ">=3.11"
# dependencies = ["pyyaml>=6.0"]
# ///
```

### Module Layout

| Layer | Components |
|-------|-----------|
| **Utilities** | `now_iso`, `validate_safe_id`, `git_command_argv/text`, `run_git`, `ensure_git_worktree`, `ensure_clean_worktree`, `current_head_sha`, `sync_governance_main`, `atomic_write_yaml`, `write_context_yaml`, `unique_paths` |
| **Domain helpers** | `get_domain_marker_path`, `get_domain_constitution_path`, `make_domain_yaml`, `make_domain_constitution_md`, `build_container_git_steps`, `build_container_result_fields`, `build_workspace_scaffold_commands`, `resolve_personal_folder` |
| **Service helpers** (new) | `get_service_marker_path`, `get_service_constitution_path`, `make_service_yaml`, `make_service_constitution_md` |
| **Command handlers** | `cmd_create_domain`, `cmd_create_service` (new) |
| **Parser** | `build_parser` — argparse with subparsers |
| **Entry** | `main` — routes subcommand, prints JSON, returns exit code |

### Key Design Decisions

**ADR-2: Service-container boundary**  
`create-service` must never create `feature.yaml`, `summary.md`, feature-index entries, or control branches. The service container is an organizational grouping only — features live inside services, not the other way around.

**ADR-3: Delegation boundary**  
`create-service` delegates parent-domain creation to `create-domain` helpers (`make_domain_yaml`, `make_domain_constitution_md`). No parallel domain-marker path exists. When `parent_domain_absent=True`, `cmd_create_service` calls these helpers directly rather than re-implementing domain creation.

---

## API Contracts

### `create-service` Success Payload

```json
{
  "status": "pass",
  "scope": "service",
  "dry_run": false,
  "path": "features/lens-dev/new-codebase/service.yaml",
  "constitution_path": "constitutions/lens-dev/new-codebase/constitution.md",
  "created_marker_paths": ["features/lens-dev/new-codebase/service.yaml"],
  "created_constitution_paths": ["constitutions/lens-dev/new-codebase/constitution.md"],
  "created_domain_marker": false,
  "created_domain_constitution": false,
  "target_projects_path": "TargetProjects/lens-dev/new-codebase",
  "docs_path": "docs/lens-dev/new-codebase",
  "context_path": ".lens/personal/context.yaml",
  "governance_git_executed": false,
  "governance_commit_sha": null,
  "git_commands": [...],
  "governance_git_commands": [...],
  "workspace_git_commands": [...],
  "remaining_git_commands": [...],
  "error": null
}
```

### ID Validation

Both `--domain` and `--service` IDs must match: `^[a-z0-9][a-z0-9._-]{0,63}$`

### Mutual Exclusion

`--dry-run` and `--execute-governance-git` are mutually exclusive — passing both returns an error immediately with no writes.

---

## Test Coverage

| Test file | Count | Scope |
|-----------|-------|-------|
| `test-create-service-ops.py` | 16 pass | CLI contract, dry-run, duplicate check, invalid IDs, scaffold paths, context.yaml, governance git, mutual exclusion, boundary (no feature artifacts), domain idempotency, discovery |
| `test-init-feature-ops.py` | 11 pass, 1 pre-existing failure | `create-domain` contract (unrelated to this feature) |

### Test runner command

```bash
cd TargetProjects/lens-dev/new-codebase/lens.core.src
uv run --with pytest --with pyyaml python -m pytest \
  "_bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-create-service-ops.py" \
  "_bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-init-feature-ops.py" -q
```

---

## Discovery Surfaces

The `new-service` command is reachable through three surfaces:

| Surface | File | Path |
|---------|------|------|
| Stub prompt | `.github/prompts/lens-new-service.prompt.md` | Runs preflight, delegates to release prompt |
| Release prompt | `_bmad/lens-work/prompts/lens-new-service.prompt.md` | Loads SKILL.md, executes create-service intent flow |
| Module help | `_bmad/lens-work/module-help.csv` | Row: `NS,new-service,Initialize a service container...` |

---

## Files Changed in This Feature

| File | Change |
|------|--------|
| `_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md` | Added new-service intent flow (7 steps) |
| `_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py` | Added service helpers + `cmd_create_service` + parser route + context writer extension |
| `_bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-create-service-ops.py` | New: 16 tests |
| `_bmad/lens-work/prompts/lens-new-service.prompt.md` | New: release prompt |
| `.github/prompts/lens-new-service.prompt.md` | New: stub prompt |
| `_bmad/lens-work/module-help.csv` | New: copied from release + new-domain and new-service rows |

**Commits:** `d3fc3a9f` (NS-8/NS-9), `90cb00e6` (NS-1–7/NS-10) on branch `develop`
