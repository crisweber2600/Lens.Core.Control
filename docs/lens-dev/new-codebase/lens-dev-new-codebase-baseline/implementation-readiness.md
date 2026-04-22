---
feature: lens-dev-new-codebase-baseline
doc_type: implementation-readiness
status: draft
goal: "Assess whether the lens-work rewrite planning set is ready to hand off into /dev without reopening scope."
key_decisions:
  - "Use the approved 21-story backlog and seven-sprint grouping as the current execution plan."
  - "Treat empty target_repos as a current governance convention and capture /dev setup risk here instead of mutating feature metadata ad hoc."
  - "Carry forward old-codebase discovery docs as parity references for runtime behavior and dependency validation."
open_questions:
  - "Should target repo registration be formalized before the first /dev run?"
  - "Who owns the final release-readiness report for Story 5.5?"
depends_on:
  - epics.md
  - stories.md
  - architecture.md
  - finalizeplan-review.md
blocks:
  - sprint-status.yaml
  - stories/
updated_at: 2026-04-22T00:00:00Z
---

# Implementation Readiness Assessment: lens-dev-new-codebase-baseline

## Readiness Assessment

### Overall Status: Conditionally Ready

| Area | Status | Notes |
|------|--------|-------|
| PRD completeness | Green | PRD freezes the retained 17-command surface, 40 FRs, 17 NFRs, and rewrite invariants. |
| Architecture reviewed | Green | Architecture and techplan review define rewrite sequencing, parity gates, and prerequisite constraints. |
| Epics well-defined | Green | Five epics and 21 stories exist with explicit acceptance criteria, dependencies, and release-gate logic. |
| Stories have acceptance criteria | Green | stories.md and story files stage independently verifiable acceptance criteria for every story. |
| Dependencies resolved | Yellow | Epic 3 to Epic 4 and Story 5.5 release-gate dependencies are explicit, but they remain execution-time constraints. |
| Team capacity confirmed | Yellow | A seven-sprint plan and 120-point estimate exist, but owner assignment and actual team velocity are still unconfirmed. |
| Environment ready | Yellow | The source target repo is known in the workspace, but target_repos remains empty by current governance convention and /dev branch setup is still a handoff task. |

## Risk Assessment

### Technical Risks

| Risk | Likelihood | Impact | Mitigation | Owner |
|------|-----------|--------|------------|-------|
| Epic 3 constitution fix slips, blocking all Epic 4 conductor rewrites | M | H | Keep Story 3.1 isolated, regression-backed, and complete before any Epic 4 branch work begins | Tech lead |
| Shared utility extraction drifts from legacy behavior | M | H | Preserve parity anchors for validation, batch, publish hook, and finalizeplan sequencing | Maintainer |
| /dev starts without an explicit target repo handoff | M | M | Confirm source repo path and branch mode at /dev kickoff; treat as an operational checkpoint, not new scope | Dev lead |

### Schedule Risks

| Risk | Likelihood | Impact | Mitigation | Owner |
|------|-----------|--------|------------|-------|
| Identity/navigation rewrites uncover hidden dependency drift from the old codebase | M | M | Use the old-codebase dependency map and parity tests to validate each command family before advancing | Maintainer |
| Story 5.5 regression gate expands late in the program | M | H | Keep required regression anchors visible in sprint planning and reserve time for release-readiness reporting | Release lead |

### Resource Risks

| Risk | Likelihood | Impact | Mitigation | Owner |
|------|-----------|--------|------------|-------|
| Seven-sprint plan exceeds available maintainer bandwidth | M | M | Rebalance by sprint after Story 3.1 completes; compress lower-complexity command stories if needed | Product/engineering lead |
| Adapter/help surface updates require multi-surface review coordination | M | M | Land Story 1.1 as a single audited slice before downstream work branches | Maintainer |

## Pre-Implementation Checklist

- [x] All epics have clear acceptance criteria.
- [x] All stories are estimated.
- [x] Architecture document is approved for planning handoff.
- [ ] Target repository branches are prepared.
- [ ] CI/CD pipeline and focused regression command ownership are confirmed.
- [x] Test strategy is defined through parity anchors and release-gate requirements.
- [ ] Monitoring and release reporting ownership for Story 5.5 is confirmed.

## Outstanding Items

| Item | Owner | Due | Status |
|------|-------|-----|--------|
| Confirm whether /dev should record `TargetProjects/lens-dev/new-codebase/lens.core.src` into feature metadata or resolve it interactively | Tech lead | Before first /dev run | Open |
| Confirm sprint owners and realistic velocity against the seven-sprint proposal | Product/engineering lead | Before Sprint 1 starts | Open |
| Confirm the exact execution owner for the Story 5.5 release-readiness report | Release lead | Before Epic 5 starts | Open |

## Go / No-Go Decision

**Decision:** Go with conditions

**Date:** 2026-04-22

**Rationale:** The planning set is internally coherent, the backlog is fully decomposed, and the critical dependency chain is explicit. Remaining gaps are operational handoff items around team capacity, /dev setup, and release ownership rather than missing scope or missing requirements.
