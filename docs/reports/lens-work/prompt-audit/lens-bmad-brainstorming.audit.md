# lens-bmad-brainstorming Prompt Audit

## Purpose Summary
- `.github/prompts/lens-bmad-brainstorming.prompt.md` is a control-repo command stub that performs a lightweight preflight, then delegates to the release prompt in `lens.core`.
- The release prompt (`lens.core/_bmad/lens-work/prompts/lens-bmad-brainstorming.prompt.md`) is also a stub and delegates execution to `bmad-lens-bmad-skill` with `--skill bmad-brainstorming`.
- Effective behavior is therefore defined by Lens BMAD wrapper policy (`bmad-lens-bmad-skill`) plus the downstream core brainstorming skill (`bmad-brainstorming` workflow).

## BMAD Skill Mapping
- Global BMAD registry maps `bmad-brainstorming` to the core skill file (`_bmad/core/bmad-brainstorming/SKILL.md`), confirming canonical skill identity.
- Global help surfaces show brainstorming as an ideation/analysis capability (`Brainstorm Project` in `1-analysis`, plus core `anytime` brainstorming discoverability).
- Lens module wiring maps the command to `bmad-lens-bmad-skill` with prompt registration for `lens-bmad-brainstorming.prompt.md`.
- Lens operational help binds this command to `preplan` via menu code `BBS` and output location `feature.yaml.docs.path`.
- Lens skill registry confirms the specific command contract: `contextMode=feature-optional`, `outputMode=planning-docs`, `phaseHints=[preplan]`, and entry path to the core brainstorming skill.

## Lifecycle Fit
- Lifecycle alignment is strong: `preplan` explicitly includes `brainstorm` artifacts and requires brainstorm output for readiness/review.
- `phase_order` and the `full` track both begin with `preplan`, so this command fits phase entry behavior naturally.
- The preplan conductor skill enforces brainstorm-first interactive flow and routes brainstorming through `bmad-lens-bmad-skill` using governance-only context and control-repo docs staging.
- Wrapper write-boundary rules for planning-doc skills block direct governance and `.github` writes, which is consistent with staged artifact discipline before governance publication.

## Evidence Refs
- Control prompt stub + preflight + path semantics:
  - `.github/prompts/lens-bmad-brainstorming.prompt.md:7`
  - `.github/prompts/lens-bmad-brainstorming.prompt.md:9`
  - `.github/prompts/lens-bmad-brainstorming.prompt.md:14`
- Release prompt delegation:
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-brainstorming.prompt.md:10`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-brainstorming.prompt.md:11`
- Required registry/help/module/lifecycle sources:
  - `lens.core/_bmad/_config/skill-manifest.csv:3`
  - `lens.core/_bmad/_config/bmad-help.csv:7`
  - `lens.core/_bmad/_config/bmad-help.csv:37`
  - `lens.core/_bmad/lens-work/module.yaml:130`
  - `lens.core/_bmad/lens-work/module.yaml:131`
  - `lens.core/_bmad/lens-work/module.yaml:240`
  - `lens.core/_bmad/lens-work/module.yaml:305`
  - `lens.core/_bmad/lens-work/module-help.csv:55`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:109`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:114`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:118`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:125`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:126`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:246`
- Relevant prompt/skill contracts:
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:5`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:11`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:12`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:13`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:31`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:32`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:64`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:76`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:125`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md:32`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md:63`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md:80`
  - `lens.core/_bmad/core/bmad-brainstorming/SKILL.md:6`
  - `lens.core/_bmad/core/bmad-brainstorming/workflow.md:43`

## Confidence
- Overall: **High** for mapping and lifecycle fit.
- Rationale: required registry/help/module/lifecycle artifacts are internally consistent and directly cross-reference the same delegation chain.
- Residual uncertainty: prompt-level behavior remains mostly declarative because both prompts are stubs; execution details live in downstream skill/workflow files.

## Gaps
- Double-stub indirection means prompt text alone does not expose acceptance criteria, failure handling, or output schema; operational guarantees depend on downstream skills.
- The control prompt uniquely adds `light-preflight.py`, while the release prompt focuses only on delegation. This split can drift if one side changes independently.
- Global help metadata shows brainstorming as both `1-analysis` and `anytime`; Lens command mapping pins it to `preplan`. This is workable, but could confuse discoverability semantics across surfaces.
- No prompt-embedded assertion verifies that delegated brainstorming output is named/stored as `brainstorm.md`; that contract is enforced by preplan/wrapper behavior rather than the prompt itself.