---
feature: lens-dev-new-codebase-bugfix-commit-artifacts-rejects-message-fl
doc_type: retrospective
status: approved
track: express
updated_at: '2026-05-04T00:00:00Z'
---

# Retrospective: Bugfix — Commit Artifacts Rejects Message Flag

## What Went Well

- All 4 stories were implemented, tested (113 tests passing), and committed cleanly on a feature branch from `develop`.
- Aliases for `--message`, `--source-branch`, `--target-branch` were added to `git-orchestration-ops.py` without breaking any existing callers.
- `feature-yaml-ops.py` `set-phase` alias and `--field phase` routing were implemented with proper validation and conflict detection.
- `light-preflight.py` Option B delegation was implemented with workspace-root detection, eliminating the need for a separate thin gate.
- `lifecycle-state.py` and `prompt-normalize.py` were introduced as durable helpers, replacing recurring ad hoc Python snippets.
- `.lens/governance-setup.yaml` governance path was made relative using `{project-root}` token to support portability across clone locations.

## What Didn't Go Well

- `.lens/governance-setup.yaml` had a stale absolute path pointing to a different workspace clone, which blocked the initial `/lens-complete` run.
- Windows console encoding (`cp1252`) caused `UnicodeEncodeError` when LENS scripts emitted Unicode checkmarks — required `PYTHONUTF8=1` prefix on all `uv run` invocations.
- `AGENTS.md` guidance on `PYTHONUTF8=1` was missing and had to be added post-dev as a separate commit.

## Key Learnings

- `governance-setup.yaml` paths must use `{project-root}` from the start to avoid stale-path failures on re-clone.
- All `uv run` invocations in this workspace must be prefixed with `PYTHONUTF8=1` on Windows.
- Durable helper scripts (`lifecycle-state.py`, `prompt-normalize.py`) prevent recurring ad hoc snippet drift and should be added early in the dev phase.

## Action Items

- [x] Added `PYTHONUTF8=1` guidance to AGENTS.md.
- [x] Converted `governance-setup.yaml` to use `{project-root}` relative path.
- [ ] `setup.py` should generate `governance_repo_path` using `{project-root}` token by default.
