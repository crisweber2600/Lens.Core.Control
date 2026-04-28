---
feature: lens-dev-new-codebase-expressplan
doc_type: tech-plan
status: draft
goal: "Implement clean-room /expressplan parity through a retained prompt, the bmad-lens-expressplan conductor, wrapper-based QuickPlan delegation, and explicit FinalizePlan reuse"
key_decisions:
  - Keep expressplan as a dedicated conductor rather than embedding express logic into businessplan or techplan.
  - Preserve QuickPlan as an internal Lens-wrapped dependency instead of a public top-level workflow.
  - Treat adversarial review as a hard stop on fail, not as advisory output.
  - Reuse FinalizePlan for downstream bundle generation and final PR topology.
  - Keep governance publication and phase mutation behind existing Lens scripts rather than direct file writes.
open_questions:
  - Whether older references to expressplan-review.md need a temporary compatibility alias in validators or docs.
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T22:50:00Z
---

# Technical Plan - ExpressPlan Command

## Technical Summary

Implement `expressplan` as a clean-room retained command that preserves the existing observable chain:

```text
published stub prompt
  -> release prompt
  -> bmad-lens-expressplan
  -> bmad-lens-bmad-skill --skill bmad-lens-quickplan
  -> bmad-lens-adversarial-review --phase expressplan
  -> bmad-lens-finalizeplan bundle reuse
```

The command should stay narrow. Its job is to compress the front half of planning safely. It is not a second planning architecture. It validates eligibility, runs QuickPlan through the Lens wrapper, enforces a hard review gate, and then hands off to FinalizePlan for the downstream bundle.

## Architecture Overview

### Published Surface

The public entry chain should remain:

```text
.github/prompts/lens-expressplan.prompt.md
  -> lens.core/_bmad/lens-work/prompts/lens-expressplan.prompt.md
  -> lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md
```

The stub prompt owns only light preflight and delegation. No lifecycle logic belongs there.

### Orchestration Layers

| Layer | Responsibility |
|---|---|
| Prompt stub | Run light preflight, stop on failure, load release prompt |
| Release prompt | Load the skill and no more |
| `bmad-lens-expressplan` | Feature resolution, express-track gating, step ordering, and handoff decisions |
| `bmad-lens-bmad-skill` | Lens-aware context injection and write-boundary enforcement for QuickPlan |
| `bmad-lens-adversarial-review` | Hard-gate review packet generation and review artifact write |
| `bmad-lens-finalizeplan` | Downstream bundle generation, PR topology, and `/dev` handoff |

### Data Flow

1. Resolve feature context and track.
2. Validate the feature can legally use expressplan.
3. Write staged planning docs to the control-repo docs path.
4. Review the staged docs and write the expressplan review artifact.
5. On pass or pass-with-warnings, signal FinalizePlan bundle execution.

## Design Decisions (ADRs)

### ADR 1 - Keep ExpressPlan as a Dedicated Conductor

**Decision:** Preserve `bmad-lens-expressplan` as its own workflow skill.

**Rationale:** The retained command surface exposes `expressplan` as a first-class command with its own gating and handoff behavior. Folding it into `businessplan` or `techplan` would erase an observable entrypoint and blur where the express-only eligibility check belongs.

### ADR 2 - Keep QuickPlan Internal but Mandatory

**Decision:** Continue to invoke QuickPlan through `bmad-lens-bmad-skill --skill bmad-lens-quickplan`.

**Rationale:** Research explicitly flags QuickPlan retention as load-bearing. It should not remain a published user command, but deleting the internal capability would silently break expressplan.

### ADR 3 - Review Failure Is a Hard Stop

**Decision:** A `fail` verdict from `bmad-lens-adversarial-review --phase expressplan` blocks progression.

**Rationale:** ExpressPlan trades time for compression, not quality for convenience. If review becomes advisory, the command stops being a governed planning path.

### ADR 4 - Downstream Bundle Is Reused, Not Reimplemented

**Decision:** FinalizePlan owns epics, stories, readiness, sprint status, story files, and final PR topology.

**Rationale:** The downstream bundle is already the most complex planning stage. Duplicating it under expressplan would create parity drift and double the regression surface.

### ADR 5 - Write Boundaries Stay Scripted

**Decision:** ExpressPlan writes staged docs in the control repo and relies on existing Lens script boundaries for governance publication and phase mutation.

**Rationale:** Current Lens rules treat the feature docs path in the control repo as authoritative and governance as a mirror. That boundary should remain explicit.

### ADR 6 - Standardize Current Review Artifact Naming

**Decision:** Stage `expressplan-adversarial-review.md` as the review artifact and treat older `expressplan-review.md` references as compatibility debt.

**Rationale:** Current lifecycle metadata and recent planning work both point to the `-adversarial-review` filename. Standardizing there avoids guesswork during implementation.

## Contracts

### Prompt Contract

Expected stub behavior:

```bash
uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py
```

- On non-zero exit: stop and surface the error.
- On success: read and follow the release prompt.

### Skill Contract

The skill should preserve this three-step sequence:

1. **QuickPlan via Lens wrapper**
   - delegate to `bmad-lens-bmad-skill --skill bmad-lens-quickplan`
   - write `business-plan.md`, `tech-plan.md`, and `sprint-plan.md`
2. **Adversarial review hard gate**
   - run `bmad-lens-adversarial-review --phase expressplan --source phase-complete`
   - halt on `fail`
3. **Advance to FinalizePlan**
   - mark the expressplan step complete through existing feature-state tooling
   - auto-advance into FinalizePlan bundle reuse

Breaking change: false.

### File Contract

Staged expressplan artifacts:

- `business-plan.md`
- `tech-plan.md`
- `sprint-plan.md`
- `expressplan-adversarial-review.md`

Staged FinalizePlan bundle after handoff:

- `finalizeplan-review.md`
- `epics.md`
- `stories.md`
- `implementation-readiness.md`
- `sprint-status.yaml`
- `stories/*.md`

## Impacted Files and Surfaces

| Surface | Purpose |
|---|---|
| `.github/prompts/lens-expressplan.prompt.md` | Public command stub |
| `lens.core/_bmad/lens-work/prompts/lens-expressplan.prompt.md` | Release prompt |
| `lens.core/_bmad/lens-work/skills/bmad-lens-expressplan/SKILL.md` | ExpressPlan orchestration contract |
| `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md` | Wrapper-based QuickPlan delegation |
| `lens.core/_bmad/lens-work/skills/bmad-lens-adversarial-review/SKILL.md` | Hard-gate review |
| `lens.core/_bmad/lens-work/skills/bmad-lens-finalizeplan/SKILL.md` | Downstream bundle reuse |
| `lens.core/_bmad/lens-work/lifecycle.yaml` | Phase metadata, artifact names, and auto-advance contract |
| `lens.core/_bmad/lens-work/module-help.csv` | Help/discoverability surface |

## Testing Strategy

Focused verification should prove:

1. `/expressplan` remains discoverable and routes through light preflight.
2. Unsupported track usage fails with a clear explanation.
3. QuickPlan is still invoked through the Lens wrapper.
4. Review fail halts before FinalizePlan handoff.
5. Review pass or pass-with-warnings produces the expected review artifact name.
6. ExpressPlan reuses FinalizePlan for downstream bundle generation instead of duplicating it.
7. Help/module surfaces continue to list expressplan.

## Rollout Notes

Implementation can be delivered incrementally:

1. Preserve prompt and help surfaces.
2. Restore or refine the expressplan skill contract.
3. Verify wrapper-based QuickPlan delegation.
4. Wire the hard review gate.
5. Prove FinalizePlan bundle reuse through narrow tests.

These planning artifacts are staged only. Live governance publication, branch validation, and phase mutation should happen through the existing Lens operational scripts once the worktree is clean and the implementation exists.