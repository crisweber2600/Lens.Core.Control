---
feature: lens-dev-new-codebase-new-feature
doc_type: business-plan
status: draft
goal: "Define clean-room business requirements for new-feature command parity"
key_decisions:
  - Treat the old-codebase prompt and init-feature contract as behavioral evidence only; do not copy source files directly.
  - Preserve the public new-feature command as a retained Lens command in the 17-command surface.
  - Require output parity for feature identity, governance registration, branch topology, and returned handoff commands.
  - Keep express-track planning PR deferral distinct from full/feature-track immediate planning PR behavior.
open_questions:
  - Should the new-codebase feature initializer restore fetch-context in the same delivery slice or leave it to a separate follow-up?
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T14:15:58Z
---

# Business Plan — New Feature Command

## Executive Summary

The `new-feature` command must become operational in the new lens-work codebase with behavior that matches the old-codebase feature initializer from a user's point of view. The feature exists to let a user create a first-class Lens feature under an existing domain and service, register it in governance, create the control-repo two-branch topology, and land in the correct next planning phase without manual repair. The business value is continuity: users can move from the current Lens release to the rewritten command surface without losing the most important entry point for feature work.

## Business Context

The baseline rewrite narrows Lens to 17 retained published commands and makes `new-feature` one of the core lifecycle entry points. Without this command, the retained surface is incomplete: users can create domains and services, but cannot start actual governed feature work in the new codebase. That breaks the rewrite promise of day-one command parity and undercuts the stated goal of rebuilding Lens through Lens itself.

The current new-codebase implementation has started the shared initializer around `new-domain`, but the `new-feature` prompt, release prompt, skill flow, and script-level `create` operation are not yet present. This feature fills that gap by implementing the new command as a clean-room equivalent of the old behavior: the old prompt and contract define required outcomes, while the implementation is independently authored in the new codebase.

## Stakeholders

| Stakeholder | Interest | Sign-off Signal |
|---|---|---|
| Lens users starting work | Need a reliable feature creation path from domain/service context into planning | They can create a feature and immediately continue with `/next` or the returned recommended command |
| Existing Lens users | Need no visible regression from old-codebase `new-feature` behavior | Feature identity, branch names, governance files, and PR behavior match old outputs |
| Lens maintainers | Need a maintainable shared initializer instead of one-off command code | `new-domain`, `new-service`, and `new-feature` share safe helpers without behavior drift |
| Release integrators | Need the retained 17-command surface to be internally coherent | Prompt stubs, release prompts, module help, and tests agree that `new-feature` is shipped |

## Success Criteria

| Criterion | Measure |
|---|---|
| Published command availability | `lens-new-feature.prompt.md` exists in the installed prompt surface and delegates through light preflight to the release prompt |
| Release prompt parity | The release prompt loads `bmad-lens-init-feature` and describes the same high-level initialization outcomes as the old command |
| Canonical identity parity | A local slug such as `auth-refresh` becomes `{domain}-{service}-auth-refresh`; passing the already canonical ID preserves it |
| Governance registration parity | `feature.yaml`, `feature-index.yaml`, and `summary.md` are created under the canonical governance path |
| Branch topology parity | Returned control-repo commands create `{featureId}` and `{featureId}-plan` through git orchestration, not hand-written checkout commands |
| Track-aware handoff parity | Full track starts at `preplan`; feature/legacy quickplan starts at `businessplan`; express starts at `expressplan` and defers the empty planning PR |
| Clean-room assurance | Implementation and docs are authored from behavioral requirements; no old-codebase files are copied into the new codebase |
| Regression coverage | Focused initializer tests pass in the new codebase and cover identity, track, governance, command-return, and dirty-repo failure behavior |

## Scope

### In Scope

- Add the user-facing `lens-new-feature` prompt stub to the new-codebase published prompt surface.
- Add the release prompt redirect for `lens-new-feature` under `_bmad/lens-work/prompts/`.
- Extend `bmad-lens-init-feature` so it explicitly supports the `new-feature` intent flow, not only `new-domain`.
- Implement the `init-feature-ops.py create` subcommand in the new codebase using clean-room code.
- Preserve output fields required by callers: feature identity, start phase, recommended command, governance paths, git command groups, activation commands, GitHub PR commands, and governance commit metadata.
- Preserve track-specific planning PR behavior, including express-track PR deferral.
- Add focused tests that assert observable parity rather than source equivalence.

### Out of Scope

- Changing the canonical feature ID formula.
- Changing feature.yaml, feature-index.yaml, or docs path schemas.
- Creating new lifecycle tracks or phases.
- Replacing git-orchestration or switch with manual branch commands.
- Directly writing planning artifacts to the governance repo outside the established publish path.
- Copying old implementation files into the new codebase.

## Risks and Mitigations

| Risk | Probability | Impact | Mitigation |
|---|---:|---:|---|
| Feature ID derivation drifts from the old command | Medium | Critical | Add tests for local slug, canonical ID, duplicate local slug across services, and invalid characters |
| New code writes governance files but fails before branch commands are executed | Medium | High | Return structured `remaining_commands` and keep governance git execution explicit and inspectable |
| Express-track behavior accidentally opens an empty planning PR | Medium | High | Add a dedicated express-track test that verifies `planning_pr_created: false` and no `gh_commands` |
| Direct file copying introduces hidden incompatibility or licensing/process concerns | Low | High | Use old artifacts only as requirement evidence; write new code and docs independently |
| Command surfaces drift after implementation | Medium | Medium | Include prompt stub, release prompt, skill docs, help/manifests, and tests in the implementation checklist |
| Existing `new-domain` behavior regresses while extending the shared initializer | Medium | High | Keep current create-domain tests green and add service/feature tests without weakening existing assertions |

## Dependencies

- Baseline rewrite requirements for the 17-command retained surface.
- Existing new-codebase `bmad-lens-init-feature` partial implementation for `create-domain`.
- Old-codebase observable behavior for `lens-new-feature`, `bmad-lens-init-feature`, and `init-feature-ops.py`.
- `bmad-lens-git-orchestration` command surface for branch creation.
- `bmad-lens-switch` command surface for activating the new feature context.
- `lifecycle.yaml` track definitions for resolving the start phase.

## Open Questions

1. Should `fetch-context` be delivered with this feature because the old initializer exposes it and follow-on phases depend on it, or should this feature focus strictly on `create` parity?
2. Should `create-service` parity be completed first as an enabling dependency, or should `create` include only the parent marker behavior needed for feature creation?
3. Should installer/help surface updates be part of this feature or handled by a broader retained-command surface task?

## Timeline Expectations

This is a high-priority parity feature because `new-feature` is one of the core entry points in the retained command surface. A reasonable delivery shape is three small increments: command surface and skill contract, script parity and tests, then integration hardening across help/manifests and git command returns.
