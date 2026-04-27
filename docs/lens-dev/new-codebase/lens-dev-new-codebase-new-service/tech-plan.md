---
feature: lens-dev-new-codebase-new-service
doc_type: tech-plan
status: draft
goal: "Implement create-service parity in bmad-lens-init-feature without changing lifecycle or feature schemas"
key_decisions:
  - Reuse the shared init-feature skill boundary rather than introducing a dedicated new-service skill.
  - Add create-service as a script subcommand with observable JSON output parity to old service creation behavior.
  - Extend context writing to support domain-only and domain-plus-service activation through one helper.
  - Validate parity through focused tests before broad rewrite validation.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T14:05:26Z
---

# Technical Plan - New Service Command

## Technical Summary

Implement `new-service` inside the new-codebase `bmad-lens-init-feature` module as a clean-room extension of the current `new-domain` implementation. The technical work adds a release prompt, a documented skill intent flow, a `create-service` script subcommand, service marker and constitution builders, context activation for domain-plus-service, command discovery metadata, and focused regression tests. The design preserves the old observable boundary: service initialization writes governance container state and optional scaffolds, but never starts a feature lifecycle.

## Architecture Overview

The command remains part of the shared init-feature family:

```text
published prompt stub: .github/prompts/lens-new-service.prompt.md
  -> release prompt: lens.core/_bmad/lens-work/prompts/lens-new-service.prompt.md
  -> skill: lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md
  -> script: lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py create-service
  -> outputs: governance markers, constitutions, optional scaffolds, personal context
```

The new-codebase source currently has the `new-domain` release prompt and a `create-domain` implementation. `create-service` should be added beside that implementation, using shared helpers for safe ID validation, atomic YAML writes, git command planning, dry-run behavior, and governance git execution. Service-specific helpers should be small and explicit: path resolvers, service marker factory, service constitution factory, and context writer support for a service value.

## Design Decisions (ADRs)

### ADR 1 - Keep `new-service` in `bmad-lens-init-feature`

**Decision:** Implement `new-service` as a second intent flow in `bmad-lens-init-feature`, not as a new skill.

**Rationale:** Baseline planning identifies `new-domain`, `new-service`, and `new-feature` as one shared container initialization family. Keeping them together preserves the existing command model and avoids extra public surface area.

**Alternatives Rejected:**

- Dedicated `bmad-lens-new-service` skill: rejected because it fragments the retained command family and duplicates configuration resolution.
- Fold service creation into `new-domain`: rejected because service creation is a separate user job and must remain independently invokable.

### ADR 2 - Preserve Service-as-Container Boundary

**Decision:** `create-service` may create domain/service markers, constitutions, configured scaffolds, context, and governance commits, but it must not create feature lifecycle artifacts.

**Rationale:** The old command intentionally initialized a service container, not a feature. Creating feature YAML, summaries, branches, or feature-index entries would blur governance scope and break user expectations.

**Alternatives Rejected:**

- Auto-create a starter feature after service creation: rejected because `new-feature` owns feature identity, track selection, and branch topology.
- Register services in `feature-index.yaml`: rejected because that registry is for features, not containers.

### ADR 3 - Auto-Establish Missing Parent Domain Container

**Decision:** If the parent domain marker or domain constitution is missing, `create-service` should create the missing parent container artifacts as part of one governance operation.

**Rationale:** The old prompt explicitly allowed service creation to create the parent `domain.yaml` and domain constitution when absent. This keeps setup progressive and avoids forcing users to repair partial governance hierarchies manually.

**Alternatives Rejected:**

- Hard fail when the domain is absent: rejected because it is stricter than the observed old command behavior.
- Create only the service and omit domain artifacts: rejected because it produces an incomplete hierarchy for constitution resolution.

### ADR 4 - Extend Context Writer Instead of Adding a Parallel Helper

**Decision:** Refactor the current context writer to accept `service: str | None` and `source`, preserving domain-only behavior for `new-domain` and enabling domain-plus-service behavior for `new-service`.

**Rationale:** The context schema is shared: `domain`, `service`, `updated_at`, `updated_by`. One helper reduces drift and makes context regression tests clearer.

**Alternatives Rejected:**

- Separate `write_service_context_yaml`: rejected because it duplicates schema construction.
- Store service context somewhere other than `.lens/personal/context.yaml`: rejected because downstream non-feature-branch commands already read that location.

### ADR 5 - Test Observable Parity Rather Than Source Parity

**Decision:** Validate the clean-room implementation by asserting output files, JSON fields, git command fields, and no-write dry-run behavior.

**Rationale:** The user explicitly requires a clean-room scenario. Tests should prove user-visible equivalence without requiring source-level duplication.

**Alternatives Rejected:**

- Copy old tests verbatim without adapting to the new codebase shape: rejected because it risks importing old assumptions mechanically.
- Only run broad module tests: rejected because missing `create-service` is a focused command parity gap that needs direct coverage.

## API Contracts

### Release Prompt Contract

Add `_bmad/lens-work/prompts/lens-new-service.prompt.md` in the new-codebase source. It should load `bmad-lens-init-feature/SKILL.md` and execute the `create-service` intent. The prompt should require config resolution for:

- `governance_repo`
- `target_projects_path` when configured
- `output_folder` when configured
- `personal_output_folder`

It should instruct callers to pass `--execute-governance-git` and to report `governance_commit_sha` on success. Breaking change: false.

### Skill Intent Contract

Extend `bmad-lens-init-feature/SKILL.md` with a `new-service` intent flow:

1. Resolve `governance_repo`, optional `target_projects_path`, optional `output_folder`, and required `personal_output_folder`.
2. Resolve or ask for the parent domain when it is not supplied by active context.
3. Ask for the service display name.
4. Derive a safe service slug using the same normalization pattern as `new-domain`.
5. Confirm the slug, with edit/cancel options.
6. Invoke `init-feature-ops.py create-service` with resolved paths and `--execute-governance-git`.
7. Report the returned governance commit SHA or remaining workspace scaffold commands.

Breaking change: false.

### Script CLI Contract

Add this subcommand:

```bash
uv run _bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py create-service \
  --governance-repo "{governance_repo}" \
  --domain "{domain}" \
  --service "{service}" \
  --name "{display_name}" \
  --username "{user_name}" \
  --personal-folder "{personal_output_folder}" \
  [--target-projects-root "{target_projects_path}"] \
  [--docs-root "{output_folder}"] \
  [--execute-governance-git] \
  [--dry-run]
```

Expected success payload fields:

- `status: pass`
- `scope: service`
- `dry_run`
- `path`
- `constitution_path`
- `created_marker_paths`
- `created_constitution_paths`
- `created_domain_marker`
- `created_domain_constitution`
- `target_projects_path`
- `docs_path`
- `context_path`
- `governance_git_executed`
- `governance_commit_sha`
- `git_commands`
- `governance_git_commands`
- `workspace_git_commands`
- `remaining_git_commands`

Breaking change: false.

## Data Model Changes

No lifecycle schema, feature schema, feature-index schema, or repo-inventory schema changes are required.

The command writes existing container data shapes:

```yaml
kind: service
id: {domain}-{service}
name: {display_name}
domain: {domain}
service: {service}
status: active
owner: {username}
created: {timestamp}
updated: {timestamp}
```

The command also writes or refreshes local personal context in the existing schema:

```yaml
domain: {domain}
service: {service}
updated_at: {timestamp}
updated_by: new-service
```

## Dependencies

- `pyyaml` for YAML writes, already used by the script.
- Git CLI for `--execute-governance-git`, following the existing `create-domain` path.
- Existing helper concepts in the new script: safe ID validation, atomic YAML write, governance git preflight, git command text generation, scaffold command generation, current head SHA resolution.
- Existing Lens config keys from module defaults: `governance_repo_path`, `target_projects_path`, `output_folder`, and `personal_output_folder`.

## Rollout Strategy

Implement behind the retained command surface directly; no feature flag is needed because the command currently does not function in the new codebase. The rollout should be staged as:

1. Add tests that describe the expected service behavior and currently fail.
2. Implement script helpers and the `create-service` parser route.
3. Extend skill and prompt metadata.
4. Run focused service tests.
5. Run the full init-feature test file.
6. Run command-surface validation that confirms `new-service` appears alongside the other retained commands.

Rollback plan: revert the `new-service` implementation files and metadata as one planning-controlled change. Because the feature does not alter schemas or existing domain behavior beyond a shared context helper, rollback should not require migration.

## Testing Strategy

Focused tests should live beside the new-codebase init-feature script tests and cover:

- invalid domain and service IDs return failures
- dry-run returns planned paths and commands without writing markers, scaffolds, or context
- basic service creation writes service marker and service constitution
- missing parent domain marker and constitution are created when needed
- duplicate service fails before overwriting existing service data
- TargetProjects scaffold creates `{domain}/{service}/.gitkeep`
- docs scaffold creates `{domain}/{service}/.gitkeep`
- personal context stores domain, service, timestamp, and `updated_by: new-service`
- `--execute-governance-git` pulls, commits, pushes, and returns `governance_commit_sha`
- dirty governance repo fails before writing when auto git is requested
- service creation does not create `feature.yaml`, `summary.md`, feature-index entries, or control-repo branches

Likely verification commands from the new-codebase source root:

```bash
uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-init-feature-ops.py -k create_service -q
uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-init-feature-ops.py -q
```

## Observability

The command is CLI-observable through its JSON result. The result should make partial completion obvious by separating governance git commands from workspace scaffold commands and by returning `remaining_git_commands` after auto governance git succeeds. Failure payloads should include the same `status: fail` and actionable `error` pattern used by `create-domain`.

No runtime telemetry is required for this planning scope. The important operational signal is deterministic command output plus focused regression coverage.

## Open Questions

None. The parity target is bounded to observable `/new-service` behavior from the old codebase and the retained-command requirements in the baseline rewrite plan.

## Clean-Room Implementation Notes

The implementation should be authored from the behavioral contract above, not copied from old files. It is acceptable to inspect old behavior and tests as evidence of expected outputs, but source code, comments, and prose should be newly written in the new-codebase style. Parity is achieved when the new command produces equivalent observable artifacts and test outcomes, not when it shares text with the old implementation.