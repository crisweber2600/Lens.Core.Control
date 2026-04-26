---
feature: lens-dev-new-codebase-new-domain
doc_type: stories
status: draft
goal: "Complete story list for the new-domain command reimplementation"
depends_on:
  - lens-dev-new-codebase-baseline
blocks:
  - lens-dev-new-codebase-new-service
  - lens-dev-new-codebase-new-feature
updated_at: 2026-04-26T00:00:00Z
---

# Stories — new-domain Command (lens-dev-new-codebase-new-domain)

**Author:** Lens FinalizePlan Bundle  
**Date:** 2026-04-26

---

## Epic 1 — Script Implementation

### Story 1.1 — Canonicalize SAFE_ID_PATTERN constant

**Type:** Implementation  
**Estimate:** S (2h)  
**Priority:** P0 (unblocks all other stories)

#### Context

The old-codebase `init-feature-ops.py` defines `SAFE_ID_PATTERN` as a module constant. The business plan and tech plan document conflicting patterns (`^[a-z0-9][a-z0-9-]{0,63}$` vs `^[a-z0-9][a-z0-9._-]{0,63}$`). Before implementation begins the exact value must be read from the old-codebase source and embedded as an authoritative constant.

**Old-codebase source location:** `TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py`

#### Acceptance Criteria

- [ ] `SAFE_ID_PATTERN` is declared as a module-level constant in the new `init-feature-ops.py`
- [ ] Pattern value is verified character-for-character against old-codebase source
- [ ] Constant has an inline comment: `# source: old-codebase init-feature-ops.py SAFE_ID_PATTERN`
- [ ] `validate_safe_id(domain)` function raises `ValueError` for invalid slugs and returns `None` for valid ones
- [ ] `test_validate_safe_id_valid` passes with at minimum: `"lens-dev"`, `"platform"`, `"my-domain-1"`
- [ ] `test_validate_safe_id_invalid` passes with at minimum: uppercase, leading hyphen, trailing hyphen, whitespace, path separator

#### Dev Notes

- Read the old-codebase constant first — do not guess or derive. File: `TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py`
- The resolved pattern becomes the shared contract for `new-service` and `new-feature` — document it in a `SHARED_CONTRACTS.md` stub in `docs/lens-dev/new-codebase/` so downstream features can reference it

---

### Story 1.2 — Implement create-domain core flow

**Type:** Implementation  
**Estimate:** M (4h)  
**Priority:** P0  
**Depends on:** Story 1.1

#### Context

Implements the main `create-domain` subcommand logic in `init-feature-ops.py`. The critical correctness constraint (from finalizeplan review Winston-P finding) is that the duplicate check must run AFTER `sync_governance_main` pull, not before.

#### Acceptance Criteria

- [ ] Script accepts all required and optional args: `--governance-repo`, `--domain`, `--name`, `--username`, `--target-projects-root`, `--docs-root`, `--personal-folder`, `--execute-governance-git`, `--dry-run`
- [ ] Operation order when `--execute-governance-git`: `validate_safe_id` → `sync_governance_main` → `duplicate_check` → writes → git sequence
- [ ] Operation order without `--execute-governance-git`: `validate_safe_id` → `duplicate_check` → writes (no pull/push)
- [ ] `domain.yaml` written at `{governance_repo}/features/{domain}/domain.yaml` with frozen schema
- [ ] `constitution.md` written at `{governance_repo}/constitutions/{domain}/constitution.md` with frozen frontmatter + body
- [ ] `.gitkeep` scaffold created at `{target_projects_root}/{domain}/.gitkeep` when flag provided
- [ ] `.gitkeep` scaffold created at `{docs_root}/{domain}/.gitkeep` when flag provided
- [ ] `context.yaml` written with `domain`, `service: null`, `updated_at`, `updated_by: new-domain` when `--personal-folder` provided
- [ ] Returns JSON to stdout (see tech-plan Return JSON schema)
- [ ] Exit code 0 on success, non-zero on any failure
- [ ] Returns `status: fail` immediately when `domain.yaml` already exists
- [ ] Returns `status: fail` before writing if `sync_governance_main` fails

#### Dev Notes

**CRITICAL — operation order:** See finalizeplan-review.md Winston-P finding. Duplicate check must run AFTER pull when `--execute-governance-git` is set. The old-codebase architecture diagram shows the wrong order — use the corrected order from finalizeplan-review.md Implementation Guidance section.

**domain.yaml frozen schema:**
```yaml
kind: domain
id: {domain}
name: {name}        # defaults to domain slug when --name omitted
domain: {domain}
status: active
owner: {username}
created: {ISO-8601}
updated: {ISO-8601}
```

**context.yaml frozen schema:**
```yaml
domain: {domain}
service: null
updated_at: {ISO-8601}
updated_by: new-domain
```

---

### Story 1.3 — Implement dry-run mode

**Type:** Implementation  
**Estimate:** S (1h)  
**Priority:** P1  
**Depends on:** Story 1.2

#### Acceptance Criteria

- [ ] `--dry-run` flag causes `Runner` to skip all file writes and git commands
- [ ] All planned paths are computed and included in the output JSON
- [ ] `dry_run: true` appears in the output JSON
- [ ] No files are created on disk
- [ ] No git commands are run
- [ ] `test_create_domain_dry_run` passes: verifies no files exist after dry-run invocation

---

### Story 1.4 — Schema parity tests

**Type:** Test  
**Estimate:** M (3h)  
**Priority:** P0  
**Depends on:** Story 1.2

#### Context

Parity tests verify the clean-room reimplementation produces schema-equivalent artifacts to the old codebase. Constitution body fixture MUST be derived from the spec, not copied from old-codebase source.

#### Acceptance Criteria

- [ ] `test_domain_yaml_schema_parity`: all fields present, correct types, field order matches expectation
- [ ] `test_constitution_content_parity`: output matches the verbatim constitution body template below (spec-derived, not copy-pasted from old codebase)
- [ ] `test_context_yaml_schema_parity`: output contains exactly `domain`, `service`, `updated_at`, `updated_by` and no other fields
- [ ] `test_create_domain_name_defaults_to_slug`: when `--name` is omitted, `domain.yaml.name` equals the domain slug value

#### Constitution body template (authoritative test fixture spec)

The `make_domain_constitution_md()` function must produce a body matching this template with `{domain}` as the only interpolation variable:

```markdown
---
permitted_tracks: [quickplan, full, hotfix, tech-change]
required_artifacts:
  planning:
    - business-plan
  dev:
    - stories
gate_mode: informational
sensing_gate_mode: informational
additional_review_participants: []
enforce_stories: true
enforce_review: true
---

# {domain} Domain Constitution

## Scope

This constitution governs all features under the `{domain}` domain.

## Tracks

All tracks listed in `permitted_tracks` are available for features in this domain.

## Artifacts

Planning artifacts and development artifacts listed in `required_artifacts` are required for features in this domain.

## Review

Reviews are `{gate_mode}`. Sensing is `{sensing_gate_mode}`.

## Notes

This is an auto-generated default constitution. Edit this file to add domain-specific governance rules.
```

The test fixture must be defined as a Python string constant inline in the test file, not loaded from a file path. This is the spec-derived reproduction requirement.

---

### Story 1.5 — Integration test suite

**Type:** Test  
**Estimate:** M (3h)  
**Priority:** P0  
**Depends on:** Story 1.2, Story 1.3

#### Acceptance Criteria

- [ ] All tests use `pytest tmp_path` fixture for governance repo root — no test references a real governance repo path
- [ ] `test_create_domain_basic`: verifies `domain.yaml`, `constitution.md`, and `context.yaml` are created at correct paths
- [ ] `test_create_domain_with_scaffolds`: verifies both `.gitkeep` files created when both scaffold flags provided
- [ ] `test_create_domain_duplicate_fails`: pre-creates `domain.yaml`, verifies `status: fail` and no side effects
- [ ] `test_create_domain_execute_governance_git`: uses a real temp git repo; verifies `governance_commit_sha` present
- [ ] `test_create_domain_governance_git_dirty_repo`: marks temp governance clone dirty; verifies `status: fail` before any file write
- [ ] All tests run in isolation — no shared state between tests
- [ ] Tests run in any order without failures caused by ordering

---

## Epic 2 — Prompt Chain

### Story 2.1 — Control repo stub: lens-new-domain.prompt.md

**Type:** Implementation  
**Estimate:** XS (30m)  
**Priority:** P1  
**Depends on:** Epic 1 complete

#### Acceptance Criteria

- [ ] File exists at `.github/prompts/lens-new-domain.prompt.md`
- [ ] Stub runs `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` as first action
- [ ] On exit 0, loads `lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md`
- [ ] File follows the same structure as at least two other existing command stubs in `.github/prompts/`
- [ ] No domain creation logic in the stub

---

### Story 2.2 — Release prompt: lens-new-domain.prompt.md

**Type:** Implementation  
**Estimate:** S (1h)  
**Priority:** P1  
**Depends on:** Story 2.1

#### Acceptance Criteria

- [ ] File exists at `lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md`
- [ ] Prompt loads `bmad-lens-init-feature` SKILL.md
- [ ] Prompt declares `intent: create-domain`
- [ ] Prompt resolves `governance_repo`, `target_projects_path`, `personal_output_folder` from config
- [ ] No domain creation logic in the release prompt itself

---

### Story 2.3 — Skill prompt UX: slug derivation + confirmation

**Type:** Implementation  
**Estimate:** S (2h)  
**Priority:** P1  
**Depends on:** Story 2.2, Story 1.1 (for `SAFE_ID_PATTERN` used in derivation)

#### Context

The skill prompt interaction is the user-facing surface of `new-domain`. The finalizeplan review John-P finding requires an explicit slug confirmation step before any write occurs.

#### Acceptance Criteria

- [ ] Skill prompt asks for display name only as the minimum required input
- [ ] Slug is derived from the display name (lowercase, spaces → hyphens, special chars removed)
- [ ] Derived slug is validated against `SAFE_ID_PATTERN`; if invalid, user is asked to provide a valid slug
- [ ] Skill presents: "Domain slug will be: `{slug}`. Proceed? [Y/n/edit]" before invoking the script
- [ ] "edit" option allows user to enter an alternative slug (re-validated against `SAFE_ID_PATTERN`)
- [ ] After successful creation with auto-git: `governance_commit_sha` is surfaced
- [ ] After successful creation without auto-git: `remaining_git_commands` are displayed for manual execution
- [ ] On `status: fail` (duplicate or sync failure): error message is surfaced verbatim from the JSON `error` field

---

## Story Priority Order for Sprint

| Sprint | Story | Estimate |
|---|---|---|
| Sprint 1 | 1.1 — SAFE_ID_PATTERN | S |
| Sprint 1 | 1.2 — Core flow | M |
| Sprint 1 | 1.3 — Dry-run | S |
| Sprint 1 | 1.4 — Parity tests | M |
| Sprint 1 | 1.5 — Integration tests | M |
| Sprint 2 | 2.1 — Stub | XS |
| Sprint 2 | 2.2 — Release prompt | S |
| Sprint 2 | 2.3 — Skill UX | S |
