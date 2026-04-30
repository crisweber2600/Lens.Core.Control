---
feature: lens-dev-new-codebase-next
doc_type: finalizeplan-review
phase: finalizeplan
source: manual-rerun
verdict: pass-with-warnings
status: responses-recorded
goal: "Final cross-artifact review of the Next command express planning packet before downstream bundle execution."
critical_count: 0
high_count: 1
medium_count: 2
low_count: 1
carry_forward_blockers: []
key_decisions:
  - Carry forward ExpressPlan H2 (constitution resolver express-track filtering) as a named implementation dependency before automated express validation is declared usable.
  - Paused-state route decision must be closed before Slice 4 exit criteria can be met.
  - Target-project file overlap with lens-dev-new-codebase-trueup must be declared or resolved before Next dev begins.
open_questions:
  - Which test file becomes the canonical next-ops.py parity suite?
  - Will the constitution resolver fix land in this feature or remain a prerequisite from lens-dev-new-codebase-constitution?
depends_on:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-next/business-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-next/tech-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-next/sprint-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-next/expressplan-adversarial-review.md
blocks: []
updated_at: 2026-04-30T22:15:00Z
review_format: abc-choice-v1
---

# FinalizePlan Review — lens-dev-new-codebase-next

**Phase:** finalizeplan  
**Source:** manual-rerun  
**Artifacts reviewed:** business-plan.md, tech-plan.md, sprint-plan.md, expressplan-adversarial-review.md  
**Verdict:** `pass-with-warnings`

---

## Summary

The Next command express planning packet is coherent and implementation-ready. The business plan is well-scoped: `next` is defined as a read-only routing conductor with fixture-backed output parity, blocker-first behavior, and pre-confirmed delegation. The tech plan correctly separates the prompt chain, the orchestration skill, and the routing script into auditable surfaces. The sprint plan sequences slices with sensible exit criteria.

The expressplan adversarial review recorded responses for all four findings (H1, H2, M1, M2) with verdict `pass-with-warnings`. All response choices are acceptance-level (A), meaning the planning packet absorbs the risks and commits to implementation gates rather than redesigning.

Three risks require governance-impact follow-through before dev completion. None block FinalizePlan from completing — they are dev-readiness gates recorded here.

---

## Response Record

| Option | Meaning |
|--------|---------|
| A / B / C | Accept the proposed resolution with its trade-offs |
| D | Provide a custom response after `D:` |
| E | Explicitly accept the finding with no action |

---

## Finding Summary

| ID | Severity | Title | Recorded Response |
|----|----------|-------|-------------------|
| H1 | High | Constitution resolver dependency has no assigned owner | A |
| M1 | Medium | Paused-state decision remains open at planning close | A |
| M2 | Medium | Shared target-project surface overlap not declared | C |
| L1 | Low | next-ops.py test file is undecided | A |

---

## Governance Cross-Check

Running before the review findings to capture impacted services and related feature docs.

### Active features that may interact with this implementation

| Feature | Phase | Relevance |
|---------|-------|-----------|
| `lens-dev-new-codebase-constitution` | `preplan` | Owns the constitution resolver express-track fix named in H2 of the expressplan review |
| `lens-dev-new-codebase-trueup` | `dev` | Currently writing to the same target project; touches discovery surface overlap risk |
| `lens-dev-new-codebase-dev` | `expressplan-complete` | At the same planning stage; no direct conflict, but both touch the target implementation surface |

**Action items from governance cross-check:**  
1. Declare a dependency on `lens-dev-new-codebase-constitution` or confirm the resolver fix will land in Next's implementation scope.  
2. Check if `lens-dev-new-codebase-trueup` is writing to `module-help.csv`, `module.yaml`, or agent discovery surfaces in the same target project before starting Slice 2 of Next.

---

## High Findings

### H1 — Constitution Resolver Dependency Has No Assigned Owner

**Dimension:** Cross-feature dependencies  
**Gate:** Before automated express validation is declared usable (Slice 4 exit)

The expressplan review H2 finding stated that the constitution resolver currently filters express tracks, causing express features to look invalid to automation even after governance state is aligned. Response A was recorded: "name resolver express-track support as a dependency or scoped fix, with no local constitution fork."

`lens-dev-new-codebase-constitution` is at `preplan` — no implementation is scheduled. The tech plan says "implementation should either land resolver allow-list support before automated expressplan validation, or explicitly depend on the constitution work package." Neither path has a concrete owner or tracked dependency in `feature.yaml`.

This is the most significant carry-forward from ExpressPlan. Without a resolution, Slice 4 exit criteria ("the express-track allow-list issue has an owner and regression expectation") cannot be met.

**Choose one:**

- **A.** Record `lens-dev-new-codebase-constitution` as a formal dependency in `feature.yaml` and require it to reach at least `expressplan-complete` before Next declares Slice 4 done.  
  **Why pick this:** Governance relationship is traceable and the dependency gate is explicit.  
  **Why not:** May delay Next's dev-complete if constitution work stalls.
- **B.** Scope the resolver express-track fix into Next's Slice 4 directly, without depending on the constitution feature.  
  **Why pick this:** Keeps Next self-contained and unblocked.  
  **Why not:** Duplicates work that the constitution feature should own.
- **C.** Mark the constitution resolver gap as informational, require a regression fixture demonstrating the failure, and treat it as a known limitation.  
  **Why pick this:** Keeps the feature unblocked while the limitation is documented.  
  **Why not:** Leaves automated express validation in a degraded state.
- **D.** Write your own response.
- **E.** Explicitly accept with no action.

**Recorded response:** A  
**Applied adjustment:** `lens-dev-new-codebase-constitution` is recorded as a formal dependency. The Slice 4 story file must confirm that the constitution feature has reached at least `expressplan-complete` before Next's Slice 4 can close. The dependency will be added to `feature.yaml` before dev begins.

---

## Medium Findings

### M1 — Paused-State Decision Remains Open at Planning Close

**Dimension:** Logic flaws / completeness  
**Gate:** Before Slice 4 exit criteria ("retained recovery behavior selected")

The expressplan H1 finding required "an explicit paused-state decision before release readiness." The sprint plan names Slice 4 as the gate and lists three acceptable implementation outcomes (internal skill route, blocker message, or new retained public command). The open question "Which retained recovery behavior should handle paused features?" is still unresolved in the planning packet.

At finalizeplan close, this decision is still only a named gate — not a selected option. The dev team will need to select one before Slice 4 can close.

**Choose one:**

- **A.** Carry forward as a Slice 4 entry decision; require the implementing agent to document the choice in a story file before closing Slice 4.  
  **Why pick this:** The decision doesn't change the planning architecture — it's an implementation choice.  
  **Why not:** Leaves an open question that could be resolved earlier with less risk.
- **B.** Select "report paused state as a blocker with instructions for the retained recovery path" now, locking the behavior in the tech plan before dev starts.  
  **Why pick this:** Simplest option; avoids adding an implicit dependency on any internal skill.  
  **Why not:** Removes flexibility for the implementing agent to choose a richer recovery path.
- **C.** Add a story file decision gate at Slice 3 (before routing engine work starts) so the test fixtures include the paused-state case.  
  **Why pick this:** Fixtures written against the selected behavior are more complete.  
  **Why not:** Adds a checkpoint before Slice 3 is otherwise blocked.
- **D.** Write your own response.
- **E.** Explicitly accept with no action.

**Recorded response:** A  
**Applied adjustment:** The paused-state route decision is carried forward as a Slice 4 entry gate. The Slice 4 story file must document the selected behavior (internal skill route, blocker message, or retained public command) before any Slice 4 implementation work begins.

### M2 — Shared Target-Project Surface Overlap Not Declared

**Dimension:** Cross-feature dependencies  
**Gate:** Before Slice 2 (discovery wiring)

`lens-dev-new-codebase-trueup` is currently in `dev` and writing to the same target project (`lens.core.src`). Next's Slice 2 touches `module-help.csv`, `module.yaml`, `lens.agent.md`, and the installer manifest. If trueup is modifying the same discovery surfaces, concurrent writes could produce merge conflicts or silent overrides.

No overlap check was performed during expressplan; this is a governance sensing finding added at finalizeplan.

**Choose one:**

- **A.** Before starting Slice 2, check which discovery surfaces `lens-dev-new-codebase-trueup` is modifying and declare a sequencing constraint if overlap exists.  
  **Why pick this:** Avoids merge conflicts without blocking either feature.  
  **Why not:** Adds a discovery step at the start of dev.
- **B.** Treat the discovery surface as append-only and depend on post-dev trueup to reconcile any surface overlap.  
  **Why pick this:** Keeps Next moving without a cross-feature gate.  
  **Why not:** Silent surface inconsistencies can persist until trueup runs.
- **C.** Coordinate with the trueup dev agent to serialize discovery-surface writes — Next appends after trueup completes its discovery surface work.  
  **Why pick this:** Eliminates the conflict risk entirely.  
  **Why not:** May require waiting on trueup's progress.
- **D.** Write your own response.
- **E.** Explicitly accept with no action.

**Recorded response:** C  
**Applied adjustment:** Discovery-surface writes are serialized with the trueup dev agent. Next's Slice 2 begins only after `lens-dev-new-codebase-trueup` confirms its discovery-surface work is complete. The Slice 2 story file must include a precondition check confirming trueup's discovery writes are done before Next appends its entries.

---

## Low Findings

### L1 — next-ops.py Test File Target Is Undecided

**Dimension:** Completeness  
**Gate:** Informational — Slice 3 entry

The sprint plan open question "Which test file should become the canonical next-ops parity suite?" is unresolved. The test path is needed to close Slice 3 exit criteria ("parity suite is in place"). This is not a blocking risk — the implementing agent can select the path at the start of Slice 3 — but leaving it unresolved could cause the story file to be underspecified.

**Choose one:**

- **A.** Defer to the implementing agent: require the Slice 3 story file to name the test file before work begins.  
  **Why pick this:** The implementing agent has better visibility into the target project structure.  
  **Why not:** Adds ambiguity to the story file.
- **B.** Use `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-next/scripts/tests/` as the canonical parity suite path, consistent with the tech plan target surface.  
  **Why pick this:** Resolves the question now; consistent with the tech plan table.  
  **Why not:** The path may not exist yet.
- **C.** Use a single test file at `tests/test_next_ops.py` alongside the script for simplicity.  
  **Why pick this:** Minimal test footprint.  
  **Why not:** Does not match the tech plan's co-located test directory convention.
- **D.** Write your own response.
- **E.** Explicitly accept with no action.

**Recorded response:** A  
**Applied adjustment:** The Slice 3 story file must name the canonical test file path before implementation begins. The implementing agent selects from the options above and documents the choice as a story entry condition.

---

## Party-Mode Blind Spot Challenge

The following blind spots were raised in the party-mode round and are recorded here for completeness. No fail-level issues were identified.

### Blind Spot 1 — The `next` skill's no-write boundary is asserted but not tested

The business and tech plans state that `next` must produce no governance or control-doc writes, and the sprint plan lists "verify no-write behavior" as a Slice 4 exit criterion. There is no story file or fixture to validate this. In other retained commands that have both a read and a write path (e.g., `finalizeplan`), the no-write contract has drifted during implementation. A specific negative test should be added to the Slice 4 story file.

**Status:** Noted — no separate finding; folded into Slice 4 story guidance.

### Blind Spot 2 — Express routing test cases depend on `lifecycle.yaml` but no test loads the actual file

The tech plan requires `next-ops.py` to read `lifecycle.yaml` for phase definitions and auto-advance commands. The parity fixtures will be invalid if they stub lifecycle.yaml contents instead of loading the actual file. A real fixture must load the installed lifecycle.yaml to confirm routing against the live schema.

**Status:** Noted — add to Slice 3 story acceptance criteria.

### Blind Spot 3 — Discovery surface wiring is in Slice 2 but the discovery format may change if trueup modifies it

`lens-dev-new-codebase-trueup` is in dev. If trueup changes how the discovery surface is structured (e.g., adds a new manifest format), Next's Slice 2 wiring could be immediately stale. This overlaps with M2 above.

**Status:** Covered by M2.

---

## Verdict

`pass-with-warnings`

The planning packet is complete and implementation-ready. No fail-level findings were raised. Three risks require active management during dev:

1. The constitution resolver dependency needs a concrete owner before Slice 4 closes (H1).
2. The paused-state route decision should be locked by Slice 3 entry (M1).
3. Target-project surface overlap with trueup should be checked before Slice 2 begins (M2).

These are dev-readiness gates, not planning gaps. FinalizePlan is complete.
