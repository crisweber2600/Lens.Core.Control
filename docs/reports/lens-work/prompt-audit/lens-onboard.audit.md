# lens-onboard Prompt Audit

## Purpose Summary
- [.github/prompts/lens-onboard.prompt.md](.github/prompts/lens-onboard.prompt.md) is a thin control-repo stub that delegates to the release prompt.
- [lens.core/_bmad/lens-work/prompts/lens-onboard.prompt.md](lens.core/_bmad/lens-work/prompts/lens-onboard.prompt.md) is also a stub; effective behavior is delegated to [lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md).
- The active contract is: run shared preflight, halt on failures, then provide role-aware handoff guidance for `/switch`, `/dev`, `/new-*`, and `/next`.

## BMAD Skill Mapping
- Prompt chain mapping:
  [.github/prompts/lens-onboard.prompt.md](.github/prompts/lens-onboard.prompt.md) -> [lens.core/_bmad/lens-work/prompts/lens-onboard.prompt.md](lens.core/_bmad/lens-work/prompts/lens-onboard.prompt.md) -> [lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md).
- Lens module registration exists for both skill and prompt surface:
  [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml).
- Lens command registry maps onboard operations explicitly:
  [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv) entries for `preflight`/`preflight-only`/`onboard` plus related `next-action` and `switch-feature` routes.
- Global BMAD registries do not expose `bmad-lens-onboard` as a canonical cross-module skill:
  [lens.core/_bmad/_config/skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv), [lens.core/_bmad/_config/bmad-help.csv](lens.core/_bmad/_config/bmad-help.csv).
- Related runtime behavior is implemented in the skill and script layer:
  [lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md), [lens.core/_bmad/lens-work/scripts/preflight.py](lens.core/_bmad/lens-work/scripts/preflight.py), and legacy subcommands in [lens.core/_bmad/lens-work/skills/bmad-lens-onboard/scripts/onboard-ops.py](lens.core/_bmad/lens-work/skills/bmad-lens-onboard/scripts/onboard-ops.py).

## Lifecycle Fit
- Onboard is correctly modeled as `anytime` operational setup/handoff, not a planning phase conductor, which is consistent with lifecycle phase ownership in [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml) (`preplan`, `businessplan`, `techplan`, `finalizeplan`).
- The role-aware handoff in [lens.core/_bmad/lens-work/scripts/preflight.py](lens.core/_bmad/lens-work/scripts/preflight.py) aligns with lifecycle entry points:
  `primary_role: dev` is routed to `/switch` then `/dev`, while non-dev users are routed to `/switch` or `/new-*` before phase work.
- This positioning avoids bypassing lifecycle gates because onboarding does not claim phase completion; it only validates workspace prerequisites and sends users into valid command surfaces.
- Track compatibility is strong: full/feature/quickdev/express all require an initialized workspace and feature context selection before meaningful phase progression, which onboarding supports without mutating phase state.

## Evidence Refs
- Prompt delegation chain
  - [.github/prompts/lens-onboard.prompt.md](.github/prompts/lens-onboard.prompt.md)
  - [lens.core/_bmad/lens-work/prompts/lens-onboard.prompt.md](lens.core/_bmad/lens-work/prompts/lens-onboard.prompt.md)
- Required registry/module/lifecycle sources
  - [lens.core/_bmad/_config/skill-manifest.csv](lens.core/_bmad/_config/skill-manifest.csv)
  - [lens.core/_bmad/_config/bmad-help.csv](lens.core/_bmad/_config/bmad-help.csv)
  - [lens.core/_bmad/lens-work/module.yaml](lens.core/_bmad/lens-work/module.yaml)
  - [lens.core/_bmad/lens-work/module-help.csv](lens.core/_bmad/lens-work/module-help.csv)
  - [lens.core/_bmad/lens-work/lifecycle.yaml](lens.core/_bmad/lens-work/lifecycle.yaml)
- Relevant prompt/skill implementation files
  - [lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md)
  - [lens.core/_bmad/lens-work/scripts/preflight.py](lens.core/_bmad/lens-work/scripts/preflight.py)
  - [lens.core/_bmad/lens-work/skills/bmad-lens-onboard/scripts/onboard-ops.py](lens.core/_bmad/lens-work/skills/bmad-lens-onboard/scripts/onboard-ops.py)

## Confidence
- Overall: High.
- Rationale: The prompt indirection and skill ownership are explicit across prompt stubs, module registration, and module-help command mapping; role-aware guidance is concretely implemented in shared preflight, not inferred.
- Residual uncertainty: Low-to-medium for legacy subcommand usage frequency because the interactive path intentionally bypasses legacy `scaffold` and `write-config` unless explicitly requested.

## Gaps
- Double-stub prompt indirection reduces prompt-local observability; almost all behavior lives in skill and script layers rather than prompt text.
- Discoverability gap across global BMAD surfaces: `bmad-lens-onboard` is module-local and absent from global `_config` help/manifest tables, which can confuse users expecting one global command catalog.
- Contract duplication risk: role-aware routing is described in [lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md) and implemented in [lens.core/_bmad/lens-work/scripts/preflight.py](lens.core/_bmad/lens-work/scripts/preflight.py); drift is possible if one changes without the other.
- Legacy pathway mismatch risk: [lens.core/_bmad/lens-work/skills/bmad-lens-onboard/scripts/onboard-ops.py](lens.core/_bmad/lens-work/skills/bmad-lens-onboard/scripts/onboard-ops.py) requires Python 3.10+, while shared preflight currently requires Python 3.11+, which may produce inconsistent onboarding outcomes in borderline environments.