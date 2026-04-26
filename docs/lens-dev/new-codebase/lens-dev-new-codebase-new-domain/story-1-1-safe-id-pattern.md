---
feature_id: lens-dev-new-codebase-new-domain
story_key: "1-1-safe-id-pattern"
epic: 1
story: 1
title: "Canonicalize SAFE_ID_PATTERN constant"
type: implementation
estimate: S
priority: P0
status: not-started
assigned: crisweber2600
sprint: 1
depends_on: []
blocks:
  - "1-2-core-flow"
  - "2-3-skill-ux"
created_at: 2026-04-26T00:00:00Z
updated_at: 2026-04-26T00:00:00Z
---

# Story 1.1 — Canonicalize SAFE_ID_PATTERN constant

## Why This Story Exists

The finalizeplan review raised a concern (finding F3/R1, Medium severity) that `SAFE_ID_PATTERN` might not be consistently documented. In the current planning bundle, the business plan and related references align on `^[a-z0-9][a-z0-9._-]{0,63}$`; however, before any slug validation code is written, the authoritative value must still be read directly from the old-codebase source and embedded with a citation so the new codebase and shared contracts are anchored to the source of truth.

This is the first story in Sprint 1. It is a prerequisite for Story 1.2 and all downstream validator implementations in `new-service` and `new-feature`.

## What To Build

A single module-level constant `SAFE_ID_PATTERN` in the new `init-feature-ops.py`, with an accompanying `validate_safe_id(domain: str) -> None` function.

Also: a `SHARED_CONTRACTS.md` stub in `docs/lens-dev/new-codebase/` that records the resolved pattern for cross-feature use.

---

## Pre-Implementation Action (Required)

Before writing any code, open the old-codebase source:

```
TargetProjects/lens-dev/old-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py
```

Find the `SAFE_ID_PATTERN` constant. Read its value verbatim. Record it below before proceeding.

**Resolved value (fill in during implementation):** `SAFE_ID_PATTERN = r"..."  # read from old-codebase line N`

---

## File Locations

| File | Action |
|---|---|
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py` | Add constant + function |
| `docs/lens-dev/new-codebase/SHARED_CONTRACTS.md` | Create with resolved pattern |

---

## Acceptance Criteria

- [ ] `SAFE_ID_PATTERN` is declared as a module-level constant in new `init-feature-ops.py`
- [ ] Pattern value matches old-codebase source character-for-character
- [ ] Constant includes inline comment: `# source: old-codebase init-feature-ops.py SAFE_ID_PATTERN`
- [ ] `validate_safe_id(domain: str) -> None` function raises `ValueError` with descriptive message for invalid slugs
- [ ] `validate_safe_id(domain)` returns `None` (no-op) for valid slugs
- [ ] `test_validate_safe_id_valid` passes with: `"lens-dev"`, `"platform"`, `"my-domain-1"`, `"a"`, `"x" * 64` (max length)
- [ ] `test_validate_safe_id_invalid` passes with: `"UPPER"`, `"-leading"`, `"trailing-"`, `"has space"`, `"a/b"`, `"x" * 65` (over max)
- [ ] `docs/lens-dev/new-codebase/SHARED_CONTRACTS.md` created with resolved pattern value

## Completion Note

When this story is marked complete, update `sprint-status.yaml` to set `"1-1-safe-id-pattern": status: complete`.

Then notify `new-service` and `new-feature` feature owners that the shared pattern is available in `SHARED_CONTRACTS.md`.
