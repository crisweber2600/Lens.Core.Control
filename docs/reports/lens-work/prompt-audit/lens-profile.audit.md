# lens-profile Prompt Audit

## Purpose Summary
- [.github/prompts/lens-profile.prompt.md](.github/prompts/lens-profile.prompt.md) is a control-level wrapper prompt. It delegates execution to release content after running a lightweight preflight command.
- [lens.core/_bmad/lens-work/prompts/lens-profile.prompt.md](lens.core/_bmad/lens-work/prompts/lens-profile.prompt.md) is also a wrapper. It delegates behavioral authority to the profile skill contract.
- Effective runtime behavior is defined in [lens.core/_bmad/lens-work/skills/bmad-lens-profile/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-profile/SKILL.md): view current onboarding profile and optionally edit fields with validation and persistence.

## BMAD Skill Mapping
- Primary Lens skill mapping is explicit in [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml) and [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv):
  - Skill id: bmad-lens-profile.
  - User command surface: profile (PF), action view, optional edit flag.
  - Availability: anytime.
- Prompt routing for this command is present in the module prompt inventory in [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml), which includes lens-profile.prompt.md in both module prompt list and adapter stub prompt list.
- Skill-level integration points in [lens.core/_bmad/lens-work/skills/bmad-lens-profile/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-profile/SKILL.md) currently reference:
  - bmad-lens-theme as active integration.
  - bmad-lens-onboard as deprecated profile creator.
- Cross-check against global BMAD registries:
  - [lens.core/_bmad/_config/skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv) has no bmad-lens-profile entry.
  - [lens.core/_bmad/_config/bmad-help.csv](lens.core/_bmad/_config/bmad-help.csv) has no Lens-specific profile command entry.
  - This appears intentional (Lens command registry lives in module-help), but it creates discoverability split across catalogs.

## Lifecycle Fit
- The profile command is lifecycle-agnostic in [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv), marked for anytime use. This fits profile editing as a personal context operation rather than a phase artifact producer.
- Lifecycle sequencing in [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml) defines planning and execution phases (preplan, businessplan, techplan, finalizeplan; plus expressplan track), with no dedicated profile phase. The profile command therefore correctly sits outside phase order.
- Operational fit is strong: profile data in .lens personal state is consistent with lifecycle v4.0 direction in [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml), where local personal state is expected under .lens.
- One wiring inconsistency exists: the control stub includes a lightweight preflight step before delegation, while the release prompt stub only delegates directly to the skill. This introduces behavior asymmetry between wrapper layers.

## Evidence Refs
- Prompt chain
  - [.github/prompts/lens-profile.prompt.md](.github/prompts/lens-profile.prompt.md)
  - [lens.core/_bmad/lens-work/prompts/lens-profile.prompt.md](lens.core/_bmad/lens-work/prompts/lens-profile.prompt.md)
- Required registry and module sources
  - [lens.core/_bmad/_config/skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv)
  - [lens.core/_bmad/_config/bmad-help.csv](lens.core/_bmad/_config/bmad-help.csv)
  - [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml)
  - [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv)
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml)
- Relevant prompt and skill files
  - [lens.core/_bmad/lens-work/skills/bmad-lens-profile/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-profile/SKILL.md)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md)

## Confidence
- Overall: High.
- Rationale: Prompt delegation, module registration, command discoverability, and lifecycle positioning are all directly inspectable and mostly coherent.
- Residual uncertainty: Medium on whether the control-level lightweight preflight in the wrapper prompt is intentionally unique versus unintended drift from release-level prompt behavior.

## Gaps
- Double-wrapper indirection limits prompt-local clarity. Both control and release prompt files are stubs, so operational truth is not visible in the prompt itself.
- Wrapper asymmetry: control prompt injects lightweight preflight, release prompt does not. This can create environment-dependent behavior differences.
- Discoverability split: Lens profile command exists in module-help/module.yaml but not in global BMAD catalogs (skill-manifest and bmad-help), which may confuse users expecting one canonical index.
- Integration drift risk: profile skill marks bmad-lens-onboard integration as deprecated while onboard still governs first-run preflight and role-aware next-step handoff, suggesting the profile-origin narrative may be stale.
- Contract completeness risk: profile skill requires persisted profile.yaml fields but does not reference an explicit schema file in the skill package, increasing the chance of field drift across tools.