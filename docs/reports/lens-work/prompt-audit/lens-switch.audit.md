# lens-switch Prompt Audit

## Purpose Summary
- [.github/prompts/lens-switch.prompt.md](.github/prompts/lens-switch.prompt.md#L1) is a thin entry stub that enforces a shared preflight and delegates to the release prompt implementation.
- The operational controller is [lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md](lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md#L1), which defines deterministic config resolution, explicit command execution, and strict menu behavior.
- Effective runtime behavior is implemented by the switch skill and script: [lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md#L1) and [lens.core/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py](lens.core/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py#L1).
- Net purpose: safely switch active feature context (or list candidates), write local context only, and avoid heuristic/autonomous feature selection.

## BMAD Skill Mapping
- Prompt chain: [.github/prompts/lens-switch.prompt.md](.github/prompts/lens-switch.prompt.md#L18) -> [lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md](lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md#L1) -> [lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md#L2).
- Module registration is explicit in [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml#L67) and prompt inclusion in [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml#L234).
- Command surface is explicitly mapped in [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv#L15) and [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv#L16):
  - FE: switch-feature (action switch)
  - LF: list-features (action list)
  - both phase scope anytime
- The global BMAD registries used for this audit do not contain bmad-lens-switch (module-local only):
  - [lens.core/_bmad/_config/skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv)
  - [lens.core/_bmad/_config/bmad-help.csv](lens.core/_bmad/_config/bmad-help.csv)

## Lifecycle Fit
- Fit with lifecycle is strong but indirect: switch is an anytime utility command, not a lifecycle promotion gate.
- Lifecycle phase authority remains in [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L238) and tracks under [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml#L243).
- Switch aligns by selecting the active feature and returning phase/status context used by phase commands, while avoiding state mutation in governance artifacts:
  - Skill non-negotiable and main-first behavior in [lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md#L10) and [lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md#L30).
  - Validation-first check in [lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md#L33).
  - Menu and explicit selection guardrails in [lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md](lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md#L52).
- Operationally, switch can attempt control-repo branch checkout to feature-plan context, but failure is surfaced rather than auto-remediated, preserving lifecycle safety ([lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md](lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md#L64), [lens.core/_bmad/lens-work/skills/bmad-lens-switch/references/switch-feature.md](lens.core/_bmad/lens-work/skills/bmad-lens-switch/references/switch-feature.md#L65)).

## Evidence Refs
- Entry prompt stub and release delegation
  - [light preflight invocation](.github/prompts/lens-switch.prompt.md#L14)
  - [delegation target](.github/prompts/lens-switch.prompt.md#L18)
- Full prompt control logic
  - [switch script path anchor](lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md#L13)
  - [switch execution template](lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md#L34)
  - [list execution template](lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md#L45)
  - [domains/features mode rules](lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md#L52)
  - [askQuestions-dependent menu behavior](lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md#L53)
  - [branch_switched and context_to_load postconditions](lens.core/_bmad/lens-work/prompts/lens-switch.prompt.md#L64)
- Skill contract and references
  - [skill identity](lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md#L2)
  - [main-first principle](lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md#L30)
  - [validation-first principle](lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md#L33)
  - [numbered menu flow](lens.core/_bmad/lens-work/skills/bmad-lens-switch/SKILL.md#L67)
  - [list output numbering and domains fallback](lens.core/_bmad/lens-work/skills/bmad-lens-switch/references/list-features.md#L43)
  - [switch output including branch_switched/context_to_load](lens.core/_bmad/lens-work/skills/bmad-lens-switch/references/switch-feature.md#L42)
- Script implementation anchors
  - [list implementation and domains fallback mode](lens.core/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py#L383)
  - [switch implementation with feature-index validation](lens.core/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py#L429)
  - [main parser subcommands list/switch/context-paths](lens.core/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py#L624)
- Module and lifecycle mapping
  - [module skill registration](lens.core/_bmad/lens-work/module.yaml#L67)
  - [module prompt registration](lens.core/_bmad/lens-work/module.yaml#L234)
  - [module help switch/list entries](lens.core/_bmad/lens-work/module-help.csv#L15)
  - [canonical phase order](lens.core/_bmad/lens-work/lifecycle.yaml#L238)
  - [tracks contract](lens.core/_bmad/lens-work/lifecycle.yaml#L243)
- Global registry gap check
  - [skill manifest searched, no bmad-lens-switch entry](lens.core/_bmad/_config/skill-manifest.csv)
  - [bmad help searched, no bmad-lens-switch entry](lens.core/_bmad/_config/bmad-help.csv)

## Confidence
- Overall: High.
- High confidence on prompt chain, command mapping, and lifecycle positioning because evidence is direct and consistent across prompt, skill, module, and script assets.
- Medium-high confidence on runtime UX behavior because menu handling varies based on availability of question tooling, though fallback behavior is explicitly defined.

## Gaps
- Dual-stub indirection means auditing only [.github/prompts/lens-switch.prompt.md](.github/prompts/lens-switch.prompt.md#L1) is insufficient; behavior assurance requires full prompt + skill + script review.
- Global BMAD discovery surfaces do not advertise bmad-lens-switch, so discoverability depends on Lens module-local registries and prompt routing.
- Lifecycle contract does not model switch as a phase/gate command (by design), so governance on misuse relies on prompt guardrails rather than lifecycle gate enforcement.
- The switch flow can return branch checkout errors without built-in remediation; this is safe, but leaves recovery dependent on user invoking the appropriate follow-up flow.
