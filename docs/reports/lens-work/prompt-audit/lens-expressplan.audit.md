# lens-expressplan Prompt Audit

## Purpose Summary
- [.github/prompts/lens-expressplan.prompt.md](.github/prompts/lens-expressplan.prompt.md#L5) is a control-repo delegation stub. It does not implement ExpressPlan logic directly; it forwards execution to the release prompt in [lens.core/_bmad/lens-work/prompts/lens-expressplan.prompt.md](lens.core/_bmad/lens-work/prompts/lens-expressplan.prompt.md#L5).
- The release prompt is also a stub and delegates to [lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L6).
- Effective behavior therefore lives in the ExpressPlan skill contract: validate the feature is on the `express` track, delegate planning to QuickPlan through the Lens BMAD wrapper, run adversarial review with required party mode, then advance to FinalizePlan on `pass` or `pass-with-warnings`.
- The prompt name and descriptions say "all planning artifacts in one session," but the implemented contract is narrower: ExpressPlan stages QuickPlan outputs and review findings, then explicitly hands off the governance bundle and PR handoff to FinalizePlan.

## BMAD Skill Mapping
- There is no native BMAD skill or help entry named `expressplan` in the global BMAD registries. [skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv) contains underlying constituent skills such as [bmad-create-prd](lens.core/_bmad/_config/skill-manifest.csv#L23), [bmad-create-architecture](lens.core/_bmad/_config/skill-manifest.csv#L29), [bmad-check-implementation-readiness](lens.core/_bmad/_config/skill-manifest.csv#L28), [bmad-sprint-planning](lens.core/_bmad/_config/skill-manifest.csv#L43), and [bmad-create-story](lens.core/_bmad/_config/skill-manifest.csv#L38), but no first-class ExpressPlan primitive.
- Global BMAD help places those underlying capabilities across multiple lifecycle bands rather than one unified express flow: [Create PRD](lens.core/_bmad/_config/bmad-help.csv#L13) in `2-planning`, [Create Architecture](lens.core/_bmad/_config/bmad-help.csv#L11) and [Check Implementation Readiness](lens.core/_bmad/_config/bmad-help.csv#L8) in `3-solutioning`, and [Sprint Planning](lens.core/_bmad/_config/bmad-help.csv#L27) plus [Create Story](lens.core/_bmad/_config/bmad-help.csv#L14) in `4-implementation`.
- Lens supplies the missing orchestration layer. [module.yaml](lens.core/_bmad/lens-work/module.yaml#L117) registers [bmad-lens-expressplan](lens.core/_bmad/lens-work/module.yaml#L118) and also registers its delegated conductor [bmad-lens-quickplan](lens.core/_bmad/lens-work/module.yaml#L55). The prompt surface is published in [module.yaml](lens.core/_bmad/lens-work/module.yaml#L221) and installed to `.github/prompts` via [module.yaml](lens.core/_bmad/lens-work/module.yaml#L287).
- Lens command help exposes ExpressPlan as its own command surface: [expressplan (LE)](lens.core/_bmad/lens-work/module-help.csv#L56). Unlike BusinessPlan or TechPlan, there are no module-help wrapper rows for a dedicated `expressplan` native BMAD delegate; instead the skill itself invokes [bmad-lens-bmad-skill](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L59) with `bmad-lens-quickplan`.
- The wrapper contract is important because ExpressPlan relies on it for context and write-boundary enforcement. The wrapper declares `feature-required` and `planning-docs` behavior in [bmad-lens-bmad-skill/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L31), [bmad-lens-bmad-skill/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L32), and [bmad-lens-bmad-skill/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L70). The same wrapper also maps BMAD subskills to their Lens phases, including [Create PRD](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L130), [Create Architecture](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L132), [Implementation Readiness](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L134), [Sprint Planning](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L135), and [Create Story](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L136).
- CLI packaging exposes the prompt broadly as available in `both` installs and phase `any` via [setup.py](setup.py#L141). Actual restriction to the `express` track happens later inside the skill contract, not at registration time.

## Lifecycle Fit
- Lifecycle defines `expressplan` as a standalone phase used only by the `express` track, not part of the canonical [phase_order](lens.core/_bmad/lens-work/lifecycle.yaml#L238). See the dedicated phase block at [lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L201) and the explicit note at [lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L236).
- The lifecycle contract matches the skill's high-level sequence: `expressplan` [auto_advance_to: /finalizeplan](lens.core/_bmad/lens-work/lifecycle.yaml#L213), requires [expressplan-adversarial-review.md](lens.core/_bmad/lens-work/lifecycle.yaml#L217), and on success updates phase state then moves to FinalizePlan as described in [lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L227) and [lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L230).
- Track gating is consistent. The `express` track starts at [expressplan](lens.core/_bmad/lens-work/lifecycle.yaml#L313), includes phases [expressplan, finalizeplan](lens.core/_bmad/lens-work/lifecycle.yaml#L309), and requires constitution permission at [lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L314). The ExpressPlan skill mirrors that with explicit validation of the track and constitution gate in [bmad-lens-expressplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L44).
- Readiness semantics also line up. Lifecycle declares ready artifacts as `business-plan`, `tech-plan`, and `sprint-plan`, but reviewed artifacts as only `business-plan` and `tech-plan` at [lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L219). The skill follows that contract by validating `review-ready` first in [bmad-lens-expressplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L51), then having adversarial review focus on `business-plan.md` and `tech-plan.md` in [bmad-lens-expressplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L69).
- QuickPlan is the main dependency and also the main fit risk. ExpressPlan says step 1 is QuickPlan and reports staged [business-plan.md, tech-plan.md, sprint-plan.md](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L63). But QuickPlan describes itself as an end-to-end planning conductor from business planning through [story creation](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L10), with explicit phase fidelity to [/businessplan, /techplan, and /finalizeplan](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L35) and pipeline rows for [Sprint Planning](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L92) and [Story Creation](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L93).
- Because FinalizePlan still owns the actual downstream bundle, PR checks, and `/dev` handoff in [bmad-lens-finalizeplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L10), [bmad-lens-finalizeplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L62), and [bmad-lens-finalizeplan/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L70), ExpressPlan fits best as an accelerated pre-FinalizePlan planning conductor, not as a true all-artifacts replacement for FinalizePlan.

## Evidence Refs
- Prompt chain
  - [.github prompt description](.github/prompts/lens-expressplan.prompt.md#L2)
  - [.github prompt stub title](.github/prompts/lens-expressplan.prompt.md#L5)
  - [.github prompt delegation](.github/prompts/lens-expressplan.prompt.md#L10)
  - [release prompt description](lens.core/_bmad/lens-work/prompts/lens-expressplan.prompt.md#L2)
  - [release prompt stub title](lens.core/_bmad/lens-work/prompts/lens-expressplan.prompt.md#L5)
  - [release prompt delegation to skill](lens.core/_bmad/lens-work/prompts/lens-expressplan.prompt.md#L10)
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
  - [module expressplan registration](lens.core/_bmad/lens-work/module.yaml#L117)
  - [module prompt listing](lens.core/_bmad/lens-work/module.yaml#L221)
  - [module prompt install surface](lens.core/_bmad/lens-work/module.yaml#L287)
  - [module-help expressplan LE](lens.core/_bmad/lens-work/module-help.csv#L56)
  - [lifecycle expressplan phase](lens.core/_bmad/lens-work/lifecycle.yaml#L201)
  - [lifecycle expressplan reviewed artifacts](lens.core/_bmad/lens-work/lifecycle.yaml#L219)
  - [lifecycle expressplan auto-advance](lens.core/_bmad/lens-work/lifecycle.yaml#L213)
  - [lifecycle express track phases](lens.core/_bmad/lens-work/lifecycle.yaml#L309)
  - [lifecycle express track start phase](lens.core/_bmad/lens-work/lifecycle.yaml#L313)
  - [lifecycle express track constitution permission](lens.core/_bmad/lens-work/lifecycle.yaml#L314)
  - [setup.py expressplan registration](setup.py#L141)
- Relevant prompt/skill files driving real behavior
  - [ExpressPlan skill overview](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L10)
  - [ExpressPlan scope and constitution requirement](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L12)
  - [ExpressPlan review-ready fast path](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L36)
  - [ExpressPlan review-ready validation command](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L51)
  - [ExpressPlan QuickPlan invocation](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L59)
  - [ExpressPlan staged outputs](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L63)
  - [ExpressPlan adversarial review invocation](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L67)
  - [ExpressPlan review scope](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L69)
  - [ExpressPlan advance to FinalizePlan](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L77)
  - [ExpressPlan artifact table](lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md#L88)
  - [QuickPlan overview](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L10)
  - [QuickPlan two-document rule](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L31)
  - [QuickPlan phase fidelity to FinalizePlan](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L35)
  - [QuickPlan sprint-planning output row](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L92)
  - [QuickPlan story-creation output row](lens.core/_bmad/lens-work/skills/bmad-lens-quickplan/SKILL.md#L93)
  - [Adversarial review supports expressplan](lens.core/_bmad/lens-work/skills/bmad-lens-adversarial-review/SKILL.md#L10)
  - [Adversarial review expressplan context loading](lens.core/_bmad/lens-work/skills/bmad-lens-adversarial-review/SKILL.md#L52)
  - [FinalizePlan overview](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L10)
  - [FinalizePlan bundled wrapper delegation](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L62)
  - [FinalizePlan phase completion to /dev](lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md#L70)
  - [Lens wrapper context modes](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L31)
  - [Lens wrapper output modes](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L32)
  - [Lens wrapper planning-docs write scope](lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md#L70)

## Confidence
- Overall: Medium-high.
- Rationale: The prompt chain is explicit and unambiguous, and the lifecycle, module metadata, and ExpressPlan skill all agree on the core shape of the flow: QuickPlan delegation, adversarial review, then FinalizePlan handoff.
- Residual uncertainty: Moderate around the exact runtime boundary between QuickPlan and FinalizePlan, because QuickPlan's own contract still describes an end-to-end pipeline through story creation and FinalizePlan inheritance, while ExpressPlan narrows step 1 to only business-plan, tech-plan, and sprint-plan staging.

## Gaps
- Double-stub indirection reduces prompt-local auditability. Both the control prompt and release prompt are shells, so all meaningful behavior is displaced into skills and lifecycle metadata.
- The phrase "all planning artifacts in one session" overstates what the actual lifecycle permits. The `express` track still includes [finalizeplan](lens.core/_bmad/lens-work/lifecycle.yaml#L309), and FinalizePlan still owns epics, stories, implementation readiness, PR readiness, and final `/dev` handoff.
- QuickPlan scope appears broader than ExpressPlan step 1 claims. QuickPlan advertises planning through story creation and inherits `/finalizeplan`, but ExpressPlan presents QuickPlan as producing only business-plan, tech-plan, and sprint-plan before handing off to FinalizePlan. That contract boundary should be tightened or documented explicitly.
- Lifecycle review semantics may surprise readers: `sprint-plan` is required for `review-ready`, but it is omitted from `reviewed_artifacts`, and the ExpressPlan review step explicitly reads only business-plan and tech-plan. This may be intentional, but the rationale is not explained in the prompt or skill.
- ExpressPlan has no first-class presence in global BMAD registries or help. Discoverability depends on Lens module surfaces rather than BMAD-wide metadata, which increases the chance of routing drift if module surfaces and skill contracts diverge.
- Registration and runtime enforcement are split. [setup.py](setup.py#L141) exposes `lens-expressplan` as phase `any`, while the skill later enforces `express` track membership and constitution permission. That is workable, but users can discover the command outside the contexts where it is actually valid.