# lens-finalizeplan Prompt Audit

## Purpose Summary
- [.github/prompts/lens-finalizeplan.prompt.md](.github/prompts/lens-finalizeplan.prompt.md#L5) is a control-repo stub, not the operative workflow. Its job is to force the shared lightweight preflight and delegate to the release prompt at [line 18](.github/prompts/lens-finalizeplan.prompt.md#L18).
- The release prompt is also only a stub and forwards execution to the real phase contract in [lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L2) via [line 10](lens.core/_bmad/lens-work/prompts/lens-finalizeplan.prompt.md#L10).
- Effective behavior therefore lives in the FinalizePlan skill: run the final planning review, confirm the `{featureId}-plan -> {featureId}` PR path, then delegate the downstream planning bundle and open the final `{featureId} -> main` PR before handing off to `/dev` ([overview](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L8), [step 1](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L56), [step 2](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L64), [step 3](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L71)).

## BMAD Skill Mapping
- FinalizePlan itself is a first-class Lens phase skill exposed in module metadata as [bmad-lens-finalizeplan](lens.core/_bmad/lens-work/module.yaml#L114), surfaced as prompt [lens-finalizeplan.prompt.md](lens.core/_bmad/lens-work/module.yaml#L225), and presented to operators as [menu code FZ](lens.core/_bmad/lens-work/module-help.csv#L52).
- The FinalizePlan skill does not author epics, readiness, sprint planning, or story files directly. It explicitly delegates that bundle in step 3 through `bmad-lens-bmad-skill` in this fixed order: [bmad-create-epics-and-stories](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L77), [bmad-check-implementation-readiness](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L78), [bmad-sprint-planning](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L79), and [bmad-create-story](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L80).
- Global BMAD registry confirms those downstream skills exist natively as [implementation readiness](lens.core/_bmad/_config/skill-manifest.csv#L28), [epics and stories](lens.core/_bmad/_config/skill-manifest.csv#L30), [create story](lens.core/_bmad/_config/skill-manifest.csv#L38), and [sprint planning](lens.core/_bmad/_config/skill-manifest.csv#L43).
- Global BMAD help places them in their native BMAD phases rather than Lens FinalizePlan: [IR in 3-solutioning](lens.core/_bmad/_config/bmad-help.csv#L8), [CE in 3-solutioning](lens.core/_bmad/_config/bmad-help.csv#L12), [CS in 4-implementation](lens.core/_bmad/_config/bmad-help.csv#L14), and [SP in 4-implementation](lens.core/_bmad/_config/bmad-help.csv#L27).
- Lens intentionally remaps those native skills into FinalizePlan through module-help wrapper commands: [BES](lens.core/_bmad/lens-work/module-help.csv#L63), [BIR](lens.core/_bmad/lens-work/module-help.csv#L64), [BSP](lens.core/_bmad/lens-work/module-help.csv#L65), and [BST](lens.core/_bmad/lens-work/module-help.csv#L66).
- That remapping is also codified in the wrapper contract itself, which lists all four downstream skills under FinalizePlan in the registered-skills table ([epics/stories](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L133), [implementation readiness](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L134), [sprint planning](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L135), [create story](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L136)) and in registry phase hints ([epics/stories](lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json#L109), [implementation readiness](lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json#L121), [sprint planning](lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json#L133), [create story](lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json#L145)).

## Lifecycle Fit
- Lifecycle fit is strong. `finalizeplan` is defined as the planning consolidation milestone with review, governance cross-check, downstream bundle preparation, and PR handoff before dev ([milestone role](lens.core/_bmad/lens-work/lifecycle.yaml#L83), [description](lens.core/_bmad/lens-work/lifecycle.yaml#L84), [entry gate](lens.core/_bmad/lens-work/lifecycle.yaml#L86)).
- The lifecycle phase contract names the exact artifact family FinalizePlan is expected to produce: [review-report](lens.core/_bmad/lens-work/lifecycle.yaml#L168), [epics](lens.core/_bmad/lens-work/lifecycle.yaml#L169), [stories](lens.core/_bmad/lens-work/lifecycle.yaml#L170), [implementation-readiness](lens.core/_bmad/lens-work/lifecycle.yaml#L171), [sprint-status](lens.core/_bmad/lens-work/lifecycle.yaml#L172), and [story-files](lens.core/_bmad/lens-work/lifecycle.yaml#L173).
- FinalizePlan is the last canonical planning phase in [phase_order](lens.core/_bmad/lens-work/lifecycle.yaml#L238) and explicitly [auto-advances to `/dev`](lens.core/_bmad/lens-work/lifecycle.yaml#L181). Dev-ready is separately [constitution-gated](lens.core/_bmad/lens-work/lifecycle.yaml#L91), which matches the prompt’s role as the final planning gate rather than an implementation phase.
- Lens command topology reinforces that interpretation: [bmad-lens-dev](lens.core/_bmad/lens-work/module-help.csv#L54) starts only after `bmad-lens-finalizeplan:plan` completes.
- The FinalizePlan skill is aligned with the lifecycle execution contract rather than merely adjacent to it. The skill requires review before bundling ([principle](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L31)), requires the planning PR path before bundling ([principle](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L33)), and ends by reporting [next action `/dev`](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L84).

## Evidence Refs
- Prompt chain
  - [Control prompt stub](.github/prompts/lens-finalizeplan.prompt.md#L5)
  - [Control prompt preflight handoff](.github/prompts/lens-finalizeplan.prompt.md#L12)
  - [Control prompt release delegation](.github/prompts/lens-finalizeplan.prompt.md#L18)
  - [Release prompt stub](lens.core/_bmad/lens-work/prompts/lens-finalizeplan.prompt.md#L5)
  - [Release prompt skill delegation](lens.core/_bmad/lens-work/prompts/lens-finalizeplan.prompt.md#L10)
- Required registry/help/module/lifecycle sources
  - [skill-manifest implementation readiness](lens.core/_bmad/_config/skill-manifest.csv#L28)
  - [skill-manifest epics and stories](lens.core/_bmad/_config/skill-manifest.csv#L30)
  - [skill-manifest create story](lens.core/_bmad/_config/skill-manifest.csv#L38)
  - [skill-manifest sprint planning](lens.core/_bmad/_config/skill-manifest.csv#L43)
  - [bmad-help implementation readiness](lens.core/_bmad/_config/bmad-help.csv#L8)
  - [bmad-help epics and stories](lens.core/_bmad/_config/bmad-help.csv#L12)
  - [bmad-help create story](lens.core/_bmad/_config/bmad-help.csv#L14)
  - [bmad-help sprint planning](lens.core/_bmad/_config/bmad-help.csv#L27)
  - [module skill bmad-lens-finalizeplan](lens.core/_bmad/lens-work/module.yaml#L114)
  - [module prompt lens-finalizeplan](lens.core/_bmad/lens-work/module.yaml#L225)
  - [module wrapper prompt epics/stories](lens.core/_bmad/lens-work/module.yaml#L248)
  - [module wrapper prompt implementation readiness](lens.core/_bmad/lens-work/module.yaml#L249)
  - [module wrapper prompt sprint planning](lens.core/_bmad/lens-work/module.yaml#L250)
  - [module wrapper prompt create story](lens.core/_bmad/lens-work/module.yaml#L251)
  - [module-help finalizeplan FZ](lens.core/_bmad/lens-work/module-help.csv#L52)
  - [module-help dev after finalizeplan](lens.core/_bmad/lens-work/module-help.csv#L54)
  - [module-help BES](lens.core/_bmad/lens-work/module-help.csv#L63)
  - [module-help BIR](lens.core/_bmad/lens-work/module-help.csv#L64)
  - [module-help BSP](lens.core/_bmad/lens-work/module-help.csv#L65)
  - [module-help BST](lens.core/_bmad/lens-work/module-help.csv#L66)
  - [lifecycle finalizeplan milestone](lens.core/_bmad/lens-work/lifecycle.yaml#L83)
  - [lifecycle dev-ready constitution gate](lens.core/_bmad/lens-work/lifecycle.yaml#L91)
  - [lifecycle finalizeplan phase block](lens.core/_bmad/lens-work/lifecycle.yaml#L157)
  - [lifecycle finalizeplan outputs](lens.core/_bmad/lens-work/lifecycle.yaml#L168)
  - [lifecycle finalizeplan auto-advance to dev](lens.core/_bmad/lens-work/lifecycle.yaml#L181)
  - [lifecycle phase_order](lens.core/_bmad/lens-work/lifecycle.yaml#L238)
- Relevant prompt/skill files driving actual behavior
  - [FinalizePlan skill overview](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L8)
  - [FinalizePlan identity and conductor role](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L16)
  - [FinalizePlan no direct wrapper-owned artifact authorship](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L18)
  - [FinalizePlan step-1 review contract](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L56)
  - [FinalizePlan step-2 PR readiness contract](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L64)
  - [FinalizePlan step-3 bundle contract](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L71)
  - [FinalizePlan artifact table](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L88)
  - [Wrapper feature docs authority](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L33)
  - [Wrapper write-boundary enforcement](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L34)
  - [Wrapper delegate-and-stop rule](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L36)
  - [Registry finalizeplan hint for epics/stories](lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json#L109)
  - [Registry finalizeplan hint for readiness](lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json#L121)
  - [Registry finalizeplan hint for sprint planning](lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json#L133)
  - [Registry finalizeplan hint for create story](lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json#L145)

## Confidence
- Overall: High.
- Rationale: all required evidence surfaces agree that the prompt is a thin stub, the operative behavior is the FinalizePlan skill, and the skill’s downstream bundle is intentionally composed from wrapper-routed BMAD skills remapped into the FinalizePlan phase.
- Residual uncertainty: Medium-Low for operational runtime details such as exact PR automation behavior, because those details depend on git-orchestration implementations not required by this audit.

## Gaps
- Prompt-local observability is low. Both visible prompt files are stubs, so phase behavior is only auditable by traversing into the skill and wrapper contracts rather than from the prompt itself.
- Cross-surface phase terminology is intentionally non-uniform. Global BMAD help still classifies `bmad-create-story` and `bmad-sprint-planning` as `4-implementation`, while Lens remaps them into `finalizeplan`; this is coherent but easy to misread without checking both [bmad-help](lens.core/_bmad/_config/bmad-help.csv#L14) and [module-help](lens.core/_bmad/lens-work/module-help.csv#L65).
- FinalizePlan depends on several synchronized metadata surfaces staying aligned: prompt stubs, module prompt list, module-help routing, wrapper registry, and lifecycle contract. Any one of those can drift without breaking the others immediately.
- The prompt and skill describe the bundle order clearly, but artifact ownership is split between the conductor and wrapper. That separation is architecturally sound, yet it makes failures harder to diagnose from prompt text alone because the conductor explicitly says it does not author wrapper-owned outputs itself ([identity note](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L18)).