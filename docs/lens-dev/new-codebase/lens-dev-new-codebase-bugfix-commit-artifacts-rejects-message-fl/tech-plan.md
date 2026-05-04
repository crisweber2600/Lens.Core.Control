---
feature: lens-dev-new-codebase-bugfix-commit-artifacts-rejects-message-fl
doc_type: tech-plan
status: approved
track: express
updated_at: '2026-05-04T02:00:00Z'
key_decisions:
  - Prefer backwards-compatible script aliases where observed command forms are safe and unambiguous.
  - Prefer durable repo-owned helpers over conductor-authored shell or Python snippets.
  - Keep all source changes in TargetProjects/lens-dev/new-codebase/lens.core.src/.
open_questions: []
---

# Tech Plan: Script Errors and On-the-fly Workflow Scripts

## Architecture Summary

This feature hardens Lens lifecycle automation at the command-interface layer. The fixes should be concentrated in existing Lens source scripts and skill documentation under:

- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-git-orchestration/scripts/`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-feature-yaml/scripts/`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/lens-preflight/`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/scripts/`
- `AGENTS.md` in the control repo when documenting agent-level terminal restrictions.

## Implementation Areas

### 1. Git orchestration command compatibility

Bugs covered:

- `commit-artifacts-rejects-message-flag-used-by-finalize-flow-0e8df239`
- `git-orchestration-create-pr-rejects-source-target-branch-ali-5d54cf50`
- `temporary-shell-heredoc-used-to-construct-pr-body-e891cbaa`

Planned changes:

- Add `--message` as a backwards-compatible alias for `commit-artifacts --description`, or update the command to emit a targeted error: `Use --description; --message is deprecated/unsupported`.
- Add `--source-branch` alias for `create-pr --head` and `--target-branch` alias for `create-pr --base`, if both can map without ambiguity.
- Ensure `create-pr` supports a normal `--body` string and/or `--body-file` without requiring conductors to create temporary files manually.
- Add tests for alias parsing and generated PR command payload.

### 2. Feature YAML phase/update ergonomics

Bugs covered:

- `feature-yaml-ops-lacks-set-phase-alias-used-by-finalize-flow-15f68061`
- `feature-yaml-ops-update-rejects-generic-field-value-usage-439aeedf`

Planned changes:

- Add a `set-phase` subcommand as a thin alias to the existing `update --phase` implementation, preserving the same transition validation.
- Add `update --field --value` only for an allowlist of safe scalar fields, or reject it with a structured message listing supported explicit flags.
- At minimum, support `--field phase --value <phase>` as an alias to `--phase <phase>` because that exact usage occurred in the lifecycle flow.
- Add tests for `set-phase`, `--field phase --value`, unsupported fields, and transition validation.

### 3. Preflight script contract alignment

Bugs covered:

- `lens-preflight-skill-documents-unsupported-light-preflight-a-63d99dcb`
- `light-preflight-bypasses-full-preflight-sync-helper-45f310c2`
- `lens-script-path-failed-from-wrong-working-directory-4c8b82f1`

Planned changes:

- Decide and encode the intended split:
  - Option A: `light-preflight.py` stays a lightweight gate, and `SKILL.md` is corrected to document only actual behavior.
  - Option B: `light-preflight.py` delegates to the full `preflight.py` helper, restoring old behavior.
- Because the old-codebase pattern used light preflight as a prompt-start sync, prefer Option B unless tests reveal unacceptable startup cost.
- Add argument parsing for documented `--caller` and `--governance-path` if full delegation is restored.
- Normalize script invocation examples so workspace-root prompts use `lens.core/...` and source-root docs use `_bmad/...` consistently.
- Add tests for root detection from workspace root and source repo root.

### 4. Durable workflow helpers for recurring on-the-fly scripts

Bugs covered:

- `lifecycle-state-checks-used-ad-hoc-python-one-liners-c3bfffe0`
- `on-the-fly-scripts-used-during-lens-workflow-execution-a4b36743`
- `prompt-repair-used-ad-hoc-python-heredoc-instead-of-repo-too-0131406d`

Planned changes:

- Add or extend a Lens script command for lifecycle state inspection that prints feature phase, track, target repos, docs path, and PR links.
- Add a repo-owned prompt normalization/repair helper for prompt file newline/path repair, or document that prompt repairs must use the helper rather than inline Python/PowerShell.
- Update `AGENTS.md` terminal-error guidance to prohibit PowerShell bulk prompt replacements and to prefer repo-owned scripts for recurring lifecycle state checks.
- Add tests around prompt repair behavior if a helper is added.

## Testing Strategy

- Focused unit tests for `feature-yaml-ops.py` aliases and validation.
- Focused unit tests for `git-orchestration-ops.py` alias parsing and PR body handling.
- Preflight tests for documented argument handling and root detection.
- If prompt repair helper is added, fixture-based tests using temporary prompt files with literal `\\r\\n` tokens.

## Risks

- Adding aliases can hide stale conductor instructions. Mitigation: mark aliases as compatibility surfaces and keep canonical docs updated.
- Delegating light preflight to full sync may slow prompt startup. Mitigation: preserve timestamp window behavior from the full helper.
- Generic `--field --value` updates can become unsafe. Mitigation: allowlist only known safe fields and reject all others.

## Rollout

Implement in a target source feature branch from `develop`. Open PR to `develop`. Do not modify `lens.core/` directly.

S4 additionally requires a control-repo change to `AGENTS.md` in this repository. That edit must be made directly in the control repo, committed to the appropriate feature branch, and landed via a separate PR targeting the control repo — independent of the `lens.core.src` source branch and PR.
