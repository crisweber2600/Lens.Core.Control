---
feature: lens-dev-new-codebase-new-feature
doc_type: stories
status: approved
goal: "Full story list for new-feature command parity across 4 epics and 4 sprints"
key_decisions:
  - NF-1.3 parity tests must be red-failing before NF-2.1 implementation begins.
  - NF-2.1 parity tests must be green before NF-2.1 is accepted as complete.
  - NF-4.3 fetch-context is non-deferrable; full parity required before feature is done.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T15:30:00Z
---

# Stories — New Feature Command

## Epic 1: Command Surface and Parity Foundation

### Story NF-1.1: Add command prompt surfaces

**Estimate:** M  
**Sprint:** 1  
**Depends on:** —  
**Blocks:** NF-1.2 (context), NF-2.1 (implementation path)

**Description:**  
Create the installed stub prompt and release prompt for the `new-feature` command in the new codebase. The stub runs light preflight on activation; the release prompt loads the `bmad-lens-init-feature` SKILL.md.

**Acceptance Criteria:**
- [ ] `lens.core/.github/prompts/lens-new-feature.prompt.md` exists in the new codebase and runs `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` on activation
- [ ] `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md` exists and loads `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md`
- [ ] Both prompt paths resolve correctly when `lens.core/` is the installed module root (relative, not absolute)
- [ ] Stub gracefully surfaces preflight failure without breaking the shell menu
- [ ] No old-codebase prompt file is copied verbatim into the new codebase

**Dev Notes:**
- Reference: old codebase stubs at `TargetProjects/lens-dev/old-codebase/lens.core.src/.github/prompts/lens-init-feature.prompt.md` and `_bmad/lens-work/prompts/lens-init-feature.prompt.md`
- Note the old codebase uses `lens-init-feature`; new codebase uses `lens-new-feature` — consistent with the retained-command rename contract from `lens-dev-new-codebase-baseline`
- The light-preflight path must stay relative to the workspace root, not to the script location

---

### Story NF-1.2: Expand skill contract for new-feature

**Estimate:** M  
**Sprint:** 1  
**Depends on:** NF-1.1  
**Blocks:** NF-2.1 (spec baseline)

**Description:**  
Update `bmad-lens-init-feature/SKILL.md` to document the full new-feature interaction flow: progressive disclosure (name/domain/service first, then explicit track), `create` script invocation, all output fields, failure behavior, and the `fetch-context` subcommand (as it is now in scope).

**Acceptance Criteria:**
- [ ] SKILL.md documents the progressive disclosure flow: collect name, domain, service; then require explicit track selection; then invoke `create`
- [ ] SKILL.md documents all required `create` output fields: `featureId`, `featureSlug`, `starting_phase`, `recommended_command`, `router_command`, `governance_git_commands`, `control_repo_git_commands`, `control_repo_activation_commands`, `remaining_commands`, `gh_commands`, `planning_pr_created`
- [ ] SKILL.md references `fetch-context` (via `./references/auto-context-pull.md` or inline) and describes when to invoke it: at feature init to load related summaries and dependency docs
- [ ] SKILL.md documents failure behavior: missing track, invalid domain/service/slug, duplicate feature, dirty governance repo
- [ ] SKILL.md documents the auto-context pull process consistent with ADR 6 in tech-plan.md
- [ ] No SKILL.md section promises behavior not implemented in the script contract

**Dev Notes:**
- Old codebase SKILL.md reference: `TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md`
- The `./references/auto-context-pull.md` file already exists in the old codebase; bring it forward or document inline
- Since NF-1.3 test skeletons are written in parallel, coordinate the fetch-context test cases with the contract documented here

---

### Story NF-1.3: Add parity test skeletons

**Estimate:** S  
**Sprint:** 1  
**Depends on:** NF-1.1, NF-1.2  
**Blocks:** NF-2.1, NF-2.2, NF-2.3 (tests must exist before implementation begins)

**Description:**  
Add red-failing parity test skeletons to `test-init-feature-ops.py` in the new codebase covering all required behaviors. Tests must fail red before Sprint 2 begins — they define the parity contract.

**Acceptance Criteria:**
- [ ] Skeleton tests exist and fail red (not skipped) for:
  - `create`: full-track start phase resolves to `preplan`
  - `create`: express-track start phase resolves to `expressplan`
  - `create`: feature/quickplan track resolves to `businessplan`
  - `create`: express track sets `planning_pr_created: false` and `gh_commands: []`
  - `create`: invalid domain slug rejected before writes
  - `create`: invalid service slug rejected before writes
  - `create`: invalid feature slug rejected before writes
  - `create`: duplicate feature ID in index rejected before writes
  - `create`: dry-run returns planned paths and commands, creates no files
  - `fetch-context`: feature not found in index returns `status: fail`
  - `fetch-context`: `--depth summaries` returns related feature `summary.md` paths
  - `fetch-context`: `--depth full` returns feature.yaml + docs paths for related features
  - `fetch-context`: `depends_on` features always return full depth
  - `fetch-context`: missing service name appears in `missing_service_refs`
- [ ] Existing `create-domain` tests from the new codebase remain green (no regressions)
- [ ] A quickplan alias test is included: `quickplan` resolves to the same start phase as `feature` track

**Dev Notes:**
- Focused test command: `uv run --with pytest --with pyyaml pytest _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-init-feature-ops.py -q`
- Run from `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- Test file already exists with create-domain coverage; add new test functions without removing existing ones
- Tests must invoke the real script (subprocess or direct import) — not mock the entire output

---

## Epic 2: Script-Level Feature Creation

### Story NF-2.1: Implement identity and feature data builders

**Estimate:** L  
**Sprint:** 2  
**Depends on:** NF-1.3 (parity test skeletons must be red-failing before this story starts)  
**Blocks:** NF-2.2, NF-2.3, NF-3.1, NF-3.2, NF-3.3

**Description:**  
Implement `cmd_create` and all supporting identity and data builder functions in `init-feature-ops.py`. The implementation must produce governance files (feature.yaml, summary.md, feature-index.yaml entry, domain/service markers) that exactly match the old codebase v4 schema. Parity test skeletons from NF-1.3 must pass green before this story is accepted.

**Acceptance Criteria:**
- [ ] `cmd_create` implemented and registered in `build_parser()` dispatch
- [ ] Feature identity: local slug normalized to lowercase-hyphen; canonical ID is `{normalized-domain}-{normalized-service}-{featureSlug}` — no variation
- [ ] `featureSlug` stored separately from `featureId` in both `feature.yaml` and `feature-index.yaml` entry
- [ ] `feature.yaml` written with all v4 schema fields (see tech-plan.md Data Model section)
- [ ] `summary.md` stub created at `features/{domain}/{service}/{featureId}/summary.md` in governance
- [ ] `feature-index.yaml` entry updated/created with all required fields: `id`, `featureId`, `featureSlug`, `domain`, `service`, `status`, `owner`, `plan_branch`, `related_features`, `updated_at`, `summary`
- [ ] Domain and service marker files confirmed/created if absent
- [ ] **NF-1.3 parity test skeletons for identity and governance-file creation pass green**
- [ ] No old-codebase implementation file copied

**Dev Notes:**
- Reference: `TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py`
- Key helpers to reuse (not reimplement): safe ID validation, atomic YAML write, `current_head_sha`, `build_container_result_fields`
- These helpers already exist in the new-codebase create-domain implementation; import/reuse them

---

### Story NF-2.2: Implement duplicate and validation gates

**Estimate:** M  
**Sprint:** 2  
**Depends on:** NF-2.1  
**Blocks:** NF-3.1, NF-3.2, NF-3.3

**Description:**  
Implement all pre-write validation and duplicate detection gates in `cmd_create`. Gates must fire before any filesystem write and return a structured `fail` result.

**Acceptance Criteria:**
- [ ] Invalid domain slug (non-lowercase-hyphen characters) rejected before writes with `status: fail` and clear error message
- [ ] Invalid service slug rejected before writes
- [ ] Invalid feature slug rejected before writes
- [ ] Duplicate feature ID in `feature-index.yaml` rejected before writes
- [ ] Missing `--track` argument returns explicit `status: fail` with a clear error (not a Python argparse error)
- [ ] Unknown track value returns `status: fail`
- [ ] Validation order is deterministic: ID validation runs before index read; duplicate check runs before file writes
- [ ] NF-1.3 validation gate test skeletons pass green

---

### Story NF-2.3: Preserve dry-run behavior

**Estimate:** M  
**Sprint:** 2  
**Depends on:** NF-2.1, NF-2.2  
**Blocks:** NF-4.1 (governance git execution depends on non-dry-run path being stable)

**Description:**  
Ensure `--dry-run` returns planned paths and all command groups without creating any files or modifying any YAML. The dry-run output must be machine-readable and structurally equivalent to a successful write-mode run.

**Acceptance Criteria:**
- [ ] `--dry-run` flag accepted by `cmd_create`
- [ ] Dry-run returns `dry_run: true` in output and all planned paths (feature_yaml_path, index path, summary path, marker paths)
- [ ] Dry-run returns all command groups (`governance_git_commands`, `control_repo_git_commands`, `control_repo_activation_commands`, `remaining_commands`, `gh_commands`)
- [ ] No files created, no YAML files modified during dry-run
- [ ] NF-1.3 dry-run test skeleton passes green
- [ ] Dry-run does not invoke external processes (git, gh)

---

## Epic 3: Git and Lifecycle Handoff Parity

### Story NF-3.1: Add lifecycle start-phase routing

**Estimate:** M  
**Sprint:** 3  
**Depends on:** NF-2.1  
**Blocks:** NF-3.2, NF-3.3

**Description:**  
Implement start-phase resolution from `lifecycle.yaml` for all tracks. No hardcoded track-to-command routing. The `quickplan` alias must resolve to the same start phase as `feature` track.

**Acceptance Criteria:**
- [ ] `starting_phase` resolved by reading `tracks.{track}.start_phase` from `lifecycle.yaml`
- [ ] `recommended_command` is `/{starting_phase}`
- [ ] `router_command` is `/next`
- [ ] `full` track resolves to `preplan`; `express` to `expressplan`; `feature`/`quickplan` to `businessplan`
- [ ] Unknown track returns `status: fail` (not a silent fallback)
- [ ] `quickplan` alias test from NF-1.3 passes green
- [ ] Track resolution does not hardcode any mapping; all routing comes from `lifecycle.yaml`

---

### Story NF-3.2: Generate control-repo and activation commands

**Estimate:** M  
**Sprint:** 3  
**Depends on:** NF-3.1  
**Blocks:** NF-4.1 (NF-3.2 output is prerequisite for governance git command group)

**Description:**  
Implement generation of `control_repo_git_commands` (routing through `bmad-lens-git-orchestration create-feature-branches`) and `control_repo_activation_commands` (routing through `bmad-lens-switch switch`). No raw `git checkout -b` commands are permitted.

**Acceptance Criteria:**
- [ ] `control_repo_git_commands` contains a `git-orchestration-ops.py create-feature-branches` invocation, not raw `git checkout -b`
- [ ] `control_repo_activation_commands` contains a `bmad-lens-switch switch` invocation
- [ ] When `--personal-folder` is provided to `cmd_create`, the activation command includes `--personal-folder`
- [ ] `remaining_git_commands` includes governance commands + control-repo branch commands when governance git was not auto-executed
- [ ] `remaining_commands` includes `remaining_git_commands` + activation commands
- [ ] NF-1.3 control-repo command tests pass green (no raw checkout commands in output)
- [ ] `featureSlug` is included in the returned output to support short target-repo branch names

---

### Story NF-3.3: Implement track-aware PR command behavior

**Estimate:** S  
**Sprint:** 3  
**Depends on:** NF-3.1  
**Blocks:** —

**Description:**  
Implement the track-aware planning PR command logic: non-express tracks populate `gh_commands`; express track leaves it empty and sets `planning_pr_created: false`.

**Acceptance Criteria:**
- [ ] Non-express tracks: `gh_commands` contains a `gh pr create` invocation with correct branch, title, and body args
- [ ] Express track: `gh_commands` is `[]` and `planning_pr_created` is `false`
- [ ] `planning_pr_created` is `true` when `gh_commands` is non-empty
- [ ] NF-1.3 express PR deferral test skeleton passes green
- [ ] No planning PR is auto-created by the script; `gh_commands` is always returned as a command string, not executed

---

## Epic 4: Context and Release-Surface Parity

### Story NF-4.1: Implement governance git execution

**Estimate:** M  
**Sprint:** 4  
**Depends on:** NF-3.2  
**Blocks:** NF-4.3 (governance must be committable before fetch-context integration tests can use it)

**Description:**  
Implement `--execute-governance-git` support in `cmd_create`. The script must preflight the governance repo for cleanliness before writing any file, then commit and push all governance artifacts and return the commit SHA.

**Acceptance Criteria:**
- [ ] `--execute-governance-git` flag accepted by `cmd_create`
- [ ] Governance repo cleanliness checked before any file write; dirty repo returns `status: fail` before writes
- [ ] On success: governance files committed and pushed; `governance_commit_sha` returned as short SHA
- [ ] `governance_git_executed` is `true` only after successful push
- [ ] `remaining_git_commands` excludes governance commands when governance git was auto-executed
- [ ] NF-1.3 governance git test skeletons pass green
- [ ] Dirty-repo test: when governance has uncommitted changes, `--execute-governance-git` fails before creating any files

---

### Story NF-4.2: Align help/manifests if owned here

**Estimate:** S  
**Sprint:** 4  
**Depends on:** NF-4.1  
**Blocks:** —

**Description:**  
Update module help CSV and prompt manifest files with `new-feature` entry, but only if this feature owns the command-surface registration (as opposed to the broader 17-command sweep feature). If the 17-command sweep owns this, this story becomes a no-op and is accepted as skipped with documented rationale.

**Acceptance Criteria:**
- [ ] If owned here: `_bmad/lens-work/module-help.csv` includes a `new-feature` row; `_bmad/lens-work/agents/lens.agent.md` shell menu includes `new-feature`; `_bmad/lens-work/skills/bmad-lens-help/assets/help-topics.yaml` includes `new-feature`
- [ ] If owned by 17-command sweep: story is explicitly accepted as skipped with a one-line rationale recorded in this story file
- [ ] No duplicate command-surface entries (check existing entries before adding)
- [ ] All three surfaces updated in sync (not partial)

**Dev Notes:**
- Codebase convention: adding or removing a command requires updating all three discovery surfaces in sync (module-help.csv, help-topics.yaml, agents/lens.agent.md)

---

### Story NF-4.3: Implement fetch-context with full parity

**Estimate:** L  
**Sprint:** 4  
**Depends on:** NF-4.1  
**Blocks:** —

**Description:**  
Implement `cmd_fetch_context` in `init-feature-ops.py` with full behavioral parity to the old codebase. All input arguments, output fields, and path-resolution logic must match exactly. This story is non-deferrable.

**Acceptance Criteria:**
- [ ] `fetch-context` subcommand registered in `build_parser()` with all arguments: `--governance-repo`, `--feature-id`, `--depth` (summaries|full), `--service-ref` (repeatable), `--service-ref-text` (repeatable)
- [ ] `cmd_fetch_context` implemented with all required output fields: `status`, `related`, `depends_on`, `blocks`, `service_refs`, `detected_service_refs`, `missing_service_refs`, `context_paths`, `service_context_paths`
- [ ] `--depth summaries`: related features (same domain, excluding self) resolve to `{governance_repo}/features/{domain}/{service}/{id}/summary.md`
- [ ] `--depth full`: related features resolve to `feature.yaml` + all docs files under `features/{domain}/{service}/{id}/docs/`
- [ ] `depends_on` and `blocks` features always resolve to full depth regardless of `--depth` flag
- [ ] `--service-ref`: named services → `service_context_paths` (service.yaml + docs + non-excluded child summary.md files)
- [ ] `--service-ref-text`: freeform text scanned by `detect_service_refs_from_texts`; detected names populate `service_context_paths`
- [ ] Missing/unresolvable service name appears in `missing_service_refs` — not a `status: fail`
- [ ] Feature not found in index → `status: fail`
- [ ] Missing or unreadable `feature-index.yaml` → `status: fail`
- [ ] `feature.yaml` not found for target feature → `status: fail`
- [ ] `context_paths` is the deduplicated union of all related + dependency + service paths
- [ ] All NF-1.3 fetch-context test skeletons pass green
- [ ] No old-codebase implementation file copied; helper functions reimplemented clean-room

**Dev Notes:**
- Old codebase implementation reference: `TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py` lines 1476–1591 (`cmd_fetch_context`) and helpers at lines 560–680
- Key helpers to reimplement: `feature_dir_from_entry`, `collect_feature_context_paths`, `collect_service_context_paths`, `collect_doc_files`, `available_service_names`, `detect_service_refs_from_texts`, `normalize_lookup_text`
- `CONTEXT_DOC_SUFFIXES` constant must be preserved exactly
- `AMBIGUOUS_SERVICE_NAMES` constant must be preserved exactly
- `unique_paths` deduplication must preserve stable order
