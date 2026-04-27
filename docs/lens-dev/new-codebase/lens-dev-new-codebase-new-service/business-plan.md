---
feature: lens-dev-new-codebase-new-service
doc_type: business-plan
status: draft
goal: "Restore /new-service as a clean-room retained command with observable parity to the old Lens behavior"
key_decisions:
  - Treat new-service as one of the 17 retained published commands required for day-1 rewrite parity.
  - Preserve the service-container boundary: governance markers, constitutions, optional scaffolds, and personal context only.
  - Do not create feature branches, feature.yaml, feature-index entries, summaries, lifecycle artifacts, or target-repo code changes.
  - Use old-codebase behavior and baseline planning docs as evidence, while authoring the new implementation and tests independently.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T14:05:26Z
---

# Business Plan - New Service Command

## Executive Summary

The `new-service` feature restores a retained Lens command that creates a service container beneath a domain. The old Lens workflow exposed `/new-service` as a governance-scaffolding command, but the new codebase currently only implements the `new-domain` half of the shared initialization family. This feature closes that parity gap by delivering the service-level behavior needed for users to continue the normal setup path: create or confirm a domain, create a service container, set local context, and then create features inside that service.

## Business Context

The Lens rewrite is a backwards-compatible command-surface reduction, not a product redesign. The baseline product plan retains 17 published commands, and `new-service` is one of the setup commands that makes the rest of the lifecycle usable. Without it, a new Lens user can create a domain but cannot complete the domain to service to feature setup sequence through the retained command surface. Existing users and integrators would also see a broken promise: the rewrite claims parity while omitting a shipped command.

This feature matters because service creation is the bridge between organizational scaffolding and feature lifecycle work. Lens governance is organized as domain, service, feature. If the service layer cannot be created from the new command surface, downstream commands such as `new-feature`, `constitution`, `discover`, and `next` lose the context they expect.

## Stakeholders

| Stakeholder | Interest | Sign-off Concern |
|---|---|---|
| New Lens users | Need a guided setup path before feature work | `/new-service` exists and behaves predictably after `/new-domain` |
| Existing Lens users | Need no regression after upgrading to the rewrite | Existing governance layout and command semantics remain valid |
| Lens maintainers | Need a smaller command surface without hidden missing dependencies | The retained init-feature family is complete and tested |
| BMAD work-system integrators | Need published prompts, help, installer metadata, and skills to agree | `/new-service` is discoverable and supported wherever retained commands are listed |
| Governance owners | Need service constitutions to preserve hierarchy rules | Service-level constitution inherits domain-level governance rather than bypassing it |

## Success Criteria

1. `/new-service` is present in the new-codebase prompt and discovery surfaces wherever retained published commands are listed.
2. The shared `bmad-lens-init-feature` skill documents a `new-service` intent flow parallel to `new-domain`.
3. The `create-service` script path creates a service marker at `features/{domain}/{service}/service.yaml`.
4. Missing parent domain markers and domain constitutions are created when required, matching the observed old setup contract.
5. A service-level constitution is created at `constitutions/{domain}/{service}/constitution.md` and explicitly inherits from the domain constitution.
6. The TargetProjects and docs scaffolds create `{domain}/{service}/.gitkeep` entries.
7. The personal context file is updated with both `domain` and `service`, using `updated_by: new-service`.
8. Governance git automation can pull, write, commit, and push to governance `main`, returning `governance_commit_sha` when successful.
9. Dry-run mode reports planned paths and commands without writing files.
10. Service creation never creates feature branches, feature.yaml, feature-index entries, summaries, lifecycle artifacts, or target-repo code.
11. Focused service regression tests pass and prove observable output parity without copying old implementation code.

## Scope

### In Scope

- Add or restore the release prompt for `lens-new-service` in the new-codebase source tree.
- Extend `bmad-lens-init-feature` documentation with a dedicated `new-service` intent flow.
- Implement a `create-service` subcommand in the new-codebase init script.
- Produce service marker YAML with the same observable fields and hierarchy meaning as the old command.
- Produce domain and service constitutions when missing, with service inheritance clearly represented.
- Support configured target-project and docs scaffolds.
- Support personal context activation for non-feature-branch workflows.
- Support `--execute-governance-git`, manual git command fallback, and `--dry-run`.
- Add tests for service creation, duplicate handling, scaffolds, context, git automation, and dry-run behavior.

### Out of Scope

- Creating features inside the new service. That remains `new-feature`.
- Creating or changing control-repo feature branches. Service creation is governance and scaffold work only.
- Changing the feature identity formula, branch topology, lifecycle schema, or feature-index schema.
- Changing the semantics of `new-domain` beyond the minimum helper refactor needed to share context-writing behavior.
- Directly copying old-codebase files, function bodies, or prose into the new implementation.

## Risks and Mitigations

| Risk | Probability | Impact | Mitigation |
|---|---:|---:|---|
| Service creation accidentally becomes feature creation | Medium | High | Treat "no feature branches, no feature.yaml, no feature-index" as a testable boundary. |
| Parent domain handling drifts from old behavior | Medium | Medium | Add tests that service creation can establish missing parent domain marker and constitution paths. |
| Personal context loses the domain during service activation | Medium | Medium | Extend context tests to assert both `domain` and `service` plus `updated_by: new-service`. |
| Prompt/help/installer surfaces advertise different command sets | Medium | High | Audit retained-command surfaces and include `new-service` in each relevant metadata file. |
| Scaffold failures are hidden behind a successful governance commit | Low | Medium | Preserve separate `workspace_git_commands` and `remaining_git_commands` fields for manual scaffold follow-up. |
| Clean-room parity weakens because old code is copied instead of specified | Low | High | Drive implementation from observable requirements and independent tests, not direct file copying. |

## Dependencies

- Baseline rewrite planning for `lens-dev-new-codebase-baseline`, especially the retained 17-command surface and backwards-compatibility constraints.
- Old `/new-service` prompt behavior as a requirements source for observable command outputs and boundaries.
- Existing new-codebase `new-domain` implementation, which provides nearby helper patterns for markers, constitutions, scaffolds, context, dry-run, and governance git execution.
- Lens lifecycle v4.0 rules that define domain, service, and feature hierarchy without requiring schema migration.

## Open Questions

None. The old command contract, baseline retained-surface decision, and current implementation gap are sufficient to proceed with implementation planning.

## Timeline Expectations

This should be delivered before the new-codebase rewrite can claim retained-command parity. It is a setup-path blocker for the command surface, so it should be completed in the same planning wave as the other retained command parity features rather than deferred to post-MVP cleanup.

## ExpressPlan Compatibility Note

This feature is currently registered as `track: full` and `phase: preplan`. The formal `lens-expressplan` lifecycle cannot auto-advance it because ExpressPlan is express-track only. These artifacts are therefore staged as clean-room business and technical plans for the full-track feature while preserving the ExpressPlan two-document rule and frontmatter contract requested for this planning task.