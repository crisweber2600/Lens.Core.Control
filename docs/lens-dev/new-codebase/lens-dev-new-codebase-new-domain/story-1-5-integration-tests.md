---
feature_id: lens-dev-new-codebase-new-domain
story_key: "1-5-integration-tests"
epic: 1
story: 5
title: "Integration test suite with isolated fixtures"
type: test
estimate: M
priority: P0
status: not-started
assigned: crisweber2600
sprint: 1
depends_on:
  - "1-2-core-flow"
  - "1-3-dry-run"
blocks: []
created_at: 2026-04-26T00:00:00Z
updated_at: 2026-04-26T00:00:00Z
---

# Story 1.5 — Integration test suite with isolated fixtures

## Why This Story Exists

The tech plan specifies 6 integration tests. The finalizeplan review (Murat finding, F9) requires all integration tests use isolated temp-dir governance repo fixtures. No test may reference a real governance repo path (e.g., `TargetProjects/lens/lens-governance`).

---

## File Locations

| File | Action |
|---|---|
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-init-feature-ops.py` | Add integration test cases |

---

## Fixture Requirements

Every test that creates files must:

1. Use `pytest tmp_path` fixture for the governance repo root
2. Initialize the temp dir as a valid git repo (`git init`, `git commit --allow-empty` for baseline) when testing governance git operations
3. Assert cleanup is handled by `tmp_path` scope automatically (no manual cleanup)

**Do not** reference any real path: no `TargetProjects/lens/lens-governance`, no absolute Windows path, no path containing the workspace root.

---

## Test Specifications

### `test_create_domain_dry_run`
- Invokes `create-domain --dry-run` with temp governance repo
- Asserts `dry_run: true` in output JSON
- Asserts no files exist at any of the returned paths

### `test_create_domain_basic`
- Invokes `create-domain` (without `--execute-governance-git`) with temp governance repo and a resolved/explicit personal folder
- Asserts `domain.yaml` exists at `{tmp_governance}/features/{domain}/domain.yaml`
- Asserts `constitution.md` exists at `{tmp_governance}/constitutions/{domain}/constitution.md`
- Asserts `context.yaml` exists at `{tmp_personal}/context.yaml`
- Asserts `status: pass` in output JSON

### `test_create_domain_with_scaffolds`
- Invokes `create-domain` with both `--target-projects-root` and `--docs-root` pointing to temp dirs
- Includes `--personal-folder` pointing to a temp dir
- Asserts `.gitkeep` at `{tmp_target}/{domain}/.gitkeep`
- Asserts `.gitkeep` at `{tmp_docs}/{domain}/.gitkeep`
- Asserts `context.yaml` at `{tmp_personal}/context.yaml`
- Asserts `created_marker_paths` contains governance marker paths only
- Asserts scaffold locations are represented by `target_projects_path` and `docs_path`

### `test_create_domain_duplicate_fails`
- Pre-creates `{tmp_governance}/features/{domain}/domain.yaml` before invocation
- Invokes `create-domain`
- Asserts `status: fail` in output JSON
- Asserts exit code 1
- Asserts no additional files were created (constitution.md not written, no scaffold files)

### `test_create_domain_execute_governance_git`
- Initializes temp governance repo as a real git repo with an initial commit
- Invokes `create-domain --execute-governance-git`
- Asserts `governance_git_executed: true` in output JSON
- Asserts `governance_commit_sha` is a non-empty string in output JSON
- Asserts `domain.yaml` and `constitution.md` exist

### `test_create_domain_governance_git_dirty_repo`
- Initializes temp governance repo as a real git repo
- Creates an untracked file to make the repo dirty (or creates a modified tracked file)
- Invokes `create-domain --execute-governance-git`
- Asserts `status: fail` in output JSON
- Asserts exit code 1
- Asserts `domain.yaml` was NOT written (fail occurs before writes)

---

## Acceptance Criteria

- [ ] All 6 tests listed above are implemented and pass
- [ ] All tests use `pytest tmp_path` for governance repo root
- [ ] No test references any real governance repo path
- [ ] Tests are independent: running them in any order produces the same results
- [ ] No shared mutable state between tests (no module-level test fixtures that persist between tests)
- [ ] All tests pass via: `uv run --with pytest --with pyyaml pytest scripts/tests/test-init-feature-ops.py -q`

## Review Requirement

Story 1.5 review must verify no test references a real governance repo path. Reviewer searches test file for `TargetProjects`, `lens-governance`, and any absolute path strings.
