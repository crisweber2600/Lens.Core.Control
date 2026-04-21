# lens-constitution Prompt Audit

## Purpose Summary
- `.github/prompts/lens-constitution.prompt.md` is a routing stub and does not define runtime governance behavior itself.
- The stub delegates to `lens.core/_bmad/lens-work/prompts/lens-constitution.prompt.md`, which is also a stub.
- Effective behavior is defined by `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/SKILL.md` and its backing script `scripts/constitution-ops.py`, which resolve constitutions, run compliance checks, and provide progressive rule display.

## BMAD Skill Mapping
- Prompt chain:
  - `.github/prompts/lens-constitution.prompt.md`
  - `lens.core/_bmad/lens-work/prompts/lens-constitution.prompt.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/SKILL.md`
- Module registration:
  - `lens.core/_bmad/lens-work/module.yaml` registers skill `bmad-lens-constitution` and includes `lens-constitution.prompt.md` in module prompts.
- Operational mapping:
  - `lens.core/_bmad/lens-work/module-help.csv` maps `bmad-lens-constitution` to display `load-constitution`, code `CO`, action `resolve`, phase `anytime`, output `constitution YAML`.
- Global registry relationship:
  - `lens.core/_bmad/_config/skill-manifest.csv` and `lens.core/_bmad/_config/bmad-help.csv` are BMAD-global catalogs and do not represent Lens module-local command authority for this skill.
  - For this prompt, authoritative mapping is Lens-local: `module.yaml` plus `module-help.csv`.

## Lifecycle Fit
- Fit is structurally strong:
  - `lens.core/_bmad/lens-work/lifecycle.yaml` defines authority domains and constitution inheritance (`org/domain/service/repo`, additive) and constitution-gated progression to `dev-ready`.
  - `bmad-lens-constitution` provides the read-only governance resolver expected by those lifecycle concepts.
- Integration fit is explicit in skill contract:
  - The skill declares integration points with `init-feature`, planning workflows, `complete`, `dashboard`, and `sensing`, matching lifecycle gates and cross-feature governance needs.
- Lifecycle vocabulary gap exists:
  - `lifecycle.yaml` phase model is `preplan/businessplan/techplan/finalizeplan` (+ express track), while constitution script/docs use `planning/dev/complete` for compliance scopes.
  - This is workable as an abstraction layer, but traceability between concrete lifecycle phases and constitution compliance phases is not explicitly codified in the prompt stub itself.

## Evidence Refs
- Prompt entrypoint stub:
  - `.github/prompts/lens-constitution.prompt.md`
- Module prompt stub delegation:
  - `lens.core/_bmad/lens-work/prompts/lens-constitution.prompt.md`
- Constitution skill contract:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/SKILL.md`
- Constitution operational implementation:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/scripts/constitution-ops.py`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/references/resolve-rules.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/references/validate-compliance.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/references/progressive-display.md`
- Lens module skill/prompt registration:
  - `lens.core/_bmad/lens-work/module.yaml`
- Lens command/action mapping:
  - `lens.core/_bmad/lens-work/module-help.csv`
- Lifecycle contract and constitution semantics:
  - `lens.core/_bmad/lens-work/lifecycle.yaml`
- Global BMAD registry context:
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: **High**.
- High confidence on prompt-to-skill routing, module-local registration, and governance intent because these are directly declared in the prompt stubs, `module.yaml`, `module-help.csv`, and constitution skill docs/script.
- Medium confidence on end-to-end gate behavior under all lifecycle transitions because this audit focused on prompt/contract mapping and did not execute integration tests across all phase conductors.

## Gaps
- Double-stub indirection:
  - Both prompt files are wrappers, so behavioral guarantees come from skill/script contracts rather than prompt content.
- Phase vocabulary mismatch:
  - Lifecycle uses detailed planning phases, while constitution compliance API uses coarse `planning/dev/complete`; explicit mapping rules are implied, not documented in the prompt.
- Registry discoverability split:
  - Constitution prompt/skill are Lens module-local and not represented in global BMAD registry files used by generic tooling.
- Prompt-level acceptance criteria:
  - The prompt stub contains no acceptance criteria or invariants; verification depends on skill/reference/script artifacts.
