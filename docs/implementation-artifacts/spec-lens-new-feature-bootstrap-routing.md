---
title: 'Lens New-Feature Bootstrap Routing'
type: 'bugfix'
created: '2026-04-20'
status: 'done'
baseline_commit: '33bbc04a78f5dbfa579149b5165c3eb4248561ff'
context: []
---

<frozen-after-approval reason="human-owned intent — do not modify unless human renegotiates">

## Intent

**Problem:** `/lens-new-feature` currently routes straight into feature initialization, so users who start a first feature in a new domain or service do not get the `TargetProjects` and `docs` scaffolds that only the domain/service bootstrap flow creates. This makes the feature entrypoint behave differently from the project bootstrap flow and produces incomplete workspace setup.

**Approach:** Make `lens-new-feature` the canonical feature-entry router: keep the direct feature-init path when the domain and service already exist, but require it to detect missing containers and delegate to the existing `lens-new-project` bootstrap flow instead of relying on feature `create`. Align the alias prompt, shell routes, and prompt contracts around that canonical router.

## Boundaries & Constraints

**Always:** Preserve the current `init-feature-ops.py` split where `create` handles feature governance and branch setup while `create-domain` and `create-service` own constitutions, workspace scaffolds, and personal context; reuse the existing `lens-new-project` prompt for missing-container bootstrap behavior instead of cloning its instructions; keep generated/stub surfaces and contract tests consistent with the final routing contract.

**Ask First:** Any change that alters target-repo provisioning semantics, removes `lens-new-project` as a public entrypoint, or rewrites the Python ops script behavior instead of the prompt/router layer.

**Never:** Add a new bootstrap script for this fix; make `create` silently scaffold `TargetProjects` or `docs`; change or revert unrelated dirty worktree files.

## I/O & Edge-Case Matrix

| Scenario | Input / State | Expected Output / Behavior | Error Handling |
|----------|--------------|---------------------------|----------------|
| Existing containers | User runs `/lens-new-feature` for a domain/service that already has governance markers | Prompt stays on the direct feature-init path and preserves current `create --execute-governance-git` behavior | Governance git failures still stop the flow and surface the script error without a manual fallback recipe |
| Missing container bootstrap | User runs `/lens-new-feature` for a domain or service that does not yet exist | Prompt inspects governance first, then delegates to the existing `lens-new-project` bootstrap sequence so `create-domain` and `create-service` can create constitutions plus `TargetProjects` and `docs` scaffolds before feature init | If the bootstrap path hits a governance git preflight or execution failure, stop at that failure and surface it directly |
| Alias consistency | User runs `/lens-init-feature` or selects `[IF]` from the Lens shell | Alias and shell routing land on the same canonical router as `/lens-new-feature`, so the same missing-container logic applies | N/A |

</frozen-after-approval>

## Code Map

- `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md` -- Canonical feature-entry router to make container-aware.
- `lens.core/_bmad/lens-work/prompts/lens-new-project.prompt.md` -- Existing bootstrap workflow to reuse for missing-domain/service cases.
- `lens.core/_bmad/lens-work/prompts/lens-init-feature.prompt.md` -- Alias prompt that should delegate to the canonical router.
- `lens.core/_bmad/lens-work/agents/lens.agent.md` -- Shell `[IF]` route that currently bypasses prompt-level bootstrap routing.
- `lens.core/_bmad/lens-work/agents/lens.agent.yaml` -- Structured mirror of the shell `[IF]` route.
- `lens.core/_bmad/lens-work/scripts/tests/test-phase-conductor-contracts.py` -- Prompt contract coverage to extend for the new routing guarantees.
- `lens.core/_bmad/lens-work/scripts/install.py` -- Installer metadata for generated prompt descriptions.
- `lens.core/_bmad/lens-work/_module-installer/installer.js` -- Parallel installer metadata for prompt descriptions.

## Tasks & Acceptance

**Execution:**
- [x] `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md` -- add missing-domain/service inspection and explicit delegation to `lens-new-project` for bootstrap cases while preserving the direct feature-init path for existing containers -- this fixes the user-facing entrypoint drift at the source.
- [x] `lens.core/_bmad/lens-work/prompts/lens-init-feature.prompt.md` -- delegate to the canonical `lens-new-feature` router instead of going straight to the skill -- this prevents alias drift.
- [x] `lens.core/_bmad/lens-work/agents/lens.agent.md` and `lens.core/_bmad/lens-work/agents/lens.agent.yaml` -- point the `[IF]` route at the canonical router -- this keeps shell usage consistent with slash-command usage.
- [x] `lens.core/_bmad/lens-work/scripts/tests/test-phase-conductor-contracts.py` -- add prompt and route assertions for missing-container delegation, alias consolidation, and shell routing -- this locks the workflow contract in CI.
- [x] `lens.core/_bmad/lens-work/scripts/install.py` and `lens.core/_bmad/lens-work/_module-installer/installer.js` -- refresh the generated prompt description for `lens-new-feature` if the prompt meaning changes -- this keeps adapter metadata aligned with runtime behavior.

**Acceptance Criteria:**
- Given a workspace where the requested domain and service already exist, when the user runs `/lens-new-feature`, then the prompt still follows the direct feature-init flow and does not route through project bootstrap.
- Given a workspace where the requested domain or service does not exist, when the user runs `/lens-new-feature`, then the prompt requires governance inspection and delegates through the existing new-project bootstrap flow instead of treating feature `create` as sufficient bootstrap.
- Given a user invokes `/lens-init-feature` or the shell `[IF]` entry, when the route resolves, then it lands on the same canonical prompt contract as `/lens-new-feature`.
- Given the prompt contract test suite runs, when these routing surfaces drift, then the suite fails on the missing delegation or alias inconsistency.

## Design Notes

The key design constraint is already encoded in `init-feature-ops.py`: `ensure_container_markers()` only creates marker YAMLs, while `cmd_create_domain()` and `cmd_create_service()` are the only supported paths that create constitutions, `.gitkeep` scaffolds, and personal context. The safest implementation is therefore to move responsibility into prompt routing, not to expand `cmd_create()`.

## Verification

**Commands:**
- `cd /home/cweber/github/Lens.Core.Control/lens.core && uv run --with pytest --with pyyaml pytest _bmad/lens-work/scripts/tests/test-phase-conductor-contracts.py::test_init_feature_handoff_surfaces_route_through_next -q` -- expected: updated alias and shell routing contract passes.
- `cd /home/cweber/github/Lens.Core.Control/lens.core && uv run --with pytest --with pyyaml pytest _bmad/lens-work/scripts/tests/test-phase-conductor-contracts.py::test_feature_init_prompt_requires_automatic_governance_git -q` -- expected: updated container-aware prompt contract passes.
- `cd /home/cweber/github/Lens.Core.Control/lens.core && uv run --script _bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-init-feature-ops.py` -- expected: existing init-feature script regressions still pass.
- `cd /home/cweber/github/Lens.Core.Control/lens.core && uv run --with pytest pytest _bmad/lens-work/scripts/tests/test-install.py -q` -- expected: installer metadata coverage passes.

## Suggested Review Order

**Router entrypoint**

- Canonical router now inspects container state before selecting a workflow.
	[`lens-new-feature.prompt.md:8`](../../lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md#L8)

- Direct feature init is now explicitly gated behind existing containers.
	[`lens-new-feature.prompt.md:13`](../../lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md#L13)

**Delegated bootstrap behavior**

- Delegated bootstrap preserves inspected container state instead of re-asking.
	[`lens-new-project.prompt.md:8`](../../lens.core/_bmad/lens-work/prompts/lens-new-project.prompt.md#L8)

- Feature-driven bootstrap now defers repo provisioning unless explicitly requested.
	[`lens-new-project.prompt.md:19`](../../lens.core/_bmad/lens-work/prompts/lens-new-project.prompt.md#L19)

**Alias and shell routing**

- Alias prompt now resolves through the canonical router.
	[`lens-init-feature.prompt.md:10`](../../lens.core/_bmad/lens-work/prompts/lens-init-feature.prompt.md#L10)

- Shell rules now allow prompt delegates as first-class routes.
	[`lens.agent.md:36`](../../lens.core/_bmad/lens-work/agents/lens.agent.md#L36)

- Shell IF now lands on the same prompt contract.
	[`lens.agent.md:50`](../../lens.core/_bmad/lens-work/agents/lens.agent.md#L50)

- Structured agent metadata mirrors the same IF route.
	[`lens.agent.yaml:26`](../../lens.core/_bmad/lens-work/agents/lens.agent.yaml#L26)

**Safety nets**

- Contract tests lock the delegated bootstrap and alias-routing guarantees.
	[`test-phase-conductor-contracts.py:198`](../../lens.core/_bmad/lens-work/scripts/tests/test-phase-conductor-contracts.py#L198)

- Installer metadata now advertises the container-aware entrypoint.
	[`install.py:219`](../../lens.core/_bmad/lens-work/scripts/install.py#L219)

- Module installer keeps generated adapters aligned with the prompt contract.
	[`installer.js:316`](../../lens.core/_bmad/lens-work/_module-installer/installer.js#L316)