# lens-next Prompt Audit

## Purpose Summary
- `.github/prompts/lens-next.prompt.md` is a routing stub, not an implementation prompt.
- It delegates to `lens.core/_bmad/lens-work/prompts/lens-next.prompt.md`, which is also a stub.
- Effective behavior is defined by the Lens Next skill contract (`bmad-lens-next`) and help metadata: resolve the single unblocked next command for a feature and auto-delegate, or return blocker guidance.

## BMAD Skill Mapping
- Prompt chain: `.github/prompts/lens-next.prompt.md` -> `lens.core/_bmad/lens-work/prompts/lens-next.prompt.md` -> `bmad-lens-next` skill.
- Canonical skill mapping (module): `bmad-lens-next` in `module.yaml` with description "Next-action recommendation based on feature state".
- Operational mapping (command surface): `module-help.csv` maps `bmad-lens-next` to display `next-action`, menu code `NA`, action `suggest`, phase `anytime`, output `delegated command or blocker report`.
- Global BMAD registries (`_config/skill-manifest.csv`, `_config/bmad-help.csv`) do not include `bmad-lens-next`; mapping authority for this prompt is module-local.

## Lifecycle Fit
- Fit is strong for phase routing/orchestration because `next-action` is phase-agnostic (`anytime`) and lifecycle phases are explicit in `lifecycle.yaml` (`preplan`, `businessplan`, `techplan`, `finalizeplan`).
- The command supports progression by selecting the next unblocked command; this aligns with phase ordering and milestones that gate movement to `dev-ready`.
- `init-feature` explicitly references `bmad-lens-next:suggest` in `before`, indicating designed handoff from initialization into guided next-step routing.

## Evidence Refs
- Prompt stub entrypoint: `.github/prompts/lens-next.prompt.md:5`, `.github/prompts/lens-next.prompt.md:10`
- Module skill registration: `lens.core/_bmad/lens-work/module.yaml:61`, `lens.core/_bmad/lens-work/module.yaml:62`
- Module prompt registration: `lens.core/_bmad/lens-work/module.yaml:210`, `lens.core/_bmad/lens-work/module.yaml:232`
- Next-action command mapping: `lens.core/_bmad/lens-work/module-help.csv:13`
- Init-feature handoff to next: `lens.core/_bmad/lens-work/module-help.csv:7`
- Lifecycle phase order and milestones: `lens.core/_bmad/lens-work/lifecycle.yaml:238`, `lens.core/_bmad/lens-work/lifecycle.yaml:243`, `lens.core/_bmad/lens-work/lifecycle.yaml:247`
- Lifecycle gate to dev-ready: `lens.core/_bmad/lens-work/lifecycle.yaml:89`, `lens.core/_bmad/lens-work/lifecycle.yaml:91`
- Mapping gap checks (no match for `bmad-lens-next`): `lens.core/_bmad/_config/skill-manifest.csv`, `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: **Medium-High**.
- High confidence on routing chain and module-level command mapping (direct textual evidence).
- Slightly reduced confidence on runtime delegation semantics because prompt files are stubs and behavior is primarily externalized to skill implementation.

## Gaps
- Double-stub indirection means prompt-level audit cannot validate execution logic without auditing `skills/bmad-lens-next/SKILL.md` and script internals.
- `bmad-lens-next` is not discoverable via global BMAD config manifests used in this audit scope; discoverability depends on Lens module-local registries.
- Prompt-level acceptance criteria are not embedded in the stub prompt, so behavior assurance relies on module/help/lifecycle contracts rather than prompt text.
