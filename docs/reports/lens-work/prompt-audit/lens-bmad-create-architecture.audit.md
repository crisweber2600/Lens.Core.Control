# lens-bmad-create-architecture Prompt Audit

## Purpose Summary
- `.github/prompts/lens-bmad-create-architecture.prompt.md` is a control-repo wrapper prompt that runs a lightweight preflight and then delegates to the release prompt under `lens.core`.
- The release prompt (`lens.core/_bmad/lens-work/prompts/lens-bmad-create-architecture.prompt.md`) is also a stub; it delegates execution to `bmad-lens-bmad-skill` with `--skill bmad-create-architecture`.
- Effective behavior is therefore governed by the Lens BMAD wrapper contract and the downstream BMAD architecture workflow (`bmad-create-architecture` + `workflow.md`).

## BMAD Skill Mapping
- Global registry mapping is consistent: `bmad-create-architecture` is registered to `_bmad/bmm/3-solutioning/bmad-create-architecture/SKILL.md`.
- Global BMAD help maps this command to `Create Architecture (CA)` in `3-solutioning`, with output artifact `architecture`.
- Lens module wiring includes both the wrapper skill (`bmad-lens-bmad-skill`) and the prompt registration for `lens-bmad-create-architecture.prompt.md`.
- Lens module help binds this command to menu code `BAR`, phase `techplan`, and output location `feature.yaml.docs.path`.
- Lens BMAD skill registry further constrains execution for this command to `contextMode=feature-required`, `outputMode=planning-docs`, `phaseHints=[techplan]`, and the same downstream `entryPath`.

## Lifecycle Fit
- Fit is strong: lifecycle `techplan` defines architecture as the phase artifact and requires it for completion review readiness.
- The phase contract and tracks (`full`, `feature`, `tech-change`, `hotfix`) all route architecture work through `techplan` or start at `techplan`, matching the `BAR` command's phase binding.
- Wrapper policy aligns with governance discipline: planning-docs mode enforces write scope to planning docs and blocks direct writes to governance/release/.github paths.
- The downstream architecture skill is intentionally thin (`SKILL.md -> workflow.md`), so practical behavior is controlled by workflow steps and Lens context injection rather than prompt prose.

## Evidence Refs
- Control prompt stub + preflight + delegation:
  - `.github/prompts/lens-bmad-create-architecture.prompt.md:7`
  - `.github/prompts/lens-bmad-create-architecture.prompt.md:12`
  - `.github/prompts/lens-bmad-create-architecture.prompt.md:14`
  - `.github/prompts/lens-bmad-create-architecture.prompt.md:18`
- Release prompt delegation:
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-create-architecture.prompt.md:7`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-create-architecture.prompt.md:10`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-create-architecture.prompt.md:11`
- Required registry/help/module/lifecycle sources:
  - `lens.core/_bmad/_config/skill-manifest.csv:29`
  - `lens.core/_bmad/_config/bmad-help.csv:11`
  - `lens.core/_bmad/lens-work/module.yaml:130`
  - `lens.core/_bmad/lens-work/module.yaml:131`
  - `lens.core/_bmad/lens-work/module.yaml:247`
  - `lens.core/_bmad/lens-work/module.yaml:312`
  - `lens.core/_bmad/lens-work/module-help.csv:62`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:148`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:154`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:158`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:163`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:246`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:255`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:264`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:274`
- Relevant prompt/skill contracts:
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:91`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:94`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:95`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:96`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:97`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:70`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:74`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:77`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:132`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-create-architecture/SKILL.md:6`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-create-architecture/workflow.md:3`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-create-architecture/workflow.md:25`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-create-architecture/workflow.md:35`

## Confidence
- Overall: **High** for mapping and lifecycle alignment.
- Rationale: all required discovery surfaces (manifest/help/module/lifecycle/registry) agree on phase and artifact contract for `bmad-create-architecture`.
- Residual uncertainty: execution details are mostly indirect due to dual prompt stubs and the thin `SKILL.md` handoff to workflow files.

## Gaps
- Dual-stub indirection reduces prompt-level observability: failure handling, expected output schema, and acceptance criteria are not visible in prompt files themselves.
- Control prompt adds `light-preflight.py` while the release prompt only delegates; this split is valid but susceptible to drift if one side changes independently.
- The architecture skill's `SKILL.md` contains only a workflow handoff, so behavior guarantees depend on `workflow.md` and step files rather than a single declarative contract.
- No prompt-level assertion confirms canonical output filename/location for architecture artifacts; that guarantee is implied by lifecycle/help/registry contracts and wrapper write-scope rules.