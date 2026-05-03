---
feature: lens-dev-new-codebase-bugfix-1777828805636-46cf
title: "Tech Plan — Preflight Guard in lens-bug-reporter"
doc_type: tech-plan
status: approved
track: express
domain: lens-dev
service: new-codebase
key_decisions:
  - Fix is entirely in bmad-lens-bug-reporter/SKILL.md — no script changes
  - Add explicit no-code-write / no-commit / no-push guard language
  - Clarify that on preflight non-zero, reporter records intake artifact then stops
open_questions: []
depends_on: []
blocks: []
updated_at: '2026-05-03T17:22:20Z'
---

# Tech Plan — Bugbash Batch Fix: Preflight Guard in lens-bug-reporter

## Architecture Analysis

The `bmad-lens-bug-reporter` skill is a thin conductor. All file I/O is delegated to
`bug-reporter-ops.py`. The skill only:

1. Runs preflight
2. Collects title/description/chat-log from the developer
3. Calls `bug-reporter-ops.py create-bug`
4. Reports the outcome

The root-cause of the bug is two behavioral gaps in SKILL.md:

1. **Step 1 says "exit on non-zero" but the executed skill continued** — the "exit on
   non-zero" instruction was not followed; the skill proceeded to make code edits. The
   language needs to be unambiguous: **no file writes, no commits, no pushes** after a
   non-zero preflight exit.

2. **No explicit prohibition on code writes** — SKILL.md lacks a hard constraint that the
   bug reporter must never write source code, never commit, and never push. Without this
   guard, an AI executor may misinterpret a bug description like "update all prompts" as a
   repair task.

## Fix Design

### Change 1 — Strengthen "exit on non-zero" language in Step 1

**File:** `lens.core/_bmad/lens-work/skills/bmad-lens-bug-reporter/SKILL.md`

**Current Step 1:**
```
1. Run `light-preflight.py` via the stub; exit on non-zero.
```

**Proposed Step 1:**
```
1. Run `light-preflight.py` via the stub.
   - If preflight exits non-zero or the script cannot be found: STOP IMMEDIATELY.
     Do not collect input. Do not write code. Do not commit. Do not push.
     Surface the preflight failure verbatim and exit.
```

### Change 2 — Add hard no-code-write constraint to Principles

**Add to Principles section:**
```
- **No-code-write** — /lens-bug-reporter never writes source code, never commits,
  and never pushes. Its only permitted write is `governance_repo/bugs/New/{slug}.md`
  via bug-reporter-ops.py.
```

### Change 3 — Add scope guard clarification to On Activation

After step 3 (resolve governance_repo), add:

```
   After resolving governance_repo, confirm the ONLY permitted write target is
   `{governance_repo}/bugs/New/`. Any request that would require writing to a
   source repo, editing prompts, or modifying non-bug files is outside this
   skill's scope. Redirect to the appropriate command.
```

## Artifact Contract

| Artifact | Change |
|----------|--------|
| `bmad-lens-bug-reporter/SKILL.md` | Add preflight hard-stop, no-code-write principle, scope guard redirect |

## Testing Strategy

| Scenario | Expected Result |
|----------|----------------|
| Preflight exits 0 | Happy path: collect inputs, create bug, report slug |
| Preflight exits non-zero | Stop immediately; no bug written, no code touched |
| Preflight script missing | Same as non-zero: stop immediately |
| User asks reporter to edit code | Redirect: "This command records bugs. Use the appropriate edit command." |

## Rollout

No runtime dependencies to update. The fix is a SKILL.md-only change. Effective on next
skill invocation after merge.
