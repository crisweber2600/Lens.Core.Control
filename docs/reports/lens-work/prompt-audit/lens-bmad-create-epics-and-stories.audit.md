# lens-bmad-create-epics-and-stories Prompt Audit

## Purpose Summary
- `.github/prompts/lens-bmad-create-epics-and-stories.prompt.md` is a control-repo stub that runs shared `light-preflight.py` and then delegates to the release-module prompt.
- The release prompt (`lens.core/_bmad/lens-work/prompts/lens-bmad-create-epics-and-stories.prompt.md`) is also a stub and delegates execution to `bmad-lens-bmad-skill` with `--skill bmad-create-epics-and-stories`.
- Effective behavior is therefore governed by the Lens BMAD wrapper contract (`bmad-lens-bmad-skill`) plus the downstream BMAD skill workflow (`bmad-create-epics-and-stories` in BMM solutioning).

## BMAD Skill Mapping
- Global skill registry confirms canonical identity: `bmad-create-epics-and-stories` maps to `_bmad/bmm/3-solutioning/bmad-create-epics-and-stories/SKILL.md` with purpose "Break requirements into epics and user stories."
- Global help surfaces this skill as `Create Epics and Stories (CE)` in phase `3-solutioning`, with `planning_artifacts` output and dependency on `bmad-create-architecture`.
- Lens module wiring includes prompt registration for `lens-bmad-create-epics-and-stories.prompt.md` and adapter stub generation into `.github/prompts/`.
- Lens operational help maps this command via `BES` to `finalizeplan`, output location `feature.yaml.docs.path`, and output "epics and stories".
- Lens skill registry and wrapper policy align this delegation as `contextMode=feature-required`, `outputMode=planning-docs`, `phaseHints=[finalizeplan]`, with entry path to the BMM solutioning skill.

## Lifecycle Fit
- Lifecycle fit is strong for Lens Next: `finalizeplan` explicitly requires downstream planning outputs including `epics`, `stories`, and `implementation-readiness`, then bundles them before PR handoff.
- The `express` track also routes into `finalizeplan`, so this command remains phase-correct for both standard and accelerated planning routes.
- Wrapper write-boundary rules for `planning-docs` enforce writing to feature docs scope (`feature.yaml.docs.path`) and block direct governance authoring, matching lifecycle discipline that artifacts land in control-repo docs first.
- The downstream BMAD workflow objective (transform PRD + architecture into actionable stories with acceptance criteria) is semantically aligned with finalizeplan's artifact production contract.

## Evidence Refs
- Control prompt stub + preflight + delegation:
  - `.github/prompts/lens-bmad-create-epics-and-stories.prompt.md:5`
  - `.github/prompts/lens-bmad-create-epics-and-stories.prompt.md:7`
  - `.github/prompts/lens-bmad-create-epics-and-stories.prompt.md:14`
  - `.github/prompts/lens-bmad-create-epics-and-stories.prompt.md:18`
- Release prompt delegation:
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-create-epics-and-stories.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-create-epics-and-stories.prompt.md:10`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-create-epics-and-stories.prompt.md:11`
- Required registry/help/module/lifecycle sources:
  - `lens.core/_bmad/_config/skill-manifest.csv:30`
  - `lens.core/_bmad/_config/bmad-help.csv:12`
  - `lens.core/_bmad/_config/bmad-help.csv:8`
  - `lens.core/_bmad/lens-work/module.yaml:248`
  - `lens.core/_bmad/lens-work/module.yaml:271`
  - `lens.core/_bmad/lens-work/module.yaml:313`
  - `lens.core/_bmad/lens-work/module-help.csv:63`
  - `lens.core/_bmad/lens-work/module-help.csv:52`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:166`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:172`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:174`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:175`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:176`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:178`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:198`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:309`
- Relevant prompt/skill contracts:
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:101`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:106`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:107`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:108`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:109`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:31`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:32`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:62`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:70`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:133`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-create-epics-and-stories/SKILL.md:3`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-create-epics-and-stories/workflow.md:1`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-create-epics-and-stories/workflow.md:3`

## Confidence
- Overall: **High** for skill mapping and lifecycle alignment.
- Rationale: all required metadata surfaces (manifest/help/module/module-help/lifecycle) consistently resolve to the same delegation chain and output intent.
- Residual uncertainty: prompt files are intentionally thin stubs, so runtime behavior quality depends on downstream wrapper and workflow contracts, not prompt-local logic.

## Gaps
- Double-stub indirection (control prompt -> release prompt -> wrapper skill) keeps prompts minimal but increases drift risk across layers.
- Phase semantics differ across surfaces: global BMAD help lists `bmad-create-epics-and-stories` in `3-solutioning`, while Lens module-help/registry bind it to `finalizeplan`; this is probably intentional but can confuse discoverability.
- Preflight behavior is only declared in the control stub (`light-preflight.py`) and not repeated in the release prompt, creating asymmetric assumptions across entry points.
- The delegated BMAD workflow is highly interactive step-file execution; the prompt itself does not expose expected artifact naming/schema beyond delegation, so operator expectations must come from lifecycle and wrapper documentation.