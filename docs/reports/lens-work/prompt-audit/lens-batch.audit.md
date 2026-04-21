# lens-batch Prompt Audit

## Purpose Summary
- `.github/prompts/lens-batch.prompt.md` is an adapter stub that does two things before delegation: run shared lightweight preflight, then hand off to the module prompt.
- `lens.core/_bmad/lens-work/prompts/lens-batch.prompt.md` is also a stub; effective behavior is implemented by `bmad-lens-batch`.
- `bmad-lens-batch` defines a universal two-pass intake contract for planning targets: pass 1 generates or refreshes `{target}-batch-input.md`; pass 2 resumes the owning planning target only when required answer blocks are complete.

## BMAD Skill Mapping
- Prompt chain:
  - `.github/prompts/lens-batch.prompt.md` -> `lens.core/_bmad/lens-work/prompts/lens-batch.prompt.md` -> `lens.core/_bmad/lens-work/skills/bmad-lens-batch/SKILL.md`.
- Module registration:
  - `module.yaml` registers `bmad-lens-batch` with description "Universal two-pass batch intake and resume flow for planning targets".
  - `module.yaml` also registers both module prompt and adapter stub prompt entries for `lens-batch.prompt.md`.
- Command surface registration:
  - `module-help.csv` maps `bmad-lens-batch` to display `batch`, menu code `BT`, action `run`, phase `anytime`, and output `{target}-batch-input.md or resumed planning target`.
- Global BMAD registry fit:
  - No `bmad-lens-batch` mapping appears in `_config/skill-manifest.csv` or `_config/bmad-help.csv`; authoritative mapping is Lens module-local (`module.yaml` + `module-help.csv`).

## Lifecycle Fit
- Strong lifecycle fit for planning orchestration:
  - Lifecycle canonical phase order is `preplan -> businessplan -> techplan -> finalizeplan`.
  - Batch skill explicitly supports those planning phases plus `expressplan` and `quickplan`, and delegates pass 2 to owning phase conductors/wrappers rather than bypassing lifecycle controls.
- Governance-safe behavior:
  - Pass 1 is explicitly constrained to questions/intake only and forbids writing lifecycle artifacts or mutating feature phase state.
  - Pass 2 uses readiness detection based on required answer blocks and resumes the owner workflow with a structured `batch_resume_context`.
- Command metadata alignment:
  - `module-help.csv` lists `batch` as `anytime`, while planning phase conductors remain phase-specific (`phase-1` through `phase-4` and `phase-express`), matching the skill's role as a reusable intake front door.

## Evidence Refs
- Adapter stub behavior: `light-preflight` then handoff
  - `.github/prompts/lens-batch.prompt.md:5`
  - `.github/prompts/lens-batch.prompt.md:14`
  - `.github/prompts/lens-batch.prompt.md:18`
- Module prompt stub handoff to skill
  - `lens.core/_bmad/lens-work/prompts/lens-batch.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-batch.prompt.md:10`
- Batch skill contract (pass model, constraints, readiness)
  - `lens.core/_bmad/lens-work/skills/bmad-lens-batch/SKILL.md:31`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-batch/SKILL.md:36`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-batch/SKILL.md:39`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-batch/SKILL.md:68`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-batch/SKILL.md:87`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-batch/SKILL.md:104`
- Lens module skill + prompt registrations
  - `lens.core/_bmad/lens-work/module.yaml:64`
  - `lens.core/_bmad/lens-work/module.yaml:65`
  - `lens.core/_bmad/lens-work/module.yaml:233`
  - `lens.core/_bmad/lens-work/module.yaml:298`
- Lens command mapping for batch and planning conductors
  - `lens.core/_bmad/lens-work/module-help.csv:14`
  - `lens.core/_bmad/lens-work/module-help.csv:48`
  - `lens.core/_bmad/lens-work/module-help.csv:49`
  - `lens.core/_bmad/lens-work/module-help.csv:50`
  - `lens.core/_bmad/lens-work/module-help.csv:52`
  - `lens.core/_bmad/lens-work/module-help.csv:53`
- Lifecycle phase and ordering anchors
  - `lens.core/_bmad/lens-work/lifecycle.yaml:109`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:129`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:148`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:166`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:201`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
- Global BMAD registry gap checks
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: **High**.
- Reasoning confidence is high because the prompt chain, skill registration, command mapping, and lifecycle phase model all align directly in source text.
- Residual uncertainty is limited to runtime implementation details not embedded in prompt stubs (for example, exact file-refresh merge behavior), though the skill contract is explicit.

## Gaps
- Prompt-level discoverability relies on multi-hop stubs; behavior cannot be inferred from `.github/prompts/lens-batch.prompt.md` alone.
- `_config/skill-manifest.csv` and `_config/bmad-help.csv` do not expose Lens-local `bmad-lens-batch`, so global BMAD-only consumers may miss this capability unless they also load Lens module registries.
- The audit validates contract and mapping, not runtime script internals; deeper assurance would require tracing any script/template logic used during `{target}-batch-input.md` refresh.
