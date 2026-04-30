---
feature: lens-dev-new-codebase-complete
doc_type: epics
status: draft
goal: "Break /complete parity work into implementation epics that preserve precondition gating, retrospective-before-archive ordering, atomic archive writes, and terminal-state recognition"
key_decisions:
  - Organize work around the contract lock, the archive mutation layer, and the orchestration layer.
  - Keep archive semantics clean-room: no schema changes, no new lifecycle files.
  - CP-7 (summary naming audit) is a gate, not cleanup — must pass before any archive-write story is considered done.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T22:55:00Z
---

# Epics - Complete Command

## Epic 1 - Contract and Risk Lock

**Goal:** Establish the observable CLI contract and regression harness before any archive mutations widen.

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| CP-1: Define complete CLI contract tests | Focused tests cover `check-preconditions`, `finalize`, `archive-status`, and dry-run behavior | 3 | None |
| CP-2: Assert archive boundary | Tests prove complete updates archive records but does not invent new lifecycle files or schema fields | 2 | CP-1 |
| CP-3: Add prompt and help surface checks | Static checks or tests confirm `/complete` remains discoverable across prompt/help surfaces | 2 | CP-1 |

**Definition of Done**

- `test-complete-ops.py` (or equivalent focused harness) exists and passes.
- Archive boundary assertions prevent scope creep in later stories.
- `/complete` is discoverable from the expected help and prompt surfaces.

---

## Epic 2 - Script Fidelity

**Goal:** Restore the three-subcommand archive script with atomic write semantics and clean terminal-state output.

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| CP-4: Preserve completable-phase checks | `check-preconditions` only allows `dev` and `dev-complete`, with clear blocker output for planning phases | 2 | CP-1 |
| CP-5: Preserve atomic archive writes | `finalize` updates `feature.yaml`, `feature-index.yaml`, and `summary.md` as one consistent change set | 3 | CP-4 |
| CP-6: Preserve archive-status query | Read-only status checks keep terminal-state recognition stable for downstream readers | 1 | CP-4 |
| CP-7: Lock summary artifact naming | Implementation and tests align on `summary.md`; audit older `final-summary` references and remove or reconcile them before CP-5 is accepted | 2 | CP-5 |

**Definition of Done**

- `check-preconditions` blocks planning-phase features and warns on missing retrospective.
- `finalize` writes all three archive files as a consistent set and never partially commits.
- `archive-status` reads terminal state without modifying any governance file.
- CP-7 audit passes: no remaining `final-summary.md` references in scripts, tests, or help text.

---

## Epic 3 - Workflow Orchestration Parity

**Goal:** Restore the conductor skill with correct retrospective-first ordering and a real confirmation gate.

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| CP-8: Preserve retrospective-first orchestration | Skill flow runs or explicitly skips retrospective before finalize | 2 | CP-5 |
| CP-9: Preserve document-before-archive ordering | `bmad-lens-document-project` stays ahead of irreversible archive state changes | 2 | CP-8 |
| CP-10: Preserve explicit confirmation gate | Wrapper flow requires affirmative confirmation before finalize executes; done condition: `test-complete-ops.py::test_finalize_requires_confirmation` passes | 2 | CP-8 |

**Definition of Done**

- The skill sequence is `check-preconditions → confirm → retrospective → document-project → finalize`.
- Skipping retrospective requires explicit confirmation and is recorded in the archive summary.
- `test-complete-ops.py::test_finalize_requires_confirmation` passes.
- Branch cleanup is NOT part of the script contract — it is documented as post-archive operational follow-up only.

---

## Epic 4 - Verification and Handoff

**Goal:** Prove the full closure path with focused regressions and produce a clean implementation handoff.

| Story | Outcome | Estimate | Dependencies |
|---|---|---:|---|
| CP-11: Run focused complete regressions | `test-complete-ops.py` passes from the source tree root | 2 | CP-7, CP-10 |
| CP-12: Run adjacent command checks | Narrow checks confirm archive-state readers and prompt routing still recognize completed features | 2 | CP-11 |
| CP-13: Prepare implementation handoff notes | Dev handoff names files, tests, non-goals, and known archive-risk seams; references `new-service` and `switch` as behavioral examples | 1 | CP-12 |

**Definition of Done**

- Focused regression suite passes end-to-end from the source tree root.
- Prompt routing and archive-state readers confirmed working for completed features.
- Handoff document explicitly states branch cleanup is out of scope for the archive script.
