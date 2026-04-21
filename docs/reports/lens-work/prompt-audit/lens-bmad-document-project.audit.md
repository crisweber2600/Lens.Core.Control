# lens-bmad-document-project Prompt Audit

## Purpose Summary
- `.github/prompts/lens-bmad-document-project.prompt.md` is an adapter stub that runs light preflight first, then delegates into the release prompt.
- The release prompt (`lens.core/_bmad/lens-work/prompts/lens-bmad-document-project.prompt.md`) is also a thin stub that delegates to `bmad-lens-document-project`.
- Effective behavior is implemented in `bmad-lens-document-project`, which resolves feature-scoped docs paths, delegates core documentation generation to BMAD `bmad-document-project`, and mirrors generated docs into governance paths.

## BMAD Skill Mapping
- Prompt chain:
  - `.github/prompts/lens-bmad-document-project.prompt.md`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-document-project.prompt.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-document-project/SKILL.md`
  - `.github/skills/bmad-document-project/SKILL.md`
  - `lens.core/_bmad/bmm/1-analysis/bmad-document-project/SKILL.md` -> `workflow.md` -> `instructions.md`
- Module-local registration is explicit:
  - `module.yaml` registers skill `bmad-lens-document-project` and prompt `lens-bmad-document-project.prompt.md`.
  - `module-help.csv` maps it to menu code `DC`, action `document-project`, phase `anytime`, outputs `project-overview.md, index.md`.
- Global BMAD registries still recognize the underlying base skill:
  - `_config/skill-manifest.csv` includes `bmad-document-project` (BMM module skill).
  - `_config/bmad-help.csv` includes `Document Project (DP)` for `bmad-document-project`.

## Lifecycle Fit
- Fit is strong as an auxiliary, phase-agnostic documentation capability:
  - `module-help.csv` marks Lens wrapper command `DC` as `anytime`.
  - `lifecycle.yaml` describes `preplan` as including "project documentation", so this prompt aligns naturally with early analysis while remaining usable outside strict phase gates.
- The skill’s delegation to BMAD document-project aligns with lifecycle discipline by producing structured project knowledge artifacts (`project-overview.md`, `index.md`) useful for planning and PRD readiness.

## Evidence Refs
- Adapter prompt preflight + delegation:
  - `.github/prompts/lens-bmad-document-project.prompt.md:14`
  - `.github/prompts/lens-bmad-document-project.prompt.md:18`
- Release prompt delegation to Lens skill:
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-document-project.prompt.md:10`
- Lens wrapper skill behavior (feature scope + delegation + overrides):
  - `lens.core/_bmad/lens-work/skills/bmad-lens-document-project/SKILL.md:8`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-document-project/SKILL.md:25`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-document-project/SKILL.md:58`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-document-project/SKILL.md:62`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-document-project/SKILL.md:76`
- Base BMAD skill delegation chain:
  - `.github/skills/bmad-document-project/SKILL.md:13`
  - `lens.core/_bmad/bmm/1-analysis/bmad-document-project/SKILL.md:6`
  - `lens.core/_bmad/bmm/1-analysis/bmad-document-project/workflow.md:3`
  - `lens.core/_bmad/bmm/1-analysis/bmad-document-project/workflow.md:27`
  - `lens.core/_bmad/bmm/1-analysis/bmad-document-project/instructions.md:10`
  - `lens.core/_bmad/bmm/1-analysis/bmad-document-project/instructions.md:12`
  - `lens.core/_bmad/bmm/1-analysis/bmad-document-project/instructions.md:50`
  - `lens.core/_bmad/bmm/1-analysis/bmad-document-project/instructions.md:54`
  - `lens.core/_bmad/bmm/1-analysis/bmad-document-project/instructions.md:101`
  - `lens.core/_bmad/bmm/1-analysis/bmad-document-project/instructions.md:109`
  - `lens.core/_bmad/bmm/1-analysis/bmad-document-project/instructions.md:122`
  - `lens.core/_bmad/bmm/1-analysis/bmad-document-project/checklist.md:7`
- Lens module mappings:
  - `lens.core/_bmad/lens-work/module.yaml:100`
  - `lens.core/_bmad/lens-work/module.yaml:101`
  - `lens.core/_bmad/lens-work/module.yaml:254`
  - `lens.core/_bmad/lens-work/module.yaml:319`
  - `lens.core/_bmad/lens-work/module-help.csv:67`
- Global BMAD mappings:
  - `lens.core/_bmad/_config/skill-manifest.csv:16`
  - `lens.core/_bmad/_config/bmad-help.csv:18`
  - `lens.core/_bmad/_config/bmad-help.csv:66`
- Lifecycle alignment:
  - `lens.core/_bmad/lens-work/lifecycle.yaml:114`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`

## Confidence
- Overall: **High**.
- Confidence is high for prompt/skill routing and mapping because all links are directly declared in prompt stubs, module registries, and skill files.
- Confidence is medium-high (not absolute) for runtime behavior because key execution details are delegated through multiple layers and rely on the downstream BMAD workflow router state machine.

## Gaps
- Multi-hop delegation increases fragility and audit overhead:
  - `.github` prompt -> release prompt -> Lens wrapper skill -> `.github` BMAD stub -> BMM workflow.
  - A break in any hop can silently degrade behavior unless each layer is validated.
- `bmad-lens-document-project` is only a single `SKILL.md` wrapper (no colocated `workflow.md`/`instructions.md` in that skill folder), so operational guarantees are mostly inherited from external downstream files.
- Discoverability is split:
  - Lens wrapper command is module-local (`module-help.csv`) while base BMAD command is global (`_config/bmad-help.csv`), which can confuse users unless help text explicitly clarifies wrapper vs base command usage.
- Prompt-level acceptance criteria are minimal in the stubs; quality constraints (resume behavior, scan levels, batching rules) live deep in downstream BMAD docs rather than near the prompt entrypoint.
