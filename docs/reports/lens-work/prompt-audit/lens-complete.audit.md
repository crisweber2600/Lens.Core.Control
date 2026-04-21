# lens-complete Prompt Audit

## Purpose Summary
- [.github/prompts/lens-complete.prompt.md](.github/prompts/lens-complete.prompt.md#L5) is a control-level stub; it does not define workflow behavior directly.
- The control prompt delegates to the release prompt at [lens.core/_bmad/lens-work/prompts/lens-complete.prompt.md](lens.core/_bmad/lens-work/prompts/lens-complete.prompt.md#L10), and that prompt is also a stub delegating to [lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md](lens.core/_bmad/lens-work/prompts/lens-complete.prompt.md#L10).
- Effective behavior is therefore implemented in the complete skill contract and its references/scripts: retrospective gate, wrapped project documentation gate, then archival/finalization.

## BMAD Skill Mapping
- Lens module registration is explicit: [bmad-lens-complete in module.yaml](lens.core/_bmad/lens-work/module.yaml#L79), with prompt distribution listed at [module prompt list](lens.core/_bmad/lens-work/module.yaml#L227) and adapter surface [module install prompt mapping](lens.core/_bmad/lens-work/module.yaml#L292).
- Lens command surface exposes complete operations in [module-help CK](lens.core/_bmad/lens-work/module-help.csv#L26), [module-help FF](lens.core/_bmad/lens-work/module-help.csv#L27), and [module-help AR](lens.core/_bmad/lens-work/module-help.csv#L46).
- Runtime routing also maps phase resolution to complete: [Phase=complete -> /complete](lens.core/_bmad/lens-work/skills/bmad-lens-next/SKILL.md#L49) and [/complete -> bmad-lens-complete](lens.core/_bmad/lens-work/skills/bmad-lens-next/SKILL.md#L75).
- Upstream BMAD dependencies used by complete are present in global registries: [skill-manifest bmad-retrospective](lens.core/_bmad/_config/skill-manifest.csv#L42), [skill-manifest bmad-document-project](lens.core/_bmad/_config/skill-manifest.csv#L16), [bmad-help Retrospective](lens.core/_bmad/_config/bmad-help.csv#L26), and [bmad-help Document Project](lens.core/_bmad/_config/bmad-help.csv#L18).
- Complete skill explicitly requires those Lens-wrapped capabilities: [retrospective integration](lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md#L108) and [document-project integration](lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md#L110).

## Lifecycle Fit
- Lifecycle progression ends canonical planning at [phase_order](lens.core/_bmad/lens-work/lifecycle.yaml#L238) with FinalizePlan auto-transitioning to [auto_advance_to: /dev](lens.core/_bmad/lens-work/lifecycle.yaml#L181); complete/close is not a listed lifecycle phase.
- Lifecycle closure semantics are milestone/state-based: [close_states](lens.core/_bmad/lens-work/lifecycle.yaml#L21), [dev-complete milestone](lens.core/_bmad/lens-work/lifecycle.yaml#L94), and [close_validation requires /close guard](lens.core/_bmad/lens-work/lifecycle.yaml#L103).
- Complete skill semantics align partially with lifecycle endpoint intent by enforcing preconditions and terminal writes: [Pre-conditions section](lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md#L55), [phase must be dev or complete](lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md#L59), and finalize outcomes in [finalize reference](lens.core/_bmad/lens-work/skills/bmad-lens-complete/references/finalize-feature.md#L7).
- Document-before-archive behavior is strongly aligned and explicit: [non-negotiable statement](lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md#L12), [principle](lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md#L31), and [reference gate language](lens.core/_bmad/lens-work/skills/bmad-lens-complete/references/document-project.md#L11).
- Important mismatch: skill preconditions mention constitution compliance [SKILL.md](lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md#L61), but script enforcement appears limited to phase + retrospective checks ([phase constants](lens.core/_bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py#L24), [phase check](lens.core/_bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py#L212), [retrospective warning](lens.core/_bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py#L220)).
- Terminology mismatch risk: lifecycle close state uses `completed` while complete script writes feature phase `complete` and index status `archived` ([close_states](lens.core/_bmad/lens-work/lifecycle.yaml#L21), [TERMINAL_PHASE](lens.core/_bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py#L25), [ARCHIVED_STATUS](lens.core/_bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py#L26)).

## Evidence Refs
- Prompt chain
  - [Control prompt stub title](.github/prompts/lens-complete.prompt.md#L5)
  - [Control prompt delegation](.github/prompts/lens-complete.prompt.md#L10)
  - [Release prompt stub title](lens.core/_bmad/lens-work/prompts/lens-complete.prompt.md#L5)
  - [Release prompt delegates to complete skill](lens.core/_bmad/lens-work/prompts/lens-complete.prompt.md#L10)
- Required registry/help/module/lifecycle sources
  - [skill-manifest: bmad-document-project](lens.core/_bmad/_config/skill-manifest.csv#L16)
  - [skill-manifest: bmad-retrospective](lens.core/_bmad/_config/skill-manifest.csv#L42)
  - [bmad-help: Document Project](lens.core/_bmad/_config/bmad-help.csv#L18)
  - [bmad-help: Retrospective](lens.core/_bmad/_config/bmad-help.csv#L26)
  - [module.yaml: bmad-lens-complete skill](lens.core/_bmad/lens-work/module.yaml#L79)
  - [module.yaml: lens-complete prompt listed](lens.core/_bmad/lens-work/module.yaml#L227)
  - [module-help: complete-doc-check](lens.core/_bmad/lens-work/module-help.csv#L26)
  - [module-help: finalize-feature](lens.core/_bmad/lens-work/module-help.csv#L27)
  - [module-help: archive-status](lens.core/_bmad/lens-work/module-help.csv#L46)
  - [lifecycle close states](lens.core/_bmad/lens-work/lifecycle.yaml#L21)
  - [lifecycle finalizeplan -> /dev](lens.core/_bmad/lens-work/lifecycle.yaml#L181)
  - [lifecycle phase order](lens.core/_bmad/lens-work/lifecycle.yaml#L238)
  - [lifecycle dev-complete close validation note](lens.core/_bmad/lens-work/lifecycle.yaml#L103)
- Relevant prompt/skill files
  - [Complete skill non-negotiable documentation gate](lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md#L12)
  - [Complete skill principle: document-before-archive](lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md#L31)
  - [Complete skill precondition: phase](lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md#L59)
  - [Complete skill precondition: constitution compliance](lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md#L61)
  - [Document-project reference contract](lens.core/_bmad/lens-work/skills/bmad-lens-complete/references/document-project.md#L7)
  - [Document-project non-negotiable gating](lens.core/_bmad/lens-work/skills/bmad-lens-complete/references/document-project.md#L11)
  - [Finalize reference terminal outcomes](lens.core/_bmad/lens-work/skills/bmad-lens-complete/references/finalize-feature.md#L7)
  - [Finalize irreversible confirmation requirement](lens.core/_bmad/lens-work/skills/bmad-lens-complete/references/finalize-feature.md#L16)
  - [Script: completable phases](lens.core/_bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py#L24)
  - [Script: terminal phase value](lens.core/_bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py#L25)
  - [Script: archived status value](lens.core/_bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py#L26)
  - [Script: phase precondition check](lens.core/_bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py#L212)
  - [Script: retrospective warning only](lens.core/_bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py#L220)
  - [Script: finalize updates feature phase](lens.core/_bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py#L274)
  - [Script: finalize updates index status](lens.core/_bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py#L285)

## Confidence
- Overall: High.
- Rationale: Prompt-level behavior is explicit delegation, and the module/help/lifecycle/skill/script surfaces are all directly inspectable and mostly consistent on command wiring and archival intent.
- Residual uncertainty: Medium on intended `/close` vs `/complete` naming convergence because lifecycle comments reference `/close`, while operational command surfaces expose `/complete`.

## Gaps
- Double-stub indirection reduces prompt-local observability. Both control and release prompts are wrappers, so behavioral truth lives outside prompt files.
- Lifecycle vs complete terminology is inconsistent (`completed` close state vs script `complete` phase and `archived` index status), increasing risk of state interpretation drift in downstream tooling.
- Skill contract includes a constitution compliance precondition, but script-level checks do not show constitution validation logic; enforcement currently appears to rely on conversational/operator discipline.
- Retrospective/documentation are described as non-negotiable, but script preconditions treat missing retrospective as warning (`warn`) rather than blocker, creating a policy-strength mismatch.
- Dependency mapping spans multiple synchronized surfaces (manifest/help/module/module-help/lifecycle + skill references); drift in any one surface can break discoverability or governance expectations.