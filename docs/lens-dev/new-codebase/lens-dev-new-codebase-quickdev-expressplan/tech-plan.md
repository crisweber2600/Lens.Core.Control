---
feature: lens-dev-new-codebase-quickdev-expressplan
doc_type: tech-plan
status: approved
goal: "Define the technical design for a public lens-quickdev conductor that wraps BMAD quick development with Lens context and evidence docs."
key_decisions:
  - Implement lens-quickdev as a thin conductor around the existing Lens BMAD quick-dev registration.
  - Run lens-quickdev only for dev-ready features so it cannot bypass planning readiness.
  - Keep target-repo mutation in TargetProjects and stage evidence docs under feature.yaml.docs.path before publishing them to governance.
  - FinalizePlan owns the target-repo metadata registration required before dev-ready handoff.
  - Commit directly to an active in-progress feature branch when one exists; otherwise use Lens git orchestration for branch preparation, push, and PR creation.
  - Treat feature-associated control-repo docs as the default non-source surface; warn and document an override before expanding to broader control-repo or packaging work.
  - Store all quickdev and commit evidence in versioned quickdev artifacts under `quickdev/`.
open_questions: []
depends_on:
  - business-plan.md
blocks: []
updated_at: '2026-05-06T16:40:00Z'
---

# Tech Plan - lens-quickdev Wrapper

## Current Codebase Assessment

The current module has these relevant pieces:

| Surface | Current State | Implication |
| --- | --- | --- |
| `lens-bmad-skill` | Registered public wrapper and skill context resolver | Use it for the implementation handoff instead of bypassing Lens context. |
| `bmad-quick-dev` | Registered in `assets/lens-bmad-skill-registry.json` with `implementation-target` output mode | Existing implementation engine should remain the single source of behavior. |
| `/lens-bug-quickdev` | Bug-specific conductor with intake, branch, commit, push, PR, and bug-artifact recording | Useful pattern, but too bug-specific for general feature work. |
| `lens-quickdev` | No public prompt, skill, or module command surface found | New wrapper is additive. |
| Quickdev track | Present in lifecycle metadata as a lifecycle track | Do not confuse the public command with the track; the wrapper is an action, not a track conversion command. |

## Architecture

Add a public `lens-quickdev` conductor with this command shape:

```text
/lens-quickdev "<ask>" [--feature-id <id>] [--dry-run]
```

The conductor should live in the source module, not the release clone:

```text
TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-quickdev.prompt.md
TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-quickdev/SKILL.md
```

Registration updates should include the module discovery surfaces that are authoritative for this repo:

```text
TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/module.yaml
TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/module-help.csv
```

Feature-associated control-repo docs are the default non-source surface in scope for command visibility and operator guidance. If implementation discovers broader control-repo or packaging/discovery updates are required and materially expand scope, warn the user and document any approved override before editing those surfaces.

## Execution Contract

1. Run prompt-start preflight with `--caller lens-quickdev`.
2. Load `lens-quickdev/SKILL.md`; keep the prompt as a redirect only.
3. Resolve feature state via `lens-feature-yaml` and require an active feature unless `--feature-id` is supplied.
4. Enforce the dev-ready gate. The feature must have the FinalizePlan dev-ready signal, such as `milestones.dev-ready` or the equivalent lifecycle state used by `lens-dev`; planning phases including `expressplan`, `expressplan-complete`, and `finalizeplan` are blocked.
5. Resolve the target repo from `feature.yaml.target_repos[0].local_path`. FinalizePlan owns registering this metadata before dev handoff. If it is absent at runtime, stop with a clear target-repo blocker and do not guess.
6. Resolve docs path from `feature.yaml.docs.path` and governance docs path from `feature.yaml.docs.governance_docs_path`; create the staged docs folder and `quickdev/` subfolder when missing.
7. Generate the evidence file name from the request as a versioned artifact under `quickdev/quickdev-[summaryofrequeststub]-vNNN.md`:
  - lowercase the request summary
  - keep the first meaningful 4-8 words
  - replace non-alphanumeric runs with `-`
  - trim leading and trailing `-`
  - place each run under the `quickdev/` subfolder so evidence stays separate from root planning docs
  - choose the next available zero-padded version suffix such as `v001`, `v002`, and `v003`
  - create a new version for every rerun of the ask; do not overwrite prior artifacts
8. Inspect target repo state before implementation:
   - current branch
   - dirty worktree status
   - relevant prompt/skill/script files for the ask
   - likely test command or focused validation path
9. Write `quickdev/quickdev-[summaryofrequeststub]-vNNN.md` with request, assessment, assumptions, validation plan, and implementation plan, then update that same file throughout the run.
10. Resolve branch policy:
  - If the target repo is already on an active in-progress feature branch for this feature, commit directly to that branch and do not create a new PR branch.
  - If no active in-progress feature branch exists, prepare a target-repo working branch through `git-orchestration-ops.py prepare-dev-branch`, push it, and create or verify a PR through `git-orchestration-ops.py create-pr`.
11. Delegate implementation through the sanctioned Lens wrapper. If there is no script facade for `lens-bmad-skill` in the installed version, the skill contract must explicitly load the registered `bmad-quick-dev` skill and pass the same Lens context. Do not invent a second quick-dev engine.
12. Run focused validation selected during assessment.
13. Commit target-repo implementation changes with a conventional commit message. If there are no implementation changes, write that no-op result to the quickdev evidence file and do not create an empty commit.
14. If validation fails before commit creation, stop before committing, mark the quickdev evidence file `status: blocked`, record the failing command/output summary, and return a fix-forward recommendation.
15. If validation fails after a commit already exists but before push or PR creation, keep the commit local, do not push or open a PR, mark the quickdev evidence file `status: validation-failed`, and ask the user whether to fix forward or revert the local commit. Do not rewrite or delete commits automatically.
16. If validation fails after a branch has already been pushed or a PR exists, do not rewrite shared history. Add a follow-up fix commit or mark the PR blocked/draft according to repo policy, and update the quickdev evidence file with the failure and recovery action.
17. Update `quickdev/quickdev-[summaryofrequeststub]-vNNN.md` with branch, commit hash, changed files, validation command, validation result, PR URL when created, and final outcome.
18. Publish the exact versioned quickdev evidence file to `feature.yaml.docs.governance_docs_path/quickdev/` through the sanctioned Lens publication path, then commit/push the governance publication.

## Artifact Contracts

### `quickdev/quickdev-[summaryofrequeststub]-vNNN.md`

Frontmatter:

```yaml
feature: <featureId>
doc_type: quickdev
status: completed|blocked|no-op|validation-failed
artifact_version: <vNNN>
target_repo: <local_path>
branch: <working_branch>
commit: <hash or null>
pr_url: <url or null>
governance_published: true|false
updated_at: <timestamp>
```

Body sections:
- Request
- Codebase Assessment
- Implementation Plan
- Assumptions and Constraints
- Validation Plan
- Outcome
- Commit Summary
- Changed Files
- Validation Evidence
- PR or Handoff
- Governance Publication

## Validation Strategy

- Contract test: `lens-quickdev.prompt.md` is redirect-only and points at `skills/lens-quickdev/SKILL.md`.
- Registration test: `module.yaml` and `module-help.csv` expose `lens-quickdev` exactly once.
- Dev-ready gate test: non-dev-ready features block before target-repo assessment.
- Gate test: missing `target_repos` blocks before quick-dev delegation.
- Evidence test: successful run writes one generated versioned quickdev artifact under `feature.yaml.docs.path/quickdev/` and publishes the same relative file to `feature.yaml.docs.governance_docs_path/quickdev/`.
- Versioning test: rerunning the same ask creates the next `vNNN` artifact without overwriting the earlier run.
- Safety test: dirty target repo with unrelated changes blocks before implementation.
- Branch policy test: active in-progress feature branch receives a direct commit; otherwise the wrapper creates a working branch and PR.
- Validation failure test: pre-commit failures block without a commit, local post-commit failures do not push, and pushed failures use fix-forward or blocked PR handling.
- Regression test: `/lens-bug-quickdev` still routes to the bug-specific conductor and does not depend on the new generic wrapper.
- Review check: if implementation needs broader non-source control-repo or packaging surfaces, the scope-creep warning and any approved override are recorded before those edits proceed.

Recommended focused command after implementation:

```bash
uv run --with pytest --with pyyaml python -m pytest _bmad/lens-work -q
```

## Risks

| Risk | Impact | Mitigation |
| --- | --- | --- |
| Wrapper duplicates quick-dev implementation logic | High | Keep `lens-quickdev` conductor-only; delegate to `bmad-quick-dev`. |
| Target repo cannot be resolved for older features | Medium | Block clearly and require FinalizePlan-owned metadata registration to add `target_repos` before dev handoff. |
| Docs record drifts from actual commit | High | Generate the quickdev evidence update after commit and read commit hash from git. |
| Required documentation or discovery work expands beyond feature docs | Medium | Warn the user about scope creep and record an explicit override before editing broader control-repo or packaging surfaces. |
| Public command conflicts with quickdev lifecycle track naming | Medium | Document wrapper as an action command, not track conversion. |
| Existing bug quickdev flow regresses | Medium | Add explicit non-regression acceptance around `/lens-bug-quickdev`. |
| Validation fails after commit | High | Do not push local failing commits; if already pushed, fix forward or mark the PR blocked without rewriting shared history. |
