---
title: 'Lens v5 upgrade foundation'
type: 'feature'
created: '2026-04-16T00:00:00Z'
status: 'in-progress'
context:
  - '../session/plan.md'
baseline_commit: 'a5e62bd097a4b7588accf9bee0cd04dc960b4fd5'
---

<frozen-after-approval reason="human-owned intent — do not modify unless human renegotiates">

## Intent

**Problem:** The planned Lens terminology rename depends on a first-party migration path, but the repository currently has no executable `lens-upgrade` implementation for a `4 -> 5` schema change. Governance data, local `.lens` state, and lifecycle metadata would all drift or break if the rename landed without an upgrade tool.

**Approach:** Start with the migration foundation instead of the full rename rollout. Define the `4 -> 5` lifecycle migration contract, add an executable `bmad-lens-upgrade` script that can dry-run and apply the core schema rewrite, and cover the main data transforms with focused tests.

## Boundaries & Constraints

**Always:** Keep this slice scoped to migration infrastructure. Reuse the repo’s existing migration patterns where practical. Support dry-run before apply. Preserve timestamps, ownership, and existing document files while rewriting machine-readable schema and paths.

**Ask First:** If the v5 migration needs to rewrite arbitrary authored markdown content beyond deterministic stubs or machine-readable YAML. If a required target schema name conflicts with another in-repo contract discovered during implementation.

**Never:** Do not rename the entire prompt and command surface in this slice. Do not add backward-compatibility aliases as a substitute for a migration. Do not auto-run remote git or release operations.

## I/O & Edge-Case Matrix

| Scenario | Input / State | Expected Output / Behavior | Error Handling |
|----------|--------------|---------------------------|----------------|
| Dry-run upgrade | v4 control repo with `.lens` state and a governance repo containing `feature-index.yaml` plus `features/.../feature.yaml` | Script reports a `4 -> 5` plan covering local state, index rename, container marker rename, and feature-to-milestone YAML transforms without modifying disk | Returns a fail result when required inputs are missing or target v5 artifacts already exist |
| Live upgrade | Same v4 fixture, executed without `--dry-run` | Script writes v5 local state, renames governance tree to `milestones/...`, rewrites YAML keys to `workstream/project/milestoneId/checkpoints`, and emits a machine-readable summary | Stops before partial overwrite when conflicting target files or directories exist |

</frozen-after-approval>

## Code Map

- `../TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/lifecycle.yaml` -- Declares lifecycle schema version and migration descriptors; this is the canonical place to add the `4 -> 5` contract.
- `../TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md` -- User-facing upgrade entrypoint; currently lacks an executable script reference.
- `../TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/skills/bmad-lens-migrate/scripts/migrate-ops.py` -- Existing dry-run/apply migration model worth mirroring for report shape and safety behavior.
- `../TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/skills/bmad-lens-feature-yaml/assets/feature-template.yaml` -- Current v4 feature schema to map into the new milestone schema.
- `../TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py` -- Defines current governance marker and feature YAML shapes that the upgrade must transform.

## Tasks & Acceptance

**Execution:**
- [x] `../TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/lifecycle.yaml` -- add a breaking `4 -> 5` migration descriptor for the terminology rename foundation -- establishes the canonical upgrade contract.
- [x] `../TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md` -- document the new executable upgrade path and the v5 rename scope -- keeps the user-facing skill aligned with the code.
- [x] `../TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/skills/bmad-lens-upgrade/scripts/upgrade-ops.py` -- implement dry-run and apply support for the core local-state and governance-schema migration -- provides the first working migration engine for the rename.
- [x] `../TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/skills/bmad-lens-upgrade/scripts/tests/test-upgrade-ops.py` -- add coverage for dry-run, apply, and conflict handling on v4 fixtures -- proves the migration path works before broader rollout.

**Acceptance Criteria:**
- Given a v4 fixture workspace and governance repo, when `upgrade-ops.py` runs with `--from 4 --to 5 --dry-run`, then it reports the planned local and governance renames without modifying files.
- Given the same v4 fixture, when `upgrade-ops.py` runs live, then `.lens` state is rewritten for `workstream/project`, `feature-index.yaml` becomes `milestone-index.yaml`, `features/.../feature.yaml` becomes `milestones/.../milestone.yaml`, and lifecycle `milestones` keys are rewritten to `checkpoints`.
- Given an existing v5 target artifact such as `milestone-index.yaml` or `milestones/`, when the upgrade runs live, then it fails before overwriting data and returns a clear conflict message.

## Spec Change Log

## Design Notes

Use a deterministic file-system migration rather than an in-place alias layer. The upgrade script should stage a complete transformed governance tree under temporary paths, validate target conflicts first, then swap the migrated files into place so partial upgrades are easy to detect and test.

## Verification

**Commands:**
- `cd TargetProjects/lens.core/src/Lens.Core.Src && uv run --with pytest --with pyyaml pytest _bmad/lens-work/skills/bmad-lens-upgrade/scripts/tests/test-upgrade-ops.py -q` -- expected: new upgrade tests pass.
- `cd TargetProjects/lens.core/src/Lens.Core.Src && uv run --with pytest --with pyyaml pytest _bmad/lens-work/skills/bmad-lens-migrate/scripts/tests/test-migrate-ops.py -q` -- expected: legacy migration coverage still passes after introducing the upgrade foundation.