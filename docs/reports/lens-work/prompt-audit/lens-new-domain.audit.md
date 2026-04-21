# lens-new-domain Prompt Audit

## Purpose Summary
- [/.github/prompts/lens-new-domain.prompt.md](.github/prompts/lens-new-domain.prompt.md#L5) is a control-layer stub that delegates behavior to the release prompt after running shared light preflight.
- The executable contract lives in [lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md](lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md#L1), which defines domain initialization as governance/container scaffolding, not feature initialization.
- The release prompt explicitly routes to [lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md#L2) and requires the `create-domain` subcommand with governance git execution.

## BMAD Skill Mapping
- Prompt chain:
  - [/.github/prompts/lens-new-domain.prompt.md](.github/prompts/lens-new-domain.prompt.md#L5)
  - [lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md](lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md#L4)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md#L74)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py](lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py#L1125)
- Module registration is consistent with this mapping:
  - [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml#L49) registers `bmad-lens-init-feature`.
  - [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml#L215) ships `lens-new-domain.prompt.md`.
  - [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml#L281) exports the control stub path under adapter `stub_prompts`.
- Module help is feature-centric, not container-subcommand-centric:
  - [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv#L7) lists `init-feature` action `create` for features.
  - There is no explicit `new-domain` or `create-domain` row in [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv).
- Global BMAD registries do not index Lens-local command surfaces for this path:
  - No `lens-new-domain`/`bmad-lens-init-feature` entries in [lens.core/_bmad/_config/skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv).
  - No `lens-new-domain`/`bmad-lens-init-feature` entries in [lens.core/_bmad/_config/bmad-help.csv](lens.core/_bmad/_config/bmad-help.csv).

## Lifecycle Fit
- Fit is intentional but indirect: `lens-new-domain` is lifecycle bootstrap scaffolding that prepares governance/context prerequisites rather than entering a planning phase.
- Lifecycle phase contract starts at planning phases (`preplan`, `businessplan`, `techplan`, `finalizeplan`) and formal tracks anchored to those phases, with no dedicated domain-bootstrap phase:
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L109)
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L129)
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L148)
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L166)
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L238)
- The prompt and skill pair correctly enforce governance-only domain setup:
  - Prompt requires no feature branch/feature.yaml creation for this flow in [lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md](lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md#L13).
  - Script implementation writes domain marker/constitution and optional scaffolds via `cmd_create_domain` in [lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py](lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py#L1125).
  - Script persists personal context with `service = null` for domain-only context in [lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py](lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py#L1231).

## Evidence Refs
- Control stub prompt:
  - [/.github/prompts/lens-new-domain.prompt.md](.github/prompts/lens-new-domain.prompt.md#L1)
  - [/.github/prompts/lens-new-domain.prompt.md](.github/prompts/lens-new-domain.prompt.md#L5)
  - [/.github/prompts/lens-new-domain.prompt.md](.github/prompts/lens-new-domain.prompt.md#L18)
- Release prompt contract:
  - [lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md](lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md#L1)
  - [lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md](lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md#L4)
  - [lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md](lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md#L9)
  - [lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md](lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md#L11)
  - [lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md](lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md#L13)
  - [lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md](lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md#L14)
- Requested module/config sources:
  - [lens.core/_bmad/_config/skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv)
  - [lens.core/_bmad/_config/bmad-help.csv](lens.core/_bmad/_config/bmad-help.csv)
  - [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml#L49)
  - [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml#L215)
  - [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml#L281)
  - [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv#L7)
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L109)
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L129)
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L148)
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L166)
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L238)
- Relevant skill/script files:
  - [lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md#L61)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md#L74)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md#L194)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md#L219)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md#L229)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py](lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py#L1125)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py](lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py#L1231)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py](lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py#L1656)

## Confidence
- Overall: High.
- Prompt purpose and routing confidence is high because both prompt layers and module registration consistently point to the same skill and subcommand path.
- Lifecycle-fit confidence is medium-high: the contract clearly shows phase coverage and confirms domain bootstrap sits outside phase progression, but this audit is document/contract-based and does not execute a live `create-domain` run.

## Gaps
- Discoverability gap: [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv#L7) documents `init-feature` but not `create-domain`, so users rely on prompt entry points rather than help-table command rows for domain scaffolding.
- Registry split gap: global registries [lens.core/_bmad/_config/skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv) and [lens.core/_bmad/_config/bmad-help.csv](lens.core/_bmad/_config/bmad-help.csv) do not surface this Lens-local mapping, which can mislead tooling that assumes global registry completeness.
- Lifecycle labeling gap: `new-domain` is operationally lifecycle-critical bootstrap work but is not represented as a lifecycle phase/track unit in [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L238), so governance prerequisite failures can appear "pre-lifecycle" rather than first-class lifecycle blockers.