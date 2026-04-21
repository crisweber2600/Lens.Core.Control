# lens-sensing Prompt Audit

## Purpose Summary
- [.github/prompts/lens-sensing.prompt.md](.github/prompts/lens-sensing.prompt.md) is a routing stub. It runs a lightweight preflight and delegates execution to the release prompt.
- [lens.core/_bmad/lens-work/prompts/lens-sensing.prompt.md](lens.core/_bmad/lens-work/prompts/lens-sensing.prompt.md) is also a stub. It delegates behavior to the sensing skill contract.
- Effective behavior is defined in [lens.core/_bmad/lens-work/skills/bmad-lens-sensing/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-sensing/SKILL.md): cross-initiative overlap detection, conflict classification, and advisory vs hard-gate resolution.

## BMAD Skill Mapping
- Prompt chain:
  - [.github/prompts/lens-sensing.prompt.md](.github/prompts/lens-sensing.prompt.md)
  - [lens.core/_bmad/lens-work/prompts/lens-sensing.prompt.md](lens.core/_bmad/lens-work/prompts/lens-sensing.prompt.md)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-sensing/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-sensing/SKILL.md)
- Lens module registration:
  - [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml#L133) registers skill id bmad-lens-sensing.
  - [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml#L236) includes lens-sensing.prompt.md in prompt catalog.
- Lens operational command mapping:
  - [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv#L70) maps bmad-lens-sensing to command sensing, action detect, code SN, phase anytime, output sensing report.
- Integration dependencies declared by skill:
  - bmad-lens-git-state for initiative and branch topology context.
  - bmad-lens-constitution for sensing_gate_mode resolution.
  - bmad-lens-theme for output persona overlay.
- Global BMAD catalog relationship:
  - [lens.core/_bmad/_config/skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv) has no bmad-lens-sensing entry.
  - [lens.core/_bmad/_config/bmad-help.csv](lens.core/_bmad/_config/bmad-help.csv) has no bmad-lens-sensing command row.
  - Result: authoritative mapping for this prompt is Lens-local in module.yaml plus module-help.csv.

## Lifecycle Fit
- Strong architectural fit:
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L63) defines design axiom A4: sensing must be automatic at lifecycle gates.
- Behavioral fit:
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L392) defines a dedicated sensing block with scope overlap and content overlap rules.
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L407) defines sensing report output format.
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L687) exposes content_sensing_mode as a constitution capability.
- Command surface fit:
  - [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv#L70) marks sensing as phase anytime, which is consistent with a cross-cutting portfolio and gate safety check.

## Evidence Refs
- [.github/prompts/lens-sensing.prompt.md](.github/prompts/lens-sensing.prompt.md)
- [lens.core/_bmad/lens-work/prompts/lens-sensing.prompt.md](lens.core/_bmad/lens-work/prompts/lens-sensing.prompt.md)
- [lens.core/_bmad/lens-work/skills/bmad-lens-sensing/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-sensing/SKILL.md)
- [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml)
- [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv)
- [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml)
- [lens.core/_bmad/_config/skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv)
- [lens.core/_bmad/_config/bmad-help.csv](lens.core/_bmad/_config/bmad-help.csv)

## Confidence
- Overall: High.
- High confidence on routing chain, module-local registration, and lifecycle intent because the mapping is explicitly declared in the prompt stubs, module registration, module help mapping, and sensing skill contract.
- Medium confidence on script-level runtime parity for every overlap classification path because this audit validated prompt and contract surfaces, not full execution traces.

## Gaps
- Double-stub indirection: both prompt files are wrappers, so executable behavior resides outside prompt text in the skill contract.
- Discoverability split: sensing is absent from global BMAD catalogs and only discoverable through Lens module-local registries.
- Automation traceability: lifecycle declares automatic sensing at gates, but per-phase completion blocks do not directly name sensing hooks; linkage is implied via workflow/skill orchestration rather than phase-local keys.
- Prompt-level acceptance criteria are not embedded in the prompt files; they are inferred from the sensing skill and lifecycle contract.