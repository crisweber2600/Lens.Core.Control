# lens-new-project Prompt Audit

## Purpose Summary
- `.github/prompts/lens-new-project.prompt.md` is a control-repo adapter stub, not the behavioral source of truth.
- The stub runs a lightweight preflight (`uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py`) and then delegates to the full Lens prompt at `lens.core/_bmad/lens-work/prompts/lens-new-project.prompt.md`.
- The full prompt defines a guided "new project stack" bootstrap flow that intentionally hides lower-level command sequencing.
- Operationally, the flow resolves domain/service existence, creates missing containers with governance git execution, initializes a feature with explicit track selection, executes the returned branch orchestration command, and optionally provisions a target repo.

## BMAD Skill Mapping
- Direct prompt-to-skill mapping is explicit in the full prompt:
  - `bmad-lens-init-feature`
  - `bmad-lens-target-repo`
- The full prompt mandates specific `init-feature-ops.py` subcommands and behavior contracts:
  - `create-domain` and `create-service` (with `--execute-governance-git` and scaffold path flags)
  - `create` for feature init (with explicit track and branch-command execution)
- Lens module registration confirms both skills are first-class Lens module skills and that `lens-new-project.prompt.md` is a shipped prompt surface.
- Lens command/help mapping confirms operational dependencies for the flow:
  - `init-feature` (`IF`)
  - `target-repo` (`TR`)
  - `next-action` (`NA`)
  - onboarding/preflight commands
- Global BMAD registries do not list this flow or these Lens-local skills:
  - no `bmad-lens-init-feature`
  - no `bmad-lens-target-repo`
  - no `lens-new-project`
  - This is consistent with Lens-local orchestration living in module-local surfaces rather than global `_config` catalogs.

## Lifecycle Fit
- Fit is strong: this prompt is an onboarding/bootstrap entrypoint that prepares lifecycle state without bypassing lifecycle governance.
- The prompt correctly avoids hardcoding post-init planning and instead requires reporting `starting_phase` and routing to `/next` or returned `recommended_command`.
- That behavior aligns with `lifecycle.yaml` track diversity where `start_phase` differs by track (`preplan`, `businessplan`, `techplan`, `finalizeplan`, or `expressplan`).
- The prompt's explicit requirement for track selection before feature creation is lifecycle-correct because tracks materially alter start phase and required gates.
- Repo provisioning is correctly sequenced after feature init so target repo metadata is written into newly created `feature.yaml`, matching Lens feature-first lifecycle data flow.

## Evidence Refs
- Control adapter prompt:
  - `.github/prompts/lens-new-project.prompt.md`
- Full Lens prompt:
  - `lens.core/_bmad/lens-work/prompts/lens-new-project.prompt.md`
- Lens skill contracts used by the prompt:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-target-repo/SKILL.md`
- Lens module registration and prompt inventory:
  - `lens.core/_bmad/lens-work/module.yaml`
- Lens command/help mappings:
  - `lens.core/_bmad/lens-work/module-help.csv`
- Lifecycle contract and track start-phase behavior:
  - `lens.core/_bmad/lens-work/lifecycle.yaml`
- Global registry scope checks (absence evidence):
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: High.
- Confidence is high on routing, skill mapping, and lifecycle alignment because they are directly declared in prompt text, module registration, module help surfaces, and lifecycle contract files.
- Confidence is moderate on runtime failure-path ergonomics because this audit validated declared contracts and did not execute an end-to-end `lens-new-project` run.

## Gaps
- Double-indirection risk:
  - The `.github` prompt is a stub delegating to another prompt, so prompt-only review can miss behavior unless auditors follow the chain into Lens prompt and skills.
- Discoverability split:
  - `lens-new-project` appears as a prompt surface in `module.yaml`, but there is no explicit `new-project` command row in `module-help.csv`; users discover underlying commands (`init-feature`, `target-repo`) more easily than the composite entrypoint.
- Registry visibility mismatch:
  - Required global catalogs (`skill-manifest.csv`, `bmad-help.csv`) do not include Lens-local skills used by this prompt, which can mislead tooling that assumes those files are exhaustive.
- Governance inspection ambiguity:
  - The prompt says to inspect governance when domain/service existence is unknown, but leaves the exact operator path/check sequence to implementation, which can yield inconsistent operator behavior across agents.
