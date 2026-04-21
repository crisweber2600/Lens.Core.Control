# lens-new-service Prompt Audit

## Purpose Summary
- `.github/prompts/lens-new-service.prompt.md` is a control-layer adapter stub that enforces a lightweight preflight and delegates behavioral authority to the release prompt.
- The effective implementation is `lens.core/_bmad/lens-work/prompts/lens-new-service.prompt.md`, which scopes the operation to service initialization (not feature initialization).
- The release prompt maps service bootstrap to the `bmad-lens-init-feature` skill's `create-service` operation and explicitly requires governance-first publication (`--execute-governance-git`) plus reporting of `governance_commit_sha` when available.
- The prompt contract is governance-centric: create service/domain markers, service constitution inheritance, optional TargetProjects scaffold, optional docs scaffold, and local personal context update; explicitly do not create feature branches or `feature.yaml`.

## BMAD Skill Mapping
- Prompt chain:
  - `.github/prompts/lens-new-service.prompt.md` (stub adapter)
  - `lens.core/_bmad/lens-work/prompts/lens-new-service.prompt.md` (behavioral prompt)
  - `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md` (execution contract)
- Lens module registration is explicit for both skill and prompt surface:
  - `module.yaml` registers `bmad-lens-init-feature` as a Lens-local skill and includes `lens-new-service.prompt.md` in the prompt list.
- Lens command/help registration is partial for this use case:
  - `module-help.csv` exposes `bmad-lens-init-feature` for feature creation and pull-context, but no dedicated `new-service` or `create-service` command row is present.
  - This means the `lens-new-service` prompt is a valid interface surface, but discoverability is prompt-driven rather than mirrored in the Lens help command table.
- Global BMAD registries are not authoritative for this Lens-local behavior:
  - No `bmad-lens-init-feature` or `lens-new-service` entries were found in `lens.core/_bmad/_config/skill-manifest.csv` or `lens.core/_bmad/_config/bmad-help.csv`.

## Lifecycle Fit
- Fit is medium-strong but intentionally indirect.
- Strong fit aspects:
  - The prompt enforces governance artifact creation and governance git execution, which aligns with lifecycle axioms that git is the shared state authority and that operational work is coordinated through governance/state gates.
  - The prompt avoids feature branch creation, correctly separating service bootstrap from feature lifecycle progression.
- Indirect fit aspects:
  - `lifecycle.yaml` models phase progression for features/tracks (`preplan -> businessplan -> techplan -> finalizeplan`) and does not define a dedicated service-bootstrap phase.
  - `module-help.csv` does not publish a first-class `new-service` lifecycle command, so lifecycle routing/discoverability relies on prompt entrypoints rather than phase-table command wiring.
- Net assessment:
  - The prompt is lifecycle-compatible as infrastructure/bootstrap orchestration, but lifecycle-native observability is weaker than prompt-native behavior because command-surface parity is incomplete.

## Evidence Refs
- Control prompt stub and delegation:
  - `.github/prompts/lens-new-service.prompt.md:2`
  - `.github/prompts/lens-new-service.prompt.md:7`
  - `.github/prompts/lens-new-service.prompt.md:14`
  - `.github/prompts/lens-new-service.prompt.md:18`
- Release prompt behavior contract:
  - `lens.core/_bmad/lens-work/prompts/lens-new-service.prompt.md:2`
  - `lens.core/_bmad/lens-work/prompts/lens-new-service.prompt.md:4`
  - `lens.core/_bmad/lens-work/prompts/lens-new-service.prompt.md:7`
  - `lens.core/_bmad/lens-work/prompts/lens-new-service.prompt.md:8`
  - `lens.core/_bmad/lens-work/prompts/lens-new-service.prompt.md:9`
  - `lens.core/_bmad/lens-work/prompts/lens-new-service.prompt.md:10`
  - `lens.core/_bmad/lens-work/prompts/lens-new-service.prompt.md:11`
  - `lens.core/_bmad/lens-work/prompts/lens-new-service.prompt.md:12`
  - `lens.core/_bmad/lens-work/prompts/lens-new-service.prompt.md:14`
  - `lens.core/_bmad/lens-work/prompts/lens-new-service.prompt.md:15`
- Skill contract for `create-service` and governance git behavior:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md:77`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md:130`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md:235`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md:247`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md:253`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md:273`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md:279`
- Lens module registration:
  - `lens.core/_bmad/lens-work/module.yaml:49`
  - `lens.core/_bmad/lens-work/module.yaml:50`
  - `lens.core/_bmad/lens-work/module.yaml:216`
  - `lens.core/_bmad/lens-work/module.yaml:282`
- Lens help/command surface:
  - `lens.core/_bmad/lens-work/module-help.csv:7`
  - `lens.core/_bmad/lens-work/module-help.csv:9`
  - `lens.core/_bmad/lens-work/module-help.csv:10`
  - `lens.core/_bmad/lens-work/module-help.csv:48`
  - `lens.core/_bmad/lens-work/module-help.csv:53`
- Lifecycle contract anchors:
  - `lens.core/_bmad/lens-work/lifecycle.yaml:59`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:64`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:89`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:243`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:248`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:303`
- Global registry absence checked:
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: **High** for prompt-purpose and skill mapping.
- Overall: **Medium** for lifecycle fit and discoverability completeness.
- Reasoning:
  - High confidence comes from direct prompt text plus direct skill contract language around `create-service` and governance git execution.
  - Medium confidence remains on lifecycle surfacing because `module-help.csv` lacks a dedicated `new-service`/`create-service` entry and this audit did not execute runtime flows.

## Gaps
- Help-surface gap:
  - `module-help.csv` does not expose a dedicated `lens-new-service`/`create-service` command row, reducing discoverability parity with the prompt surface.
- Registry split gap:
  - Lens-local skill usage is not represented in global `_config` registries, so registry-only tooling can under-report true operational mappings.
- Contract indirection gap:
  - The control prompt is a stub and relies on a second prompt plus skill contract; reviewers must follow the full chain to avoid false conclusions from prompt-only inspection.
- Verification gap:
  - This audit is contract-based and did not run `create-service` in a live workspace, so it does not confirm runtime edge conditions (dirty worktree handling, partial scaffold failures, or push rejection behavior).