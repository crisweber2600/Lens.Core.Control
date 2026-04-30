# Story NS-1: Define service CLI contract tests

Status: ready-for-dev

## Story

As a Lens setup user,
I want the `create-service` command to have a documented, tested CLI contract,
so that I can rely on its observable behavior regardless of the internal implementation.

## Acceptance Criteria

1. Failing test exists for `create-service` success path (standard invocation produces expected JSON fields)
2. Failing test exists for `--dry-run` behavior (no files written; dry_run=true in output)
3. Failing test exists for duplicate service rejection (service already exists in governance)
4. Failing test exists for invalid service ID rejection (bad characters, empty string)
5. Failing test exists for scaffold output structure (correct paths returned in output)
6. Failing test exists for context file output (`context_path` in JSON, file written)
7. Failing test exists for governance git behavior (`--execute-governance-git` returns `governance_commit_sha`)
8. Failing test exists for `--dry-run + --execute-governance-git` mutual exclusion (rejected-arguments error, no files written)
9. All 8 tests are in red state — `create-service` subcommand does not yet exist

## Tasks / Subtasks

- [ ] Task 1: Create test file `tests/test_create_service.py` (AC: 1–9)
  - [ ] Define `test_create_service_success_json_fields` — assert all fields from `tech-plan.md` API Contracts section present
  - [ ] Define `test_create_service_dry_run_no_writes` — assert no filesystem changes; `dry_run: true` in output
  - [ ] Define `test_create_service_duplicate_rejected` — setup existing service marker; assert error/rejection JSON
  - [ ] Define `test_create_service_invalid_id_rejected` — pass invalid slug; assert error JSON
  - [ ] Define `test_create_service_scaffold_paths` — assert scaffold output paths in `git_commands` or `workspace_git_commands`
  - [ ] Define `test_create_service_context_path` — assert `context_path` in output; assert file exists after run
  - [ ] Define `test_create_service_governance_git` — pass `--execute-governance-git`; assert `governance_commit_sha` present
  - [ ] Define `test_create_service_dry_run_plus_execute_git_rejected` — pass both flags; assert rejection error without file writes

## Dev Notes

- Script target: `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py`
- Test location: `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/`
- Test runner: `uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/ -k create_service -q`
- All tests must be **red** at the end of NS-1 — do not implement `create-service` in this story
- JSON output contract is in `tech-plan.md` § "Script CLI Contract" success payload fields
- The mutual-exclusion error (AC 8) must not write any files — verify filesystem state after the rejected call

### Project Structure Notes

- New test file goes beside `test_init_feature_ops.py` in the same `tests/` directory
- Use the same `tmp_path` and `uv run` invocation patterns as existing tests
- Import no old-codebase source — clean-room: tests may only import from the new-codebase script path

### References

- [tech-plan.md § Script CLI Contract](../tech-plan.md)
- [sprint-plan.md § Sprint 1](../sprint-plan.md)
- [stories.md § NS-1](../stories.md)

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List
