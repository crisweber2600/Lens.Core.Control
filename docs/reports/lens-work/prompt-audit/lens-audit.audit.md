# lens-audit Prompt Audit

## Purpose Summary
- `.github/prompts/lens-audit.prompt.md` is a routing stub that performs a lightweight preflight (`light-preflight.py`) and then delegates to module content.
- `lens.core/_bmad/lens-work/prompts/lens-audit.prompt.md` is also a stub; it delegates execution to the skill contract in `bmad-lens-audit`.
- Effective behavior is defined in `lens.core/_bmad/lens-work/skills/bmad-lens-audit/SKILL.md`: run a read-only, cross-initiative compliance dashboard that checks lifecycle/state/artifacts/branch/governance signals and reports findings by severity.

## BMAD Skill Mapping
- Prompt chain:
  - `.github/prompts/lens-audit.prompt.md`
  - `lens.core/_bmad/lens-work/prompts/lens-audit.prompt.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-audit/SKILL.md`
- Module registration:
  - `lens.core/_bmad/lens-work/module.yaml` registers `bmad-lens-audit` and includes `lens-audit.prompt.md` in the module prompt catalog.
- Operational command mapping:
  - `lens.core/_bmad/lens-work/module-help.csv` maps `bmad-lens-audit` to command `audit`, menu code `AA`, phase `anytime`, output location `docs/reports/lens-work/quality-scan`, output `audit report`.
- Delegated check dependencies (from skill contract):
  - `bmad-lens-constitution` (scope compliance)
  - `bmad-lens-git-state` (initiative enumeration/state load)
  - `bmad-lens-feature-yaml` (feature metadata)
  - `bmad-lens-theme` (persona overlay)
- Global BMAD catalog relation:
  - `lens.core/_bmad/_config/skill-manifest.csv` and `lens.core/_bmad/_config/bmad-help.csv` are global BMAD inventories; `bmad-lens-audit` is Lens module-local and resolved via `lens-work/module.yaml` + `module-help.csv` rather than those global registries.

## Lifecycle Fit
- Fit is strong and intentional:
  - `lifecycle.yaml` is the canonical source for phases/milestones (`preplan`, `businessplan`, `techplan`, `finalizeplan`, `dev-ready`).
  - The audit skill explicitly validates initiative `current_phase` and `current_milestone` against `lifecycle.yaml` and checks phase artifact completeness.
- Governance alignment:
  - The skill includes constitution scope compliance as a first-class check, which matches lifecycle design axioms emphasizing explicit authority domains and constitution-gated progression.
- Placement in command surface:
  - `module-help.csv` marks audit as `anytime`, which is consistent with a health/compliance dashboard intended to monitor across lifecycle stages rather than execute one phase.

## Evidence Refs
- Prompt entrypoint and preflight delegation:
  - `.github/prompts/lens-audit.prompt.md`
- Module prompt stub delegation to skill:
  - `lens.core/_bmad/lens-work/prompts/lens-audit.prompt.md`
- Audit behavior, scope, args, checks, integration points:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-audit/SKILL.md`
- Lens module skill + prompt registration:
  - `lens.core/_bmad/lens-work/module.yaml`
- Lens operational command mapping/output location:
  - `lens.core/_bmad/lens-work/module-help.csv`
- Lifecycle contract (schema, phases, milestones, gates, axioms):
  - `lens.core/_bmad/lens-work/lifecycle.yaml`
- Global BMAD inventories used for mapping context and gap checks:
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`
- Related prompt/skill pattern consistency references:
  - `.github/prompts/lens-next.prompt.md`
  - `lens.core/_bmad/lens-work/prompts/lens-next.prompt.md`

## Confidence
- Overall: **High**.
- High confidence on routing chain, module-local mapping, and lifecycle/compliance intent because all are explicitly declared in prompt stubs, `module.yaml`, `module-help.csv`, and `SKILL.md`.
- Moderate confidence on runtime data-source details (for example remote branch existence checks and initiative enumeration mechanics) because this prompt audit did not inspect all underlying script implementations.

## Gaps
- Double-stub indirection means behavior assurance depends on `SKILL.md` and delegated skills, not the prompt text itself.
- Audit checks are described contractually, but this report does not verify script-level implementation parity for each check path.
- Module-local discoverability is strong, but global BMAD registries do not serve as the primary source for Lens-local skills; operators must use `module.yaml`/`module-help.csv` for authoritative Lens command mapping.
- Prompt-level acceptance criteria are not embedded in `lens-audit.prompt.md`; they are inferred from the skill contract and lifecycle definitions.
