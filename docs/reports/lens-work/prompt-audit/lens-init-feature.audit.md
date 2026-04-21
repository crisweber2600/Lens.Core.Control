# lens-init-feature Prompt Audit

## Purpose Summary
- `.github/prompts/lens-init-feature.prompt.md` is a thin adapter stub, not the behavioral source of truth.
- It delegates to `lens.core/_bmad/lens-work/prompts/lens-init-feature.prompt.md`, which is itself a second stub.
- Effective behavior is defined by the Lens-local `bmad-lens-init-feature` skill and its `references/init-feature.md` flow contract.
- That downstream contract owns feature initialization for Lens: gather minimal inputs, derive `featureId` and `featureSlug`, require explicit track selection, create the control-repo 2-branch topology, publish governance artifacts on `main`, and hand off to the lifecycle router using the returned `starting_phase` and `recommended_command`.

## BMAD Skill Mapping
- Prompt chain:
  - `.github/prompts/lens-init-feature.prompt.md`
  - `lens.core/_bmad/lens-work/prompts/lens-init-feature.prompt.md`
  - `bmad-lens-init-feature` skill
- Lens module registration is explicit:
  - `lens.core/_bmad/lens-work/module.yaml` registers `bmad-lens-init-feature` as a Lens skill and also ships `lens-init-feature.prompt.md` as a prompt surface plus adapter stubs for GitHub Copilot and other IDEs.
- Lens command/help mapping is explicit:
  - `lens.core/_bmad/lens-work/module-help.csv` maps the skill to display `init-feature`, menu code `IF`, action `create`, phase `anytime`, and an intended handoff to `bmad-lens-next:suggest`.
  - The same help surface also exposes `pull-context` as a secondary action on the same skill, showing that feature creation and immediate context loading are intentionally bundled under one command family.
- Global BMAD registries are not authoritative for this prompt:
  - `lens.core/_bmad/_config/skill-manifest.csv` does not contain `bmad-lens-init-feature`.
  - `lens.core/_bmad/_config/bmad-help.csv` does not contain `bmad-lens-init-feature`.
  - That absence is consistent with this being a Lens module-local orchestration skill rather than a global BMAD core or BMM skill.

## Lifecycle Fit
- Fit is strong because init-feature is the lifecycle entrypoint, not just a repository bootstrap helper.
- The downstream skill explicitly requires progressive disclosure, explicit track choice, atomic feature visibility in `feature-index.yaml`, and post-create routing through the next recommended command instead of hardcoding a planning step.
- `lifecycle.yaml` confirms why that matters:
  - canonical phase order is `preplan -> businessplan -> techplan -> finalizeplan`
  - tracks define different `start_phase` values, so init-feature must return the correct lifecycle start rather than assuming one fixed next step
  - some tracks such as `express` and `hotfix-express` require constitution permission, which further supports the skill's rule that track defaults may be suggested but never silently applied
- `module-help.csv` marks the command `anytime`, which is slightly broader than its practical role. In practice it is a front-door operation that initializes a feature and then routes into the lifecycle, with follow-on edges into `next`, `quickplan`, `preplan`, `expressplan`, and `target-repo`.
- The reference flow strengthens this fit by specifying returned values such as `planning_pr_created`, `starting_phase`, `recommended_command`, and `router_command`, and by differentiating `express` from other tracks through deferred planning PR behavior.

## Evidence Refs
- Control prompt stub:
  - `.github/prompts/lens-init-feature.prompt.md:5`
  - `.github/prompts/lens-init-feature.prompt.md:10`
- Release prompt stub:
  - `lens.core/_bmad/lens-work/prompts/lens-init-feature.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-init-feature.prompt.md:10`
- Lens module registration and shipped prompt surfaces:
  - `lens.core/_bmad/lens-work/module.yaml:49`
  - `lens.core/_bmad/lens-work/module.yaml:50`
  - `lens.core/_bmad/lens-work/module.yaml:214`
  - `lens.core/_bmad/lens-work/module.yaml:280`
- Lens operational help:
  - `lens.core/_bmad/lens-work/module-help.csv:7`
  - `lens.core/_bmad/lens-work/module-help.csv:9`
  - `lens.core/_bmad/lens-work/module-help.csv:10`
  - `lens.core/_bmad/lens-work/module-help.csv:48`
  - `lens.core/_bmad/lens-work/module-help.csv:53`
- Lifecycle contract:
  - `lens.core/_bmad/lens-work/lifecycle.yaml:120`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:248`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:256`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:265`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:286`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:303`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:313`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:314`
- Real behavior contract in source prompt/skill chain:
  - `TargetProjects/lens.core/src/Lens.Core.Src/.github/prompts/lens-init-feature.prompt.md`
  - `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/prompts/lens-init-feature.prompt.md`
  - `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md`
  - `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/skills/bmad-lens-init-feature/references/init-feature.md`
- Global registry absence reviewed in scope:
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: **High**.
- High confidence on the prompt-routing chain, Lens-local skill mapping, module registration, and lifecycle alignment because those are directly declared in the prompt stubs, `module.yaml`, `module-help.csv`, `lifecycle.yaml`, and the source skill/reference contract.
- Slightly lower confidence on runtime edge-case behavior because this audit did not execute `init-feature-ops.py`; it validated contract surfaces rather than running a feature initialization end to end.

## Gaps
- Double-stub indirection:
  - Neither prompt layer contains operational guidance, so prompt-only review would miss the actual creation contract unless the auditor follows the chain into the skill and reference file.
- Registry discoverability split:
  - The required global registries in scope do not list `bmad-lens-init-feature`, so tooling that treats `_config/skill-manifest.csv` or `_config/bmad-help.csv` as exhaustive will under-report this prompt's true implementation target.
- Prompt-level lifecycle guidance is implicit, not explicit:
  - The stubs do not explain why track choice matters, how `starting_phase` is determined, or why `/next` is the preferred post-init router.
- `anytime` is a loose phase label:
  - The help surface is discoverability-friendly, but it obscures that init-feature is effectively the lifecycle entry gate and that downstream behavior changes materially by track.