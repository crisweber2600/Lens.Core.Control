# lens-bmad-check-implementation-readiness Prompt Audit

## Purpose Summary
- `.github/prompts/lens-bmad-check-implementation-readiness.prompt.md` is a control-repo stub that adds a lightweight preflight gate, then delegates to the release prompt in `lens.core`.
- `lens.core/_bmad/lens-work/prompts/lens-bmad-check-implementation-readiness.prompt.md` is also a stub and delegates execution to `bmad-lens-bmad-skill` with `--skill bmad-check-implementation-readiness`.
- Effective runtime behavior is therefore governed primarily by Lens wrapper policy (`bmad-lens-bmad-skill`) and the downstream BMAD readiness workflow (`_bmad/bmm/3-solutioning/bmad-check-implementation-readiness`).

## BMAD Skill Mapping
- Canonical BMAD registry mapping exists in `skill-manifest.csv` for `bmad-check-implementation-readiness` under module `bmm`, targeting `_bmad/bmm/3-solutioning/bmad-check-implementation-readiness/SKILL.md`.
- Global BMAD help metadata maps this skill to code `IR`, phase `3-solutioning`, prerequisite `bmad-create-epics-and-stories`, and output contract `planning_artifacts` + readiness report.
- Lens module wiring registers both `bmad-lens-bmad-skill` and the `lens-bmad-check-implementation-readiness.prompt.md` prompt surface.
- Lens operational command mapping binds this to menu code `BIR`, action `check-implementation-readiness`, phase `finalizeplan`, and output location `feature.yaml.docs.path`.
- Wrapper policy explicitly classifies this skill as `feature-required` + `planning-docs` with `finalizeplan` phase hint, matching module-help and lifecycle expectations.

## Lifecycle Fit
- Fit is strong: lifecycle `finalizeplan` includes `implementation-readiness` as a declared artifact and as a downstream bundle output before final PR handoff.
- Lens `phase_order` includes `finalizeplan`, and the `full` track includes `finalizeplan` before `dev-ready`; this aligns with running implementation readiness as a pre-dev planning gate.
- Lens command topology places `bmad-check-implementation-readiness` in `finalizeplan` while `dev` launches after `bmad-lens-finalizeplan:plan`, reinforcing gating intent.
- Control-level preflight in the `.github` prompt improves execution safety by forcing a lightweight environment sync before wrapper delegation.

## Evidence Refs
- Prompt chain and delegation:
  - `.github/prompts/lens-bmad-check-implementation-readiness.prompt.md:5`
  - `.github/prompts/lens-bmad-check-implementation-readiness.prompt.md:14`
  - `.github/prompts/lens-bmad-check-implementation-readiness.prompt.md:18`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-check-implementation-readiness.prompt.md:2`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-check-implementation-readiness.prompt.md:10`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-check-implementation-readiness.prompt.md:11`
- Required registry/help/module/lifecycle evidence:
  - `lens.core/_bmad/_config/skill-manifest.csv:28`
  - `lens.core/_bmad/_config/bmad-help.csv:8`
  - `lens.core/_bmad/lens-work/module.yaml:130`
  - `lens.core/_bmad/lens-work/module.yaml:131`
  - `lens.core/_bmad/lens-work/module.yaml:249`
  - `lens.core/_bmad/lens-work/module.yaml:314`
  - `lens.core/_bmad/lens-work/module-help.csv:52`
  - `lens.core/_bmad/lens-work/module-help.csv:54`
  - `lens.core/_bmad/lens-work/module-help.csv:64`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:166`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:176`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:186`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:198`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:244`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:246`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:247`
- Relevant skill/workflow contracts:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:31`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:32`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:62`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:70`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:95`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:134`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-check-implementation-readiness/SKILL.md:6`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-check-implementation-readiness/workflow.md:3`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-check-implementation-readiness/workflow.md:17`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-check-implementation-readiness/workflow.md:21`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-check-implementation-readiness/workflow.md:26`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-check-implementation-readiness/workflow.md:28`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-check-implementation-readiness/workflow.md:38`
  - `lens.core/_bmad/bmm/3-solutioning/bmad-check-implementation-readiness/workflow.md:49`

## Confidence
- Overall: **High** for mapping and lifecycle placement.
- Rationale: prompt routing, global BMAD metadata, Lens module command surfaces, and lifecycle artifacts all converge on `finalizeplan`-phase readiness assessment with planning-doc output boundaries.
- Residual uncertainty: low-level execution details (menus, step transitions, intermediate outputs) are owned by the downstream BMAD workflow rather than these prompt stubs.

## Gaps
- Double-stub indirection (`.github` stub -> `lens.core` stub -> wrapper skill) means prompt-level text does not itself define acceptance criteria or output schema.
- Preflight behavior is split: only the control prompt explicitly invokes `light-preflight.py`; the release prompt itself is pure delegation.
- The downstream readiness workflow is strongly interactive and sequence-constrained (wait-for-input, halt-at-menu, no-step-skipping), but prompt surfaces do not state how non-interactive callers should handle that mode.
- Global BMAD metadata places this in `3-solutioning`, while Lens routes it under `finalizeplan`; this is coherent but cross-surface terminology can cause discoverability ambiguity if operators expect one naming system only.
