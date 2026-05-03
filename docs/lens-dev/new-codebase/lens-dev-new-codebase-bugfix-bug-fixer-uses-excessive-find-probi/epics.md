---
feature: lens-dev-new-codebase-bugfix-bug-fixer-uses-excessive-find-probi
doc_type: epics
status: approved
track: express
domain: lens-dev
service: new-codebase
depends_on: []
blocks: []
key_decisions:
  - Two epics: one for SKILL.md conductor fix, one for script regression guard
  - Story S1 targets the source-writable SKILL.md path only
  - Story S2 adds a regression test and commits the already-applied truncation fix
open_questions: []
updated_at: '2026-05-03T19:15:00Z'
stepsCompleted: [epics-and-stories]
---

# Epics — Bug Fixer Excessive Find Probing

## Epic 1: Fix Bug-Fixer Conductor Path Ambiguity

**Goal:** Eliminate exploratory `find`/`cat` commands and double preflight invocations from
the `/lens-bug-fixer` conductor by providing an explicit, unambiguous invocation path in
`bmad-lens-bug-fixer/SKILL.md`.

**Value:** Reduces execution token waste, makes the conductor behaviour deterministic, and
prevents incorrect working-directory preflight failures.

---

### Story 1.1 — Fix SKILL.md preflight path and working directory

**As a** Lens workbench user  
**I want** `/lens-bug-fixer` to invoke preflight exactly once from the correct working directory  
**So that** no exploratory `find` commands run and the execution trace is clean

**Acceptance Criteria:**

- [ ] Edit `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-bug-fixer/SKILL.md` (NOT `lens.core/`)
- [ ] "On Activation" step 1 specifies the exact CWD: `{control_repo}/TargetProjects/lens-dev/new-codebase/lens.core.src`
- [ ] "On Activation" step 1 specifies the exact relative script path: `_bmad/lens-work/lens-preflight/scripts/light-preflight.py`
- [ ] No `find` or `cat` probing commands appear before the preflight invocation in a fresh run
- [ ] Preflight is invoked at most once in a successful `--fix-all-new` run
- [ ] Preflight must be invoked from `{control_repo}/TargetProjects/lens-dev/new-codebase/lens.core.src`

**Story Points:** 2  
**Dependencies:** None

---

## Epic 2: Script Quality — Stub Truncation Regression Guard

**Goal:** Ensure the `derive-feature-id` stub truncation fix is committed to the source
branch and protected by a regression test so it cannot silently regress.

**Value:** Prevents `init-feature-ops.py` from rejecting feature IDs with a 64-char length
violation in future bugbash runs.

---

### Story 2.1 — Commit and test derive-feature-id stub truncation fix

**As a** maintainer  
**I want** `bug-fixer-ops.py derive-feature-id` to have a regression test for long slugs  
**So that** the 64-char limit is automatically enforced and does not regress

**Acceptance Criteria:**

- [ ] The stub truncation fix (`max_stub_len = 64 - len(FEATURE_ID_PREFIX)`) is committed to the `lens.core.src` feature branch `lens-dev-new-codebase-bugfix-bug-fixer-uses-excessive-find-probi`
- [ ] A test exists in `_bmad/lens-work/scripts/tests/` (or equivalent) that passes a slug longer than 35 chars to `derive_feature_id_from_slugs`
- [ ] The test asserts `len(feature_id) <= 64`
- [ ] The test passes with the current code
- [ ] `MAX_STUB_LEN` is optionally extracted as a named constant with a comment referencing `SAFE_ID_PATTERN` in `init-feature-ops.py` (nice-to-have)

**Story Points:** 1  
**Dependencies:** None
