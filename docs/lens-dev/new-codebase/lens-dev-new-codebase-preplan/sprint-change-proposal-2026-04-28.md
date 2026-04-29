---
feature: lens-dev-new-codebase-preplan
doc_type: sprint-change-proposal
status: approved
date: 2026-04-28
scope: moderate
handoff: developer
---

# Sprint Change Proposal — Analyst Activation and Brainstorm Mode Choice

**Feature:** `lens-dev-new-codebase-preplan`
**Date:** 2026-04-28
**Scope:** Moderate — Story PP-2.2 expanded; PP-1.2 and PP-1.3 acceptance criteria updated; tech-plan and business-plan updated.
**Handoff:** Developer (backlog update; stories remain within Sprint 2)

---

## Section 1: Issue Summary

**Problem Statement:**
The preplan conductor's brainstorm-first step was hard-wired to invoke `bmad-brainstorming` directly, with no analyst framing step and no alternative ideation mode. Two requirements were missing from the approved plan:

1. `bmad-agent-analyst` must activate before any brainstorm mode is selected, to frame requirements context (goals, constraints, known assumptions) for the feature before ideation begins.
2. The conductor must offer the user a choice between `bmad-brainstorming` (divergent ideation) and `bmad-cis` (structured creative innovation suite) rather than silently selecting one.

**Discovery Context:**
Identified during sprint planning review of PP-2.2 acceptance criteria against the intended Lens brainstorm-first contract. The analyst framing step and the `bmad-cis` option were present in the broader Lens command design intent but were not carried into the preplan planning artifacts.

**Evidence:**
- PP-2.2 AC said: "invokes `bmad-lens-bmad-skill` with `bmad-brainstorming` as first authoring step" — no analyst step, no mode choice.
- Business plan success criterion "Brainstorm-first ordering" described only BMAD brainstorming setup questions — no analyst framing.
- Tech plan dependency map listed only `bmad-brainstorming` under brainstorm authoring — `bmad-cis` and `bmad-agent-analyst` were absent.

---

## Section 2: Impact Analysis

### Epic Impact
| Epic/Sprint | Impact |
|---|---|
| Sprint 1 | PP-1.2 and PP-1.3 AC updated — SKILL.md contract now documents analyst + mode choice; parity test skeletons expanded |
| Sprint 2 | PP-2.2 scope expanded — story now includes analyst activation + mode selection; no additional stories needed |
| Sprint 3–4 | Not affected — shared utility wiring and phase completion are unchanged |

### Story Impact
| Story | Change | Reason |
|---|---|---|
| PP-1.2 | AC updated: contract documents analyst activation and mode choice | SKILL.md must specify the full flow before implementation |
| PP-1.3 | AC updated: three new test skeleton categories added | Analyst ordering, bmad-brainstorming path, bmad-cis path |
| PP-2.2 | Title and AC updated: analyst activation + mode choice added | Core story for this change; complexity remains M |

### Artifact Conflicts
| Artifact | Change |
|---|---|
| `business-plan.md` | Success criterion "Brainstorm-first ordering" updated; In-Scope list split into analyst activation, mode-choice wiring, and research/product-brief wiring |
| `tech-plan.md` | Architecture chain updated; dependency map adds `bmad-agent-analyst` and `bmad-cis`; ADR 7 added; parity test categories expanded with three new rows |
| `sprint-plan.md` | PP-1.2, PP-1.3, PP-2.2 AC updated |

### Technical Impact
- No new shared utilities required.
- No changes to lifecycle.yaml, feature.yaml schema, or branch topology.
- No changes to batch mode, review-ready, adversarial review, or phase completion paths.
- `bmad-lens-bmad-skill` already supports arbitrary BMAD skill delegation; no wrapper changes needed.

---

## Section 3: Recommended Approach

**Option 1 — Direct Adjustment** ✅ Selected

Modify planning artifact AC and descriptions within the existing story structure. No new stories, no rollback, no MVP scope change.

**Rationale:**
- The feature is in `expressplan-complete` (planning phase only) — no implementation has been written, so there is nothing to roll back.
- The change is additive within Sprint 2: PP-2.2 gains an analyst activation step and a mode-choice branch. Complexity classification remains M.
- Both brainstorm modes (`bmad-brainstorming` and `bmad-cis`) route through the existing `bmad-lens-bmad-skill` delegation mechanism, so no new architectural primitives are introduced.

**Effort:** Medium
**Risk:** Low — analyst activation is a prepend step; mode choice is a new branch but both paths produce `brainstorm.md`, preserving the ordering invariant.
**Timeline impact:** PP-2.2 effort estimate unchanged (M); PP-1.3 gains three parity test skeletons (minor addition within S estimate).

---

## Section 4: Detailed Change Proposals

### business-plan.md — Success Criteria

**Story:** Success criterion "Brainstorm-first ordering"

OLD:
```
| Brainstorm-first ordering | Non-batch interactive runs always start with BMAD brainstorming setup questions before any research or product-brief authoring begins; a brainstorm.md must exist before downstream synthesis is invoked |
```

NEW:
```
| Brainstorm-first ordering | Non-batch interactive runs always activate `bmad-agent-analyst` first to frame requirements context; the conductor then presents the user with a choice of brainstorm mode (`bmad-brainstorming` or `bmad-cis`) before any research or product-brief authoring begins; a brainstorm.md must exist before downstream synthesis is invoked |
```

**Rationale:** Analyst framing grounds the brainstorm session; mode choice gives users structured vs. divergent options.

---

### business-plan.md — In Scope

**Section:** In Scope

OLD:
```
- Wire brainstorming, research, and product-brief authoring through `bmad-lens-bmad-skill`.
```

NEW:
```
- Activate `bmad-agent-analyst` at the start of the interactive flow to frame requirements context before any brainstorm mode is selected.
- Wire the brainstorm mode choice — present the user with options for `bmad-brainstorming` (divergent ideation) and `bmad-cis` (structured creative innovation) — through `bmad-lens-bmad-skill` after analyst framing completes.
- Wire research and product-brief authoring through `bmad-lens-bmad-skill`.
```

**Rationale:** Three distinct delegation steps replace the previous single-line entry.

---

### tech-plan.md — Architecture Chain

OLD:
```
      -> bmad-lens-bmad-skill (bmad-brainstorming)  (brainstorm authoring)
```

NEW:
```
      -> bmad-agent-analyst                         (requirements framing before brainstorm mode selection)
      -> bmad-lens-bmad-skill (bmad-brainstorming | bmad-cis)  (brainstorm authoring — user-selected mode)
```

---

### tech-plan.md — ADR 7 (new)

Added after ADR 2 alternatives:

```
### ADR 7 — Analyst Activation and Brainstorm Mode Choice

Decision: Before invoking any brainstorm authoring wrapper, the conductor activates
`bmad-agent-analyst` to frame requirements context. After analyst framing, the conductor
presents a choice between `bmad-brainstorming` and `bmad-cis`. Both route through
`bmad-lens-bmad-skill`. The brainstorm.md existence gate applies regardless of mode.

Rationale: Analyst framing prevents brainstorming from starting without grounded context,
reducing rework in research and product-brief steps. Offering `bmad-cis` serves users
who prefer structured innovation over open divergent ideation; both paths produce a
brainstorm.md that satisfies the preplan ordering contract.
```

---

### tech-plan.md — Parity Test Categories (3 new rows)

Added before the existing brainstorm-first ordering row:

| Analyst activation ordering | `bmad-agent-analyst` is invoked before any brainstorm mode wrapper | Analyst-skip regression |
| Brainstorm mode choice — bmad-brainstorming | `bmad-lens-bmad-skill` called with `bmad-brainstorming`; `bmad-cis` not invoked | Mode routing error |
| Brainstorm mode choice — bmad-cis | `bmad-lens-bmad-skill` called with `bmad-cis`; `bmad-brainstorming` not invoked | Mode routing error |

---

### sprint-plan.md — PP-1.2

OLD:
```
SKILL.md documents brainstorm-first ordering, batch delegation, review-ready delegation,
no-governance-write invariant, phase completion gate, and all integration points
```

NEW:
```
SKILL.md documents analyst activation before brainstorm mode selection, user choice between
`bmad-brainstorming` and `bmad-cis` modes, brainstorm-first ordering, batch delegation,
review-ready delegation, no-governance-write invariant, phase completion gate, and all
integration points
```

---

### sprint-plan.md — PP-1.3

OLD:
```
Tests fail red for brainstorm-first ordering, batch pass 1 stop, batch pass 2 resume,
review-ready delegation, phase gate on fail, phase gate on pass, and no-governance-write invariant
```

NEW:
```
Tests fail red for analyst activation ordering, brainstorm mode choice (bmad-brainstorming path),
brainstorm mode choice (bmad-cis path), brainstorm-first ordering, batch pass 1 stop, batch pass 2
resume, review-ready delegation, phase gate on fail, phase gate on pass, and no-governance-write invariant
```

---

### sprint-plan.md — PP-2.2

OLD:
```
| PP-2.2 | Implement brainstorm-first interactive flow | M |
Interactive mode asks brainstorming setup questions before any authoring wrapper is invoked;
brainstorm.md must exist before research or product-brief wrappers are offered;
invokes `bmad-lens-bmad-skill` with `bmad-brainstorming` as first authoring step |
Ordering invariant must be tested, not assumed |
```

NEW:
```
| PP-2.2 | Implement analyst activation and brainstorm mode selection | M |
Interactive mode activates `bmad-agent-analyst` to frame requirements context before any authoring
wrapper is invoked; after analyst framing, the conductor presents the user with a choice between
`bmad-brainstorming` (divergent ideation) and `bmad-cis` (structured innovation); selected mode
routes through `bmad-lens-bmad-skill`; brainstorm.md must exist regardless of mode before research
or product-brief wrappers are offered |
Both brainstorm modes must be tested; ordering invariant must be tested, not assumed |
```

---

## Section 5: Implementation Handoff

**Scope classification:** Moderate — backlog AC update within existing story structure.

**Handoff:** Developer

| Recipient | Responsibility |
|---|---|
| Developer (PP-2.2) | Implement `bmad-agent-analyst` activation step; add mode-choice branch routing to `bmad-brainstorming` or `bmad-cis`; confirm `brainstorm.md` gate applies to both paths |
| Developer (PP-1.2) | Update SKILL.md contract to document analyst + mode-choice flow |
| Developer (PP-1.3) | Add three new failing test skeletons: analyst ordering, brainstorm mode (bmad-brainstorming), brainstorm mode (bmad-cis) |

**Success criteria for implementation:**
- `bmad-agent-analyst` is invoked on every non-batch interactive preplan run before any brainstorm mode wrapper is called.
- User is offered both `bmad-brainstorming` and `bmad-cis` options; selection routes correctly through `bmad-lens-bmad-skill`.
- `brainstorm.md` existence gate applies regardless of which mode was selected.
- All three new parity test skeletons pass green after PP-2.2 implementation.
- No regression in existing parity test categories.
