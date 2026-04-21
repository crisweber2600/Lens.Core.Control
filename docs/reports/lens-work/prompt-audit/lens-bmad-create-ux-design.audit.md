# lens-bmad-create-ux-design Prompt Audit

## Purpose Summary
- `.github/prompts/lens-bmad-create-ux-design.prompt.md` is a control-repo command stub that enforces `light-preflight.py` and then delegates to the release prompt in `lens.core`.
- The release prompt (`lens.core/_bmad/lens-work/prompts/lens-bmad-create-ux-design.prompt.md`) is also a stub and delegates to `bmad-lens-bmad-skill` with `--skill bmad-create-ux-design`.
- Runtime behavior is therefore determined by Lens wrapper routing (`bmad-lens-bmad-skill`) plus the canonical BMAD UX workflow (`_bmad/bmm/2-plan-workflows/bmad-create-ux-design/workflow.md`), not by prompt-local procedural logic.

## BMAD Skill Mapping
- `skill-manifest.csv` registers `bmad-create-ux-design` in BMM at `_bmad/bmm/2-plan-workflows/bmad-create-ux-design/SKILL.md`.
- `bmad-help.csv` maps this to `Create UX (CU)` in `2-planning`, with `planning_artifacts` output and upstream dependency on `bmad-create-prd`.
- Lens module wiring includes both `bmad-lens-bmad-skill` and `lens-bmad-create-ux-design.prompt.md` in prompt/adapter surfaces.
- Lens operational help maps UX routing to command `BUX` in `businessplan`, action `create-ux-design`, output location `feature.yaml.docs.path`.
- Wrapper policy classifies `bmad-create-ux-design` as `feature-required` + `planning-docs` with businessplan phase hint and write-scope enforcement.

## Lifecycle Fit
- Lifecycle `businessplan` explicitly requires artifacts `prd` and `ux-design`, and phase completion review checks those artifacts before auto-advance to `/techplan`.
- `phase_order` and both `full` and `feature` tracks include `businessplan`, consistent with `BUX` placement in Lens module-help.
- Wrapper write-boundary rules are aligned with lifecycle governance: planning outputs resolve to feature docs scope (`feature.yaml.docs.path`) instead of governance/release roots.
- Control-level preflight gate is lifecycle-positive (workspace sync check before delegation), but it exists only in the control stub, not in the release prompt.

## Evidence Refs
- Prompt delegation and preflight chain:
  - `.github/prompts/lens-bmad-create-ux-design.prompt.md:5`
  - `.github/prompts/lens-bmad-create-ux-design.prompt.md:14`
  - `.github/prompts/lens-bmad-create-ux-design.prompt.md:18`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-create-ux-design.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-create-ux-design.prompt.md:10`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-create-ux-design.prompt.md:11`
- Required registry/help/module/lifecycle evidence:
  - `lens.core/_bmad/_config/skill-manifest.csv:24`
  - `lens.core/_bmad/_config/bmad-help.csv:16`
  - `lens.core/_bmad/lens-work/module.yaml:130`
  - `lens.core/_bmad/lens-work/module.yaml:246`
  - `lens.core/_bmad/lens-work/module.yaml:311`
  - `lens.core/_bmad/lens-work/module-help.csv:49`
  - `lens.core/_bmad/lens-work/module-help.csv:61`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:129`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:137`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:144`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:244`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:252`
- Relevant prompt/skill/workflow contracts:
  - `.github/skills/bmad-create-ux-design/SKILL.md:6`
  - `.github/skills/bmad-create-ux-design/SKILL.md:13`
  - `lens.core/_bmad/bmm/2-plan-workflows/bmad-create-ux-design/SKILL.md:6`
  - `lens.core/_bmad/bmm/2-plan-workflows/bmad-create-ux-design/workflow.md:30`
  - `lens.core/_bmad/bmm/2-plan-workflows/bmad-create-ux-design/workflow.md:36`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:31`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:32`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:33`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:70`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:74`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:131`

## Confidence
- Overall: High.
- Rationale: prompt routing, global BMAD mappings, Lens module command surfaces, and lifecycle artifact requirements all converge on a consistent `businessplan` UX-design execution path.
- Residual uncertainty: medium-low for runtime interaction details because both prompt layers are intentionally stub-based and defer behavior to wrapper/workflow files.

## Gaps
- Double-stub indirection reduces prompt-local transparency: acceptance criteria and detailed UX workflow behavior are not visible in either prompt body.
- Preflight asymmetry: control stub enforces `light-preflight.py`, while release prompt is pure delegation; if operators bypass control stubs, preflight expectations can drift.
- Output naming mismatch risk: Lens businessplan help advertises `ux-design.md` in phase outputs, but canonical UX workflow default output is `{planning_artifacts}/ux-design-specification.md`.
- Cross-surface terminology differences (`2-planning` in BMAD help vs `businessplan` in Lens lifecycle/module-help) are coherent but increase discoverability and onboarding friction.