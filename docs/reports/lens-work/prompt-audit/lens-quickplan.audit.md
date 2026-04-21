# lens-quickplan Prompt Audit

## Purpose Summary
- [.github/prompts/lens-quickplan.prompt.md](.github/prompts/lens-quickplan.prompt.md#L5) is a control-repo delegation stub. It does not define QuickPlan behavior directly; it forwards to the release prompt at [lens.core/_bmad/lens-work/prompts/lens-quickplan.prompt.md](lens.core/_bmad/lens-work/prompts/lens-quickplan.prompt.md#L5).
- The release prompt is also a stub and delegates to the implementation contract in [lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L6).
- Effective behavior is skill-driven: QuickPlan is defined as an end-to-end planning conductor from business planning through story creation, with staged handoff semantics and two-pass batch behavior in [bmad-lens-quickplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L10).
- The same contract explicitly says QuickPlan preserves phase-conductor boundaries rather than directly writing around them, via phase fidelity to `/businessplan`, `/techplan`, and `/finalizeplan` in [bmad-lens-quickplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L35).

## BMAD Skill Mapping
- There is no native BMAD-global skill entry named `quickplan` in [skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv). Instead, QuickPlan composes existing BMAD capabilities that are registered separately, including [bmad-create-prd](lens.core/_bmad/_config/skill-manifest.csv#L23), [bmad-create-architecture](lens.core/_bmad/_config/skill-manifest.csv#L29), [bmad-check-implementation-readiness](lens.core/_bmad/_config/skill-manifest.csv#L28), [bmad-sprint-planning](lens.core/_bmad/_config/skill-manifest.csv#L43), and [bmad-create-story](lens.core/_bmad/_config/skill-manifest.csv#L38).
- BMAD-global help also has no first-class QuickPlan command in [bmad-help.csv](lens.core/_bmad/_config/bmad-help.csv); the mapped primitives remain distributed across lifecycle bands: [Create PRD](lens.core/_bmad/_config/bmad-help.csv#L13), [Create Architecture](lens.core/_bmad/_config/bmad-help.csv#L11), [Check Implementation Readiness](lens.core/_bmad/_config/bmad-help.csv#L8), [Sprint Planning](lens.core/_bmad/_config/bmad-help.csv#L27), and [Create Story](lens.core/_bmad/_config/bmad-help.csv#L14).
- Lens module metadata provides the orchestration layer: [module.yaml](lens.core/_bmad/lens-work/module.yaml#L55) registers `bmad-lens-quickplan`, includes the prompt in [module.yaml](lens.core/_bmad/lens-work/module.yaml#L267), and exposes a GitHub stub install surface in [module.yaml](lens.core/_bmad/lens-work/module.yaml#L332).
- Lens command help exposes QuickPlan as a direct command surface with lifecycle-agnostic availability in [module-help.csv](lens.core/_bmad/lens-work/module-help.csv#L10) (`phase=anytime`) and a paired validation command in [module-help.csv](lens.core/_bmad/lens-work/module-help.csv#L11).
- Wrapper mapping confirms downstream BMAD delegates are phase-scoped through Lens wrapper contracts, not run as ad hoc direct calls: [bmad-lens-bmad-skill/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L31), [bmad-lens-bmad-skill/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L32), [bmad-lens-bmad-skill/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L130), [bmad-lens-bmad-skill/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L132), [bmad-lens-bmad-skill/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L134), [bmad-lens-bmad-skill/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L135), and [bmad-lens-bmad-skill/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L136).

## Lifecycle Fit
- QuickPlan is not a lifecycle phase in the canonical lifecycle sequence. Canonical `phase_order` remains [preplan, businessplan, techplan, finalizeplan] in [lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L238).
- The QuickPlan skill's own contract aligns with that sequencing by explicitly inheriting `/businessplan`, `/techplan`, and `/finalizeplan` contracts in [bmad-lens-quickplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L35) and by routing execution to Lens phase conductors in [bmad-lens-quickplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L129).
- Lifecycle does reference QuickPlan, but only as an ExpressPlan trigger (`quickplan-via-lens-wrapper`) in [lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L223) and [lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L224), with ExpressPlan then advancing to FinalizePlan in [lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L213) and [lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L309).
- FinalizePlan remains the lifecycle checkpoint that owns final review, bundled planning outputs, and `/dev` handoff in [bmad-lens-finalizeplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L10), [bmad-lens-finalizeplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L62), and [bmad-lens-finalizeplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L70).
- Net fit: QuickPlan is lifecycle-compatible as a high-level conductor that composes existing planning phases, but it is metadata-distinct from lifecycle phase identity (command surface `anytime` vs canonical phase progression).

## Evidence Refs
- Prompt chain
  - [.github quickplan prompt description](.github/prompts/lens-quickplan.prompt.md#L2)
  - [.github quickplan stub title](.github/prompts/lens-quickplan.prompt.md#L5)
  - [.github quickplan delegation](.github/prompts/lens-quickplan.prompt.md#L10)
  - [release quickplan prompt description](lens.core/_bmad/lens-work/prompts/lens-quickplan.prompt.md#L2)
  - [release quickplan stub title](lens.core/_bmad/lens-work/prompts/lens-quickplan.prompt.md#L5)
  - [release quickplan delegation](lens.core/_bmad/lens-work/prompts/lens-quickplan.prompt.md#L10)
- Required registry/help/module/lifecycle sources
  - [skill-manifest Create PRD](lens.core/_bmad/_config/skill-manifest.csv#L23)
  - [skill-manifest Implementation Readiness](lens.core/_bmad/_config/skill-manifest.csv#L28)
  - [skill-manifest Create Architecture](lens.core/_bmad/_config/skill-manifest.csv#L29)
  - [skill-manifest Create Story](lens.core/_bmad/_config/skill-manifest.csv#L38)
  - [skill-manifest Sprint Planning](lens.core/_bmad/_config/skill-manifest.csv#L43)
  - [bmad-help Create Architecture](lens.core/_bmad/_config/bmad-help.csv#L11)
  - [bmad-help Create PRD](lens.core/_bmad/_config/bmad-help.csv#L13)
  - [bmad-help Create Story](lens.core/_bmad/_config/bmad-help.csv#L14)
  - [bmad-help Sprint Planning](lens.core/_bmad/_config/bmad-help.csv#L27)
  - [module quickplan registration](lens.core/_bmad/lens-work/module.yaml#L55)
  - [module quickplan prompt listing](lens.core/_bmad/lens-work/module.yaml#L267)
  - [module quickplan prompt install surface](lens.core/_bmad/lens-work/module.yaml#L332)
  - [module-help quickplan plan command](lens.core/_bmad/lens-work/module-help.csv#L10)
  - [module-help quickplan validate command](lens.core/_bmad/lens-work/module-help.csv#L11)
  - [lifecycle canonical phase order](lens.core/_bmad/lens-work/lifecycle.yaml#L238)
  - [lifecycle expressplan quickplan trigger](lens.core/_bmad/lens-work/lifecycle.yaml#L223)
  - [lifecycle expressplan quickplan trigger detail](lens.core/_bmad/lens-work/lifecycle.yaml#L224)
  - [lifecycle expressplan auto-advance](lens.core/_bmad/lens-work/lifecycle.yaml#L213)
  - [lifecycle express track phases](lens.core/_bmad/lens-work/lifecycle.yaml#L309)
- Relevant prompt/skill implementation files
  - [QuickPlan overview](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L10)
  - [QuickPlan two-document rule](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L31)
  - [QuickPlan phase fidelity](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L35)
  - [QuickPlan pipeline section](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L85)
  - [QuickPlan story creation row](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L93)
  - [QuickPlan phase-conductor delegation](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L129)
  - [Lens wrapper context modes](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L31)
  - [Lens wrapper output modes](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L32)
  - [FinalizePlan overview](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L10)
  - [FinalizePlan bundled wrapper delegation](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L62)
  - [FinalizePlan /dev handoff](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L70)

## Confidence
- Overall: Medium-high.
- Rationale: The prompt chain, module registration, and QuickPlan skill contract are explicit and mutually consistent about stub delegation and phase-conductor orchestration.
- Residual uncertainty: Moderate around output-scope expectations, because QuickPlan documentation includes story creation while module-help describes quickplan outputs primarily as `business-plan.md`, `tech-plan.md`, and `finalizeplan-review.md`.

## Gaps
- Prompt-local observability is low. Both control and release prompts are stubs, so behavior is only auditable by reading skill and lifecycle contracts.
- Global BMAD discoverability gap remains. There is no first-class `quickplan` primitive in BMAD-global registries/help; discoverability depends on Lens-local surfaces.
- Lifecycle identity versus command identity is split. QuickPlan is exposed as `anytime` in module help, but canonical lifecycle progression is phase-based and does not include `quickplan` as a phase.
- Output-contract ambiguity persists. QuickPlan states end-to-end flow through story creation, while module-help quickplan outputs list only business-plan, tech-plan, and finalizeplan-review. That mismatch can create user expectation drift for what QuickPlan guarantees before FinalizePlan completion.