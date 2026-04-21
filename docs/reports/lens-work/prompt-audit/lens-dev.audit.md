# lens-dev Prompt Audit

## Purpose Summary
- `.github/prompts/lens-dev.prompt.md` is a control-repo stub that enforces a lightweight preflight (`light-preflight.py`) before any dev prompt delegation.
- The release-layer prompt at `lens.core/_bmad/lens-work/prompts/lens-dev.prompt.md` is also a stub and delegates directly to `bmad-lens-dev` skill instructions.
- Effective runtime behavior is therefore defined primarily by `bmad-lens-dev/SKILL.md`, not by prompt-local logic.
- The `bmad-lens-dev` skill defines Dev as an epic implementation conductor with target-repo branch resolution, mandatory task delegation (`runSubagent`), story-level review loops, final adversarial/party-mode closeout gates, and final PR handling.

## BMAD Skill Mapping
- Lens command mapping exposes `bmad-lens-dev` as command `DV`, action `implement`, with `after=bmad-lens-finalizeplan:plan`, showing Dev starts after planning handoff.
- Lens also maps delegated BMAD implementation helpers via `bmad-lens-bmad-skill`:
  - `BIR` (`bmad-check-implementation-readiness`) and `BSP` (`bmad-sprint-planning`) routed in `finalizeplan`.
  - `BST` (`bmad-create-story`) in `finalizeplan`.
  - `BQD` (`bmad-quick-dev`) and `BCR` (`bmad-code-review`) in `dev`.
- Global BMAD registry (`skill-manifest.csv`) confirms canonical implementation skills used by the dev loop (`bmad-create-story`, `bmad-dev-story`, `bmad-code-review`, `bmad-quick-dev`, `bmad-sprint-planning`) and implementation agents (`bmad-agent-dev`, `bmad-agent-qa`, `bmad-agent-sm`).
- Global BMAD help (`bmad-help.csv`) aligns these as implementation-stage workflows:
  - `SP` produces sprint status and kicks off implementation flow.
  - `CS -> VS -> DS -> CR` represents the core story execution loop.
  - `QQ` is an anytime implementation fast-path.

## Lifecycle Fit
- Lifecycle contract says dev execution follows `finalizeplan` and `dev-ready` is constitution-gated after finalizeplan completion; this matches the skill's prior-phase checks and finalizeplan artifact publication requirement.
- Lifecycle also explicitly states Dev is a delegation command, not a lifecycle phase. This aligns with Lens module-help where `bmad-lens-dev` is phase `delegation` rather than a canonical lifecycle phase entry.
- Fit is strong overall: prompt chain delegates into a skill that enforces planning-before-coding gates, review checkpoints, and closeout controls before final PR completion.
- Operational nuance: module-help places some BMAD skills in `finalizeplan` while global BMAD help labels those as `4-implementation`; this appears intentional in Lens orchestration but remains a cross-surface translation point.

## Evidence Refs
- Control prompt stub:
  - `.github/prompts/lens-dev.prompt.md:2`
  - `.github/prompts/lens-dev.prompt.md:12`
  - `.github/prompts/lens-dev.prompt.md:14`
  - `.github/prompts/lens-dev.prompt.md:18`
- Release prompt stub:
  - `lens.core/_bmad/lens-work/prompts/lens-dev.prompt.md:2`
  - `lens.core/_bmad/lens-work/prompts/lens-dev.prompt.md:7`
  - `lens.core/_bmad/lens-work/prompts/lens-dev.prompt.md:10`
- Dev skill contract:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-dev/SKILL.md:3`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-dev/SKILL.md:10`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-dev/SKILL.md:12`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-dev/SKILL.md:36`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-dev/SKILL.md:47`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-dev/SKILL.md:48`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-dev/SKILL.md:113`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-dev/SKILL.md:176`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-dev/SKILL.md:224`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-dev/SKILL.md:233`
- Required registry/help/module/lifecycle sources:
  - `lens.core/_bmad/_config/skill-manifest.csv:28`
  - `lens.core/_bmad/_config/skill-manifest.csv:32`
  - `lens.core/_bmad/_config/skill-manifest.csv:33`
  - `lens.core/_bmad/_config/skill-manifest.csv:35`
  - `lens.core/_bmad/_config/skill-manifest.csv:36`
  - `lens.core/_bmad/_config/skill-manifest.csv:38`
  - `lens.core/_bmad/_config/skill-manifest.csv:39`
  - `lens.core/_bmad/_config/skill-manifest.csv:41`
  - `lens.core/_bmad/_config/skill-manifest.csv:43`
  - `lens.core/_bmad/_config/bmad-help.csv:8`
  - `lens.core/_bmad/_config/bmad-help.csv:9`
  - `lens.core/_bmad/_config/bmad-help.csv:14`
  - `lens.core/_bmad/_config/bmad-help.csv:15`
  - `lens.core/_bmad/_config/bmad-help.csv:17`
  - `lens.core/_bmad/_config/bmad-help.csv:25`
  - `lens.core/_bmad/_config/bmad-help.csv:27`
  - `lens.core/_bmad/lens-work/module.yaml:127`
  - `lens.core/_bmad/lens-work/module.yaml:130`
  - `lens.core/_bmad/lens-work/module.yaml:226`
  - `lens.core/_bmad/lens-work/module-help.csv:54`
  - `lens.core/_bmad/lens-work/module-help.csv:64`
  - `lens.core/_bmad/lens-work/module-help.csv:65`
  - `lens.core/_bmad/lens-work/module-help.csv:66`
  - `lens.core/_bmad/lens-work/module-help.csv:68`
  - `lens.core/_bmad/lens-work/module-help.csv:69`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:52`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:89`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:91`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:93`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:181`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:233`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
- Relevant downstream BMAD skill stubs referenced by manifests:
  - `lens.core/_bmad/bmm/4-implementation/bmad-create-story/SKILL.md:2`
  - `lens.core/_bmad/bmm/4-implementation/bmad-dev-story/SKILL.md:2`
  - `lens.core/_bmad/bmm/4-implementation/bmad-code-review/SKILL.md:2`

## Confidence
- Overall: **High** for routing, mapping, and lifecycle placement.
- Rationale: all required surfaces agree that `lens-dev` is a delegated implementation command that starts after finalizeplan and coordinates implementation-focused skills.
- Residual uncertainty: the two prompt files are stubs, so behavior guarantees rely on downstream skill/workflow artifacts rather than prompt-local instructions.

## Gaps
- Dual-stub indirection reduces prompt-local transparency: policy and failure behavior are mostly hidden in downstream skill files.
- The release prompt does not repeat preflight enforcement; only the control prompt enforces `light-preflight.py`. This split is workable but fragile if entry points bypass control-layer stubs.
- Cross-surface terminology requires operator translation:
  - Global BMAD help frames several commands as `4-implementation`.
  - Lens module-help routes some of those same commands through `finalizeplan` before Dev.
- `bmad-lens-dev` is highly prescriptive (branch policy, checkpoints, final review gates), but there is no prompt-local condensed contract in `lens-dev.prompt.md`; onboarding readers must traverse multiple indirection layers to understand execution obligations.