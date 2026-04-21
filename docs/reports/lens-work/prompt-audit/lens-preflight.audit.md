# lens-preflight Prompt Audit

## Purpose Summary
- `.github/prompts/lens-preflight.prompt.md` is a control-repo stub that exists to run a lightweight sync first, then delegate to the release prompt in `lens.core`.
- The control stub explicitly runs `light-preflight.py` before loading the release prompt (`lens.core/_bmad/lens-work/prompts/lens-preflight.prompt.md`).
- The release prompt is also a stub: it runs the full `preflight.py` flow and explicitly forbids onboarding scaffold/write-config side flows.
- Effective behavior therefore lives in script contracts (`light-preflight.py`, `preflight.py`) plus Lens onboarding skill guidance, not in prompt-local orchestration logic.

## BMAD Skill Mapping
- `lens-preflight` is Lens-module local and maps to onboarding/preflight surfaces, not to a canonical global BMAD Method skill ID.
- `module.yaml` registers the Lens onboarding skill (`bmad-lens-onboard`) and ships `lens-preflight.prompt.md` as a module prompt surface.
- `module-help.csv` maps operational commands through onboarding:
  - `PO` (`preflight-only`) runs `uv run ./lens.core/_bmad/lens-work/scripts/preflight.py [--caller <name>] [--governance-path <path>]`.
  - `NB` (`onboard`) runs the same preflight script with `--caller onboard` and adds role-aware next-step guidance.
- In global BMAD registries, `skill-manifest.csv` and `bmad-help.csv` contain canonical BMAD skills (for example `bmad-init`, `bmad-help`, `bmad-code-review`, `bmad-quick-dev`), but no `bmad-lens-preflight` entry. This indicates `lens-preflight` is a Lens wrapper command rather than a standalone BMAD core skill.

## Lifecycle Fit
- `lifecycle.yaml` defines canonical planning progression as `phase_order: [preplan, businessplan, techplan, finalizeplan]`, with `expressplan` explicitly outside phase order.
- `lens-preflight` is an anytime workspace hygiene/sync gate and does not represent a lifecycle phase transition.
- This is consistent with module help placement: preflight and onboarding commands are defined as anytime operational flows, while planning phases are managed by phase conductors.
- Script-level behavior reinforces this role:
  - `light-preflight.py` performs lightweight sync behavior and can auto-commit before sync.
  - `preflight.py` performs governance sync (`pull --rebase --autostash`) and supports caller-aware behavior, including role-aware next-step handoff for onboarding.

## Evidence Refs
- Control prompt chain:
  - `.github/prompts/lens-preflight.prompt.md:7`
  - `.github/prompts/lens-preflight.prompt.md:14`
  - `.github/prompts/lens-preflight.prompt.md:18`
- Release prompt behavior:
  - `lens.core/_bmad/lens-work/prompts/lens-preflight.prompt.md:7`
  - `lens.core/_bmad/lens-work/prompts/lens-preflight.prompt.md:12`
  - `lens.core/_bmad/lens-work/prompts/lens-preflight.prompt.md:15`
- Required registry/help/module/lifecycle sources:
  - `lens.core/_bmad/_config/skill-manifest.csv:7`
  - `lens.core/_bmad/_config/skill-manifest.csv:9`
  - `lens.core/_bmad/_config/skill-manifest.csv:28`
  - `lens.core/_bmad/_config/skill-manifest.csv:36`
  - `lens.core/_bmad/_config/skill-manifest.csv:41`
  - `lens.core/_bmad/_config/bmad-help.csv:8`
  - `lens.core/_bmad/_config/bmad-help.csv:9`
  - `lens.core/_bmad/_config/bmad-help.csv:25`
  - `lens.core/_bmad/_config/bmad-help.csv:41`
  - `lens.core/_bmad/lens-work/module.yaml:94`
  - `lens.core/_bmad/lens-work/module.yaml:95`
  - `lens.core/_bmad/lens-work/module.yaml:213`
  - `lens.core/_bmad/lens-work/module.yaml:279`
  - `lens.core/_bmad/lens-work/module-help.csv:37`
  - `lens.core/_bmad/lens-work/module-help.csv:38`
  - `lens.core/_bmad/lens-work/module-help.csv:39`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:201`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:236`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
- Relevant prompt/skill/script contracts:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md:11`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md:14`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md:16`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md:70`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-onboard/SKILL.md:88`
  - `lens.core/_bmad/lens-work/scripts/light-preflight.py:19`
  - `lens.core/_bmad/lens-work/scripts/light-preflight.py:148`
  - `lens.core/_bmad/lens-work/scripts/light-preflight.py:266`
  - `lens.core/_bmad/lens-work/scripts/preflight.py:104`
  - `lens.core/_bmad/lens-work/scripts/preflight.py:120`
  - `lens.core/_bmad/lens-work/scripts/preflight.py:484`
  - `lens.core/_bmad/lens-work/scripts/preflight.py:531`
  - `lens.core/_bmad/lens-work/scripts/preflight.py:747`
  - `lens.core/_bmad/lens-work/scripts/preflight.py:774`

## Confidence
- Overall: High.
- Rationale: prompt chain, module wiring, command mapping, and script contracts all align on the same operational model (light preflight -> shared preflight, with caller-aware onboarding guidance).
- Residual uncertainty: Medium-Low for runtime side effects in edge conditions (for example auto-commit/push behavior under unusual git states), because this audit is static-analysis based.

## Gaps
- Double-stub indirection reduces prompt-local observability. Most behavior is outside prompt files, so auditing requires traversing scripts and skill contracts.
- There is no dedicated `bmad-lens-preflight` canonical skill identity in global BMAD registries, which can make discovery and governance semantics less explicit across cross-module tooling.
- The control prompt runs `light-preflight.py` while the release prompt runs `preflight.py`; this split is intentional but creates drift risk if one side changes without corresponding updates.
- Preflight is lifecycle-adjacent but phase-agnostic. Teams may misread it as a planning phase command unless help surfaces continue to keep it clearly in anytime operational routing.
