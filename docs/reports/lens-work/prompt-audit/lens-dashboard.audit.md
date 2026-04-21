# lens-dashboard Prompt Audit

## Purpose Summary
- `.github/prompts/lens-dashboard.prompt.md` is a thin entry stub that delegates to the release prompt path.
- `lens.core/_bmad/lens-work/prompts/lens-dashboard.prompt.md` is also a stub; runtime intent is delegated to `bmad-lens-dashboard`.
- The effective purpose (from skill + script) is to generate a self-contained cross-feature HTML dashboard from governance data, including feature overview, dependency graph, problem heatmap, sprint progress, and retrospective availability.
- The operational center of gravity is `lens.core/_bmad/lens-work/skills/bmad-lens-dashboard/SKILL.md` plus `scripts/dashboard-ops.py`, not prompt body text.

## BMAD Skill Mapping
- Prompt chain mapping:
  - `.github/prompts/lens-dashboard.prompt.md` -> `lens.core/_bmad/lens-work/prompts/lens-dashboard.prompt.md` -> `bmad-lens-dashboard` skill.
- Lens module registration:
  - `lens.core/_bmad/lens-work/module.yaml` registers `bmad-lens-dashboard` with description "Cross-feature dashboard generator with dependency graphs" and includes `lens-dashboard.prompt.md` in prompt inventory.
- Lens command surface mapping:
  - `lens.core/_bmad/lens-work/module-help.csv` maps `bmad-lens-dashboard` into three anytime commands:
    - `collect-data` (`CD`) action `collect`
    - `dependency-graph` (`DG`) action `dependency-data`
    - `generate-dashboard` (`GD`) action `generate`
  - `generate-dashboard` declares `after=bmad-lens-dashboard:collect`, implying intended two-step flow before HTML generation.
- Global registry scope mismatch:
  - `lens.core/_bmad/_config/skill-manifest.csv` has no `bmad-lens-dashboard` entry.
  - `lens.core/_bmad/_config/bmad-help.csv` has no `bmad-lens-dashboard` entry.
  - Result: discovery for this prompt is Lens-module-local rather than global BMAD registry-driven.

## Lifecycle Fit
- Fit classification: strong for lifecycle observability, weak for lifecycle gating.
- Why strong for observability:
  - `lifecycle.yaml` defines phase progression, milestones, and adversarial-review gates; dashboard summarizes cross-feature phase and staleness state that helps humans monitor lifecycle health.
  - `module-help.csv` marks dashboard commands as `anytime`, which aligns with a read/report function usable before or after any lifecycle phase.
- Why weak for gating:
  - No `before`/`required` lifecycle dependency is declared for dashboard commands in `module-help.csv`.
  - Dashboard output is informational only; it does not update lifecycle state, satisfy completion review gates, or enforce milestone entry conditions in `lifecycle.yaml`.
- Practical placement:
  - Best used as a status/risk cockpit adjacent to planning and execution workflows, not as an approval gate.

## Evidence Refs
- Prompt entry stub: `.github/prompts/lens-dashboard.prompt.md`
- Release prompt stub: `lens.core/_bmad/lens-work/prompts/lens-dashboard.prompt.md`
- Skill contract: `lens.core/_bmad/lens-work/skills/bmad-lens-dashboard/SKILL.md`
- Skill implementation: `lens.core/_bmad/lens-work/skills/bmad-lens-dashboard/scripts/dashboard-ops.py`
- Dashboard template: `lens.core/_bmad/lens-work/skills/bmad-lens-dashboard/assets/dashboard-template.html`
- Skill reference docs:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-dashboard/references/feature-overview.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-dashboard/references/dependency-graph.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-dashboard/references/generate-dashboard.md`
- Lens module registration and prompt inventory: `lens.core/_bmad/lens-work/module.yaml`
- Lens command mapping: `lens.core/_bmad/lens-work/module-help.csv`
- Lifecycle contract and phase/milestone model: `lens.core/_bmad/lens-work/lifecycle.yaml`
- Global BMAD registries checked for mapping presence:
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: **High** on purpose and mapping, **Medium-High** on lifecycle fit.
- Rationale:
  - High confidence because both prompt files are explicit stubs and module-help + SKILL define behavior directly.
  - Slight confidence reduction because some lifecycle fit conclusions are inferential (informational vs gating role) based on absence of `required` and gate bindings, not explicit prohibition text.

## Gaps
- Prompt-level thinness:
  - Both prompt files are stubs; prompt text itself provides almost no acceptance criteria or runbook detail.
- Registry discoverability inconsistency:
  - `bmad-lens-dashboard` is absent from global BMAD registries used for broad command discovery (`skill-manifest.csv`, `bmad-help.csv`) and only visible via Lens-local metadata.
- Minor contract drift signal:
  - `dashboard-ops.py` top docstring still mentions plan branch sourcing for deep files, while SKILL + references specify `main` for deep content; implementation currently reads `main`, so this appears to be stale documentation in code comments.
- Integration under-specification:
  - SKILL Integration Points mention planning and retrospectives, but no concrete handoff hooks are declared in `module-help.csv` (`before`/`after`) beyond internal dashboard subcommand sequencing.
