# Project Overview: bmad-lens-switch

**Feature:** lens-dev-new-codebase-switch
**Version:** 1.0.0 (initial delivery)
**Generated:** 2026-04-27
**Language:** Python ≥3.10
**Dependencies:** PyYAML ≥6.0

---

## What It Does

`bmad-lens-switch` is a Lens skill that manages the **active feature context** for a Lens agent session. It allows a developer or AI agent to:

1. **List available features** from `feature-index.yaml` (or fall back to domain/service inventory scan)
2. **Switch the active feature** — validate the target, load its context, write a local session pointer, and check out the associated control-repo branch

Switching context **never modifies governance state**. The only write is to `.lens/personal/context.yaml` — a local, gitignored session pointer.

---

## Architecture

### Single-File Core

The implementation is a single uv-runnable Python script:

```
_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py
```

The script is self-contained: no imports from other Lens modules. It reads YAML files from disk, validates inputs, writes local state, and outputs JSON to stdout. This keeps it easily testable and independently runnable.

### Command Surface

Two top-level subcommands:

| Command | Purpose | Key Output Fields |
|---------|---------|-------------------|
| `list` | List available features | `mode`, `features` or `domains`, `total` |
| `switch` | Switch active feature context | `feature_id`, `phase`, `stale`, `branch_switched`, `context_paths` |

### Data Flow

```
switch-ops.py list
  └── resolve_governance_repo()        # config precedence: CLI → override file → module config
      └── load_feature_index()         # reads feature-index.yaml with size/structure validation
          └── [if empty] scan_domain_inventory()  # fallback: reads domain.yaml / service.yaml
              └── emit JSON (mode: features | domains)

switch-ops.py switch --feature-id <id>
  └── resolve_governance_repo()
      └── load_feature_index()
          └── validate_identifier()    # safe path chars only
              └── find_feature_yaml()  # locate in features/<domain>/<service>/<id>/feature.yaml
                  └── compute staleness (30-day threshold from feature.yaml updated field)
                  └── build_context_paths()  # related=summary, depends_on/blocks=tech-plan, with exists flags
                  └── write_context_yaml()   # .lens/personal/context.yaml (local only)
                  └── git_checkout_branch()  # checkout control-repo feature branch; report result
                  └── emit JSON (full feature context response)
```

---

## Key Design Decisions

### Config Resolution — 3-Tier Precedence

Governance repo path is resolved in order:
1. `--governance-repo` CLI arg (explicit override for scripts/tests)
2. `.lens/governance-setup.yaml` → `governance_repo_path` (workspace-local override)
3. Module `bmadconfig.yaml` → `governance_repo_path` (standard config)

If none of the above yield a value, the command fails with `error: config_missing` and instructs the user to run `/lens-onboard`. There is no implicit default.

### ID Validation Before File I/O

All feature IDs, domain names, and service names are validated against `SAFE_ID_PATTERN = r"^[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$"` before they are used in path construction. This prevents path traversal and ensures consistent naming.

### Staleness Detection

A feature is considered stale if the `updated` timestamp in `feature.yaml` is older than 30 days (`STALE_DAYS = 30`). The response includes `stale: true/false`. Stale warnings are surfaced inline in the switch confirmation — the switch still succeeds.

### Context Paths — Dependency-Aware

The `context_paths` field in the switch response provides pre-resolved paths to supporting artifacts for all feature relationships:

| Relationship | Path Template | Rationale |
|-------------|--------------|-----------|
| `related` | `features/<domain>/<service>/<id>/summary.md` | Light summary context only |
| `depends_on` | `features/<domain>/<service>/<id>/docs/tech-plan.md` | Full technical context needed |
| `blocks` | `features/<domain>/<service>/<id>/docs/tech-plan.md` | Full technical context needed |

Each path includes an `exists` boolean — the agent can skip loading paths that don't exist yet.

### Local-Only Context Write

The session context write goes to `.lens/personal/context.yaml`. This path is:
- **local only** — not committed to any repo
- **session pointer** — tells future agent calls which feature is active
- **the only write** during a switch — no feature.yaml, no governance repo, no control repo modifications

### Branch Checkout Reporting

The switch command attempts `git checkout <working_branch>` in the control repo. The result is reported via:
- `branch_switched: true/false`
- `branch_error: <message or null>`

Branch checkout **never blocks** the switch. If the branch doesn't exist or the tree is dirty, the switch completes with the context loaded and `branch_switched: false` + an actionable message.

---

## JSON Response Contracts

### `list` — Features Mode

```json
{
  "status": "ok",
  "mode": "features",
  "total": 3,
  "features": [
    {
      "num": 1,
      "id": "lens-dev-new-codebase-switch",
      "domain": "lens-dev",
      "service": "new-codebase",
      "phase": "dev",
      "status": "preplan",
      "owner": "crisweber2600",
      "summary": ""
    }
  ]
}
```

### `list` — Domains Mode (fallback when no index)

```json
{
  "status": "ok",
  "mode": "domains",
  "total": 2,
  "domains": [
    { "domain": "lens-dev", "services": ["new-codebase", "old-codebase"] }
  ]
}
```

### `switch` — Success

```json
{
  "status": "ok",
  "feature_id": "lens-dev-new-codebase-switch",
  "domain": "lens-dev",
  "service": "new-codebase",
  "phase": "dev",
  "track": "express",
  "priority": "medium",
  "owner": "crisweber2600",
  "stale": false,
  "context_path": "TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-switch",
  "target_repo_state": {
    "repo": "lens.core.src",
    "working_branch": "feature/switch-dev",
    "base_branch": null,
    "pr_state": null,
    "final_pr_url": null
  },
  "context_paths": [
    {
      "feature_id": "some-dependency",
      "relationship": "depends_on",
      "path": "TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/some-dependency/docs/tech-plan.md",
      "exists": true
    }
  ],
  "branch_switched": true,
  "branch_error": null,
  "message": "[lens-dev-new-codebase-switch] active. Phase: dev. Priority: medium."
}
```

### `switch` — Failure

```json
{
  "status": "fail",
  "error": "feature_not_found",
  "message": "Feature 'bad-id' not found in feature-index.yaml. Run list to see available features."
}
```

---

## Discovery Surfaces

The switch command is registered in all Lens discovery surfaces:

| Surface | File | Entry |
|---------|------|-------|
| Module help CSV | `module-help.csv` | `switch-feature` (alias `SW`), `list-features` (alias `LF`) |
| Release prompt | `prompts/lens-switch.prompt.md` | Preflight gate, config resolution contract, menu control |
| Agent menu | `agents/lens.agent.md` | `[SW] Switch: Switch the active Lens feature context` |
| Skill reference | `skills/bmad-lens-switch/SKILL.md` | Full API contract |

---

## Test Coverage

**Focused regression command (from repo root under `TargetProjects/lens-dev/new-codebase/lens.core.src`):**

```bash
uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-switch/scripts/tests/test-switch-ops.py -q
```

**Coverage:**

| Test Area | Cases |
|-----------|-------|
| Feature listing (active filter, all, empty index, domain fallback) | 4 |
| Menu numbering (1-indexed `num` field) | 1 |
| Switch success (full response shape, stale detection, context write) | 3 |
| Switch errors (invalid ID, missing feature, missing index) | 3 |
| Context paths (related/depends_on/blocks, deduplication, exists flags) | 3 |
| Branch operations (existing branch, missing branch, dirty tree) | 2 |
| Governance no-write regression | 1 |
| Discovery surface (no deprecated command strings) | 1 |
| **Total** | **18** |

---

## Known Issues / Caveats

1. **Windows encoding:** All config and template file reads must use `encoding="utf-8"` explicitly. The agent menu file contains em-dashes that the Windows cp1252 default codec cannot decode.
2. **Branch checkout requires clean working tree.** If the control repo has uncommitted changes, `git checkout` will fail. `branch_switched: false` and an actionable message are returned — the switch itself still completes.
3. **Config missing is fail-fast.** If no governance repo config is found, the command returns `error: config_missing` immediately. Run `/lens-onboard` to configure.
