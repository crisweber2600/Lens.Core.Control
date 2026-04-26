---
feature: lens-dev-new-codebase-new-domain
doc_type: tech-plan
status: draft
goal: "Design the clean-room reimplementation of the new-domain command: prompt chain, init-feature-ops.py create-domain subcommand, all output schemas, and regression coverage"
key_decisions:
  - new-domain delegates to bmad-lens-init-feature create-domain; no standalone skill is introduced
  - All output schemas (domain.yaml, constitution.md, context.yaml) are frozen and identical to old codebase
  - --execute-governance-git provides atomic governance-main checkout/pull/add/commit/push
  - Dry-run mode returns complete planned-operations JSON without side effects
  - Duplicate detection (fail-fast on existing domain.yaml) is implemented before any write
  - Governance git preflight (sync_governance_main) runs before any file write when auto-git is requested
  - context.yaml persists active domain with service=null after successful domain creation
  - SAFE_ID_PATTERN validation applied to domain input before any filesystem operation
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks:
  - lens-dev-new-codebase-new-service
  - lens-dev-new-codebase-new-feature
updated_at: 2026-04-26T00:00:00Z
---

# Tech Plan — new-domain Command (lens-dev-new-codebase-new-domain)

**Author:** CrisWeber  
**Date:** 2026-04-26

---

## Technical Summary

The `new-domain` command reimplements the `create-domain` subcommand of `bmad-lens-init-feature` from scratch. The prompt stub runs `light-preflight.py`, then loads the release prompt, which delegates to `bmad-lens-init-feature` SKILL.md. The skill's `Create Domain` capability invokes `init-feature-ops.py create-domain`. The script validates the domain slug, checks for duplicates, optionally runs governance-main sync preflight, writes `domain.yaml` and `constitution.md` atomically, creates optional workspace scaffolds, writes `context.yaml` to the personal folder, and optionally executes the governance git sequence. All output schemas are frozen from the old codebase. No schema changes are introduced anywhere in this feature.

---

## Architecture Overview

### Component Topology

```
User invokes: /new-domain
    │
    ▼
.github/prompts/lens-new-domain.prompt.md          [control repo — published stub]
    │  runs: uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py
    │  on exit 0: loads release prompt
    ▼
lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md  [release — thin redirect]
    │  mode: agent
    │  loads: bmad-lens-init-feature SKILL.md
    │  intent: create-domain subcommand
    ▼
lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md  [Create Domain capability]
    │  validates: domain input, config resolution, governance_repo
    │  invokes: init-feature-ops.py create-domain
    ▼
lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py
    subcommand: create-domain
    │
    ├── validate_safe_id(domain)                   [input sanitization before any I/O]
    ├── sync_governance_main(governance_repo)       [only when --execute-governance-git]
    ├── duplicate check: domain.yaml exists → fail
    ├── atomic_write_yaml(domain.yaml)             [make_domain_yaml()]
    ├── constitution_path.write_text()             [make_domain_constitution_md()]
    ├── tp_gitkeep_path.touch()                    [only when --target-projects-root]
    ├── docs_gitkeep_path.touch()                  [only when --docs-root]
    ├── write_context_yaml(personal_folder, domain, None, "new-domain")  [when --personal-folder]
    └── governance git sequence                    [only when --execute-governance-git]
         (git add → git commit → git push)
    │
    └── returns JSON to stdout
```

### Integration Points

| Component | Role | Direction |
|---|---|---|
| `light-preflight.py` | Prompt-start gate; freezes cache; pulls repos | Inbound (every stub calls this) |
| `bmad-lens-init-feature` SKILL.md | Owning skill for all container creation (domain, service, feature) | Delegate from prompt |
| `init-feature-ops.py` | Python CLI implementation | Script invoked by skill |
| Governance repo (main branch) | Receives `domain.yaml`, `constitution.md` | Write target |
| Control repo (current branch) | Receives docs scaffold `.gitkeep` | Write target |
| TargetProjects tree | Receives TargetProjects scaffold `.gitkeep` | Write target |
| `.lens/personal/context.yaml` | Receives active domain context | Write target; local-only |
| `bmad-lens-constitution` | Provides constitution hierarchy context during skill prompt presentation | Inbound dependency |

### Components Not Affected

- No changes to `feature.yaml` schema or `feature-index.yaml`
- No changes to `dev-session.yaml` or any dev-phase artifacts
- No changes to governance repo branch topology (governance stays on `main`)
- No changes to control repo branch topology (`{featureId}`/`{featureId}-plan` model is unaffected by domain scaffolding)

---

## Design Decisions (ADRs)

### ADR-1: new-domain as alias for bmad-lens-init-feature (not a standalone skill)

**Decision:** `new-domain` does not own a dedicated `bmad-lens-new-domain` skill. Instead, it loads `bmad-lens-init-feature` SKILL.md and invokes the `create-domain` subcommand of `init-feature-ops.py`. This is unchanged from the old codebase architecture.

**Rationale:** All three container-creation commands (`new-domain`, `new-service`, `new-feature`) share the same validation primitives (`validate_safe_id`), the same governance git interface (`sync_governance_main`, `atomic_write_yaml`), and the same output conventions (JSON to stdout with `status`, `path`, `remaining_git_commands`). A standalone skill would duplicate every one of these. Consolidation in `bmad-lens-init-feature` means a fix to slug validation, governance git preflight, or atomic write propagates to all three commands simultaneously.

**Alternatives rejected:**
- `bmad-lens-new-domain` standalone skill: duplicates all shared utilities; two maintenance surfaces for identical logic; inconsistency risk when one skill is updated but not the other.
- Generic `bmad-lens-scaffold` skill combining all three: too broad; would require routing logic that adds complexity without reducing duplication.

---

### ADR-2: --execute-governance-git for atomic governance-main operations

**Decision:** `create-domain` accepts an optional `--execute-governance-git` flag. When set, the script performs `sync_governance_main` (checkout main + pull), writes all governance artifacts, then executes `git add → git commit → git push` atomically before returning. When absent, the script returns `governance_git_commands` as a list of manual commands. The flag is surfaced by the skill prompt to the user.

**Rationale:** Governance repo is always-on-main. Any write to governance artifacts must be immediately committed and pushed so that other agents and tools see consistent state. Providing auto-commit as an opt-in flag enables both human-in-the-loop verification (don't auto-commit, review first) and full automation (auto-commit, CI proceeds). Without the flag, users can inspect what was written before pushing.

**Alternatives rejected:**
- Always auto-commit: removes the ability to dry-run or inspect before committing; breaks the skill prompt's review moment.
- Never auto-commit: relies on users never forgetting to push; leaves governance in a diverged state if users forget; inconsistent with the stated goal of atomic visibility.

---

### ADR-3: context.yaml for domain/service active context

**Decision:** After a successful `create-domain`, write `context.yaml` to `{personal_folder}/context.yaml` with `domain: {domain}`, `service: null`, `updated_at: {ISO timestamp}`, `updated_by: new-domain`. This file is local-only and not git-tracked.

**Rationale:** Immediately after creating a domain, the user's next command is typically `new-service {domain}`. If context.yaml is not written, `new-service` cannot resolve the active domain without requiring the user to re-specify it on the command line. context.yaml provides a simple, persistent, non-git-tracked way to remember the last active domain/service across commands and terminal sessions.

**Alternatives rejected:**
- Environment variables: not persistent across terminal restarts; breaks tool on session close.
- Command arguments: forces repetition of domain on every command; error-prone for users with long domain slugs.
- Git-tracked state file: governance repo should not hold local personal state; creates merge conflicts in multi-user setups.

---

### ADR-4: Fail-fast duplicate detection

**Decision:** Before writing any file, check whether `{governance_repo}/features/{domain}/domain.yaml` exists. If it exists, return `{"status": "fail", "error": "Domain '{domain}' already exists at {path}"}` immediately without creating any artifact.

**Rationale:** Idempotent domain creation would silently overwrite a live constitution with the default template, destroying any customization the user made. Fail-fast is the only safe behavior because the caller can inspect and decide whether to proceed (e.g., by running `new-domain` with a different name).

**Alternatives rejected:**
- Idempotent merge: too complex; risk of overwriting user-authored constitution content with defaults.
- Warn-and-continue: warning without stopping leaves governance in an ambiguous state.

---

### ADR-5: Governance git preflight before any write (when auto-git is requested)

**Decision:** When `--execute-governance-git` is set, call `sync_governance_main(governance_repo)` before writing any governance artifact. If sync fails (dirty worktree, merge conflict), return `status: fail` without writing anything.

**Rationale:** Writing artifacts to a stale governance clone and then pushing will produce a push rejection or silent merge divergence. Running preflight pull first eliminates this by ensuring the local clone is at the remote tip before any write. The fail-fast-before-write contract means governance artifacts are never written to a state that cannot be cleanly pushed.

**Alternatives rejected:**
- Write then sync: writes may succeed but push fails, leaving uncommitted artifacts in the worktree.
- Sync on best-effort: silently proceeding past sync failures would be worse than a visible error.

---

## API Contracts

### Script Interface: `init-feature-ops.py create-domain`

```bash
uv run scripts/init-feature-ops.py create-domain \
  --governance-repo <path>           # required: absolute path to governance repo root
  --domain <slug>                    # required: new domain identifier
  --name <display-name>              # optional: human display name; defaults to domain slug
  --username <username>              # required: owner username; used in domain.yaml and context.yaml
  [--target-projects-root <path>]    # optional: TargetProjects root; triggers .gitkeep scaffold
  [--docs-root <path>]               # optional: docs output root in control repo; triggers .gitkeep scaffold
  [--personal-folder <path>]         # optional: personal context folder; writes context.yaml
  [--execute-governance-git]         # optional: auto-execute governance git sequence
  [--dry-run]                        # optional: return planned operations JSON without writing
```

**Validation rules:**

| Input | Rule | Pattern |
|---|---|---|
| `--domain` | Must match `SAFE_ID_PATTERN` | `^[a-z0-9][a-z0-9._-]{0,63}$` |
| `--governance-repo` | Must exist as a directory | Validated before any operation |
| `--domain` | Must not already have `domain.yaml` at computed path | Duplicate check after validation |

**Return JSON schema (exit code 0 = pass, non-zero = fail):**

```json
{
  "status": "pass" | "fail",
  "scope": "domain",
  "path": "<absolute path to domain.yaml>",
  "constitution_path": "<absolute path to constitution.md>",
  "created_marker_paths": ["<governance-repo-relative path>"],
  "created_constitution_paths": ["<governance-repo-relative path>"],
  "target_projects_path": "<absolute path to scaffold dir or null>",
  "docs_path": "<absolute path to docs scaffold dir or null>",
  "context_path": "<absolute path to context.yaml or null>",
  "governance_git_commands": ["<shell command>"],
  "workspace_git_commands": ["<shell command>"],
  "remaining_git_commands": ["<shell command>"],
  "governance_git_executed": true | false,
  "governance_commit_sha": "<40-char SHA or null>",
  "dry_run": true | false          // always present; true when --dry-run was set
}
```

**Breaking change flag:** `false`. This contract is identical to the old codebase. No field is added, removed, or renamed.

### Prompt Interface

The `lens-new-domain.prompt.md` stub is a read-only redirect that:
1. Runs `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` (frozen gate contract)
2. On exit 0, loads `lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md`
3. The release prompt loads `bmad-lens-init-feature` SKILL.md with `create-domain` intent

The skill prompt then:
1. Resolves config (`governance_repo`, `target_projects_path`, `output_folder`, `personal_output_folder`) from `bmadconfig.yaml` and `config.user.yaml`
2. Asks the user for domain name (minimum ask; all else derived or configured)
3. Derives the domain slug and shows it for confirmation
4. Invokes `create-domain` with resolved args
5. Reports `governance_commit_sha` on auto-git success; surfaces `remaining_git_commands` for manual follow-up
6. Does not create feature branches, feature.yaml, or any lifecycle artifact

---

## Data Model Changes

**No schema changes are introduced in this feature.** All output schemas are frozen from the old codebase. The following tables document the frozen schemas for verification purposes.

### domain.yaml (frozen)

```yaml
kind: domain               # always "domain"
id: {domain}               # normalized slug; equals --domain arg
name: {name}               # display name; defaults to slug when --name omitted
domain: {domain}           # normalized slug; redundant with id; preserved for schema stability
status: active             # always "active" on creation
owner: {username}          # from --username arg
created: {ISO-8601}        # UTC timestamp at creation
updated: {ISO-8601}        # UTC timestamp at creation (same as created on initial write)
```

Written to: `{governance_repo}/features/{domain}/domain.yaml`

### constitution.md frontmatter (frozen)

```yaml
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
```

Written to: `{governance_repo}/constitutions/{domain}/constitution.md`

The body sections (## Scope, ## Tracks, ## Artifacts, ## Review, ## Notes) are populated with domain-name-interpolated default text. The exact template is owned by `make_domain_constitution_md()` in the script and must match the old codebase output character-for-character (verified by content-equality assertion in regression suite).

### context.yaml (frozen)

```yaml
domain: {domain}           # from --domain arg
service: null              # always null when written by new-domain
updated_at: {ISO-8601}     # UTC timestamp at write
updated_by: new-domain     # literal string "new-domain"
```

Written to: `{personal_folder}/context.yaml`  
Local-only; never git-tracked. Overwritten (not merged) on each `new-domain` invocation.

---

## Dependencies

| Dependency | Version | Source |
|---|---|---|
| Python | ≥ 3.10 | Runtime prerequisite (same as old codebase) |
| `pyyaml` | ≥ 6.0 | `atomic_write_yaml` uses yaml.safe_dump |
| `bmad-lens-init-feature` | internal | Owning skill; holds the `create-domain` capability and script |
| `bmad-lens-constitution` | internal | Provides constitution context during skill prompt; not invoked by the script directly |
| `light-preflight.py` | current | Prompt-start gate; frozen exit-code interface |
| Governance repo | on `main` | Write target for domain.yaml and constitution.md |

---

## Rollout Strategy

**Rollout approach:** Standard clean-room rewrite delivery. No feature flags. No phased rollout.

**Schema migration:** None required. All output schemas are v4.0 drop-in. Users on the old codebase can upgrade to the new codebase without running `/upgrade` and without any changes to existing `domain.yaml` or `constitution.md` files.

**Backwards compatibility:** Complete. Existing domain.yaml files written by the old codebase are valid under the new implementation and vice versa. `new-service` and `new-feature` depend on the `domain.yaml` `kind: domain` and path conventions, which are unchanged.

**Rollback plan:** Revert to the previous lens-core release tag via `git revert` or release tag rollback in the lens-core deployment pipeline. The governance artifacts written by the new implementation are schema-identical to the old implementation, so rollback does not require artifact migration.

---

## Testing Strategy

### Unit Tests

| Test | Scope | Assertion |
|---|---|---|
| `test_validate_safe_id_valid` | `validate_safe_id` | Accepts valid domain slugs: `lens-dev`, `platform`, `my-domain-1` |
| `test_validate_safe_id_invalid` | `validate_safe_id` | Rejects: uppercase, leading/trailing hyphens, whitespace, path separators |
| `test_make_domain_yaml` | `make_domain_yaml()` | Output matches expected schema dict field by field |
| `test_make_domain_constitution_md` | `make_domain_constitution_md()` | Output matches old-codebase fixture character-for-character |
| `test_write_context_yaml_new_domain` | `write_context_yaml()` | Writes `service: null`, `updated_by: new-domain`; reads back correctly |

### Integration Tests

| Test | Scope | Assertion |
|---|---|---|
| `test_create_domain_dry_run` | `create-domain --dry-run` | Returns planned-operations JSON; no files written; no git executed |
| `test_create_domain_basic` | `create-domain` | `domain.yaml` and `constitution.md` written at correct paths; `context.yaml` written |
| `test_create_domain_with_scaffolds` | `create-domain --target-projects-root --docs-root` | Both `.gitkeep` files created at correct paths |
| `test_create_domain_duplicate_fails` | `create-domain` (domain already exists) | Returns `status: fail` before writing any file |
| `test_create_domain_execute_governance_git` | `create-domain --execute-governance-git` | Governance git sequence runs; `governance_commit_sha` present in output |
| `test_create_domain_governance_git_dirty_repo` | `create-domain --execute-governance-git` (dirty governance repo) | Returns `status: fail` before writing any file; no governance artifacts created |

### Regression Tests (Parity against old codebase)

All domain-creation test cases from the old-codebase `test-init-feature-ops.py` must pass against the new implementation without modification to the test inputs or expected outputs. This is the primary parity acceptance gate.

Additional rewrite-era regression:

| Test | Assertion |
|---|---|
| `test_domain_yaml_schema_parity` | `domain.yaml` produced by new implementation is byte-for-byte schema-equivalent to old-codebase fixture (same fields, same field order in YAML dump) |
| `test_constitution_content_parity` | `constitution.md` produced by new implementation matches old-codebase fixture content exactly |
| `test_context_yaml_schema_parity` | `context.yaml` produced by `new-domain` matches old-codebase fixture exactly |

---

## Observability

The `new-domain` command is a CLI tool, not a service. Observability is through the script's stdout JSON output.

| Signal | Format | Source |
|---|---|---|
| Success + written paths | JSON: `status: pass`, `path`, `constitution_path`, `docs_path`, `context_path` | Script stdout |
| Governance commit SHA | JSON: `governance_commit_sha` (when auto-git succeeded) | Script stdout |
| Manual git commands | JSON: `remaining_git_commands`, `governance_git_commands` | Script stdout (when auto-git not used) |
| Failure reason | JSON: `status: fail`, `error` (human-readable message) | Script stdout |

The skill prompt surfaces `governance_commit_sha` to the user as confirmation. The `error` field is displayed verbatim by the skill when `status: fail`.

No new metrics, dashboards, or alerts are required. This is a local CLI tool with local filesystem and git side effects only.

---

## Open Questions

None. All technical questions are resolved by the behavioral baseline from the old codebase and the frozen contracts established in `lens-dev-new-codebase-baseline`:

- **Schema version:** v4.0 drop-in confirmed (baseline C1-A resolution). No schema fields may change.
- **context.yaml schema:** Frozen (fields: `domain`, `service`, `updated_at`, `updated_by`). No additions, no removals.
- **Domain slug pattern:** Frozen as `^[a-z0-9][a-z0-9._-]{0,63}$`. Matches old-codebase `SAFE_ID_PATTERN`.
- **Constitution path template:** Frozen as `constitutions/{domain}/constitution.md` under governance repo root.
