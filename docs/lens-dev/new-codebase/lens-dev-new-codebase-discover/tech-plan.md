---
feature: lens-dev-new-codebase-discover
doc_type: tech-plan
status: approved
goal: "Technical design for the discover command rewrite: component boundaries, script interface contract, auto-commit isolation, and regression coverage strategy"
key_decisions:
  - discover-ops.py owns all file operations; the skill orchestrates git directly (not via a helper script)
  - Conditional auto-commit uses a pre/post hash comparison inline in the skill (not in discover-ops.py)
  - add-entry is idempotent by remote_url uniqueness, not name uniqueness
  - Path resolution uses Path.resolve() for all disk comparisons to avoid drive-letter / symlink mismatches
  - No-remote edge case is deferred to a follow-on feature (OQ-FP2 — E)
deferred:
  - "Hash comparison ownership: resolved — inline in skill orchestration section (not in discover-ops.py)"
  - "No-remote edge case: deferred to follow-on feature (OQ-FP2 — E); note in SKILL.md out-of-scope section"
depends_on: [business-plan]
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# Tech Plan — Discover Command

**Feature:** `lens-dev-new-codebase-discover`
**Author:** CrisWeber
**Date:** 2026-04-28

---

## Overview

`discover` is a maintenance command that closes drift between a governance inventory and a local workspace. Its implementation is composed of two layers:

1. **`discover-ops.py`** — a standalone Python script responsible for file-system and YAML operations (scanning disk, querying inventory, appending entries, validating schema). All operations are deterministic and side-effect-free with respect to git.
2. **`SKILL.md` (orchestration layer)** — reads discover-ops.py output, drives user interaction or headless confirmation, invokes `git clone` for missing repos, and executes the conditional auto-commit/push to governance `main` directly.

This split cleanly isolates testable file logic from git operations. The git layer is thin — three commands (`git add`, `git commit`, `git push`) called inline by the skill — and does not require its own script abstraction.

---

## System Architecture

```
User (or headless caller)
  │
  ▼
lens-discover.prompt.md  ← stub: runs light-preflight, redirects to release prompt
  │
  ▼
lens.core/_bmad/lens-work/prompts/lens-discover.prompt.md  ← resolves config, delegates to SKILL
  │
  ▼
bmad-lens-discover/SKILL.md  ← behavioral orchestration (this feature's primary deliverable)
  │
  ├── discover-ops.py scan          → reports drift in both directions (JSON)
  ├── git clone ...                 → for each entry in missing_from_disk
  ├── discover-ops.py add-entry     → for each entry in untracked
  ├── discover-ops.py validate      → sanity-check inventory after mutations
  └── git add / commit / push       → only if inventory hash changed (auto-commit exception)
```

### Component Boundaries

| Component | Owner | Writes to |
|---|---|---|
| `lens-discover.prompt.md` (stub) | lens-work release | — (redirect only) |
| `bmad-lens-discover/SKILL.md` | This feature | Orchestration only — no direct file writes |
| `discover-ops.py` | This feature | `repo-inventory.yaml` (via `add-entry`) |
| Git layer (inline in skill) | This feature | Governance `main` (the approved exception) |

---

## Script Interface Contract

### `discover-ops.py scan`

**Purpose:** Compare `repo-inventory.yaml` against `TargetProjects/` disk state. Pure read — no mutations.

**Arguments:**
```
scan
  --inventory-path   PATH    Path to repo-inventory.yaml
  --target-root      PATH    Root of local TargetProjects/ directory
  [--json]                   Emit structured JSON output (required for skill consumption)
```

**Output (JSON):**
```json
{
  "missing_from_disk": [
    { "name": "Repo.Name", "remote_url": "https://...", "local_path": "TargetProjects/..." }
  ],
  "untracked": [
    { "name": "Repo.Name", "local_path": "TargetProjects/..." }
  ],
  "already_cloned": [
    { "name": "Repo.Name", "local_path": "TargetProjects/..." }
  ],
  "summary": {
    "missing_from_disk": 0,
    "untracked": 0,
    "already_cloned": 0
  }
}
```

**Path resolution rules:**
- Inventory entries with `local_path` starting with `TargetProjects/` are resolved relative to the project root (parent of `--target-root`)
- Disk entries are detected by walking `--target-root` up to 3 levels deep, finding directories with a `.git` subdirectory
- All path comparisons use `Path.resolve()` to handle symlinks and drive-letter normalisation on Windows

**YAML key normalisation:**
- Accepts both `repositories:` (canonical) and `repos:` (legacy) at the top level
- Always writes `repositories:` key on any mutation

---

### `discover-ops.py add-entry`

**Purpose:** Append a new repo entry to `repo-inventory.yaml`. Idempotent by `remote_url`.

**Arguments:**
```
add-entry
  --inventory-path   PATH    Path to repo-inventory.yaml
  --name             STR     Repository name (display label)
  --remote-url       URL     Git remote URL (deduplication key)
  [--local-path      STR]    Relative local path (default: TargetProjects/{name})
  [--json]                   Emit structured JSON output
```

**Output (JSON):**
```json
{ "added": true, "reason": "new_entry" }
// or
{ "added": false, "reason": "already_exists" }
```

**Idempotency guarantee:** If an entry with the same `remote_url` already exists, the file is not written and `added: false` is returned. No duplicate entries are ever created.

---

### `discover-ops.py validate`

**Purpose:** Check all entries in the inventory have required fields (`name`, `remote_url`). Pure read — no mutations.

**Arguments:**
```
validate
  --inventory-path   PATH    Path to repo-inventory.yaml
  [--json]                   Emit structured JSON output
```

**Output (JSON):**
```json
{
  "valid": true,
  "errors": []
}
// or
{
  "valid": false,
  "errors": [
    { "index": 2, "name": null, "remote_url": "https://...", "issue": "missing name" }
  ]
}
```

---

## Auto-Commit Design (Governance-Main Exception)

The conditional auto-commit is implemented inline in the skill's orchestration section. It must not be extracted into a helper script or util module to keep its isolation visible.

### Algorithm

```
1. Capture sha256 hash of repo-inventory.yaml BEFORE any add-entry calls
2. Run all clone operations (for missing_from_disk entries)
3. Run all add-entry operations (for untracked entries)
4. Capture sha256 hash of repo-inventory.yaml AFTER add-entry calls
5. IF pre-hash == post-hash:
      Print "[discover] Nothing to do — inventory unchanged."
      Exit 0 (no commit)
6. ELSE:
      git -C {governance_repo_path} add repo-inventory.yaml
      git -C {governance_repo_path} commit -m "[discover] Sync repo-inventory.yaml"
      git -C {governance_repo_path} push
      Print "[discover] Inventory updated and pushed to governance main."
```

### Why a Hash Comparison?

A hash comparison is the most reliable guard. It detects file content changes even if `git status` has stale index state, and it is consistent across platforms. An alternative (`git diff --quiet`) would also work but requires the file to be tracked by git — the hash approach works even for a freshly initialised inventory.

### Isolation Requirement

This conditional-commit block is the ONLY place in the entire `lens-work` skill set that is permitted to `git push` to governance `main` outside of `publish-to-governance`. To enforce isolation:

- The commit logic is in `SKILL.md` under a clearly labelled section: `## Auto-Commit (Governance-Main Exception)`
- No other SKILL.md in the skill set may reference or copy this section
- Architecture review (SC-5 in business-plan) audits for this before `dev-complete`

---

## Configuration Resolution

The skill resolves the governance repo path through two sources in priority order:

1. **Primary:** `.lens/governance-setup.yaml` → `governance_repo_path` field
2. **Fallback:** `lens.core/_bmad/_config/custom/lens-work/lens-work/bmadconfig.yaml` → `governance_repo_path` field

The `target_projects_path` default is `TargetProjects/` relative to the workspace root (the parent of `lens.core/`).

---

## Interaction Design

### Interactive Mode (default)

```
[discover] Scanning TargetProjects/ against repo-inventory.yaml…

  Missing from disk (1):
    → Lens.Hermes  (https://github.com/example/Lens.Hermes)

  Untracked on disk (1):
    → TargetProjects/plugins/notify/Lens.Notify

Clone missing repos and register untracked repos? [Y/n]
> Y

  Cloning Lens.Hermes … done
  Registering Lens.Notify … done
  Committing inventory update to governance main … done ✓

[discover] Complete. 1 cloned, 1 registered.
```

### Headless Mode (`--headless` / `-H`)

All confirmation prompts are skipped. All discovered actions execute immediately.

### Dry-Run Mode (`--dry-run`)

Scan output is printed. No files are written, no clones are executed, no git commits are made.

### No-Op Output

```
[discover] Scanning TargetProjects/ against repo-inventory.yaml…
[discover] Nothing to do — all repos are cloned and registered.
```

---

## Architectural Decision Records

### ADR-1: Script owns file operations, skill owns git

**Decision:** `discover-ops.py` handles all YAML read/write. The skill handles all git operations.

**Rationale:** The script can be tested without git infrastructure. The git operations are so simple (three commands) that abstracting them into a script would add ceremony without benefit. Keeping git in the skill also keeps the auto-commit exception visible at the orchestration layer rather than hidden in a utility.

**Consequences:** The skill's auto-commit section is the canonical reference for the governance-main exception. Future contributors looking for git write operations in skills will find a clear example here — and a clear non-example in all other skills.

---

### ADR-2: Idempotency key for add-entry is remote_url, not name

**Decision:** An entry is considered "already registered" if its `remote_url` matches an existing entry, regardless of name.

**Rationale:** Repository names can change (e.g., renames, forks). The remote URL is the stable identifier. Using name as the key would allow duplicate entries to accumulate if a repo is renamed without updating the inventory.

**Consequences:** `--remote-url` is a required argument for `add-entry`. The skill must resolve the remote URL before calling `add-entry` for untracked repos (via `git remote get-url origin`).

---

### ADR-3: Conditional commit uses file hash comparison

**Decision:** Pre/post SHA-256 hash of `repo-inventory.yaml` determines whether to commit.

**Rationale:** More robust than `git diff` (works on untracked files), simpler than tracking a mutation flag across multiple add-entry calls, and cross-platform.

**Consequences:** The skill must capture the hash before any mutations and compare after. This is two `hashlib.sha256` calls with a file read — lightweight and deterministic.

---

### ADR-4: Path comparisons use Path.resolve()

**Decision:** All path equality checks use `Path.resolve()` before comparison.

**Rationale:** Windows drive letters, relative paths, and symlinks all produce different string representations of the same physical location. `resolve()` normalises all of these.

**Consequences:** Tests must construct paths using the same resolution chain. The existing `test_scan_tracks_project_root_relative_local_path` test already follows this pattern.

---

## Testing Strategy

### Test Infrastructure (already established)

- `tempfile.TemporaryDirectory` for isolation
- `init_repo(path)` helper: creates a real `git init` repo with an initial commit
- `subprocess.run` to call the script directly
- JSON assertion helpers: `assert_eq`, `assert_true`
- Script target: `lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/test-discover-ops.py`

### Required Test Cases

| ID | Test Name | Scenario | Pass Condition |
|---|---|---|---|
| T1 | `test_scan_tracks_project_root_relative_local_path` | Inventory has TargetProjects/-relative path; repo exists on disk | `already_cloned` count = 1, `missing_from_disk` = 0, `untracked` = 0 |
| T2 | `test_scan_reports_untracked_repo_with_targetprojects_prefix` | Repo on disk not in inventory | `untracked` count = 1, `already_cloned` = 0 |
| T3 | `test_scan_reports_missing_from_disk` | Inventory entry present; no local repo | `missing_from_disk` count = 1 |
| T4 | `test_add_entry_creates_new_entry` | Empty inventory; add new entry | `added: true`, inventory file contains one entry |
| T5 | `test_add_entry_is_idempotent_by_remote_url` | Entry with same remote_url already in inventory | `added: false`, inventory file unchanged |
| T6 | `test_validate_passes_well_formed_inventory` | All entries have name and remote_url | `valid: true`, `errors: []` |
| T7 | `test_validate_fails_missing_name` | Entry missing name field | `valid: false`, error recorded |
| T8 | `test_noop_run_produces_unchanged_hash` | Disk and inventory in sync | File hash before == file hash after; no commit triggered |

### Run Command

```bash
uv run --with pytest pytest lens.core/_bmad/lens-work/skills/bmad-lens-discover/scripts/tests/test-discover-ops.py -q
```

---

## Implementation Channel

All SKILL.md and script authoring must go through BMB-first:

- **OW (Open Workflow):** Use for creating or modifying existing workflow files within a BMad module
- **EW (Edit Workflow):** Use for editing a workflow step
- **CW (Create Workflow):** Use for creating new skill assets from scratch

The BMad Builder docs at `lens.core/_bmad/bmb/` must be consulted before creating or modifying any lens-work skill or workflow. This is a hard requirement from the domain constitution.

### Files to Author / Update (via BMB)

| File | Action | Notes |
|---|---|---|
| `bmad-lens-discover/SKILL.md` | Review + finalize | Exists; verify auto-commit section, headless mode, dry-run mode, no-op path, config resolution |
| `bmad-lens-discover/scripts/discover-ops.py` | Review + patch | Exists; add conditional auto-commit guard if not present; verify path resolution uses `resolve()` |
| `bmad-lens-discover/scripts/tests/test-discover-ops.py` | Extend | Exists partially; add T3–T8 test cases |
| `.github/prompts/lens-discover.prompt.md` | Create stub | Standard stub: run light-preflight.py, redirect to release prompt |
| `lens.core/_bmad/lens-work/prompts/lens-discover.prompt.md` | Verify/create | Release prompt: resolve config, delegate to SKILL |
