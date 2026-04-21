# lens-module-management Prompt Audit

## Purpose Summary
- `.github/prompts/lens-module-management.prompt.md` is a control-layer stub that runs a shared lightweight preflight, then delegates into the release prompt.
- `lens.core/_bmad/lens-work/prompts/lens-module-management.prompt.md` is a second stub that delegates again into `bmad-lens-module-management`.
- Effective behavior therefore lives in `lens.core/_bmad/lens-work/skills/bmad-lens-module-management/SKILL.md`, not in either prompt file.
- The skill's real purpose is narrow and operational: compare installed vs release `module_version`, explain whether an upgrade is available, and explicitly separate schema freshness from legacy branch migration readiness.

## BMAD Skill Mapping
- Prompt chain:
  - `.github/prompts/lens-module-management.prompt.md`
  - `lens.core/_bmad/lens-work/prompts/lens-module-management.prompt.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-module-management/SKILL.md`
- Lens module registration is explicit in `lens.core/_bmad/lens-work/module.yaml`, where `bmad-lens-module-management` is declared as a Lens skill with the description `Module version checking and self-service upgrade guidance`.
- Lens command-surface mapping is explicit in `lens.core/_bmad/lens-work/module-help.csv`: display name `module-management`, menu code `MM`, action `check`, phase `anytime`, output `version check results`.
- Installer exposure is also explicit in `lens.core/_bmad/lens-work/scripts/install.py`, which emits both the prompt alias and the command adapter for `module-management`.
- Global BMAD registries do not own this surface. `lens.core/_bmad/_config/skill-manifest.csv` has no `bmad-lens-module-management` entry, and `lens.core/_bmad/_config/bmad-help.csv` has no `module-management` or `bmad-lens-module-management` entry. Discovery authority is Lens module-local, not global.

## Lifecycle Fit
- Fit is moderate and mostly operational rather than phase-driving.
- The lifecycle contract is built around PR-gated progression and explicit milestone sequencing, while `module-management` is registered as `anytime`. That is appropriate for a status/check utility, but it means the command is observability and maintenance support, not a lifecycle gate.
- The skill text fits the migration model well: it explicitly warns that version parity is not migration parity and routes unresolved legacy-branch cases toward `/lens-migrate scan` or `/lens-migrate` rather than declaring the workspace fully current.
- The skill also aligns with the neighboring upgrade surface. `bmad-lens-upgrade` repeats the same separation between schema upgrades and legacy branch migration, which makes `module-management` a safe read-first companion rather than a competing workflow.
- The main lifecycle weakness is that the prompt surface itself does not explain where this command fits. The user only learns that it is a non-destructive maintenance aid after following the double-stub chain into the skill.

## Evidence Refs
- Control prompt stub and shared preflight:
  - `.github/prompts/lens-module-management.prompt.md:2`
  - `.github/prompts/lens-module-management.prompt.md:14`
  - `.github/prompts/lens-module-management.prompt.md:18`
- Release prompt stub and skill delegation:
  - `lens.core/_bmad/lens-work/prompts/lens-module-management.prompt.md:2`
  - `lens.core/_bmad/lens-work/prompts/lens-module-management.prompt.md:10`
- Skill contract and guardrails:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-module-management/SKILL.md:3`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-module-management/SKILL.md:10`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-module-management/SKILL.md:17`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-module-management/SKILL.md:39`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-module-management/SKILL.md:44`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-module-management/SKILL.md:45`
- Module registration and shipped prompt/adapters:
  - `lens.core/_bmad/lens-work/module.yaml:142`
  - `lens.core/_bmad/lens-work/module.yaml:143`
  - `lens.core/_bmad/lens-work/module.yaml:144`
  - `lens.core/_bmad/lens-work/module.yaml:255`
  - `lens.core/_bmad/lens-work/module.yaml:320`
  - `lens.core/_bmad/lens-work/module.yaml:353`
  - `lens.core/_bmad/lens-work/module.yaml:375`
  - `lens.core/_bmad/lens-work/module.yaml:398`
  - `lens.core/_bmad/lens-work/module.yaml:420`
- Command mapping and user-facing help:
  - `lens.core/_bmad/lens-work/module-help.csv:73`
  - `lens.core/_bmad/lens-work/docs/lifecycle-reference.md:124`
- Installer exposure:
  - `lens.core/_bmad/lens-work/scripts/install.py:147`
  - `lens.core/_bmad/lens-work/scripts/install.py:269`
- Lifecycle contract context:
  - `lens.core/_bmad/lens-work/lifecycle.yaml:61`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:83`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:89`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:166`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
- Global registry absence checks reviewed:
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: **Medium**.
- High confidence in prompt routing, module registration, adapter exposure, and lifecycle adjacency because those are directly declared in the prompt stubs, skill text, module registry, help index, installer, and lifecycle contract.
- Confidence is reduced because `bmad-lens-module-management` is specification-only in this payload. The skill folder contains `SKILL.md` only, with no colocated executable ops script to verify runtime behavior beyond the documented contract.

## Gaps
- Double-stub indirection means the prompt files themselves contain almost no behavioral signal; meaningful auditability only begins at the skill layer.
- Discoverability is split. The command is clearly registered inside Lens surfaces, but absent from the global `_bmad/_config` registries, which can mislead reviewers who expect one canonical registry.
- Lifecycle placement is intentionally broad (`anytime`), but the prompt text does not explain whether this is only informational, whether it should be used before `/lens-upgrade`, or how it relates to promotion readiness.
- The skill promises compatibility summaries such as schema changes, new skills, and removed features, but no backing script is colocated with the skill to show how those diffs are actually computed.
- The control prompt runs `light-preflight.py` before delegating, but the downstream skill is otherwise read-oriented; that extra startup dependency may be operationally useful, yet the prompt does not justify why a version check requires preflight.