# lens-retrospective Prompt Audit

## Purpose Summary
- `.github/prompts/lens-retrospective.prompt.md` is a control-surface stub that delegates to the release prompt and does not define operational behavior.
- `lens.core/_bmad/lens-work/prompts/lens-retrospective.prompt.md` is also a stub; it delegates directly to `bmad-lens-retrospective`.
- Effective behavior lives in `lens.core/_bmad/lens-work/skills/bmad-lens-retrospective/SKILL.md`: analyze feature problem history, generate a retrospective report, and update cross-feature insights.
- Operationally, this prompt is a learning/feedback surface, not a phase-conductor. It is used to convert problem logs into reusable governance memory.

## BMAD Skill Mapping
- Prompt chain:
  - `.github/prompts/lens-retrospective.prompt.md`
  - `lens.core/_bmad/lens-work/prompts/lens-retrospective.prompt.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-retrospective/SKILL.md`
- Lens module registration:
  - `lens.core/_bmad/lens-work/module.yaml` registers `bmad-lens-retrospective` and exports `lens-retrospective.prompt.md` in the module prompt catalog and adapter stubs.
- Lens command/help mapping:
  - `lens.core/_bmad/lens-work/module-help.csv` maps `bmad-lens-retrospective` to:
    - `AP` / `analyze-problems` (action `analyze`)
    - `GR` / `generate-report` (action `generate-report`)
    - `UI` / `update-insights` (action `update-insights`)
  - All three are marked `anytime`, with explicit dependency chain `analyze -> generate-report -> update-insights`.
- Global BMAD relation:
  - `lens.core/_bmad/_config/skill-manifest.csv` and `lens.core/_bmad/_config/bmad-help.csv` define canonical `bmad-retrospective` (BMM 4-implementation, optional at epic end).
  - Lens does not route this prompt through `bmad-lens-bmad-skill`; it is a Lens-native retrospective skill with governance-specific outputs.
- Completion integration:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md` defines retrospective as first-class and retrospective-first before archive/finalization.

## Lifecycle Fit
- Fit is strong for lifecycle closure and learning loops, but it is not part of canonical phase progression:
  - `lifecycle.yaml` canonical `phase_order` remains `preplan -> businessplan -> techplan -> finalizeplan`.
  - Retrospective is surfaced as an `anytime` operation in `module-help.csv`, which is consistent with cross-phase diagnostic use.
- Fit with close semantics:
  - `lifecycle.yaml` defines terminal close states and dev-complete validation concepts; retrospective provides the learning artifact expected before archival decisions.
  - `bmad-lens-complete` explicitly requires/strongly prioritizes retrospective before finalizing archive state.
- Cross-surface phase vocabulary caveat:
  - `bmad-lens-retrospective` vocabulary includes `dev` and `complete` as problem phases, while lifecycle comments explicitly note `dev` is not a lifecycle phase in canonical phase order.
  - This is operationally workable (retrospective tags incident timing), but semantically different from lifecycle phase taxonomy.

## Evidence Refs
- Prompt delegation chain:
  - `.github/prompts/lens-retrospective.prompt.md`
  - `lens.core/_bmad/lens-work/prompts/lens-retrospective.prompt.md`
- Lens retrospective behavior contract:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-retrospective/SKILL.md`
- Required registry/help/module/lifecycle sources:
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`
  - `lens.core/_bmad/lens-work/module.yaml`
  - `lens.core/_bmad/lens-work/module-help.csv`
  - `lens.core/_bmad/lens-work/lifecycle.yaml`
- Relevant supporting prompt/skill contracts:
  - `.github/skills/bmad-retrospective/SKILL.md`
  - `lens.core/_bmad/bmm/4-implementation/bmad-retrospective/SKILL.md`
  - `lens.core/_bmad/bmm/4-implementation/bmad-retrospective/workflow.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md`

## Confidence
- Overall: **High** for prompt routing, Lens skill mapping, and module/lifecycle surface alignment.
- Rationale: all required metadata surfaces consistently indicate a stub-to-skill chain and a Lens-local command surface for retrospective operations.
- Residual uncertainty: **Medium-Low** for strict runtime enforcement details (for example, whether every retrospective-first policy statement is hard-blocked at script level), since this audit is contract/document based.

## Gaps
- Double-stub indirection reduces prompt-local transparency; meaningful behavior is only visible after following delegation into skill contracts.
- Discoverability split across global BMAD registries and Lens-local module registries can confuse operators:
  - Global catalogs describe canonical `bmad-retrospective`.
  - Lens command behavior is authoritative in `module.yaml` and `module-help.csv`.
- Taxonomy drift risk:
  - Retrospective skill tags include `dev` and `complete`, while canonical lifecycle phase ordering excludes `dev` as a true phase and does not include a dedicated `complete` phase in `phase_order`.
- Prompt-level acceptance criteria and failure semantics are not embedded in prompt text; they are inherited from downstream skill contracts and references.
