---
feature: lens-dev-new-codebase-new-service
doc_type: sprint-plan
status: draft
goal: "Sequence clean-room /new-service parity work into validation-first implementation slices"
key_decisions:
  - Implement service parity as a focused init-feature extension rather than a separate skill.
  - Start with failing service tests to lock observable behavior before script changes.
  - Keep prompt/discovery metadata changes in the same delivery slice so the retained command is actually reachable.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T14:13:14Z
---

# Sprint Plan - New Service Command

## Sprint Goal

Restore `/new-service` as a retained Lens setup command in the new codebase, with clean-room observable parity to the old service-container workflow and no lifecycle schema changes.

## Sprint 1 - Contract and Test Lock

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| NS-1: Define service CLI contract tests | Failing tests cover `create-service` success, dry-run, duplicate, invalid ID, scaffolds, context, and governance git behavior. Test matrix must explicitly include the `--dry-run + --execute-governance-git` mutual-exclusion rejection case: passing both flags simultaneously must return a rejected-arguments error without writing any files. | 3 | business-plan.md, tech-plan.md |
| NS-2: Assert service-not-feature boundary | Tests prove service creation does not create `feature.yaml`, `summary.md`, feature-index entries, or control branches. Also include an integration test variant that starts from a pre-existing domain container to validate that the parent-domain creation path is idempotent (running `create-service` when the domain already exists must not overwrite or duplicate the existing domain marker or constitution). | 2 | NS-1 |
| NS-3: Add prompt and help discovery expectations | Tests or static checks confirm `lens-new-service.prompt.md` and module help expose the retained command | 2 | NS-1 |

## Sprint 2 - Script Implementation

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| NS-4: Add service marker and constitution builders | Script can produce service YAML and inherited service constitution with stable fields | 3 | NS-1 |
| NS-5: Implement `create-service` parser route | CLI accepts governance, domain, service, name, username, scaffold roots, personal folder, dry-run, and auto-git flags | 3 | NS-4 |
| NS-6: Extend context writing safely | Context writer supports domain-only and domain-plus-service updates without schema drift | 2 | NS-5 |
| NS-7: Preserve governance git behavior | Auto governance git pull/add/commit/push returns commit SHA and leaves scaffold follow-up commands visible. **The governance git path must be idempotent:** a failed and retried run must not create duplicate service markers, domain markers, or constitution files. Add an explicit retry test: run `create-service --execute-governance-git` once, simulate a partial failure, re-run, and assert that governance state is consistent with no duplicate artifacts. | 3 | NS-5 |

## Sprint 3 - Skill, Prompt, and Surface Parity

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| NS-8: Document `new-service` intent flow | `bmad-lens-init-feature` explains service display-name prompt, slug confirmation, and script invocation | 2 | NS-5 |
| NS-9: Add release prompt | New release prompt delegates to init-feature with `create-service` intent and records no feature lifecycle writes | 2 | NS-8 |
| NS-10: Align command discovery metadata | Module help and packaging surfaces include `new-service` consistently with retained command policy | 2 | NS-9 |

## Sprint 4 - Verification and Handoff

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| NS-11: Run focused service parity tests | `-k create_service` passes from the new-codebase source root | 2 | NS-7, NS-10 |
| NS-12: Run full init-feature regression | Full init-feature test file passes without regressing `new-domain`. Before marking this story complete, also verify that NS-13 handoff notes exist and reference the correct test commands, file list, implementation channel decisions, and clean-room constraints. NS-12 is the gating check for NS-13 completeness. | 2 | NS-11 |
| NS-13: Prepare implementation handoff notes (**required**) | Dev handoff names files, tests, non-goals, clean-room constraints, and implementation channel decisions: skill artifact changes via `.github/skills/bmad-module-builder`; prompt and workflow artifacts via `.github/skills/bmad-workflow-builder`; direct `lens.core.src` edits for NS-4–NS-7 are accepted deviations recorded here. This story is a **required gate** for `/dev` handoff — not optional polish. | 1 | NS-12 |

## Dependencies and Sequencing

- `NS-1` through `NS-3` should run first so implementation is pinned to behavior, not copied source.
- `NS-4` through `NS-7` produce the script surface that all prompt and skill changes depend on.
- `NS-8` through `NS-10` make the command reachable and discoverable.
- `NS-11` through `NS-13` prove the implementation can move into development with a focused regression story.
- This feature **blocks** `lens-dev-new-codebase-new-feature` (pre-plan): `new-feature` planning should not begin until `new-service` reaches `dev-ready`. This dependency is recorded in both feature.yaml files.

## Risks To Track During Execution

| Risk | Mitigation |
|---|---|
| Service creation accidentally mutates feature lifecycle state | Keep NS-2 as a hard regression and inspect generated governance tree after each run |
| Auto-git behavior commits scaffold files unintentionally | Keep governance git and workspace scaffold commands separated in output |
| Context writer regresses `new-domain` | Run both domain and service context tests in the full init-feature suite |
| Prompt metadata ships without script support | Treat discovery metadata changes as dependent on passing script tests |

## Definition of Done

- **Pre-development setup:** Associated target repos (`lens.core.src`, `lens-governance`) cloned into `TargetProjects/lens-dev/new-codebase/` and `TargetProjects/lens/` respectively; `/discover` run with `domain: lens-dev, service: new-codebase` before Sprint 1 begins.
- Focused service parity tests pass.
- Full init-feature regression passes.
- `new-service` appears in the retained prompt surface and module help.
- No lifecycle schema, feature schema, branch topology, or feature-index semantics change.
- NS-13 handoff notes exist and record: implementation channel decisions (`.github/skills/bmad-module-builder` for skill updates, `.github/skills/bmad-workflow-builder` for prompt/workflow), clean-room constraint, test commands, and files changed.
- Dev handoff can proceed with implementation stories that are small enough for targeted review.