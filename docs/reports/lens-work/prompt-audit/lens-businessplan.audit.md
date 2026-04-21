# lens-businessplan Prompt Audit

## Purpose Summary
- [.github/prompts/lens-businessplan.prompt.md](.github/prompts/lens-businessplan.prompt.md#L5) is a control-repo delegation stub. It does not implement phase logic directly; it forwards execution to the release prompt in [lens.core/_bmad/lens-work/prompts/lens-businessplan.prompt.md](lens.core/_bmad/lens-work/prompts/lens-businessplan.prompt.md#L5).
- The release prompt is also a stub and delegates to [lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md#L6).
- Effective behavior therefore lives in the BusinessPlan skill contract: run BusinessPlan for one feature, ensure PrePlan dependency, route PRD and UX generation via Lens wrappers, and run adversarial review before phase completion.

## BMAD Skill Mapping
- Global BMAD registry includes the native planning skills used by BusinessPlan: [bmad-create-prd](lens.core/_bmad/_config/skill-manifest.csv#L23) and [bmad-create-ux-design](lens.core/_bmad/_config/skill-manifest.csv#L24), plus phase personas [bmad-agent-pm](lens.core/_bmad/_config/skill-manifest.csv#L21) and [bmad-agent-ux-designer](lens.core/_bmad/_config/skill-manifest.csv#L22).
- Global help maps these native skills to `2-planning`: [Create PRD (CP)](lens.core/_bmad/_config/bmad-help.csv#L13) and [Create UX (CU)](lens.core/_bmad/_config/bmad-help.csv#L16).
- Lens module wiring exposes BusinessPlan skill/prompt surfaces: [bmad-lens-businessplan skill entry](lens.core/_bmad/lens-work/module.yaml#L108), [lens-businessplan prompt listing](lens.core/_bmad/lens-work/module.yaml#L222), and adapter prompt surfaces for PRD/UX wrapper commands ([create-prd](lens.core/_bmad/lens-work/module.yaml#L245), [create-ux](lens.core/_bmad/lens-work/module.yaml#L246)).
- Lens command help maps the phase conductor and wrapper routes: [businessplan (LB)](lens.core/_bmad/lens-work/module-help.csv#L49), [BPR create-prd](lens.core/_bmad/lens-work/module-help.csv#L60), and [BUX create-ux-design](lens.core/_bmad/lens-work/module-help.csv#L61).
- BusinessPlan skill explicitly delegates through wrapper-native mapping: [prd -> bmad-create-prd](lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md#L61) and [ux-design -> bmad-create-ux-design](lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md#L62).
- Wrapper policy confirms both are `feature-required` + `planning-docs` during `businessplan` ([PRD row](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L130), [UX row](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L131)).

## Lifecycle Fit
- Lifecycle defines `businessplan` as the phase for PRD and UX outputs ([phase definition](lens.core/_bmad/lens-work/lifecycle.yaml#L129), [artifacts](lens.core/_bmad/lens-work/lifecycle.yaml#L136), [artifacts](lens.core/_bmad/lens-work/lifecycle.yaml#L137)).
- Readiness gate for BusinessPlan is explicitly tied to those artifacts ([ready_when_artifacts](lens.core/_bmad/lens-work/lifecycle.yaml#L144), [reviewed_artifacts](lens.core/_bmad/lens-work/lifecycle.yaml#L145)).
- Canonical phase ordering places BusinessPlan between PrePlan and TechPlan ([phase_order](lens.core/_bmad/lens-work/lifecycle.yaml#L238)); tracks `full` and `feature` both include it ([full](lens.core/_bmad/lens-work/lifecycle.yaml#L244), [full phases](lens.core/_bmad/lens-work/lifecycle.yaml#L246), [feature](lens.core/_bmad/lens-work/lifecycle.yaml#L252), [feature phases](lens.core/_bmad/lens-work/lifecycle.yaml#L254)).
- BusinessPlan skill behavior aligns with lifecycle contracts:
  - Enforces predecessor completion ([preplan dependency check](lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md#L46)).
  - Uses lifecycle `review-ready` contract against staged docs ([validation command](lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md#L51)).
  - Runs adversarial review before phase state update ([review trigger](lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md#L52), [Phase Completion gate](lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md#L93)).
  - Advances to TechPlan after successful completion ([next action](lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md#L98)).

## Evidence Refs
- Prompt chain
  - [Control prompt stub title](.github/prompts/lens-businessplan.prompt.md#L5)
  - [Control prompt delegation to release prompt](.github/prompts/lens-businessplan.prompt.md#L10)
  - [Release prompt stub title](lens.core/_bmad/lens-work/prompts/lens-businessplan.prompt.md#L5)
  - [Release prompt delegation to BusinessPlan skill](lens.core/_bmad/lens-work/prompts/lens-businessplan.prompt.md#L10)
- Required registry/help/module/lifecycle sources
  - [skill-manifest PRD](lens.core/_bmad/_config/skill-manifest.csv#L23)
  - [skill-manifest UX](lens.core/_bmad/_config/skill-manifest.csv#L24)
  - [bmad-help Create PRD](lens.core/_bmad/_config/bmad-help.csv#L13)
  - [bmad-help Create UX](lens.core/_bmad/_config/bmad-help.csv#L16)
  - [module skill bmad-lens-businessplan](lens.core/_bmad/lens-work/module.yaml#L108)
  - [module prompt lens-businessplan](lens.core/_bmad/lens-work/module.yaml#L222)
  - [module-help LB](lens.core/_bmad/lens-work/module-help.csv#L49)
  - [module-help BPR](lens.core/_bmad/lens-work/module-help.csv#L60)
  - [module-help BUX](lens.core/_bmad/lens-work/module-help.csv#L61)
  - [lifecycle businessplan phase](lens.core/_bmad/lens-work/lifecycle.yaml#L129)
  - [lifecycle businessplan readiness artifacts](lens.core/_bmad/lens-work/lifecycle.yaml#L144)
  - [lifecycle phase_order](lens.core/_bmad/lens-work/lifecycle.yaml#L238)
  - [lifecycle feature track includes businessplan](lens.core/_bmad/lens-work/lifecycle.yaml#L254)
- Relevant prompt/skill files driving real behavior
  - [BusinessPlan skill overview](lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md#L10)
  - [BusinessPlan principles: stage then publish](lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md#L32)
  - [BusinessPlan feature docs authority](lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md#L33)
  - [BusinessPlan wrapper delegation mapping](lens.core/_bmad/lens-work/skills/bmad-lens-businessplan/SKILL.md#L61)
  - [Wrapper feature-required/planning-docs policy](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L31)
  - [Wrapper feature docs authority](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L33)
  - [Native PRD skill delegates to workflow](lens.core/_bmad/bmm/2-plan-workflows/bmad-create-prd/SKILL.md#L6)
  - [Native UX skill delegates to workflow](lens.core/_bmad/bmm/2-plan-workflows/bmad-create-ux-design/SKILL.md#L6)

## Confidence
- Overall: High.
- Rationale: The control prompt and release prompt are both explicit stubs with unambiguous delegation to BusinessPlan skill, and lifecycle/module/help/registry files all consistently map BusinessPlan to PRD+UX production and review gating.
- Residual uncertainty: Low-to-medium for runtime conversational details because most execution behavior is encapsulated in skill/workflow contracts rather than prompt-local procedural content.

## Gaps
- Double-stub indirection lowers prompt-local observability: neither prompt file contains acceptance criteria, workflow branching, or direct artifact checks.
- Potential naming drift exists between declared artifacts and expected filenames across surfaces. Lifecycle uses artifact IDs (`prd`, `ux-design`), while phase outputs in help and skill examples imply concrete files (`prd.md`, `ux-design.md`).
- Mapping is spread across multiple synchronization points (skill-manifest, bmad-help, module.yaml prompt list, module-help, lifecycle, wrapper registry behavior). A single stale surface can cause discoverability or routing drift even when core skill logic remains correct.
- BusinessPlan relies heavily on wrapper and downstream skills for authoring guarantees; prompt-level auditability of write-boundary enforcement is indirect and depends on wrapper policy, not prompt text.