# lens-bmad-technical-research Prompt Audit

## Purpose Summary
- `.github/prompts/lens-bmad-technical-research.prompt.md` is a control-repo stub that runs shared lightweight preflight and delegates to the release prompt in `lens.core`.
- The release prompt (`lens.core/_bmad/lens-work/prompts/lens-bmad-technical-research.prompt.md`) is also a stub; it delegates to `bmad-lens-bmad-skill` with `--skill bmad-technical-research`.
- Effective runtime behavior therefore comes from the Lens wrapper contract and the downstream BMAD technical-research workflow, not from prompt-local logic.

## BMAD Skill Mapping
- Canonical BMAD skill identity is present in `_config/skill-manifest.csv`: `bmad-technical-research` maps to `_bmad/bmm/1-analysis/research/bmad-technical-research/SKILL.md`.
- `_config/bmad-help.csv` maps this to BMad Method `TR` in `1-analysis`, with research-document outputs under planning/project knowledge.
- Lens module wiring includes `lens-bmad-technical-research.prompt.md` in prompt surfaces and exposes `bmad-lens-bmad-skill` for delegated BMAD execution.
- Lens operational help maps `BTR` to `bmad-lens-bmad-skill` action `technical-research` in `preplan`, outputting to `feature.yaml.docs.path`.
- Lens skill registry aligns the command to `contextMode=feature-required`, `outputMode=planning-docs`, and `phaseHints=[preplan]` for `bmad-technical-research`.
- Downstream BMAD skill is a stub that delegates to `workflow.md`, where the technical-research contract is defined (including mandatory web-search capability and technical output naming).

## Lifecycle Fit
- The lifecycle contract defines `preplan` as the analysis phase and includes `research` as a required planning artifact set, consistent with this skill's `preplan` routing.
- `module-help.csv` places `BTR` under `preplan`, matching lifecycle and registry expectations.
- `module.yaml` includes both the phase conductor (`bmad-lens-preplan`) and the technical-research prompt entry, which is consistent with Lens' delegated phase orchestration model.
- Wrapper write-scope behavior for planning-doc skills (`feature.yaml.docs.path` authority and governance direct-write blocking) matches lifecycle discipline that planning artifacts are staged in control-repo docs paths before downstream governance publication.

## Evidence Refs
- Control prompt stub + preflight + delegation:
  - `.github/prompts/lens-bmad-technical-research.prompt.md:5`
  - `.github/prompts/lens-bmad-technical-research.prompt.md:14`
  - `.github/prompts/lens-bmad-technical-research.prompt.md:18`
- Control skill stub delegation:
  - `.github/skills/bmad-technical-research/SKILL.md:2`
  - `.github/skills/bmad-technical-research/SKILL.md:6`
  - `.github/skills/bmad-technical-research/SKILL.md:10`
  - `.github/skills/bmad-technical-research/SKILL.md:13`
- Release prompt delegation to Lens wrapper:
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-technical-research.prompt.md:2`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-technical-research.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-technical-research.prompt.md:10`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-technical-research.prompt.md:11`
- Required registry/help/module/lifecycle surfaces:
  - `lens.core/_bmad/_config/skill-manifest.csv:20`
  - `lens.core/_bmad/_config/bmad-help.csv:29`
  - `lens.core/_bmad/lens-work/module.yaml:105`
  - `lens.core/_bmad/lens-work/module.yaml:244`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:109`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:117`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
  - `lens.core/_bmad/lens-work/module-help.csv:48`
  - `lens.core/_bmad/lens-work/module-help.csv:59`
- Relevant prompt/skill wrapper + downstream workflow contracts:
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:53`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:58`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:59`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:60`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:61`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:31`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:32`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:62`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:70`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:95`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:129`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-technical-research/SKILL.md:2`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-technical-research/SKILL.md:6`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-technical-research/workflow.md:10`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-technical-research/workflow.md:12`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-technical-research/workflow.md:15`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-technical-research/workflow.md:38`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-technical-research/workflow.md:42`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-technical-research/workflow.md:45`

## Confidence
- Overall: **High** for mapping/routing/lifecycle alignment.
- Rationale: the control prompt, release prompt, skill registry, help surfaces, module wiring, lifecycle contract, wrapper semantics, and downstream workflow all consistently map technical research to Lens `preplan` via delegated execution.
- Residual uncertainty: **Medium-Low** for runtime interaction details because both prompt layers and SKILL entrypoints are intentionally stubs; concrete behavior is delegated to wrapper/workflow execution at runtime.

## Gaps
- Double-stub layering (control prompt and release prompt) keeps most behavior out of the prompt itself; operators must inspect wrapper and downstream workflow to understand true execution semantics.
- The workflow's hard prerequisite (`Web search required`) is not surfaced in the release prompt stub text; it only appears in downstream workflow content.
- Discoverability is spread across multiple surfaces (`module.yaml`, `module-help.csv`, `_config` manifests, and `lens-bmad-skill-registry.json`), which raises drift risk if one mapping is changed without synchronized updates.
- `_config/bmad-help.csv` exposes base BMAD `TR` metadata, while Lens-specific wrapper routing (`BTR`, output to `feature.yaml.docs.path`) is only in `module-help.csv`; audits that read only global help can miss Lens write-boundary semantics.