---
feature: lens-dev-new-codebase-bugfix-1777828805636-46cf
title: "Bugbash Batch Fix - Preflight Guard in lens-bug-reporter"
doc_type: business-plan
status: approved
track: express
domain: lens-dev
service: new-codebase
key_decisions:
  - lens-bug-reporter must gate on preflight before any code action
  - preflight failure must write a bug intake artifact, not attempt a fix
open_questions: []
depends_on: []
blocks: []
updated_at: '2026-05-03T17:22:20Z'
---

# Business Plan — Bugbash Batch Fix: Preflight Guard in lens-bug-reporter

## Problem

When `/lens-bug-reporter` is invoked and the preflight gate fails, the skill should halt
immediately and write a structured bug intake artifact to `governance_repo/bugs/New/`.
Instead, the skill proceeded to make corrective code changes, commit them, and push to the
branch — effectively acting as a code-fixer while bypassing all governance storage.

This breaks the separation between bug reporting and bug fixing. The bug is never registered
in governance, the fix is untracked, and the intent of the preflight gate (to block unsafe
operations) is circumvented.

## Affected Users

- Developers invoking `/lens-bug-reporter` with a failing preflight
- Governance operators who expect `bugs/New/` to be the source of truth for all bugs
- `/lens-bug-fixer` which scans `bugs/New/` for actionable items

## Business Goals

1. Guarantee that a preflight failure in `/lens-bug-reporter` always writes a bug intake
   artifact to `governance_repo/bugs/New/` before stopping.
2. Guarantee that `/lens-bug-reporter` never writes code, commits, or pushes when preflight
   fails or when the reporter skill file is missing.
3. Ensure all reported bugs are traceable in governance from the moment they are reported.

## Scope

**In scope:**
- Enforce the preflight gate as a hard stop in `bmad-lens-bug-reporter` SKILL.md
- Add explicit "stop-and-store" logic when preflight exits non-zero
- Validate behavior when the bug reporter skill file itself is missing

**Out of scope:**
- Changes to `/lens-bug-fixer` discovery or fix logic
- Changes to the preflight script itself

## Success Criteria

1. When preflight exits non-zero, `/lens-bug-reporter` writes a bug intake artifact to
   `governance_repo/bugs/New/` and halts; it does not write code.
2. When the bug reporter skill file is missing, the reporter halts with an error message.
3. Existing `/lens-bug-reporter` happy-path tests continue to pass.

## Risks

- Low: The fix is a behavioral constraint in SKILL.md — no new scripts needed.
- Low: The only state touched on failure is a new file in `bugs/New/`.
