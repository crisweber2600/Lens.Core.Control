# lens-move-feature Prompt Audit

## Purpose Summary
- `.github/prompts/lens-move-feature.prompt.md` is a control-surface stub that delegates execution to the release prompt/skill path rather than defining behavior directly.
- The release prompt at `lens.core/_bmad/lens-work/prompts/lens-move-feature.prompt.md` is also a stub; effective behavior is in `bmad-lens-move-feature` skill assets.
- The functional purpose is feature relocation across domain/service boundaries with strict preconditions, explicit confirmation, metadata updates, and cross-reference patching.

## BMAD Skill Mapping
- Prompt chain:
  - `.github/prompts/lens-move-feature.prompt.md`
  - `lens.core/_bmad/lens-work/prompts/lens-move-feature.prompt.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/SKILL.md`
- Module registration is explicit in Lens module config:
  - `module.yaml` registers `bmad-lens-move-feature` as a Lens skill and ships `lens-move-feature.prompt.md` in prompt inventory.
- Command-surface mapping is explicit in Lens help registry:
  - `module-help.csv` maps `bmad-lens-move-feature` to `move-feature` (`MF`) and `list-references` (`LR`), with `patch-references` (`PH`) also mapped to the same skill.
- Skill/script contract mapping:
  - `SKILL.md` requires interactive confirmation before execution.
  - `move-feature-ops.py` implements `validate`, `move`, and `patch-references` operations matching the skill and module-help contract.
- Global BMAD registries do not contain this Lens-local skill mapping:
  - No `bmad-lens-move-feature` entry in `_config/skill-manifest.csv` or `_config/bmad-help.csv`; discoverability is module-local.

## Lifecycle Fit
- Lifecycle compatibility is good for an operational utility command:
  - `module-help.csv` marks move-feature operations as `anytime`, which is consistent with relocation being a cross-cutting maintenance action rather than a phase-conductor.
  - Canonical lifecycle phase progression remains `preplan -> businessplan -> techplan -> finalizeplan`, and move-feature does not attempt to alter that progression.
- Governance/quality fit is strong:
  - Skill principles enforce confirmation gating and all-reference patching.
  - Move is blocked when stories are `in-progress` or `done`, which protects lifecycle continuity once execution has started.
- Fit caveat:
  - Because both prompt layers are stubs, lifecycle-safe behavior is guaranteed only by downstream skill/script implementation, not by prompt text itself.

## Evidence Refs
- Control prompt stub delegation:
  - `.github/prompts/lens-move-feature.prompt.md:5`
  - `.github/prompts/lens-move-feature.prompt.md:10`
- Release prompt stub delegation:
  - `lens.core/_bmad/lens-work/prompts/lens-move-feature.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-move-feature.prompt.md:10`
- Lens module registration (`module.yaml`):
  - `lens.core/_bmad/lens-work/module.yaml:82`
  - `lens.core/_bmad/lens-work/module.yaml:83`
  - `lens.core/_bmad/lens-work/module.yaml:230`
  - `lens.core/_bmad/lens-work/module.yaml:295`
- Lens command/help mapping (`module-help.csv`):
  - `lens.core/_bmad/lens-work/module-help.csv:28`
  - `lens.core/_bmad/lens-work/module-help.csv:29`
  - `lens.core/_bmad/lens-work/module-help.csv:47`
- Lifecycle contract alignment:
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:243`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:69`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:88`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:89`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:181`
- Skill-level behavior and confirmation requirements:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/SKILL.md:2`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/SKILL.md:3`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/SKILL.md:14`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/SKILL.md:42`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/SKILL.md:68`
- Reference workflow details:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/references/move-feature.md:3`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/references/move-feature.md:16`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/references/move-feature.md:48`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/references/move-feature.md:124`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/references/notify-dependents.md:24`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/references/notify-dependents.md:57`
- Scripted enforcement:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/scripts/move-feature-ops.py:29`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/scripts/move-feature-ops.py:167`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/scripts/move-feature-ops.py:226`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/scripts/move-feature-ops.py:321`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-move-feature/scripts/move-feature-ops.py:334`
- Global registry absence check:
  - `lens.core/_bmad/_config/skill-manifest.csv` (no `bmad-lens-move-feature` match)
  - `lens.core/_bmad/_config/bmad-help.csv` (no `bmad-lens-move-feature` match)

## Confidence
- Overall: **High**.
- High confidence on routing, mapping, and lifecycle positioning because they are explicitly declared in prompt stubs, module config, command help, and lifecycle contract.
- Medium-high confidence on runtime safety because script-level checks for blockers and patching were inspected, but not executed in this audit.

## Gaps
- Stub indirection gap:
  - Both prompt layers are wrappers, so prompt-local text does not carry operational safeguards by itself.
- Discoverability split:
  - This is a Lens-local skill; global BMAD registries do not expose it, which can mislead audits that only inspect `_config` manifests.
- Lifecycle guidance discoverability:
  - `module-help.csv` marks the command `anytime`, but move operations are operationally safest before dev execution begins; this nuance is expressed in skill/script blockers, not lifecycle metadata.