# lens-discover Prompt Audit

## Purpose Summary
- `.github/prompts/lens-discover.prompt.md` is a control-layer stub that only delegates to the Lens release prompt.
- `lens.core/_bmad/lens-work/prompts/lens-discover.prompt.md` is also a stub and delegates execution to `bmad-lens-discover`.
- Effective behavior is therefore implemented in `bmad-lens-discover`: reconcile governance `repo-inventory.yaml` with `TargetProjects/` by scanning drift, cloning missing repos, registering untracked repos, validating inventory, and committing inventory updates.

## BMAD Skill Mapping
- Prompt chain: `.github/prompts/lens-discover.prompt.md` -> `lens.core/_bmad/lens-work/prompts/lens-discover.prompt.md` -> `lens.core/_bmad/lens-work/skills/bmad-lens-discover/SKILL.md`.
- Lens module registration includes `bmad-lens-discover` with description "Sync TargetProjects with governance repo inventory," and also lists `lens-discover.prompt.md` in module prompt surfaces.
- Lens command surface maps this skill to `discover` (`DR`), action `sync`, phase `anytime`, args `[--headless|-H] [--dry-run]`, output `repo inventory sync report`.
- Global BMAD registries in this audit scope (`lens.core/_bmad/_config/skill-manifest.csv` and `lens.core/_bmad/_config/bmad-help.csv`) do not contain `bmad-lens-discover`; mapping authority is Lens-module-local (`module.yaml` + `module-help.csv`).

## Lifecycle Fit
- Fit is strong for cross-phase orchestration work because the command is explicitly marked `anytime` in Lens help metadata and operates on repository inventory hygiene rather than phase artifact production.
- The skill is compatible with lifecycle governance intent in `lifecycle.yaml`: disciplined, auditable workflow state (`A1`), explicit authority boundaries (`A3`), and control-repo operational behavior (`A5`).
- `discover` does not appear in lifecycle `phase_order` (which is planning phases only), which is expected: it functions as an operational support command for workspace/repo topology, not a milestone progression step.
- Net assessment: lifecycle-compatible utility command with no direct milestone gating responsibility.

## Evidence Refs
- Control prompt stub and delegation:
  - `.github/prompts/lens-discover.prompt.md:5`
  - `.github/prompts/lens-discover.prompt.md:7`
  - `.github/prompts/lens-discover.prompt.md:10`
- Release prompt delegation to skill:
  - `lens.core/_bmad/lens-work/prompts/lens-discover.prompt.md:1`
  - `lens.core/_bmad/lens-work/prompts/lens-discover.prompt.md:2`
  - `lens.core/_bmad/lens-work/prompts/lens-discover.prompt.md:4`
- Skill behavior contract:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-discover/SKILL.md:2`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-discover/SKILL.md:10`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-discover/SKILL.md:12`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-discover/SKILL.md:48`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-discover/SKILL.md:89`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-discover/SKILL.md:106`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-discover/SKILL.md:129`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-discover/SKILL.md:140`
- Module registration and prompt surface:
  - `lens.core/_bmad/lens-work/module.yaml:91`
  - `lens.core/_bmad/lens-work/module.yaml:92`
  - `lens.core/_bmad/lens-work/module.yaml:93`
  - `lens.core/_bmad/lens-work/module.yaml:261`
- Lens command/help mapping:
  - `lens.core/_bmad/lens-work/module-help.csv:36`
  - `lens.core/_bmad/lens-work/module-help.csv:37`
  - `lens.core/_bmad/lens-work/module-help.csv:39`
- Lifecycle compatibility anchors:
  - `lens.core/_bmad/lens-work/lifecycle.yaml:51`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:60`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:62`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:64`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
- Global registry gap check (no `bmad-lens-discover` match observed):
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: **High** for prompt-routing and Lens module mapping.
- Rationale: both prompt layers are explicit stubs and module/help/skill docs agree on command identity and behavior.
- Residual uncertainty: runtime outcomes depend on environment state (governance path, git remotes, clone/push permissions), which is outside prompt text.

## Gaps
- Double-stub prompt architecture means prompt-local acceptance criteria are minimal; behavior assurance requires auditing the underlying skill (and optionally script) contract.
- `bmad-lens-discover` is absent from global BMAD registries used by many generic discovery flows; discoverability depends on Lens-specific module surfaces.
- The skill performs write operations (clone/add-entry/commit/push) but prompt-layer docs do not restate rollback or conflict-handling policy beyond the skill-level push-failure note.