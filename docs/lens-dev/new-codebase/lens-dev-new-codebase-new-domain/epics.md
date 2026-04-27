---
feature: lens-dev-new-codebase-new-domain
doc_type: epics
status: draft
goal: "Deliver the clean-room reimplementation of the new-domain command with full behavioral parity to the old codebase"
stepsCompleted:
  - requirements-extraction
  - epic-design
  - story-creation
inputDocuments:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-new-domain/business-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-new-domain/tech-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-new-domain/finalizeplan-review.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/product-brief.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/research.md
depends_on:
  - lens-dev-new-codebase-baseline
blocks:
  - lens-dev-new-codebase-new-service
  - lens-dev-new-codebase-new-feature
updated_at: 2026-04-26T00:00:00Z
---

# Epics — new-domain Command (lens-dev-new-codebase-new-domain)

**Author:** Lens FinalizePlan Bundle  
**Date:** 2026-04-26

---

## Requirements Summary

### Functional Requirements (FRs)

| ID | Requirement | Source |
|---|---|---|
| FR-1 | create-domain subcommand validates domain slug against `SAFE_ID_PATTERN` before any I/O | tech-plan §API Contracts |
| FR-2 | create-domain writes `domain.yaml` at `{governance_repo}/features/{domain}/domain.yaml` | tech-plan §Data Model |
| FR-3 | create-domain writes `constitution.md` at `{governance_repo}/constitutions/{domain}/constitution.md` | tech-plan §Data Model |
| FR-4 | When `--target-projects-root` is provided, creates `{target_projects_root}/{domain}/.gitkeep` | business-plan §Success Criteria |
| FR-5 | When `--docs-root` is provided, creates `{docs_root}/{domain}/.gitkeep` | business-plan §Success Criteria |
| FR-6 | Always writes `context.yaml` with `domain`, `service: null`, `updated_by: new-domain` after successful create-domain (resolved folder; `--personal-folder` override) | tech-plan §Data Model |
| FR-7 | When `--execute-governance-git` is set: runs `sync_governance_main` (pull), duplicate check, write, then git add/commit/push | tech-plan ADR-2, ADR-5, finalizeplan-review Winston-P |
| FR-8 | Duplicate detection: if `domain.yaml` already exists, return `status: fail` before writing any file | tech-plan ADR-4 |
| FR-9 | `--dry-run` returns complete planned-operations JSON without writing any file or running any git command | business-plan §Success Criteria |
| FR-10 | Returns JSON to stdout with `status`, `path`, `constitution_path`, `governance_git_executed`, `governance_commit_sha` | tech-plan §API Contracts |
| FR-11 | Prompt stub runs `light-preflight.py`, on exit 0 loads release prompt which loads `bmad-lens-init-feature` SKILL.md | tech-plan §Architecture |
| FR-12 | Skill prompt derives domain slug from display name, shows slug to user, requires confirmation before invoking script | finalizeplan-review John-P |

### Non-Functional Requirements (NFRs)

| ID | Requirement | Source |
|---|---|---|
| NFR-1 | All output schemas (domain.yaml, constitution.md, context.yaml) are byte-for-byte schema-equivalent to old codebase | business-plan §Success Criteria |
| NFR-2 | SAFE_ID_PATTERN must be embedded as a module constant with citation to old-codebase source | finalizeplan-review F3/R1 |
| NFR-3 | Integration tests use isolated temp-dir governance repo fixtures; no shared governance clone touched | finalizeplan-review Murat |
| NFR-4 | Parity test for constitution body must use spec-derived fixture, not old-codebase source copy | finalizeplan-review F10/Winston |
| NFR-5 | Operation order when `--execute-governance-git`: validate_safe_id → sync_governance_main → duplicate_check → write → git sequence | finalizeplan-review Winston-P |
| NFR-6 | No schema changes: no field additions, removals, or renames to any output schema | tech-plan §Data Model |

---

## Epic 1 — Script Implementation: create-domain subcommand

**Goal:** Implement `init-feature-ops.py create-domain` with correct operation order, frozen schemas, and full test coverage.

**Scope in:** slug validation, sync_governance_main, duplicate check, domain.yaml write, constitution.md write, scaffold creation, context.yaml write, governance git sequence, dry-run mode, JSON stdout output.

**Scope out:** new-service and new-feature subcommands; prompt stub and release prompt (Epic 2); SAFE_ID_PATTERN shared library (Story 1.1 resolves first).

**Acceptance:** All integration tests pass with isolated temp-dir fixtures. Regression suite from old-codebase `test-init-feature-ops.py` passes without modification to test inputs or expected outputs.

### Story 1.1 — Canonicalize SAFE_ID_PATTERN constant

As a developer implementing `create-domain`, I need the authoritative SAFE_ID_PATTERN constant defined as a module-level constant with a citation comment pointing to its old-codebase origin, so that slug validation is consistent across new-domain, new-service, and new-feature.

**Acceptance Criteria:**
- `SAFE_ID_PATTERN` is declared as a module constant in `init-feature-ops.py`
- The constant includes an inline comment citing the old-codebase source file and line
- The pattern is verified against the old-codebase `SAFE_ID_PATTERN` value character-for-character
- `test_validate_safe_id_valid` and `test_validate_safe_id_invalid` pass

**Notes:** This story resolves F3/R1 (SAFE_ID_PATTERN discrepancy). Must be completed before Story 1.2. The resolved pattern must be documented in a comment block and communicated to `new-service` and `new-feature` as a shared contract.

---

### Story 1.2 — Implement create-domain core flow

As an agent invoking `init-feature-ops.py create-domain`, I need the script to execute in the correct operation order (validate → sync → duplicate-check → write → git), so that governance artifacts are never written to a stale clone and are never duplicated.

**Acceptance Criteria:**
- Operation order is: `validate_safe_id` → `sync_governance_main` (when auto-git) → `duplicate_check` → `write_domain_yaml` → `write_constitution_md` → `write_scaffolds` → `write_context_yaml` → `governance_git_sequence`
- Duplicate check runs AFTER `sync_governance_main` pull, not before
- Returns `status: fail` with error message when `domain.yaml` already exists
- Returns `status: fail` when `sync_governance_main` fails (dirty worktree, merge conflict)
- All written files match frozen schema exactly
- `governance_git_executed: true` and `governance_commit_sha` present in output when auto-git succeeds
- `remaining_git_commands` populated when auto-git not used

**Notes:** Winston-P finding from finalizeplan review — this ordering is the single highest-priority implementation correctness requirement.

---

### Story 1.3 — Implement dry-run mode

As a developer using `create-domain --dry-run`, I need the script to return the complete planned-operations JSON without creating any files or executing any git commands, so I can verify what would happen before committing.

**Acceptance Criteria:**
- `--dry-run` flag returns complete JSON with all planned paths populated
- No files are created when dry-run is active
- No git commands are executed when dry-run is active
- `dry_run: true` is present in the output JSON
- `test_create_domain_dry_run` passes

---

### Story 1.4 — Schema parity tests: domain.yaml, constitution.md, context.yaml

As a reviewer verifying the clean-room constraint, I need automated parity tests that verify the new implementation produces schema-equivalent output to the old codebase on every output file, so the clean-room guarantee has an automated signal beyond peer review.

**Acceptance Criteria:**
- `test_domain_yaml_schema_parity`: output matches old-codebase fixture field-by-field
- `test_constitution_content_parity`: output matches the spec-derived verbatim constitution body template (NOT copied from old-codebase source)
- `test_context_yaml_schema_parity`: output matches expected schema exactly
- Constitution body fixture is defined inline in the test from the tech-plan spec, not loaded from old-codebase files
- `test_create_domain_name_defaults_to_slug` passes (carry-forward R3 — when `--name` omitted, `domain.yaml.name` equals the slug)

**Notes:** F10 requirement — fixture must be spec-derived. R3 requirement — test the `--name` default path explicitly.

---

### Story 1.5 — Integration test suite with isolated fixtures

As a developer maintaining `create-domain`, I need a full integration test suite using isolated temp-dir governance fixtures, so tests are reliable and never leave dirty state in a shared clone.

**Acceptance Criteria:**
- All 6 integration tests from tech-plan testing strategy pass:
  - `test_create_domain_dry_run`
  - `test_create_domain_basic`
  - `test_create_domain_with_scaffolds`
  - `test_create_domain_duplicate_fails`
  - `test_create_domain_execute_governance_git`
  - `test_create_domain_governance_git_dirty_repo`
- All tests use `pytest tmp_path` (or equivalent) for governance repo root
- No test references a real governance repo path
- Tests run in any order without state leakage

---

## Epic 2 — Prompt Chain: stub + release prompt

**Goal:** Deliver the `.github/prompts/lens-new-domain.prompt.md` stub and `lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md` release prompt so the command is invocable from the Lens command surface.

**Scope in:** stub YAML frontmatter, preflight call, release prompt delegation to `bmad-lens-init-feature` SKILL.md, slug derivation + user confirmation UX in the skill prompt.

**Scope out:** Script implementation (Epic 1); any changes to `bmad-lens-init-feature` SKILL.md core content that would affect `new-service` or `new-feature`.

**Acceptance:** Running `/new-domain` invokes `light-preflight.py`, loads the skill on exit 0, and presents the slug confirmation step before calling `create-domain`.

### Story 2.1 — Publish control repo stub: lens-new-domain.prompt.md

As a Lens user, I need the `.github/prompts/lens-new-domain.prompt.md` stub to exist in the control repo so `/new-domain` is a discoverable, invocable command.

**Acceptance Criteria:**
- Stub file exists at `.github/prompts/lens-new-domain.prompt.md`
- Stub runs `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` as its first action
- On exit 0, stub loads `lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md`
- Stub follows the frozen preflight gate contract (identical pattern to other command stubs)
- No behavioral logic lives in the stub itself

---

### Story 2.2 — Release prompt: lens-new-domain.prompt.md (thin redirect)

As a Lens agent loading the release prompt, I need the release prompt to load `bmad-lens-init-feature` SKILL.md and declare `create-domain` as the intent, so the skill knows which subcommand to drive.

**Acceptance Criteria:**
- Release prompt exists at `lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md`
- Prompt loads `bmad-lens-init-feature` SKILL.md
- Prompt declares `intent: create-domain`
- Prompt resolves config (governance_repo, target_projects_path, personal_output_folder) from bmadconfig.yaml
- No domain creation logic lives in the release prompt itself

---

### Story 2.3 — Skill prompt UX: slug derivation + confirmation

As a Lens user running `/new-domain`, I need the skill prompt to derive the domain slug from my display name input, show me the derived slug, and require my confirmation before any file is written, so I am never surprised by an unexpected slug.

**Acceptance Criteria:**
- Skill prompt asks for display name only (minimum ask)
- Slug is derived by lowercasing and replacing spaces/special chars with hyphens, trimmed to `SAFE_ID_PATTERN`
- Derived slug is shown to user before invocation: "Domain slug will be: `{slug}`. Proceed? [Y/n/edit]"
- User can override the derived slug by typing "edit" and entering a manual slug
- Confirmed slug is passed to `create-domain --domain {slug} --name {display_name}`
- Post-success: skill reports `governance_commit_sha` if auto-git succeeded, or lists `remaining_git_commands`

---

## Dependencies Between Epics

```
Story 1.1 (SAFE_ID_PATTERN)
    ↓ must be complete before
Story 1.2 (core flow) + Story 1.3 (dry-run)
    ↓ both complete before
Story 1.4 (parity tests) + Story 1.5 (integration tests)
    
Epic 1 complete before
Story 2.1 (stub) + Story 2.2 (release prompt) — parallel
    ↓ both complete before
Story 2.3 (skill UX) — final integration story

All of Epic 2 complete before:
    → lens-dev-new-codebase-new-service (blocked)
    → lens-dev-new-codebase-new-feature (blocked)
```

**Cross-feature note (Mary-P):** `new-service` and `new-feature` must not implement their slug validators until Story 1.1 (`SAFE_ID_PATTERN`) is merged and the resolved pattern is documented as a shared constant.
