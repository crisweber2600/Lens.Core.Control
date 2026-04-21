# lens-techplan Prompt Audit

## Purpose Summary
- `.github/prompts/lens-techplan.prompt.md` is a control-layer delegation stub.
- The release-layer prompt `lens.core/_bmad/lens-work/prompts/lens-techplan.prompt.md` is also a stub and delegates to `bmad-lens-techplan`.
- Effective TechPlan behavior is owned by `lens.core/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md`: publish reviewed BusinessPlan artifacts, run architecture through Lens BMAD wrapper routing, enforce review gate, and advance phase state.
- Architecture authoring is intentionally delegated through `bmad-lens-bmad-skill` to downstream `bmad-create-architecture` rather than authored by the TechPlan conductor itself.

## BMAD Skill Mapping
- Prompt chain mapping:
  - `.github/prompts/lens-techplan.prompt.md` -> `lens.core/_bmad/lens-work/prompts/lens-techplan.prompt.md` -> `lens.core/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md`.
- Lens module registration maps both prompt and skill surfaces for TechPlan in `lens.core/_bmad/lens-work/module.yaml`.
- Lens command-surface mapping in `lens.core/_bmad/lens-work/module-help.csv` binds `bmad-lens-techplan` to menu/action `techplan` (`LT`) in phase `phase-3`.
- `bmad-lens-techplan` maps architecture work to the Lens BMAD wrapper (`bmad-lens-bmad-skill`) using `--skill bmad-create-architecture` (also reflected by the wrapper prompt `lens-bmad-create-architecture.prompt.md`).
- Global BMAD catalogs (`lens.core/_bmad/_config/skill-manifest.csv`, `lens.core/_bmad/_config/bmad-help.csv`) canonically register `bmad-create-architecture` for solutioning but do not list Lens-local wrapper skill IDs like `bmad-lens-techplan`; discoverability is module-local for Lens phase conductors.

## Lifecycle Fit
- Strong fit: `lens.core/_bmad/lens-work/lifecycle.yaml` defines `techplan` as the technical-design phase with required artifact `architecture` and auto-advance to `finalizeplan`.
- `bmad-lens-techplan` aligns with lifecycle sequencing by requiring BusinessPlan completion (or valid track skip), validating `review-ready` artifacts, then invoking adversarial review before phase completion.
- TechPlan’s staged-authoring model (control repo docs path first, governance publication sequencing, no direct governance file patching) is consistent with Lens lifecycle and governance discipline.
- Track alignment is coherent:
  - `full` and `feature` include `techplan` in normal order.
  - `tech-change` and `hotfix` start at `techplan`, which matches this prompt’s role as architecture entrypoint.
  - `express` bypasses standalone `techplan` because it runs `expressplan` then `finalizeplan`; this is lifecycle-consistent and does not conflict with `bmad-lens-techplan`.

## Evidence Refs
- Prompt chain:
  - `.github/prompts/lens-techplan.prompt.md`
  - `lens.core/_bmad/lens-work/prompts/lens-techplan.prompt.md`
- Required registry/help/module/lifecycle sources:
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`
  - `lens.core/_bmad/lens-work/module.yaml`
  - `lens.core/_bmad/lens-work/module-help.csv`
  - `lens.core/_bmad/lens-work/lifecycle.yaml`
- Relevant prompt/skill contracts:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-create-architecture.prompt.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md`
  - `docs/reports/lens-work/prompt-audit/lens-bmad-create-architecture.audit.md`

## Confidence
- Overall: **High** for routing and lifecycle alignment.
- Rationale: all required surfaces (prompt stubs, Lens module registry, Lens help surface, lifecycle contract, and wrapper/skill contracts) align on TechPlan’s phase role and architecture delegation chain.
- Residual uncertainty: **Medium-Low** for runtime UX details because both prompt files are stubs and most behavior resides in skill-level contracts and delegated workflows.

## Gaps
- Double-stub indirection reduces prompt-local transparency; meaningful behavior is in skill contracts rather than prompt text.
- Discoverability split persists between global BMAD manifests (canonical base skills like `bmad-create-architecture`) and Lens-local conductor skills (`bmad-lens-techplan`).
- Phase semantics in `module-help.csv` use `phase-3` labels while lifecycle contract uses canonical phase IDs (`preplan`, `businessplan`, `techplan`, `finalizeplan`), creating mild terminology drift risk.
- TechPlan runtime assurance depends on delegated contracts (`bmad-lens-bmad-skill` and downstream architecture workflow); prompt-level acceptance criteria remain implicit.
