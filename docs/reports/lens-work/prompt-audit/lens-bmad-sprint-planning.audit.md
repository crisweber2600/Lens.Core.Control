# lens-bmad-sprint-planning Prompt Audit

## Purpose Summary
- `.github/prompts/lens-bmad-sprint-planning.prompt.md` is a control-repo stub that runs shared lightweight preflight, then delegates to the release prompt.
- `lens.core/_bmad/lens-work/prompts/lens-bmad-sprint-planning.prompt.md` is also a stub and delegates to `bmad-lens-bmad-skill` with `--skill bmad-sprint-planning`.
- Runtime behavior is therefore defined primarily by Lens wrapper policy (`bmad-lens-bmad-skill`), registry metadata (`lens-bmad-skill-registry.json`), and downstream BMAD sprint planning workflow (`_bmad/bmm/4-implementation/bmad-sprint-planning/workflow.md`).

## BMAD Skill Mapping
- Canonical BMAD registration is present in `lens.core/_bmad/_config/skill-manifest.csv` for `bmad-sprint-planning` under module `bmm`, pointing to `_bmad/bmm/4-implementation/bmad-sprint-planning/SKILL.md`.
- Global BMAD help maps `bmad-sprint-planning` to code `SP`, phase `4-implementation`, output location `implementation_artifacts`, and output `sprint status`.
- Lens module registration includes both wrapper skill (`bmad-lens-bmad-skill`) and prompt surface (`lens-bmad-sprint-planning.prompt.md`).
- Lens operational mapping routes this command as `BSP` in phase `finalizeplan`, action `sprint-planning`, output location `docs/implementation-artifacts`, output `sprint status`.
- Wrapper and registry metadata classify `bmad-sprint-planning` as `contextMode=feature-required`, `outputMode=planning-docs`, and `phaseHints=[finalizeplan]`, with entry path to the BMAD 4-implementation sprint planning skill.

## Lifecycle Fit
- Lifecycle fit is strong at Lens orchestration level: `finalizeplan` explicitly includes `sprint-status` and `story-files` artifacts and is in canonical `phase_order` before dev handoff.
- Lens intentionally re-contextualizes BMAD sprint planning from global BMM `4-implementation` into Lens `finalizeplan` so sprint status can be generated during planning bundle/handoff.
- Downstream workflow contract is operationally compatible with this placement because it generates/updates `sprint-status.yaml` from epics/stories, supports preservation of advanced statuses, and validates consistency.
- Net fit judgment: coherent and intentional, but it depends on cross-surface understanding (global BMAD phase labels vs Lens lifecycle phase routing).

## Evidence Refs
- Prompt delegation chain:
  - `.github/prompts/lens-bmad-sprint-planning.prompt.md:5`
  - `.github/prompts/lens-bmad-sprint-planning.prompt.md:14`
  - `.github/prompts/lens-bmad-sprint-planning.prompt.md:18`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-sprint-planning.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-sprint-planning.prompt.md:10`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-sprint-planning.prompt.md:11`
- Required registry/help/module/lifecycle evidence:
  - `lens.core/_bmad/_config/skill-manifest.csv:43`
  - `lens.core/_bmad/_config/bmad-help.csv:27`
  - `lens.core/_bmad/_config/bmad-help.csv:28`
  - `lens.core/_bmad/lens-work/module.yaml:130`
  - `lens.core/_bmad/lens-work/module.yaml:131`
  - `lens.core/_bmad/lens-work/module.yaml:250`
  - `lens.core/_bmad/lens-work/module.yaml:315`
  - `lens.core/_bmad/lens-work/module-help.csv:65`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:177`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:178`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
- Relevant wrapper/skill contracts:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:31`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:32`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:135`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:125`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:130`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:132`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:133`
  - `lens.core/_bmad/bmm/4-implementation/bmad-sprint-planning/SKILL.md:3`
  - `lens.core/_bmad/bmm/4-implementation/bmad-sprint-planning/SKILL.md:6`
  - `lens.core/_bmad/bmm/4-implementation/bmad-sprint-planning/workflow.md:1`
  - `lens.core/_bmad/bmm/4-implementation/bmad-sprint-planning/workflow.md:17`
  - `lens.core/_bmad/bmm/4-implementation/bmad-sprint-planning/workflow.md:18`
  - `lens.core/_bmad/bmm/4-implementation/bmad-sprint-planning/workflow.md:30`
  - `lens.core/_bmad/bmm/4-implementation/bmad-sprint-planning/workflow.md:112`
  - `lens.core/_bmad/bmm/4-implementation/bmad-sprint-planning/workflow.md:122`

## Confidence
- Overall: **High** for prompt routing, skill registration, and lifecycle placement.
- Rationale: prompt stubs, global manifests, Lens module help surfaces, wrapper registry, and lifecycle contract all agree on the command path and expected sprint-status output.
- Residual uncertainty: runtime UX and detailed prompting behavior are owned by the downstream BMAD workflow and can evolve without changing stub prompts.

## Gaps
- Dual-stub indirection (`.github` prompt -> release prompt -> wrapper skill) limits prompt-local visibility of acceptance criteria and failure behavior.
- Cross-surface phase terminology differs: global BMAD help labels sprint planning `4-implementation`, while Lens routes it through `finalizeplan`.
- Output semantics can be confusing across surfaces:
  - Wrapper/registry classify sprint planning as `planning-docs`.
  - Lens module-help output location is `docs/implementation-artifacts`.
  - Downstream workflow computes `status_file` from `{implementation_artifacts}`.
  This is workable but should remain explicitly documented to avoid operator confusion.
- Preflight behavior exists only in the control prompt stub (`light-preflight.py`) and not in the release prompt stub, creating policy split across layers.