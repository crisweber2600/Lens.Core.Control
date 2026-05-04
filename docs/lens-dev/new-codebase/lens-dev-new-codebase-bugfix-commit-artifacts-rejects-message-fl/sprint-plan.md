---
feature: lens-dev-new-codebase-bugfix-commit-artifacts-rejects-message-fl
doc_type: sprint-plan
status: approved
track: express
updated_at: '2026-05-04T02:00:00Z'
---

# Sprint Plan: Script Errors and On-the-fly Workflow Scripts

## Sprint Goal

Make the observed Lens lifecycle script failures and recurring on-the-fly script workarounds durable, testable, and command-owned.

## Story Map

| Story | Scope | Bugs Covered | Target Repo |
|---|---|---|---|
| S1 | Feature YAML command aliases | set-phase, field/value update | `TargetProjects/lens-dev/new-codebase/lens.core.src` |
| S2 | Git orchestration command aliases and PR body handling | commit-artifacts --message, create-pr source/target branch aliases, temp PR body heredoc | `TargetProjects/lens-dev/new-codebase/lens.core.src` |
| S3 | Preflight contract alignment | light-preflight behavior/docs mismatch, cwd-safe script paths | `TargetProjects/lens-dev/new-codebase/lens.core.src` |
| S4 | Durable helpers for one-off script patterns | lifecycle Python one-liners, prompt repair heredoc, general on-the-fly scripts | `TargetProjects/lens-dev/new-codebase/lens.core.src`, `AGENTS.md` |

## Sprint Sequence

1. **S1 — Feature YAML aliases**
   - Add `set-phase` alias or equivalent compatibility path.
   - Add allowlisted `--field phase --value <phase>` support or structured rejection.
   - Add tests for supported and unsupported update forms.

2. **S2 — Git orchestration compatibility**
   - Add `commit-artifacts --message` compatibility or targeted guidance.
   - Add `create-pr --source-branch/--target-branch` aliases.
   - Ensure PR body generation is script-owned and does not require temporary shell heredocs.
   - Add tests.

3. **S3 — Preflight contract alignment**
   - Reconcile `lens-preflight/SKILL.md` with actual `light-preflight.py` behavior.
   - Prefer restoring full-sync delegation if prompt-start sync remains required.
   - Add root/path tests for workspace-root and source-root invocation forms.

4. **S4 — Durable no-inline-script helpers**
   - Provide lifecycle state inspection through a Lens script or documented existing command.
   - Provide prompt repair/normalization helper if prompt file repairs remain an expected operation.
   - Update `AGENTS.md` with precise prohibitions and approved alternatives.

## Acceptance Criteria

- All 11 bugs map to at least one story.
- Every new command alias has a focused regression test or an explicit structured rejection test.
- Preflight documentation and script behavior agree.
- Planning identifies all edits as TargetProjects/source changes except the control-root `AGENTS.md` guidance.
- No story requires writing to the read-only `lens.core/` clone.

## Dev Notes

- Branch from `develop` in `TargetProjects/lens-dev/new-codebase/lens.core.src`.
- Keep changes minimal and scoped to Lens lifecycle tooling.
- Use repo-owned scripts for validation rather than ad hoc Python or PowerShell snippets.
