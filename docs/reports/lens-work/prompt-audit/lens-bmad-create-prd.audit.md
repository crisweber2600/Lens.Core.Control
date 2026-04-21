# lens-bmad-create-prd Prompt Audit

## Purpose Summary
- `.github/prompts/lens-bmad-create-prd.prompt.md` is a control-repo command stub that adds a mandatory lightweight preflight, then delegates to the release prompt in `lens.core`.
- The release prompt (`lens.core/_bmad/lens-work/prompts/lens-bmad-create-prd.prompt.md`) is itself a delegation stub that routes through `bmad-lens-bmad-skill` with `--skill bmad-create-prd`.
- Effective behavior therefore comes from the wrapper (`bmad-lens-bmad-skill`) plus the downstream BMAD PRD workflow (`bmad-create-prd`), not from prompt body logic.

## BMAD Skill Mapping
- Global skill registry explicitly defines canonical `bmad-create-prd` as the BMM 2-planning skill at `_bmad/bmm/2-plan-workflows/bmad-create-prd/SKILL.md`.
- Global help maps `bmad-create-prd` to `Create PRD (CP)` in `2-planning`, with `planning_artifacts` output and direct coupling to PRD-adjacent flows (`create-ux-design`, `validate-prd`).
- Lens module wiring includes `lens-bmad-create-prd.prompt.md` in prompt surfaces and `.github` command adapters, confirming command discoverability across installed agent shells.
- Lens operational help maps this to `bmad-lens-bmad-skill` command `BPR` in `businessplan`, using action `create-prd` and output location `feature.yaml.docs.path`.
- Wrapper contract classifies `bmad-create-prd` as `feature-required` + `planning-docs` with phase hint `businessplan`, and enforces planning write boundaries through resolved `write_scope`.

## Lifecycle Fit
- Lifecycle contract defines `businessplan` as the phase producing `prd` and `ux-design`, and marks those artifacts as readiness requirements for phase completion review.
- `phase_order` places `businessplan` between `preplan` and `techplan`; `full` and `feature` tracks both include `businessplan`, matching where `BPR` is surfaced in `module-help.csv`.
- The wrapper-based mapping in Lens help (`BPR` in `businessplan`) is consistent with lifecycle semantics and with required planning-doc output location discipline (`feature.yaml.docs.path`).
- The control stub’s explicit `light-preflight.py` gate strengthens lifecycle hygiene by preventing delegation when workspace sync checks fail.

## Evidence Refs
- Target control prompt and delegation chain:
  - `.github/prompts/lens-bmad-create-prd.prompt.md:5`
  - `.github/prompts/lens-bmad-create-prd.prompt.md:14`
  - `.github/prompts/lens-bmad-create-prd.prompt.md:18`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-create-prd.prompt.md:2`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-create-prd.prompt.md:11`
- Required registry/help/module/lifecycle artifacts:
  - `lens.core/_bmad/_config/skill-manifest.csv:23`
  - `lens.core/_bmad/_config/bmad-help.csv:13`
  - `lens.core/_bmad/_config/bmad-help.csv:16`
  - `lens.core/_bmad/_config/bmad-help.csv:30`
  - `lens.core/_bmad/lens-work/module.yaml:245`
  - `lens.core/_bmad/lens-work/module.yaml:310`
  - `lens.core/_bmad/lens-work/module-help.csv:49`
  - `lens.core/_bmad/lens-work/module-help.csv:60`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:129`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:137`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:144`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:246`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:254`
- Relevant prompt/skill files shaping real behavior:
  - `.github/skills/bmad-create-prd/SKILL.md:5`
  - `lens.core/_bmad/bmm/2-plan-workflows/bmad-create-prd/SKILL.md:6`
  - `lens.core/_bmad/bmm/2-plan-workflows/bmad-create-prd/workflow.md:3`
  - `lens.core/_bmad/bmm/2-plan-workflows/bmad-create-prd/workflow.md:62`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:31`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:32`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:70`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:74`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:130`

## Confidence
- Overall: High.
- Rationale: mapping from control prompt → release prompt → Lens wrapper → canonical BMAD create-prd workflow is direct and internally consistent across all required registry/help/module/lifecycle surfaces.
- Residual uncertainty: medium-low around runtime UX details, because two prompt layers are stubs and defer operational behavior to downstream skill/workflow files.

## Gaps
- Double-stub layering reduces local observability: both prompt files are declarative routers, so acceptance criteria and output quality checks are not visible at prompt level.
- The control prompt adds `light-preflight.py` while the release prompt only delegates; this split is useful but can drift if one side changes without synchronized updates.
- No prompt-level assertion guarantees that downstream output is always concretely named `prd.md`; that is implied by downstream workflow (`outputFile: {planning_artifacts}/prd.md`) rather than enforced in prompt text.
- Discoverability is distributed across multiple surfaces (module prompt list, adapters, global help, module-help, and skill registry), so mapping regressions can occur if one registry entry changes without companion updates.