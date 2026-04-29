---
feature: lens-dev-new-codebase-preplan
doc_type: business-plan
status: draft
goal: "Define clean-room business requirements for preplan command parity in the new codebase"
key_decisions:
  - Treat the old-codebase preplan behavior and baseline story 4.1 acceptance criteria as behavioral evidence only; no source files are copied into the new codebase.
  - Preserve the public preplan command as a retained Lens command in the 17-command surface.
  - Require output parity for brainstorm-first ordering, batch 2-pass contract, review-ready fast path, and the no-governance-write invariant.
  - Enforce baseline stories 1-2 (validate-phase-artifacts shared utility) and 1-3 (batch 2-pass contract) as hard prerequisites before preplan rewrite begins.
  - Enforce baseline story 3-1 (fix-constitution-partial-hierarchy) as a prerequisite because preplan calls bmad-lens-constitution at activation.
open_questions:
  - Should the target-repo delegation interruption-and-resume model within preplan be implemented in this slice, or deferred as a follow-up story?
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# Business Plan — Preplan Command

## Executive Summary

The `preplan` command is the first phase in the full lifecycle track and the entry point for all governed feature work in Lens. Without a working preplan implementation in the new codebase, users cannot begin the planning journey from an empty feature state. The business value is completeness: preplan is one of five full-track planning commands and enforces the brainstorm-first ordering that separates disciplined Lens planning from unconstrained AI content generation. This feature delivers a clean-room preplan conductor in the new codebase, using the release SKILL.md as the behavioral specification and shared utilities as the implementation substrate.

## Business Context

The baseline rewrite targets 17 retained published commands, with `preplan` as the entry command of the full planning track (`preplan → businessplan → techplan → finalizeplan`). Preplan is responsible for producing three early-stage artifacts — brainstorm, research, and product brief — before any business requirements or technical design work begins. In the release payload (`lens.core/`), the SKILL.md for `bmad-lens-preplan` already describes the correct behavior, including brainstorm-first ordering, delegation to shared utilities, and the no-governance-write invariant. The gap is in the running implementation in `TargetProjects/lens-dev/new-codebase/lens.core.src`, where the command either does not exist or still uses inline batch and review-ready logic rather than the shared utilities mandated by the baseline architecture.

This feature closes that gap. The work is a clean-room rewrite of the preplan conductor. The old-codebase preplan prompt stub and the baseline story 4.1 acceptance criteria define the required outcomes. The new implementation is authored independently, with no direct file copying from the old codebase.

The broader importance: this is the dogfooding proof point the baseline rewrite was designed to achieve. Rebuilding Lens using Lens — starting with `preplan`, the command that launches every feature — demonstrates that the workflow can produce stability when it is actually obeyed. A broken preplan command would contradict that proof.

## Stakeholders

| Stakeholder | Interest | Sign-off Signal |
|---|---|---|
| Lens users starting new features | Need a working brainstorm-first preplan that stages all three artifacts before businessplan begins | They can run `/preplan`, complete the brainstorm → research → product-brief session, and proceed to `/businessplan` without manual repair |
| Lens users in batch mode | Need the 2-pass batch contract to work consistently with no broken inline batch logic | Pass 1 writes `preplan-batch-input.md` and stops cleanly; pass 2 resumes with pre-approved context loaded |
| Lens maintainers | Need preplan to be a thin conductor backed by shared utilities, not a duplicate inline implementation | Removing a shared utility and running preplan should produce an explicit error, not silent incorrect behavior |
| Release integrators | Need the preplan command to be part of the installed 17-command surface and discoverable | Prompt stub, release prompt, SKILL.md, `module-help.csv`, and `lens.agent.md` all agree `preplan` exists and routes correctly |

## Success Criteria

| Criterion | Measure |
|---|---|
| Published command availability | `lens-preplan.prompt.md` exists in the new-codebase installed prompt surface and delegates through light preflight to the release prompt |
| Release prompt parity | The release prompt loads `bmad-lens-preplan` and describes the same preplan phase outcomes as the release SKILL.md |
| Brainstorm-first ordering | Non-batch interactive runs always activate `bmad-agent-analyst` first to frame requirements context; the conductor then presents the user with a choice of brainstorm mode (`bmad-brainstorming` or `bmad-cis`) before any research or product-brief authoring begins; a brainstorm.md must exist before downstream synthesis is invoked |
| Batch delegation parity | Batch mode delegates to `bmad-lens-batch --target preplan` with no inline batch logic in the conductor; pass 1 stops after writing `preplan-batch-input.md`; pass 2 resumes with approved context |
| Review-ready delegation parity | Review-ready check calls `validate-phase-artifacts.py --phase preplan --contract review-ready` with no inline artifact detection logic in the conductor |
| Phase completion gate parity | Adversarial review (party mode) runs before `feature.yaml` is updated; a `fail` verdict blocks the phase transition |
| No governance writes during preplan | Preplan stages artifacts in the control-repo docs path only; `publish-to-governance` is NOT called during preplan; BusinessPlan publishes the reviewed preplan set at phase handoff |
| Constitution prerequisite enforced | Preplan validates that `bmad-lens-constitution` resolves the service hierarchy without hard-failing on missing org-level entries (baseline story 3-1 must be in place) |
| Clean-room assurance | Implementation and docs are authored from the behavioral specification; no old-codebase files are copied into the new codebase |
| Regression coverage | Focused parity tests pass for brainstorm-first ordering, batch pass semantics, review-ready routing, phase completion gate, and no-governance-write invariant |

## Scope

### In Scope

- Add the user-facing `lens-preplan` prompt stub to the new-codebase published prompt surface.
- Add the release prompt redirect for `lens-preplan` under `_bmad/lens-work/prompts/`.
- Implement `bmad-lens-preplan` as a thin conductor SKILL.md in `TargetProjects/lens-dev/new-codebase/lens.core.src` with no owned script layer.
- Wire the review-ready fast path through `validate-phase-artifacts.py` with no inline logic in the conductor.
- Wire the batch 2-pass contract through `bmad-lens-batch` with no inline batch logic.
- Activate `bmad-agent-analyst` at the start of the interactive flow to frame requirements context before any brainstorm mode is selected.
- Wire the brainstorm mode choice — present the user with options for `bmad-brainstorming` (divergent ideation) and `bmad-cis` (structured creative innovation) — through `bmad-lens-bmad-skill` after analyst framing completes.
- Wire research and product-brief authoring through `bmad-lens-bmad-skill`.
- Wire the phase completion adversarial review gate through `bmad-lens-adversarial-review`.
- Add focused parity tests for brainstorm-first ordering, batch pass semantics, review-ready routing, phase gate, and governance-write invariant.
- Confirm module-help.csv and lens.agent.md include `preplan` as a retained command.

### Out of Scope

- Changing the preplan artifact set (brainstorm, research, product-brief are fixed outputs).
- Changing phase completion requirements (party-mode adversarial review gate is frozen).
- Implementing the businessplan publish-before-author hook (owned by the businessplan feature).
- Changes to bmad-lens-target-repo interruption-and-resume behavior (separate concern; deferred unless accepted into scope by the team).
- Changes to lifecycle.yaml, feature.yaml schema, or branch topology.
- Copying old-codebase implementation files into the new codebase.

## Risks and Mitigations

| Risk | Probability | Impact | Mitigation |
|---|---:|---:|---|
| Baseline Epic 1 shared utilities (validate-phase-artifacts.py, bmad-lens-batch) not ready when preplan rewrite begins | Medium | High | Treat baseline stories 1-2 and 1-3 as hard prerequisites with `depends_on` gates; do not begin preplan conductor work until both pass in the new codebase |
| Constitution partial-hierarchy fix (baseline story 3-1) not complete | Medium | High | Enforce story 3-1 as a prerequisite; preplan calls `bmad-lens-constitution` and will silently fail or panic on org-level lookups without the fix |
| Brainstorm-first ordering inadvertently skipped in interactive mode | Low | Medium | Add a parity test that asserts no research or product-brief BMAD wrapper is invoked before `brainstorm.md` exists in the docs path |
| Inline batch logic accidentally re-introduced during implementation | Low | Medium | Code review gate: any inline `if mode == batch and batch_resume_context absent` logic in the conductor is treated as a defect |
| Review-ready check reimplemented inline instead of delegated | Low | Medium | Parity test asserts `validate-phase-artifacts.py` is called via the canonical CLI invocation and no inline file presence checks exist in the conductor |
| Governance publish incorrectly triggered during preplan | Low | High | Test the no-governance-write invariant explicitly: run preplan to completion and assert `publish-to-governance` was not invoked at any step |
