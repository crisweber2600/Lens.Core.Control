---
feature: lens-dev-new-codebase-preplan
doc_type: tech-plan
status: draft
goal: "Implement clean-room preplan command parity in the new codebase as a thin conductor backed by shared utilities"
key_decisions:
  - Preplan has no owned script layer; all non-authoring operations delegate to shared utilities.
  - Brainstorm-first ordering is enforced at the conductor level before any BMAD wrapper is invoked.
  - Batch mode delegates entirely to bmad-lens-batch; no inline batch logic in the conductor.
  - Review-ready check delegates entirely to validate-phase-artifacts.py; no inline artifact detection.
  - No governance writes during preplan; publish-to-governance is deferred to businessplan phase handoff.
  - Constitution partial-hierarchy fix (baseline story 3-1) is a hard prerequisite; preplan does not add a workaround.
open_questions:
  - Whether bmad-lens-target-repo interruption-and-resume within preplan is required for parity in this delivery slice.
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# Tech Plan — Preplan Command

## Technical Summary

Implement `lens-preplan` in the new codebase as a clean-room conductor that matches the behavioral specification in the release SKILL.md (`lens.core/_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md`). The implementation adds the missing prompt surfaces, writes a thin SKILL.md conductor under `TargetProjects/lens-dev/new-codebase/lens.core.src`, and delegates all non-authoring operations to the shared utilities extracted by baseline Epics 1 and 3. The design is deliberately minimal: preplan owns no script, produces no files directly, and calls shared utilities and BMAD wrappers for every operation it performs.

## Architecture Overview

The command follows the invariant 3-hop chain used by all 17 retained commands:

```text
.github/prompts/lens-preplan.prompt.md              (stub — user entry point)
  -> _bmad/lens-work/prompts/lens-preplan.prompt.md (release prompt — thin redirect)
    -> _bmad/lens-work/skills/bmad-lens-preplan/SKILL.md  (conductor — no owned script)
      -> validate-phase-artifacts.py                (review-ready fast path)
      -> bmad-lens-batch                            (batch 2-pass contract)
      -> bmad-lens-constitution                     (domain constitution loading)
      -> bmad-agent-analyst                         (requirements framing before brainstorm mode selection)
      -> bmad-lens-bmad-skill (bmad-brainstorming | bmad-cis)  (brainstorm authoring — user-selected mode)
      -> bmad-lens-bmad-skill (research wrappers)   (research authoring)
      -> bmad-lens-bmad-skill (bmad-product-brief)  (product-brief authoring)
      -> bmad-lens-adversarial-review               (phase completion gate)
      -> bmad-lens-feature-yaml                     (phase state update)
      -> bmad-lens-git-orchestration                (control-repo artifact commit)
```

The implementation target is `TargetProjects/lens-dev/new-codebase/lens.core.src`. The release payload under `lens.core/` and the old-codebase tree remain read-only behavioral references.

The SKILL.md is the only implementation artifact owned by this feature in the skills directory. There is no `preplan-ops.py` script and no new shared utility introduced.

## Implementation Structure

```text
TargetProjects/lens-dev/new-codebase/lens.core.src/
  .github/prompts/
    lens-preplan.prompt.md           <- stub (light preflight + redirect to release prompt)
  _bmad/lens-work/
    prompts/
      lens-preplan.prompt.md         <- thin redirect: load and follow SKILL.md
    skills/bmad-lens-preplan/
      SKILL.md                       <- conductor (no script layer)
```

## Design Decisions (ADRs)

### ADR 1 — No Owned Script Layer

**Decision:** `bmad-lens-preplan` has no owned implementation scripts (no `preplan-ops.py` or equivalent). A `scripts/tests/` subdirectory for parity test fixtures is permitted and does not constitute an owned script layer; the "no scripts" constraint applies exclusively to implementation scripts.

**Rationale:** Preplan produces artifacts via BMAD analyst and brainstorming agents, not file-writing scripts. Every non-authoring operation — review-ready checking, batch intake, phase completion review, and phase state mutation — is owned by a shared utility that preplan delegates to. Adding a proprietary script layer would duplicate logic that the shared utilities own and maintain.

**Alternatives Rejected:**
- Create a `preplan-ops.py` for orchestration: rejected because the three shared utilities already cover all the logic that a script would contain.

### ADR 2 — Brainstorm-First Ordering Enforced at Conductor Level

**Decision:** The conductor asks BMAD brainstorming setup questions before invoking any document authoring wrapper. A `brainstorm.md` must exist in the docs path before the conductor asks whether to synthesize research or a product brief.

**Rationale:** The "no PRD leap" principle requires brainstorming context before downstream synthesis. BMAD wrappers for research and product-brief are unaware of the preplan sequencing contract. The conductor is the only place that can enforce this ordering consistently.

**Alternatives Rejected:**
- Let the user choose the order: rejected because it allows skipping brainstorming, breaking the phase's behavioral contract with the next phase (businessplan).
- Enforce ordering inside the BMAD brainstorming skill: rejected because it would push a preplan-specific ordering rule into a general-purpose skill.

### ADR 7 — Analyst Activation and Brainstorm Mode Choice

**Decision:** Before invoking any brainstorm authoring wrapper, the conductor activates `bmad-agent-analyst` to frame requirements context (goals, constraints, and known assumptions for the feature). After analyst framing completes, the conductor presents the user with a choice between `bmad-brainstorming` (divergent creative ideation) and `bmad-cis` (structured creative innovation suite). Both modes route through `bmad-lens-bmad-skill`. The `brainstorm.md` existence gate applies regardless of which mode is selected.

**Rationale:** Analyst framing prevents the brainstorming session from starting without a grounded understanding of the feature's purpose, reducing artifact rework in the research and product-brief steps. Offering `bmad-cis` as an alternative to `bmad-brainstorming` serves users who prefer a structured innovation methodology over open divergent ideation; both paths produce a `brainstorm.md` that satisfies the preplan ordering contract.

**Alternatives Rejected:**
- Hard-wire `bmad-brainstorming` only: rejected because it removes the structured innovation option that `bmad-cis` provides for more complex or ambiguous feature spaces.
- Skip analyst framing and go directly to brainstorm mode selection: rejected because it allows sessions to begin without grounding context, which increases downstream rework in research and product-brief phases.

### ADR 3 — Batch Mode Delegates Entirely to `bmad-lens-batch`

**Decision:** In batch mode, the conductor calls `bmad-lens-batch --target preplan` and stops on pass 1. Pass 2 resumes with `batch_resume_context` loaded as pre-approved answers. No inline batch logic in the conductor.

**Rationale:** Baseline story 1-3 extracts the 2-pass batch contract as a shared utility. Inline batch logic in preplan would duplicate that contract and require coordinated edits across multiple phase files when the shared contract evolves.

**Alternatives Rejected:**
- Keep the inline `if mode == batch and batch_resume_context absent` block: rejected because it violates the shared utility extraction requirement from the baseline architecture.

### ADR 4 — Review-Ready Check Delegates Entirely to `validate-phase-artifacts.py`

**Decision:** The conductor calls `uv run .../validate-phase-artifacts.py --phase preplan --contract review-ready --lifecycle-path ... --docs-root ... --json` and acts on the JSON status field. No inline file presence checks.

**Rationale:** Baseline story 1-2 implements the review-ready fast path as a single shared script. Inline artifact detection in each phase skill diverges from the lifecycle contract over time (e.g., when artifact names change, only the script is updated).

**Alternatives Rejected:**
- Check for `brainstorm.md`, `research.md`, `product-brief.md` directly in the SKILL.md logic: rejected because it duplicates what the shared script owns and decouples the fast path from the lifecycle contract definition.

### ADR 5 — No Governance Writes During Preplan

**Decision:** Preplan commits artifacts to the control-repo docs path via `bmad-lens-git-orchestration` for local staging only. It does NOT call `publish-to-governance`. BusinessPlan calls `publish-to-governance --phase preplan` at its entry hook, publishing the reviewed preplan set (including the adversarial review report) to the governance mirror.

**Rationale:** The lifecycle contract requires reviewed artifacts before governance publication. Preplan produces draft artifacts that receive adversarial review only at phase completion; at that point they are ready for governance publication, but the publish step belongs to the next phase entry — not to preplan's completion step.

**Alternatives Rejected:**
- Publish to governance after the adversarial review passes: rejected because it breaks the publish-before-author hook contract that BusinessPlan owns and creates a race condition if the user does not immediately proceed to BusinessPlan.

### ADR 6 — Constitution Prerequisite Enforced via Baseline Story 3-1

**Decision:** Preplan calls `bmad-lens-constitution` without adding a workaround for missing org-level constitution entries. The fix belongs exclusively in `bmad-lens-constitution` (baseline story 3-1).

**Rationale:** Adding a preplan-local workaround would duplicate the fix across multiple phase skills. The correct resolution is the canonical shared fix in the constitution skill, which is a stated prerequisite for the preplan rewrite.

**Alternatives Rejected:**
- Add a `try/except` fallback in the conductor for missing org constitution: rejected because it masks a real bug that story 3-1 is required to fix.

## Dependency Map

| Dependency | Role | Activation Step |
|---|---|---|
| `validate-phase-artifacts.py` | Review-ready fast path — determines if authoring can be skipped | On Activation step 14 |
| `bmad-lens-batch` | Batch 2-pass contract — pass 1 writes intake file; pass 2 resumes | On Activation steps 12-13 |
| `bmad-lens-constitution` | Domain constitution loading and partial hierarchy resolution | On Activation step 8 |
| `bmad-lens-feature-yaml` | Read feature.yaml at activation; write phase update at completion | On Activation step 3; Phase Completion step 2 |
| `bmad-lens-init-feature` (fetch-context) | Cross-feature context loading (related summaries, optional named-service docs) | On Activation step 7 |
| `bmad-agent-analyst` | Requirements framing — activated before brainstorm mode is selected to establish feature context and scope constraints | On Activation step 11.3 |
| `bmad-lens-bmad-skill` → `bmad-brainstorming` | Brainstorm artifact authoring (divergent ideation mode — user choice) | On Activation step 11.4 |
| `bmad-lens-bmad-skill` → `bmad-cis` | Brainstorm artifact authoring (structured creative innovation mode — user choice) | On Activation step 11.4 |
| `bmad-lens-bmad-skill` → `bmad-domain-research` / `bmad-market-research` / `bmad-technical-research` | Research artifact authoring (narrowest applicable wrapper selected at runtime) | On Activation step 11.8 |
| `bmad-lens-bmad-skill` → `bmad-product-brief` | Product brief artifact authoring | On Activation step 11.8 |
| `bmad-lens-adversarial-review` | Phase completion gate (party mode, `--phase preplan --source phase-complete`) | Phase Completion step 1 |
| `bmad-lens-git-orchestration` | Control-repo staging commits for preplan artifacts | Phase Completion (before or after review) |

## Testing Strategy

### Parity Test Categories

| Category | Test Assertion | Risk Mitigated |
|---|---|---|
| Analyst activation ordering | `bmad-agent-analyst` is invoked before any brainstorm mode wrapper is called | Analyst-skip regression |
| Brainstorm mode choice — bmad-brainstorming | When user selects `bmad-brainstorming`, `bmad-lens-bmad-skill` is called with `bmad-brainstorming`; `bmad-cis` is not invoked | Mode routing error |
| Brainstorm mode choice — bmad-cis | When user selects `bmad-cis`, `bmad-lens-bmad-skill` is called with `bmad-cis`; `bmad-brainstorming` is not invoked | Mode routing error |
| Brainstorm-first ordering | No research or product-brief BMAD wrapper is invoked before `brainstorm.md` exists in the staged docs path (applies to both modes) | Brainstorm-skip regression |
| Batch delegation — pass 1 | On batch pass 1, `bmad-lens-batch --target preplan` is called; no lifecycle artifacts are written; the conductor stops | Inline batch re-introduction |
| Batch delegation — pass 2 | On batch pass 2 with `batch_resume_context` loaded, the conductor resumes without re-asking setup questions | Pass 2 loop regression |
| Review-ready delegation | `validate-phase-artifacts.py` is called with `--phase preplan --contract review-ready` args; no inline file-check logic | Inline review-ready re-introduction |
| Review-ready fast path routing | When `validate-phase-artifacts.py` returns `status=pass` and feature phase is `preplan`, the conductor skips authoring and proceeds directly to adversarial review | Fast path bypass |
| Phase gate — fail verdict | When adversarial review returns `fail`, `feature.yaml` is NOT updated and the conductor stops with blocking findings | Gate bypass |
| Phase gate — pass verdict | When adversarial review returns `pass` or `pass-with-warnings`, `feature.yaml` is updated to `preplan-complete` | Gate false-block |
| No governance writes | After a full preplan run (including phase completion), `publish-to-governance` has NOT been called | Accidental governance publish |
| Constitution prerequisite | Preplan activation with a partial constitution hierarchy (org level missing) does not panic; constitution skill handles it gracefully | Regression without story 3-1 |

### Test Location

`TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-preplan/scripts/tests/test-preplan-parity.py`

All tests use the same focused invocation pattern as other Lens parity tests:

```bash
uv run --with pytest pytest _bmad/lens-work/skills/bmad-lens-preplan/scripts/tests/test-preplan-parity.py -q
```

## Technical Constraints

- **BMB-first:** All source changes to `lens.core.src` are routed through the BMB implementation channel, not applied directly.
- **Schema v4 frozen:** `lifecycle.yaml` schema v4 must not be altered; preplan reads it via `validate-phase-artifacts.py` and the constitution skill.
- **Stub path invariant:** The stub at `.github/prompts/lens-preplan.prompt.md` must run `light-preflight.py` before delegating. This contract cannot change.
- **Governance write boundary:** `publish-to-governance` may not be called from within preplan under any code path.
- **Wrapper-only authoring:** Preplan must never write `brainstorm.md`, `research.md`, or `product-brief.md` directly; authoring is always routed through `bmad-lens-bmad-skill`.
