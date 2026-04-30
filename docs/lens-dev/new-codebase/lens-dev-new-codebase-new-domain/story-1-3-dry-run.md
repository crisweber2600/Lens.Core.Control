---
feature_id: lens-dev-new-codebase-new-domain
story_key: "1-3-dry-run"
epic: 1
story: 3
title: "Implement dry-run mode"
type: implementation
estimate: S
priority: P1
status: not-started
assigned: crisweber2600
sprint: 1
depends_on:
  - "1-2-core-flow"
blocks:
  - "1-5-integration-tests"
created_at: 2026-04-26T00:00:00Z
updated_at: 2026-04-26T00:00:00Z
---

# Story 1.3 — Implement dry-run mode

## What To Build

Add `--dry-run` support to the `create-domain` subcommand. When active, the script must compute all planned operation paths and return them in JSON — without creating any file or running any git command.

---

## File Locations

| File | Action |
|---|---|
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py` | Add dry-run guard |

---

## Behavior

When `--dry-run` is set:

1. `validate_safe_id` runs normally (this is a read-only check, not a write)
2. All planned paths are computed (domain.yaml path, constitution path, scaffold paths, context path)
3. No file is created
4. No git command is executed (no pull, no add, no commit, no push)
5. JSON is returned to stdout with all path fields populated as if they would have been written
6. `dry_run: true` appears in the JSON output

**Return JSON example (dry-run):**
```json
{
  "status": "pass",
  "scope": "domain",
  "path": "/governance/features/my-domain/domain.yaml",
  "constitution_path": "/governance/constitutions/my-domain/constitution.md",
  "created_marker_paths": [],
  "created_constitution_paths": [],
  "target_projects_path": null,
  "docs_path": null,
  "context_path": "/personal/context.yaml",
  "governance_git_commands": [],
  "workspace_git_commands": [],
  "remaining_git_commands": [],
  "governance_git_executed": false,
  "governance_commit_sha": null,
  "dry_run": true
}
```

---

## Acceptance Criteria

- [ ] `--dry-run` flag accepted by `create-domain` subcommand
- [ ] No files created when `--dry-run` is active
- [ ] No git commands executed when `--dry-run` is active
- [ ] All planned paths (domain.yaml, constitution, scaffolds if flags provided, context path) populated in JSON output
- [ ] `dry_run: true` present in output JSON
- [ ] `test_create_domain_dry_run` passes:
  - Invokes with `--dry-run`
  - Asserts `dry_run: true` in output
  - Asserts target files do NOT exist on disk after invocation
  - Asserts returned paths are non-null strings
