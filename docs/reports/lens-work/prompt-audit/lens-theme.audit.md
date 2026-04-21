# lens-theme Prompt Audit

## Purpose Summary
- The control-repo prompt is a stub wrapper. [../../../../.github/prompts/lens-theme.prompt.md](../../../../.github/prompts/lens-theme.prompt.md)
- The release prompt is also a stub and delegates behavior to the theme skill. [../../../../lens.core/_bmad/lens-work/prompts/lens-theme.prompt.md](../../../../lens.core/_bmad/lens-work/prompts/lens-theme.prompt.md)
- Functional behavior lives in the skill and script layer: load, list, set preference, and easter-egg checks. [../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/SKILL.md](../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/SKILL.md), [../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/scripts/theme-ops.py](../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/scripts/theme-ops.py)
- The skill explicitly constrains theme behavior to persona/wording overlays only and forbids capability changes, which is a strong safety boundary for lifecycle orchestration. [../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/SKILL.md](../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/SKILL.md)

## BMAD Skill Mapping
- Lens-local authoritative mapping exists in module metadata: bmad-lens-theme is registered as a Lens skill with description Theme loading and persona overlay system. [../../../../lens.core/_bmad/lens-work/module.yaml](../../../../lens.core/_bmad/lens-work/module.yaml)
- Lens command mapping is explicit: apply-theme with menu code TH, action load, phase anytime. [../../../../lens.core/_bmad/lens-work/module-help.csv](../../../../lens.core/_bmad/lens-work/module-help.csv)
- Global BMAD registries do not currently expose bmad-lens-theme directly. The global manifests focus on core, bmm, cis, gds, tea, and wds surfaces rather than Lens-local theme overlays. [../../../../lens.core/_bmad/_config/skill-manifest.csv](../../../../lens.core/_bmad/_config/skill-manifest.csv), [../../../../lens.core/_bmad/_config/bmad-help.csv](../../../../lens.core/_bmad/_config/bmad-help.csv)
- Practical mapping result: lens-theme is a Lens platform skill, not a globally promoted BMAD workflow skill.

## Lifecycle Fit
- Fit is strong as a cross-cutting runtime overlay because module-help marks it anytime, and the skill is designed to be loaded on activation by user-facing Lens skills. [../../../../lens.core/_bmad/lens-work/module-help.csv](../../../../lens.core/_bmad/lens-work/module-help.csv), [../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/SKILL.md](../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/SKILL.md)
- Fit is intentionally non-gating. No lifecycle phase transitions or artifact gates are implemented by this prompt/skill; it only affects persona expression while lifecycle state remains governed elsewhere. [../../../../lens.core/_bmad/lens-work/lifecycle.yaml](../../../../lens.core/_bmad/lens-work/lifecycle.yaml)
- This separation is consistent with lifecycle architecture where canonical ordered phases are preplan, businessplan, techplan, and finalizeplan, with expressplan as standalone in tracks; theme does not alter that contract. [../../../../lens.core/_bmad/lens-work/lifecycle.yaml](../../../../lens.core/_bmad/lens-work/lifecycle.yaml)

## Evidence Refs
- Prompt chain
- [../../../../.github/prompts/lens-theme.prompt.md](../../../../.github/prompts/lens-theme.prompt.md)
- [../../../../lens.core/_bmad/lens-work/prompts/lens-theme.prompt.md](../../../../lens.core/_bmad/lens-work/prompts/lens-theme.prompt.md)
- Required registries and module surfaces
- [../../../../lens.core/_bmad/_config/skill-manifest.csv](../../../../lens.core/_bmad/_config/skill-manifest.csv)
- [../../../../lens.core/_bmad/_config/bmad-help.csv](../../../../lens.core/_bmad/_config/bmad-help.csv)
- [../../../../lens.core/_bmad/lens-work/module.yaml](../../../../lens.core/_bmad/lens-work/module.yaml)
- [../../../../lens.core/_bmad/lens-work/module-help.csv](../../../../lens.core/_bmad/lens-work/module-help.csv)
- [../../../../lens.core/_bmad/lens-work/lifecycle.yaml](../../../../lens.core/_bmad/lens-work/lifecycle.yaml)
- Relevant skill implementation
- [../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/SKILL.md](../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/SKILL.md)
- [../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/scripts/theme-ops.py](../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/scripts/theme-ops.py)

## Confidence
- Overall: High.
- Reason: The prompt path is explicit and short (stub to stub to skill), and module/module-help/lifecycle files are consistent about lifecycle scope versus theme scope.
- Residual uncertainty: Medium on full runtime theme catalogs in governance repos because this audit focused on contract surfaces and script behavior, not a specific governance repo theme inventory snapshot.

## Gaps
- Double-stub indirection reduces prompt-local transparency. Both control and release prompt files defer behavior entirely to the skill, so prompt-only review cannot validate operational detail. [../../../../.github/prompts/lens-theme.prompt.md](../../../../.github/prompts/lens-theme.prompt.md), [../../../../lens.core/_bmad/lens-work/prompts/lens-theme.prompt.md](../../../../lens.core/_bmad/lens-work/prompts/lens-theme.prompt.md)
- Discoverability is split across Lens-local and global BMAD surfaces. Lens users can execute apply-theme via module-help, but the global BMAD registries do not provide a direct bmad-lens-theme entry, which may confuse users expecting one consolidated index. [../../../../lens.core/_bmad/lens-work/module-help.csv](../../../../lens.core/_bmad/lens-work/module-help.csv), [../../../../lens.core/_bmad/_config/skill-manifest.csv](../../../../lens.core/_bmad/_config/skill-manifest.csv), [../../../../lens.core/_bmad/_config/bmad-help.csv](../../../../lens.core/_bmad/_config/bmad-help.csv)
- On-activation defaults reference governance repo and user profile paths that are configuration-dependent; when those are missing, fallback behavior is graceful but largely silent, which can hinder diagnostics without explicit troubleshooting mode. [../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/SKILL.md](../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/SKILL.md), [../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/scripts/theme-ops.py](../../../../lens.core/_bmad/lens-work/skills/bmad-lens-theme/scripts/theme-ops.py)
