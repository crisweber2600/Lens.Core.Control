---
feature: lens-dev-new-codebase-new-feature
doc_type: tech-plan
status: draft
goal: "Implement clean-room new-feature command parity in the new codebase"
key_decisions:
  - Implement observable parity through new-codebase code rather than copying old-codebase files.
  - Extend the shared bmad-lens-init-feature skill and script instead of creating a dedicated bmad-lens-new-feature skill.
  - Use lifecycle.yaml to resolve start phases and track behavior.
  - Delegate control-repo branch creation to git-orchestration and activation to switch.
  - Keep governance auto-publish optional and fail-fast on dirty governance repos.
open_questions:
  - Whether fetch-context is required in this delivery slice for full command parity.
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T14:15:58Z
---

# Tech Plan — New Feature Command

## Technical Summary

Implement `lens-new-feature` in the new codebase as a clean-room parity implementation of the existing Lens feature initializer. The work adds the missing prompt surfaces, expands `bmad-lens-init-feature` beyond its current `new-domain` flow, and implements a script-level `create` operation that writes governance feature metadata and returns the exact follow-up command structure Lens callers expect. The design preserves the old command's observable outputs while allowing the new implementation to be smaller, clearer, and covered by focused parity tests.

## Architecture Overview

The command keeps the existing three-layer Lens command architecture:

```text
.github/prompts/lens-new-feature.prompt.md
  -> _bmad/lens-work/prompts/lens-new-feature.prompt.md
    -> _bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md
      -> _bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py create
        -> governance files on main
        -> returned git-orchestration branch command
        -> returned switch activation command
        -> returned gh planning PR command when applicable
```

The implementation target is `TargetProjects/lens-dev/new-codebase/lens.core.src`. The release payload under `lens.core/` and the old-codebase tree remain read-only behavioral references for this feature.

The shared initializer should expose three conceptual layers:

| Layer | Responsibility |
|---|---|
| Prompt layer | Run light preflight, load release prompt, and never resolve paths against the wrong repository |
| Skill layer | Apply progressive disclosure, require explicit track selection, explain outcomes, and invoke the script |
| Script layer | Validate inputs, create governance artifacts, generate follow-up commands, optionally commit/push governance changes |

## Design Decisions (ADRs)

### ADR 1 — Keep `new-feature` in `bmad-lens-init-feature`

**Decision:** Extend the shared initializer instead of introducing `bmad-lens-new-feature`.

**Rationale:** The baseline command surface maps `new-domain`, `new-service`, and `new-feature` to one shared initialization skill. That keeps domain, service, feature identity, governance paths, and context writes in one place.

**Alternatives Rejected:**

- Create a dedicated skill for `new-feature`: rejected because it duplicates identity and governance helper logic.
- Route `new-feature` through a generic quickplan wrapper: rejected because feature creation is a lifecycle/container operation, not an artifact-authoring phase.

### ADR 2 — Preserve Canonical Feature Identity Exactly

**Decision:** Resolve feature identity as `{normalized-domain}-{normalized-service}-{featureSlug}` and store both `featureId` and `featureSlug`.

**Rationale:** Branch names, governance paths, feature-index entries, docs paths, and target-repo handoffs all depend on this formula. It is the highest-risk compatibility contract in this feature.

**Alternatives Rejected:**

- Use the local slug only: rejected because features with the same slug in different services would collide.
- Allow dots/underscores in feature slugs: rejected because the old behavior accepts those only for domain/service-safe IDs, not feature slugs.

### ADR 3 — Resolve Start Phase from `lifecycle.yaml`

**Decision:** Read `tracks.{track}.start_phase` from lifecycle metadata instead of hardcoding track-to-command routing.

**Rationale:** Lifecycle routing belongs to the lifecycle contract. This also preserves legacy `quickplan` normalization if the new codebase keeps the old compatibility alias.

**Alternatives Rejected:**

- Hardcode `full -> preplan`, `express -> expressplan`, `quickplan -> businessplan`: rejected because it makes lifecycle changes require script edits.
- Default missing tracks silently: rejected because track selection must be explicit and validated.

### ADR 4 — Return Branch Creation Commands, Do Not Hand-Roll Branches

**Decision:** The script returns a `git-orchestration-ops.py create-feature-branches` command for control-repo topology creation.

**Rationale:** Git orchestration resolves the control repo default branch before creating `{featureId}` and `{featureId}-plan`. Replacing it with raw `git checkout -b` commands can create branches from the wrong base.

**Alternatives Rejected:**

- Direct branch creation inside `init-feature-ops.py`: rejected because it crosses ownership boundaries and duplicates git orchestration.
- Return raw git checkout commands: rejected because old parity tests explicitly guard against that regression.

### ADR 5 — Optional Governance Git Execution Remains Fail-Fast

**Decision:** Support `--execute-governance-git`, but run governance preflight before writing files and stop on dirty repos.

**Rationale:** The command must be safe in automated flows while preserving the governance-main-only contract. Dirty governance state must be surfaced before partial writes.

**Alternatives Rejected:**

- Always auto-commit governance artifacts: rejected because dry-run/manual modes remain useful and tested.
- Write files first and then check git cleanliness: rejected because that can leave a dirty governance repo after a preventable failure.

## API Contracts

### Prompt Contract

The installed stub runs:

```bash
uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py
```

On success it loads:

```text
lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md
```

The release prompt loads:

```text
lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md
```

### Script Contract

The implementation must support:

```bash
uv run _bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py create \
  --governance-repo {governance_repo} \
  --control-repo {control_repo} \
  --feature-id {feature_slug_or_canonical_id} \
  --domain {domain} \
  --service {service} \
  --name "{feature_name}" \
  --track {track} \
  --username {username} \
  [--personal-folder {personal_output_folder}] \
  [--execute-governance-git] \
  [--dry-run]
```

Required success payload fields:

| Field | Requirement |
|---|---|
| `status` | `pass` on success, `fail` on validation or execution failure |
| `featureId` | Canonical ID with normalized domain/service prefix |
| `featureSlug` | Short local feature slug |
| `starting_phase` | Resolved from lifecycle track start phase |
| `recommended_command` | `/{starting_phase}` |
| `router_command` | `/next` |
| `feature_yaml_path` | Absolute or caller-usable path to created/planned feature.yaml |
| `index_updated` | `true` when feature-index is created or updated |
| `summary_path` | Path to created/planned summary stub |
| `container_markers` | Governance-relative parent marker paths created or confirmed |
| `governance_git_commands` | checkout/pull/add/commit/push command list for governance artifacts |
| `control_repo_git_commands` | branch creation command list for the control repo |
| `control_repo_activation_commands` | switch command list that activates the plan branch and writes personal context when configured |
| `remaining_git_commands` | Full command list unless governance git already executed; then only control-repo follow-up |
| `remaining_commands` | `remaining_git_commands` plus activation commands |
| `gh_commands` | Planning PR command list for non-express tracks; empty for express |
| `planning_pr_created` | `true` when `gh_commands` is non-empty |
| `governance_git_executed` | `true` only after successful auto-publish |
| `governance_commit_sha` | Short SHA when governance auto-publish succeeds |

Required failure behavior:

- Invalid domain/service IDs fail before file writes.
- Invalid feature slug or canonical ID fails before file writes.
- Missing `--track` fails with a clear explicit-track error.
- Duplicate feature ID in `feature-index.yaml` fails before file writes.
- Dirty governance repo with `--execute-governance-git` fails before file writes.
- Dry-run returns planned paths and commands but creates no files.

## Data Model Changes

No schema changes are introduced. New features created by this command must use the existing v4 feature schema:

```yaml
name: {feature_name}
description: ""
featureId: {domain-service-featureSlug}
featureSlug: {featureSlug}
domain: {domain}
service: {service}
phase: {starting_phase}
track: {track}
milestones:
  businessplan: null
  techplan: null
  finalizeplan: null
  dev-ready: null
  dev-complete: null
team:
  - username: {username}
    role: lead
dependencies:
  depends_on: []
  depended_by: []
target_repos: []
links:
  retrospective: null
  issues: []
  pull_request: null
priority: medium
created: {timestamp}
updated: {timestamp}
phase_transitions:
  - phase: {starting_phase}
    timestamp: {timestamp}
    user: {username}
docs:
  path: docs/{domain}/{service}/{featureId}
  governance_docs_path: features/{domain}/{service}/{featureId}/docs
```

The feature-index entry must include `id`, `featureId`, `featureSlug`, `domain`, `service`, `status`, `owner`, `plan_branch`, `related_features`, `updated_at`, and `summary`.

## Dependencies

| Dependency | Use |
|---|---|
| `lifecycle.yaml` | Track validation and start phase resolution |
| `feature-index.yaml` | Duplicate detection and registry update |
| `bmad-lens-git-orchestration` | Control-repo feature and plan branch creation |
| `bmad-lens-switch` | Activation of the new feature context after branch creation |
| `gh` CLI | Planning PR creation for non-express tracks |
| `pyyaml` | Safe YAML reads/writes |
| Existing create-domain helpers | Shared safe ID validation, atomic YAML writes, governance git helpers |

## Rollout Strategy

1. Add tests first for the observable old behavior in the new-codebase test suite.
2. Add prompt and skill contract files for the `new-feature` surface.
3. Implement the script-level `create` path with dry-run support before enabling write mode.
4. Enable governance git execution after the non-git write path is covered.
5. Run the focused initializer test suite from the new-codebase root.
6. Update help/manifests only after the command is executable.

Rollback is straightforward before governance publication: revert the new-codebase prompt, skill, script, and test changes. After publication, rollback requires removing the command from help/manifests and restoring the previous initializer script while preserving any governance features already created by users.

## Testing Strategy

Focused verification command:

```bash
cd TargetProjects/lens-dev/new-codebase/lens.core.src
uv run --with pytest --with pyyaml pytest _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-init-feature-ops.py -q
```

Minimum test cases:

| Area | Tests |
|---|---|
| Identity | local slug canonicalizes; canonical ID is preserved; invalid slug/domain/service rejected |
| Track | full starts in preplan; feature/quickplan starts in businessplan; express starts in expressplan |
| Governance files | feature.yaml, feature-index.yaml, summary.md, domain marker, service marker created |
| Duplicate detection | existing feature ID fails before writes |
| Dry run | planned paths and commands returned; no files created |
| Governance git | auto-publish succeeds on clean repo and returns SHA; dirty repo fails before writes |
| Control repo commands | branch creation delegated to git-orchestration; activation delegated to switch; no raw checkout commands |
| Express behavior | `gh_commands` empty and `planning_pr_created` false |
| Personal folder | activation command includes `--personal-folder` when configured |
| Regression safety | existing create-domain tests remain green |

## Observability

The command is primarily a CLI/script workflow, so observability comes from structured JSON results and git evidence rather than runtime telemetry. Every path should return a machine-readable `status`, clear `error` text on failure, and command groups that make the next manual or automated action explicit. Governance auto-publish should report `governance_commit_sha`, and callers should relay that SHA to users.

## Open Questions

1. Should `fetch-context` be implemented now so downstream planning can immediately load related summaries and dependency docs in the new codebase?
2. Should the new implementation keep `quickplan` as a legacy track alias to `feature` even if the rewritten public track names emphasize `full` and `express`?
3. Should command-surface updates for module help and prompt manifests be included in this feature's implementation branch or in a broader retained-command sweep?
