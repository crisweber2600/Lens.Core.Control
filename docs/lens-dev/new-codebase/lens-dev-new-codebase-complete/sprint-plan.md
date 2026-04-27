---
feature: lens-dev-new-codebase-complete
doc_type: sprint-plan
status: draft
goal: "Sequence clean-room /complete parity work into testable closure slices that preserve retrospective, documentation, and archive semantics"
key_decisions:
  - Keep complete as a dedicated closure workflow rather than folding it into dev or finalizeplan.
  - Start with command-level regression coverage for preconditions, archive-state queries, and irreversible finalize behavior.
  - Treat summary.md as the canonical archive summary artifact and audit older final-summary references during implementation.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T21:36:55Z
---

# Sprint Plan - Complete Command

## Sprint Goal

Restore `/complete` as a retained Lens closure command in the new codebase, with clean-room observable parity to the current release-module behavior and no lifecycle or governance schema changes.

## Sprint 1 - Contract and Risk Lock

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| CP-1: Define complete CLI contract tests | Focused tests cover `check-preconditions`, `finalize`, `archive-status`, and dry-run behavior | 3 | business-plan.md, tech-plan.md |
| CP-2: Assert archive boundary | Tests prove complete updates archive records but does not invent new lifecycle files or schema fields | 2 | CP-1 |
| CP-3: Add prompt and help surface checks | Static checks or tests confirm `/complete` remains discoverable across prompt/help surfaces | 2 | CP-1 |

## Sprint 2 - Script Fidelity

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| CP-4: Preserve completable-phase checks | `check-preconditions` only allows `dev` and `complete`, with clear blocker output for planning phases | 2 | CP-1 |
| CP-5: Preserve atomic archive writes | `finalize` updates `feature.yaml`, `feature-index.yaml`, and `summary.md` as one consistent archive change set | 3 | CP-4 |
| CP-6: Preserve archive-status query | Read-only status checks keep terminal-state recognition stable for downstream readers | 1 | CP-4 |
| CP-7: Keep summary artifact canonical | Implementation and tests align on `summary.md`, while older `final-summary` references are audited and reconciled | 2 | CP-5 |

## Sprint 3 - Workflow Orchestration Parity

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| CP-8: Preserve retrospective-first orchestration | Skill flow runs or explicitly skips retrospective before finalize | 2 | CP-5 |
| CP-9: Preserve document-before-archive ordering | `bmad-lens-document-project` stays ahead of irreversible archive state changes | 2 | CP-8 |
| CP-10: Preserve explicit confirmation gate | Wrapper flow requires affirmative confirmation before finalize executes | 2 | CP-8 |

## Sprint 4 - Verification and Handoff

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| CP-11: Run focused complete regressions | The `complete-ops` focused test file passes from the source tree root | 2 | CP-7, CP-10 |
| CP-12: Run adjacent command checks | Narrow checks confirm archive-state readers and prompt routing still recognize completed features | 2 | CP-11 |
| CP-13: Prepare implementation handoff notes | Dev handoff names files, tests, non-goals, and known archive-risk seams | 1 | CP-12 |

## Dependencies and Sequencing

- `CP-1` through `CP-3` lock the observable contract before code changes widen.
- `CP-4` through `CP-7` stabilize the script layer first because it owns the archive mutation boundary.
- `CP-8` through `CP-10` verify the higher-level orchestration that users actually experience.
- `CP-11` through `CP-13` prove the feature is ready to move into implementation with bounded review scope.

## Risks To Track During Execution

| Risk | Mitigation |
|---|---|
| Reference docs disagree about `summary.md` vs `final-summary.md` | Keep CP-7 explicit and treat filename alignment as a test-backed decision |
| Branch cleanup behavior drifts between docs and script | Capture the intended boundary in implementation notes before expanding scope |
| Retrospective skip becomes silent behavior | Keep confirmation and archive-summary skip recording under focused tests |
| Archive writes succeed partially under failure paths | Preserve atomic file-write behavior and fail loudly in regression coverage |

## Definition of Done

- Focused complete-command regressions pass.
- Narrow archive-state reader checks pass.
- `/complete` remains present in retained prompt/help surfaces.
- No lifecycle, feature, or feature-index schema changes are introduced.
- The implementation handoff can proceed without unresolved blockers on archive semantics.