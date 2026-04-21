# lens-feature-yaml Prompt Audit

## Purpose Summary
- [.github/prompts/lens-feature-yaml.prompt.md](.github/prompts/lens-feature-yaml.prompt.md) is a control-repo stub only. It delegates to the release prompt rather than defining behavior itself.
- [lens.core/_bmad/lens-work/prompts/lens-feature-yaml.prompt.md](lens.core/_bmad/lens-work/prompts/lens-feature-yaml.prompt.md) is also a stub and forwards execution to the Lens-local skill [lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/SKILL.md).
- Effective behavior lives in the feature-yaml skill plus its implementation artifacts: it is a governance metadata manager for `feature.yaml`, covering create, read, update, validate, and list operations, with the Python implementation in [lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/feature-yaml-ops.py](lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/feature-yaml-ops.py) and schema template in [lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/assets/feature-template.yaml](lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/assets/feature-template.yaml).
- This makes the prompt a foundational metadata utility, not a phase-conductor workflow like `lens-preplan`, `lens-businessplan`, or `lens-finalizeplan`.

## BMAD Skill Mapping
- There is no direct `bmad-lens-feature-yaml` entry in the global BMAD catalogs [lens.core/_bmad/_config/skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv) or [lens.core/_bmad/_config/bmad-help.csv](lens.core/_bmad/_config/bmad-help.csv). That is consistent with this being a Lens module-local capability rather than a portable BMAD skill.
- Lens registers it explicitly in the module contract as a first-class local skill: [module skill entry](lens.core/_bmad/lens-work/module.yaml#L34) and [skill file path](lens.core/_bmad/lens-work/module.yaml#L35).
- Lens also publishes the prompt surface directly in the module prompt list and GitHub adapter stub list: [prompt listing](lens.core/_bmad/lens-work/module.yaml#L262) and [adapter stub listing](lens.core/_bmad/lens-work/module.yaml#L327).
- The user-facing command mapping is Lens-specific, not BMAD-global: [module-help FY row](lens.core/_bmad/lens-work/module-help.csv#L2) exposes menu code `FY`, display name `manage-feature-yaml`, action `read`, and phase `anytime`.
- Unlike the planning prompts that map through `bmad-lens-bmad-skill` into native BMAD workflows, this prompt terminates at a native Lens skill. The skill itself advertises direct capabilities via `create`, `read`, `update`, `validate`, and `list`, and routes those to local reference docs and the local script instead of a downstream BMAD workflow.

## Lifecycle Fit
- Fit is functionally strong but structurally different from phase workflows. [lens.core/_bmad/lens-work/module-help.csv#L2](lens.core/_bmad/lens-work/module-help.csv#L2) marks the command as `anytime`, which matches the skill's role as shared lifecycle metadata management rather than a single phase activity.
- The skill clearly operates on lifecycle state, because `feature.yaml` stores feature identity, track, phase, milestone timestamps, dependencies, docs paths, and target repo metadata. That makes it a cross-cutting dependency for most Lens workflows.
- The lifecycle contract reinforces that separation: canonical ordered phases are [phase_order](lens.core/_bmad/lens-work/lifecycle.yaml#L238) with `preplan`, `businessplan`, `techplan`, and `finalizeplan`, while `dev` is explicitly described as not a lifecycle phase in the lifecycle notes immediately above `phase_order`.
- The feature-yaml contract therefore acts as an operational state layer that spans both canonical phases and non-phase states such as `dev`, `paused`, and completion states. That is reasonable for internal state management, but it is broader than the lifecycle's phase model.
- Milestone alignment is partial. The lifecycle milestone backbone defines [techplan](lens.core/_bmad/lens-work/lifecycle.yaml#L76), [finalizeplan](lens.core/_bmad/lens-work/lifecycle.yaml#L80), [dev-ready](lens.core/_bmad/lens-work/lifecycle.yaml#L89), and [dev-complete](lens.core/_bmad/lens-work/lifecycle.yaml#L94). The feature template includes `techplan`, `finalizeplan`, `dev-ready`, and `dev-complete`, but also includes an extra `businessplan` milestone in [lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/assets/feature-template.yaml](lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/assets/feature-template.yaml).
- Track alignment is weaker. The feature template defaults to `track: "quickplan"` and documents `quickplan` as a valid track in [lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/assets/feature-template.yaml](lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/assets/feature-template.yaml), and the script validates `quickplan` in [lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/feature-yaml-ops.py](lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/feature-yaml-ops.py). But the lifecycle track registry in [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml) defines `full`, `feature`, `tech-change`, `hotfix`, `hotfix-express`, `spike`, `quickdev`, and `express`, not `quickplan`.

## Evidence Refs
- Prompt chain
  - [.github/prompts/lens-feature-yaml.prompt.md](.github/prompts/lens-feature-yaml.prompt.md)
  - [lens.core/_bmad/lens-work/prompts/lens-feature-yaml.prompt.md](lens.core/_bmad/lens-work/prompts/lens-feature-yaml.prompt.md)
- Required registry/help/module/lifecycle sources
  - [lens.core/_bmad/_config/skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv)
  - [lens.core/_bmad/_config/bmad-help.csv](lens.core/_bmad/_config/bmad-help.csv)
  - [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml)
  - [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv)
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml)
- Relevant prompt and skill implementation files
  - [lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/SKILL.md)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/assets/feature-template.yaml](lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/assets/feature-template.yaml)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/references/create.md](lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/references/create.md)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/references/update.md](lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/references/update.md)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/references/validate.md](lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/references/validate.md)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/feature-yaml-ops.py](lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/feature-yaml-ops.py)

## Confidence
- Overall: High.
- Rationale: The prompt chain is unambiguous, the Lens module wiring is explicit, and the feature-yaml skill exposes its real contract in a small, inspectable set of template, reference, and script files.
- Residual uncertainty: Medium for whether the observed schema drift is intentional backward compatibility or stale metadata, because the lifecycle file and feature-yaml implementation are clearly out of sync on track naming but do not explain the intended compatibility story.

## Gaps
- Global discoverability gap: `bmad-lens-feature-yaml` is absent from the BMAD-global catalogs [lens.core/_bmad/_config/skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv) and [lens.core/_bmad/_config/bmad-help.csv](lens.core/_bmad/_config/bmad-help.csv), so discovery depends entirely on Lens-local module surfaces.
- Track schema drift: the feature-yaml template and script accept `quickplan`, but the lifecycle track contract in [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml) does not define `quickplan`; it defines `quickdev` instead.
- Milestone schema drift: the feature template persists a `businessplan` milestone even though the lifecycle milestone backbone only names `techplan`, `finalizeplan`, `dev-ready`, and `dev-complete`.
- Phase-model drift: the feature-yaml layer treats `dev`, `complete`, `paused`, and `*-complete` as valid phase states, while the lifecycle contract explicitly says `dev` is not a lifecycle phase. That is workable as an operational overlay, but the contract boundary is undocumented in the prompt itself.
- Auditability gap: both prompt files are pure stubs, so reviewers must inspect the skill, template, references, and script to understand actual behavior. The prompt alone provides almost no local observability.