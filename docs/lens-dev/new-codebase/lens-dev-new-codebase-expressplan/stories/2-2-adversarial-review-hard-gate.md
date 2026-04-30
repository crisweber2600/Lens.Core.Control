---
feature: lens-dev-new-codebase-expressplan
doc_type: story-file
story_id: 2.2
status: ready-for-dev
goal: "Make adversarial review a true hard gate for the express path"
depends_on:
  - 2.1
updated_at: 2026-04-27T22:50:00Z
---

# Story 2.2 - Adversarial-Review Hard Gate

## User Story

As a release owner, I want a failed expressplan review to stop progression so the fast path does not become a planning-quality escape hatch.

## Acceptance Criteria

- Expressplan invokes `bmad-lens-adversarial-review --phase expressplan --source phase-complete` after QuickPlan artifacts exist.
- A `fail` verdict blocks FinalizePlan handoff.
- A pass or pass-with-warnings verdict writes the chosen review artifact filename consistently.
- The review result is surfaced in a concise, actionable summary.

## Implementation Notes

- Treat review artifact naming as a tested contract.
- Keep the party-mode challenge short and phase-specific.
- Avoid any code path that advances the phase before the verdict exists.

## Validation Targets

- Review fail hard-stop test
- Review pass artifact creation test
- Review filename contract check