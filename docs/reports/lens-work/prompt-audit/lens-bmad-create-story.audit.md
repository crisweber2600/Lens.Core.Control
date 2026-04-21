# lens-bmad-create-story Prompt Audit

## Purpose Summary
- The top-level prompt `.github/prompts/lens-bmad-create-story.prompt.md` is a control-repo stub that enforces a lightweight preflight first, then delegates to the release prompt.
- The release prompt `lens.core/_bmad/lens-work/prompts/lens-bmad-create-story.prompt.md` is also a stub; it delegates execution to the Lens BMAD wrapper skill with `--skill bmad-create-story`.
- Effective behavior therefore comes from the wrapper `bmad-lens-bmad-skill` plus the downstream BMAD implementation skill `bmad-create-story` and its workflow.
- Net purpose: run BMAD Create Story with Lens feature/governance context and constrained write scope, producing story files for implementation.

## BMAD Skill Mapping
- Prompt entrypoint mapping:
  - `.github/prompts/lens-bmad-create-story.prompt.md` -> `lens.core/_bmad/lens-work/prompts/lens-bmad-create-story.prompt.md`.
  - Release prompt -> `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md` with `--skill bmad-create-story`.
- Canonical BMAD skill registration:
  - `lens.core/_bmad/_config/skill-manifest.csv` registers `bmad-create-story` to `_bmad/bmm/4-implementation/bmad-create-story/SKILL.md`.
  - `lens.core/_bmad/_config/bmad-help.csv` exposes both `Create Story (CS)` and `Validate Story (VS)`.
- Lens wrapper registration:
  - `lens.core/_bmad/lens-work/module.yaml` includes wrapper skill `bmad-lens-bmad-skill` and prompt `lens-bmad-create-story.prompt.md`.
  - `lens.core/_bmad/lens-work/module-help.csv` maps `bmad-lens-bmad-skill,bmad-create-story` to `create-story` in `finalizeplan` with output `docs/implementation-artifacts` and `story files`.

## Lifecycle Fit
- Lifecycle contract fit is strong at the Lens layer:
  - `lifecycle.yaml` establishes `finalizeplan` as the planning consolidation milestone before `dev-ready` and `dev` handoff.
  - `module-help.csv` places wrapper-based `bmad-create-story` in `finalizeplan`, aligning with the FinalizePlan bundle outputs (`stories`, `story-files`, `sprint-status`).
- Downstream BMAD baseline is different:
  - Global BMAD help classifies `bmad-create-story` under `4-implementation` story cycle semantics.
  - Lens intentionally re-contextualizes it earlier (FinalizePlan) via wrapper orchestration to pre-generate story files before dev handoff.
- Practical fit verdict:
  - Fit is valid in Lens Next architecture, but relies on understanding module-local remapping rather than global BMAD phase labels.

## Evidence Refs
- Prompt stubs and delegation:
  - `.github/prompts/lens-bmad-create-story.prompt.md:5`
  - `.github/prompts/lens-bmad-create-story.prompt.md:14`
  - `.github/prompts/lens-bmad-create-story.prompt.md:18`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-create-story.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-create-story.prompt.md:11`
- BMAD registration and help:
  - `lens.core/_bmad/_config/skill-manifest.csv:38`
  - `lens.core/_bmad/_config/bmad-help.csv:14`
  - `lens.core/_bmad/_config/bmad-help.csv:15`
- Lens module and command surface:
  - `lens.core/_bmad/lens-work/module.yaml:130`
  - `lens.core/_bmad/lens-work/module.yaml:131`
  - `lens.core/_bmad/lens-work/module.yaml:251`
  - `lens.core/_bmad/lens-work/module.yaml:316`
  - `lens.core/_bmad/lens-work/module-help.csv:52`
  - `lens.core/_bmad/lens-work/module-help.csv:65`
  - `lens.core/_bmad/lens-work/module-help.csv:66`
- Lifecycle contract:
  - `lens.core/_bmad/lens-work/lifecycle.yaml:83`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:89`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:166`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
- Relevant wrapper/skill implementation files:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:31`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:32`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:136`
  - `lens.core/_bmad/bmm/4-implementation/bmad-create-story/SKILL.md:2`
  - `lens.core/_bmad/bmm/4-implementation/bmad-create-story/SKILL.md:6`
  - `lens.core/_bmad/bmm/4-implementation/bmad-create-story/workflow.md:3`
  - `lens.core/_bmad/bmm/4-implementation/bmad-create-story/workflow.md:30`
  - `lens.core/_bmad/bmm/4-implementation/bmad-create-story/workflow.md:95`

## Confidence
- Overall confidence: Medium-High.
- High confidence on mapping and lifecycle placement because the evidence is explicit in prompt stubs, manifest/help CSVs, module metadata, and wrapper contract.
- Confidence is not full because execution behavior is split across indirection layers (top-level stub, release stub, wrapper, then downstream workflow).

## Gaps
- Dual-stub indirection reduces prompt-local observability:
  - The top-level prompt still encodes `light-preflight.py` and direct release-prompt delegation, while the release prompt delegates through the wrapper skill.
  - This is workable but increases the risk of drift between control-repo prompt guidance and release behavior if one layer changes.
- Phase terminology mismatch can confuse operators:
  - Global BMAD help labels `bmad-create-story` as `4-implementation`, but Lens module-help routes it under `finalizeplan` for story-file generation.
  - This is intentional in Lens Next, but not self-evident without reading both help surfaces.
- Lifecycle contract does not directly name `bmad-create-story`:
  - `lifecycle.yaml` defines phase/milestone behavior and required artifacts, but command-level routing depends on `module-help.csv` and wrapper registry semantics.