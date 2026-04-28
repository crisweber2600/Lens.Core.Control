---
feature: lens-dev-new-codebase-complete
doc_type: stories
status: draft
goal: "Define implementation-ready stories for restoring /complete parity in the new codebase with clean-room archive semantics"
key_decisions:
  - Stories stay aligned to the four retained complete seams: contract lock, archive script, orchestration, and verification.
  - Acceptance criteria stay observable at the command boundary or script CLI level.
  - CP-7 summary-naming audit is a gate for CP-5 acceptance — not a post-completion cleanup task.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T22:55:00Z
---

# Stories - Complete Command

## Story CP-1 - Define Complete CLI Contract Tests

**User story:** As a maintainer, I want a focused regression harness locked before implementation so the archive contract cannot silently change.

**Acceptance criteria**

- `test-complete-ops.py` (or equivalent) exists with test stubs for `check-preconditions`, `finalize`, `archive-status`, and dry-run modes.
- All test stubs are either passing (with mock fixtures) or clearly marked as expected-to-fail red-phase until implementation lands.
- The test file is runnable from the source tree root via `uv run --with pytest pytest <path> -q`.

---

## Story CP-2 - Assert Archive Boundary

**User story:** As a governance owner, I want the test suite to prove complete does not invent new lifecycle schemas so future contributions cannot widen the archive mutation surface.

**Acceptance criteria**

- Tests assert that `finalize` only writes `feature.yaml`, `feature-index.yaml`, and `summary.md` — no new files or new YAML keys.
- Tests assert no new entries in the governance constitutions or lifecycle YAML.
- These boundary assertions run as part of the normal focused test pass.

---

## Story CP-3 - Prompt and Help Surface Checks

**User story:** As a Lens operator, I want `/complete` to remain discoverable in help surfaces so I can find the closure command without reading source code.

**Acceptance criteria**

- `/complete` appears in the expected prompt stub and module help surface after implementation.
- A static check or test confirms the prompt stub exists and delegates to the release path.
- The help surface entry does not describe deprecated behavior (no `final-summary.md` references).

---

## Story CP-4 - Preserve Completable-Phase Checks

**User story:** As a feature author, I want `check-preconditions` to tell me clearly if my feature is not ready to close so I never archive work that is still in planning.

**Acceptance criteria**

- `check-preconditions` returns `fail` for features in planning phases (`preplan`, `businessplan`, `techplan`, `finalizeplan`, `expressplan`).
- `check-preconditions` returns `warn` for features missing a retrospective record and `pass` when all conditions are met.
- `check-preconditions` returns `fail` for features already in `complete` or `abandoned` phase.
- A dry-run mode surfaces all precondition issues before any write happens.

---

## Story CP-5 - Preserve Atomic Archive Writes

**User story:** As a governance owner, I want `finalize` to write the archive record atomically so a partial failure cannot leave the registry in an inconsistent state.

**Acceptance criteria**

- `finalize` updates `feature.yaml` (phase: complete), `feature-index.yaml` (status: archived), and `summary.md` as a consistent set.
- If any write fails mid-sequence, the other files are not left in a partially updated state (or the failure state is clearly documented and surfaced).
- CP-7 audit must pass before CP-5 is accepted: no `final-summary.md` references remain in script, test, or help surfaces.
- `finalize` does NOT delete branches or perform any Git topology operations.

---

## Story CP-6 - Preserve Archive-Status Query

**User story:** As a downstream command owner, I want `archive-status` to let me check whether a feature is archived without mutating anything so readers can stay safe.

**Acceptance criteria**

- `archive-status` reads `feature.yaml` and `feature-index.yaml` and returns a structured result.
- `archive-status` makes no writes to any file.
- Terminal state (`phase: complete`, `status: archived`) is recognized correctly.
- Non-terminal features return a clear non-archived result rather than an error.

---

## Story CP-7 - Lock Summary Artifact Naming

**User story:** As a maintainer, I want the summary artifact name to be unambiguous so every reader, test, and help text refers to the same file.

**Acceptance criteria**

- No references to `final-summary.md` remain in scripts, tests, help text, or prompt surfaces after this story lands.
- `summary.md` is the only name used in `complete-ops.py`, `test-complete-ops.py`, and any help output.
- If any `final-summary` reference is found in adjacent features' docs that depend on `complete`, it is flagged as a follow-on task (not silently removed).
- **Gate:** CP-5 acceptance criteria cannot be signed off until this story passes.

---

## Story CP-8 - Preserve Retrospective-First Orchestration

**User story:** As a feature author, I want the conductor skill to run retrospective before archive so I cannot accidentally archive a feature with no closure narrative.

**Acceptance criteria**

- The skill flow is: `check-preconditions → confirm → retrospective → document-project → finalize`.
- If the user explicitly skips retrospective, the skill requires a separate confirmation for the skip.
- The archive summary records whether retrospective was completed or explicitly skipped.

---

## Story CP-9 - Preserve Document-Before-Archive Ordering

**User story:** As a documentation consumer, I want final project docs captured before archive state changes so the historical record is never partially written.

**Acceptance criteria**

- `bmad-lens-document-project` completes (or is explicitly skipped) before `finalize` is called.
- If `document-project` fails or is skipped, the user is prompted before irreversible archive execution continues.
- The ordering is reflected in the skill's decision tree, not just described in documentation.

---

## Story CP-10 - Preserve Explicit Confirmation Gate

**User story:** As a feature author, I want an explicit confirmation before `/complete` archives my feature irreversibly so I cannot trigger archival by accident.

**Acceptance criteria**

- The skill presents a clear irreversibility warning before calling `finalize`.
- The user must provide affirmative confirmation to proceed.
- `finalize` is never called without that confirmation step being executed.
- **Done condition:** `test-complete-ops.py::test_finalize_requires_confirmation` passes.

---

## Story CP-11 - Run Focused Complete Regressions

**User story:** As a maintainer, I want the focused test suite to run cleanly from the source tree root so CI can gate on archive behavior.

**Acceptance criteria**

- `uv run --with pytest pytest <path>/test-complete-ops.py -q` passes from the source tree root.
- All archive boundary, phase-check, atomic-write, and confirmation-gate tests pass.
- No test relies on real governance repo state — all tests use fixture data.

---

## Story CP-12 - Run Adjacent Command Checks

**User story:** As a downstream command owner, I want narrow checks that confirm archive-state readers recognize completed features so `/switch`, `/next`, and dashboard views stay correct.

**Acceptance criteria**

- A narrow check confirms `archive-status` returns a recognized terminal state for at least one fixture representing a `complete`-phase feature.
- Prompt routing confirms `/complete` resolves to `bmad-lens-complete` without ambiguity.
- No adjacent command returns a misleading result for an archived feature.

---

## Story CP-13 - Prepare Implementation Handoff Notes

**User story:** As a developer picking up this feature, I want a handoff document that names the files, tests, non-goals, and known risk seams so I can start implementation without re-reading all planning docs.

**Acceptance criteria**

- The handoff document names: `complete-ops.py`, `test-complete-ops.py`, `bmad-lens-complete/SKILL.md`, the three archive output files, and the three script subcommands.
- Explicitly states: branch cleanup is NOT part of the `/complete` script contract.
- References `lens-dev-new-codebase-new-service` and `lens-dev-new-codebase-switch` as behavioral examples of archived express features.
- References `lens-dev-new-codebase-baseline` as the depends-on feature providing lifecycle graph and schema definitions.

## Dependency Summary

- CP-1 through CP-3 lock the observable contract before archive mutations widen.
- CP-4 through CP-7 stabilize the script layer first because it owns archive mutation boundaries.
- CP-8 through CP-10 verify higher-level orchestration that users experience.
- CP-11 through CP-13 prove end-to-end and hand off cleanly to `/dev`.
