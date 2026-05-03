---
feature: lens-dev-new-codebase-bugfix-1777828805636-46cf
title: "Sprint Plan — Preflight Guard in lens-bug-reporter"
doc_type: sprint-plan
status: approved
track: express
domain: lens-dev
service: new-codebase
key_decisions:
  - Single sprint: SKILL.md patch only
  - No script changes needed
  - Validate via manual test scenario review
open_questions: []
depends_on: []
blocks: []
updated_at: '2026-05-03T17:22:20Z'
---

# Sprint Plan — Bugbash Batch Fix: Preflight Guard in lens-bug-reporter

## Sprint 1 — Patch bmad-lens-bug-reporter SKILL.md

**Goal:** Close the behavioral gap that allowed lens-bug-reporter to make code changes when
preflight failed. Single-sprint fix: SKILL.md patch only.

### Stories

#### Story 1.1 — Strengthen preflight guard in Step 1

**File:** `lens.core/_bmad/lens-work/skills/bmad-lens-bug-reporter/SKILL.md`

Change Step 1 of "On Activation" to be an unambiguous hard stop:

```
Before: 1. Run `light-preflight.py` via the stub; exit on non-zero.

After:  1. Run `light-preflight.py` via the stub.
           - If preflight exits non-zero or the script cannot be found: STOP IMMEDIATELY.
             Do not collect input. Do not write code. Do not commit. Do not push.
             Surface the preflight failure verbatim and exit.
```

**Acceptance criteria:**
- AC1: The phrase "exit on non-zero" is replaced with the explicit multi-line stop guard
- AC2: The word "immediately" is present in the preflight stop instruction
- AC3: "Do not write code. Do not commit. Do not push." is explicit in the guard

#### Story 1.2 — Add no-code-write principle

Add to the **Principles** section of SKILL.md:

```
- **No-code-write** — /lens-bug-reporter never writes source code, never commits,
  and never pushes. Its only permitted write is `governance_repo/bugs/New/{slug}.md`
  via bug-reporter-ops.py.
```

**Acceptance criteria:**
- AC1: `No-code-write` principle is present in the Principles list
- AC2: The principle explicitly names "never writes source code, never commits, and never pushes"

#### Story 1.3 — Add scope-redirect guard after governance_repo resolution

In "On Activation", after step 3 (resolve governance_repo), insert:

```
   3a. Confirm the ONLY permitted write target is `{governance_repo}/bugs/New/`. If the
       user's request describes editing source files, prompts, or any non-bug content:
       stop and redirect — "This command records bugs only. Use the appropriate command
       for source edits."
```

**Acceptance criteria:**
- AC1: Step 3a (or equivalent insertion) is present after governance_repo resolution
- AC2: The instruction explicitly covers "editing source files, prompts, or any non-bug content"
- AC3: A redirect message is provided

### Dependencies

None. This sprint has no blockers or upstream dependencies.

### Risks

- Low: AI executors reading the SKILL.md may still misinterpret intent if they skip the
  non-negotiables. Mitigation: make the stop guard the first sentence of Step 1 with
  capitalized "STOP IMMEDIATELY".

### Definition of Done

1. All three story acceptance criteria are met in SKILL.md
2. No new scripts added
3. Existing `create-bug` happy path is unaffected
