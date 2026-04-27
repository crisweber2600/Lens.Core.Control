# Story NS-5: Implement `create-service` parser route

Status: ready-for-dev

## Story

As a Lens agent,
I want the `init-feature-ops.py create-service` subcommand to accept all specified arguments,
so that I can invoke it with the correct governance, domain, service, and optional flags.

## Acceptance Criteria

1. CLI accepts required args: `--governance-repo`, `--domain`, `--service`, `--name`, `--username`, `--personal-folder`
2. CLI accepts optional args: `--target-projects-root`, `--docs-root`
3. CLI accepts optional flags: `--execute-governance-git`, `--dry-run`
4. `--dry-run` and `--execute-governance-git` are mutually exclusive: passing both must return a clear rejected-arguments error without writing any files (NS-1 test 8 turns green)
5. Success JSON payload includes all fields from `tech-plan.md` API Contracts section:
   - `status`, `scope`, `dry_run`, `path`, `constitution_path`, `created_marker_paths`, `created_constitution_paths`
   - `created_domain_marker`, `created_domain_constitution`, `target_projects_path`, `docs_path`
   - `context_path`, `governance_git_executed`, `governance_commit_sha`, `git_commands`
   - `governance_git_commands`, `workspace_git_commands`, `remaining_git_commands`
6. NS-1 success and dry-run contract tests turn green
7. NS-1 mutual-exclusion test turns green

## Tasks / Subtasks

- [ ] Task 1: Register `create-service` subcommand in the argparse routing (AC: 1–3)
  - [ ] Add `create-service` as a subcommand beside `create-domain`
  - [ ] Register all required and optional arguments
  - [ ] Wire `--dry-run` / `--execute-governance-git` mutual exclusion
- [ ] Task 2: Implement command handler `handle_create_service(args)` (AC: 4–5)
  - [ ] Validate args (slug safety check, repo paths exist)
  - [ ] Call NS-4 helpers for service marker and constitution
  - [ ] ADR-3: if parent domain absent, call `create-domain` helpers (not a new implementation)
  - [ ] Write service marker and constitution files (atomic write)
  - [ ] Write context file (NS-6 extended helper)
  - [ ] Build JSON output with all required fields; include remaining git commands when `--execute-governance-git` not passed
  - [ ] Print JSON output to stdout
- [ ] Task 3: Run NS-1 success and dry-run tests (AC: 6–7)

## Dev Notes

- Follow the `create-domain` handler as the reference pattern for the parser route and handler structure
- `scope: service` in output (vs `scope: domain` for `create-domain`)
- `--dry-run` must produce no filesystem side effects; all file writes must be gated by `not args.dry_run`
- JSON output is the contract — do not add print statements or non-JSON lines to stdout; log to stderr only
- Context write is deferred to NS-6 but the placeholder call should be wired here so NS-5 tests can run against a no-op context helper

### Project Structure Notes

- Handler function: add in the same section as `handle_create_domain`
- Argparse subcommand: add in the `subparsers.add_parser(...)` block
- Import: no new dependencies — reuse `pathlib`, `json`, `datetime`, `subprocess` already imported

### References

- [tech-plan.md § Script CLI Contract](../tech-plan.md)
- [tech-plan.md § ADR-3](../tech-plan.md)
- [stories.md § NS-5](../stories.md)

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List
