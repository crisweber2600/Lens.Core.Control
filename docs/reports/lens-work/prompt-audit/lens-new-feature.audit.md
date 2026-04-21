# lens-new-feature Prompt Audit

## Purpose Summary
- `.github/prompts/lens-new-feature.prompt.md` is a control-layer adapter stub that runs a shared light preflight, then delegates to the release prompt implementation.
- The effective behavior lives in `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md`, which defines feature initialization expectations: 2-branch topology, governance writes on `main`, express-track PR deferral, progressive disclosure, and explicit track selection before any write.
- The release prompt delegates execution to `bmad-lens-init-feature` and relies on returned lifecycle routing outputs (`starting_phase`, `recommended_command`) with a strong preference for `/next` routing instead of hardcoded phase jumps.

## BMAD Skill Mapping
- Prompt chain:
  - `.github/prompts/lens-new-feature.prompt.md`
  - `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md`
- Lens module registration confirms this mapping:
  - `lens.core/_bmad/lens-work/module.yaml` registers `bmad-lens-init-feature` and `bmad-lens-next` as Lens skills and ships `lens-new-feature.prompt.md` as an exported prompt surface.
- Lens command/help mapping confirms runtime surfaces:
  - `lens.core/_bmad/lens-work/module-help.csv` maps `bmad-lens-init-feature` to `init-feature` (`IF`, action `create`, phase `anytime`) and includes explicit adjacency to `bmad-lens-next` (`NA`) and lifecycle phase conductors (`preplan`, `businessplan`, `techplan`, `finalizeplan`, `expressplan`).
- Global BMAD registries in scope are non-authoritative for this prompt:
  - `lens.core/_bmad/_config/skill-manifest.csv` has no `bmad-lens-init-feature` or `bmad-lens-next` entries.
  - `lens.core/_bmad/_config/bmad-help.csv` has no `lens-new-feature`/Lens-local command entries.

## Lifecycle Fit
- Fit is strong: this prompt is an entry gate into the Lens lifecycle, not an isolated repo scaffold operation.
- `lifecycle.yaml` requires phase/track-aware routing (`phase_order` and track-specific `start_phase`), which aligns with the prompt rule to return script-provided `starting_phase` and `recommended_command` and to avoid hardcoding `/quickplan`.
- Express-track handling in the prompt aligns with lifecycle semantics by deferring the planning PR until planning artifacts exist, then using lifecycle routing to continue.
- `module-help.csv` labels `init-feature` as `anytime`; this improves discoverability but can understate that the command is practically a lifecycle bootstrap with strong downstream coupling to `/next` and phase commands.

## Evidence Refs
- Control prompt stub:
  - `.github/prompts/lens-new-feature.prompt.md:2`
  - `.github/prompts/lens-new-feature.prompt.md:8`
  - `.github/prompts/lens-new-feature.prompt.md:14`
  - `.github/prompts/lens-new-feature.prompt.md:18`
- Release prompt behavior contract:
  - `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md:4`
  - `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md:10`
  - `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md:12`
  - `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md:15`
  - `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md:17`
  - `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md:20`
- Lens module registration:
  - `lens.core/_bmad/lens-work/module.yaml:49`
  - `lens.core/_bmad/lens-work/module.yaml:61`
  - `lens.core/_bmad/lens-work/module.yaml:217`
  - `lens.core/_bmad/lens-work/module.yaml:283`
- Lens command/help surface:
  - `lens.core/_bmad/lens-work/module-help.csv:7`
  - `lens.core/_bmad/lens-work/module-help.csv:8`
  - `lens.core/_bmad/lens-work/module-help.csv:13`
  - `lens.core/_bmad/lens-work/module-help.csv:48`
  - `lens.core/_bmad/lens-work/module-help.csv:49`
  - `lens.core/_bmad/lens-work/module-help.csv:50`
  - `lens.core/_bmad/lens-work/module-help.csv:52`
  - `lens.core/_bmad/lens-work/module-help.csv:53`
- Lifecycle contract:
  - `lens.core/_bmad/lens-work/lifecycle.yaml:120`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:243`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:303`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:307`
- Relevant skill contracts:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md:8`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md:12`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md:37`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md:97`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-next/SKILL.md:8`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-next/SKILL.md:27`
- Required global-registry checks (absence):
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: **High**.
- High confidence on purpose, mapping, and lifecycle alignment because they are explicitly stated across the prompt chain, Lens module registry/help surfaces, lifecycle contract, and linked skill docs.
- Slightly reduced confidence on edge-case runtime behavior because this audit validated documented contracts and mappings, not a live end-to-end run of `init-feature-ops.py`.

## Gaps
- Double-stub indirection: both prompt layers are lightweight wrappers, so auditors must follow delegation into Lens skill docs to understand real behavior.
- Registry discoverability split: required global `_config` registries do not index Lens-local skills, so cross-module tooling that assumes global registry completeness can miss this prompt's implementation target.
- Prompt-level rationale is terse: the control stub does not explain why `/next` routing is preferred over direct phase commands, even though the release prompt and skills enforce that behavior.
- `anytime` classification in `module-help.csv` is useful for menu reachability but does not communicate that `init-feature` is effectively lifecycle bootstrap with track-sensitive branch/PR semantics.