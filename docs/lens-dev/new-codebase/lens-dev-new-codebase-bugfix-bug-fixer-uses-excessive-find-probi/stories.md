---
feature: lens-dev-new-codebase-bugfix-bug-fixer-uses-excessive-find-probi
doc_type: stories
status: approved
track: express
domain: lens-dev
service: new-codebase
depends_on: []
blocks: []
key_decisions:
  - Story 1.1 targets the writable SKILL.md path only
  - Story 2.1 bundles commit + regression test into one story
open_questions: []
updated_at: '2026-05-03T19:15:00Z'
---

# Stories — Bug Fixer Excessive Find Probing

## Story 1.1 — Fix SKILL.md preflight path and working directory

| Field | Value |
|---|---|
| ID | 1.1 |
| Epic | 1 |
| Points | 2 |
| Status | Ready |
| Dependencies | None |

**As a** Lens workbench user  
**I want** `/lens-bug-fixer` to invoke preflight exactly once from the correct working directory  
**So that** no exploratory `find` commands run and the execution trace is clean

### Acceptance Criteria

- [ ] Edit `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-bug-fixer/SKILL.md` (NOT `lens.core/`)
- [ ] "On Activation" step 1 specifies the exact CWD: `{control_repo}/TargetProjects/lens-dev/new-codebase/lens.core.src`
- [ ] "On Activation" step 1 specifies the relative script path: `_bmad/lens-work/lens-preflight/scripts/light-preflight.py`
- [ ] No `find` or `cat` probing commands appear before preflight in a fresh run
- [ ] Preflight invoked at most once per `--fix-all-new` run
- [ ] Preflight invoked from `{control_repo}/TargetProjects/lens-dev/new-codebase/lens.core.src`

### Implementation Notes

Replace the vague "Run light-preflight.py via the stub" instruction in SKILL.md "On
Activation" step 1 with the following explicit block:

```bash
cd "{control_repo}/TargetProjects/lens-dev/new-codebase/lens.core.src"
uv run --script _bmad/lens-work/lens-preflight/scripts/light-preflight.py
```

---

## Story 2.1 — Commit and test derive-feature-id stub truncation fix

| Field | Value |
|---|---|
| ID | 2.1 |
| Epic | 2 |
| Points | 1 |
| Status | Ready |
| Dependencies | None |

**As a** maintainer  
**I want** the stub truncation fix committed and guarded by a regression test  
**So that** the 64-char feature ID limit cannot silently regress

### Acceptance Criteria

- [ ] Commit `_bmad/lens-work/scripts/bug-fixer-ops.py` (with `max_stub_len = 64 - len(FEATURE_ID_PREFIX)`) to `lens.core.src` on branch `lens-dev-new-codebase-bugfix-bug-fixer-uses-excessive-find-probi`
- [ ] Test exists: pass a slug > 35 chars to `derive_feature_id_from_slugs`; assert `len(feature_id) <= 64`
- [ ] Test passes with current code
- [ ] (Optional) Extract `MAX_STUB_LEN = 64 - len(FEATURE_ID_PREFIX)` as a named constant

### Implementation Notes

The fix is already applied in the working tree. This story covers:
1. `git add` + `git commit` the fix to `lens.core.src` feature branch
2. Add a test file (or append to existing) in `_bmad/lens-work/scripts/tests/`
