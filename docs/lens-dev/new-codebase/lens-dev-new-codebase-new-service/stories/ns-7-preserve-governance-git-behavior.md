# Story NS-7: Preserve governance git behavior

Status: ready-for-dev

## Story

As a governance maintainer,
I want the `--execute-governance-git` path to be idempotent,
so that a failed and retried run never creates duplicate service markers, domain markers, or constitution files.

## Acceptance Criteria

1. `--execute-governance-git` on the success path returns `governance_commit_sha` in the JSON output
2. Remaining git commands for workspace scaffolds are included in `remaining_git_commands` when git is not executed
3. **Idempotency test passes:** run `create-service --execute-governance-git`, simulate a partial failure (files written but commit not completed), re-run; governance state must be consistent with no duplicate artifacts
4. NS-1 governance git behavior test turns green
5. Retry test demonstrates no duplicate service markers, domain markers, or constitution files on the second run

## Tasks / Subtasks

- [ ] Task 1: Wire `--execute-governance-git` in handler (AC: 1–2)
  - [ ] Reuse the same `execute_governance_git(repo, message)` helper already proven by `create-domain`
  - [ ] On success: include `governance_commit_sha` in JSON; set `governance_git_executed: true`
  - [ ] On skip: include `remaining_git_commands` with add/commit/push text; set `governance_git_executed: false`
- [ ] Task 2: Add idempotency guard for service marker writes (AC: 3)
  - [ ] Before writing `service.yaml`: check if file already exists with identical content; skip write if content matches
  - [ ] Before writing `service-constitution.md`: same existence check
  - [ ] Before calling parent-domain helpers (ADR-3): check if domain marker already exists; delegate to `create-domain` which already has this guard
- [ ] Task 3: Add idempotency test (AC: 5)
  - [ ] In `tests/test_create_service.py`: write a test that runs `create-service --execute-governance-git`, leaves files in place, runs again; asserts no duplicate artifacts and exit code 0
- [ ] Task 4: Run NS-1 governance git test (AC: 4)

## Dev Notes

- **Existing pattern:** `create-domain` already has a governance git helper — reuse exactly the same code path. Do NOT re-implement git automation inside `create-service`.
- **Idempotency strategy:** file-level existence + content check before each write; git `add` is idempotent by design; commit may produce "nothing to commit" on retry — handle that gracefully (treat as success, return last commit SHA)
- When `--execute-governance-git` is not passed, `git_commands` and `governance_git_commands` must contain human-readable command text for the user to run manually
- **Clean separation:** governance git commands target the governance repo; workspace scaffold commands (scaffold roots, context file) target the workspace and are in `workspace_git_commands` / `remaining_git_commands`

### Project Structure Notes

- Governance git helper location: look for `execute_governance_git` or equivalent in the `create-domain` handler section
- Error handling: if the governance git helper throws, surface a clear error JSON with `status: fail`; do not partially-commit state

### References

- [tech-plan.md § Script CLI Contract — governance_commit_sha](../tech-plan.md)
- [sprint-plan.md § NS-7 idempotency requirement](../sprint-plan.md)
- [finalizeplan-review.md § M3 and M4 responses](../finalizeplan-review.md)
- [stories.md § NS-7](../stories.md)

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List
