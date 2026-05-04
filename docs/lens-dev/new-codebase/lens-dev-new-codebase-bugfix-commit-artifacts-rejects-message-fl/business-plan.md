---
feature: lens-dev-new-codebase-bugfix-commit-artifacts-rejects-message-fl
doc_type: business-plan
status: approved
track: express
updated_at: '2026-05-04T02:00:00Z'
---

# Business Plan: Script Errors and On-the-fly Workflow Scripts

## Problem

Recent Lens lifecycle runs exposed a repeated reliability problem: conductors hit unsupported script flags, mixed workspace-root and source-root script paths, and compensated by using ad hoc shell/Python snippets. Those snippets got work unstuck, but they bypassed durable Lens tooling, were hard to review, and created follow-on bugs such as prompt newline corruption.

This bugfix feature bundles 11 New bugs into one express-track remediation plan focused on making recurring workflow operations durable, tested, and script-owned.

## Bugs in Scope

1. `commit-artifacts-rejects-message-flag-used-by-finalize-flow-0e8df239`
2. `feature-yaml-ops-lacks-set-phase-alias-used-by-finalize-flow-15f68061`
3. `feature-yaml-ops-update-rejects-generic-field-value-usage-439aeedf`
4. `git-orchestration-create-pr-rejects-source-target-branch-ali-5d54cf50`
5. `lens-preflight-skill-documents-unsupported-light-preflight-a-63d99dcb`
6. `lens-script-path-failed-from-wrong-working-directory-4c8b82f1`
7. `lifecycle-state-checks-used-ad-hoc-python-one-liners-c3bfffe0`
8. `light-preflight-bypasses-full-preflight-sync-helper-45f310c2`
9. `on-the-fly-scripts-used-during-lens-workflow-execution-a4b36743`
10. `prompt-repair-used-ad-hoc-python-heredoc-instead-of-repo-too-0131406d`
11. `temporary-shell-heredoc-used-to-construct-pr-body-e891cbaa`

## Goals

- Reduce lifecycle command retries caused by unsupported flags and inconsistent path contracts.
- Replace recurring one-off terminal snippets with durable Lens scripts or documented safe command surfaces.
- Align `lens-preflight` documentation with actual script behavior, or restore delegated full-sync behavior where required.
- Preserve compatibility for recently observed conductor command forms when it is safe to do so.

## Non-Goals

- Do not redesign the full Lens lifecycle.
- Do not merge source `develop` to `main`.
- Do not change the read-only `lens.core/` release clone directly.
- Do not make broad formatting or unrelated prompt rewrites.

## Users

- Lens conductors running `/lens-bug-fixer`, `/lens-finalizeplan`, `/lens-dev`, and `/lens-complete`.
- Maintainers reviewing source repo PRs for lifecycle automation.
- Future agents depending on stable script interfaces instead of transient shell snippets.

## Requirements

1. `git-orchestration-ops.py commit-artifacts` accepts or clearly rejects the `--message` command shape used during finalizeplan.
2. `feature-yaml-ops.py` exposes a stable phase transition alias for `set-phase` or updates all conductor docs to use the existing supported command.
3. `feature-yaml-ops.py update` handles known generic field updates safely or rejects them with a clear remediation message.
4. `git-orchestration-ops.py create-pr` supports common source/target branch aliases or reports the supported `--head/--base` form clearly.
5. `lens-preflight` skill docs and `light-preflight.py` behavior match.
6. Script path invocation guidance is cwd-safe across workspace-root and source-root contexts.
7. Recurring lifecycle state inspection is handled by Lens scripts rather than ad hoc Python one-liners.
8. Prompt repair/normalization has a durable repo-owned script or clear prohibition against on-the-fly repairs.
9. PR body generation is handled by the PR orchestration command or a durable helper, not temporary shell heredocs.

## Success Criteria

- All 11 bugs have implementation stories traceable to concrete files and acceptance criteria.
- Focused regression tests cover new command aliases, error messages, and preflight/script contract behavior.
- Future conductors can complete equivalent lifecycle flows without unsupported flag retries or one-off repair scripts.
- Feature can advance to FinalizePlan with no unresolved blocking findings.
