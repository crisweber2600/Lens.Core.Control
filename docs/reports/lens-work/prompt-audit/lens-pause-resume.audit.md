# lens-pause-resume Prompt Audit

## Purpose Summary
- `.github/prompts/lens-pause-resume.prompt.md` is a delegation stub, not an execution prompt.
- It delegates to `lens.core/_bmad/lens-work/prompts/lens-pause-resume.prompt.md`, which is also a stub.
- Effective behavior is defined in `lens.core/_bmad/lens-work/skills/bmad-lens-pause-resume/SKILL.md` and its `pause-resume-ops.py` script:
  - pause requires a non-empty reason,
  - pause preserves current phase in `paused_from`,
  - resume restores phase from `paused_from` and clears pause fields,
  - status reports pause metadata.

## BMAD Skill Mapping
- Prompt chain:
  - `.github/prompts/lens-pause-resume.prompt.md` -> `lens.core/_bmad/lens-work/prompts/lens-pause-resume.prompt.md` -> `bmad-lens-pause-resume` skill.
- Module registration:
  - `lens.core/_bmad/lens-work/module.yaml` registers `bmad-lens-pause-resume` with description "Pause and resume features with state preservation".
  - The same module file includes `lens-pause-resume.prompt.md` in the Lens prompt list.
- Command-surface mapping:
  - `lens.core/_bmad/lens-work/module-help.csv` maps three user commands to this skill:
    - `pause` (`PA`) -> action `pause`
    - `resume` (`RS`) -> action `resume`
    - `pause-status` (`PT`) -> action `status`
  - All are phase `anytime`, indicating cross-phase operational availability.
- Global BMAD registry alignment:
  - `_bmad/_config/skill-manifest.csv` and `_bmad/_config/bmad-help.csv` do not expose Lens-local `bmad-lens-*` entries, including pause/resume; authority for this prompt is module-local (`module.yaml` + `module-help.csv`).

## Lifecycle Fit
- Strong fit as a lifecycle control utility, not a canonical lifecycle phase:
  - Canonical phase order in `lifecycle.yaml` is `preplan -> businessplan -> techplan -> finalizeplan`; `paused` is not listed as a phase in `phase_order` or track phase lists.
  - `module-help.csv` correctly marks pause/resume as `anytime`, which matches utility behavior outside strict phase progression.
- State-model fit is implemented in feature lifecycle tooling:
  - `bmad-lens-feature-yaml` allows `paused` in valid phases and transition maps include transitions into `paused` and from `paused` back to base phases.
  - `bmad-lens-next` explicitly detects `phase == "paused"` and routes to `/pause-resume` with rationale text that includes `paused_from` when present.
- Net: lifecycle contract remains clean (no `paused` in canonical phase chain), while pause/resume is implemented as an orthogonal stateful gate that can temporarily suspend progression and later re-enter the correct phase.

## Evidence Refs
- Prompt stub chain:
  - `.github/prompts/lens-pause-resume.prompt.md`
  - `lens.core/_bmad/lens-work/prompts/lens-pause-resume.prompt.md`
- Skill contract and references:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-pause-resume/SKILL.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-pause-resume/references/pause-feature.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-pause-resume/references/resume-feature.md`
- Operational implementation:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-pause-resume/scripts/pause-resume-ops.py`
- Module and help mapping authority:
  - `lens.core/_bmad/lens-work/module.yaml`
  - `lens.core/_bmad/lens-work/module-help.csv`
- Lifecycle and routing integration:
  - `lens.core/_bmad/lens-work/lifecycle.yaml`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/scripts/feature-yaml-ops.py`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-next/scripts/next-ops.py`
- Global registry comparison sources:
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: **High**.
- Confidence is high for mapping and intent because the prompt chain, module registration, command mappings, and script contract are all explicit and mutually consistent.
- Confidence is slightly reduced for full runtime enforcement across all lifecycle conductors because this audit is prompt/contract-centric and does not execute full end-to-end pause/resume flows.

## Gaps
- Double-stub discoverability gap:
  - Both control and release prompt files are stubs, so users cannot infer behavior without loading skill docs.
- Lifecycle-doc explicitness gap:
  - `lifecycle.yaml` intentionally does not model `paused` as a phase, but it also does not explicitly document pause/resume as an orthogonal lifecycle suspension state.
- Global manifest visibility gap:
  - Pause/resume is absent from `_config/skill-manifest.csv` and `_config/bmad-help.csv`; consumers relying only on global BMAD registries may miss this command surface.
- Enforcement gap to validate separately:
  - Skill docs say all phase-advancing skills should reject progression when paused, but this audit only confirmed direct handling in `next-ops.py` and transition support in `feature-yaml-ops.py`; broader conductor-level guard consistency is not proven here.
